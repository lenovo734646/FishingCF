Timer = {}

function Timer:New(func,duration,loop,safe)
	scale = scale or false and true
	loop = loop or 1
	return setmetatable({func = func, duration = duration, time = duration, loop = loop, scale = scale, running = false},self)
end

function Timer:Start()
	self.running = true

	if not self.handle then
		self.handle = UpdateBeat:CreateListner(self.Update,self)
	end

	UpdateBeat:AddListner(self.handle)
end

function Timer:Reset(func,duration,loop,safe,updateFunc)
	self.duration = duration
	self.loop = loop or 1
	self.scale = scale
	self.func = func
	self.time = duration
	self.updateFunc = updateFunc
end

function Timer:Stop()
	self.running = false
	UpdateBeat:RemoveListner(self.handle)
end

function Timer:Update()
	local delta = self.scale and Time.deltaTime or Time.unscaledDeltaTime
	self.time = self.time - delta

	if self.updateFunc then
		self.updateFunc(delta,self.time)
	end

	if self.time <= 0 then
		self.func()

		if self.loop > 0 then
			self.loop = self.loop - 1
			self.time = self.time + self.duration
		end

		if self.loop == 0 then
			self:Stop()
		elseif self.loop < 0 then
			self.time = self.time + self.duration
		end
	end
end

return class(nil,Timer,Timer.New)