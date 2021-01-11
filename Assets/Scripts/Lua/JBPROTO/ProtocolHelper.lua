
local ProtocolHelper = {}

local idRouters = {}
local nameRouters = {}
local ntfListeners = {}
local ackListeners = {}

--添加ntf监听，一个ntf可以添加多个监听
function ProtocolHelper.addNotifyHandler(name, callback)
    local t = ntfListeners[name]
    if not t then
        t = {}
        ntfListeners[name] = t
    end
    table.insert(t, callback)
end
--移除ntf监听
function ProtocolHelper.removeNotifyHandler(name, callback)
    local t = ntfListeners[name]
    if t then
        table.removebyvalue(t, callback)
    end
end
--清空ntf监听
function ProtocolHelper.clearNotifyHandler(name)
    ntfListeners[name] = nil
end

--添加ack监听
function ProtocolHelper.addResponseHandler(name, callback)
    local t = ackListeners[name]
    if not t then
        t = {}
        ackListeners[name] = t
    end
    table.insert(t, callback)
end
--移除某个ack所有监听
function ProtocolHelper.clearResponseHandlers(name)
    ackListeners[name] = nil
end
--清空所有ack监听
function ProtocolHelper.clearAllResponseHandlers()
    ackListeners = {}
end

--注册声明协议
function ProtocolHelper.registerProtocol(name, cls, mid, pid)
    local data =
    {
        name = name,
        cls = cls,
        mid = mid,
        pid = pid,
    }
    if not idRouters[mid] then
        idRouters[mid] = {}
    end
    idRouters[mid][pid] = data
end
--根据Id获取协议信息
function ProtocolHelper.getProtocolData(mid, pid)
    local t = idRouters[mid]
    return t and t[pid]
end

--接收到网络数据
function ProtocolHelper.onReceiveNetData(br)
    --优先考虑ack监听，再考虑ntf监听
    local data = ProtocolHelper.getProtocolData(br.ModuleId, br.ProtocolId)
    if data then
        local callback = nil
        local name = data.name
        local t = ackListeners[name]
        if t and #t > 0 then
            callback = t[1]
            table.remove(t, 1)
        else
            t = ntfListeners[name]
            if t and #t > 0 then
                callback = t
            end
        end

        if callback then
            local proto = data.cls()
            proto:deserialize(br)
            if type(callback) == "function" then
                callback(proto)
            elseif type(callback) == "table" then
                for _, v in ipairs(callback) do
                    v(proto)
                end
            end
        end
    end
end

return ProtocolHelper
