NetController = class(nil, {})

local ProtocolHelper = require("JBPROTO.ProtocolHelper")

function NetController:__init()
    self:AddNotifyListener()
end

function NetController:AddNotifyListener()
    ProtocolHelper.addNotifyHandler("CLFRFishAppearNtf", functional.bind1(self.onRecv_CLFRFishAppearNtf, self))
    ProtocolHelper.addNotifyHandler("CLFRFishTimeRateChangeNtf", functional.bind1(self.onRecv_CLFRFishTimeRateChangeNtf, self))
    ProtocolHelper.addNotifyHandler("CLFRPlayerJoinNtf", functional.bind1(self.onRecv_CLFRPlayerJoinNtf, self))
    ProtocolHelper.addNotifyHandler("CLFRPlayerLeaveNtf", functional.bind1(self.onRecv_CLFRPlayerLeaveNtf, self))
    ProtocolHelper.addNotifyHandler("CLFRGunValueChangeNtf", functional.bind1(self.onRecv_CLFRGunValueChangeNtf, self))
    ProtocolHelper.addNotifyHandler("CLFRShootNtf", functional.bind1(self.onRecv_CLFRShootNtf, self))
    ProtocolHelper.addNotifyHandler("CLFRMultiShootNtf", functional.bind1(self.onRecv_CLFRMultiShootNtf, self))
    ProtocolHelper.addNotifyHandler("CLFRAcrossShootNtf", functional.bind1(self.onRecv_CLFRAcrossShootNtf, self))
    ProtocolHelper.addNotifyHandler("CLFRAcrossMultiShootNtf", functional.bind1(self.onRecv_CLFRAcrossMultiShootNtf, self))
    ProtocolHelper.addNotifyHandler("CLFRHitNtf", functional.bind1(self.onRecv_CLFRHitNtf, self))
    ProtocolHelper.addNotifyHandler("CLFRAcrossHitNtf", functional.bind1(self.onRecv_CLFRAcrossHitNtf, self))
    ProtocolHelper.addNotifyHandler("CLFRFishRemoveNtf", functional.bind1(self.onRecv_CLFRFishRemoveNtf, self))
    ProtocolHelper.addNotifyHandler("CLFRShowWheel1Ntf", functional.bind1(self.onRecv_CLFRShowWheel1Ntf, self))
    ProtocolHelper.addNotifyHandler("CLFRShowWheel2Ntf", functional.bind1(self.onRecv_CLFRShowWheel2Ntf, self))
    ProtocolHelper.addNotifyHandler("CLFRSeatResourceChangedNtf", functional.bind1(self.onRecv_CLFRSeatResourceChangedNtf, self))
    ProtocolHelper.addNotifyHandler("CLFRBonusPoolChangedNtf", functional.bind1(self.onRecv_CLFRBonusPoolChangedNtf, self))
    ProtocolHelper.addNotifyHandler("CLFRFishTideStartNtf", functional.bind1(self.onRecv_CLFRFishTideStartNtf, self))
    ProtocolHelper.addNotifyHandler("CLFREnergyShootNtf", functional.bind1(self.onRecv_CLFREnergyShootNtf, self))
end

function NetController:CreateErrorMsg(proto, errcode)
    local key = proto .. "_" .. errcode
    local hintMsg = table.find(TLanguageErrcode, function(t)
        return t.key == key
    end).CN
    GameController.Instance:CreateHintMessage(hintMsg)
end

--加入玩法请求
function NetController:SendEnterSiteReq(siteId, roomId, seatId)
    roomId = roomId ~= nil and roomId or -1
    seatId = seatId ~= nil and seatId or -1
    local proto = CLFMEnterSiteReq()
    proto.site_id = siteId
    proto:asyncRequest("CLFMEnterSiteAck", function(ack)
        log("CLFMEnterSiteAck:" .. json.encode(ack))
        if ack.errcode == 0 then
            self:SendEnterFishingGameReq(LocalDefines.RoomConfigID, roomId, seatId)
        else
            self:CreateErrorMsg("CLFMEnterSiteAck", ack.errcode)
        end
    end)
end

--退出玩法请求
function NetController:SendExitSiteReq(siteId, reason)
    reason = reason ~= nil and reason or 0
    local proto = CLFMExitSiteReq()
    proto.site_id = siteId
    proto:asyncRequest("CLFMExitSiteAck", function(ack)
        log("CLFMExitSiteAck:" .. json.encode(ack))
        if ack.errcode == 0 then
            -- MessageCenter.instance:SendMessage(MsgType.NET_EXITSITE_ACK, {
            --     SiteId = siteId,
            --     Reason = reason
            -- })
            FishUIController.instance:OnLeaveFishRoom()
        else
            self:CreateErrorMsg("CLFMExitSiteAck", ack.errcode)
        end
    end)
end

--进入捕鱼游戏
function NetController:SendEnterFishingGameReq(configId, roomId, seatId)
    local proto = CLFREnterGameReq()
    proto.config_id = configId
    proto.room_id = roomId
    proto.seat_id = seatId
    proto:asyncRequest("CLFREnterGameAck", function(ack)
        if ack.errcode == 0 then
            FishRoomController.instance:ProcessEnterFishingRoomAck(ack, configId)
        else
            self:CreateErrorMsg("CLFREnterGameAck", ack.errcode)
        end
    end)
end

--退出捕鱼房间
function NetController:SendExitGameReq(action)
    local proto = CLFRExitGameReq()
    proto:asyncRequest("CLFRExitGameAck", function(ack)
        if ack.errcode == 0 then
            FishRoomController.instance:ProcessExitGameAck(ack, action)
        else
            self:CreateErrorMsg("CLFRExitGameAck", ack.errcode)
        end
    end)
end

--表示客户端已准备可以接收服务器的推送
function NetController:SendGetReadyReq()
    local proto = CLFRGetReadyReq()
    proto:asyncRequest("CLFRGetReadyAck", function(ack)
        if ack.errcode == 0 then
            FishRoomController.instance:ProcessGetReadyAck(ack)
        else
            self:CreateErrorMsg("CLFRGetReadyAck", ack.errcode)
        end
    end)
end

--发炮请求
function NetController:SendShootReq(angle, lockFishId, isPenetrate, multiple)
    if not LocalDefines.IsInFishingGame then return end
    
    multiple = multiple ~= nil and multiple or 1
    if not isPenetrate then
        local bullet = FishRoomController.instance:CreateVirtualBulletForPlayerSelf(angle, lockFishId, false, multiple)
        if bullet then
            local proto = CLFRShootReq()
            proto.angle = angle
            proto.lock_fish = lockFishId
            proto.multiple = multiple
            proto:asyncRequest("CLFRShootAck", function(ack)
                FishRoomController.instance:ProcessShootAck(ack, angle, bullet)
            end)
        end
    else
        local bullet = FishRoomController.instance:CreateVirtualBulletForPlayerSelf(angle, 0, true, multiple)
        if bullet then
            local proto = CLFRAcrossShootReq()
            proto.angle = angle
            proto.multiple = multiple
            proto:asyncRequest("CLFRAcrossShootAck", function(ack)
                FishRoomController.instance:ProcessAcrossShootAck(ack, angle, bullet)
            end)
        end
    end
end

--多炮台同时发炮请求
function NetController:SendCloneShootReq(angles, lockFishIds, isPenetrate, multiple)
    if not LocalDefines.IsInFishingGame then return end
    
    multiple = multiple ~= nil and multiple or 1
    if not isPenetrate then
        local proto = CLFRMultiShootReq()
        proto.shoot_len = #angles
        local bullets = {}
        local info = {}
        for i = 1, #angles do
            local bullet = FishRoomController.instance:CreateVirtualBulletForPlayerSelf(angles[i], lockFishIds[i], false, multiple, i + 1)
            if bullet then
                table.insert(info, {
                    angle = angles[i],
                    lock_fish = lockFishIds[i]
                })
                table.insert(bullets, bullet)
            end
        end
        proto.shoot_array = info
        proto.multiple = multiple
        if #info == #angles then
            proto:asyncRequest("CLFRMultiShootAck", function(ack)
                FishRoomController.instance:ProcessMultiShootAck(ack, bullets)
            end)
        end
    else
        local proto = CLFRAcrossMultiShootReq()
        proto.shoot_len = #angles
        local bullets = {}
        local info = {}
        for i = 1, #angles do
            local bullet = FishRoomController.instance:CreateVirtualBulletForPlayerSelf(angles[i], 0, true, multiple, i + 1)
            if bullet then
                table.insert(info, {
                    angle = angles[i],
                    lock_fish = lockFishIds[i]
                })
                table.insert(bullets, bullet)
            end
        end
        proto.shoot_array = info
        proto.multiple = multiple
        proto:asyncRequest("CLFRAcrossMultiShootAck", function(ack)
            FishRoomController.instance:ProcessAcrossMultiShootAck(ack, bullets)
        end)
    end
end

--命中请求
function NetController:SendHitReq(bulletId, fishId, related)
    if not LocalDefines.IsInFishingGame then return end
    
    local proto = CLFRHitReq()
    proto.bullet_id = bulletId
    proto.fish_id = fishId;
    if related then
        proto.related_fish_array = related
        proto.related_fish_len = #related
    end
    proto:asyncRequest("CLFRHitAck", function(ack)
        FishRoomController.instance:ProcessHitAck(ack, bulletId, fishId)
    end)
end

--上报机器人子弹碰撞信息
function NetController:SendRobotHitRpt(bulletId, fishId)
    if not LocalDefines.IsInFishingGame then return end
    
    local proto = CLFRRobotHitRpt()
    proto.bullet_id = bulletId
    proto.fish_id = fishId
    proto:send()
end

--穿透命中请求
function NetController:SendAcrossHitReq(bulletId, fishId, related, hitCache)
    if not LocalDefines.IsInFishingGame then return end
    
    local proto = CLFRAcrossHitReq()
    proto.bullet_id = bulletId
    proto.fish_id = fishId;
    if related then
        proto.related_fish_array = related
        proto.related_fish_len = #related
    end
    proto:asyncRequest("CLFRAcrossHitAck", function(ack)
        FishRoomController.instance:ProcessAcrossHitAck(ack, bulletId, fishId, hitCache)
    end)
end

--改变炮值请求
function NetController:SendGunValueChangeReq(gunValue)
    if not LocalDefines.IsInFishingGame then return end

    local proto = CLFRGunValueChangeReq()
    proto.gun_value = gunValue
    proto:asyncRequest("CLFRGunValueChangeAck", function(ack)
        FishRoomController.instance:ProcessGunValueChangeAck(ack)
    end)
end

--奖金抽奖请求
function NetController:SendBonusWheelReq()
    local proto = CLFRBonusWheelReq()
    proto:asyncRequest("CLFRBonusWheelAck", function(ack)
        MessageCenter.instance:SendMessage(MsgType.NET_BONUSWHEEL_ACK,ack)
    end)
end

--能量炮蓄力请求
function NetController:SendEnergyStoreReq()
    if not LocalDefines.IsInFishingGame then return end

    local proto = CLFREnergyStoreReq()
    proto:asyncRequest("CLFREnergyStoreAck", function(ack)
        FishRoomController.instance:ProcessEnergyStoreAck(ack)
    end)
end

--能量炮发射请求
function NetController:SendEnergyShootReq(angle,idArray)
    if not LocalDefines.IsInFishingGame then return end

    local proto = CLFREnergyShootReq()
    proto.angle = angle
    proto.related_fish_len = #idArray
    proto.related_fish_array = idArray
    proto:asyncRequest("CLFREnergyShootAck", function(ack)
        FishRoomController.instance:ProcessEnergyShootAck(ack)
    end)
end

--测试用!!!召唤鱼潮
function NetController:SendFishTideForTestReq()
    local proto = CLFRFishTideForTestReq()
    proto:asyncRequest("CLFRFishTideForTestAck", function(ack)
        FishRoomController.instance:ProcessFishTideForTest(ack)
    end)
end

--新鱼出现通知
function NetController:onRecv_CLFRFishAppearNtf(ntf)
    FishRoomController.instance:ProcessFishAppearNtf(ntf)
end

--鱼的时间变化率改变通知
function NetController:onRecv_CLFRFishTimeRateChangeNtf(ntf)
    FishRoomController.instance:ProcessFishTimeRateChangeNtf(ntf)
end

--玩家加入通知
function NetController:onRecv_CLFRPlayerJoinNtf(ntf)
    FishRoomController.instance:ProcessPlayerJoinNtf(ntf)
end

--玩家离开通知
function NetController:onRecv_CLFRPlayerLeaveNtf(ntf)
    FishRoomController.instance:ProcessPlayerLeaveNtf(ntf)
end

--炮值改变通知
function NetController:onRecv_CLFRGunValueChangeNtf(ntf)
    FishRoomController.instance:ProcessGunValueChangeNtf(ntf)
end

--发炮通知
function NetController:onRecv_CLFRShootNtf(ntf)
    FishRoomController.instance:ProcessShootNtf(ntf)
end

--多炮台同时发炮通知
function NetController:onRecv_CLFRMultiShootNtf(ntf)
    FishRoomController.instance:ProcessMultiShootNtf(ntf)
end

--穿透发炮通知
function NetController:onRecv_CLFRAcrossShootNtf(ntf)
    FishRoomController.instance:ProcessAcrossShootNtf(ntf)
end

--多炮台穿透发炮通知
function NetController:onRecv_CLFRAcrossMultiShootNtf(ntf)
    FishRoomController.instance:ProcessAcrossMultiShootNtf(ntf)
end

--命中通知
function NetController:onRecv_CLFRHitNtf(ntf)
    FishRoomController.instance:ProcessHitNtf(ntf)
end

--穿透命中通知
function NetController:onRecv_CLFRAcrossHitNtf(ntf)
    FishRoomController.instance:ProcessAcrossHitNtf(ntf)
end

--鱼被移除通知
function NetController:onRecv_CLFRFishRemoveNtf(ntf)
    FishRoomController.instance:ProcessFishRemoveNtf(ntf)
end

--转盘鱼通知
function NetController:onRecv_CLFRShowWheel1Ntf(ntf)
    FishRoomController.instance:ProcessShowWheel1Ntf(ntf)
end

--双转盘鱼通知
function NetController:onRecv_CLFRShowWheel2Ntf(ntf)
    FishRoomController.instance:ProcessShowWheel2Ntf(ntf)
end

--房间内座位资源变化通知
function NetController:onRecv_CLFRSeatResourceChangedNtf(ntf)
    FishRoomController.instance:ProcessSeatResourceChangedNtf(ntf)
end

--奖金鱼池奖金变动通知
function NetController:onRecv_CLFRBonusPoolChangedNtf(ntf)
    FishRoomController.instance:ProcessBonusPoolChangedNtf(ntf)
end

--鱼潮来临通知
function NetController:onRecv_CLFRFishTideStartNtf(ntf)
    FishRoomController.instance:ProcessFishTideStartNtf(ntf)
end

--能量炮发射通知
function NetController:onRecv_CLFREnergyShootNtf(ntf)
    FishRoomController.instance:ProcessEnergyShootNtf(ntf)
end

