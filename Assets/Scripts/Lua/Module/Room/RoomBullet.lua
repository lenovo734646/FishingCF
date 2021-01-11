RoomBullet = class(nil, {})

local DesignWidth = 1920
local DesignHeight = 1080

function RoomBullet:__init(bulletInfo)
    self.SnapshotInfo = {}
    
    self.BulletId = bulletInfo.bulletId
    self.SeatId = bulletInfo.seatId
    self.UserId = bulletInfo.userId
    self.GunValue = bulletInfo.gunValue
    self.LockFishId = 0
    self.IsPenetrate = bulletInfo.isPenetrate
    self.Multiple = bulletInfo.multiple
    self.startPosX = bulletInfo.startX + DesignWidth * 0.5
    self.startPosY = bulletInfo.startY + DesignHeight * 0.5
    self.startTimestamp = TimeHelper.GetServerTimestamp()
    self.startAngle = bulletInfo.startAngle
    self.pathNodeList = {}
    self.currentNodeIndex = 1
    self:expand()
    
    self.WaitingHitFishList = {}
    
    if bulletInfo.isPenetrate then return end
    local lockTarget
    if bulletInfo.lockFishId > 0 then
        local fish = FishRoomController.instance.FishSpawn:FindFishById(bulletInfo.lockFishId)
        if fish then lockTarget = fish.transform end
    end
    if lockTarget then
        local pos = FishRoomController.instance.transform:InverseTransformPoint(lockTarget.transform.position)
        local dx = pos.x + DesignWidth * 0.5
        local dy = pos.y + DesignHeight * 0.5
        local distance = UnityHelper.CalculateDistance(Vector2(dx, dy), Vector2(self.startPosX, self.startPosY))
        self.totalLockTime = 1000 * distance / (SysDefines.BulletSpeed * LocalDefines.NormalBulletSpeedRate * (self.IsPenetrate and LocalDefines.PenetrateBulletSpeedRate or 1))
        self.LockFishId = bulletInfo.lockFishId
    end
end

function RoomBullet:AcceptNewBulletInfo(newBulletId, lockFishId)
    self.BulletId = newBulletId
    if self.LockFishId ~= 0 and lockFishId == 0 then
        self.LockFishId = 0
    end
end

function RoomBullet:UpdateSnapshotInfo(timestamp)
    if self.LockFishId > 0 and self:fillSnapshotByLockFish(timestamp) then return end
    
    self.LockFishId = 0
    local node = self.pathNodeList[self.currentNodeIndex]
    while (timestamp > node.EndTimestamp)
    do
        self.currentNodeIndex = self.currentNodeIndex + 1
        if #self.pathNodeList <= self.currentNodeIndex then
            node = self:expand()
        else
            node = self.pathNodeList[self.currentNodeIndex]
        end
    end
    self:fillSnapshotInfo(self.SnapshotInfo, node, timestamp)
end

function RoomBullet:fillSnapshotInfo(info, node, timestamp)
    info.bulletId = self.BulletId
    info.userId = self.UserId
    info.gunValue = self.GunValue
    info.IsPlayerSelf = self.UserId == GameController.Instance.Player.UserID
    info.IsBelongToRobot = self.UserId < 1000000
    
    local rate = (timestamp - node.StartTimestamp) / (node.EndTimestamp - node.StartTimestamp)
    info.x = (node.StartPosition.x + (node.EndPosition.x - node.StartPosition.x) * rate) - DesignWidth * 0.5
    info.y = (node.StartPosition.y + (node.EndPosition.y - node.StartPosition.y) * rate) - DesignHeight * 0.5
    info.rotation = node.Angle
end

function RoomBullet:fillSnapshotByLockFish(timestamp)
    if self.totalLockTime <= 0 or timestamp - self.startTimestamp > self.totalLockTime then return false end
    local lockTarget
    if self.LockFishId > 0 then
        local fish = FishRoomController.instance.FishSpawn:FindFishById(self.LockFishId)
        if fish then lockTarget = fish.transform end
    end
    if not lockTarget then return false end
    
    local pos = FishRoomController.instance.transform:InverseTransformPoint(lockTarget.transform.position)
    local rate = (timestamp - self.startTimestamp) / self.totalLockTime
    local dx = pos.x + DesignWidth * 0.5
    local dy = pos.y + DesignHeight * 0.5
    local x = self.startPosX + (dx - self.startPosX) * rate
    local y = self.startPosY + (dy - self.startPosY) * rate
    
    self.SnapshotInfo.bulletId = self.BulletId
    self.SnapshotInfo.userId = self.UserId
    self.SnapshotInfo.gunValue = self.GunValue
    self.SnapshotInfo.IsPlayerSelf = self.userId == GameController.Instance.Player.UserID
    self.SnapshotInfo.IsBelongToRobot = self.UserId < 1000000
    self.SnapshotInfo.x = x - DesignWidth * 0.5
    self.SnapshotInfo.y = y - DesignHeight * 0.5
    self.SnapshotInfo.rotation = self:VectorAngle(Vector2(self.startPosX, self.startPosY), Vector2(dx, dy))
    return true
end

function RoomBullet:VectorAngle(from, to)
    local angle = UnityHelper.CalculateAnticlockwiseAngle(from.x, from.y, to.x, to.y)
    if angle <= 0 then angle = angle + 360 end
    return angle
end

function RoomBullet:expand()
    local node
    if #self.pathNodeList == 0 then
        node = self:generateBulletPathNode(self.startTimestamp, Vector2(self.startPosX, self.startPosY), self.startAngle)
    else
        local refer = self.pathNodeList[#self.pathNodeList]
        node = self:generateBulletPathNode(refer.EndTimestamp, refer.EndPosition, refer.ReboundAngle)
    end
    table.insert(self.pathNodeList, node)
    return node
end

function RoomBullet:generateBulletPathNode(timestamp, startPos, angle)
    angle = UnityHelper.RectifyAngle(angle)
    if angle == 0 then
        return self:constructBulletPathNode(timestamp, startPos, angle, Vector2(self.IsPenetrate and 10000 or DesignWidth, startPos.y), 180)
    elseif angle == 90 then
        return self:constructBulletPathNode(timestamp, startPos, angle, Vector2(startPos.x, self.IsPenetrate and 10000 or DesignHeight), 270)
    elseif angle == 180 then
        return self:constructBulletPathNode(timestamp, startPos, angle, Vector2(self.IsPenetrate and -10000 or 0, startPos.y), 0)
    elseif angle == 270 then
        return self:constructBulletPathNode(timestamp, startPos, angle, Vector2(startPos.x, self.IsPenetrate and -10000 or 0), 90)
    end
    
    local collisionX = Vector2.zero
    local collisionY = Vector2.zero
    local reboundAngleX = 0
    local reboundAngleY = 0
    
    local len
    if angle < 90 then
        len = (DesignHeight - startPos.y) * math.tan(UnityHelper.AngleToRadian(90 - angle))
        collisionX.x = startPos.x + len
        collisionX.y = DesignHeight
        reboundAngleX = 360 - angle
        
        len = (DesignWidth - startPos.x) * math.tan(UnityHelper.AngleToRadian(angle))
        collisionY.x = DesignWidth
        collisionY.y = startPos.y + len
        reboundAngleY = 180 - angle
    elseif angle < 180 then
        len = (DesignHeight - startPos.y) * math.tan(UnityHelper.AngleToRadian(angle - 90))
        collisionX.x = startPos.x - len
        collisionX.y = DesignHeight
        reboundAngleX = 360 - angle
        
        len = startPos.x * math.tan(UnityHelper.AngleToRadian(180 - angle))
        collisionY.x = 0
        collisionY.y = startPos.y + len
        reboundAngleY = 180 - angle
    elseif angle < 270 then
        len = startPos.y * math.tan(UnityHelper.AngleToRadian(270 - angle))
        collisionX.x = startPos.x - len
        collisionX.y = 0
        reboundAngleX = 360 - angle
        
        len = startPos.x * math.tan(UnityHelper.AngleToRadian(angle - 180))
        collisionY.x = 0
        collisionY.y = startPos.y - len
        reboundAngleY = 540 - angle
    else
        len = startPos.y * math.tan(UnityHelper.AngleToRadian(angle - 270))
        collisionX.x = startPos.x + len
        collisionX.y = 0
        reboundAngleX = 360 - angle
        
        len = (DesignWidth - startPos.x) * math.tan(UnityHelper.AngleToRadian(360 - angle))
        collisionY.x = DesignWidth
        collisionY.y = startPos.y - len
        reboundAngleY = 540 - angle
    end
    
    local isCollisionX = collisionX.x >= 0 and collisionX.x < DesignWidth
    if self.IsPenetrate then
        isCollisionX = not isCollisionX
    end
    return self:constructBulletPathNode(timestamp, startPos, angle, isCollisionX and collisionX or collisionY, isCollisionX and reboundAngleX or reboundAngleY)
end

function RoomBullet:constructBulletPathNode(timestamp, startPos, angle, endPos, reboundAngle)
    local r = {
        StartTimestamp = timestamp,
        StartPosition = startPos,
        EndPosition = endPos,
        Angle = angle,
        ReboundAngle = reboundAngle,
    }
    local distance = UnityHelper.CalculateDistance(startPos, endPos)
    local time = distance / (SysDefines.BulletSpeed * LocalDefines.NormalBulletSpeedRate * (self.IsPenetrate and LocalDefines.PenetrateBulletSpeedRate or 1))
    r.EndTimestamp = r.StartTimestamp + math.max(time * 1000, 1)
    return r
end
