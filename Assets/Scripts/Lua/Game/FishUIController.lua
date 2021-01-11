FishUIController = class(LuaBase, {})

function FishUIController:__init()

end

function FishUIController:CreateLoadingUI()
    if self.loadUI == nil then
        local prefab = Loader:LoadAsset(Path_UI .. "LoadingUI.prefab", typeof(GameObject))
        self.loadUI = Instantiate(prefab)
        self.loadUI.transform:SetParent(self.transform, false)
    end
    self.loadUI.gameObject:SetActive(true)
end

function FishUIController:HideLoadingUI()
    --self.loadUI.gameObject:SetActive(false)
end

function FishUIController:CreateRoomUI()
    local prefab = Loader:LoadAsset(Path_UI .. "RoomUI.prefab", typeof(GameObject))
    local obj = Instantiate(prefab)
    obj.transform:SetParent(self.transform, false)
    self.roomUI = RoomUI(LuaClass(obj))
end

function FishUIController:CloseRoomUI()
    Destroy(self.roomUI.gameObject)
    self.roomUI = nil
end

function FishUIController:CreateGameUI()
    local prefab = Loader:LoadAsset(Path_UI .. "GameUI.prefab", typeof(GameObject))
    local obj = Instantiate(prefab)
    obj.transform:SetParent(self.transform, false)
    self.gameUI = GameUI(LuaClass(obj))
end

function FishUIController:CloseGameUI()
    self.gameUI:onLeaveFishRoom()
    Destroy(self.gameUI.gameObject)
    self.gameUI = nil
end

function FishUIController:CreateLeaveUI()
    local prefab = Loader:LoadAsset(Path_UI .. "MessageLeaveFishUI.prefab", typeof(GameObject))
    local obj = Instantiate(prefab)
    obj.transform:SetParent(self.transform, false)
    self.leaveUI = LeaveUI(LuaClass(obj))
end

function FishUIController:CloseLeaveUI()
    Destroy(self.leaveUI.gameObject)
    self.leaveUI = nil
end

function FishUIController:CreateSettingUI()
    local prefab = Loader:LoadAsset(Path_UI .. "SetUI.prefab", typeof(GameObject))
    local obj = Instantiate(prefab)
    obj.transform:SetParent(self.transform, false)
    self.settingUI = SettingUI(LuaClass(obj))
end

function FishUIController:CloseSettingUI()
    Destroy(self.settingUI.gameObject)
    self.settingUI = nil
end

function FishUIController:CreateFishBookUI()
    local prefab = Loader:LoadAsset(Path_UI .. "FishHandBookUI.prefab", typeof(GameObject))
    local obj = Instantiate(prefab)
    obj.transform:SetParent(self.transform, false)
    self.fishBookUI = FishHandBookUI(LuaClass(obj))
end

function FishUIController:CloseFishBookUI()
    Destroy(self.fishBookUI.gameObject)
    self.fishBookUI = nil
end

function FishUIController:CreateLaserGunCome()
    local prefab = Loader:LoadAsset(Path_UI .. "laserGunUI.prefab", typeof(GameObject))
    local obj = PoolManager.instance:Spawn({
        prefab = prefab,
        parent = self.transform,
        script = LiveEff,
    })
    obj:Reset(2)
    PlaySound("laserUI")
end

function FishUIController:OnEnterFishRoom()
    --self:CreateLoadingUI()
    self:CloseRoomUI()
    self:CreateGameUI()
    local timer = Timer(function()
        self:HideLoadingUI()
    end, 2, 1)
    timer:Start()
end

function FishUIController:OnLeaveFishRoom()
    --self:CreateLoadingUI()
    self:CreateRoomUI()
    self:CloseGameUI()
    local timer = Timer(function()
        self:HideLoadingUI()
    end, 2, 1)
    timer:Start()
    AudioManager:StopAllSoudEff()
end
