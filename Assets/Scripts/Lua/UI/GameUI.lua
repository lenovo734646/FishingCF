GameUI = class(LuaBase, {})

function GameUI:__init()
    UpdateLuaBehaviour.Bind(self.gameObject, self)
    
    MessageCenter.instance:AddListener(MsgType.LUA_ROOM_PLAYER_JOIN, self.onPlayerJoin, self)
    MessageCenter.instance:AddListener(MsgType.LUA_ROOM_PLAYER_LEAVE, self.onPlayerLeave, self)
    MessageCenter.instance:AddListener(MsgType.CLIENT_PLAYER_CURRENCY_CHANGED, self.onPlayerCurrencyChange, self)
    MessageCenter.instance:AddListener(MsgType.LUA_ROOM_AUTO_FIRE_CHANGE, self.onAutoChange, self)
    MessageCenter.instance:AddListener(MsgType.LUA_ROOM_LOCK_FIRE_CHANGE,self.onLockChange,self)
    MessageCenter.instance:AddListener(MsgType.LUA_ROOM_PENETRATE_FIRE_CHANGE, self.onPenetrateChange, self)
    MessageCenter.instance:AddListener(MsgType.LUA_ROOM_MULTIPLE_FIRE_CHANGE, self.onMultipleChange, self)
    MessageCenter.instance:AddListener(MsgType.LUA_ROOM_CLONE_FIRE_CHANGE, self.onCloneChange, self)
    MessageCenter.instance:AddListener(MsgType.LUA_ROOM_FISH_CREATE, self.onFishCreate, self)
    MessageCenter.instance:AddListener(MsgType.LUA_ROOM_FISH_DESTROY, self.onFishDestroy, self)
    MessageCenter.instance:AddListener(MsgType.LUA_ROOM_FISH_CREATE_GOLD, self.onFishCreateGold, self)
    MessageCenter.instance:AddListener(MsgType.LUA_ROOM_FISHTIDE_NTF,self.onFishTideNtf,self)
    
    -- self.imgHead = self:get("TopUI/Player/Head/head_00", "Image")
    -- self.txtName = self:get("TopUI/Player/Name", "Text")
    -- self.txtGold = self:get("TopUI/Player/Gold/Text", "Text")
    
    local btnFishBook = self:get("LeftUI/Group/btnExplain")
    UGUIClickLuaBehaviour.Bind(btnFishBook.gameObject, functional.bind1(self.onBtnFishBook, self))
    local btnSet = self:get("LeftUI/Group/btnSet")
    UGUIClickLuaBehaviour.Bind(btnSet.gameObject, functional.bind1(self.onBtnSet, self))
    local btnClose = self:get("LeftUI/Group/btnClose")
    UGUIClickLuaBehaviour.Bind(btnClose.gameObject, functional.bind1(self.onBtnClose, self))
    local btnLeft = self:get("LeftUI/Arrows")
    UGUIClickLuaBehaviour.Bind(btnLeft.gameObject, functional.bind1(self.OnBtnLeft, self))

    self.togLock = self:get("DownUI/togLock","Toggle")
    UGUIClickLuaBehaviour.Bind(self.togLock.gameObject, functional.bind1(self.onTogLock, self))
    self.togAuto = self:get("DownUI/togAuto","Toggle")
    UGUIClickLuaBehaviour.Bind(self.togAuto.gameObject, functional.bind1(self.onTogAuto, self))
    
    local transPlayerShow = self:get("PlayerShow").transform
    local count = transPlayerShow.childCount
    self.awaits = {}
    self.playerCannons = {}
    for i = 1, count do
        local site = transPlayerShow:GetChild(i - 1)
        table.insert(self.awaits, site:GetChild(0))
        table.insert(self.playerCannons, PlayerCannon(LuaClass(site:GetChild(1).gameObject)))
    end
    
    self.DownT = self:get("down")
    self.UpT = self:get("up")
    self.TopT = self:get("top")
    self.EffectT = self:get("effect")
    self.LeftUI = self:get("LeftUI")
    self.LeftUI.transform:DOLocalMoveX(self.transform:GetComponent("RectTransform").rect.width*0.5*1.1,0)

    self.isShowLeftUI = false
    self.isAuto = false
    self.isLock = false
    
    self.awakeTime = TimeHelper.GetServerTimestamp()
    
    self.fishComeQueue = {}
    self.isShowFishComing = false
    
    PlayMusic("BGM_Room" .. LocalDefines.RoomConfigID)
end

function GameUI:onLeaveFishRoom()
    MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_PLAYER_JOIN, self.onPlayerJoin, self)
    MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_PLAYER_LEAVE, self.onPlayerLeave, self)
    MessageCenter.instance:RemoveListener(MsgType.CLIENT_PLAYER_CURRENCY_CHANGED, self.onPlayerCurrencyChange, self)
    MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_AUTO_FIRE_CHANGE, self.onAutoChange, self)
    MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_LOCK_FIRE_CHANGE,self.onLockChange,self)
    MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_PENETRATE_FIRE_CHANGE, self.onPenetrateChange, self)
    MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_MULTIPLE_FIRE_CHANGE, self.onMultipleChange, self)
    MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_CLONE_FIRE_CHANGE, self.onCloneChange, self)
    MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_FISH_CREATE, self.onFishCreate, self)
    MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_FISH_DESTROY, self.onFishDestroy, self)
    MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_FISH_CREATE_GOLD, self.onFishCreateGold, self)
    MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_FISHTIDE_NTF,self.onFishTideNtf,self)
    
    for k, v in pairs(self.playerCannons) do
        if v.gameObject.activeSelf then
            v:Clear()
        end
    end
end

function GameUI:Update()
    for k, v in pairs(self.playerCannons) do
        v:OnUpdate()
    end
end

function GameUI:OnBtnLeft()
    PlaySound("SE_click")
    if self.isShowLeftUI then
        self.LeftUI.transform:DOLocalMoveX(self.transform:GetComponent("RectTransform").rect.width*0.5*1.1,0.3)
        self.isShowLeftUI = false
    else
        self.LeftUI.transform:DOLocalMoveX(self.transform:GetComponent("RectTransform").rect.width*0.5*0.9,0.3)
        self.isShowLeftUI = true
    end
    --NetController.instance:SendFishTideForTestReq()
end

function GameUI:onBtnFishBook()
    PlaySound("SE_click")
    FishUIController.instance:CreateFishBookUI()
end

function GameUI:onBtnSet()
    PlaySound("SE_click")
    FishUIController.instance:CreateSettingUI()
end

function GameUI:onBtnClose()
    PlaySound("SE_click")
    -- FishUIController.instance:CreateLeaveUI()
    FishRoomController.instance:LeaveRoom()
end

function GameUI:onBtnFreeDraw()
-- body
end

function GameUI:onTogClone()
    self.isClone = self.togClone.Toggle.isOn
    MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_CLONE_FIRE_CHANGE, {
        Clone = self.isClone
    })
end



function GameUI:onCloneChange(msg)
    local clone = msg.Clone
    self.isClone = clone
    self.togClone.Toggle.isOn = clone
end

function GameUI:onTogMultiple()
    self.isMultiple = self.togMultiple.Toggle.isOn
    MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_MULTIPLE_FIRE_CHANGE, {
        Multiple = self.isMultiple
    })
end

function GameUI:onMultipleChange(msg)
-- body
end

function GameUI:onTogPenetrate()
    self.isPenetrate = self.togPenetrate.Toggle.isOn
    MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_PENETRATE_FIRE_CHANGE, {
        Penetrate = self.isPenetrate
    })
end

function GameUI:onPenetrateChange(msg)
    local penetrate = msg.Penetrate
    self.isPenetrate = penetrate
    self.togPenetrate.Toggle.isOn = penetrate
end

function GameUI:onTogAuto()
    PlaySound("SE_click")
    self.isAuto = self.togAuto.Toggle.isOn
    MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_AUTO_FIRE_CHANGE, {
        Auto = self.isAuto
    })
end

function GameUI:onTogLock()
    PlaySound("SE_click")
    self.isLock = self.togLock.Toggle.isOn
    MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_LOCK_FIRE_CHANGE, {
        Lock = self.isLock
    })
end

function GameUI:onAutoChange(msg)
    local auto = msg.Auto
    self.isAuto = auto
    self.togAuto.Toggle.isOn = auto
end

function GameUI:onLockChange(msg)
    local lock = msg.Lock
    self.isLock = lock
    self.togLock.Toggle.isOn = lock
end

function GameUI:onPlayerJoin(msg)
    PlaySound("SE_Getinroom")
    local player = msg.Player
    self:createPlayerByRoomPlayer(player)
    
    if player:IsPlayerSelf() then
        self.player = player
        -- self.imgHead.Image.sprite = Loader:LoadAsset(Path_Res .. "Module/Head/head_0" .. player.Head .. ".prefab", typeof(GameObject)):GetComponent("Image").sprite
        -- self.txtName.Text.text = player.NickName
        --self.txtGold.Text.text = player.Currency
    end
end

function GameUI:onPlayerLeave(msg)
    local player = msg.Player
    local seatIndex = player:GetClientSeat()
    self.playerCannons[seatIndex + 1]:PlayerLeave()
    self.awaits[seatIndex + 1].gameObject:SetActive(true)
end

function GameUI:createPlayerByRoomPlayer(player)
    local seatIndex = player:GetClientSeat()
    self.playerCannons[seatIndex + 1]:SetPlayer(player)
    self.awaits[seatIndex + 1].gameObject:SetActive(false)
end

function GameUI:onPlayerCurrencyChange(msg)
    local params = msg:get_Item("Params"):get_Item("0")
    local reason = params:get_Item("Reason")
    local delta = params:get_Item("Delta")
    if reason == 0 then
        self.player:DeltaCurrency(delta)
        self.playerCannons[self.player:GetClientSeat() + 1]:SetGoldValue(GameController.Instance.Player.Currency)
    end
end

function GameUI:onFishCreate(msg)
    local fish = msg.Fish
    local fishData = TFish[fish.ConfigID]
    if fish:GetRealTimeForBorn() > self.awakeTime then
        if fishData.FishType == 6 then
            self:playFishCome(fishData)
        end
    end
end

function GameUI:onFishDestroy(msg)
    local player = msg.Player
    if player then
        local fish = msg.Fish
        local deltaGold = msg.DeltaGold
        local pos = fish.SnapshotInfo.position
        local fishData = TFish[fish.ConfigID]
        local fishType = fishData.FishType
        if fishType > 2 then
            local shakePower = 0
            if fishType == 3 or fishType == 6 then shakePower = 2 end
            --GameController.Instance:ShakeCamera(shakePower)
        end
        if not string.isNullOrEmpty(fishData.DieVoice) then
            local volume = player:IsPlayerSelf() and 1 or 0.5
            PlaySound(fishData.DieVoice, false, volume)
        end
        if deltaGold ~= 0 then
            self:CreateImageGold(player, fishData.Multiple, pos, deltaGold)
            self:CreateTextGold(player, fishType, pos, deltaGold)
            local forceWheel = msg.ForceWheel
            local wheel = forceWheel ~= 0 and forceWheel or fishData.GoldWheel
            self:CreateBomb(player, wheel, deltaGold)
        end
        local playerCannon = self.playerCannons[player:GetClientSeat() + 1]
        if self.isLock and playerCannon:GetLockFish() ~= 0 and fish.FishID == playerCannon:GetLockFish() then
            playerCannon:RemoveLockFish()
        end
    end
end

function GameUI:onFishCreateGold(msg)
    local fish = msg.Fish
    local player = msg.Player
    local fishData = TFish[fish.ConfigID]
    local pos = fish.SnapshotInfo.position
    self:CreateImageGold(player, fishData.Multiple, pos, 0)
end

function GameUI:CreateImageGold(player, multiple, fishPos, deltaCurrency, quenceAction)
    local path = Path_Effect .. (player:IsPlayerSelf() and "Eff_gold_01" or "Eff_gold_02") .. ".prefab"
    if multiple < 80 then
        local row = 1
        local count = 2
        if multiple <= 10 then
            count = 2
        elseif multiple <= 30 then
            count = 4
        else
            row = 2
            count = 8
        end
        local disf = (count - 1) / 2
        for j = 0, row - 1 do
            local random = {}
            for i = 0, count - 1 do table.insert(random, i) end
            for i = 0, count - 1 do
                local prefab = Loader:LoadAsset(path, typeof(GameObject))
                local coin = PoolManager.instance:Spawn({
                    prefab = prefab,
                    parent = self.DownT.transform,
                    script = ImageCoinEff,
                })
                local pos = fishPos
                pos.z = 0
                pos.y = pos.y+10
                coin.transform.localScale = Vector3(0.7,0.7,1)
                coin.transform.localPosition = pos + (i - disf) * 50 * Vector3.right + j * Vector3.up * 50
                
                local r = math.random(1, #random)
                local delay = random[r]
                table.remove(random, r)
                if (j + 1) == row and (i + 1) == count then
                    coin:SetCoin(self.playerCannons[player:GetClientSeat() + 1].rotate.transform.position, delay * 0.1, function()
                        if quenceAction then quenceAction() end
                        if deltaCurrency ~= 0 then
                            player:DeltaCurrency(deltaCurrency)
                        end
                    end)
                else
                    coin:SetCoin(self.playerCannons[player:GetClientSeat() + 1].rotate.transform.position, delay * 0.1)
                end
            end
        end
    else
        local count = 15
        local radius = 300
        if multiple <= 300 then
            count = 15
        elseif multiple <= 500 then
            count = 20
        else
            count = 25
        end
        for i = 0, count - 1 do
            local theta = math.random() * 1.5 * math.pi
            local r = math.random(0, radius)
            local x = fishPos.x + math.sin(math.rad(theta)) * r
            local y = fishPos.y + math.cos(math.rad(theta)) * r
            local prefab = Loader:LoadAsset(path, typeof(GameObject))
            local coin = PoolManager.instance:Spawn({
                prefab = prefab,
                parent = self.DownT.transform,
                script = ImageCoinEff,
            })
            local pos = Vector3(x, y, 0)
            coin.transform.localPosition = pos
            if i + 1 == count then
                coin:SetCoin(self.playerCannons[player:GetClientSeat() + 1].rotate.transform.position, i * 0.1, function()
                    if quenceAction then quenceAction() end
                    if deltaCurrency ~= 0 then
                        player:DeltaCurrency(deltaCurrency)
                    end
                end)
            else
                coin:SetCoin(self.playerCannons[player:GetClientSeat() + 1].rotate.transform.position, i * 0.1)
            end
        end
    end
end

function GameUI:CreateTextGold(player, fishType, fishPos, delta)
    local path = Path_UIControl .. (player:IsPlayerSelf() and "txtGoldMy" or "txtGoldOther") .. ".prefab"
    local prefab = Loader:LoadAsset(path, typeof(GameObject))
    local text = PoolManager.instance:Spawn({
        prefab = prefab,
        parent = self.DownT.transform,
        script = TextCoinEff,
    })
    local pos = fishPos
    pos.z = 0
    text.transform.localPosition = pos
    local scale = 1
    if fishType == 0 or fishType == 1 then
        scale = 0.6
    elseif fishType == 2 then
        scale = 0.8
    end
    text:Play(scale, delta)
end

function GameUI:CreateBomb(player, effect, deltaGold)
    local path = ""
    local v3Pos = Vector3.zero
    PlaySound("SE_capture0")
    if effect ~= 0 then
        path = "fishBigBomb"
        v3Pos = self.UpT.transform:InverseTransformPoint(self.playerCannons[player:GetClientSeat() + 1].rotate.transform.position)
        if player:GetClientSeat() > 1 then
            v3Pos = v3Pos - Vector3.up * 260
        else
            v3Pos = v3Pos + Vector3.up * 260
        end
    end

    if not string.isNullOrEmpty(path) then
        local prefab = Loader:LoadAsset(Path_UIControl .. path .. ".prefab", typeof(GameObject))
        local bombEff = PoolManager.instance:Spawn({
            prefab = prefab,
            parent = self.UpT.transform,
            script = BombEff,
        })
        bombEff.transform.localPosition = v3Pos
        bombEff:Play(deltaGold,effect,name)
    end
end

function GameUI:playFishCome(config)
    table.insert(self.fishComeQueue, config)
    if not self.isShowFishComing then
        self.isShowFishComing = true
        self:dequeueFishCome()
    end
end

function GameUI:dequeueFishCome()
    if #self.fishComeQueue > 0 then
        StartCoroutine(functional.bind1(self.playFishComeAnimation, self))
    else
        self.isShowFishComing = false
    end
end

function GameUI:playFishComeAnimation()
    local data = self.fishComeQueue[1]
    table.remove(self.fishComeQueue, 1)
    local prefab = Loader:LoadAsset(Path_UIControl .. "bossComeTips.prefab", typeof(GameObject))
    local fishComeObj = PoolManager.instance:Spawn({
        prefab = prefab,
        parent = self.EffectT.transform,
    })
    --fishComeObj:Init(data)
    PlaySound("SE_warning")
    WaitForSeconds(5.5)
    self:dequeueFishCome()
end

function GameUI:onFishTideNtf(msg)
    self.fishComeQueue = {}
    local prefab = Loader:LoadAsset(Path_UIControl .. "fishTideCome.prefab", typeof(GameObject))
    local fishComeObj = PoolManager.instance:Spawn({
        prefab = prefab,
        parent = self.EffectT.transform,
    })
    PlaySound("SE_warning")
end



PlayerCannon = class(LuaBase, {})

function PlayerCannon:__init()
    MessageCenter.instance:AddListener(MsgType.LUA_ROOM_BULLET_CREATE, self.onBulletCreate, self)
    self.Module = ModuleManager.instance:Get("FishModule")
    self.room = TFishRoom[self.Module.RoomID]
    self.paoTai = self:get("imgPaoTai")
    self.gunMultipleBg = self:get("GunMultiple")
    self.txtGunValue = self:get("GunMultiple/txtGunMultiple", "Text")
    self.txtGold = self:get("GoldShow/Text","Text")
    self.ImgLock = self:get("EffectLock")
    self.rotate = self:get("Rotate")
    self.bankrupt = self:get("Bankrupt")

    self.btnAdd = self:get("btnAdd")
    UGUIClickLuaBehaviour.Bind(self.btnAdd.gameObject, functional.bind2(self.onBtnChangeGunValue, self, 1))
    self.btnSub = self:get("btnSub")
    UGUIClickLuaBehaviour.Bind(self.btnSub.gameObject, functional.bind2(self.onBtnChangeGunValue, self, -1))

    self.tblGunValue = {}
    self.tblGunId = {}
    for k, v in pairs(TFishGunValue) do
        if v.GunValue >= self.room.MinGun and v.GunValue <= self.room.MaxGun then
            table.insert(self.tblGunValue, v.GunValue)
            table.insert(self.tblGunId, v.GunModel)
        end
    end
end

function PlayerCannon:SetPlayer(player)
    self.player = player
    self.gameObject:SetActive(true)
    self:SetGoldValue(self.player.Currency)
    
    self.player:SetFishingSite(self)
    self:CancelAllState()

    self:CreateCannonById(self.player.CannoId,self.player:IsPlayerSelf())
    if self.player:IsPlayerSelf() then   
        self.btnAdd.gameObject:SetActive(true)
        self.btnSub.gameObject:SetActive(true)
    end
    self.paoTai.gameObject:SetActive(true)
    self.gunMultipleBg.gameObject:SetActive(true)
    self:get("GoldShow").gameObject:SetActive(true)
    
    MessageCenter.instance:AddListener(MsgType.LUA_ROOM_PLAYER_GUNVALUE_CHANGED, self.onPlayerGunValueChanged, self)
    MessageCenter.instance:AddListener(MsgType.LUA_ROOM_PLAYER_CURRENCY_CHANGED,self.onGoldChange,self)
    
    if self.player:IsPlayerSelf() then
        MessageCenter.instance:AddListener(MsgType.LUA_ROOM_AUTO_FIRE_CHANGE, self.onAutoChange, self)
        MessageCenter.instance:AddListener(MsgType.LUA_ROOM_LOCK_FIRE_CHANGE,self.onLockChange,self)
        --self:SendGunValueChangeReq(self.room.MinGun)
    else
        MessageCenter.instance:AddListener(MsgType.LUA_ROOM_OTHER_USE_LAYER, self.onOtherUseLayer, self)
        --MessageCenter.instance:AddListener(MsgType.LUA_ROOM_CLONE_FIRE_OTHER_CHANGE, self.onOtherCloneChange, self)
        MessageCenter.instance:AddListener(MsgType.LUA_ROOM_GUN_ROTATION_CHANGED, self.onGunRotationChanged, self)
    end
end

function PlayerCannon:Clear()
    MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_BULLET_CREATE, self.onBulletCreate, self)
    MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_PLAYER_GUNVALUE_CHANGED, self.onPlayerGunValueChanged, self)
    MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_PLAYER_CURRENCY_CHANGED,self.onGoldChange,self)
    if self.player:IsPlayerSelf() then
        MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_AUTO_FIRE_CHANGE, self.onAutoChange, self)
        MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_LOCK_FIRE_CHANGE,self.onLockChange,self)
    else
        MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_OTHER_USE_LAYER, self.onOtherUseLayer, self)
        MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_GUN_ROTATION_CHANGED, self.onGunRotationChanged, self)
    end
end


function PlayerCannon:PlayerLeave()
    Destroy(self.cannonCtrl.gameObject)
    MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_BULLET_CREATE, self.onBulletCreate, self)
    MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_PLAYER_GUNVALUE_CHANGED, self.onPlayerGunValueChanged, self)
    MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_PLAYER_CURRENCY_CHANGED,self.onGoldChange,self)
    if self.player:IsPlayerSelf() then
        MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_AUTO_FIRE_CHANGE, self.onAutoChange, self)
        MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_LOCK_FIRE_CHANGE,self.onLockChange,self)
    else
        MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_OTHER_USE_LAYER, self.onOtherUseLayer, self)
        MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_GUN_ROTATION_CHANGED, self.onGunRotationChanged, self)
    end

    self.player = nil
    self.gameObject:SetActive(false)
end

function PlayerCannon:OnUpdate()
    if not self.player then return end
    if self.player:IsPlayerSelf() then
        if self.isLock then
            local fish = self.Module:GetFishById(self.cannonCtrl.lockFishId) 
            if fish then
                local fishScript = FishRoomController.instance.FishSpawn:FindFishById(fish.FishID)
                if fishScript and not fishScript:CheckOutOfScreen() then
                    self.cannonCtrl.lockFishId = 0
                    self.cannonCtrl.lockTarget = nil
                end
            end
        end
    end

    if self.player.Currency <= 0 and (not self.bankrupt.gameObject.activeSelf) then
        self.bankrupt.gameObject:SetActive(true)
    end

    if self.player.Currency > 0 and self.bankrupt.gameObject.activeSelf then
        self.bankrupt.gameObject:SetActive(false)
    end

    self.cannonCtrl:OnUpdate()
end

function PlayerCannon:CreateCannonById(cannonId, isPlayer)
    if self.cannonCtrl then
        Destroy(self.cannonCtrl.gameObject)
        self.cannonCtrl = nil
    end

    UnityHelper.ClearChildren(self.rotate.transform)
    local prefab = Loader:LoadAsset(Path_Res .. "Cannon/Cannon_" .. cannonId .. ".prefab", typeof(GameObject))
    local obj = Instantiate(prefab)
    obj.transform:SetParent(self.rotate.transform, false)
    obj.transform.localRotation = Quaternion.identity
    obj.transform.localPosition = Vector3.zero
    self.cannonCtrl = CannonCtrl(LuaClass(obj.gameObject))
    self.cannonCtrl:Init(self, isPlayer, cannonId)
    self:SetGunValue(self.player.GunValue)
    self:SetCannonOther(cannonId)
end

function PlayerCannon:CancelAllState()
    self.isLock = false
    self.isAuto = false
end

function PlayerCannon:GetCannoCtrl()
    return self.cannonCtrl
end

function PlayerCannon:RemoveLockFish()
    self.cannonCtrl.lockFishId = 0
    self.cannonCtrl.lockTarget = nil
end

function PlayerCannon:GetLockFish()
    return self.cannonCtrl.lockFishId
end

function PlayerCannon:onBtnChangeGunValue(para)
    if self.isLock then
        return 
    end
    local curIndex = 1
    local curGunId = 1
    for k, v in pairs(self.tblGunValue) do
        if v == self.player.GunValue then
            curIndex = k
        end
    end
    curIndex = curIndex + para
    if curIndex > #self.tblGunValue then curIndex = 1 end
    if curIndex < 1 then curIndex = #self.tblGunValue end

    if curIndex > 5 then curGunId = curGunId + para end
    if curIndex <= 5 then curGunId = 1 end
    self:SendGunValueChangeReq(self.tblGunValue[curIndex])
end

function PlayerCannon:SendGunValueChangeReq(gunValue)
    local proto = CLFRGunValueChangeReq()
    proto.gun_value = gunValue
    proto:asyncRequest("CLFRGunValueChangeAck", function(ack)
        if ack.errcode == 0 then
            self.player:SetGunValue(ack.gun_value)
            local curIndex = 1
            for k, v in pairs(self.tblGunValue) do
                if v == self.player.GunValue then
                    curIndex = k
                end
            end
            self:GunChanged(self.tblGunId[curIndex][self.Module.RoomID])
        end
    end)
end

function PlayerCannon:onPlayerGunValueChanged(msg)
    if self.player == msg.Player then
        self:SetGunValue(msg.NewValue)
        local curIndex = 1
        for k, v in pairs(self.tblGunValue) do
            if v == msg.NewValue then
                curIndex = k
            end
        end
        self:GunChanged(self.tblGunId[curIndex][self.Module.RoomID])
    end
end

function PlayerCannon:GunChanged(gunId)
    self.player.CannoId = gunId
    self:CreateCannonById(gunId,self.player:IsPlayerSelf())
    PlaySound("SE_swithgun")
end

function PlayerCannon:SetCannonOther(cannonId)
    self.btnAdd.transform:GetComponent("Image").sprite = Loader:LoadAsset(Path_Res .. "AddBtn/add_" .. cannonId .. ".prefab", typeof(GameObject)):GetComponent("Image").sprite
    self.btnSub.transform:GetComponent("Image").sprite = Loader:LoadAsset(Path_Res .. "SubBtn/sub_" .. cannonId .. ".prefab", typeof(GameObject)):GetComponent("Image").sprite
    self.gunMultipleBg.transform:GetComponent("Image").sprite = Loader:LoadAsset(Path_Res .. "GunMultipleBg/gunMultipleBg_" .. cannonId .. ".prefab", typeof(GameObject)):GetComponent("Image").sprite
    self.paoTai.transform:GetComponent("Image").sprite = Loader:LoadAsset(Path_Res .. "CannonBase/CannonBase_" .. cannonId .. ".prefab", typeof(GameObject)):GetComponent("Image").sprite
end

function PlayerCannon:onAutoChange(msg)
    local auto = msg.Auto
    self.isAuto = auto
    self.cannonCtrl.isAuto = self.isAuto
end

function PlayerCannon:onLockChange(msg)
    local lock = msg.Lock
    self.isLock = lock
    self.cannonCtrl.isLock = self.isLock
    self.ImgLock.gameObject:SetActive(self.isLock)
end


function PlayerCannon:onBulletCreate(msg)
    local player = msg.Player
    if player == self.player then
        self.cannonCtrl:PlayFire()
        if self.player:IsPlayerSelf() then
            PlaySound("SE_gunfire1")
        end
    end

end

function PlayerCannon:onGoldChange(msg)
    local player = msg.Player
    if player == self.player then
        self.txtGold.Text.text = UnityHelper.NumberFormat(msg.NewValue)
    end
end

function PlayerCannon:onGunRotationChanged(msg)
    local player = msg.Player
    if player == self.player then
        local angles = msg.Angle
        if angles then
            self.cannonCtrl.transform.localEulerAngles = Vector3(0, 0, angles[1] - 90)
            -- if not self.isClone then
                
            -- else
            --     for i = 1, #angles do
            --         self.cannons[i + 1].transform.localEulerAngles = Vector3(0, 0, angles[i] - 90)
            --     end
            -- end
        end
        
        local fishIdArray = msg.FishIdArray
        if fishIdArray and self.player then
            for i = 1, #fishIdArray do
                -- if not self.isClone then
                    self.cannonCtrl:DirectToFish(fishIdArray[i])
                -- else
                --     self.cannons[i + 1]:DirectToFish(fishIdArray[i])
                -- end
            end
        end
    end
end

function PlayerCannon:SetGunValue(value)
    self.txtGunValue.Text.text = UnityHelper.NumberFormat(value)
end

function PlayerCannon:SetGoldValue(value)
    self.txtGold.Text.text = UnityHelper.NumberFormat(value)
end

CannonCtrl = class(LuaBase, {})

function CannonCtrl:__init()
    self.Module = ModuleManager.instance:Get("FishModule")
    self.animator = self:get("Canno", "Animator")
    self.fireLocate = self:get("FireLocate")
    self.dotT = self:get("DotT")
    self.fireEff = self:get("CannonFire")
    
    self.isFire = false
    self.shootAngle = 0
    self.lockFishId = 0
    self.lockTarget = nil

    self.gunValue = 0
    self.lastFireTimestamp = 0
    self.lastLockTimestamp = 0
end

function CannonCtrl:Init(owner, isPlayer ,cannonId)
    self.owner = owner
    self.isPlayer = isPlayer
    self.bulletShootCD = LocalDefines.BulletShootCD / LocalDefines.NormalBulletShootRate
    self.gunValue = self.owner.player.GunValue

    if self.isPlayer then
        local prefab = Loader:LoadAsset(Path_UIControl .. "ImgLock.prefab", typeof(GameObject))
        self.imgLock = Instantiate(prefab)
        self.imgLock.transform:SetParent(self.transform, false)
        self.imgLock.gameObject:SetActive(false)
    end
end

-- function CannonCtrl:SetGun(gunId)
--     self:get("Canno").transform:GetComponent("Image").sprite = Loader:LoadAsset(Path_Res .. "AddBtn/add_" .. cannonId .. ".prefab", typeof(GameObject)):GetComponent("Image").sprite
-- end

function CannonCtrl:OnUpdate()
    if not self.owner.player then return end
    
    if Input.GetMouseButtonDown() then
        self.detectMouseDown = true
    end
    
    if self.isPlayer then
        self.isFire = false
        local pos = Vector3.zero
        
        if self.owner.bankrupt.gameObject.activeSelf then return end

        local IsNotUI = (not UnityHelper.IsPointerOverUIObject()) and Input.GetMouseButton()
        if IsNotUI then
            local ray = GameController.Instance.MainCamera:ScreenPointToRay(Input.mousePosition)
            local isCast, hit = UnityHelper.Raycast(ray, 1000, SysDefines.FishLayer)
            if isCast and self.owner.isLock and self.detectMouseDown then
                local id = tonumber(hit.transform.gameObject.name)
                local fish = FishRoomController.instance.FishSpawn:FindFishById(id)
                if fish ~= nil then
                    self.lockTarget = fish
                    --self.lastLockTimestamp = TimeHelper.GetServerTimestamp()
                end
            else
                pos = Input.mousePosition
                self.isFire = true
            end
        else
            pos = GameController.Instance.MainCamera:WorldToScreenPoint(self.fireLocate.transform.position)
        end
        if self.owner.isAuto then
            self.isFire = true
        end

        if not self.owner.isLock then
            self.lockTarget = nil
            self.lockFishId = 0
            self:showImgLock(false)
            UnityHelper.ClearChildren(self.dotT.transform)
        end

        if self.lockTarget then
            pos = GameController.Instance.MainCamera:WorldToScreenPoint(self.lockTarget.transform.position)
            self:rectifyCannonRotation(pos)
            self.lockFishId = self.lockTarget.FishID
            local imgLockPos = self.imgLock.transform.parent:InverseTransformPoint(self.lockTarget.transform.position)
            imgLockPos.z = 0
            self.imgLock.transform.localPosition = imgLockPos

            UnityHelper.ClearChildren(self.dotT.transform)
            local dic = Vector2.Distance(Vector2(imgLockPos.x,imgLockPos.y),Vector2(self.dotT.transform.localPosition.x,self.dotT.transform.localPosition.y))
            for i = 1, math.floor(dic/90) do
                local prefab = Loader:LoadAsset(Path_UIControl .. "dot.prefab", typeof(GameObject))
                local dotItem = PoolManager.instance:Spawn({
                    prefab = prefab,
                    parent = self.dotT.transform,
                })
            end
            self:showImgLock(true)
        else
            self:showImgLock(false)
            UnityHelper.ClearChildren(self.dotT.transform)
        end
        
        if self.isFire then
            local angle = self:rectifyCannonRotation(pos)
            local timestamp = TimeHelper.GetServerTimestamp()
            if self.isPlayer then
                if self.lastFireTimestamp + self.bulletShootCD / (self.owner.isRage and LocalDefines.RageSkillPara or 1) <= timestamp then
                    self.shootAngle = angle
                    self.isFire = true
                else
                    self.isFire = false
                end
            end
        end
        
        local firePermit = self.Module:GetBulletPermit()
        local currency = self.owner.player.Currency
        local totalGunValue = self.gunValue * 1
        if self.isFire and firePermit > 0 and totalGunValue <= currency then
        --if self.isFire and firePermit > 0 and totalGunValue <= currency and (TimeHelper.GetServerTimestamp() - self.lastLockTimestamp > 500) then
            self.lastFireTimestamp = TimeHelper.GetServerTimestamp()
            self:attemptFire()
        end
    end
    self.detectMouseDown = false
end

function CannonCtrl:PlayFire()
    if self.gameObject.activeSelf then
        self.animator.Animator:SetTrigger("Fire")
        
        self.fireEff.gameObject:SetActive(true)
    end
end

function CannonCtrl:attemptFire()
    self:PlayFire()
    NetController.instance:SendShootReq(math.floor(self.shootAngle), self.lockFishId, false, self.owner.multiple and 2 or 1)
end

function CannonCtrl:DirectToFish(fishId)
    local fishTarget = FishRoomController.instance.FishSpawn:FindFishById(fishId)
    if fishTarget ~= nil then
        local pos = GameController.Instance.MainCamera:WorldToScreenPoint(fishTarget.transform.position)
        local angle = self:rectifyCannonRotation(pos)
        self.transform.localRotation = Quaternion.Euler(0, 0, angle - 90)
    end
end

function CannonCtrl:showImgLock(show)
    self.imgLock.gameObject:SetActive(show)
    if show then
        self.imgLock.transform:DORestart()
    else
        self.imgLock.transform:DOPause()
    end
end

function CannonCtrl:rectifyCannonRotation(pos)
    local angle = self:calculateAngleDirection(pos)
    self.shootAngle = angle
    self.transform.localRotation = Quaternion.Euler(0, 0, angle - 90)
    return angle
end

function CannonCtrl:calculateAngleDirection(pos)
    local pos1 = GameController.Instance.MainCamera:WorldToScreenPoint(self.transform.position)
    local angle = UnityHelper.CalculateAnticlockwiseAngle(pos1.x, pos1.y, pos.x, pos.y)
    
    if angle < 0 then
        angle = angle + 180
    end

    if angle > 180 then
        angle = 180
    elseif angle < 0 then
        angle = 0
    end
    return angle
end



ButtonLaserGun = class(LuaBase, {})

function ButtonLaserGun:__init()
    DestroyLuaBehaviour.Bind(self.gameObject, self)
    
    MessageCenter.instance:AddListener(MsgType.LUA_ROOM_PLAYER_LASER_ENERGY_CHANGE, self.reset, self)
    MessageCenter.instance:AddListener(MsgType.LUA_ROOM_PLAYER_GUNVALUE_CHANGED, self.reset, self)
    MessageCenter.instance:AddListener(MsgType.LUA_ROOM_TRY_USE_LASER_SUCCESS, self.onTryUseLaserSuccess, self)
    
    self.Module = ModuleManager.instance:Get("FishModule")
    
    UGUIClickLuaBehaviour.Bind(self.gameObject, functional.bind1(self.onClick, self))
    self.effLaserFull = self:get("Eff_LaserGun_UI")
    self.effLaserCanUse = self:get("Eff_LaserGun_UI/Can use")
    self.effLaserTryUse = self:get("Eff_LaserGun_UI/Warn")
    self.imgFill = self:get("Bar/Fill", "Image")
    self.txtCount = self:get("Quantity/Text", "Text")
end

function ButtonLaserGun:OnDestroy()
    MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_PLAYER_LASER_ENERGY_CHANGE, self.reset, self)
    MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_PLAYER_GUNVALUE_CHANGED, self.reset, self)
    MessageCenter.instance:RemoveListener(MsgType.LUA_ROOM_TRY_USE_LASER_SUCCESS, self.onTryUseLaserSuccess, self)
end

function ButtonLaserGun:onClick()
    if self.canUseCount >= 1 then
        MessageCenter.instance:SendMessage(MsgType.LUA_ROOM_TRY_USE_LASER)
    end
end

function ButtonLaserGun:reset(msg)
    local config = TFishGlobal[1]
    local curEnergy = GameController.Instance.Player.LaserGunEnergy
    local curGunValue = self.Module.PlayerSelf.GunValue
    local cost = curGunValue * config.EnergyCostDegreeForShoot
    local count = curEnergy / cost
    if count >= 1 then
        self.effLaserFull.gameObject:SetActive(true)
        self.effLaserCanUse.gameObject:SetActive(true)
    else
        self.effLaserFull.gameObject:SetActive(false)
        self.effLaserCanUse.gameObject:SetActive(false)
    end
    self.imgFill.Image.fillAmount = count % 1
    self.canUseCount = math.floor(count)
    self.txtCount.Text.text = self.canUseCount
end

function ButtonLaserGun:onTryUseLaserSuccess(msg)
    self.effLaserTryUse.gameObject:SetActive(false)
    self.effLaserTryUse.gameObject:SetActive(true)
    self:reset()
end



LaserGunEffect = class(LuaBase, {})

function LaserGunEffect:__init()
    self.RotateT = self:get("rotate")
    self.ArrowT = self:get("rotate/Img")
    self.EffectCast = self:get("Eff_nengliangpao_xuli")
    self.EffectFire = self:get("rotate/Eff_nengliangpao_jiguang")
    self.TransTips = self:get("tips")
    self.TxtTime = self:get("tips/timeCount", "Text")
end

function LaserGunEffect:Cast()
    self.ArrowT.gameObject:SetActive(true)
    self.EffectCast.gameObject:SetActive(true)
    self.EffectFire.gameObject:SetActive(false)
    self.RotateT.transform.rotation = Quaternion.Euler(0, 0, 0)
    self.TransTips.gameObject:SetActive(true)
end

function LaserGunEffect:Fire()
    self.ArrowT.gameObject:SetActive(false)
    self.EffectCast.gameObject:SetActive(false)
    self.EffectFire.gameObject:SetActive(true)
    self.TransTips.gameObject:SetActive(false)
end

function LaserGunEffect:SetTime(secs)
    self.TxtTime.Text.text = "(" .. secs .. "秒后将自动发射)"
end

function LaserGunEffect:RectifyPosition(pos)
    local angle = self:calculateAngleDirection(pos)
    self.RotateT.transform.rotation = Quaternion.Euler(0, 0, angle - 90)
end

function LaserGunEffect:RectifyRotation(angle)
    self.RotateT.transform.rotation = Quaternion.Euler(0, 0, angle)
end

function LaserGunEffect:calculateAngleDirection(pos)
    local pos1 = GameController.Instance.MainCamera:WorldToScreenPoint(self.transform.position);
    local angle = UnityHelper.CalculateAnticlockwiseAngle(pos1.x, pos1.y, pos.x, pos.y);
    if angle < 0 then
        angle = angle + 180
    end
    
    if angle > 180 then
        angle = 180
    elseif angle < 0 then
        angle = 0
    end
    
    return angle;
end



LaserGunCollider = class(LuaBase, {})

function LaserGunCollider:__init()
    ColliderLuaBehaviour.Bind(self.gameObject, self)
end

function LaserGunCollider:AddCallback(callback)
    self.callback = callback
    self.fishIdList = {}
    self.active = true
    StartCoroutine(functional.bind1(self.collect, self))
end

function LaserGunCollider:OnTriggerStay(collider)
    if not self.active then return end
    if collider:CompareTag("fish") then
        local id = tonumber(collider.gameObject.name)
        local fish = FishRoomController.instance.FishSpawn:FindFishById(id)
        if fish and fish:IsValid() then
            fish:Hit(true)
            if #self.fishIdList < 30 and not table.contains(self.fishIdList, fish.FishID) then
                table.insert(self.fishIdList, fish.FishID)
            end
        end
    end
end

function LaserGunCollider:collect()
    WaitForSeconds(0.1)
    self.active = false
    self.callback(self.fishIdList)
    Destroy(self.gameObject)
end



ImageCoinEff = class(LuaBase, {})

function ImageCoinEff:__init()
    self.transCoin = self.transform:GetChild(0)
end

function ImageCoinEff:SetCoin(pos, delay, callback)
    delay = delay and delay or 0
    self.endPos = pos
    self.startDelay = delay
    self.endCallback = callback
    
    StartCoroutine(functional.bind1(self.checkDelay, self))
end

function ImageCoinEff:checkDelay()
    self.transCoin.gameObject:SetActive(false)
    WaitForSeconds(self.startDelay)
    local sequence = DOTween.Sequence()
    sequence:AppendInterval(1)
    local time = Vector3.Distance(self.transform.position, self.endPos) * 0.03
    sequence:Append(self.transform:DOMove(self.endPos, time):SetEase(Ease.InQuad))
    sequence:AppendCallback(function()
        if self.endCallback then
            self.endCallback()
            self.endCallback = nil
        end
        PlaySound("SE_hitfishcoin")
        PoolManager.instance:Unspawn(self)
    end)
    self.transCoin.gameObject:SetActive(true);
end



TextCoinEff = class(LuaBase, {})

function TextCoinEff:__init()
    UpdateLuaBehaviour.Bind(self.gameObject, self)
    
    self.uiText = self.transform:GetComponent("Text")
    self.orgColor = self.uiText.color
    self.scaleMax = 0
    self.isScale1 = false
    self.isScale2 = false
    self.isMove = false
    self.Scale = 0
    self.NumberStr = ""
    self.scaleMaxRat = 1.4
    self.stayTime = 0.4
    self.moveSpeed = 1
    self.fadeTime = 0.8
    self.scaleSpeed = 0.8
    self.fadeSpeed = 1
end

function TextCoinEff:Play(scale, numStr)
    self.NumberStr = numStr
    self.Scale = scale
    self.transform.localScale = Vector3.one * self.Scale
    self.isScale1 = true
    self.isScale2 = false
    self.uiText.color = self.orgColor
    self.scaleMax = self.Scale * self.scaleMaxRat
    self.uiText.text = "+" .. UnityHelper.NumberFormat(self.NumberStr)
    if self.moveCoroutine ~= nil then
        StopCoroutine(self.moveCoroutine)
    end
    self.moveCoroutine = StartCoroutine(functional.bind1(self.move, self))
end

function TextCoinEff:Update()
    if self.isScale1 then
        self.transform.localScale = self.transform.localScale + (Vector3.one * self.scaleSpeed * Time.deltaTime)
        if self.transform.localScale.x >= self.scaleMax then
            self.isScale2 = true
            self.isScale1 = false
            self.transform.localScale = Vector3.one * self.scaleMax
        end
    end
    if self.isScale2 then
        self.transform.localScale = self.transform.localScale - (Vector3.one * self.scaleSpeed * Time.deltaTime)
        if self.transform.localScale.x <= self.Scale then
            self.isScale2 = false
            self.transform.localScale = Vector3.one * self.Scale
        end
    end
    if self.isMove then
        self.transform.position = self.transform.position + (Vector3.up * self.moveSpeed * Time.deltaTime)
        self.uiText.color = self.uiText.color - Color(0, 0, 0, self.fadeSpeed * Time.deltaTime)
    end
end

function TextCoinEff:move()
    self.isMove = false
    WaitForSeconds(self.stayTime)
    self.isMove = true
    WaitForSeconds(self.fadeTime)
    PoolManager.instance:Unspawn(self)
end



FishComeTips = class(LuaBase, {})

function FishComeTips:__init()
    self.rectBoss = self:get("boss")
    self.rectSpecial = self:get("specialFish")
    self.rectBoss.gameObject:SetActive(false)
    self.rectSpecial.gameObject:SetActive(false)
end

function FishComeTips:Init(fish)
    self.hideDelay = 3
    if fish.FishType == 6 then
        self.rectBoss.transform:Find("Hint/txtFishMultiple"):GetComponent("Text").text = fish.MultipleView == 1 and fish.Multiple or "???"
        self.rectBoss.transform:Find("Hint/txtFishName"):GetComponent("Text").text = "[" .. fish.Name .. "]"
        self.rectBoss.gameObject:SetActive(true)
    elseif fish.FishType == 4 then
        self.rectSpecial.transform:Find("specialFish/txtFishName"):GetComponent("Text").text = "[" .. fish.Name .. "]"
        self.rectSpecial.gameObject:SetActive(true)
    end
    StartCoroutine(functional.bind1(self.hide, self))
end

function FishComeTips:hide()
    WaitForSeconds(self.hideDelay)
    self.rectBoss.gameObject:SetActive(false)
    self.rectSpecial.gameObject:SetActive(false)
    PoolManager.instance:Unspawn(self)
end



BombEff = class(LuaBase, {})

function BombEff:__init()
    UpdateLuaBehaviour.Bind(self.gameObject, self)
    
    self.stayTime = 3
    self.scaleSpeed = 4
    self.scaleSpeed2 = 3
    self.isScale1 = false
    self.isScale2 = false
    self.isMove = false
    self.uiText = self:get("Font", "Text")
end

function BombEff:Update()
    if self.isScale1 then
        self.transform.localScale = self.transform.localScale + (self.scale * self.scaleSpeed * Time.deltaTime)
        if self.transform.localScale.x >= self.scale.x * 1.2 then
            self.isScale2 = true
            self.isScale1 = false
            self.transform.localScale = self.scale * 1.2
        end
    end
    if self.isScale2 then
        self.transform.localScale = self.transform.localScale - (self.scale * self.scaleSpeed * 1.3 * Time.deltaTime)
        if self.transform.localScale.x <= self.scale.x then
            self.isScale2 = false
            self.transform.localScale = self.scale
        end
    end
    if self.isMove then
        self.transform.localScale = self.transform.localScale - (self.scale * self.scaleSpeed2 * Time.deltaTime)
        self.transform.localScale = Vector3.Max(Vector3.zero, self.transform.localScale)
        
        if self.transform.localScale == Vector3.zero then
            PoolManager.instance:Unspawn(self)
        end
    end
end

function BombEff:Play(numStr)
    self.NumberStr = numStr
    self.transform.localScale = Vector3.zero
    self.scale = Vector3.one
    self.isScale1 = true
    self.isScale2 = false
    self.uiText.Text.text = UnityHelper.NumberFormat(self.NumberStr)
    if self.moveCoroutine ~= nil then
        StopCoroutine(self.moveCoroutine)
    end
    self.moveCoroutine = StartCoroutine(functional.bind1(self.move, self))
end

function BombEff:move()
    self.isMove = false
    WaitForSeconds(self.stayTime)
    self.isMove = true
end
