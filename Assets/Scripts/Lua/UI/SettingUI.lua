SettingUI = class(LuaBase,{})

function SettingUI:__init()
    self.btnClose = self:get("Set/Close")
    UGUIClickLuaBehaviour.Bind(self.btnClose.gameObject, functional.bind1(self.onBtnClose, self))
    
    self.togMusic = self:get("Set/Music/Toggle")
    UGUIClickLuaBehaviour.Bind(self.togMusic.gameObject, functional.bind1(self.onToggleMusic, self))
    self.togSound = self:get("Set/Sound/Toggle")
    UGUIClickLuaBehaviour.Bind(self.togSound.gameObject, functional.bind1(self.onToggleSound, self))

    self.isPlayMusic = AudioManager:GetBGMVolumScale() ~= 0
    self.isPlaySound = AudioManager:GetEFFVolumScale() ~= 0

    self:SetMusicUI()
    self:SetSoundUI()
end

function SettingUI:SetMusicUI()
    self:get("Set/Music/Toggle/s_1").gameObject:SetActive(not self.isPlayMusic)
    self:get("Set/Music/Toggle/s_2").gameObject:SetActive(self.isPlayMusic)
end

function SettingUI:SetSoundUI()
    self:get("Set/Sound/Toggle/s_1").gameObject:SetActive(not self.isPlaySound)
    self:get("Set/Sound/Toggle/s_2").gameObject:SetActive(self.isPlaySound)
end

function SettingUI:onBtnClose()
    FishUIController.instance:CloseSettingUI()
end

function SettingUI:onToggleMusic()
    if self.isPlayMusic then
        self.isPlayMusic = false
    else
        self.isPlayMusic = true
    end
    AudioManager:SetBGMVolumScale(self.isPlayMusic and 1 or 0)
    self:SetMusicUI()
end

function SettingUI:onToggleSound()
    if self.isPlaySound then
        self.isPlaySound = false
    else
        self.isPlaySound = true
    end
    AudioManager:SetEFFVolumScale(self.isPlaySound and 1 or 0)
    self:SetSoundUI()
end
