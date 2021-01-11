local ProtocolHelper = import(".ProtocolHelper")
--协议结构基类
local ProtocolBase = class(nil)

function ProtocolBase:__init(t)
    --初始化
    if t and type(t) == "table" then
        for k, v in pairs(t) do
            self[k] = v
        end
    end
end

--序列化
function ProtocolBase:serialize(bw)
end

--反序列化
function ProtocolBase:deserialize(br)
end

--发送协议
function ProtocolBase:send()
    local bw = CS.NetBinaryWriterProxy(self.mid, self.pid)
    self:serialize(bw)
    bw:Send()
end

--发送req，异步等待ack
function ProtocolBase:asyncRequest(name, callback)
    self:send()
    ProtocolHelper.addResponseHandler(name, callback)
end

return ProtocolBase
