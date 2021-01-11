----------------------------------------------------------
--二进制协议序列化/反序列化文件，由工具自动生成，请勿手动修改
--JBPROTO POWERED BY monkey256 ^_^
----------------------------------------------------------

--引入模块
local ProtocolBase = _G.import(".ProtocolBase")
local ProtocolHelper = _G.import(".ProtocolHelper")

--登入服务应答
--gun_id number 当前的炮台Id
--max_gun_value number 解锁的最大炮值
--bonus_pool number 个人累积的奖金池数量
--bonus_count number 个人累积的打死奖金鱼数量
--multiple_hit number 倍击倍数
_G.CLFMEnterServerNtf = _G.class(ProtocolBase)
function _G.CLFMEnterServerNtf:__init(t)
    _G.CLFMEnterServerNtf.super:__init(t)
    self.mid = 10
    self.pid = 0
end
function _G.CLFMEnterServerNtf:deserialize(br)
    self.gun_id = br:ReadInt32()
    self.max_gun_value = br:ReadInt64()
    self.bonus_pool = br:ReadInt64()
    self.bonus_count = br:ReadInt32()
    self.multiple_hit = br:ReadInt32()
end
function _G.CLFMEnterServerNtf:serialize(bw)
    bw:WriteInt32(self.gun_id)
    bw:WriteInt64(self.max_gun_value)
    bw:WriteInt64(self.bonus_pool)
    bw:WriteInt32(self.bonus_count)
    bw:WriteInt32(self.multiple_hit)
end

--加入玩法请求
--site_id number 玩法Id 1捕鱼3D 2捕鱼2D
_G.CLFMEnterSiteReq = _G.class(ProtocolBase)
function _G.CLFMEnterSiteReq:__init(t)
    _G.CLFMEnterSiteReq.super:__init(t)
    self.mid = 10
    self.pid = 1
end
function _G.CLFMEnterSiteReq:deserialize(br)
    self.site_id = br:ReadInt32()
end
function _G.CLFMEnterSiteReq:serialize(bw)
    bw:WriteInt32(self.site_id)
end

--加入玩法回应
--errcode number 0成功 1无可用服务器 2系统错误
_G.CLFMEnterSiteAck = _G.class(ProtocolBase)
function _G.CLFMEnterSiteAck:__init(t)
    _G.CLFMEnterSiteAck.super:__init(t)
    self.mid = 10
    self.pid = 2
end
function _G.CLFMEnterSiteAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLFMEnterSiteAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--退出玩法请求
--site_id number 玩法Id 1捕鱼3D 2捕鱼2D
_G.CLFMExitSiteReq = _G.class(ProtocolBase)
function _G.CLFMExitSiteReq:__init(t)
    _G.CLFMExitSiteReq.super:__init(t)
    self.mid = 10
    self.pid = 3
end
function _G.CLFMExitSiteReq:deserialize(br)
    self.site_id = br:ReadInt32()
end
function _G.CLFMExitSiteReq:serialize(bw)
    bw:WriteInt32(self.site_id)
end

--退出玩法回应
--errcode number 0成功
_G.CLFMExitSiteAck = _G.class(ProtocolBase)
function _G.CLFMExitSiteAck:__init(t)
    _G.CLFMExitSiteAck.super:__init(t)
    self.mid = 10
    self.pid = 4
end
function _G.CLFMExitSiteAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLFMExitSiteAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--炮台锻造请求
--use_crystal number 是否使用100个水晶保证成功
_G.CLFMGunForgeReq = _G.class(ProtocolBase)
function _G.CLFMGunForgeReq:__init(t)
    _G.CLFMGunForgeReq.super:__init(t)
    self.mid = 10
    self.pid = 5
end
function _G.CLFMGunForgeReq:deserialize(br)
    self.use_crystal = br:ReadInt8()
end
function _G.CLFMGunForgeReq:serialize(bw)
    bw:WriteInt8(self.use_crystal)
end

--炮台锻造回应
--errcode number 0成功 1锻造失败 2资源不足 3道具不足 4系统错误
--max_gun_value number 解锁后最大炮值
--diamond_delta number 钻石变化量
--currency_delta number 金币变化量
--bind_currency_delta number 金币变化量
--crystal_id number 水晶物品Id
--crystal_sub_id number 水晶物品子Id
--crystal_count number 锻造失败时返还的水晶数量
_G.CLFMGunForgeAck = _G.class(ProtocolBase)
function _G.CLFMGunForgeAck:__init(t)
    _G.CLFMGunForgeAck.super:__init(t)
    self.mid = 10
    self.pid = 6
end
function _G.CLFMGunForgeAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.max_gun_value = br:ReadInt64()
    self.diamond_delta = br:ReadInt64()
    self.currency_delta = br:ReadInt64()
    self.bind_currency_delta = br:ReadInt64()
    self.crystal_id = br:ReadInt32()
    self.crystal_sub_id = br:ReadInt32()
    self.crystal_count = br:ReadInt32()
end
function _G.CLFMGunForgeAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt64(self.max_gun_value)
    bw:WriteInt64(self.diamond_delta)
    bw:WriteInt64(self.currency_delta)
    bw:WriteInt64(self.bind_currency_delta)
    bw:WriteInt32(self.crystal_id)
    bw:WriteInt32(self.crystal_sub_id)
    bw:WriteInt32(self.crystal_count)
end

--比赛排行信息
--min_rank number 排名段起始
--max_rank number 排名段结束
--nickname string[32] 昵称
--integral number 排名段结束名次所对应积分
local function CLFMMatchRankInfo_deserialize(br)
    local r = {}
    r.min_rank = br:ReadInt32()
    r.max_rank = br:ReadInt32()
    r.nickname = br:ReadString()
    r.integral = br:ReadInt32()
    return r
end
local function CLFMMatchRankInfo_serialize(t, bw)
    bw:WriteInt32(t.min_rank)
    bw:WriteInt32(t.max_rank)
    bw:WriteString(t.nickname, 32)
    bw:WriteInt32(t.integral)
end

--比赛排行榜请求
--match_type number 1免费赛 2大奖赛
_G.CLFMMatchRankReq = _G.class(ProtocolBase)
function _G.CLFMMatchRankReq:__init(t)
    _G.CLFMMatchRankReq.super:__init(t)
    self.mid = 10
    self.pid = 7
end
function _G.CLFMMatchRankReq:deserialize(br)
    self.match_type = br:ReadInt32()
end
function _G.CLFMMatchRankReq:serialize(bw)
    bw:WriteInt32(self.match_type)
end

--比赛排行榜回应
--my_day_integral number 我的今日积分
--my_day_rank number 我的今日排名
--my_week_integral number 我的本周积分
--top_week_user_id number 预计周冠军的玩家Id
--top_week_nickname string[32] 预计周冠军的昵称
--top_week_integral number 预计周冠军的总积分
--rank_section_len number 排行榜分段数组长度
--rank_section_array MatchRankInfo[20] 排行榜分段数组
_G.CLFMMatchRankAck = _G.class(ProtocolBase)
function _G.CLFMMatchRankAck:__init(t)
    _G.CLFMMatchRankAck.super:__init(t)
    self.mid = 10
    self.pid = 8
end
function _G.CLFMMatchRankAck:deserialize(br)
    self.my_day_integral = br:ReadInt32()
    self.my_day_rank = br:ReadInt32()
    self.my_week_integral = br:ReadInt32()
    self.top_week_user_id = br:ReadInt32()
    self.top_week_nickname = br:ReadString()
    self.top_week_integral = br:ReadInt32()
    self.rank_section_len = br:ReadInt32()
    self.rank_section_array = {}
    for i=1,self.rank_section_len do
        self.rank_section_array[i] = CLFMMatchRankInfo_deserialize(br)
    end
end
function _G.CLFMMatchRankAck:serialize(bw)
    bw:WriteInt32(self.my_day_integral)
    bw:WriteInt32(self.my_day_rank)
    bw:WriteInt32(self.my_week_integral)
    bw:WriteInt32(self.top_week_user_id)
    bw:WriteString(self.top_week_nickname, 32)
    bw:WriteInt32(self.top_week_integral)
    assert(not self.rank_section_array or #self.rank_section_array <= 20, "CLFMMatchRankAck.rank_section_array数组长度超过规定限制! 期望:20 实际:" .. #self.rank_section_array)
    bw:WriteInt32(#(self.rank_section_array or {}))
    for i=1,#(self.rank_section_array or {}) do
        CLFMMatchRankInfo_serialize(self.rank_section_array[i], bw)
    end
end

--房间人数信息
--config_id number 房间配置Id
--total_count number 在线人数
local function CLFMRoomUserCountInfo_deserialize(br)
    local r = {}
    r.config_id = br:ReadInt32()
    r.total_count = br:ReadInt32()
    return r
end
local function CLFMRoomUserCountInfo_serialize(t, bw)
    bw:WriteInt32(t.config_id)
    bw:WriteInt32(t.total_count)
end

--查询房间总人数请求
--site_id number 玩法Id 1捕鱼3D 2捕鱼2D
_G.CLFMRoomUserCountSummaryReq = _G.class(ProtocolBase)
function _G.CLFMRoomUserCountSummaryReq:__init(t)
    _G.CLFMRoomUserCountSummaryReq.super:__init(t)
    self.mid = 10
    self.pid = 9
end
function _G.CLFMRoomUserCountSummaryReq:deserialize(br)
    self.site_id = br:ReadInt32()
end
function _G.CLFMRoomUserCountSummaryReq:serialize(bw)
    bw:WriteInt32(self.site_id)
end

--查询房间总人数回应
--info_len number 房间人数信息数组长度
--info_array RoomUserCountInfo[100] 房间人数信息数组
_G.CLFMRoomUserCountSummaryAck = _G.class(ProtocolBase)
function _G.CLFMRoomUserCountSummaryAck:__init(t)
    _G.CLFMRoomUserCountSummaryAck.super:__init(t)
    self.mid = 10
    self.pid = 10
end
function _G.CLFMRoomUserCountSummaryAck:deserialize(br)
    self.info_len = br:ReadInt32()
    self.info_array = {}
    for i=1,self.info_len do
        self.info_array[i] = CLFMRoomUserCountInfo_deserialize(br)
    end
end
function _G.CLFMRoomUserCountSummaryAck:serialize(bw)
    assert(not self.info_array or #self.info_array <= 100, "CLFMRoomUserCountSummaryAck.info_array数组长度超过规定限制! 期望:100 实际:" .. #self.info_array)
    bw:WriteInt32(#(self.info_array or {}))
    for i=1,#(self.info_array or {}) do
        CLFMRoomUserCountInfo_serialize(self.info_array[i], bw)
    end
end

--查询房间详细人数请求
--site_id number 玩法Id 1捕鱼3D 2捕鱼2D
--config_id number 房间配置Id
--start_room_id number 请求房间的起始编号
--count number 请求数量
_G.CLFMRoomUserCountDetailReq = _G.class(ProtocolBase)
function _G.CLFMRoomUserCountDetailReq:__init(t)
    _G.CLFMRoomUserCountDetailReq.super:__init(t)
    self.mid = 10
    self.pid = 11
end
function _G.CLFMRoomUserCountDetailReq:deserialize(br)
    self.site_id = br:ReadInt32()
    self.config_id = br:ReadInt32()
    self.start_room_id = br:ReadInt32()
    self.count = br:ReadInt32()
end
function _G.CLFMRoomUserCountDetailReq:serialize(bw)
    bw:WriteInt32(self.site_id)
    bw:WriteInt32(self.config_id)
    bw:WriteInt32(self.start_room_id)
    bw:WriteInt32(self.count)
end

--查询房间详细人数回应
--errcode number 0成功 1配置id非法
--amount_len number 房间人数数组长度
--amount_array number[100] 房间人数数组
_G.CLFMRoomUserCountDetailAck = _G.class(ProtocolBase)
function _G.CLFMRoomUserCountDetailAck:__init(t)
    _G.CLFMRoomUserCountDetailAck.super:__init(t)
    self.mid = 10
    self.pid = 12
end
function _G.CLFMRoomUserCountDetailAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.amount_len = br:ReadInt32()
    self.amount_array = {}
    for i=1,self.amount_len do
        self.amount_array[i] = br:ReadInt8()
    end
end
function _G.CLFMRoomUserCountDetailAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.amount_array or #self.amount_array <= 100, "CLFMRoomUserCountDetailAck.amount_array数组长度超过规定限制! 期望:100 实际:" .. #self.amount_array)
    bw:WriteInt32(#(self.amount_array or {}))
    for i=1,#(self.amount_array or {}) do
        bw:WriteInt8(self.amount_array[i])
    end
end

--注册协议
ProtocolHelper.registerProtocol("CLFMEnterServerNtf", _G.CLFMEnterServerNtf, 10, 0)
ProtocolHelper.registerProtocol("CLFMEnterSiteReq", _G.CLFMEnterSiteReq, 10, 1)
ProtocolHelper.registerProtocol("CLFMEnterSiteAck", _G.CLFMEnterSiteAck, 10, 2)
ProtocolHelper.registerProtocol("CLFMExitSiteReq", _G.CLFMExitSiteReq, 10, 3)
ProtocolHelper.registerProtocol("CLFMExitSiteAck", _G.CLFMExitSiteAck, 10, 4)
ProtocolHelper.registerProtocol("CLFMGunForgeReq", _G.CLFMGunForgeReq, 10, 5)
ProtocolHelper.registerProtocol("CLFMGunForgeAck", _G.CLFMGunForgeAck, 10, 6)
ProtocolHelper.registerProtocol("CLFMMatchRankReq", _G.CLFMMatchRankReq, 10, 7)
ProtocolHelper.registerProtocol("CLFMMatchRankAck", _G.CLFMMatchRankAck, 10, 8)
ProtocolHelper.registerProtocol("CLFMRoomUserCountSummaryReq", _G.CLFMRoomUserCountSummaryReq, 10, 9)
ProtocolHelper.registerProtocol("CLFMRoomUserCountSummaryAck", _G.CLFMRoomUserCountSummaryAck, 10, 10)
ProtocolHelper.registerProtocol("CLFMRoomUserCountDetailReq", _G.CLFMRoomUserCountDetailReq, 10, 11)
ProtocolHelper.registerProtocol("CLFMRoomUserCountDetailAck", _G.CLFMRoomUserCountDetailAck, 10, 12)
