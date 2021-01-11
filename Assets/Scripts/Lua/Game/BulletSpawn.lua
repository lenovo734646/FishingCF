BulletSpawn = class(LuaBase, {})

function BulletSpawn:__init()
    MessageCenter.instance:AddListener(MsgType.LUA_ROOM_LEAVE_FISH_ROOM, self.onLeaveFishRoom, self)
    MessageCenter.instance:AddListener(MsgType.LUA_ROOM_BULLET_CREATE, self.bulletCreate, self)
    MessageCenter.instance:AddListener(MsgType.LUA_ROOM_BULLET_DESTROY, self.bulletDestroy, self)
    MessageCenter.instance:AddListener(MsgType.LUA_ROOM_PENETRATE_CREATE_WEB, self.createPenetrateWeb, self)
    MessageCenter.instance:AddListener(MsgType.LUA_ROOM_BULLET_UPDATE_INFO, self.bulletUpdateInfo, self)
    
    self.thisT = self.transform
    self.bulletPrefabMap = {}
    self.webPrefabMap = {}
    self.bulletList = {}
    
    self.active = false
end

function BulletSpawn:onLeaveFishRoom()
    self.active = false
    for k, v in pairs(self.bulletList) do
        PoolManager.instance:Unspawn(v)
    end
    self.bulletList = {}
end

function BulletSpawn:Active()
    self.active = true
end

function BulletSpawn:FindBulletById(bulletId)
    for k, v in pairs(self.bulletList) do
        if v.BulletId == bulletId then
            return v
        end
    end
end

function BulletSpawn:bulletCreate(msg)
    if not self.active then return end

    local m = msg.Bullet
    local p = msg.Player
    local path = Path_Res .. (m.Multiple > 1 and "Bullet/Bullet_Double" or "Bullet/Bullet_0")..p.CannoId .. ".prefab"
    local prefab = Loader:LoadAsset(path, typeof(GameObject))
    --print("bulletCreate  : " .. p.CannoId)
    --prefab.gameObject:GetComponent("Image").sprite = Loader:LoadAsset(Path_Res.."Bullet/Bullet_0"..p.CannoId..".prefab",typeof(GameObject)):GetComponent("Image").sprite
    local bulletScript = PoolManager.instance:Spawn({
        prefab = prefab,
        parent = self.thisT,
        script = BulletScript,
    })    
    bulletScript.transform.localScale = Vector3(0.55, 0.6, 1)
    bulletScript.BulletId = m.BulletId
    bulletScript.CannoId = p.CannoId
    bulletScript:Reset()
    bulletScript:RefreshPosition()
    table.insert(self.bulletList, bulletScript)
end

function BulletSpawn:bulletDestroy(msg)
    local m = msg.Bullet
    local isShowNet = msg.IsShowNet
    local bulletScript = table.find(self.bulletList, function(b)
        return b.BulletId == m.BulletId
    end)
    if bulletScript then
        if isShowNet then
            local effPos = bulletScript.transform.localPosition

            local path = Path_Res .. "Web/Web.prefab"
            local prefab = Loader:LoadAsset(path, typeof(GameObject))
            local web = PoolManager.instance:Spawn({
                prefab = prefab,
                parent = self.thisT,
                script = LiveEff,
            })
            web.transform.localPosition = effPos
            web:Reset(0.5)
        end
        bulletScript:RemoveBullet()
        table.removebyvalue(self.bulletList, bulletScript)
    end
end

function BulletSpawn:createPenetrateWeb(msg)
    local m = msg.Bullet
    local isShowNet = msg.IsShowNet
    local bulletScript = table.find(self.bulletList, function(b)
        return b.BulletId == m.BulletId
    end)
    if bulletScript then
        if isShowNet then
            local effPos = bulletScript.transform.localPosition
            local path = Path_Res .. "Web/CT_Web.prefab"
            local prefab = Loader:LoadAsset(path, typeof(GameObject))
            local web = PoolManager.instance:Spawn({
                prefab = prefab,
                parent = self.thisT,
                script = LiveEff,
            })
            web.transform.localPosition = effPos
            web.transform.localEulerAngles = Vector3(0, 0, bulletScript.transform.localEulerAngles.z)
            web:Reset(0.5)
        end
    end
end

function BulletSpawn:bulletUpdateInfo(msg)
    local roomBullet = msg.Bullet
    local newBulletId = msg.NewBulletId
    local bullet = table.find(self.bulletList, function(b)
        return b.BulletId == roomBullet.BulletId
    end)
    if bullet then
        bullet.BulletId = newBulletId
    end
end
