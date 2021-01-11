RoomPlayer = class(nil, {})

function RoomPlayer:__init(playerInfo)
    self.Module = ModuleManager.instance:Get("FishModule")
    self.room = TFishRoom[self.Module.RoomID]
    self.tblGunValue = {}
    self.tblGunId = {}
    for k, v in pairs(TFishGunValue) do
        if v.GunValue >= self.room.MinGun and v.GunValue <= self.room.MaxGun then
            table.insert(self.tblGunValue, v.GunValue)
            table.insert(self.tblGunId, v.GunModel)
        end
    end
    table.print(self.tblGunValue)
    table.print(self.tblGunId)
    self.UserID = playerInfo.user_id
    self.NickName = playerInfo.nickname
    self.Gender = playerInfo.gender
    self.Head = playerInfo.head
    self.HeadFrame = playerInfo.head_frame
    self.Level = playerInfo.level
    self.LevelExp = playerInfo.level_exp
    self.VIPLevel = playerInfo.vip_level
    self.Currency = playerInfo.currency
    self.Diamond = playerInfo.diamond
    self.ServerSeat = playerInfo.seat_id
    self.GunValue = playerInfo.gun_value
    self.BulletLeft = playerInfo.left_gun_num
    self.GameIntegral = playerInfo.integral
    self.ItemUsingQueue = {}
    local curIndex = 1
    for k, v in pairs(self.tblGunValue) do
        if v == self.GunValue then
            curIndex = k
        end
    end
    self.CannoId = self.tblGunId[curIndex][self.Module.RoomID]
end

function RoomPlayer:GetClientSeat()
    return SeatHelper.ConvertServerSeatPositionToClient(self.ServerSeat)
end

function RoomPlayer:IsRobot()
    return self.UserID < 1000000
end

function RoomPlayer:IsPlayerSelf()
    return self.UserID == GameController.Instance.Player.UserID
end

function RoomPlayer:SetCurrency(currency)
    if self.Currency ~= currency then
        local oldValue = self.Currency
        self.Currency = currency
        MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_PLAYER_CURRENCY_CHANGED, {
            Player = self,
            OldValue = oldValue,
            NewValue = self.Currency
        })
    end
end

function RoomPlayer:SetDiamond(diamond)
    if self.Diamond ~= diamond then
        local oldValue = self.Diamond
        self.Diamond = diamond
        MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_PLAYER_DIAMOND_CHANGED, {
            Player = self,
            OldValue = oldValue,
            NewValue = self.Diamond
        })
    end
end

function RoomPlayer:SetRotationAngle(angles, lockFishIdArray)
    MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_GUN_ROTATION_CHANGED, {
        Player = self,
        ClientSeat = self:GetClientSeat(),
        Angle = angles,
        FishIdArray = lockFishIdArray
    })
end

function RoomPlayer:SetGunValue(gunValue)
    if self.GunValue ~= gunValue then
        local oldValue = self.GunValue
        self.GunValue = gunValue
        MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_PLAYER_GUNVALUE_CHANGED, {
            Player = self,
            OldValue = oldValue,
            NewValue = self.GunValue
        })
    end
end

function RoomPlayer:DeltaCurrency(delta)
    if delta ~= 0 then
        local oldValue = self.Currency
        self.Currency = self.Currency + delta
        MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_PLAYER_CURRENCY_CHANGED, {
            Player = self,
            OldValue = oldValue,
            NewValue = self.Currency
        })
    end
end

function RoomPlayer:DeltaDiamond(delta)
    if delta ~= 0 then
        local oldValue = self.Diamond
        self.Diamond = self.Diamond + delta
        MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_PLAYER_DIAMOND_CHANGED, {
            Player = self,
            OldValue = oldValue,
            NewValue = self.Diamond
        })
    end
end

function RoomPlayer:SwitchGun(gunId)
    if self.CannoId ~= gunId then
        self.CannoId = gunId
        MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_PLAYER_GUN_CHANGED, {
            Player = self,
            GunId = self.CannoId,
            ClientSeat = self.ServerSeat
        })
    end
end

function RoomPlayer:SetFishingSite(siteCtrl)
    self.Site = siteCtrl
end

function RoomPlayer:AddItemUsingInfo(itemUsing)
    table.insert(self.ItemUsingQueue, itemUsing)
end

function RoomPlayer:ApplyItemUsingInfo()
    local item = self.ItemUsingQueue[1]
    table.remove(self.ItemUsingQueue, 1)
    MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_PLAYER_USEITEM, {
        Player = self,
        ItemInfo = item,
    })
end
