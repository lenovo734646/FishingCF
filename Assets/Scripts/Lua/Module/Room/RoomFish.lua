RoomFish = class(nil, {
    FishID,
    ConfigID,
    ViewFloor,
    PathId,
    IsMultipleGroup,
    
    timeConverter,
    fishPath,
    
    fishTimeForBorn, --鱼出生时所对应的鱼的时间
    fishTimeForDead, --鱼死亡时所对应的鱼的时间
    realTimeForBorn, --鱼出生时所对应的真实时间
    realTimeForDead, --鱼死亡时所对应的真实时间
    
    isDirty,
    
    cacheFishTime, --缓存鱼上一帧的时间戳
    
    SnapshotInfo,
})

function RoomFish:__init()
    self.FishID = 0
    self.ConfigID = 0
    self.ViewFloor = 0
    self.IsMultipleGroup = 0
    
    self.timeConverter = FishTimeConverter()
    self.fishPath = FishPath()
    
    self.fishTimeForBorn = 0
    self.fishTimeForDead = 0
    self.realTimeForBorn = 0
    self.realTimeForDead = 0
    
    self.isDirty = false
    
    self.cacheFishTime = 0
    
    self.SnapshotInfo = {
        position = Vector3.zero,
        rotation = 0,
        direction = 1
    }
end

function RoomFish:AddPath(pathId, startTime, stopTime, offsetX, offsetY)
    self.fishTimeForBorn = startTime
    self.fishTimeForDead = stopTime
    --local pathName = TPathName[pathId].name
    local points = TPathWayPoints[pathId].points
    --points = json.decode(points)
    local path = {}
    for k, v in ipairs(points) do
        table.insert(path, Vector3(v[1], v[2], v[3]))
    end
    self.fishPath:InitPath(path)
    self.isDirty = true
end

function RoomFish:AddTimeRate(time, duration, rate, reason)
    self.timeConverter:addTimeRate(time, duration, rate, reason)
    self.isDirty = true
end

function RoomFish:GetRealTimeForBorn()
    if self.isDirty then self:updateData() end
    return self.realTimeForBorn
end

function RoomFish:IsBorned(time)
    if self.isDirty then self:updateData() end
    return time > self.realTimeForBorn
end

function RoomFish:IsExpired(time)
    if self.isDirty then self:updateData() end
    return time > self.realTimeForDead
end

function RoomFish:UpdateSnapshotInfo(time)
    if self.isDirty then self:updateData() end
    local fishTime = self.timeConverter:convertRealTimeToFishTime(time)
    local timeRate = self.timeConverter:getTimeRate(time)
    local reason = self.timeConverter:getReason(time)
    
    fishTime = math.max(fishTime, self.cacheFishTime)
    self.cacheFishTime = fishTime
    if fishTime < self.fishTimeForBorn or fishTime >= self.fishTimeForDead then
        return nil
    else
        self.SnapshotInfo.fishId = self.FishID
        self.SnapshotInfo.configId = self.ConfigID
        self.SnapshotInfo.timeRate = timeRate
        self.SnapshotInfo.timeRateReason = reason
        
        local t = (fishTime - self.fishTimeForBorn) / (self.fishTimeForDead - self.fishTimeForBorn)
        self:fillSnapshotInfoByPath(t)
        
        return self.SnapshotInfo
    end
end

function RoomFish:updateData()
    if not self.isDirty then return end
    
    self.isDirty = false
    self.realTimeForBorn = self.timeConverter:convertFishTimeToRealTime(self.fishTimeForBorn)
    self.realTimeForDead = self.timeConverter:convertFishTimeToRealTime(self.fishTimeForDead)
end

function RoomFish:fillSnapshotInfoByPath(time)
    local data = self.fishPath:GetData(time)
    self.SnapshotInfo.position = Vector3(data.position.x,data.position.y,0) 
    self.SnapshotInfo.rotation = data.rotation
    self.SnapshotInfo.direction = data.direction
end