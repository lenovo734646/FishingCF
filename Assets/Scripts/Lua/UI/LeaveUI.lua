LeaveUI = class(LuaBase,{})

function LeaveUI:__init()
    local btnConfirm = self:get("MessageLeaveFish/btnOK")
    UGUIClickLuaBehaviour.Bind(btnConfirm.gameObject, functional.bind1(self.onBtnConfirm, self))
    local btnCancel = self:get("MessageLeaveFish/btnRelease")
    UGUIClickLuaBehaviour.Bind(btnCancel.gameObject, functional.bind1(self.onBtnCancel, self))
end

function LeaveUI:onBtnConfirm()
    FishRoomController.instance:LeaveRoom()
    FishUIController.instance:CloseLeaveUI()
end

function LeaveUI:onBtnCancel()
    FishUIController.instance:CloseLeaveUI()
end