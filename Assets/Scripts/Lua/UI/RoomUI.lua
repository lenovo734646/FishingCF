RoomUI = class(LuaBase, {})

local player = GameController.Instance.Player

function RoomUI:__init()
    self.Module = ModuleManager.instance:Get("MainModule")

    for i = 0, 2 do
        local roomObj = self:get("Room/Room_0" .. i).gameObject
        UGUIClickLuaBehaviour.Bind(roomObj, functional.bind2(self.OnRoomClick, self, i + 1))
        local textRate = self:get("Room/Room_0" .. i .. "/RoomText", "Text")
        local config = TFishRoom[i + 1]
        textRate.Text.text = 
            UnityHelper.NumberFormat(config.MinGun) .. "-" .. 
            UnityHelper.NumberFormat(config.MaxGun)
    end

    local btnQuit = self:get("Room/TopUI/btnClose")
    UGUIClickLuaBehaviour.Bind(btnQuit.gameObject, functional.bind1(self.OnBtnQuit, self))

    PlayMusic("BGM_Login")
end

function RoomUI:OnRoomClick(index)
    PlaySound("SE_click")
    local config = TFishRoom[index]
    local hintMsg = ""
    if config.LimitMinCurrency > player.Currency then
        hintMsg = string.format("进入该房间需要金币%s", UnityHelper.NumberFormat(config.LimitMinCurrency))
    end
    if not string.isNullOrEmpty(hintMsg) then
        --UIManager.Instance:OpenMessageBoxUI(hintMsg)
        GameController.Instance:CreateHintMessage(hintMsg)
    else
        LocalDefines.RoomConfigID = config.Id
        NetController.instance:SendEnterSiteReq(LocalDefines.SiteId)
    end
end

function RoomUI:OnBtnQuit()
    GameToHall()
end
