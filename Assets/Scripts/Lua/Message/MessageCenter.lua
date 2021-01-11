MessageCenter = class(nil, {})

function MessageCenter:__init()
    self.dicMsgBind = {}
    self.dicMsgEvents = {}
end

function MessageCenter:AddListener(msgType, func, target)
    if self.dicMsgBind[msgType] == nil then
        self.dicMsgBind[msgType] = {}
    end
    local bindFunc = functional.bind1(func, target)
    table.insert(self.dicMsgBind[msgType], {
        bind = bindFunc,
        func = func,
        target = target,
    })
    self:_addListener(msgType, bindFunc)
end

function MessageCenter:_addListener(msgType, event)
    if self.dicMsgEvents[msgType] == nil then
        self.dicMsgEvents[msgType] = {}
    end
    if not table.contains(self.dicMsgEvents[msgType], event) then
        table.insert(self.dicMsgEvents[msgType], event)
    end
end

function MessageCenter:RemoveListener(msgType, func, target)
    local tbl = table.findArray(self.dicMsgBind[msgType], function(b)
        return b.func == func and b.target == target
    end)
    for k, v in pairs(tbl) do
        self:_removeListener(msgType, v.bind)
        table.removebyvalue(self.dicMsgBind[msgType], v)
    end
end

function MessageCenter:RemoveAllByType(msgType,target)
    local tbl = table.findArray(self.dicMsgBind[msgType], function(b)
        return b.target == target
    end)
    for k, v in pairs(tbl) do
        self:_removeListener(msgType, v.bind)
        table.removebyvalue(self.dicMsgBind[msgType], v)
    end
end

function MessageCenter:RemoveAllByTarget(target)
    local tbl = {}
    for k,v in pairs(self.dicMsgBind) do
        for k1,v1 in pairs(v) do
            if v1.target == target then
                table.insert( tbl, {
                    msgType = k,
                    value = v1,
                })
            end
        end
    end
    for k, v in pairs(tbl) do
        self:_removeListener(v.msgType, v.value.bind)
        table.removebyvalue(self.dicMsgBind[v.msgType], v.value)
    end
end

function MessageCenter:_removeListener(msgType, event)
    if self.dicMsgEvents[msgType] ~= nil then
        local index = table.contains(self.dicMsgEvents[msgType], event)
        if index then
            table.removebyvalue(self.dicMsgEvents[msgType], event, true)
        end
    end
end

function MessageCenter:RemoveAllListener()
    self.dicMsgEvents = {}
end

function MessageCenter:SendMessage(msgType, content)
    if self.dicMsgEvents[msgType] ~= nil then
        for k, v in ipairs(self.dicMsgEvents[msgType]) do
            v(content)
        end
    end
end
