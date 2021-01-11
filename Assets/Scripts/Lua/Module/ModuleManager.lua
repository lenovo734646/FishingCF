ModuleManager = class(nil, {
    moduleMap,
})

function ModuleManager:__init()
    self:RegisterAllModules()
end

function ModuleManager:RegisterAllModules()
    self.moduleMap = {}
    self.moduleMap["MainModule"] = MainModule()
    self.moduleMap["FishModule"] = FishModule()
end

function ModuleManager:Get(name)
    return self.moduleMap[name]
end
