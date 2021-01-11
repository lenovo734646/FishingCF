require "LuaUtil/LuaRequires"

local UIParent
local OnGameToHallMap
local ProtocolHelper = require("JBPROTO.ProtocolHelper")

function Main()
    log("PortraitFish Lua Main")
    
    local bundleMgr = Context.Game.BundleMgr
    local bundleName = bundleMgr:GetAssetBundleName("Assets/Scenes/MainScene.unity")
    bundleMgr:LoadAssetBundle(bundleName)
    SceneManager.LoadScene("MainScene")
    
    OnGameToHallMap = {}
    LoadConfig()
    MessageCenter.instance = MessageCenter()
    ModuleManager.instance = ModuleManager()
    PoolManager.instance = PoolManager()
    NetController.instance = NetController()
end

function LoadConfig()
    if SysDefines.IsInnerGameLoadFromNet == true then
        TLanguageErrcode = json.decode(UnityHelper.LoadConfigByTableName("LanguageErrcode"))
        TPathName = json.decode(UnityHelper.LoadConfigByTableName("PathName"))
        TPathWayPoints = json.decode(UnityHelper.LoadConfigByTableName("PathWayPoints"))
        TFish = json.decode(UnityHelper.LoadConfigByTableName("CFFish2D"))
        TFishGlobal = json.decode(UnityHelper.LoadConfigByTableName("CFFish2DGlobal"))
        TFishGun = json.decode(UnityHelper.LoadConfigByTableName("CFFish2DGun"))
        TFishGunValue = json.decode(UnityHelper.LoadConfigByTableName("CFFish2DGunValue"))
        TFishRoom = json.decode(UnityHelper.LoadConfigByTableName("CFFish2DRoom"))
        TFishFuncEffect = json.decode(UnityHelper.LoadConfigByTableName("FishFunctionalEffect"))
    else
        TLanguageErrcode = require "Table.LanguageErrcode"
        TPathName = require "Table.PathName"
        TPathWayPoints = require "Table.PathWayPoints"
        TFish = require "Table.CFFish2D"
        TFishGlobal = require "Table.CFFish2DGlobal"
        TFishGun = require "Table.CFFish2DGun"
        TFishGunValue = require "Table.CFFish2DGunValue"
        TFishRoom = require "Table.CFFish2DRoom"
        TFishFuncEffect = require "Table.FishFunctionalEffect"
    end
end

function OnSceneLoaded(scene, mode)
    if scene.name == "MainScene" then
        if AppRoot:IsRunInHall() then
            UIParent = GameController.Instance.UIParent
        else
            local canvasPrefab = Loader:LoadAsset("Assets/Resources/UIPrefab/Canvas.prefab", typeof(GameObject))
            local Canvas = Instantiate(canvasPrefab)
            UIParent = Canvas.transform:Find("UIParent")
        end
        local prefab = Loader:LoadAsset(Path_UI .. "FishRoot.prefab", typeof(GameObject))
        local root = Instantiate(prefab)
        root.transform:SetParent(UIParent, false)
        FishRoomController.instance = FishRoomController(LuaClass(root.gameObject))
        PoolManager.instance = PoolManager(root.transform:Find("PoolNode"))
        FishUIController.instance = FishUIController(LuaClass(root.transform:Find("UI").gameObject))
        FishUIController.instance:CreateRoomUI()
    end
end 

function GameToHall()
    if AppRoot:IsRunInHall() then
        if #OnGameToHallMap > 0 then
            for k, v in pairs(OnGameToHallMap) do
                v()
            end
        end
        AppRoot:GameToHall()
    else
        Application.Quit()
    end
end

function AddGameToHallListener(func)
    table.insert(OnGameToHallMap, func)
end

function OnReceiveNetData(br)
    ProtocolHelper.onReceiveNetData(br)
end

function OnReceiveCSharpData(msg)
    local CSSharpMsgType = msg.dicDatas:get_Item("MsgType")
    MessageCenter.instance:SendMessage(CSSharpMsgType, msg.dicDatas)
end

function PlayMusic(name)
    local bgm = Loader:LoadAsset(string.format("%s%s.ogg", Path_Audios, name), typeof(AudioClip))
    AudioManager:PlayMusic(bgm)
end

function PlaySound(name, loop, volumn)
    loop = loop ~= nil and loop or false
    volumn = volumn ~= nil and volumn or 1
    local sound = Loader:LoadAsset(string.format("%s%s.ogg", Path_Audios, name), typeof(AudioClip))
    AudioManager:PlaySoundEff2D(sound, loop, volumn)
end

function StopSound(name)
    AudioManager:StopSoundEff(name)
end

Main()
