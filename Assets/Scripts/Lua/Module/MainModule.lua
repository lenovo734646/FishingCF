MainModule = class(nil, {})

function MainModule:__init()
    
end

--加入玩法请求
function MainModule:SendEnterSiteReq(siteId, roomId, seatId)
    roomId = roomId ~= nil and roomId or -1
    seatId = seatId ~= nil and seatId or -1
    local proto = CLFMEnterSiteReq()
    proto.site_id = siteId
    proto:asyncRequest("CLFMEnterSiteAck", function(ack)
        log("CLFMEnterSiteAck:" .. json.encode(ack))
        local hintMsg = ""
        if ack.errcode == 0 then
            self:SendEnterFishingGameReq(siteId, roomId, seatId)
        else
            hintMsg = table.find(TLanguageErrcode, function(t)
                return t.key == "CLFMEnterSiteAck_" .. ack.errcode
            end).CN
        end
        
        if not string.isNullOrEmpty(hintMsg) then
            log(hintMsg)
            GameController.Instance:CreateHintMessage(hintMsg)
        end
    end)
end

--退出玩法请求
function MainModule:SendExitSiteReq(siteId, reason)
    local proto = CLFMExitSiteReq()
    proto.site_id = siteId
    proto:asyncRequest("CLFMExitSiteAck", function(ack)
        
    end)
end

--进入捕鱼游戏
function MainModule:SendEnterFishingGameReq(configId, roomId, seatId)
    local proto = CLFREnterGameReq()
    proto.config_id = configId
    proto.room_id = roomId
    proto.seat_id = seatId
    proto:asyncRequest("CLFREnterGameAck", function(ack)
        if ack.errcode == 0 then
            FishRoomController.instance:ProcessEnterFishingRoomAck(ack, configId)
        end
    end)
end