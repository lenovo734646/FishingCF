BulletScript = class(LuaBase, {})

function BulletScript:__init(bulletInfo)
    self.thisT = self.transform
    self.isDead = false
    
    self.Module = ModuleManager.instance:Get("FishModule")
    
    UpdateLuaBehaviour.Bind(self.gameObject, self)
    ColliderLuaBehaviour.Bind(self.gameObject, self)
end

function BulletScript:Reset()
    self.isDead = false
end

function BulletScript:Update()
    if not self.isDead then
        self:RefreshPosition()
    end
end

function BulletScript:OnTriggerEnter(collider)
    if collider:CompareTag("fish") then
        local id = tonumber(collider.gameObject.name)
        local fish = FishRoomController.instance.FishSpawn:FindFishById(id)
        if fish and fish:IsValid() then
            local m = self.Module:GetBulletById(self.BulletId)
            if m then
                if m.LockFishId == 0 or m.LockFishId == fish.FishID then
                    -- fish:Hit(m.SnapshotInfo.IsPlayerSelf)
                    fish:Hit(true)
                    FishRoomController.instance:ProcessHitLogic(self.BulletId, fish.FishID)
                end
            end
        end
    end
end

function BulletScript:RefreshPosition()
    if self.Module then
        local m = self.Module:GetBulletById(self.BulletId)
        if m then
            local snapshot = m.SnapshotInfo
            self.thisT.localPosition = Vector3(snapshot.x, snapshot.y, 0)
            self.thisT.localEulerAngles = Vector3(0, 0, snapshot.rotation - 90)
        end
    end
end

function BulletScript:RemoveBullet()
    self.isDead = true
    PoolManager.instance:Unspawn(self)
end

function BulletScript:CheckOutOfScreen()
    local pos = GameController.Instance.MainCamera:WorldToScreenPoint(self.transform.position)
    return CS.UnityEngine.Screen.safeArea:Contains(pos)
end
