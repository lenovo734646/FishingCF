LuaComponentExample = class(LuaBase,{
	buttonCreate,
	buttonChange,
	})

function LuaComponentExample:__init()
	log("LuaComponent:Init")
	log(self.gameObject.name)
	UpdateLuaBehaviour.Bind(self.gameObject,self)

	self:getComponents("Image")
	self.Image.color = Color.black
	self.index = 1;
	self.button = self:get("PanelMain/Button","Button","Image")	--get()第一个参数是子物体的名字，后面的参数是UnityEngine组件的名字，可填多个组件
	--两种绑定点击响应的方式，推荐第二种，可以对无Button组件的UI元素进行响应
	--self.button.Button.onClick:AddListener(functional.bind1(self.OnButtonClick,self))
	UGUIClickLuaBehaviour.Bind(self.button.gameObject,functional.bind1(self.OnButtonClick,self))

	--self.toggle.Toggle:OnValueChanged.AddListener()

	--UGUIClickLuaBehaviour.Bind(self.button.gameObject,functional.bind2(self.OnButtonClick,self，1))
	--如果是多个按钮绑定同一个响应方法，需要添加参数就用functional.bind2(方法，self，参数)

	-- self.buttonCreate = self:get("GameObject/Button1","Button","Image")
	-- self.buttonChange = self:get("Button2","Button")

	-- self.someTimer = Timer(functional.bind1(self.TestTimeFunc,self),1,3)
	-- self.someTimer:Start()
end

function LuaComponentExample:Update()
	log("LuaComponent:Update")
end

function LuaComponentExample:OnButtonClick(paras)
	log("LuaComponent:ButtonClick")
	log(self.index)
end

function LuaComponentExample:TestTimeFunc()
	-- body
end

function LuaComponentExample:Hide( ... )
	--todo destroy instance
	self.someTimer:Stop()
	self.someTimer:Reset(functional.bind1(self.TestTimeFunc,self),1,3)
	self.someTimer:Start()
	self.someTimer = nil
end