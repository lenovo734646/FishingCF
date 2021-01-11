FishRoomController = class(LuaBase, {})

function FishRoomController:__init()
    UpdateLuaBehaviour.Bind(self.gameObject, self)
    AddGameToHallListener(functional.bind1(self.OnGameToHall, self))
    
    self.Module = ModuleManager.instance:Get("FishModule")
    self.bg = self:get("FishBackground", "Image")
    local fishPoolNode = self:get("FishPool").gameObject
    self.FishSpawn = FishSpawn(LuaClass(fishPoolNode))
    local bulletPoolNode = self:get("BulletPool").gameObject
    self.BulletSpawn = BulletSpawn(LuaClass(bulletPoolNode))
end

function FishRoomController:OnGameToHall()
    Destroy(self.gameObject)
end

function FishRoomController:Update()
    if self.FishSpawn ~= nil then
        self.Module:OnUpdate()
        self.FishSpawn:OnUpdate()
    end
end

function FishRoomController:ProcessEnterFishingRoomAck(ack, configId)
    TimeHelper.SetServerTimestamp(ack.time_stamp)
    self.Module:InitData()
    self.Module.RoomID = configId
    
    self.penetrateHitCacheIndex = 0
    self.penetrateHitBulletDic = {}
    
    NetController.instance:SendGetReadyReq()
end

function FishRoomController:ProcessExitGameAck(ack, action)
    action()
end

function FishRoomController:ProcessGetReadyAck(ack)
    local bulletRate = TFishGlobal[1].BulletRate
    LocalDefines.NormalBulletShootRate = bulletRate[1]
    LocalDefines.PenetrateBulletShootRate = bulletRate[2]
    local bulletSpeed = TFishGlobal[1].BulletSpeed
    LocalDefines.NormalBulletSpeedRate = bulletSpeed[1]
    LocalDefines.PenetrateBulletSpeedRate = bulletSpeed[2]
    
    FishUIController.instance:OnEnterFishRoom()
    
    self.FishSpawn:Active()
    self.BulletSpawn:Active()
    
    --处理玩家
    for k, v in pairs(ack.players) do
        if v.user_id == GameController.Instance.Player.UserID then
            self.Module:AddPlayer(RoomPlayer(v))
        end
    end
    for k, v in pairs(ack.players) do
        if v.user_id ~= GameController.Instance.Player.UserID then
            self.Module:AddPlayer(RoomPlayer(v))
        end
    end
    --处理鱼
    for k, v in pairs(ack.fishes) do
        self.Module:AddFish(v)
    end
    
    --背景图临时修复 超凡没有4号背景
    if ack.background_id == 4 then
        ack.background_id = 1
    end
    self.bg.Image.sprite = Loader:LoadAsset(Path_Res .. "FishBackground/FishBackground_1_" .. ack.background_id .. ".prefab", typeof(GameObject)):GetComponent("Image").sprite
    GameController.Instance.Player:SetLaserEnergy(ack.energy)
    LocalDefines.IsInFishingGame = true
    self.currentVirtualBulletIndex = 0
end

function FishRoomController:ProcessFishAppearNtf(ntf)
    for k, v in pairs(ntf.fishes) do
        self.Module:AddFish(v)
    end
end

function FishRoomController:ProcessFishTimeRateChangeNtf(ntf)
    local fish = self.Module:GetFishById(ntf.fish_id)
    if fish then
        fish:AddTimeRate(ntf.rate.time, ntf.rate.duration, ntf.rate.rate, ntf.rate.reason)
    else
        self.Module:AddTimeRateToNotBornFish(ntf.fish_id, ntf.rate.time, ntf.rate.duration, ntf.rate.rate, ntf.rate.reason)
    end
end

function FishRoomController:ProcessFishRemoveNtf(ntf)
    local player = self.Module:GetPlayerBySeatId(ntf.seat_id)
    if player then
        self.Module:RemoveFishNtf(ntf, player)
    end
    if player.IsPlayerSelf() then
        GameController.Instance.Player.DeltaCurrency(ntf.delta, 1)
    end
end

function FishRoomController:ProcessPlayerJoinNtf(ntf)
    local player = RoomPlayer(ntf.player)
    self.Module:AddPlayer(player)
end

function FishRoomController:ProcessPlayerLeaveNtf(ntf)
    self.Module:RemovePlayerBySeat(ntf.seat_id)
end

function FishRoomController:ProcessShootAck(ack, angle, bullet)
    if ack.errcode == 0 then
        local player = self.Module.PlayerSelf
        if player then
            GameController.Instance.Player:DeltaCurrency(ack.currency_delta)
            GameController.Instance.Player:DeltaLaserEnergy(ack.energy_delta)
            self.Module:AcceptNewBulletInfo(bullet, ack.bullet_id, ack.lock_fish)
        end
    else
        NetController.instance:CreateErrorMsg("CLFRShootAck", ack.errcode)
        self.Module:RemoveBullet(bullet.BulletId, false)
        self.Module:RemoveBulletFromWaitingQueue(bullet)
    end
end

function FishRoomController:ProcessShootNtf(ntf)
    local player = self.Module:GetPlayerBySeatId(ntf.seat_id)
    if player then
        -- MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_CLONE_FIRE_OTHER_CHANGE, {
        --     Clone = false,
        --     Player = player
        -- })
        player:DeltaCurrency(ntf.currency_delta)
        if player:IsRobot() and ntf.lock_fish > 0 then
            local fish = self.FishSpawn:FindFishById(ntf.lock_fish)
            if fish == nil or not fish:CheckOutOfScreen() or not fish:IsValid() then
                ntf.lock_fish = 0
            end
        end
        player:SetRotationAngle({ntf.angle}, {ntf.lock_fish})
        self.Module:CreateBullet(player.ServerSeat, ntf.bullet_id, ntf.angle, ntf.lock_fish, 1, false, ntf.multiple)
    end
end

function FishRoomController:ProcessAcrossShootAck(ack, angle, bullet)
    if ack.errcode == 0 then
        local player = self.Module.PlayerSelf
        if player then
            self.Module:AcceptNewBulletInfo(bullet, ack.bullet_id, 0)
        end
    else
        NetController.instance:CreateErrorMsg("CLFRAcrossShootAck", ack.errcode)
        self.Module:RemoveBullet(bullet.BulletId, false)
        self.Module:RemoveBulletFromWaitingQueue(bullet)
    end
end

function FishRoomController:ProcessAcrossShootNtf(ntf)
    local player = self.Module:GetPlayerBySeatId(ntf.seat_id)
    if player then
        MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_CLONE_FIRE_OTHER_CHANGE, {
            Clone = false,
            Player = player
        })
        player:SetRotationAngle({ntf.angle}, {ntf.lock_fish})
        self.Module:CreateBullet(player.ServerSeat, ntf.bullet_id, ntf.angle, 0, 1, true, ntf.multiple)
    end
end

function FishRoomController:ProcessMultiShootAck(ack, bullets)
    if ack.errcode == 0 then
        local player = self.Module.PlayerSelf
        if player then
            GameController.Instance.Player:DeltaCurrency(ack.currency_delta)
            GameController.Instance.Player:DeltaLaserEnergy(ack.energy_delta)
            for i = 1, ack.bullet_len do
                local info = ack.bullet_array[i]
                self.Module:AcceptNewBulletInfo(bullets[i], info.bullet_id, info.lock_fish)
            end
        end
    else
        NetController.instance:CreateErrorMsg("CLFRMultiShootAck", ack.errcode)
        for k, v in pairs(bullets) do
            self.Module:RemoveBullet(v.BulletId, false)
            self.Module:RemoveBulletFromWaitingQueue(v)
        end
    end
end

function FishRoomController:ProcessMultiShootNtf(ntf)
    local player = self.Module:GetPlayerBySeatId(ntf.seat_id)
    if player then
        MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_CLONE_FIRE_OTHER_CHANGE, {
            Clone = true,
            Player = player
        })
        player:DeltaCurrency(ntf.currency_delta)
        local bullets = ntf.bullet_array
        local angles = {}
        local locks = {}
        for i = 1, #bullets do
            if player:IsRobot() then
                local fish = self.FishSpawn:FindFishById(bullets[i].lock_fish)
                if fish == nil or not fish:CheckOutOfScreen() or not fish:IsValid() then
                    bullets[i].lock_fish = 0
                end
            end
            angles[i] = bullets[i].angle
            locks[i] = bullets[i].lock_fish
            self.Module:CreateBullet(player.ServerSeat, bullets[i].bullet_id, bullets[i].angle, bullets[i].lock_fish, i + 1, false, ntf.multiple)
        end
        player:SetRotationAngle(angles, locks)
    end
end

function FishRoomController:ProcessAcrossMultiShootAck(ack, bullets)
    if ack.errcode == 0 then
        local player = self.Module.PlayerSelf
        if player then
            for i = 1, ack.bullet_len do
                local info = ack.bullet_array[i]
                self.Module:AcceptNewBulletInfo(bullets[i], info.bullet_id, 0)
            end
        end
    else
        NetController.instance:CreateErrorMsg("CLFRAcrossMultiShootAck", ack.errcode)
        for k, v in pairs(bullets) do
            self.Module:RemoveBullet(v.BulletId, false)
            self.Module:RemoveBulletFromWaitingQueue(v)
        end
    end
end

function FishRoomController:ProcessAcrossMultiShootNtf(ntf)
    local player = self.Module:GetPlayerBySeatId(ntf.seat_id)
    if player then
        MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_CLONE_FIRE_OTHER_CHANGE, {
            Clone = true,
            Player = player
        })
        local bullets = ntf.bullet_array
        local angles = {}
        local locks = {}
        for i = 1, #bullets do
            angles[i] = bullets[i].angle
            locks[i] = bullets[i].lock_fish
            self.Module:CreateBullet(player.ServerSeat, bullets[i].bullet_id, bullets[i].angle, 0, i + 1, true, ntf.multiple)
        end
        player:SetRotationAngle(angles, locks)
    end
end

function FishRoomController:ProcessHitLogic(bulletId, fishId)
    local bullet = self.Module:GetBulletById(bulletId)
    local player = self.Module:GetPlayerBySeatId(bullet.SeatId)
    local fish = self.Module:GetFishById(fishId)
    if bullet and fish then
        local info = bullet.SnapshotInfo
        local related = {}
        local tFish = table.find(TFish, function(b)
            return b.Id == fish.ConfigID
        end)
        if tFish.FishTypeServer == 4 then
            local lst = self.FishSpawn:FindFishByRadius(fishId, 330)
            related = lst
        end
        if bullet.UserId == GameController.Instance.Player.UserID then
            if bulletId < 0 then
                table.insert(bullet.WaitingHitFishList, {
                    id = fishId,
                    related = related,
                })
                if not bullet.IsPenetrate then
                    self.Module:AddBulletToWaitingList(bullet)
                end
            else
                if bullet.IsPenetrate then
                    local result = self:checkPenetrateHit(bullet.GunValue)
                    if result ~= -1 then
                        NetController.instance:SendAcrossHitReq(bulletId, fishId, related, result)
                    end
                else
                    NetController.instance:SendHitReq(bulletId, fishId, related)
                end
            end
        elseif info.IsBelongToRobot and self.Module:IsPlayerRobotProxy() then
            NetController.instance:SendRobotHitRpt(bulletId, fishId)
        end
        if not bullet.IsPenetrate then
            self.Module:RemoveBullet(bulletId, true)
        else
            MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_PENETRATE_CREATE_WEB, {
                Bullet = bullet,
                IsShowNet = true,
            })
        end
    end
end

--处理穿透扣金币由于面板延迟到账导致的显示错误
function FishRoomController:checkPenetrateHit(gunValue)
    local result = -1
    local total = 0
    for k, v in pairs(self.penetrateHitBulletDic) do
        total = total + v
    end
    local player = self.Module.PlayerSelf
    if total + gunValue <= player.Currency then
        self.penetrateHitCacheIndex = self.penetrateHitCacheIndex + 1
        self.penetrateHitBulletDic[self.penetrateHitCacheIndex] = gunValue
        result = self.penetrateHitCacheIndex
    end
    return result
end

function FishRoomController:removeHitBullet(cacheIndex)
    self.penetrateHitBulletDic[cacheIndex] = nil
end

function FishRoomController:ProcessHitAck(ack, bulletId, fishId)
    if ack.errcode == 0 then
        local player = self.Module.PlayerSelf
        if player then
            GameController.Instance.Player:DeltaCurrency(ack.currency_delta, 1)
            GameController.Instance.Player:DeltaLaserEnergy(ack.energy_delta)
            local fish = self.Module:GetFishById(fishId)
            if ack.is_boom == 1 and fish then
                print("ProcessHitAck  reason : " .. ack.related_remove_reason)
                if ack.related_remove_reason ~= 0 then
                    local functionalFish = {
                        FuncFish = fish,
                        RelatedIds = ack.related_fish_array,
                        Reason = ack.related_remove_reason,
                        Player = player,
                        DeltaGold = ack.currency_delta,
                    }
                    self.Module:RemoveFunctionalFish(functionalFish)
                else
                    self.Module:RemoveFish(1,fish, ack.currency_delta, player)
                end
            end
        end
    else
        NetController.instance:CreateErrorMsg("CLFRHitAck", ack.errcode)
    end
end

function FishRoomController:ProcessHitNtf(ntf)
    local player = self.Module:GetPlayerBySeatId(ntf.seat_id)
    if player then
        local fish = self.Module:GetFishById(ntf.fish_id)
        if ntf.is_boom == 1 and fish then
            if ntf.related_remove_reason ~= 0 then
                local functionalFish = {
                    FuncFish = fish,
                    RelatedIds = ntf.related_fish_array,
                    Reason = ntf.related_remove_reason,
                    Player = player,
                    DeltaGold = ntf.currency_delta,
                }
                self.Module:RemoveFunctionalFish(functionalFish)
            else
                self.Module:RemoveFish(1,fish, ntf.currency_delta, player)
            end
        end
    end
end

function FishRoomController:ProcessAcrossHitAck(ack, bulletId, fishId, hitCache)
    if ack.errcode == 0 then
        local player = self.Module.PlayerSelf
        if player then
            GameController.Instance.Player:DeltaCurrency(ack.currency_delta)
            GameController.Instance.Player:DeltaCurrency(ack.win_currency, 1)
            GameController.Instance.Player:DeltaLaserEnergy(ack.energy_delta)
            local fish = self.Module:GetFishById(fishId)
            if ack.is_boom == 1 and fish then
                if ack.related_remove_reason ~= 0 then
                    local functionalFish = {
                        FuncFish = fish,
                        RelatedIds = ack.related_fish_array,
                        Reason = ack.related_remove_reason,
                        Player = player,
                        DeltaGold = ack.win_currency,
                    }
                    self.Module:RemoveFunctionalFish(functionalFish)
                else
                    self.Module:RemoveFish(1,fish, ack.win_currency, player)
                end
            end
        end
        self:removeHitBullet(hitCache)
    else
        NetController.instance:CreateErrorMsg("CLFRAcrossHitAck", ack.errcode)
    end
end

function FishRoomController:ProcessAcrossHitNtf(ntf)
    local player = self.Module:GetPlayerBySeatId(ntf.seat_id)
    if player then
        player:DeltaCurrency(ntf.currency_delta)
        local fish = self.Module:GetFishById(ntf.fish_id)
        if ntf.is_boom == 1 and fish then
            if ntf.related_remove_reason ~= 0 then
                local functionalFish = {
                    FuncFish = fish,
                    RelatedIds = ntf.related_fish_array,
                    Reason = ntf.related_remove_reason,
                    Player = player,
                    DeltaGold = ntf.win_currency,
                }
                self.Module:RemoveFunctionalFish(functionalFish)
            else
                self.Module:RemoveFish(1,fish, ntf.win_currency, player)
            end
        end
    end
end

function FishRoomController:ProcessGunValueChangeAck(ack)
    if ack.errcode == 0 then
        local player = self.Module.PlayerSelf
        if player then
            player:SetGunValue(ack.gun_value)
        end
    else
        NetController.instance:CreateErrorMsg("CLFRGunValueChangeAck", ack.errcode)
    end
end

function FishRoomController:ProcessGunValueChangeNtf(ntf)
    local player = self.Module:GetPlayerBySeatId(ntf.seat_id)
    if player then
        player:SetGunValue(ntf.gun_value)
    end
end

function FishRoomController:ProcessEnergyStoreAck(ack)
    if ack.errcode == 0 then
        GameController.Instance.Player:DeltaLaserEnergy(ack.energy_delta)
        MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_TRY_USE_LASER_SUCCESS)
    else
        NetController.instance:CreateErrorMsg("CLFREnergyStoreAck", ack.errcode)
    end
end

function FishRoomController:ProcessEnergyShootAck(ack)
    if ack.errcode == 0 then
        local player = self.Module.PlayerSelf
        if player then
            local totalCurrencyDelta = 0
            if ack.boom_length > 0 then
                for i = 1, ack.boom_length do
                    local boomInfo = ack.boom_array[i]
                    local fish = self.Module:GetFishById(boomInfo.fish_id)
                    if fish then
                        if boomInfo.related_remove_reason ~= 0 then
                            local functionalFish = {
                                FuncFish = fish,
                                RelatedIds = ack.related_fish_array,
                                Reason = ack.related_remove_reason,
                                Player = player,
                                DeltaGold = ack.currency_delta,
                            }
                            self.Module:RemoveFunctionalFish(functionalFish)
                        else
                            self.Module:RemoveFish(1,fish, boomInfo.currency_delta, player)
                        end
                    end
                    totalCurrencyDelta = totalCurrencyDelta + boomInfo.currency_delta
                end
            end
            GameController.Instance.Player:DeltaCurrency(totalCurrencyDelta, 1)
        end
    else
        NetController.instance:CreateErrorMsg("CLFREnergyShootAck", ack.errcode)
    end
end

function FishRoomController:ProcessEnergyShootNtf(ntf)
    local player = self.Module:GetPlayerBySeatId(ntf.seat_id)
    if player then
        player:SetRotationAngle({ntf.angle}, {0})
        MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_OTHER_USE_LAYER, {
            Player = player,
            Angle = ntf.angle
        })
        
        if ntf.boom_length > 0 then
            for i = 1, ntf.boom_length do
                local boomInfo = ntf.boom_array[i]
                local fish = self.Module:GetFishById(boomInfo.fish_id)
                if fish then
                    if boomInfo.related_remove_reason ~= 0 then
                        local functionalFish = {
                            FuncFish = fish,
                            RelatedIds = ntf.related_fish_array,
                            Reason = ntf.related_remove_reason,
                            Player = player,
                            DeltaGold = ntf.currency_delta,
                        }
                        self.Module:RemoveFunctionalFish(functionalFish)
                    else
                        self.Module:RemoveFish(1,fish, boomInfo.currency_delta, player)
                    end
                end
            end
        end
    end
end

function FishRoomController:ProcessShowWheel1Ntf(ntf)

end

function FishRoomController:ProcessShowWheel2Ntf(ntf)

end

function FishRoomController:ProcessSeatResourceChangedNtf(ntf)
    local player = self.Module:GetPlayerBySeatId(ntf.seat_id)
    if player then
        player:DeltaCurrency(ntf.currency_delta)
    end
end

function FishRoomController:ProcessFishTideStartNtf(ntf)
    self.Module:FishTideStart(ntf)
end

function FishRoomController:ProcessFishTideForTest(ack)
    if ack.errcode == 0 then
        self.Module:FishTideStart(ack)
    end
end


--创建虚拟子弹
function FishRoomController:CreateVirtualBulletForPlayerSelf(angle, lockFishId, isPenetrate, multiple, bulletIndex)
    bulletIndex = bulletIndex ~= nil and bulletIndex or 1
    local r
    local player = self.Module.PlayerSelf
    if player then
        self.currentVirtualBulletIndex = self.currentVirtualBulletIndex - 1
        r = self.Module:CreateBullet(player.ServerSeat, self.currentVirtualBulletIndex, angle, lockFishId, bulletIndex, isPenetrate, multiple)
    end
    return r
end

function FishRoomController:LeaveRoom()
    LocalDefines.IsInFishingGame = false
    NetController.instance:SendExitGameReq(function()
        NetController.instance.SendExitSiteReq(LocalDefines.SiteId)
    end)
    self.Module.active = false
    MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_LEAVE_FISH_ROOM)
end
