FishHandBookUI = class(LuaBase, {})

function FishHandBookUI:__init()
    self.btnClose = self:get("Help/Close")
    UGUIClickLuaBehaviour.Bind(self.btnClose.gameObject, functional.bind1(self.onBtnClose, self))

    self.contentT = self:get("Help/ScrollView/Viewport/Content")

    self:Init()
end


function FishHandBookUI:onBtnClose()
    FishUIController.instance:CloseFishBookUI()
end

function FishHandBookUI:Init()
    local infos = {}
    for k, v in pairs(TFish) do
        if v.PicType == 1 then
            table.insert(infos,v)
        end
        if (v.Id < 29 and v.Id%2 == 0) or v.Id == 29 then
            local prefab = Loader:LoadAsset(Path_UIControl .."HelpItem_2.prefab", typeof(GameObject))
            local doubleItem = PoolManager.instance:Spawn({
                prefab = prefab,
                parent = self.contentT.transform,
                script = FishHandItemDoubule,
            })
            doubleItem:Init(infos)
            infos = {}
        end
        if v.PicType == 3 then
            local prefab = Loader:LoadAsset(Path_UIControl .."HelpItem_1.prefab", typeof(GameObject))
            local bigItem = PoolManager.instance:Spawn({
                prefab = prefab,
                parent = self.contentT.transform,
                script = FishHandItemBig,
            })
            bigItem:Init(v)
        end
    end
end

FishHandItemDoubule = class(LuaBase,{})
function FishHandItemDoubule:__init()
    
end
function FishHandItemDoubule:Init(fishInfos)
    for i = 1, #fishInfos do
        local item = self:get("HelpItemLittle_"..i)
        item.gameObject:SetActive(true)
        local ItemScript = FishHandItemLittle(LuaClass(item.gameObject))
        ItemScript:Init(fishInfos[i])
    end
end


FishHandItemBig = class(LuaBase, {})

function FishHandItemBig:__init()
    self.icon = self:get("Icon","Image")
    self.name = self:get("Name","Text")
    self.content = self:get("Text","Text")
end

function FishHandItemBig:Init(fishInfo)
    self.icon.Image.sprite = Loader:LoadAsset(Path_Res.."UI/HandBook/"..fishInfo.FishIcon..".prefab",typeof(GameObject)):GetComponent("Image").sprite
    self.name.Text.text = fishInfo.Name
    --self.content.Text.text = fishInfo.Content
end


FishHandItemLittle = class(LuaBase, {})

function FishHandItemLittle:__init()
    self.icon = self:get("Icon","Image")
    self.bet = self:get("Font","Text")
end

function FishHandItemLittle:Init(fishInfo)
    self.icon.Image.sprite = Loader:LoadAsset(Path_Res.."UI/HandBook/"..fishInfo.FishIcon..".prefab",typeof(GameObject)):GetComponent("Image").sprite
    self.bet.Text.text = fishInfo.ShowMultiple.."倍"
end