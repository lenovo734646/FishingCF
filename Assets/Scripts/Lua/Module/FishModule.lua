FishModule = class(nil, {})

function FishModule:__init()
    self.active = false
end

function FishModule:InitData()
    self.RoomFishMap = {}
    self.AppearFishList = {}
    self.IsAppearFishListDirty = false
    self.RoomPlayerMap = {}
    self.PlayerSelf = nil
    self.RoomBulletMap = {}
    self.RoomBulletWaitingList = {}
    self.PlayerBulletActiveCount = 0
    self.PlayerBulletLimit = TFishGlobal[1].BulletLimit
    self.funcDelayRemoveList = {}
    
    self.active = true
end

function FishModule:OnUpdate()
    if not self.active then return end
    
    local timestamp = TimeHelper.GetServerTimestamp()
    
    if #self.AppearFishList > 0 then
        if self.IsAppearFishListDirty then
            table.sort(self.AppearFishList, function(a, b)
                local cmp = false
                if a:GetRealTimeForBorn() < b:GetRealTimeForBorn() then cmp = true end
                return cmp
            end)
            self.IsAppearFishListDirty = false
        end
        while (#self.AppearFishList > 0)
        do
            local fish = self.AppearFishList[1]
            if timestamp < fish:GetRealTimeForBorn() then break end
            table.remove(self.AppearFishList, 1)
            fish:UpdateSnapshotInfo(timestamp)
            
            self.RoomFishMap[fish.FishID] = fish
            MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_FISH_CREATE, {Fish = fish})
        end
    end
    
    local lst = {}
    for k, v in pairs(self.RoomFishMap) do
        v:UpdateSnapshotInfo(timestamp)
        if v:IsExpired(timestamp) then
            table.insert(lst, v)
        end
    end
    for k, v in pairs(lst) do
        self:RemoveFish(-1,v, 0)
    end
    
    for k, v in pairs(self.RoomBulletMap) do
        if v.IsPenetrate then
            local bulletScript = FishRoomController.instance.BulletSpawn:FindBulletById(v.BulletId)
            if not bulletScript:CheckOutOfScreen() then
                self:RemoveBullet(v.BulletId)
                if v.BulletId < 0 then
                    self:AddBulletToWaitingList(v)
                end
            end
        end
    end
    for k, v in pairs(self.RoomBulletMap) do
        v:UpdateSnapshotInfo(timestamp)
    end
    
    for id, ff in pairs(self.funcDelayRemoveList) do
        ff.DelayTime = ff.DelayTime - Time.deltaTime
        if ff.DelayTime <= 0 then
            if ff.FuncFish then
                local delta = ff.DeltaGold
                if ff.Reason == 5 then
                    self:RemoveFish(1, ff.FuncFish, delta, ff.Player, ff.ItemList, 1)
                else
                    self:RemoveFish(1, ff.FuncFish, delta, ff.Player, ff.ItemList)
                end
            end
            for k, f in pairs(ff.RelatedFish) do
                self:RemoveFish(1, f, 0)
                MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_FISH_CREATE_GOLD, {
                    Fish = f,
                    Player = ff.Player
                })
            end
            self.funcDelayRemoveList[id] = nil
        end
    end
end

--fish operation
function FishModule:GetFishById(fishId)
    return self.RoomFishMap[fishId]
end

function FishModule:AddFish(info)
    local tFish = TFish[info.config_id]
    if not tFish then return end
    
    local fish = RoomFish()
    fish.FishID = info.fish_id
    fish.ConfigID = info.config_id
    fish.ViewFloor = tFish.ViewFloor
    fish.PathId = info.path_id
    fish.IsMultipleGroup = info.is_multiple_group
    fish:AddPath(info.path_id, info.start_time, info.stop_time, info.offset_x, info.offset_y)
    for k, v in pairs(info.rates) do
        fish:AddTimeRate(v.time, v.duration, v.rate, v.reason)
    end
    table.insert(self.AppearFishList, fish)
    self.IsAppearFishListDirty = true
end

function FishModule:RemoveFish(reason,fish, deltaGold, player, itemList, forceWheel)
    forceWheel = forceWheel and forceWheel or 0
    MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_FISH_DESTROY, {
        Reason = reason,
        Fish = fish,
        DeltaGold = deltaGold,
        Player = player,
        ItemList = itemList,
        ForceWheel = forceWheel
    })
    table.removekvpair(self.RoomFishMap, fish)
end

function FishModule:RemoveFunctionalFish(functionalFish)
    local lst = {}
    local flst = {}
    table.insert(flst, functionalFish.FuncFish.FishID)
    for k, v in pairs(functionalFish.RelatedIds) do
        table.insert(flst, v)
    end
    for k, v in pairs(flst) do
        local find = table.find(self.AppearFishList, function(f)
            return f.FishID == v
        end)
        if find then
            table.removebyvalue(self.AppearFishList, find)
        else
            table.insert(lst, v)
        end
    end
    if not table.contains(lst, functionalFish.FuncFish.FishID) then
        functionalFish.FuncFish = nil
    else
        table.removebyvalue(lst, functionalFish.FuncFish.FishID)
    end
    functionalFish.RelatedFish = {}
    for k, v in pairs(self.RoomFishMap) do
        if table.contains(lst, v.FishID) then
            table.insert(functionalFish.RelatedFish, v)
        end
    end
    if functionalFish.FuncFish then
        local config = table.find(TFishFuncEffect, function(t)
            return t.FunctionalId == functionalFish.Reason
        end)
        if config then
            if config.DurationMin == 0 then
                functionalFish.DelayTime = config.DurationMax
            else
                local time = config.DurationMin * #functionalFish.RelatedFish
                if time < config.DurationMin then time = config.DurationMin end
                if time > config.DurationMax then time = config.DurationMax end
                functionalFish.DelayTime = time
            end
        else
            functionalFish.DelayTime = 0
        end
        MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_FUNCTIONAL_REMOVE_FISH, {
            FunctionalFish = functionalFish
        })
    else
        functionalFish.DelayTime = 0
    end
    self.funcDelayRemoveList[functionalFish.FuncFish.FishID] = functionalFish
end

function FishModule:RemoveFishNtf(ntf, player)
    local lst = {}
    if ntf.fish_ids ~= nil then
        local tmp = {}
        for k, v in ipairs(self.AppearFishList) do
            if table.contains(ntf.fish_ids, v.FishID) then
                table.insert(tmp, v.FishID)
                table.remove(self.AppearFishList, k)
            end
        end
        lst = table.findMap(ntf.fish_ids, function(id)
            return not table.contains(tmp, id)
        end)
    end
    
    local roomFishList = table.findMap(self.RoomFishMap, function(rf)
        return table.contains(lst, rf.FishID)
    end)
    
    if table.count(lst) > 0 or ntf.delta ~= 0 then
        MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_REMOVE_FISH, {
            FishList = roomFishList,
            Reason = ntf.reason,
            Player = player,
            Delta = ntf.delta
        })
    end
    
    for k, v in pairs(roomFishList) do
        if self.RoomFishMap[v.FishID] ~= nil then
            self:RemoveFish(ntf.reason,v, 0, 0)
        end
    end
end

--player operation
function FishModule:GetPlayerByUserId(userId)
    return table.find(self.RoomPlayerMap, function(p)
        return p.UserID == userId
    end)
end

function FishModule:GetPlayerBySeatId(seatId)
    return table.find(self.RoomPlayerMap, function(p)
        return p.ServerSeat == seatId
    end)
end

function FishModule:AddPlayer(player)
    table.insert(self.RoomPlayerMap, player)
    if player.UserID == GameController.Instance.Player.UserID then
        self.PlayerSelf = player
        SeatHelper.SetMyServerSeatPosition(player.ServerSeat)
    end
    MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_PLAYER_JOIN, {
        Player = player,
    })
end

function FishModule:RemovePlayerBySeat(seatId)
    local player = self:GetPlayerBySeatId(seatId)
    if player ~= nil then
        MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_PLAYER_LEAVE, {
            Player = player,
        })
        table.removebyvalue(self.RoomPlayerMap, player)
        if player == self.PlayerSelf then
            self.PlayerSelf = nil
        end
        self:ClearBulletWhenPlayerLeave(seatId)
    end
end

function FishModule:IsPlayerRobotProxy()
    local r = false
    for i = 1, 4 do
        local player = self:GetPlayerBySeatId(i - 1)
        repeat
            if player == nil or player:IsRobot() then break end
        until true
        if player and player:IsPlayerSelf() then r = true end
        break;
    end
    return r
end

--bullet operation
function FishModule:GetBulletById(bulletId)
    return self.RoomBulletMap[bulletId]
end

function FishModule:CreateBullet(seatId, bulletId, angle, lockFishId, bulletIndex, isPenetrate, multiple)
    bulletIndex = bulletIndex and bulletIndex or 1
    isPenetrate = isPenetrate == nil and false or isPenetrate
    multiple = multiple and multiple or 1
    local roomBullet
    local player = self:GetPlayerBySeatId(seatId)
    if player and player.Site and player.Site:GetCannoCtrl(bulletIndex) then
        local fireLocate = player.Site:GetCannoCtrl(bulletIndex).fireLocate
        local startPos = FishRoomController.instance.transform:InverseTransformPoint(fireLocate.transform.position)
        if ((SeatHelper.IsServerSeatNegative() and (seatId == 0 or seatId == 1)) or ((not SeatHelper.IsServerSeatNegative()) and (seatId == 2 or seatId == 3))) then
            angle = angle + 180
        end
        roomBullet = RoomBullet({
            bulletId = bulletId,
            seatId = seatId,
            userId = player.UserID,
            gunValue = player.GunValue,
            startX = startPos.x,
            startY = startPos.y,
            startAngle = angle,
            lockFishId = lockFishId,
            isPenetrate = isPenetrate,
            multiple = multiple
        })
        roomBullet:UpdateSnapshotInfo(TimeHelper.GetServerTimestamp())
    end
    if roomBullet then self:AddBullet(roomBullet, player) end
    return roomBullet
end

function FishModule:AddBullet(bullet, player)
    self.RoomBulletMap[bullet.BulletId] = bullet
    if player:IsPlayerSelf() then
        self.PlayerBulletActiveCount = self.PlayerBulletActiveCount + 1
    end
    MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_BULLET_CREATE, {
        Bullet = bullet,
        Player = player
    })
end

function FishModule:RemoveBullet(bulletId, isShowNet)
    local bullet = self.RoomBulletMap[bulletId]
    if bullet then
        if self:GetPlayerBySeatId(bullet.SeatId) and self:GetPlayerBySeatId(bullet.SeatId):IsPlayerSelf() then self.PlayerBulletActiveCount = self.PlayerBulletActiveCount - 1 end
        MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_BULLET_DESTROY, {
            Bullet = bullet,
            IsShowNet = isShowNet
        })
        table.removekvpair(self.RoomBulletMap, bullet)
    end
end

function FishModule:RemoveBulletFromWaitingQueue(bullet)
    table.removebyvalue(self.RoomBulletWaitingList, bullet)
end

function FishModule:GetBulletPermit()
    return TFishGlobal[1].BulletLimit - self.PlayerBulletActiveCount
end

function FishModule:ClearBulletWhenPlayerLeave(seatId)
    local tbl = table.findMap(self.RoomBulletMap, function(b)
        return b.SeatId == seatId
    end)
    for k, v in pairs(tbl) do
        MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_BULLET_DESTROY, {
            Bullet = v,
            IsShowNet = false
        })
        table.removekvpair(self.RoomBulletMap, v)
    end
end

function FishModule:AcceptNewBulletInfo(bullet, newBulletId, lockFishId)
    if self.RoomBulletMap[bullet.BulletId] ~= nil then
        table.removekvpair(self.RoomBulletMap, bullet)
        MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_BULLET_UPDATE_INFO, {
            Bullet = bullet,
            NewBulletId = newBulletId
        })
        bullet:AcceptNewBulletInfo(newBulletId, lockFishId)
        self.RoomBulletMap[bullet.BulletId] = bullet
    else
        table.removebyvalue(self.RoomBulletWaitingList, bullet)
    end
    for k, v in pairs(bullet.WaitingHitFishList) do
        if k > 0 then
            if bullet.IsPenetrate then
                local result = FishRoomController.instance:checkPenetrateHit(bullet.GunValue)
                if result ~= -1 then
                    NetController.instance:SendAcrossHitReq(newBulletId, k, v, result)
                end
            else
                NetController.instance:SendHitReq(newBulletId, k, v)
            end
        end
    end
    bullet.WaitingHitFishList = {}
end

function FishModule:AddBulletToWaitingList(bullet)
    table.insert(self.RoomBulletWaitingList, bullet)
end

function FishModule:FishTideStart(ntf)
    MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_FISHTIDE_NTF,{
        Ntf = ntf
    })
end

function FishModule:AddTimeRateToNotBornFish(fishId, realTime, durationInMillionSeconds,rateInTenThousand,reason)
    local fish = nil
    for k,v in pairs(self.AppearFishList) do
        if v.FishID == fishId then
            fish = v
            break
        end
    end
    if fish then
        fish:AddTimeRate(realTime, durationInMillionSeconds, rateInTenThousand, reason)
        self.IsAppearFishListDirty = true
    end
end
