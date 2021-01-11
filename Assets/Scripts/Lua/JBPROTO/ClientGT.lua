----------------------------------------------------------
--二进制协议序列化/反序列化文件，由工具自动生成，请勿手动修改
--JBPROTO POWERED BY monkey256 ^_^
----------------------------------------------------------

--引入模块
local ProtocolBase = _G.import(".ProtocolBase")
local ProtocolHelper = _G.import(".ProtocolHelper")

--握手请求
--platform number 运行平台 1:IOS 2:ANDRIOD 3:WINDOWS 4:LINUX 5:MAC
--product number 产品代号 0:未知的产品 1:游戏平台
--version number 产品版本号
--device string[64] 机器设备码
--channel string[32] 渠道
--country string[16] 国家标识
--language string[16] 语言标识
_G.CLGTHandReq = _G.class(ProtocolBase)
function _G.CLGTHandReq:__init(t)
    _G.CLGTHandReq.super:__init(t)
    self.mid = 1
    self.pid = 0
end
function _G.CLGTHandReq:deserialize(br)
    self.platform = br:ReadUInt32()
    self.product = br:ReadUInt32()
    self.version = br:ReadUInt32()
    self.device = br:ReadString()
    self.channel = br:ReadString()
    self.country = br:ReadString()
    self.language = br:ReadString()
end
function _G.CLGTHandReq:serialize(bw)
    bw:WriteUInt32(self.platform)
    bw:WriteUInt32(self.product)
    bw:WriteUInt32(self.version)
    bw:WriteString(self.device, 64)
    bw:WriteString(self.channel, 32)
    bw:WriteString(self.country, 16)
    bw:WriteString(self.language, 16)
end

--握手回应
--errcode number 0成功 1无法识别的平台 2无法识别的产品 3版本太老需强更 4拒绝访问 5你的IP已被封禁 6你的设备已被封禁
--payload number 当前网关负载
--random_key number 随机数秘钥
_G.CLGTHandAck = _G.class(ProtocolBase)
function _G.CLGTHandAck:__init(t)
    _G.CLGTHandAck.super:__init(t)
    self.mid = 1
    self.pid = 1
end
function _G.CLGTHandAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.payload = br:ReadInt32()
    self.random_key = br:ReadInt32()
end
function _G.CLGTHandAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt32(self.payload)
    bw:WriteInt32(self.random_key)
end

--网络中断通知
--code number 0断开通知 1连接超时 2被踢下线 3被挤下线 4网关维护 5平台维护 6游戏维护 7与平台服务器断开连接 8与游戏服务器断开连接 9系统错误 10离线挂机
_G.CLGTDisconnectNtf = _G.class(ProtocolBase)
function _G.CLGTDisconnectNtf:__init(t)
    _G.CLGTDisconnectNtf.super:__init(t)
    self.mid = 1
    self.pid = 2
end
function _G.CLGTDisconnectNtf:deserialize(br)
    self.code = br:ReadUInt8()
end
function _G.CLGTDisconnectNtf:serialize(bw)
    bw:WriteUInt8(self.code)
end

--物品信息结构
--item_id number 物品主类型
--item_sub_id number 物品子类型
--item_count number 物品数量
local function CLGTItemInfo_deserialize(br)
    local r = {}
    r.item_id = br:ReadInt32()
    r.item_sub_id = br:ReadInt32()
    r.item_count = br:ReadInt64()
    return r
end
local function CLGTItemInfo_serialize(t, bw)
    bw:WriteInt32(t.item_id)
    bw:WriteInt32(t.item_sub_id)
    bw:WriteInt64(t.item_count)
end

--登录平台请求
--login_type number 登录方式 1游客 2手机登录 3QQ 4微信 5Facebook 6GooglePlay 7GameCenter
--token string[256] 唯一标识串，CA3加密
_G.CLGTLoginReq = _G.class(ProtocolBase)
function _G.CLGTLoginReq:__init(t)
    _G.CLGTLoginReq.super:__init(t)
    self.mid = 1
    self.pid = 3
end
function _G.CLGTLoginReq:deserialize(br)
    self.login_type = br:ReadUInt8()
    self.token = br:ReadString()
end
function _G.CLGTLoginReq:serialize(bw)
    bw:WriteUInt8(self.login_type)
    bw:WriteString(self.token, 256)
end

--管理员登录平台请求
--user_id number 目标玩家Id
_G.CLGTAdminLoginReq = _G.class(ProtocolBase)
function _G.CLGTAdminLoginReq:__init(t)
    _G.CLGTAdminLoginReq.super:__init(t)
    self.mid = 1
    self.pid = 4
end
function _G.CLGTAdminLoginReq:deserialize(br)
    self.user_id = br:ReadInt32()
end
function _G.CLGTAdminLoginReq:serialize(bw)
    bw:WriteInt32(self.user_id)
end

--登录平台应答
--errcode number 0成功 1平台服务器不可用 2账号被封禁 3系统繁忙 4系统错误 5系统暂未开放
--user_id number 玩家Id
--nickname string[32] 昵称
--nickname_mdf number 昵称是否修改过 1是 0否
--gender number 性别 0保密 1男 2女
--head number 头像Id
--head_frame number 头像框Id
--level number 玩家等级
--level_exp number 玩家等级经验
--vip_level number 玩家vip等级
--vip_level_exp number 玩家vip等级经验
--phone string[20] 绑定手机
--diamond number 平台钻石
--currency number 平台货币
--bind_currency number 平台绑定货币
--integral number 平台积分
--item_len number 数组长度
--items ItemInfo[100] 物品数组
--server_timestamp number 服务器时间戳
--extra_params string[4096] 额外附加参数
_G.CLGTLoginAck = _G.class(ProtocolBase)
function _G.CLGTLoginAck:__init(t)
    _G.CLGTLoginAck.super:__init(t)
    self.mid = 1
    self.pid = 5
end
function _G.CLGTLoginAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.user_id = br:ReadInt32()
    self.nickname = br:ReadString()
    self.nickname_mdf = br:ReadInt8()
    self.gender = br:ReadInt32()
    self.head = br:ReadInt32()
    self.head_frame = br:ReadInt32()
    self.level = br:ReadInt32()
    self.level_exp = br:ReadInt64()
    self.vip_level = br:ReadInt32()
    self.vip_level_exp = br:ReadInt64()
    self.phone = br:ReadString()
    self.diamond = br:ReadInt64()
    self.currency = br:ReadInt64()
    self.bind_currency = br:ReadInt64()
    self.integral = br:ReadInt64()
    self.item_len = br:ReadInt32()
    self.items = {}
    for i=1,self.item_len do
        self.items[i] = CLGTItemInfo_deserialize(br)
    end
    self.server_timestamp = br:ReadUInt32()
    self.extra_params = br:ReadString()
end
function _G.CLGTLoginAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt32(self.user_id)
    bw:WriteString(self.nickname, 32)
    bw:WriteInt8(self.nickname_mdf)
    bw:WriteInt32(self.gender)
    bw:WriteInt32(self.head)
    bw:WriteInt32(self.head_frame)
    bw:WriteInt32(self.level)
    bw:WriteInt64(self.level_exp)
    bw:WriteInt32(self.vip_level)
    bw:WriteInt64(self.vip_level_exp)
    bw:WriteString(self.phone, 20)
    bw:WriteInt64(self.diamond)
    bw:WriteInt64(self.currency)
    bw:WriteInt64(self.bind_currency)
    bw:WriteInt64(self.integral)
    assert(not self.items or #self.items <= 100, "CLGTLoginAck.items数组长度超过规定限制! 期望:100 实际:" .. #self.items)
    bw:WriteInt32(#(self.items or {}))
    for i=1,#(self.items or {}) do
        CLGTItemInfo_serialize(self.items[i], bw)
    end
    bw:WriteUInt32(self.server_timestamp)
    bw:WriteString(self.extra_params, 4096)
end

--访问游戏服务请求
--group_id number 服务组Id
--action number 1加入服务 2离开服务
_G.CLGTAccessServiceReq = _G.class(ProtocolBase)
function _G.CLGTAccessServiceReq:__init(t)
    _G.CLGTAccessServiceReq.super:__init(t)
    self.mid = 1
    self.pid = 6
end
function _G.CLGTAccessServiceReq:deserialize(br)
    self.group_id = br:ReadInt32()
    self.action = br:ReadInt32()
end
function _G.CLGTAccessServiceReq:serialize(bw)
    bw:WriteInt32(self.group_id)
    bw:WriteInt32(self.action)
end

--访问游戏服务应答
--errcode number 0成功 1服务不存在 2拒绝访问
_G.CLGTAccessServiceAck = _G.class(ProtocolBase)
function _G.CLGTAccessServiceAck:__init(t)
    _G.CLGTAccessServiceAck.super:__init(t)
    self.mid = 1
    self.pid = 7
end
function _G.CLGTAccessServiceAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLGTAccessServiceAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--心跳包，客户端应当每间隔10秒发一个心跳包，证明你还活着
_G.CLGTKeepAlive = _G.class(ProtocolBase)
function _G.CLGTKeepAlive:__init(t)
    _G.CLGTKeepAlive.super:__init(t)
    self.mid = 1
    self.pid = 8
end

--注册协议
ProtocolHelper.registerProtocol("CLGTHandReq", _G.CLGTHandReq, 1, 0)
ProtocolHelper.registerProtocol("CLGTHandAck", _G.CLGTHandAck, 1, 1)
ProtocolHelper.registerProtocol("CLGTDisconnectNtf", _G.CLGTDisconnectNtf, 1, 2)
ProtocolHelper.registerProtocol("CLGTLoginReq", _G.CLGTLoginReq, 1, 3)
ProtocolHelper.registerProtocol("CLGTAdminLoginReq", _G.CLGTAdminLoginReq, 1, 4)
ProtocolHelper.registerProtocol("CLGTLoginAck", _G.CLGTLoginAck, 1, 5)
ProtocolHelper.registerProtocol("CLGTAccessServiceReq", _G.CLGTAccessServiceReq, 1, 6)
ProtocolHelper.registerProtocol("CLGTAccessServiceAck", _G.CLGTAccessServiceAck, 1, 7)
ProtocolHelper.registerProtocol("CLGTKeepAlive", _G.CLGTKeepAlive, 1, 8)
