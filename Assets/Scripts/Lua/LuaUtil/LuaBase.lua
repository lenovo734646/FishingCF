local LuaClass = require "LuaUtil/LuaClass"

local LuaBase = class()

function LuaBase:New(obj,...)
	setmetatable(obj,self)
	if obj.__init then
		obj:__init(...)
	end
	return obj
end

function LuaBase:get(str,...)
	local tf = self.transform:Find(str)

	if not tf then
		logError("can not find child:"..str)
	end

	local obj = LuaClass(tf.gameObject)
	obj = LuaBase(obj)
	local typeNames = {...}
	for _,typeName in pairs(typeNames) do
		local component = tf:GetComponent(typeName)
		if component then
			if type(typeName) == "userdata" then
				obj[typeName.Name] = component
			else
				obj[typeName] = component
			end
		else
			logError("can not find component:"..typeName.." of "..str)
		end
	end
	return obj
end

function LuaBase:getComponents(...)
	local typeNames = {...};
	for _,typeName in pairs(typeNames) do
		local component = self.gameObject:GetComponent(typeName)
		if component then
			if type(typeName) == "userdata" then
				self[typeName.Name] = component
			else
				self[typeName] = component
			end
		else
			logError("can not find component:"..typeName.." of "..self.gameObject.Name)
		end
	end
end

--显示
function LuaBase:Show()
	if not	self.gameObject.activeSelf then
		self.gameObject:SetActive(true)
	end
end

--隐藏
function LuaBase:Hide()
	if 	self.gameObject.activeSelf then
		self.gameObject:SetActive(false)
	end
end

--设置子物体的状态
function LuaBase:SetActiveChildren(parent, active)
	local count = parent.childCount
	for i = 1, count do
		local child = parent:GetChild(i - 1).gameObject
		if child.activeSelf ~= active then
			child:SetActive(active)
		end
	end
end

--清除根节点下的子物体
function LuaBase:CloseChildren(parent)
	if	parent == nil then
		return
	end
	local count = parent.childCount
	for i = 1, count do
		local child = parent:GetChild(i - 1).gameObject
		Destroy(child)
	end
end

--关闭页面
function LuaBase:Close()
	Destroy(self.gameObject)
end

return LuaBase