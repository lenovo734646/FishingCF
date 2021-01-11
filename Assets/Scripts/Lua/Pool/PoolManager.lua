PoolManager = class(nil, {
    tPool,
    parent,
})

function PoolManager:__init(poolNode)
    self.tPool = {}
    self.parent = poolNode ~= nil and poolNode or Instantiate(GameObject("PoolNode"))
    CS.UnityEngine.Object.DontDestroyOnLoad(self.parent.gameObject)
end

function PoolManager:Spawn(param)
    local prefab = param.prefab
    local pos = param.position ~= nil and param.position or Vector3.zero
    local rot = param.rotation ~= nil and param.rotation or Quaternion.identity
    local parent = param.parent ~= nil and param.parent or self.parent
    local script = param.script
    
    local pool
    for k, v in pairs(self.tPool) do
        if v.prefab == prefab then
            pool = v
        end
    end
    if pool == nil then
        pool = Pool(prefab)
        table.insert(self.tPool, pool)
    end
    return pool:Spawn(pos, rot, parent, script)
end

function PoolManager:Unspawn(obj)
    for k, v in pairs(self.tPool) do
        if v:Unspawn(obj) then return end
    end
    Destroy(obj.gameObject)
end

function PoolManager:ClearAll()
    for k, v in pairs(self.tPool) do
        v:Clear()
    end
end

function PoolManager:OnDestroy()
    for k, v in pairs(self.tPool) do
        v:Clear()
    end
    if self.parent.gameObject.name == "PoolNode" then
        Destroy(self.parent.gameObject)
    end
end

function PoolManager:GetOPMTransform()
    return self.parent.transform
end



Pool = class(nil, {
    prefab,
    tActive,
    tInactive,
    max
})

function Pool:__init(_prefab, max)
    self.prefab = _prefab
    self.max = max == nil and 1000 or max
    self.tActive = {}
    self.tInactive = {}
end

function Pool:Spawn(pos, rot, parent, script)
    local poolObj
    if #self.tInactive == 0 then
        local obj = Instantiate(self.prefab, pos, rot)
        if script then
            poolObj = script(LuaClass(obj))
        else
            poolObj = obj
        end
    else
        poolObj = self.tInactive[1]
        table.remove(self.tInactive, 1)
    end
    poolObj.transform:SetParent(parent, false)
    poolObj.transform.localScale = Vector3.one
    poolObj.transform.localPosition = pos
    poolObj.transform.localRotation = rot
    poolObj.gameObject:SetActive(true)
    table.insert(self.tActive, poolObj)
    
    return poolObj
end

function Pool:Unspawn(poolObj)
    if table.contains(self.tActive, poolObj) then
        poolObj.gameObject:SetActive(false)
        table.insert(self.tInactive, poolObj)
        table.removebyvalue(self.tActive, poolObj)
        poolObj.transform:SetParent(PoolManager.instance:GetOPMTransform())
        return true
    else
        return false
    end
end

function Pool:MachObjectCount(count)
    if count > self.max then return end
    
    local currentCount = #self.tActive + #self.tInactive
    for i = currentCount, count do
        local obj = Instantiate(self.prefab)
        obj.transform:SetParent(PoolManager.instance:GetOPMTransform())
        obj.gameObject:SetActive(false)
        table.insert(self.tInactive, obj)
    end
end

function Pool:Clear()
    for k, v in pairs(self.tActive) do
        Destroy(v.gameObject)
    end
    for k, v in pairs(self.tInactive) do
        Destroy(v.gameObject)
    end
    self.tActive = {}
    self.tInactive = {}
end
