----------------------------------------------------------
--二进制协议序列化/反序列化文件，由工具自动生成，请勿手动修改
--JBPROTO POWERED BY monkey256 ^_^
----------------------------------------------------------

--引入模块
local ProtocolBase = _G.import(".ProtocolBase")
local ProtocolHelper = _G.import(".ProtocolHelper")

--房间内玩家信息
--user_id number 玩家Id
--nickname string[32] 昵称
--gender number 性别 0保密 1男 2女
--head number 头像Id
--head_frame number 头像框Id
--level number 玩家等级
--level_exp number 玩家等级经验
--vip_level number 玩家vip等级
--vip_level_exp number 玩家vip等级经验
--currency number 平台货币
--bind_currency number 平台绑定货币
--diamond number 平台钻石
--seat_id number 炮台位置：0~3
--gun_id number 炮台Id
--gun_value number 当前炮值
--left_gun_num number 当前比赛剩余开炮次数
--integral number 当前比赛积分
local function CLFRRoomPlayerInfo_deserialize(br)
    local r = {}
    r.user_id = br:ReadInt32()
    r.nickname = br:ReadString()
    r.gender = br:ReadInt32()
    r.head = br:ReadInt32()
    r.head_frame = br:ReadInt32()
    r.level = br:ReadInt32()
    r.level_exp = br:ReadInt64()
    r.vip_level = br:ReadInt32()
    r.vip_level_exp = br:ReadInt64()
    r.currency = br:ReadInt64()
    r.bind_currency = br:ReadInt64()
    r.diamond = br:ReadInt64()
    r.seat_id = br:ReadInt32()
    r.gun_id = br:ReadInt32()
    r.gun_value = br:ReadInt64()
    r.left_gun_num = br:ReadInt32()
    r.integral = br:ReadInt32()
    return r
end
local function CLFRRoomPlayerInfo_serialize(t, bw)
    bw:WriteInt32(t.user_id)
    bw:WriteString(t.nickname, 32)
    bw:WriteInt32(t.gender)
    bw:WriteInt32(t.head)
    bw:WriteInt32(t.head_frame)
    bw:WriteInt32(t.level)
    bw:WriteInt64(t.level_exp)
    bw:WriteInt32(t.vip_level)
    bw:WriteInt64(t.vip_level_exp)
    bw:WriteInt64(t.currency)
    bw:WriteInt64(t.bind_currency)
    bw:WriteInt64(t.diamond)
    bw:WriteInt32(t.seat_id)
    bw:WriteInt32(t.gun_id)
    bw:WriteInt64(t.gun_value)
    bw:WriteInt32(t.left_gun_num)
    bw:WriteInt32(t.integral)
end

--鱼游动的时间变化率信息
--time number 开始时间，单位毫秒
--duration number 持续时间，单位毫秒
--rate number 时间变化率万分比
--reason number 变化原因 1冰冻 2鱼潮来临 3装逼悬停 4普通悬停
local function CLFRFishTimeRateInfo_deserialize(br)
    local r = {}
    r.time = br:ReadUInt64()
    r.duration = br:ReadUInt64()
    r.rate = br:ReadInt32()
    r.reason = br:ReadInt32()
    return r
end
local function CLFRFishTimeRateInfo_serialize(t, bw)
    bw:WriteUInt64(t.time)
    bw:WriteUInt64(t.duration)
    bw:WriteInt32(t.rate)
    bw:WriteInt32(t.reason)
end

--鱼出现信息
--fish_id number 鱼Id
--config_id number 鱼配置Id
--start_time number 开始时间，单位毫秒
--stop_time number 结束时间，单位毫秒
--path_id number 路径Id
--offset_x number 路径偏移量，单位自己决定
--offset_y number 路径偏移量，单位自己决定
--rate_len number 时间变化率数组长度
--rates FishTimeRateInfo[20] 时间变化率数组
--is_multiple_group number 是否是一网打尽鱼群中的鱼 1是0否
local function CLFRFishAppearInfo_deserialize(br)
    local r = {}
    r.fish_id = br:ReadInt32()
    r.config_id = br:ReadInt32()
    r.start_time = br:ReadUInt64()
    r.stop_time = br:ReadUInt64()
    r.path_id = br:ReadInt32()
    r.offset_x = br:ReadInt32()
    r.offset_y = br:ReadInt32()
    r.rate_len = br:ReadInt32()
    r.rates = {}
    for i=1,r.rate_len do
        r.rates[i] = CLFRFishTimeRateInfo_deserialize(br)
    end
    r.is_multiple_group = br:ReadInt8()
    return r
end
local function CLFRFishAppearInfo_serialize(t, bw)
    bw:WriteInt32(t.fish_id)
    bw:WriteInt32(t.config_id)
    bw:WriteUInt64(t.start_time)
    bw:WriteUInt64(t.stop_time)
    bw:WriteInt32(t.path_id)
    bw:WriteInt32(t.offset_x)
    bw:WriteInt32(t.offset_y)
    assert(not t.rates or #t.rates <= 20, "CLFRFishAppearInfo.rates数组长度超过规定限制! 期望:20 实际:" .. #t.rates)
    bw:WriteInt32(#(t.rates or {}))
    for i=1,#(t.rates or {}) do
        CLFRFishTimeRateInfo_serialize(t.rates[i], bw)
    end
    bw:WriteInt8(t.is_multiple_group)
end

--世界boss出现信息
--config_id number boss配置Id，关联FishBoss表的Id
--start_time number 开始时间，单位毫秒
--stop_time number 结束时间，单位毫秒
--left_blood number 防御罩剩余血量百分比0-100，该值等于0时boss处于落体状态
local function CLFRBossAppearInfo_deserialize(br)
    local r = {}
    r.config_id = br:ReadInt32()
    r.start_time = br:ReadUInt64()
    r.stop_time = br:ReadUInt64()
    r.left_blood = br:ReadInt8()
    return r
end
local function CLFRBossAppearInfo_serialize(t, bw)
    bw:WriteInt32(t.config_id)
    bw:WriteUInt64(t.start_time)
    bw:WriteUInt64(t.stop_time)
    bw:WriteInt8(t.left_blood)
end

--物品信息结构
--item_id number 物品主类型
--item_sub_id number 物品子类型
--item_count number 物品数量
local function CLFRItemInfo_deserialize(br)
    local r = {}
    r.item_id = br:ReadInt32()
    r.item_sub_id = br:ReadInt32()
    r.item_count = br:ReadInt64()
    return r
end
local function CLFRItemInfo_serialize(t, bw)
    bw:WriteInt32(t.item_id)
    bw:WriteInt32(t.item_sub_id)
    bw:WriteInt64(t.item_count)
end

--使用中的物品信息
--seat_id number 炮台位置：0~3
--item_id number 物品主类型
--item_sub_id number 物品子类型
--end_time number 效果结束时间戳
local function CLFRItemUsingInfo_deserialize(br)
    local r = {}
    r.seat_id = br:ReadInt32()
    r.item_id = br:ReadInt32()
    r.item_sub_id = br:ReadInt32()
    r.end_time = br:ReadUInt64()
    return r
end
local function CLFRItemUsingInfo_serialize(t, bw)
    bw:WriteInt32(t.seat_id)
    bw:WriteInt32(t.item_id)
    bw:WriteInt32(t.item_sub_id)
    bw:WriteUInt64(t.end_time)
end

--进入游戏请求
--config_id number 房间配置Id，对应房间配置表
--room_id number 指定房间Id，-1代表随机分配
--seat_id number 指定座位Id，-1代表随机分配
_G.CLFREnterGameReq = _G.class(ProtocolBase)
function _G.CLFREnterGameReq:__init(t)
    _G.CLFREnterGameReq.super:__init(t)
    self.mid = 11
    self.pid = 0
end
function _G.CLFREnterGameReq:deserialize(br)
    self.config_id = br:ReadInt32()
    self.room_id = br:ReadInt32()
    self.seat_id = br:ReadInt32()
end
function _G.CLFREnterGameReq:serialize(bw)
    bw:WriteInt32(self.config_id)
    bw:WriteInt32(self.room_id)
    bw:WriteInt32(self.seat_id)
end

--进入游戏回应
--errcode number 0成功 1最大炮倍不足尚未解锁 2房间未开放 3房间已满 4指定的座位已被占用 5系统错误
--time_stamp number 当前时间戳
--room_id number 房间Id
--match_finish_count number 当天已完成的比赛次数
--is_matching number 是否正在比赛中 1是0否
_G.CLFREnterGameAck = _G.class(ProtocolBase)
function _G.CLFREnterGameAck:__init(t)
    _G.CLFREnterGameAck.super:__init(t)
    self.mid = 11
    self.pid = 1
end
function _G.CLFREnterGameAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.time_stamp = br:ReadUInt64()
    self.room_id = br:ReadInt32()
    self.match_finish_count = br:ReadInt32()
    self.is_matching = br:ReadInt8()
end
function _G.CLFREnterGameAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteUInt64(self.time_stamp)
    bw:WriteInt32(self.room_id)
    bw:WriteInt32(self.match_finish_count)
    bw:WriteInt8(self.is_matching)
end

--退出游戏请求
_G.CLFRExitGameReq = _G.class(ProtocolBase)
function _G.CLFRExitGameReq:__init(t)
    _G.CLFRExitGameReq.super:__init(t)
    self.mid = 11
    self.pid = 2
end

--退出游戏回应
--errcode number 0成功 1不在房间中
_G.CLFRExitGameAck = _G.class(ProtocolBase)
function _G.CLFRExitGameAck:__init(t)
    _G.CLFRExitGameAck.super:__init(t)
    self.mid = 11
    self.pid = 3
end
function _G.CLFRExitGameAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLFRExitGameAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--我已准备好，可以接收数据了
_G.CLFRGetReadyReq = _G.class(ProtocolBase)
function _G.CLFRGetReadyReq:__init(t)
    _G.CLFRGetReadyReq.super:__init(t)
    self.mid = 11
    self.pid = 4
end

--当前房间内玩家信息和鱼的信息
--errcode number 0成功 1系统错误
--player_len number 房间内玩家数量
--players RoomPlayerInfo[4] 房间内玩家信息
--fish_count number 鱼数量
--fishes FishAppearInfo[1000] 房间内所有鱼的路径数组
--item_using_len number 使用中的物品长度
--item_using_array ItemUsingInfo[100] 使用中的物品数组
--background_id number 房间背景Id 1~4
--in_tide number 是否正在鱼潮来临中 1是0否
--tide_left_time number 距离鱼潮结束剩余秒数
--has_boss number 是否存在世界boss 1是0否
--boss_info BossAppearInfo 世界boss信息
--energy number 玩家当前累积能量
_G.CLFRGetReadyAck = _G.class(ProtocolBase)
function _G.CLFRGetReadyAck:__init(t)
    _G.CLFRGetReadyAck.super:__init(t)
    self.mid = 11
    self.pid = 5
end
function _G.CLFRGetReadyAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.player_len = br:ReadInt32()
    self.players = {}
    for i=1,self.player_len do
        self.players[i] = CLFRRoomPlayerInfo_deserialize(br)
    end
    self.fish_count = br:ReadInt32()
    self.fishes = {}
    for i=1,self.fish_count do
        self.fishes[i] = CLFRFishAppearInfo_deserialize(br)
    end
    self.item_using_len = br:ReadInt32()
    self.item_using_array = {}
    for i=1,self.item_using_len do
        self.item_using_array[i] = CLFRItemUsingInfo_deserialize(br)
    end
    self.background_id = br:ReadInt32()
    self.in_tide = br:ReadInt8()
    self.tide_left_time = br:ReadInt32()
    self.has_boss = br:ReadInt8()
    self.boss_info = CLFRBossAppearInfo_deserialize(br)
    self.energy = br:ReadInt64()
end
function _G.CLFRGetReadyAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.players or #self.players <= 4, "CLFRGetReadyAck.players数组长度超过规定限制! 期望:4 实际:" .. #self.players)
    bw:WriteInt32(#(self.players or {}))
    for i=1,#(self.players or {}) do
        CLFRRoomPlayerInfo_serialize(self.players[i], bw)
    end
    assert(not self.fishes or #self.fishes <= 1000, "CLFRGetReadyAck.fishes数组长度超过规定限制! 期望:1000 实际:" .. #self.fishes)
    bw:WriteInt32(#(self.fishes or {}))
    for i=1,#(self.fishes or {}) do
        CLFRFishAppearInfo_serialize(self.fishes[i], bw)
    end
    assert(not self.item_using_array or #self.item_using_array <= 100, "CLFRGetReadyAck.item_using_array数组长度超过规定限制! 期望:100 实际:" .. #self.item_using_array)
    bw:WriteInt32(#(self.item_using_array or {}))
    for i=1,#(self.item_using_array or {}) do
        CLFRItemUsingInfo_serialize(self.item_using_array[i], bw)
    end
    bw:WriteInt32(self.background_id)
    bw:WriteInt8(self.in_tide)
    bw:WriteInt32(self.tide_left_time)
    bw:WriteInt8(self.has_boss)
    CLFRBossAppearInfo_serialize(self.boss_info, bw)
    bw:WriteInt64(self.energy)
end

--报名比赛请求
_G.CLFRJoinMatchReq = _G.class(ProtocolBase)
function _G.CLFRJoinMatchReq:__init(t)
    _G.CLFRJoinMatchReq.super:__init(t)
    self.mid = 11
    self.pid = 6
end

--报名比赛回应
--errcode number 0成功 1比赛未开放 2比赛即将结束 3未在比赛房间 4已在比赛中 5钻石不足 6已达参赛次数上限 7系统错误
--left_gun_num number 当前比赛剩余开炮次数
_G.CLFRJoinMatchAck = _G.class(ProtocolBase)
function _G.CLFRJoinMatchAck:__init(t)
    _G.CLFRJoinMatchAck.super:__init(t)
    self.mid = 11
    self.pid = 7
end
function _G.CLFRJoinMatchAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.left_gun_num = br:ReadInt32()
end
function _G.CLFRJoinMatchAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt32(self.left_gun_num)
end

--报名比赛通知
--seat_id number 炮台位置：0~3
--left_gun_num number 当前比赛剩余开炮次数
_G.CLFRJoinMatchNtf = _G.class(ProtocolBase)
function _G.CLFRJoinMatchNtf:__init(t)
    _G.CLFRJoinMatchNtf.super:__init(t)
    self.mid = 11
    self.pid = 8
end
function _G.CLFRJoinMatchNtf:deserialize(br)
    self.seat_id = br:ReadInt32()
    self.left_gun_num = br:ReadInt32()
end
function _G.CLFRJoinMatchNtf:serialize(bw)
    bw:WriteInt32(self.seat_id)
    bw:WriteInt32(self.left_gun_num)
end

--比赛结束通知
--reason number 1正常打完结束 2比赛截止强制结束
--integral number 比赛获得积分
--gun_addition number 炮台加成百分比
--challenge_addition number 挑战加成百分比
--vip_addition number vip加成百分比
--final_integral number 最终结算的积分
--rank number 当前积分所对应的名次
--diamond_reward number 获得钻石奖励数量
--has_give_reward number 是否已发放过首次完成奖励 1是0否
_G.CLFRMatchOverNtf = _G.class(ProtocolBase)
function _G.CLFRMatchOverNtf:__init(t)
    _G.CLFRMatchOverNtf.super:__init(t)
    self.mid = 11
    self.pid = 9
end
function _G.CLFRMatchOverNtf:deserialize(br)
    self.reason = br:ReadInt8()
    self.integral = br:ReadInt32()
    self.gun_addition = br:ReadInt32()
    self.challenge_addition = br:ReadInt32()
    self.vip_addition = br:ReadInt32()
    self.final_integral = br:ReadInt32()
    self.rank = br:ReadInt32()
    self.diamond_reward = br:ReadInt32()
    self.has_give_reward = br:ReadInt8()
end
function _G.CLFRMatchOverNtf:serialize(bw)
    bw:WriteInt8(self.reason)
    bw:WriteInt32(self.integral)
    bw:WriteInt32(self.gun_addition)
    bw:WriteInt32(self.challenge_addition)
    bw:WriteInt32(self.vip_addition)
    bw:WriteInt32(self.final_integral)
    bw:WriteInt32(self.rank)
    bw:WriteInt32(self.diamond_reward)
    bw:WriteInt8(self.has_give_reward)
end

--玩家加入通知
--player RoomPlayerInfo 玩家信息
_G.CLFRPlayerJoinNtf = _G.class(ProtocolBase)
function _G.CLFRPlayerJoinNtf:__init(t)
    _G.CLFRPlayerJoinNtf.super:__init(t)
    self.mid = 11
    self.pid = 10
end
function _G.CLFRPlayerJoinNtf:deserialize(br)
    self.player = CLFRRoomPlayerInfo_deserialize(br)
end
function _G.CLFRPlayerJoinNtf:serialize(bw)
    CLFRRoomPlayerInfo_serialize(self.player, bw)
end

--玩家离开通知
--seat_id number 炮台位置：0~3
_G.CLFRPlayerLeaveNtf = _G.class(ProtocolBase)
function _G.CLFRPlayerLeaveNtf:__init(t)
    _G.CLFRPlayerLeaveNtf.super:__init(t)
    self.mid = 11
    self.pid = 11
end
function _G.CLFRPlayerLeaveNtf:deserialize(br)
    self.seat_id = br:ReadInt32()
end
function _G.CLFRPlayerLeaveNtf:serialize(bw)
    bw:WriteInt32(self.seat_id)
end

--鱼潮来临通知
--background_id number 房间背景Id
--duration number 鱼潮持续时间，单位秒
_G.CLFRFishTideStartNtf = _G.class(ProtocolBase)
function _G.CLFRFishTideStartNtf:__init(t)
    _G.CLFRFishTideStartNtf.super:__init(t)
    self.mid = 11
    self.pid = 12
end
function _G.CLFRFishTideStartNtf:deserialize(br)
    self.background_id = br:ReadInt32()
    self.duration = br:ReadInt32()
end
function _G.CLFRFishTideStartNtf:serialize(bw)
    bw:WriteInt32(self.background_id)
    bw:WriteInt32(self.duration)
end

--新鱼出现通知
--type number 0普通鱼 1鱼潮
--fish_count number 鱼信息数组数量
--fishes FishAppearInfo[1000] 房间内所有鱼的信息数组
_G.CLFRFishAppearNtf = _G.class(ProtocolBase)
function _G.CLFRFishAppearNtf:__init(t)
    _G.CLFRFishAppearNtf.super:__init(t)
    self.mid = 11
    self.pid = 13
end
function _G.CLFRFishAppearNtf:deserialize(br)
    self.type = br:ReadInt32()
    self.fish_count = br:ReadInt32()
    self.fishes = {}
    for i=1,self.fish_count do
        self.fishes[i] = CLFRFishAppearInfo_deserialize(br)
    end
end
function _G.CLFRFishAppearNtf:serialize(bw)
    bw:WriteInt32(self.type)
    assert(not self.fishes or #self.fishes <= 1000, "CLFRFishAppearNtf.fishes数组长度超过规定限制! 期望:1000 实际:" .. #self.fishes)
    bw:WriteInt32(#(self.fishes or {}))
    for i=1,#(self.fishes or {}) do
        CLFRFishAppearInfo_serialize(self.fishes[i], bw)
    end
end

--鱼的时间变化率改变通知
--fish_id number 鱼Id
--rate FishTimeRateInfo 时间变化率信息
_G.CLFRFishTimeRateChangeNtf = _G.class(ProtocolBase)
function _G.CLFRFishTimeRateChangeNtf:__init(t)
    _G.CLFRFishTimeRateChangeNtf.super:__init(t)
    self.mid = 11
    self.pid = 14
end
function _G.CLFRFishTimeRateChangeNtf:deserialize(br)
    self.fish_id = br:ReadInt32()
    self.rate = CLFRFishTimeRateInfo_deserialize(br)
end
function _G.CLFRFishTimeRateChangeNtf:serialize(bw)
    bw:WriteInt32(self.fish_id)
    CLFRFishTimeRateInfo_serialize(self.rate, bw)
end

--改变炮值请求
--gun_value number 炮值
_G.CLFRGunValueChangeReq = _G.class(ProtocolBase)
function _G.CLFRGunValueChangeReq:__init(t)
    _G.CLFRGunValueChangeReq.super:__init(t)
    self.mid = 11
    self.pid = 15
end
function _G.CLFRGunValueChangeReq:deserialize(br)
    self.gun_value = br:ReadInt64()
end
function _G.CLFRGunValueChangeReq:serialize(bw)
    bw:WriteInt64(self.gun_value)
end

--改变炮值回应
--errcode number 0成功 1系统错误
--gun_value number 当前炮值
_G.CLFRGunValueChangeAck = _G.class(ProtocolBase)
function _G.CLFRGunValueChangeAck:__init(t)
    _G.CLFRGunValueChangeAck.super:__init(t)
    self.mid = 11
    self.pid = 16
end
function _G.CLFRGunValueChangeAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.gun_value = br:ReadInt64()
end
function _G.CLFRGunValueChangeAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt64(self.gun_value)
end

--炮值改变通知
--seat_id number 座位Id
--gun_value number 当前炮值
_G.CLFRGunValueChangeNtf = _G.class(ProtocolBase)
function _G.CLFRGunValueChangeNtf:__init(t)
    _G.CLFRGunValueChangeNtf.super:__init(t)
    self.mid = 11
    self.pid = 17
end
function _G.CLFRGunValueChangeNtf:deserialize(br)
    self.seat_id = br:ReadInt32()
    self.gun_value = br:ReadInt64()
end
function _G.CLFRGunValueChangeNtf:serialize(bw)
    bw:WriteInt32(self.seat_id)
    bw:WriteInt64(self.gun_value)
end

--发炮请求
--angle number 发炮角度
--lock_fish number 锁定的鱼Id
--multiple number 倍击倍数，如果该字段大于1则优先使用该字段当做倍击倍数
_G.CLFRShootReq = _G.class(ProtocolBase)
function _G.CLFRShootReq:__init(t)
    _G.CLFRShootReq.super:__init(t)
    self.mid = 11
    self.pid = 18
end
function _G.CLFRShootReq:deserialize(br)
    self.angle = br:ReadInt32()
    self.lock_fish = br:ReadInt32()
    self.multiple = br:ReadUInt8()
end
function _G.CLFRShootReq:serialize(bw)
    bw:WriteInt32(self.angle)
    bw:WriteInt32(self.lock_fish)
    bw:WriteUInt8(self.multiple)
end

--发炮回应
--errcode number 0成功 1金币不足 2子弹数量不足 3系统错误
--currency_delta number 金币变化量
--bind_currency_delta number 绑定金币变化量
--bullet_id number 子弹Id
--lock_fish number 锁定的鱼Id
--multiple number 倍击倍数
--energy_delta number 积累的能量变化
_G.CLFRShootAck = _G.class(ProtocolBase)
function _G.CLFRShootAck:__init(t)
    _G.CLFRShootAck.super:__init(t)
    self.mid = 11
    self.pid = 19
end
function _G.CLFRShootAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.currency_delta = br:ReadInt64()
    self.bind_currency_delta = br:ReadInt64()
    self.bullet_id = br:ReadInt32()
    self.lock_fish = br:ReadInt32()
    self.multiple = br:ReadUInt8()
    self.energy_delta = br:ReadInt64()
end
function _G.CLFRShootAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt64(self.currency_delta)
    bw:WriteInt64(self.bind_currency_delta)
    bw:WriteInt32(self.bullet_id)
    bw:WriteInt32(self.lock_fish)
    bw:WriteUInt8(self.multiple)
    bw:WriteInt64(self.energy_delta)
end

--发炮通知
--seat_id number 座位Id
--currency_delta number 金币变化量
--bind_currency_delta number 绑定金币变化量
--bullet_id number 子弹Id
--angle number 发炮角度
--lock_fish number 锁定的鱼Id
--multiple number 倍击倍数
_G.CLFRShootNtf = _G.class(ProtocolBase)
function _G.CLFRShootNtf:__init(t)
    _G.CLFRShootNtf.super:__init(t)
    self.mid = 11
    self.pid = 20
end
function _G.CLFRShootNtf:deserialize(br)
    self.seat_id = br:ReadInt32()
    self.currency_delta = br:ReadInt64()
    self.bind_currency_delta = br:ReadInt64()
    self.bullet_id = br:ReadInt32()
    self.angle = br:ReadInt32()
    self.lock_fish = br:ReadInt32()
    self.multiple = br:ReadUInt8()
end
function _G.CLFRShootNtf:serialize(bw)
    bw:WriteInt32(self.seat_id)
    bw:WriteInt64(self.currency_delta)
    bw:WriteInt64(self.bind_currency_delta)
    bw:WriteInt32(self.bullet_id)
    bw:WriteInt32(self.angle)
    bw:WriteInt32(self.lock_fish)
    bw:WriteUInt8(self.multiple)
end

--发炮信息
--angle number 发炮角度
--lock_fish number 锁定的鱼Id
local function CLFRShootInfo_deserialize(br)
    local r = {}
    r.angle = br:ReadInt32()
    r.lock_fish = br:ReadInt32()
    return r
end
local function CLFRShootInfo_serialize(t, bw)
    bw:WriteInt32(t.angle)
    bw:WriteInt32(t.lock_fish)
end

--子弹信息
--bullet_id number 子弹Id
--angle number 发炮角度
--lock_fish number 锁定的鱼Id
local function CLFRBulletInfo_deserialize(br)
    local r = {}
    r.bullet_id = br:ReadInt32()
    r.angle = br:ReadInt32()
    r.lock_fish = br:ReadInt32()
    return r
end
local function CLFRBulletInfo_serialize(t, bw)
    bw:WriteInt32(t.bullet_id)
    bw:WriteInt32(t.angle)
    bw:WriteInt32(t.lock_fish)
end

--多炮台同时发炮请求
--shoot_len number 发炮数量
--shoot_array ShootInfo[10] 发炮信息数组
--multiple number 倍击倍数，如果该字段大于1则优先使用该字段当做倍击倍数
_G.CLFRMultiShootReq = _G.class(ProtocolBase)
function _G.CLFRMultiShootReq:__init(t)
    _G.CLFRMultiShootReq.super:__init(t)
    self.mid = 11
    self.pid = 21
end
function _G.CLFRMultiShootReq:deserialize(br)
    self.shoot_len = br:ReadInt8()
    self.shoot_array = {}
    for i=1,self.shoot_len do
        self.shoot_array[i] = CLFRShootInfo_deserialize(br)
    end
    self.multiple = br:ReadUInt8()
end
function _G.CLFRMultiShootReq:serialize(bw)
    assert(not self.shoot_array or #self.shoot_array <= 10, "CLFRMultiShootReq.shoot_array数组长度超过规定限制! 期望:10 实际:" .. #self.shoot_array)
    bw:WriteInt8(#(self.shoot_array or {}))
    for i=1,#(self.shoot_array or {}) do
        CLFRShootInfo_serialize(self.shoot_array[i], bw)
    end
    bw:WriteUInt8(self.multiple)
end

--多炮台同时发炮回应
--errcode number 0成功 1金币不足 2子弹数量不足 3系统错误
--currency_delta number 金币变化量
--bind_currency_delta number 绑定金币变化量
--bullet_len number 子弹数量
--bullet_array BulletInfo[10] 子弹信息数组
--multiple number 倍击倍数
--energy_delta number 积累的能量变化
_G.CLFRMultiShootAck = _G.class(ProtocolBase)
function _G.CLFRMultiShootAck:__init(t)
    _G.CLFRMultiShootAck.super:__init(t)
    self.mid = 11
    self.pid = 22
end
function _G.CLFRMultiShootAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.currency_delta = br:ReadInt64()
    self.bind_currency_delta = br:ReadInt64()
    self.bullet_len = br:ReadInt8()
    self.bullet_array = {}
    for i=1,self.bullet_len do
        self.bullet_array[i] = CLFRBulletInfo_deserialize(br)
    end
    self.multiple = br:ReadUInt8()
    self.energy_delta = br:ReadInt64()
end
function _G.CLFRMultiShootAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt64(self.currency_delta)
    bw:WriteInt64(self.bind_currency_delta)
    assert(not self.bullet_array or #self.bullet_array <= 10, "CLFRMultiShootAck.bullet_array数组长度超过规定限制! 期望:10 实际:" .. #self.bullet_array)
    bw:WriteInt8(#(self.bullet_array or {}))
    for i=1,#(self.bullet_array or {}) do
        CLFRBulletInfo_serialize(self.bullet_array[i], bw)
    end
    bw:WriteUInt8(self.multiple)
    bw:WriteInt64(self.energy_delta)
end

--多炮台同时发炮通知
--seat_id number 座位Id
--currency_delta number 金币变化量
--bind_currency_delta number 绑定金币变化量
--bullet_len number 子弹数量
--bullet_array BulletInfo[10] 子弹信息数组
--multiple number 倍击倍数
_G.CLFRMultiShootNtf = _G.class(ProtocolBase)
function _G.CLFRMultiShootNtf:__init(t)
    _G.CLFRMultiShootNtf.super:__init(t)
    self.mid = 11
    self.pid = 23
end
function _G.CLFRMultiShootNtf:deserialize(br)
    self.seat_id = br:ReadInt32()
    self.currency_delta = br:ReadInt64()
    self.bind_currency_delta = br:ReadInt64()
    self.bullet_len = br:ReadInt8()
    self.bullet_array = {}
    for i=1,self.bullet_len do
        self.bullet_array[i] = CLFRBulletInfo_deserialize(br)
    end
    self.multiple = br:ReadUInt8()
end
function _G.CLFRMultiShootNtf:serialize(bw)
    bw:WriteInt32(self.seat_id)
    bw:WriteInt64(self.currency_delta)
    bw:WriteInt64(self.bind_currency_delta)
    assert(not self.bullet_array or #self.bullet_array <= 10, "CLFRMultiShootNtf.bullet_array数组长度超过规定限制! 期望:10 实际:" .. #self.bullet_array)
    bw:WriteInt8(#(self.bullet_array or {}))
    for i=1,#(self.bullet_array or {}) do
        CLFRBulletInfo_serialize(self.bullet_array[i], bw)
    end
    bw:WriteUInt8(self.multiple)
end

--命中请求
--bullet_id number 子弹Id
--fish_id number 鱼Id
--related_fish_len number 关联的鱼数组长度，目前用于黑洞鱼
--related_fish_array number[30] 关联的鱼Id数组，目前用于黑洞鱼
_G.CLFRHitReq = _G.class(ProtocolBase)
function _G.CLFRHitReq:__init(t)
    _G.CLFRHitReq.super:__init(t)
    self.mid = 11
    self.pid = 24
end
function _G.CLFRHitReq:deserialize(br)
    self.bullet_id = br:ReadInt32()
    self.fish_id = br:ReadInt32()
    self.related_fish_len = br:ReadInt8()
    self.related_fish_array = {}
    for i=1,self.related_fish_len do
        self.related_fish_array[i] = br:ReadInt32()
    end
end
function _G.CLFRHitReq:serialize(bw)
    bw:WriteInt32(self.bullet_id)
    bw:WriteInt32(self.fish_id)
    assert(not self.related_fish_array or #self.related_fish_array <= 30, "CLFRHitReq.related_fish_array数组长度超过规定限制! 期望:30 实际:" .. #self.related_fish_array)
    bw:WriteInt8(#(self.related_fish_array or {}))
    for i=1,#(self.related_fish_array or {}) do
        bw:WriteInt32(self.related_fish_array[i])
    end
end

--命中回应
--errcode number 0成功 1系统错误
--is_boom number 是否打爆 0否 1是
--currency_delta number 金币变化量
--diamond_delta number 钻石变化量
--match_integral_delta number 比赛积分变化量
--item_len number 掉落物品数量
--items ItemInfo[100] 掉落的物品数组
--related_fish_len number 其他关联的死亡鱼的数组长度
--related_fish_array number[100] 其他关联的死亡鱼Id的数组
--related_remove_reason number 移除原因 0未知 1炸弹鱼移除 2闪电鱼移除 3鱼王移除 4黑洞鱼移除 5一网打尽
--energy_delta number 积累的能量变化
_G.CLFRHitAck = _G.class(ProtocolBase)
function _G.CLFRHitAck:__init(t)
    _G.CLFRHitAck.super:__init(t)
    self.mid = 11
    self.pid = 25
end
function _G.CLFRHitAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.is_boom = br:ReadInt8()
    self.currency_delta = br:ReadInt64()
    self.diamond_delta = br:ReadInt64()
    self.match_integral_delta = br:ReadInt32()
    self.item_len = br:ReadInt8()
    self.items = {}
    for i=1,self.item_len do
        self.items[i] = CLFRItemInfo_deserialize(br)
    end
    self.related_fish_len = br:ReadInt8()
    self.related_fish_array = {}
    for i=1,self.related_fish_len do
        self.related_fish_array[i] = br:ReadInt32()
    end
    self.related_remove_reason = br:ReadInt8()
    self.energy_delta = br:ReadInt64()
end
function _G.CLFRHitAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt8(self.is_boom)
    bw:WriteInt64(self.currency_delta)
    bw:WriteInt64(self.diamond_delta)
    bw:WriteInt32(self.match_integral_delta)
    assert(not self.items or #self.items <= 100, "CLFRHitAck.items数组长度超过规定限制! 期望:100 实际:" .. #self.items)
    bw:WriteInt8(#(self.items or {}))
    for i=1,#(self.items or {}) do
        CLFRItemInfo_serialize(self.items[i], bw)
    end
    assert(not self.related_fish_array or #self.related_fish_array <= 100, "CLFRHitAck.related_fish_array数组长度超过规定限制! 期望:100 实际:" .. #self.related_fish_array)
    bw:WriteInt8(#(self.related_fish_array or {}))
    for i=1,#(self.related_fish_array or {}) do
        bw:WriteInt32(self.related_fish_array[i])
    end
    bw:WriteInt8(self.related_remove_reason)
    bw:WriteInt64(self.energy_delta)
end

--命中通知
--seat_id number 座位Id
--bullet_id number 子弹Id
--fish_id number 鱼Id
--is_boom number 是否打爆 0否 1是
--currency_delta number 金币变化量
--diamond_delta number 钻石变化量
--match_integral_delta number 比赛积分变化量
--item_len number 掉落物品数量
--items ItemInfo[100] 掉落的物品数组
--related_fish_len number 其他关联的死亡鱼的数组长度
--related_fish_array number[100] 其他关联的死亡鱼Id的数组
--related_remove_reason number 移除原因 0未知 1炸弹鱼移除 2闪电鱼移除 3鱼王移除 4黑洞鱼移除 5一网打尽
_G.CLFRHitNtf = _G.class(ProtocolBase)
function _G.CLFRHitNtf:__init(t)
    _G.CLFRHitNtf.super:__init(t)
    self.mid = 11
    self.pid = 26
end
function _G.CLFRHitNtf:deserialize(br)
    self.seat_id = br:ReadInt32()
    self.bullet_id = br:ReadInt32()
    self.fish_id = br:ReadInt32()
    self.is_boom = br:ReadInt8()
    self.currency_delta = br:ReadInt64()
    self.diamond_delta = br:ReadInt64()
    self.match_integral_delta = br:ReadInt32()
    self.item_len = br:ReadInt8()
    self.items = {}
    for i=1,self.item_len do
        self.items[i] = CLFRItemInfo_deserialize(br)
    end
    self.related_fish_len = br:ReadInt8()
    self.related_fish_array = {}
    for i=1,self.related_fish_len do
        self.related_fish_array[i] = br:ReadInt32()
    end
    self.related_remove_reason = br:ReadInt8()
end
function _G.CLFRHitNtf:serialize(bw)
    bw:WriteInt32(self.seat_id)
    bw:WriteInt32(self.bullet_id)
    bw:WriteInt32(self.fish_id)
    bw:WriteInt8(self.is_boom)
    bw:WriteInt64(self.currency_delta)
    bw:WriteInt64(self.diamond_delta)
    bw:WriteInt32(self.match_integral_delta)
    assert(not self.items or #self.items <= 100, "CLFRHitNtf.items数组长度超过规定限制! 期望:100 实际:" .. #self.items)
    bw:WriteInt8(#(self.items or {}))
    for i=1,#(self.items or {}) do
        CLFRItemInfo_serialize(self.items[i], bw)
    end
    assert(not self.related_fish_array or #self.related_fish_array <= 100, "CLFRHitNtf.related_fish_array数组长度超过规定限制! 期望:100 实际:" .. #self.related_fish_array)
    bw:WriteInt8(#(self.related_fish_array or {}))
    for i=1,#(self.related_fish_array or {}) do
        bw:WriteInt32(self.related_fish_array[i])
    end
    bw:WriteInt8(self.related_remove_reason)
end

--机器人子弹碰撞通知
--bullet_id number 子弹Id
--fish_id number 鱼Id
_G.CLFRRobotHitRpt = _G.class(ProtocolBase)
function _G.CLFRRobotHitRpt:__init(t)
    _G.CLFRRobotHitRpt.super:__init(t)
    self.mid = 11
    self.pid = 27
end
function _G.CLFRRobotHitRpt:deserialize(br)
    self.bullet_id = br:ReadInt32()
    self.fish_id = br:ReadInt32()
end
function _G.CLFRRobotHitRpt:serialize(bw)
    bw:WriteInt32(self.bullet_id)
    bw:WriteInt32(self.fish_id)
end

--boss出现通知
--boss_info BossAppearInfo 世界boss信息
_G.CLFRBossAppearNtf = _G.class(ProtocolBase)
function _G.CLFRBossAppearNtf:__init(t)
    _G.CLFRBossAppearNtf.super:__init(t)
    self.mid = 11
    self.pid = 28
end
function _G.CLFRBossAppearNtf:deserialize(br)
    self.boss_info = CLFRBossAppearInfo_deserialize(br)
end
function _G.CLFRBossAppearNtf:serialize(bw)
    CLFRBossAppearInfo_serialize(self.boss_info, bw)
end

--boss命中请求
--bullet_id number 子弹Id
_G.CLFRBossHitReq = _G.class(ProtocolBase)
function _G.CLFRBossHitReq:__init(t)
    _G.CLFRBossHitReq.super:__init(t)
    self.mid = 11
    self.pid = 29
end
function _G.CLFRBossHitReq:deserialize(br)
    self.bullet_id = br:ReadInt32()
end
function _G.CLFRBossHitReq:serialize(bw)
    bw:WriteInt32(self.bullet_id)
end

--boss命中回应
--errcode number 0成功 1系统错误
--is_boom number 是否爆金 0miss 1爆金 2死亡
--currency_delta number 金币变化量
--energy_delta number 积累的能量变化
_G.CLFRBossHitAck = _G.class(ProtocolBase)
function _G.CLFRBossHitAck:__init(t)
    _G.CLFRBossHitAck.super:__init(t)
    self.mid = 11
    self.pid = 30
end
function _G.CLFRBossHitAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.is_boom = br:ReadInt8()
    self.currency_delta = br:ReadInt64()
    self.energy_delta = br:ReadInt64()
end
function _G.CLFRBossHitAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt8(self.is_boom)
    bw:WriteInt64(self.currency_delta)
    bw:WriteInt64(self.energy_delta)
end

--boss命中通知
--seat_id number 座位Id
--bullet_id number 子弹Id
--is_boom number 是否爆金 0miss 1爆金 2死亡
--currency_delta number 金币变化量
_G.CLFRBossHitNtf = _G.class(ProtocolBase)
function _G.CLFRBossHitNtf:__init(t)
    _G.CLFRBossHitNtf.super:__init(t)
    self.mid = 11
    self.pid = 31
end
function _G.CLFRBossHitNtf:deserialize(br)
    self.seat_id = br:ReadInt32()
    self.bullet_id = br:ReadInt32()
    self.is_boom = br:ReadInt8()
    self.currency_delta = br:ReadInt64()
end
function _G.CLFRBossHitNtf:serialize(bw)
    bw:WriteInt32(self.seat_id)
    bw:WriteInt32(self.bullet_id)
    bw:WriteInt8(self.is_boom)
    bw:WriteInt64(self.currency_delta)
end

--boss防御罩血量变化通知
--left_blood number 防御罩剩余血量百分比0-100，该值等于0时boss处于落体状态
_G.CLFRBossDefenceBloodChangedNtf = _G.class(ProtocolBase)
function _G.CLFRBossDefenceBloodChangedNtf:__init(t)
    _G.CLFRBossDefenceBloodChangedNtf.super:__init(t)
    self.mid = 11
    self.pid = 32
end
function _G.CLFRBossDefenceBloodChangedNtf:deserialize(br)
    self.left_blood = br:ReadInt8()
end
function _G.CLFRBossDefenceBloodChangedNtf:serialize(bw)
    bw:WriteInt8(self.left_blood)
end

--世界boss被杀死通知
--rank number 你的排名，0代表未上榜
--gain_currency number 获得的金币数量，仅用于显示
--item_length number 奖励物品数组长度
--item_array ItemInfo[100] 奖励物品数组
_G.CLFRBossKilledNtf = _G.class(ProtocolBase)
function _G.CLFRBossKilledNtf:__init(t)
    _G.CLFRBossKilledNtf.super:__init(t)
    self.mid = 11
    self.pid = 33
end
function _G.CLFRBossKilledNtf:deserialize(br)
    self.rank = br:ReadInt32()
    self.gain_currency = br:ReadInt64()
    self.item_length = br:ReadInt8()
    self.item_array = {}
    for i=1,self.item_length do
        self.item_array[i] = CLFRItemInfo_deserialize(br)
    end
end
function _G.CLFRBossKilledNtf:serialize(bw)
    bw:WriteInt32(self.rank)
    bw:WriteInt64(self.gain_currency)
    assert(not self.item_array or #self.item_array <= 100, "CLFRBossKilledNtf.item_array数组长度超过规定限制! 期望:100 实际:" .. #self.item_array)
    bw:WriteInt8(#(self.item_array or {}))
    for i=1,#(self.item_array or {}) do
        CLFRItemInfo_serialize(self.item_array[i], bw)
    end
end

--Boss排行榜玩家信息
--user_id number 玩家Id
--nickname string[32] 昵称
--gender number 性别 0保密 1男 2女
--head number 头像Id
--head_frame number 头像框Id
--level number 玩家等级
--vip_level number 玩家vip等级
--rank_value number 打boss过程中所获得的金币数量
local function CLFRBossRankPlayerInfo_deserialize(br)
    local r = {}
    r.user_id = br:ReadInt32()
    r.nickname = br:ReadString()
    r.gender = br:ReadInt32()
    r.head = br:ReadInt32()
    r.head_frame = br:ReadInt32()
    r.level = br:ReadInt32()
    r.vip_level = br:ReadInt32()
    r.rank_value = br:ReadInt64()
    return r
end
local function CLFRBossRankPlayerInfo_serialize(t, bw)
    bw:WriteInt32(t.user_id)
    bw:WriteString(t.nickname, 32)
    bw:WriteInt32(t.gender)
    bw:WriteInt32(t.head)
    bw:WriteInt32(t.head_frame)
    bw:WriteInt32(t.level)
    bw:WriteInt32(t.vip_level)
    bw:WriteInt64(t.rank_value)
end

--获取Boss排行榜请求
_G.CLFRBossRankReq = _G.class(ProtocolBase)
function _G.CLFRBossRankReq:__init(t)
    _G.CLFRBossRankReq.super:__init(t)
    self.mid = 11
    self.pid = 34
end

--获取Boss排行榜回应
--errcode number 0成功 1系统错误
--rank_len number 数组长度
--rank_rows BossRankPlayerInfo[100] 排行榜数据数组
--killer_user_id number 击杀boss的玩家Id
--boss_config_id number 世界boss的模板Id
_G.CLFRBossRankAck = _G.class(ProtocolBase)
function _G.CLFRBossRankAck:__init(t)
    _G.CLFRBossRankAck.super:__init(t)
    self.mid = 11
    self.pid = 35
end
function _G.CLFRBossRankAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.rank_len = br:ReadInt32()
    self.rank_rows = {}
    for i=1,self.rank_len do
        self.rank_rows[i] = CLFRBossRankPlayerInfo_deserialize(br)
    end
    self.killer_user_id = br:ReadInt32()
    self.boss_config_id = br:ReadInt32()
end
function _G.CLFRBossRankAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.rank_rows or #self.rank_rows <= 100, "CLFRBossRankAck.rank_rows数组长度超过规定限制! 期望:100 实际:" .. #self.rank_rows)
    bw:WriteInt32(#(self.rank_rows or {}))
    for i=1,#(self.rank_rows or {}) do
        CLFRBossRankPlayerInfo_serialize(self.rank_rows[i], bw)
    end
    bw:WriteInt32(self.killer_user_id)
    bw:WriteInt32(self.boss_config_id)
end

--鱼被移除通知
--seat_id number 关联的座位Id
--len number 被移除数量
--fish_ids number[100] 被移除的鱼数组
--reason number 移除原因 1使用弹头道具
--delta number 金币变化量
_G.CLFRFishRemoveNtf = _G.class(ProtocolBase)
function _G.CLFRFishRemoveNtf:__init(t)
    _G.CLFRFishRemoveNtf.super:__init(t)
    self.mid = 11
    self.pid = 36
end
function _G.CLFRFishRemoveNtf:deserialize(br)
    self.seat_id = br:ReadInt32()
    self.len = br:ReadInt32()
    self.fish_ids = {}
    for i=1,self.len do
        self.fish_ids[i] = br:ReadInt32()
    end
    self.reason = br:ReadInt32()
    self.delta = br:ReadInt64()
end
function _G.CLFRFishRemoveNtf:serialize(bw)
    bw:WriteInt32(self.seat_id)
    assert(not self.fish_ids or #self.fish_ids <= 100, "CLFRFishRemoveNtf.fish_ids数组长度超过规定限制! 期望:100 实际:" .. #self.fish_ids)
    bw:WriteInt32(#(self.fish_ids or {}))
    for i=1,#(self.fish_ids or {}) do
        bw:WriteInt32(self.fish_ids[i])
    end
    bw:WriteInt32(self.reason)
    bw:WriteInt64(self.delta)
end

--显示老虎机转盘
--seat_id number 关联的座位Id
--fish_config_id number 鱼的配置Id
--delta number 金币变化量
--multiple number 显示倍数
_G.CLFRShowWheel1Ntf = _G.class(ProtocolBase)
function _G.CLFRShowWheel1Ntf:__init(t)
    _G.CLFRShowWheel1Ntf.super:__init(t)
    self.mid = 11
    self.pid = 37
end
function _G.CLFRShowWheel1Ntf:deserialize(br)
    self.seat_id = br:ReadInt32()
    self.fish_config_id = br:ReadInt32()
    self.delta = br:ReadInt64()
    self.multiple = br:ReadInt32()
end
function _G.CLFRShowWheel1Ntf:serialize(bw)
    bw:WriteInt32(self.seat_id)
    bw:WriteInt32(self.fish_config_id)
    bw:WriteInt64(self.delta)
    bw:WriteInt32(self.multiple)
end

--显示双轮盘转盘
--seat_id number 关联的座位Id
--fish_config_id number 鱼的配置Id
--delta number 金币变化量
--multiple number 鱼倍率
--degree number 系数万分比，使用时需除以10000
_G.CLFRShowWheel2Ntf = _G.class(ProtocolBase)
function _G.CLFRShowWheel2Ntf:__init(t)
    _G.CLFRShowWheel2Ntf.super:__init(t)
    self.mid = 11
    self.pid = 38
end
function _G.CLFRShowWheel2Ntf:deserialize(br)
    self.seat_id = br:ReadInt32()
    self.fish_config_id = br:ReadInt32()
    self.delta = br:ReadInt64()
    self.multiple = br:ReadInt32()
    self.degree = br:ReadInt32()
end
function _G.CLFRShowWheel2Ntf:serialize(bw)
    bw:WriteInt32(self.seat_id)
    bw:WriteInt32(self.fish_config_id)
    bw:WriteInt64(self.delta)
    bw:WriteInt32(self.multiple)
    bw:WriteInt32(self.degree)
end

--座位资源变化通知
--seat_id number 座位Id
--diamond_delta number 钻石变化
--currency_delta number 金币变化量
--bind_currency_delta number 绑定金币变化量
--reason number 变化原因，详见:resource_log_reason_type
_G.CLFRSeatResourceChangedNtf = _G.class(ProtocolBase)
function _G.CLFRSeatResourceChangedNtf:__init(t)
    _G.CLFRSeatResourceChangedNtf.super:__init(t)
    self.mid = 11
    self.pid = 39
end
function _G.CLFRSeatResourceChangedNtf:deserialize(br)
    self.seat_id = br:ReadInt32()
    self.diamond_delta = br:ReadInt64()
    self.currency_delta = br:ReadInt64()
    self.bind_currency_delta = br:ReadInt64()
    self.reason = br:ReadInt32()
end
function _G.CLFRSeatResourceChangedNtf:serialize(bw)
    bw:WriteInt32(self.seat_id)
    bw:WriteInt64(self.diamond_delta)
    bw:WriteInt64(self.currency_delta)
    bw:WriteInt64(self.bind_currency_delta)
    bw:WriteInt32(self.reason)
end

--座位使用道具通知
--seat_id number 座位Id
--item ItemInfo 使用的物品
_G.CLFRSeatItemUsedNtf = _G.class(ProtocolBase)
function _G.CLFRSeatItemUsedNtf:__init(t)
    _G.CLFRSeatItemUsedNtf.super:__init(t)
    self.mid = 11
    self.pid = 40
end
function _G.CLFRSeatItemUsedNtf:deserialize(br)
    self.seat_id = br:ReadInt32()
    self.item = CLFRItemInfo_deserialize(br)
end
function _G.CLFRSeatItemUsedNtf:serialize(bw)
    bw:WriteInt32(self.seat_id)
    CLFRItemInfo_serialize(self.item, bw)
end

--炮台解锁请求
_G.CLFRGunUnlockReq = _G.class(ProtocolBase)
function _G.CLFRGunUnlockReq:__init(t)
    _G.CLFRGunUnlockReq.super:__init(t)
    self.mid = 11
    self.pid = 41
end

--炮台解锁回应
--errcode number 0成功 1资源不足 2道具不足 3系统错误
--max_gun_value number 解锁后最大炮值
--diamond_delta number 钻石变化量
--currency_delta number 金币变化量
--bind_currency_delta number 绑定金币变化量
_G.CLFRGunUnlockAck = _G.class(ProtocolBase)
function _G.CLFRGunUnlockAck:__init(t)
    _G.CLFRGunUnlockAck.super:__init(t)
    self.mid = 11
    self.pid = 42
end
function _G.CLFRGunUnlockAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.max_gun_value = br:ReadInt64()
    self.diamond_delta = br:ReadInt64()
    self.currency_delta = br:ReadInt64()
    self.bind_currency_delta = br:ReadInt64()
end
function _G.CLFRGunUnlockAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt64(self.max_gun_value)
    bw:WriteInt64(self.diamond_delta)
    bw:WriteInt64(self.currency_delta)
    bw:WriteInt64(self.bind_currency_delta)
end

--奖金鱼池发生变化通知
--bonus_pool number 最新奖池金币数量
--bonus_count number 最新击杀奖金鱼数量
_G.CLFRBonusPoolChangedNtf = _G.class(ProtocolBase)
function _G.CLFRBonusPoolChangedNtf:__init(t)
    _G.CLFRBonusPoolChangedNtf.super:__init(t)
    self.mid = 11
    self.pid = 43
end
function _G.CLFRBonusPoolChangedNtf:deserialize(br)
    self.bonus_pool = br:ReadInt64()
    self.bonus_count = br:ReadInt32()
end
function _G.CLFRBonusPoolChangedNtf:serialize(bw)
    bw:WriteInt64(self.bonus_pool)
    bw:WriteInt32(self.bonus_count)
end

--奖金抽奖请求
_G.CLFRBonusWheelReq = _G.class(ProtocolBase)
function _G.CLFRBonusWheelReq:__init(t)
    _G.CLFRBonusWheelReq.super:__init(t)
    self.mid = 11
    self.pid = 44
end

--奖金抽奖回应
--errcode number 0成功 1条件不足 2系统错误
--item ItemInfo 奖励的道具信息
_G.CLFRBonusWheelAck = _G.class(ProtocolBase)
function _G.CLFRBonusWheelAck:__init(t)
    _G.CLFRBonusWheelAck.super:__init(t)
    self.mid = 11
    self.pid = 45
end
function _G.CLFRBonusWheelAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.item = CLFRItemInfo_deserialize(br)
end
function _G.CLFRBonusWheelAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    CLFRItemInfo_serialize(self.item, bw)
end

--添加分身效果通知
--seat_id number 座位Id：0~3
_G.CLFRAddEffectShadowNtf = _G.class(ProtocolBase)
function _G.CLFRAddEffectShadowNtf:__init(t)
    _G.CLFRAddEffectShadowNtf.super:__init(t)
    self.mid = 11
    self.pid = 46
end
function _G.CLFRAddEffectShadowNtf:deserialize(br)
    self.seat_id = br:ReadInt32()
end
function _G.CLFRAddEffectShadowNtf:serialize(bw)
    bw:WriteInt32(self.seat_id)
end

--召唤鱼通知
--seat_id number 座位Id：0~3
--fish FishAppearInfo 新召唤的鱼
_G.CLFRFishSummonNtf = _G.class(ProtocolBase)
function _G.CLFRFishSummonNtf:__init(t)
    _G.CLFRFishSummonNtf.super:__init(t)
    self.mid = 11
    self.pid = 47
end
function _G.CLFRFishSummonNtf:deserialize(br)
    self.seat_id = br:ReadInt32()
    self.fish = CLFRFishAppearInfo_deserialize(br)
end
function _G.CLFRFishSummonNtf:serialize(bw)
    bw:WriteInt32(self.seat_id)
    CLFRFishAppearInfo_serialize(self.fish, bw)
end

--炮台切换请求
--gun_id number 新的炮台Id
_G.CLFRGunSwitchReq = _G.class(ProtocolBase)
function _G.CLFRGunSwitchReq:__init(t)
    _G.CLFRGunSwitchReq.super:__init(t)
    self.mid = 11
    self.pid = 48
end
function _G.CLFRGunSwitchReq:deserialize(br)
    self.gun_id = br:ReadInt32()
end
function _G.CLFRGunSwitchReq:serialize(bw)
    bw:WriteInt32(self.gun_id)
end

--炮台切换回应
--errcode number 0成功 1vip等级不足 2系统错误
_G.CLFRGunSwitchAck = _G.class(ProtocolBase)
function _G.CLFRGunSwitchAck:__init(t)
    _G.CLFRGunSwitchAck.super:__init(t)
    self.mid = 11
    self.pid = 49
end
function _G.CLFRGunSwitchAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLFRGunSwitchAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--炮台切换通知
--seat_id number 座位Id：0~3
--gun_id number 炮台Id
_G.CLFRGunSwitchNtf = _G.class(ProtocolBase)
function _G.CLFRGunSwitchNtf:__init(t)
    _G.CLFRGunSwitchNtf.super:__init(t)
    self.mid = 11
    self.pid = 50
end
function _G.CLFRGunSwitchNtf:deserialize(br)
    self.seat_id = br:ReadInt32()
    self.gun_id = br:ReadInt32()
end
function _G.CLFRGunSwitchNtf:serialize(bw)
    bw:WriteInt32(self.seat_id)
    bw:WriteInt32(self.gun_id)
end

--申请鱼潮来临请求
_G.CLFRFishTideForTestReq = _G.class(ProtocolBase)
function _G.CLFRFishTideForTestReq:__init(t)
    _G.CLFRFishTideForTestReq.super:__init(t)
    self.mid = 11
    self.pid = 51
end

--申请鱼潮来临回应
--errcode number 0成功 1还未结束呢 2你不在房间中
_G.CLFRFishTideForTestAck = _G.class(ProtocolBase)
function _G.CLFRFishTideForTestAck:__init(t)
    _G.CLFRFishTideForTestAck.super:__init(t)
    self.mid = 11
    self.pid = 52
end
function _G.CLFRFishTideForTestAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLFRFishTideForTestAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--查询房间内获取到的积分请求
_G.CLFRIntegralGainQueryReq = _G.class(ProtocolBase)
function _G.CLFRIntegralGainQueryReq:__init(t)
    _G.CLFRIntegralGainQueryReq.super:__init(t)
    self.mid = 11
    self.pid = 53
end

--查询房间内获取到的积分回应
--gain_integral number 房间内获取到的积分
_G.CLFRIntegralGainQueryAck = _G.class(ProtocolBase)
function _G.CLFRIntegralGainQueryAck:__init(t)
    _G.CLFRIntegralGainQueryAck.super:__init(t)
    self.mid = 11
    self.pid = 54
end
function _G.CLFRIntegralGainQueryAck:deserialize(br)
    self.gain_integral = br:ReadInt64()
end
function _G.CLFRIntegralGainQueryAck:serialize(bw)
    bw:WriteInt64(self.gain_integral)
end

--使用弹头锁定鱼请求
--item_id number 弹头物品Id
--item_sub_id number 弹头物品子Id
--item_count number 使用弹头的数量
--fish_id number 弹头想要炸死的鱼Id
_G.CLFRWarheadLockReq = _G.class(ProtocolBase)
function _G.CLFRWarheadLockReq:__init(t)
    _G.CLFRWarheadLockReq.super:__init(t)
    self.mid = 11
    self.pid = 55
end
function _G.CLFRWarheadLockReq:deserialize(br)
    self.item_id = br:ReadInt32()
    self.item_sub_id = br:ReadInt32()
    self.item_count = br:ReadInt32()
    self.fish_id = br:ReadInt32()
end
function _G.CLFRWarheadLockReq:serialize(bw)
    bw:WriteInt32(self.item_id)
    bw:WriteInt32(self.item_sub_id)
    bw:WriteInt32(self.item_count)
    bw:WriteInt32(self.fish_id)
end

--使用弹头锁定鱼回应
--errcode number 0成功 1物品不足 2已使用弹头锁定过 3鱼不存在 4该鱼不能被炸
_G.CLFRWarheadLockAck = _G.class(ProtocolBase)
function _G.CLFRWarheadLockAck:__init(t)
    _G.CLFRWarheadLockAck.super:__init(t)
    self.mid = 11
    self.pid = 56
end
function _G.CLFRWarheadLockAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLFRWarheadLockAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--弹头锁定通知
--seat_id number 炮台位置：0~3
--item_id number 弹头物品Id
--item_sub_id number 弹头物品子Id
--item_count number 使用弹头的数量
--fish_id number 弹头命中的鱼Id
_G.CLFRWarheadLockNtf = _G.class(ProtocolBase)
function _G.CLFRWarheadLockNtf:__init(t)
    _G.CLFRWarheadLockNtf.super:__init(t)
    self.mid = 11
    self.pid = 57
end
function _G.CLFRWarheadLockNtf:deserialize(br)
    self.seat_id = br:ReadInt32()
    self.item_id = br:ReadInt32()
    self.item_sub_id = br:ReadInt32()
    self.item_count = br:ReadInt32()
    self.fish_id = br:ReadInt32()
end
function _G.CLFRWarheadLockNtf:serialize(bw)
    bw:WriteInt32(self.seat_id)
    bw:WriteInt32(self.item_id)
    bw:WriteInt32(self.item_sub_id)
    bw:WriteInt32(self.item_count)
    bw:WriteInt32(self.fish_id)
end

--弹头爆炸请求
_G.CLFRWarheadBoomReq = _G.class(ProtocolBase)
function _G.CLFRWarheadBoomReq:__init(t)
    _G.CLFRWarheadBoomReq.super:__init(t)
    self.mid = 11
    self.pid = 58
end

--弹头爆炸回应
--errcode number 0成功 1尚未使用弹头锁定过 2该鱼已不存在 3弹头配置错误
_G.CLFRWarheadBoomAck = _G.class(ProtocolBase)
function _G.CLFRWarheadBoomAck:__init(t)
    _G.CLFRWarheadBoomAck.super:__init(t)
    self.mid = 11
    self.pid = 59
end
function _G.CLFRWarheadBoomAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLFRWarheadBoomAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--弹头爆炸通知
--seat_id number 炮台位置：0~3
--item_id number 弹头物品Id
--item_sub_id number 弹头物品子Id
--item_count number 使用弹头的数量
--fish_id number 弹头命中的鱼Id
--boom_result number 爆炸结果 0成功炸死鱼 1鱼已不存在炸失败 2弹头配置错误
_G.CLFRWarheadBoomNtf = _G.class(ProtocolBase)
function _G.CLFRWarheadBoomNtf:__init(t)
    _G.CLFRWarheadBoomNtf.super:__init(t)
    self.mid = 11
    self.pid = 60
end
function _G.CLFRWarheadBoomNtf:deserialize(br)
    self.seat_id = br:ReadInt32()
    self.item_id = br:ReadInt32()
    self.item_sub_id = br:ReadInt32()
    self.item_count = br:ReadInt32()
    self.fish_id = br:ReadInt32()
    self.boom_result = br:ReadInt32()
end
function _G.CLFRWarheadBoomNtf:serialize(bw)
    bw:WriteInt32(self.seat_id)
    bw:WriteInt32(self.item_id)
    bw:WriteInt32(self.item_sub_id)
    bw:WriteInt32(self.item_count)
    bw:WriteInt32(self.fish_id)
    bw:WriteInt32(self.boom_result)
end

--倍击倍数改变请求
--value number 倍击倍数
_G.CLFRMultipleHitChangeReq = _G.class(ProtocolBase)
function _G.CLFRMultipleHitChangeReq:__init(t)
    _G.CLFRMultipleHitChangeReq.super:__init(t)
    self.mid = 11
    self.pid = 61
end
function _G.CLFRMultipleHitChangeReq:deserialize(br)
    self.value = br:ReadInt32()
end
function _G.CLFRMultipleHitChangeReq:serialize(bw)
    bw:WriteInt32(self.value)
end

--倍击倍数改变回应
--errcode number 0成功 1vip等级不足
--value number 当前倍击倍数
_G.CLFRMultipleHitChangeAck = _G.class(ProtocolBase)
function _G.CLFRMultipleHitChangeAck:__init(t)
    _G.CLFRMultipleHitChangeAck.super:__init(t)
    self.mid = 11
    self.pid = 62
end
function _G.CLFRMultipleHitChangeAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.value = br:ReadInt32()
end
function _G.CLFRMultipleHitChangeAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt32(self.value)
end

--boss下次出现的时间请求
_G.CLFRBossNextAppearTimeReq = _G.class(ProtocolBase)
function _G.CLFRBossNextAppearTimeReq:__init(t)
    _G.CLFRBossNextAppearTimeReq.super:__init(t)
    self.mid = 11
    self.pid = 63
end

--boss下次出现的时间回应
--errcode number 0成功 1你不在房间中 2该房间不会出现boss
--boss_id number bossId
--timestamp number boss出现的时间戳，单位总秒数
_G.CLFRBossNextAppearTimeAck = _G.class(ProtocolBase)
function _G.CLFRBossNextAppearTimeAck:__init(t)
    _G.CLFRBossNextAppearTimeAck.super:__init(t)
    self.mid = 11
    self.pid = 64
end
function _G.CLFRBossNextAppearTimeAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.boss_id = br:ReadInt32()
    self.timestamp = br:ReadUInt32()
end
function _G.CLFRBossNextAppearTimeAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt32(self.boss_id)
    bw:WriteUInt32(self.timestamp)
end

--召唤boss请求
--boss_id number 想要召唤的bossId
_G.CLFRBossSummonReq = _G.class(ProtocolBase)
function _G.CLFRBossSummonReq:__init(t)
    _G.CLFRBossSummonReq.super:__init(t)
    self.mid = 11
    self.pid = 65
end
function _G.CLFRBossSummonReq:deserialize(br)
    self.boss_id = br:ReadInt32()
end
function _G.CLFRBossSummonReq:serialize(bw)
    bw:WriteInt32(self.boss_id)
end

--召唤boss回应
--errcode number 0成功 1你不在房间 2boss还活着
_G.CLFRBossSummonAck = _G.class(ProtocolBase)
function _G.CLFRBossSummonAck:__init(t)
    _G.CLFRBossSummonAck.super:__init(t)
    self.mid = 11
    self.pid = 66
end
function _G.CLFRBossSummonAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLFRBossSummonAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--穿透发炮请求
--angle number 发炮角度
--multiple number 倍击倍数，如果该字段大于1则优先使用该字段当做倍击倍数
_G.CLFRAcrossShootReq = _G.class(ProtocolBase)
function _G.CLFRAcrossShootReq:__init(t)
    _G.CLFRAcrossShootReq.super:__init(t)
    self.mid = 11
    self.pid = 67
end
function _G.CLFRAcrossShootReq:deserialize(br)
    self.angle = br:ReadInt32()
    self.multiple = br:ReadUInt8()
end
function _G.CLFRAcrossShootReq:serialize(bw)
    bw:WriteInt32(self.angle)
    bw:WriteUInt8(self.multiple)
end

--穿透发炮回应
--errcode number 0成功 1金币不足 2子弹数量不足 3系统错误
--bullet_id number 子弹Id
_G.CLFRAcrossShootAck = _G.class(ProtocolBase)
function _G.CLFRAcrossShootAck:__init(t)
    _G.CLFRAcrossShootAck.super:__init(t)
    self.mid = 11
    self.pid = 68
end
function _G.CLFRAcrossShootAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.bullet_id = br:ReadInt32()
end
function _G.CLFRAcrossShootAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt32(self.bullet_id)
end

--穿透发炮通知
--seat_id number 座位Id
--bullet_id number 子弹Id
--angle number 发炮角度
--multiple number 倍击倍数
_G.CLFRAcrossShootNtf = _G.class(ProtocolBase)
function _G.CLFRAcrossShootNtf:__init(t)
    _G.CLFRAcrossShootNtf.super:__init(t)
    self.mid = 11
    self.pid = 69
end
function _G.CLFRAcrossShootNtf:deserialize(br)
    self.seat_id = br:ReadInt32()
    self.bullet_id = br:ReadInt32()
    self.angle = br:ReadInt32()
    self.multiple = br:ReadUInt8()
end
function _G.CLFRAcrossShootNtf:serialize(bw)
    bw:WriteInt32(self.seat_id)
    bw:WriteInt32(self.bullet_id)
    bw:WriteInt32(self.angle)
    bw:WriteUInt8(self.multiple)
end

--穿透子弹信息
--bullet_id number 子弹Id
--angle number 发炮角度
local function CLFRAcrossBulletInfo_deserialize(br)
    local r = {}
    r.bullet_id = br:ReadInt32()
    r.angle = br:ReadInt32()
    return r
end
local function CLFRAcrossBulletInfo_serialize(t, bw)
    bw:WriteInt32(t.bullet_id)
    bw:WriteInt32(t.angle)
end

--多炮台穿透发炮请求
--shoot_len number 发炮数量
--shoot_array number[10] 发炮信息数组
--multiple number 倍击倍数，如果该字段大于1则优先使用该字段当做倍击倍数
_G.CLFRAcrossMultiShootReq = _G.class(ProtocolBase)
function _G.CLFRAcrossMultiShootReq:__init(t)
    _G.CLFRAcrossMultiShootReq.super:__init(t)
    self.mid = 11
    self.pid = 70
end
function _G.CLFRAcrossMultiShootReq:deserialize(br)
    self.shoot_len = br:ReadInt8()
    self.shoot_array = {}
    for i=1,self.shoot_len do
        self.shoot_array[i] = br:ReadInt32()
    end
    self.multiple = br:ReadUInt8()
end
function _G.CLFRAcrossMultiShootReq:serialize(bw)
    assert(not self.shoot_array or #self.shoot_array <= 10, "CLFRAcrossMultiShootReq.shoot_array数组长度超过规定限制! 期望:10 实际:" .. #self.shoot_array)
    bw:WriteInt8(#(self.shoot_array or {}))
    for i=1,#(self.shoot_array or {}) do
        bw:WriteInt32(self.shoot_array[i])
    end
    bw:WriteUInt8(self.multiple)
end

--多炮台穿透发炮回应
--errcode number 0成功 1金币不足 2子弹数量不足 3系统错误
--bullet_len number 子弹数量
--bullet_array AcrossBulletInfo[10] 子弹信息数组
_G.CLFRAcrossMultiShootAck = _G.class(ProtocolBase)
function _G.CLFRAcrossMultiShootAck:__init(t)
    _G.CLFRAcrossMultiShootAck.super:__init(t)
    self.mid = 11
    self.pid = 71
end
function _G.CLFRAcrossMultiShootAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.bullet_len = br:ReadInt8()
    self.bullet_array = {}
    for i=1,self.bullet_len do
        self.bullet_array[i] = CLFRAcrossBulletInfo_deserialize(br)
    end
end
function _G.CLFRAcrossMultiShootAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.bullet_array or #self.bullet_array <= 10, "CLFRAcrossMultiShootAck.bullet_array数组长度超过规定限制! 期望:10 实际:" .. #self.bullet_array)
    bw:WriteInt8(#(self.bullet_array or {}))
    for i=1,#(self.bullet_array or {}) do
        CLFRAcrossBulletInfo_serialize(self.bullet_array[i], bw)
    end
end

--多炮台同时发炮通知
--seat_id number 座位Id
--bullet_len number 子弹数量
--bullet_array AcrossBulletInfo[10] 子弹信息数组
--multiple number 倍击倍数
_G.CLFRAcrossMultiShootNtf = _G.class(ProtocolBase)
function _G.CLFRAcrossMultiShootNtf:__init(t)
    _G.CLFRAcrossMultiShootNtf.super:__init(t)
    self.mid = 11
    self.pid = 72
end
function _G.CLFRAcrossMultiShootNtf:deserialize(br)
    self.seat_id = br:ReadInt32()
    self.bullet_len = br:ReadInt8()
    self.bullet_array = {}
    for i=1,self.bullet_len do
        self.bullet_array[i] = CLFRAcrossBulletInfo_deserialize(br)
    end
    self.multiple = br:ReadUInt8()
end
function _G.CLFRAcrossMultiShootNtf:serialize(bw)
    bw:WriteInt32(self.seat_id)
    assert(not self.bullet_array or #self.bullet_array <= 10, "CLFRAcrossMultiShootNtf.bullet_array数组长度超过规定限制! 期望:10 实际:" .. #self.bullet_array)
    bw:WriteInt8(#(self.bullet_array or {}))
    for i=1,#(self.bullet_array or {}) do
        CLFRAcrossBulletInfo_serialize(self.bullet_array[i], bw)
    end
    bw:WriteUInt8(self.multiple)
end

--穿透命中请求
--bullet_id number 子弹Id
--fish_id number 鱼Id
--related_fish_len number 关联的鱼数组长度，目前用于黑洞鱼
--related_fish_array number[30] 关联的鱼Id数组，目前用于黑洞鱼
_G.CLFRAcrossHitReq = _G.class(ProtocolBase)
function _G.CLFRAcrossHitReq:__init(t)
    _G.CLFRAcrossHitReq.super:__init(t)
    self.mid = 11
    self.pid = 73
end
function _G.CLFRAcrossHitReq:deserialize(br)
    self.bullet_id = br:ReadInt32()
    self.fish_id = br:ReadInt32()
    self.related_fish_len = br:ReadInt8()
    self.related_fish_array = {}
    for i=1,self.related_fish_len do
        self.related_fish_array[i] = br:ReadInt32()
    end
end
function _G.CLFRAcrossHitReq:serialize(bw)
    bw:WriteInt32(self.bullet_id)
    bw:WriteInt32(self.fish_id)
    assert(not self.related_fish_array or #self.related_fish_array <= 30, "CLFRAcrossHitReq.related_fish_array数组长度超过规定限制! 期望:30 实际:" .. #self.related_fish_array)
    bw:WriteInt8(#(self.related_fish_array or {}))
    for i=1,#(self.related_fish_array or {}) do
        bw:WriteInt32(self.related_fish_array[i])
    end
end

--穿透命中回应
--errcode number 0成功 1金币不足 2子弹数量不足 3系统错误
--currency_delta number 金币变化量
--bind_currency_delta number 绑定金币变化量
--is_boom number 是否打爆 0否 1是
--win_currency number 获得的金币
--win_diamond number 获得的钻石
--win_match_integral number 获得的比赛积分
--item_len number 掉落物品数量
--items ItemInfo[100] 掉落的物品数组
--related_fish_len number 其他关联的死亡鱼的数组长度
--related_fish_array number[100] 其他关联的死亡鱼Id的数组
--related_remove_reason number 移除原因 0未知 1炸弹鱼移除 2闪电鱼移除 3鱼王移除 4黑洞鱼移除 5一网打尽
--multiple number 倍击倍数
--energy_delta number 积累的能量变化
_G.CLFRAcrossHitAck = _G.class(ProtocolBase)
function _G.CLFRAcrossHitAck:__init(t)
    _G.CLFRAcrossHitAck.super:__init(t)
    self.mid = 11
    self.pid = 74
end
function _G.CLFRAcrossHitAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.currency_delta = br:ReadInt64()
    self.bind_currency_delta = br:ReadInt64()
    self.is_boom = br:ReadInt8()
    self.win_currency = br:ReadInt64()
    self.win_diamond = br:ReadInt64()
    self.win_match_integral = br:ReadInt32()
    self.item_len = br:ReadInt8()
    self.items = {}
    for i=1,self.item_len do
        self.items[i] = CLFRItemInfo_deserialize(br)
    end
    self.related_fish_len = br:ReadInt8()
    self.related_fish_array = {}
    for i=1,self.related_fish_len do
        self.related_fish_array[i] = br:ReadInt32()
    end
    self.related_remove_reason = br:ReadInt8()
    self.multiple = br:ReadUInt8()
    self.energy_delta = br:ReadInt64()
end
function _G.CLFRAcrossHitAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt64(self.currency_delta)
    bw:WriteInt64(self.bind_currency_delta)
    bw:WriteInt8(self.is_boom)
    bw:WriteInt64(self.win_currency)
    bw:WriteInt64(self.win_diamond)
    bw:WriteInt32(self.win_match_integral)
    assert(not self.items or #self.items <= 100, "CLFRAcrossHitAck.items数组长度超过规定限制! 期望:100 实际:" .. #self.items)
    bw:WriteInt8(#(self.items or {}))
    for i=1,#(self.items or {}) do
        CLFRItemInfo_serialize(self.items[i], bw)
    end
    assert(not self.related_fish_array or #self.related_fish_array <= 100, "CLFRAcrossHitAck.related_fish_array数组长度超过规定限制! 期望:100 实际:" .. #self.related_fish_array)
    bw:WriteInt8(#(self.related_fish_array or {}))
    for i=1,#(self.related_fish_array or {}) do
        bw:WriteInt32(self.related_fish_array[i])
    end
    bw:WriteInt8(self.related_remove_reason)
    bw:WriteUInt8(self.multiple)
    bw:WriteInt64(self.energy_delta)
end

--穿透命中通知
--seat_id number 座位Id
--bullet_id number 子弹Id
--fish_id number 鱼Id
--currency_delta number 金币变化量
--bind_currency_delta number 绑定金币变化量
--is_boom number 是否打爆 0否 1是
--win_currency number 获得的金币
--win_diamond number 获得的钻石
--win_match_integral number 获得的比赛积分
--item_len number 掉落物品数量
--items ItemInfo[100] 掉落的物品数组
--related_fish_len number 其他关联的死亡鱼的数组长度
--related_fish_array number[100] 其他关联的死亡鱼Id的数组
--related_remove_reason number 移除原因 0未知 1炸弹鱼移除 2闪电鱼移除 3鱼王移除 4黑洞鱼移除 5一网打尽
--multiple number 倍击倍数
_G.CLFRAcrossHitNtf = _G.class(ProtocolBase)
function _G.CLFRAcrossHitNtf:__init(t)
    _G.CLFRAcrossHitNtf.super:__init(t)
    self.mid = 11
    self.pid = 75
end
function _G.CLFRAcrossHitNtf:deserialize(br)
    self.seat_id = br:ReadInt32()
    self.bullet_id = br:ReadInt32()
    self.fish_id = br:ReadInt32()
    self.currency_delta = br:ReadInt64()
    self.bind_currency_delta = br:ReadInt64()
    self.is_boom = br:ReadInt8()
    self.win_currency = br:ReadInt64()
    self.win_diamond = br:ReadInt64()
    self.win_match_integral = br:ReadInt32()
    self.item_len = br:ReadInt8()
    self.items = {}
    for i=1,self.item_len do
        self.items[i] = CLFRItemInfo_deserialize(br)
    end
    self.related_fish_len = br:ReadInt8()
    self.related_fish_array = {}
    for i=1,self.related_fish_len do
        self.related_fish_array[i] = br:ReadInt32()
    end
    self.related_remove_reason = br:ReadInt8()
    self.multiple = br:ReadUInt8()
end
function _G.CLFRAcrossHitNtf:serialize(bw)
    bw:WriteInt32(self.seat_id)
    bw:WriteInt32(self.bullet_id)
    bw:WriteInt32(self.fish_id)
    bw:WriteInt64(self.currency_delta)
    bw:WriteInt64(self.bind_currency_delta)
    bw:WriteInt8(self.is_boom)
    bw:WriteInt64(self.win_currency)
    bw:WriteInt64(self.win_diamond)
    bw:WriteInt32(self.win_match_integral)
    assert(not self.items or #self.items <= 100, "CLFRAcrossHitNtf.items数组长度超过规定限制! 期望:100 实际:" .. #self.items)
    bw:WriteInt8(#(self.items or {}))
    for i=1,#(self.items or {}) do
        CLFRItemInfo_serialize(self.items[i], bw)
    end
    assert(not self.related_fish_array or #self.related_fish_array <= 100, "CLFRAcrossHitNtf.related_fish_array数组长度超过规定限制! 期望:100 实际:" .. #self.related_fish_array)
    bw:WriteInt8(#(self.related_fish_array or {}))
    for i=1,#(self.related_fish_array or {}) do
        bw:WriteInt32(self.related_fish_array[i])
    end
    bw:WriteInt8(self.related_remove_reason)
    bw:WriteUInt8(self.multiple)
end

--boss穿透命中请求
--bullet_id number 子弹Id
_G.CLFRAcrossBossHitReq = _G.class(ProtocolBase)
function _G.CLFRAcrossBossHitReq:__init(t)
    _G.CLFRAcrossBossHitReq.super:__init(t)
    self.mid = 11
    self.pid = 76
end
function _G.CLFRAcrossBossHitReq:deserialize(br)
    self.bullet_id = br:ReadInt32()
end
function _G.CLFRAcrossBossHitReq:serialize(bw)
    bw:WriteInt32(self.bullet_id)
end

--boss穿透命中回应
--errcode number 0成功 1金币不足 2子弹数量不足 3系统错误
--currency_delta number 金币变化量
--bind_currency_delta number 绑定金币变化量
--is_boom number 是否爆金 0miss 1爆金 2死亡
--win_currency number 获得的金币
--multiple number 倍击倍数
--energy_delta number 积累的能量变化
_G.CLFRAcrossBossHitAck = _G.class(ProtocolBase)
function _G.CLFRAcrossBossHitAck:__init(t)
    _G.CLFRAcrossBossHitAck.super:__init(t)
    self.mid = 11
    self.pid = 77
end
function _G.CLFRAcrossBossHitAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.currency_delta = br:ReadInt64()
    self.bind_currency_delta = br:ReadInt64()
    self.is_boom = br:ReadInt8()
    self.win_currency = br:ReadInt64()
    self.multiple = br:ReadUInt8()
    self.energy_delta = br:ReadInt64()
end
function _G.CLFRAcrossBossHitAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt64(self.currency_delta)
    bw:WriteInt64(self.bind_currency_delta)
    bw:WriteInt8(self.is_boom)
    bw:WriteInt64(self.win_currency)
    bw:WriteUInt8(self.multiple)
    bw:WriteInt64(self.energy_delta)
end

--boss穿透命中通知
--seat_id number 座位Id
--bullet_id number 子弹Id
--currency_delta number 金币变化量
--bind_currency_delta number 绑定金币变化量
--is_boom number 是否爆金 0miss 1爆金 2死亡
--win_currency number 获得的金币
--multiple number 倍击倍数
_G.CLFRAcrossBossHitNtf = _G.class(ProtocolBase)
function _G.CLFRAcrossBossHitNtf:__init(t)
    _G.CLFRAcrossBossHitNtf.super:__init(t)
    self.mid = 11
    self.pid = 78
end
function _G.CLFRAcrossBossHitNtf:deserialize(br)
    self.seat_id = br:ReadInt32()
    self.bullet_id = br:ReadInt32()
    self.currency_delta = br:ReadInt64()
    self.bind_currency_delta = br:ReadInt64()
    self.is_boom = br:ReadInt8()
    self.win_currency = br:ReadInt64()
    self.multiple = br:ReadUInt8()
end
function _G.CLFRAcrossBossHitNtf:serialize(bw)
    bw:WriteInt32(self.seat_id)
    bw:WriteInt32(self.bullet_id)
    bw:WriteInt64(self.currency_delta)
    bw:WriteInt64(self.bind_currency_delta)
    bw:WriteInt8(self.is_boom)
    bw:WriteInt64(self.win_currency)
    bw:WriteUInt8(self.multiple)
end

--能量炮打死的鱼信息
--fish_id number 打死的鱼Id
--currency_delta number 金币变化量
--diamond_delta number 钻石变化量
--item_len number 掉落物品数量
--items ItemInfo[100] 掉落的物品数组
--related_fish_len number 其他关联的死亡鱼的数组长度
--related_fish_array number[100] 其他关联的死亡鱼Id的数组
--related_remove_reason number 移除原因 0未知 1炸弹鱼移除 2闪电鱼移除 3鱼王移除 4黑洞鱼移除 5一网打尽
local function CLFREnergyFishBoomInfo_deserialize(br)
    local r = {}
    r.fish_id = br:ReadInt32()
    r.currency_delta = br:ReadInt64()
    r.diamond_delta = br:ReadInt64()
    r.item_len = br:ReadInt8()
    r.items = {}
    for i=1,r.item_len do
        r.items[i] = CLFRItemInfo_deserialize(br)
    end
    r.related_fish_len = br:ReadInt8()
    r.related_fish_array = {}
    for i=1,r.related_fish_len do
        r.related_fish_array[i] = br:ReadInt32()
    end
    r.related_remove_reason = br:ReadInt8()
    return r
end
local function CLFREnergyFishBoomInfo_serialize(t, bw)
    bw:WriteInt32(t.fish_id)
    bw:WriteInt64(t.currency_delta)
    bw:WriteInt64(t.diamond_delta)
    assert(not t.items or #t.items <= 100, "CLFREnergyFishBoomInfo.items数组长度超过规定限制! 期望:100 实际:" .. #t.items)
    bw:WriteInt8(#(t.items or {}))
    for i=1,#(t.items or {}) do
        CLFRItemInfo_serialize(t.items[i], bw)
    end
    assert(not t.related_fish_array or #t.related_fish_array <= 100, "CLFREnergyFishBoomInfo.related_fish_array数组长度超过规定限制! 期望:100 实际:" .. #t.related_fish_array)
    bw:WriteInt8(#(t.related_fish_array or {}))
    for i=1,#(t.related_fish_array or {}) do
        bw:WriteInt32(t.related_fish_array[i])
    end
    bw:WriteInt8(t.related_remove_reason)
end

--能量炮蓄力请求
_G.CLFREnergyStoreReq = _G.class(ProtocolBase)
function _G.CLFREnergyStoreReq:__init(t)
    _G.CLFREnergyStoreReq.super:__init(t)
    self.mid = 11
    self.pid = 79
end

--能量炮蓄力回应
--errcode number 0成功 1你不在房间中 2能量不足 3已蓄力
--energy_delta number 能量变化值，这里值为负
_G.CLFREnergyStoreAck = _G.class(ProtocolBase)
function _G.CLFREnergyStoreAck:__init(t)
    _G.CLFREnergyStoreAck.super:__init(t)
    self.mid = 11
    self.pid = 80
end
function _G.CLFREnergyStoreAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.energy_delta = br:ReadInt64()
end
function _G.CLFREnergyStoreAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt64(self.energy_delta)
end

--能量炮发射请求
--angle number 发炮角度
--related_fish_len number 能量炮命中的鱼数组长度
--related_fish_array number[30] 能量炮命中的鱼Id数组
_G.CLFREnergyShootReq = _G.class(ProtocolBase)
function _G.CLFREnergyShootReq:__init(t)
    _G.CLFREnergyShootReq.super:__init(t)
    self.mid = 11
    self.pid = 81
end
function _G.CLFREnergyShootReq:deserialize(br)
    self.angle = br:ReadInt32()
    self.related_fish_len = br:ReadInt8()
    self.related_fish_array = {}
    for i=1,self.related_fish_len do
        self.related_fish_array[i] = br:ReadInt32()
    end
end
function _G.CLFREnergyShootReq:serialize(bw)
    bw:WriteInt32(self.angle)
    assert(not self.related_fish_array or #self.related_fish_array <= 30, "CLFREnergyShootReq.related_fish_array数组长度超过规定限制! 期望:30 实际:" .. #self.related_fish_array)
    bw:WriteInt8(#(self.related_fish_array or {}))
    for i=1,#(self.related_fish_array or {}) do
        bw:WriteInt32(self.related_fish_array[i])
    end
end

--能量炮发射回应
--errcode number 0成功 1还未蓄力 2系统错误
--boom_length number 能量炮打死的鱼数组长度
--boom_array EnergyFishBoomInfo[30] 能量炮打死的鱼数组
_G.CLFREnergyShootAck = _G.class(ProtocolBase)
function _G.CLFREnergyShootAck:__init(t)
    _G.CLFREnergyShootAck.super:__init(t)
    self.mid = 11
    self.pid = 82
end
function _G.CLFREnergyShootAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.boom_length = br:ReadInt8()
    self.boom_array = {}
    for i=1,self.boom_length do
        self.boom_array[i] = CLFREnergyFishBoomInfo_deserialize(br)
    end
end
function _G.CLFREnergyShootAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.boom_array or #self.boom_array <= 30, "CLFREnergyShootAck.boom_array数组长度超过规定限制! 期望:30 实际:" .. #self.boom_array)
    bw:WriteInt8(#(self.boom_array or {}))
    for i=1,#(self.boom_array or {}) do
        CLFREnergyFishBoomInfo_serialize(self.boom_array[i], bw)
    end
end

--能量炮发射通知
--seat_id number 座位Id
--angle number 发炮角度
--related_fish_len number 能量炮命中的鱼数组长度
--related_fish_array number[30] 能量炮命中的鱼Id数组
--boom_length number 能量炮打死的鱼数组长度
--boom_array EnergyFishBoomInfo[30] 能量炮打死的鱼数组
_G.CLFREnergyShootNtf = _G.class(ProtocolBase)
function _G.CLFREnergyShootNtf:__init(t)
    _G.CLFREnergyShootNtf.super:__init(t)
    self.mid = 11
    self.pid = 83
end
function _G.CLFREnergyShootNtf:deserialize(br)
    self.seat_id = br:ReadInt32()
    self.angle = br:ReadInt32()
    self.related_fish_len = br:ReadInt8()
    self.related_fish_array = {}
    for i=1,self.related_fish_len do
        self.related_fish_array[i] = br:ReadInt32()
    end
    self.boom_length = br:ReadInt8()
    self.boom_array = {}
    for i=1,self.boom_length do
        self.boom_array[i] = CLFREnergyFishBoomInfo_deserialize(br)
    end
end
function _G.CLFREnergyShootNtf:serialize(bw)
    bw:WriteInt32(self.seat_id)
    bw:WriteInt32(self.angle)
    assert(not self.related_fish_array or #self.related_fish_array <= 30, "CLFREnergyShootNtf.related_fish_array数组长度超过规定限制! 期望:30 实际:" .. #self.related_fish_array)
    bw:WriteInt8(#(self.related_fish_array or {}))
    for i=1,#(self.related_fish_array or {}) do
        bw:WriteInt32(self.related_fish_array[i])
    end
    assert(not self.boom_array or #self.boom_array <= 30, "CLFREnergyShootNtf.boom_array数组长度超过规定限制! 期望:30 实际:" .. #self.boom_array)
    bw:WriteInt8(#(self.boom_array or {}))
    for i=1,#(self.boom_array or {}) do
        CLFREnergyFishBoomInfo_serialize(self.boom_array[i], bw)
    end
end

--注册协议
ProtocolHelper.registerProtocol("CLFREnterGameReq", _G.CLFREnterGameReq, 11, 0)
ProtocolHelper.registerProtocol("CLFREnterGameAck", _G.CLFREnterGameAck, 11, 1)
ProtocolHelper.registerProtocol("CLFRExitGameReq", _G.CLFRExitGameReq, 11, 2)
ProtocolHelper.registerProtocol("CLFRExitGameAck", _G.CLFRExitGameAck, 11, 3)
ProtocolHelper.registerProtocol("CLFRGetReadyReq", _G.CLFRGetReadyReq, 11, 4)
ProtocolHelper.registerProtocol("CLFRGetReadyAck", _G.CLFRGetReadyAck, 11, 5)
ProtocolHelper.registerProtocol("CLFRJoinMatchReq", _G.CLFRJoinMatchReq, 11, 6)
ProtocolHelper.registerProtocol("CLFRJoinMatchAck", _G.CLFRJoinMatchAck, 11, 7)
ProtocolHelper.registerProtocol("CLFRJoinMatchNtf", _G.CLFRJoinMatchNtf, 11, 8)
ProtocolHelper.registerProtocol("CLFRMatchOverNtf", _G.CLFRMatchOverNtf, 11, 9)
ProtocolHelper.registerProtocol("CLFRPlayerJoinNtf", _G.CLFRPlayerJoinNtf, 11, 10)
ProtocolHelper.registerProtocol("CLFRPlayerLeaveNtf", _G.CLFRPlayerLeaveNtf, 11, 11)
ProtocolHelper.registerProtocol("CLFRFishTideStartNtf", _G.CLFRFishTideStartNtf, 11, 12)
ProtocolHelper.registerProtocol("CLFRFishAppearNtf", _G.CLFRFishAppearNtf, 11, 13)
ProtocolHelper.registerProtocol("CLFRFishTimeRateChangeNtf", _G.CLFRFishTimeRateChangeNtf, 11, 14)
ProtocolHelper.registerProtocol("CLFRGunValueChangeReq", _G.CLFRGunValueChangeReq, 11, 15)
ProtocolHelper.registerProtocol("CLFRGunValueChangeAck", _G.CLFRGunValueChangeAck, 11, 16)
ProtocolHelper.registerProtocol("CLFRGunValueChangeNtf", _G.CLFRGunValueChangeNtf, 11, 17)
ProtocolHelper.registerProtocol("CLFRShootReq", _G.CLFRShootReq, 11, 18)
ProtocolHelper.registerProtocol("CLFRShootAck", _G.CLFRShootAck, 11, 19)
ProtocolHelper.registerProtocol("CLFRShootNtf", _G.CLFRShootNtf, 11, 20)
ProtocolHelper.registerProtocol("CLFRMultiShootReq", _G.CLFRMultiShootReq, 11, 21)
ProtocolHelper.registerProtocol("CLFRMultiShootAck", _G.CLFRMultiShootAck, 11, 22)
ProtocolHelper.registerProtocol("CLFRMultiShootNtf", _G.CLFRMultiShootNtf, 11, 23)
ProtocolHelper.registerProtocol("CLFRHitReq", _G.CLFRHitReq, 11, 24)
ProtocolHelper.registerProtocol("CLFRHitAck", _G.CLFRHitAck, 11, 25)
ProtocolHelper.registerProtocol("CLFRHitNtf", _G.CLFRHitNtf, 11, 26)
ProtocolHelper.registerProtocol("CLFRRobotHitRpt", _G.CLFRRobotHitRpt, 11, 27)
ProtocolHelper.registerProtocol("CLFRBossAppearNtf", _G.CLFRBossAppearNtf, 11, 28)
ProtocolHelper.registerProtocol("CLFRBossHitReq", _G.CLFRBossHitReq, 11, 29)
ProtocolHelper.registerProtocol("CLFRBossHitAck", _G.CLFRBossHitAck, 11, 30)
ProtocolHelper.registerProtocol("CLFRBossHitNtf", _G.CLFRBossHitNtf, 11, 31)
ProtocolHelper.registerProtocol("CLFRBossDefenceBloodChangedNtf", _G.CLFRBossDefenceBloodChangedNtf, 11, 32)
ProtocolHelper.registerProtocol("CLFRBossKilledNtf", _G.CLFRBossKilledNtf, 11, 33)
ProtocolHelper.registerProtocol("CLFRBossRankReq", _G.CLFRBossRankReq, 11, 34)
ProtocolHelper.registerProtocol("CLFRBossRankAck", _G.CLFRBossRankAck, 11, 35)
ProtocolHelper.registerProtocol("CLFRFishRemoveNtf", _G.CLFRFishRemoveNtf, 11, 36)
ProtocolHelper.registerProtocol("CLFRShowWheel1Ntf", _G.CLFRShowWheel1Ntf, 11, 37)
ProtocolHelper.registerProtocol("CLFRShowWheel2Ntf", _G.CLFRShowWheel2Ntf, 11, 38)
ProtocolHelper.registerProtocol("CLFRSeatResourceChangedNtf", _G.CLFRSeatResourceChangedNtf, 11, 39)
ProtocolHelper.registerProtocol("CLFRSeatItemUsedNtf", _G.CLFRSeatItemUsedNtf, 11, 40)
ProtocolHelper.registerProtocol("CLFRGunUnlockReq", _G.CLFRGunUnlockReq, 11, 41)
ProtocolHelper.registerProtocol("CLFRGunUnlockAck", _G.CLFRGunUnlockAck, 11, 42)
ProtocolHelper.registerProtocol("CLFRBonusPoolChangedNtf", _G.CLFRBonusPoolChangedNtf, 11, 43)
ProtocolHelper.registerProtocol("CLFRBonusWheelReq", _G.CLFRBonusWheelReq, 11, 44)
ProtocolHelper.registerProtocol("CLFRBonusWheelAck", _G.CLFRBonusWheelAck, 11, 45)
ProtocolHelper.registerProtocol("CLFRAddEffectShadowNtf", _G.CLFRAddEffectShadowNtf, 11, 46)
ProtocolHelper.registerProtocol("CLFRFishSummonNtf", _G.CLFRFishSummonNtf, 11, 47)
ProtocolHelper.registerProtocol("CLFRGunSwitchReq", _G.CLFRGunSwitchReq, 11, 48)
ProtocolHelper.registerProtocol("CLFRGunSwitchAck", _G.CLFRGunSwitchAck, 11, 49)
ProtocolHelper.registerProtocol("CLFRGunSwitchNtf", _G.CLFRGunSwitchNtf, 11, 50)
ProtocolHelper.registerProtocol("CLFRFishTideForTestReq", _G.CLFRFishTideForTestReq, 11, 51)
ProtocolHelper.registerProtocol("CLFRFishTideForTestAck", _G.CLFRFishTideForTestAck, 11, 52)
ProtocolHelper.registerProtocol("CLFRIntegralGainQueryReq", _G.CLFRIntegralGainQueryReq, 11, 53)
ProtocolHelper.registerProtocol("CLFRIntegralGainQueryAck", _G.CLFRIntegralGainQueryAck, 11, 54)
ProtocolHelper.registerProtocol("CLFRWarheadLockReq", _G.CLFRWarheadLockReq, 11, 55)
ProtocolHelper.registerProtocol("CLFRWarheadLockAck", _G.CLFRWarheadLockAck, 11, 56)
ProtocolHelper.registerProtocol("CLFRWarheadLockNtf", _G.CLFRWarheadLockNtf, 11, 57)
ProtocolHelper.registerProtocol("CLFRWarheadBoomReq", _G.CLFRWarheadBoomReq, 11, 58)
ProtocolHelper.registerProtocol("CLFRWarheadBoomAck", _G.CLFRWarheadBoomAck, 11, 59)
ProtocolHelper.registerProtocol("CLFRWarheadBoomNtf", _G.CLFRWarheadBoomNtf, 11, 60)
ProtocolHelper.registerProtocol("CLFRMultipleHitChangeReq", _G.CLFRMultipleHitChangeReq, 11, 61)
ProtocolHelper.registerProtocol("CLFRMultipleHitChangeAck", _G.CLFRMultipleHitChangeAck, 11, 62)
ProtocolHelper.registerProtocol("CLFRBossNextAppearTimeReq", _G.CLFRBossNextAppearTimeReq, 11, 63)
ProtocolHelper.registerProtocol("CLFRBossNextAppearTimeAck", _G.CLFRBossNextAppearTimeAck, 11, 64)
ProtocolHelper.registerProtocol("CLFRBossSummonReq", _G.CLFRBossSummonReq, 11, 65)
ProtocolHelper.registerProtocol("CLFRBossSummonAck", _G.CLFRBossSummonAck, 11, 66)
ProtocolHelper.registerProtocol("CLFRAcrossShootReq", _G.CLFRAcrossShootReq, 11, 67)
ProtocolHelper.registerProtocol("CLFRAcrossShootAck", _G.CLFRAcrossShootAck, 11, 68)
ProtocolHelper.registerProtocol("CLFRAcrossShootNtf", _G.CLFRAcrossShootNtf, 11, 69)
ProtocolHelper.registerProtocol("CLFRAcrossMultiShootReq", _G.CLFRAcrossMultiShootReq, 11, 70)
ProtocolHelper.registerProtocol("CLFRAcrossMultiShootAck", _G.CLFRAcrossMultiShootAck, 11, 71)
ProtocolHelper.registerProtocol("CLFRAcrossMultiShootNtf", _G.CLFRAcrossMultiShootNtf, 11, 72)
ProtocolHelper.registerProtocol("CLFRAcrossHitReq", _G.CLFRAcrossHitReq, 11, 73)
ProtocolHelper.registerProtocol("CLFRAcrossHitAck", _G.CLFRAcrossHitAck, 11, 74)
ProtocolHelper.registerProtocol("CLFRAcrossHitNtf", _G.CLFRAcrossHitNtf, 11, 75)
ProtocolHelper.registerProtocol("CLFRAcrossBossHitReq", _G.CLFRAcrossBossHitReq, 11, 76)
ProtocolHelper.registerProtocol("CLFRAcrossBossHitAck", _G.CLFRAcrossBossHitAck, 11, 77)
ProtocolHelper.registerProtocol("CLFRAcrossBossHitNtf", _G.CLFRAcrossBossHitNtf, 11, 78)
ProtocolHelper.registerProtocol("CLFREnergyStoreReq", _G.CLFREnergyStoreReq, 11, 79)
ProtocolHelper.registerProtocol("CLFREnergyStoreAck", _G.CLFREnergyStoreAck, 11, 80)
ProtocolHelper.registerProtocol("CLFREnergyShootReq", _G.CLFREnergyShootReq, 11, 81)
ProtocolHelper.registerProtocol("CLFREnergyShootAck", _G.CLFREnergyShootAck, 11, 82)
ProtocolHelper.registerProtocol("CLFREnergyShootNtf", _G.CLFREnergyShootNtf, 11, 83)
