FishPath = class(nil, {
    data,
    pathType, --1曲线 2直线
    points,
    lastPosition,
})

function FishPath:__init()
    self.data = {
        position = Vector3.zero,
        rotation = 0
    }
    self.points = {}
end

function FishPath:InitPath(pathNodeList)
    -- local testMode = 2
    -- local newList = {}
    -- if testMode == 1 then
    --     for i = 1, #pathNodeList do
    --         table.insert(newList, Vector3(pathNodeList[i].y, pathNodeList[i].x, pathNodeList[i].z))
    --     end
    -- else
    --     for i = 1, #pathNodeList do
    --         table.insert(newList, Vector3(pathNodeList[i].x * 1080 / 1920, pathNodeList[i].y * 1920 / 1080, pathNodeList[i].z))
    --     end
    -- end
    
    -- if #pathNodeList > 2 then
    --     self.pathType = 1
    --     self:CreateCurvePath(pathNodeList)
    -- else
    --     self.pathType = 2
    --     self:CreateLinearPath(pathNodeList)
    -- end
    
    if #pathNodeList > 2 then
        self.pathType = 1
        self:CreateCurvePath(pathNodeList)
    else
        self.pathType = 2
        self:CreateLinearPath(pathNodeList)
    end
    self.lastPosition = Vector3.zero
end

function FishPath:CreateCurvePath(pathNodeList)
    if SeatHelper.IsServerSeatNegative() then
        pathNodeList = ReversePath(pathNodeList)
    end
    self.points = {}
    table.insert(self.points, pathNodeList[1])
    for i = 1, #pathNodeList do
        table.insert(self.points, pathNodeList[i])
    end
    table.insert(self.points, pathNodeList[#pathNodeList])
end

function FishPath:CreateLinearPath(pathNodeList)
    if SeatHelper.IsServerSeatNegative() then
        pathNodeList = ReversePath(pathNodeList)
    end
    self.points = {}
    for i = 1, #pathNodeList do
        table.insert(self.points, pathNodeList[i])
    end
end

function FishPath:GetData(time)
    if self.pathType == 1 then
        self.data.position = GetCurveInterPoint(self.points, time)
    else
        self.data.position = GetLinearInterPoint(self.points, time)
    end
    if self.lastPosition == Vector3.zero then self.lastPosition = self.data.position end
    self.data.rotation = CalculateRotation(self.lastPosition, self.data.position)
    self.data.direction = self.lastPosition.x <= self.data.position.x and 1 or -1
    self.lastPosition = self.data.position
    return self.data
end

function ReversePath(pathNodeList)
    for k, v in ipairs(pathNodeList) do
        v.x = v.x * -1
        v.y = v.y * -1
    end
    return pathNodeList
end

function GetCurveInterPoint(points, time)
    local numSections = #points - 3
    local cur = math.min(math.floor(time * numSections), numSections - 1)
    local u = time * numSections - cur
    local a = points[cur + 1]
    local b = points[cur + 2]
    local c = points[cur + 3]
    local d = points[cur + 4]
    return Vector3(
        CalculatePosition(a.x, b.x, c.x, d.x, u),
        CalculatePosition(a.y, b.y, c.y, d.y, u),
        CalculatePosition(a.z, b.z, c.z, d.z, u)
)
end

function GetLinearInterPoint(points, time)
    return Vector3.Lerp(Vector3())
end

function CalculatePosition(a, b, c, d, u)
    return 1.2 * 0.5 * ((-a + 3 * b - 3 * c + d) * (u * u * u) + (2 * a - 5 * b + 4 * c - d) * (u * u) + (-a + c) * u + 2 * b)
end

function CalculateRotation(preP, curP)
    -- if LocalDefines.FishViewAngle == 1 then
    --     local from = Vector3.forward
    --     local to = Vector3(curP.x, curP.y, curP.z - preP.z) - Vector3(preP.x, preP.y, 0)
    --     if to == Vector3.zero then
    --         return Quaternion.Euler(0, 0, 0)
    --     else
    --         return Quaternion.LookRotation(to)
    --     end
    -- else
    --     local vec1 = Vector3.up
    --     local vec2 = Vector3(curP.x - preP.x, curP.y - preP.y, 0)
    --     local angle = Vector3.Angle(vec1, vec2)
    --     local normal = Vector3.Cross(vec1, vec2)
    --     local dir
    --     if Vector3.Dot(normal, Vector3.forward) > 0 then
    --         dir = 1
    --     elseif Vector3.Dot(normal, Vector3.forward)<0 then
    --         dir = -1
    --     else
    --         dir = 0
    --     end
    --     angle = angle * dir
    --     local quat = Quaternion.AngleAxis(angle, Vector3.forward) * Quaternion.Euler(-90, 90, -90)
    --     quat = Quaternion.AngleAxis(40, Vector3.right) * quat
    --     return quat
    -- end
    local vec = Vector2(curP.x - preP.x, curP.y - preP.y)
    local rot
    if vec.x == 0 then
        if vec.y > 0 then rot = 90
        elseif vec.y == 0 then rot = 0
        else rot = -90 end
    else
        rot = math.deg(math.atan(vec.y, vec.x))
    end
    return rot
end
