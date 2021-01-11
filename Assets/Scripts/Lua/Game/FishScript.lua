FishScript = class(LuaBase, {
    FishID,
    ConfigID,

})

function FishScript:__init()
    self.module = ModuleManager.instance:Get("FishingModule")
    
    self.thisT = self.transform
    self.rotateT = self:get("rotate")
    self.spineAnims = {}
    local spines = self.thisT:GetComponentsInChildren(typeof(SkeletonGraphic))
    for i = 0, spines.Length - 1 do
        table.insert(self.spineAnims, spines[i])
    end

    self.colliders = {}
    local cols = self.thisT:GetComponentsInChildren(typeof(Collider))
    for i = 0, cols.Length - 1 do
        table.insert(self.colliders, cols[i])
    end
    
    -- self.materials = {}
    -- local renderers = self.thisT:GetComponentsInChildren(typeof(Renderer))
    -- for i = 0, renderers.Length - 1 do
    --     table.insert(self.materials, renderers[i].material)
    -- end
    
    -- self.dotweens = {}
    -- local dot = self.thisT:GetComponents(typeof(CS.DG.Tweening.DOTweenAnimation))
    -- for i = 0, dot.Length - 1 do
    --     table.insert(self.dotweens, dot[i])
    -- end
    
    self.hitAnimTime = 0
end

function FishScript:Reset()
    self.isDead = false
    self.isSuspended = false
    self.isFreezing = false
    
    self.fishConfig = TFish[self.ConfigID]
    -- self.hitAnimForbid = false
    -- self.deadAnimForbid = false
    -- self.idleAnimForbid = false
    -- for k, v in pairs(self.fishConfig.AnimControl) do
    --     if v == 2 then
    --         self.hitAnimForbid = true
    --     end
    --     if v == 3 then
    --         self.deadAnimForbid = true
    --     end
    --     if v == 4 then
    --         self.idleAnimForbid = true
    --     end
    -- end
    
    --self.thisT.localScale = Vector3.one * 100
    --self.lastZ = -1000

    self.thisT.localScale = Vector3.one
    
    for k, v in pairs(self.colliders) do
        v.gameObject.name = self.FishID
    end
    
    for k, v in pairs(self.spineAnims) do
        v.color = Color.white
    end
end

function FishScript:OnUpdate()
    if self:IsValid() then
        self:RefreshPosition()
    end
end

function FishScript:RefreshPosition()
    local module = ModuleManager.instance:Get("FishModule")
    local roomFish = module:GetFishById(self.FishID)
    if roomFish then
        local snapshot = roomFish.SnapshotInfo
        self:CheckRateState(snapshot.timeRate, snapshot.timeRateReason)

        self.transform.localPosition = Vector3(snapshot.position.x, snapshot.position.y, snapshot.position.z)
        self.transform.localEulerAngles = Vector3(0,0,snapshot.rotation)
    --self.rotateT.transform.rotation = Quaternion(snapshot.rotation.x, snapshot.rotation.y, snapshot.rotation.z, snapshot.rotation.w)
    end
end

local hitAnimCd = 2
function FishScript:Hit(isPlayer)
    PlaySound("SE_hit")
    if isPlayer then
        for k,v in pairs(self.spineAnims) do
            v.color = Color.red
        end
    end

    local timer = Timer(function()
        for k, v in pairs(self.spineAnims) do
            v.color = Color.white
        end
    end, 0.2, 1)
    timer:Start()

    -- if isPlayer then
    --     for k, v in pairs(self.materials) do
    --         v:SetFloat("_Hit", 1)
    --     end
    -- end

    
    -- if Time.time - self.hitAnimTime > hitAnimCd then
    --     for k, v in pairs(self.animators) do
    --         v:SetTrigger("hit")
    --     end
    --     self.hitAnimTime = Time.time
    -- end
end

function FishScript:CheckRateState(timeRate, reason)
    if timeRate == 1 then
        if self.isFreezing then self:CastFrozenEffect(false) end
    else
        if reason == 1 then
            self:CastFrozenEffect(true)
        end
    end
end

function FishScript:SetDead()
    if self.isDead then return end
    self.isDead = true
end

function FishScript:IsValid()
    return not self.isDead and not self.isSuspended and self.gameObject.activeSelf
end

function FishScript:RemoveFish(isPlayAnim, delay, reason)
    if self.isSuspended then return end
    local play = false
    if not self.deadAnimForbid and isPlayAnim then play = true end
    self:FinalRemove(play, delay, reason)
end

function FishScript:FinalRemove(isPlayAnim, delay, reason)
    self.isSuspended = true
    self.isDead = true
    
    if reason ~= -1 then
        local path
        if self.fishConfig.FishType == 1 then
            path = Path_Effect.."Fishdie_01.prefab"
        elseif self.fishConfig.FishType == 2 then
            path = Path_Effect.."Fishdie_02.prefab"
        elseif self.fishConfig.FishType == 3 or self.fishConfig.FishType == 6 then
            path = Path_Effect.."Fishdie_03.prefab"
        end
        
        if path then
            local prefab = Loader:LoadAsset(path, typeof(GameObject))
            local effObj = PoolManager.instance:Spawn({
                prefab = prefab,
                parent = FishUIController.instance.gameUI.EffectT.transform,
                script = LiveEff,
            })
            local pos = FishUIController.instance.gameUI.EffectT.transform:InverseTransformPoint(self.transform.position)
            effObj.transform.localPosition = Vector3(pos.x, pos.y, 0)
            effObj:Reset(delay)
        end
    end
    
    local unspawnTimer = Timer(functional.bind1(self.UnspawnFish, self), delay, 1)
    unspawnTimer:Start()
end

function FishScript:UnspawnFish()
    PoolManager.instance:Unspawn(self)
end

function FishScript:CheckOutOfScreen()
    local pos = GameController.Instance.MainCamera:WorldToScreenPoint(self.transform.position)
    return CS.UnityEngine.Screen.safeArea:Contains(pos)
end
