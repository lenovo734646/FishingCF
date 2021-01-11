SeatHelper = {}

local myServerSeatPosition

function SeatHelper.SetMyServerSeatPosition(serverPos)
    if serverPos >= 0 and serverPos <= 3 then
        myServerSeatPosition = serverPos
    end
end

function SeatHelper.IsServerSeatNegative()
    return myServerSeatPosition == 2 or myServerSeatPosition == 3
end

function SeatHelper.ConvertServerSeatPositionToClient(serverPos)
    local r = serverPos
    if myServerSeatPosition == 2 or myServerSeatPosition == 3 then
        if serverPos == 0 then
            r = 2
        elseif serverPos == 1 then
            r = 3
        elseif serverPos == 2 then
            r = 0
        elseif serverPos == 3 then
            r = 1
        end
    end
    return r
end
