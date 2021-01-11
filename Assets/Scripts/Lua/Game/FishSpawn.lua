FishSpawn = class(LuaBase, {})

function FishSpawn:__init()
    MessageCenter.instance:AddListener(MsgType.LUA_ROOM_LEAVE_FISH_ROOM, self.onLeaveFishRoom, self)
    MessageCenter.instance:AddListener(MsgType.LUA_ROOM_FISH_CREATE, self.fishCreate, self)
    MessageCenter.instance:AddListener(MsgType.LUA_ROOM_FISH_DESTROY, self.fishDestroy, self)
    MessageCenter.instance:AddListener(MsgType.LUA_ROOM_REMOVE_FISH, self.fishRemove, self)
    MessageCenter.instance:AddListener(MsgType.LUA_ROOM_FUNCTIONAL_REMOVE_FISH, self.fishFunctionalRemove, self)
    
    self.Fishes = {}
    self.DicFishPrefab = {}
    
    self.active = false
end

function FishSpawn:onLeaveFishRoom()
    self.active = false
    for k, v in pairs(self.Fishes) do
        PoolManager.instance:Unspawn(v)
    end
    self.Fishes = {}
end

function FishSpawn:Active()
    self.active = true
end

function FishSpawn:OnUpdate()
    if not self.active then return end
    
    for k, v in pairs(self.Fishes) do
        v:OnUpdate()
    end
end

function FishSpawn:FindFishById(fishId)
    return table.find(self.Fishes, function(f)
        return f.FishID == fishId
    end)
end

function FishSpawn:FindFishByGameObject(go)
    return table.find(self.Fishes, function(f)
        return f.gameObject == go
    end)
end

function FishSpawn:FindFishByRadius(fishId, radius)
    local lst = {}
    local targetFish = self:FindFishById(fishId)
    if targetFish then
        local pos = Vector2(targetFish.transform.localPosition.x, targetFish.transform.localPosition.y)
        for k, v in pairs(self.Fishes) do
            local dis = Vector2.Distance(pos, Vector2(v.transform.localPosition.x, v.transform.localPosition.y))
            if dis <= radius then
                table.insert(lst, v.FishID)
            end
        end
    end
    return lst
end

function FishSpawn:fishCreate(msg)
    local fish = msg.Fish
    local prefab
    if self.DicFishPrefab[fish.ConfigID] ~= nil then
        if fish.IsMultipleGroup == 1 then
            local path = Path_Fish .. "Fish_special_1_".. (fish.ConfigID%10) .. ".prefab"
            prefab = Loader:LoadAsset(path, typeof(GameObject))
            self.DicFishPrefab[fish.ConfigID] = prefab
        else
            local t = table.find(TFish, function(t)
                return t.Id == fish.ConfigID
            end)
            local path = Path_Fish ..t.Model  .. ".prefab"
            prefab = Loader:LoadAsset(path, typeof(GameObject))
            self.DicFishPrefab[fish.ConfigID] = prefab
        end
    else
        local t = table.find(TFish, function(t)
            return t.Id == fish.ConfigID
        end)
        local path = Path_Fish .. t.Model .. ".prefab"
        if fish.IsMultipleGroup == 1 and (t.Id >=11 and t.Id <= 17) then
            path = Path_Fish .. "Fish_special_1_".. (t.Id%10) .. ".prefab"
        end
        prefab = Loader:LoadAsset(path, typeof(GameObject))
        self.DicFishPrefab[fish.ConfigID] = prefab
    end
    local fishScript = PoolManager.instance:Spawn({
        prefab = prefab,
        parent = self.transform,
        script = FishScript,
    })
    fishScript.FishID = fish.FishID
    fishScript.ConfigID = fish.ConfigID
    fishScript.FishConfig = TFish[fish.ConfigID]
    fishScript:Reset()
    fishScript:RefreshPosition()
    table.insert(self.Fishes, fishScript)
end

function FishSpawn:fishDestroy(msg)
    local roomFish = msg.Fish
    local fish = table.find(self.Fishes, function(f)
        return f.FishID == roomFish.FishID
    end)
    if fish then
        local p = msg.Player
        local reason = msg.Reason
        local isPlayer = p ~= nil
        fish:RemoveFish(isPlayer, isPlayer and 1.2 or 0, reason)
        table.removebyvalue(self.Fishes, fish)
    end
end

function FishSpawn:fishRemove(msg)
    local p = msg.Player
    if not p then return end
    local lst = msg.FishID
    if lst then
        for k, v in pairs(lst) do
            local fish = table.find(self.Fishes, function(f)
                return f.FishID == v.FishID
            end)
            if fish then
                fish:RemoveFish(true, 1.2, 0)
                table.removebyvalue(self.Fishes, fish)
            end
        end
    end
end

function FishSpawn:fishFunctionalRemove(msg)
    local funcFish = msg.FunctionalFish
    local reason = funcFish.Reason
    if reason == 1 then
        self:processFireFish(funcFish)
        PlaySound("firefish")
    elseif reason == 2 then
        self:processLightningFish(funcFish)
    elseif reason == 3 then
        self:processFishKing(funcFish)
    elseif reason == 4 then
        self:processBlackHoleFish(funcFish)
        PlaySound("Eff_heidong")
    end
end

function FishSpawn:processFireFish(funcFish)
    local frFish = table.find(self.Fishes, function(f)
        return f.FishID == funcFish.FuncFish.FishID
    end)
    frFish:RemoveFish(true, funcFish.DelayTime, funcFish.Reason)
    local prefab = Loader:LoadAsset(Path_Res .. "Effects/Eff_FireFish_die.prefab", typeof(GameObject))
    local eff = PoolManager.instance:Spawn({
        prefab = prefab,
        parent = FishUIController.instance.gameUI.EffectT.transform,
        script = LiveEff,
    })
    local pos = FishUIController.instance.gameUI.EffectT.transform:InverseTransformPoint(frFish.transform.position)
    eff.transform.localPosition = Vector3(pos.x, pos.y, 0)
    eff:Reset(3)
    local related = funcFish.RelatedFish
    for k, v in pairs(related) do
        local rlFish = table.find(self.Fishes, function(f)
            return f.FishID == v.FishID
        end)
        rlFish:RemoveFish(true, funcFish.DelayTime, funcFish.Reason)
    end
end

function FishSpawn:processLightningFish(funcFish)
    local lFish = table.find(self.Fishes, function(f)
        return f.FishID == funcFish.FuncFish.FishID
    end)
    lFish:RemoveFish(false, funcFish.DelayTime, funcFish.Reason)
    local pos = lFish.transform.position
    local prefab = Loader:LoadAsset(Path_Effect .. "Eff_shandianyudie_01.prefab", typeof(GameObject))
    local related = funcFish.RelatedFish
    for k, v in pairs(related) do
        local rlFish = table.find(self.Fishes, function(f)
            return f.FishID == v.FishID
        end)
        rlFish:RemoveFish(true, funcFish.DelayTime, funcFish.Reason)
        local targetPos = rlFish.transform.position
        local eff = PoolManager.instance:Spawn({
            prefab = prefab,
            parent = FishUIController.instance.gameUI.EffectT.transform,
            script = LightningChain,
        })
        local positions = {pos, targetPos}
        eff:Play(positions, funcFish.DelayTime)
    end
end

function FishSpawn:processFishKing(funcFish)
    local king = table.find(self.Fishes, function(f)
        return f.FishID == funcFish.FuncFish.FishID
    end)
    king:RemoveFish(true, funcFish.DelayTime, funcFish.Reason)
    local pos = FishUIController.instance.gameUI.EffectT.transform:InverseTransformPoint(king.transform.position)
    local prefab = Loader:LoadAsset(Path_Res .. "Effects/Eff_FishKing.prefab", typeof(GameObject))
    local related = funcFish.RelatedFish
    for k, v in pairs(related) do
        local rlFish = table.find(self.Fishes, function(f)
            return f.FishID == v.FishID
        end)
        rlFish:RemoveFish(true, funcFish.DelayTime, funcFish.Reason)
        local targetPos = rlFish.transform.position
        local eff = PoolManager.instance:Spawn({
            prefab = prefab,
            parent = FishUIController.instance.gameUI.EffectT.transform,
            script = LiveEff,
        })
        eff.transform.localPosition = pos
        eff.transform.DOMove(targetPos, funcFish.DelayTime)
        eff:Reset(funcFish.DelayTime)
    end
end

function FishSpawn:processBlackHoleFish(funcFish)
    local bhFish = table.find(self.Fishes, function(f)
        return f.FishID == funcFish.FuncFish.FishID
    end)
    bhFish:RemoveFish(false, 0, funcFish.Reason)
    local pos = FishUIController.instance.gameUI.EffectT.transform:InverseTransformPoint(bhFish.transform.position)
    local prefab = Loader:LoadAsset(Path_Res .. "Effects/Eff_heidong.prefab", typeof(GameObject))
    local eff = PoolManager.instance:Spawn({
        prefab = prefab,
        parent = FishUIController.instance.gameUI.EffectT.transform,
        script = LiveEff,
    })
    eff.transform.localPosition = Vector3(pos.x,pos.y,20000)
    eff:Reset(7)
    local related = funcFish.RelatedFish
    for k, v in pairs(related) do
        local rlFish = table.find(self.Fishes, function(f)
            return f.FishID == v.FishID
        end)
        rlFish:RemoveFish(false, funcFish.DelayTime, funcFish.Reason)
        local blackHoleMove = rlFish.gameObject:GetComponent("BlackHoleMove")
        if not blackHoleMove then
            blackHoleMove = rlFish.gameObject:AddComponent("BlackHoleMove")
        end
        blackHoleMove.TargetPos = eff.transform.localPosition
        blackHoleMove.enabled = true
    end
end



LightningChain = class(LuaBase, {})

function LightningChain:__init(...)
    UpdateLuaBehaviour.Bind(self.gameObject, self)
    
    self.lineRenderer = self.gameObject:GetComponent(typeof(CS.UnityEngine.LineRenderer))
    
    self.isActive = false
    self.detail = 3
    self.displacement = 3
end

function LightningChain:Update()
    if self.isActive then
        self.curTime = self.curTime + Time.deltaTime*1.5
    end
    if self.curTime > self.eachDuration then
        self.curTime = self.curTime - self.eachDuration
        if self.isActive then
            self.curIndex = self.curIndex + 1
        end
        if self.curIndex > table.count(self.positions) - 1 then
            if self.isActive then
                StartCoroutine(functional.bind1(self.close, self))
            end
        end
    end
    local index = self.curIndex >= table.count(self.positions) and self.curIndex - 1 or self.curIndex
    self.curTime = self.curIndex >= table.count(self.positions) and self.eachDuration or self.curTime
    local pos = Vector3.Lerp(self.positions[self.curIndex - 1], self.positions[index], self.curTime / self.eachDuration)
    local posList = {}
    table.insert(posList, self.positions[0])
    for i = 1, table.count(self.positions) do
        if i > index then break end
        local endPos
        if i < index then
            endPos = self.positions[i]
        else
            endPos = pos
        end
        self.linePosList = {}
        self:CollectLinePos(self.positions[i - 1], endPos, self.displacement)
        for j = 1, #self.linePosList do
            table.insert(posList, self.linePosList[j])
        end
        table.insert(posList, endPos)
    end
    self.lineRenderer.positionCount = #posList
    for i = 1, #posList do
        self.lineRenderer:SetPosition(i - 1, posList[i])
    end
end

function LightningChain:Play(positions, eachDuration)
    self.positions = {}
    for i = 1, #positions do
        self.positions[i - 1] = positions[i]
    end
    self.eachDuration = eachDuration
    self.isActive = true
    self.curIndex = 1
    self.curTime = 0
end

function LightningChain:close()
    self.isActive = false
    WaitForOneFrame()
    PoolManager.instance:Unspawn(self)
end

function LightningChain:CollectLinePos(startPos, destPos, displace)
    if displace < self.detail then
        table.insert(self.linePosList, startPos)
    else
        local midX = (startPos.x + destPos.x) / 2
        local midY = (startPos.y + destPos.y) / 2
        local midZ = (startPos.z + destPos.z) / 2
        midX = midX + (math.random() - 0.5) * displace
        midY = midY + (math.random() - 0.5) * displace
        midZ = midZ + (math.random() - 0.5) * displace
        local midPos = Vector3(midX, midY, midZ)
        
        self:CollectLinePos(startPos, midPos, displace / 2)
        self:CollectLinePos(midPos, destPos, displace / 2)
    end
end
