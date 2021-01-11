local setmetatable = setmetatable
local xpcall = xpcall
local pcall = pcall
local assert = assert
local rawget = rawget
local error = error
local print = print
local unpack = unpack or table.unpack
local ilist = ilist

local _xpcall = {}

_xpcall.__call = function(self,...)
	local args = {...}

	if self.obj == nil then
		local func = function()
			self.func(unpack(args))
		end
		return xpcall(func,__TRACKBACK__)
	else
		local func = function()
			self.func(self.obj,unpack(args))
		end
		return xpcall(func,__TRACKBACK__)
	end
end

_xpcall.__eq = function(lhs,rhs)
	return lhs.func == rhs.func and lhs.obj == rhs.obj
end

local function xfunctor(func,obj)
	return setmetatable({func = func, obj = obj}, _xpcall)
end

local _pcall = {}

_pcall.__call = function(self,...)
	if self.obj == nil then
		return pcall(self.func,...)
	else
		return pcall(self.func,self.obj,...)
	end
end

_pcall.__eq = function(lhs,rhs)
	return lhs.func == rhs.func and lhs.obj == rhs.obj
end

local function functor(func,obj)
	return setmetatable({func = func, obj = obj}, _pcall)
end

local function handlerWrapper(handle)
	return {handle = handle, __mark_delete = false}
end

local _event = {}
_event.__index = _event

function _event:CreateListner(func,obj)
	if self.keepSafe then
		func = xfunctor(func,obj)
	else
		func = functor(func,obj)
	end

	return func
end

function _event:AddListner(handle)
	if not handle then return end

	local exist = false
	for _,v in ipairs(self.list) do
		if v and v.handle == handle and not v.__mark_delete then
			exist = true
			break
		end
	end

	if not exist then 
		table.insert(self.list,handlerWrapper(handle))
	end
end

function _event:RemoveListner(handle)
	local find
	for _,v in ipairs(self.list) do
		if v and v.handle == handle and not v.__mark_delete then
			find = v
			break
		end
	end

	if find then
		find.__mark_delete = true
	end
end

function _event:Count()
	local count = 0
	for _,v in ipairs(self.list) do
		if v and not v.__mark_delete then
			count = count + 1
		end
	end
	return count
end

function _event:Clear()
	self.list = {}
	self.keepSafe = false
end

function _event:Dump()
	local count = 0

	for _,v in ipairs(self.list) do
		if v and v.handle and not v.__mark_delete then
			v = v.handle

			if v.obj then
				print("update function:",v.func,"object name:",v.obj.name)
			else
				print("update function:",v.func)
			end

			count = count + 1
		end
	end

	print("all function is:",count)
end

function _event:__call(...)
	local run_list = {}
	for _,v in ipairs(self.list) do
		if v and v.handle and not v.__mark_delete then
			table.insert(run_list,v)
		end
	end

	for _,v in ipairs(run_list) do
		if v and v.handle and not v.__mark_delete then
			local flag,msg = v.handle(...)

			if not flag then
				if self.keepSafe then
					v.__mark_delete = true
				end
			end
		end
	end

	for i=#self.list,1,-1 do
		local v = self.list[i];
		if not v or v.__mark_delete then
			table.remove(self.list,i)
		end
	end
end

local function event(name,safe)
	safe = safe or false
	return setmetatable({name = name, keepSafe = safe, list = {}},_event)
end

UpdateBeat = event("Update",true)
LateUpdateBeat = event("LateUpdate",true)
FixedUpdateBeat = event("FixedUpdate",true)

local UpdateBeat = UpdateBeat
local LateUpdateBeat = LateUpdateBeat
local FixedUpdateBeat = FixedUpdateBeat

function Update()
	UpdateBeat()
end

function LateUpdate()
	LateUpdateBeat()
end

function FixedUpdate()
	FixedUpdateBeat()
end

function PrintEvents()
	UpdateBeat:Dump()
	FixedUpdateBeat:Dump()
end

return event