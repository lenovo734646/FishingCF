LiveEff = class(LuaBase, {})

function LiveEff:__init()
    UpdateLuaBehaviour.Bind(self.gameObject, self)

    self.active = false
end

function LiveEff:Reset(live)
    self.live = live
    self.active = true
end

function LiveEff:Update()
    if not self.active then return end
    if self.live <= 0 then return end
    local delta = Time.deltaTime
    self.live = self.live - delta
    if self.live <= 0 then
        self:Destroy()
    end
end

function LiveEff:Destroy()
    self.active = false
    PoolManager.instance:Unspawn(self)
end
