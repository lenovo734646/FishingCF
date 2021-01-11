local LuaClass = class(nil,{
	transform,
	gameObject,
	})

function LuaClass:New(go)
	local obj = {}
	setmetatable(obj,self)
	if obj.__init then
		obj:__init(go)
	end
	return obj
end

function LuaClass:__init(go)
	self.gameObject = go
	self.transform = go.transform
end

return LuaClass