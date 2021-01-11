----------------------------------------------------------
--二进制协议序列化/反序列化文件，由工具自动生成，请勿手动修改
--JBPROTO POWERED BY monkey256 ^_^
----------------------------------------------------------

--引入模块
local ProtocolBase = _G.import(".ProtocolBase")
local ProtocolHelper = _G.import(".ProtocolHelper")

--登出请求
_G.CLPFLogoutReq = _G.class(ProtocolBase)
function _G.CLPFLogoutReq:__init(t)
    _G.CLPFLogoutReq.super:__init(t)
    self.mid = 2
    self.pid = 0
end

--资源同步通知
--diamond number 钻石数量
--currency number 金币数量
--integral number 积分数量
_G.CLPFResSyncNtf = _G.class(ProtocolBase)
function _G.CLPFResSyncNtf:__init(t)
    _G.CLPFResSyncNtf.super:__init(t)
    self.mid = 2
    self.pid = 1
end
function _G.CLPFResSyncNtf:deserialize(br)
    self.diamond = br:ReadInt64()
    self.currency = br:ReadInt64()
    self.integral = br:ReadInt64()
end
function _G.CLPFResSyncNtf:serialize(bw)
    bw:WriteInt64(self.diamond)
    bw:WriteInt64(self.currency)
    bw:WriteInt64(self.integral)
end

--资源变化通知
--res_type number 资源类型 1钻石 2金币 3绑定金币 4积分
--res_value number 资源值
--res_delta number 资源变化量
_G.CLPFResChangedNtf = _G.class(ProtocolBase)
function _G.CLPFResChangedNtf:__init(t)
    _G.CLPFResChangedNtf.super:__init(t)
    self.mid = 2
    self.pid = 2
end
function _G.CLPFResChangedNtf:deserialize(br)
    self.res_type = br:ReadInt8()
    self.res_value = br:ReadInt64()
    self.res_delta = br:ReadInt64()
end
function _G.CLPFResChangedNtf:serialize(bw)
    bw:WriteInt8(self.res_type)
    bw:WriteInt64(self.res_value)
    bw:WriteInt64(self.res_delta)
end

--物品信息结构
--item_id number 物品主类型
--item_sub_id number 物品子类型
--item_count number 物品数量
local function CLPFItemInfo_deserialize(br)
    local r = {}
    r.item_id = br:ReadInt32()
    r.item_sub_id = br:ReadInt32()
    r.item_count = br:ReadInt64()
    return r
end
local function CLPFItemInfo_serialize(t, bw)
    bw:WriteInt32(t.item_id)
    bw:WriteInt32(t.item_sub_id)
    bw:WriteInt64(t.item_count)
end

--获取物品列表请求
_G.CLPFItemGetListReq = _G.class(ProtocolBase)
function _G.CLPFItemGetListReq:__init(t)
    _G.CLPFItemGetListReq.super:__init(t)
    self.mid = 2
    self.pid = 3
end

--获取物品列表回应
--item_len number 数组长度
--items ItemInfo[100] 物品数组
_G.CLPFItemGetListAck = _G.class(ProtocolBase)
function _G.CLPFItemGetListAck:__init(t)
    _G.CLPFItemGetListAck.super:__init(t)
    self.mid = 2
    self.pid = 4
end
function _G.CLPFItemGetListAck:deserialize(br)
    self.item_len = br:ReadInt32()
    self.items = {}
    for i=1,self.item_len do
        self.items[i] = CLPFItemInfo_deserialize(br)
    end
end
function _G.CLPFItemGetListAck:serialize(bw)
    assert(not self.items or #self.items <= 100, "CLPFItemGetListAck.items数组长度超过规定限制! 期望:100 实际:" .. #self.items)
    bw:WriteInt32(#(self.items or {}))
    for i=1,#(self.items or {}) do
        CLPFItemInfo_serialize(self.items[i], bw)
    end
end

--使用物品请求
--item ItemInfo 物品信息
--group_id number 游戏Id
--service_id number 玩法Id
_G.CLPFItemUseReq = _G.class(ProtocolBase)
function _G.CLPFItemUseReq:__init(t)
    _G.CLPFItemUseReq.super:__init(t)
    self.mid = 2
    self.pid = 5
end
function _G.CLPFItemUseReq:deserialize(br)
    self.item = CLPFItemInfo_deserialize(br)
    self.group_id = br:ReadInt32()
    self.service_id = br:ReadInt32()
end
function _G.CLPFItemUseReq:serialize(bw)
    CLPFItemInfo_serialize(self.item, bw)
    bw:WriteInt32(self.group_id)
    bw:WriteInt32(self.service_id)
end

--使用物品回应
--errcode number 0成功 1数量不足 2配置表错误 3使用失败 4鱼潮即将来临禁止使用 5狂暴下不能使用瞄准 6分身下不能使用瞄准 7vip等级不足
_G.CLPFItemUseAck = _G.class(ProtocolBase)
function _G.CLPFItemUseAck:__init(t)
    _G.CLPFItemUseAck.super:__init(t)
    self.mid = 2
    self.pid = 6
end
function _G.CLPFItemUseAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLPFItemUseAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--物品数量变化通知
--item_len number 数组长度
--items ItemInfo[100] 物品数组
_G.CLPFItemCountChangeNtf = _G.class(ProtocolBase)
function _G.CLPFItemCountChangeNtf:__init(t)
    _G.CLPFItemCountChangeNtf.super:__init(t)
    self.mid = 2
    self.pid = 7
end
function _G.CLPFItemCountChangeNtf:deserialize(br)
    self.item_len = br:ReadInt32()
    self.items = {}
    for i=1,self.item_len do
        self.items[i] = CLPFItemInfo_deserialize(br)
    end
end
function _G.CLPFItemCountChangeNtf:serialize(bw)
    assert(not self.items or #self.items <= 100, "CLPFItemCountChangeNtf.items数组长度超过规定限制! 期望:100 实际:" .. #self.items)
    bw:WriteInt32(#(self.items or {}))
    for i=1,#(self.items or {}) do
        CLPFItemInfo_serialize(self.items[i], bw)
    end
end

--物品购买请求
--item ItemInfo 购买的物品
_G.CLPFItemBuyReq = _G.class(ProtocolBase)
function _G.CLPFItemBuyReq:__init(t)
    _G.CLPFItemBuyReq.super:__init(t)
    self.mid = 2
    self.pid = 8
end
function _G.CLPFItemBuyReq:deserialize(br)
    self.item = CLPFItemInfo_deserialize(br)
end
function _G.CLPFItemBuyReq:serialize(bw)
    CLPFItemInfo_serialize(self.item, bw)
end

--物品购买回应
--errcode number 0购买成功 1购买数量非法 2物品不存在 3该道具不卖 4资源不足 5vip等级不足
--item ItemInfo 获得的物品，仅用于显示
_G.CLPFItemBuyAck = _G.class(ProtocolBase)
function _G.CLPFItemBuyAck:__init(t)
    _G.CLPFItemBuyAck.super:__init(t)
    self.mid = 2
    self.pid = 9
end
function _G.CLPFItemBuyAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.item = CLPFItemInfo_deserialize(br)
end
function _G.CLPFItemBuyAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    CLPFItemInfo_serialize(self.item, bw)
end

--商城购买次数信息
--shop_id number 商城购买项Id
--buy_count number 购买次数
local function CLPFShopBuyCountItem_deserialize(br)
    local r = {}
    r.shop_id = br:ReadInt32()
    r.buy_count = br:ReadInt32()
    return r
end
local function CLPFShopBuyCountItem_serialize(t, bw)
    bw:WriteInt32(t.shop_id)
    bw:WriteInt32(t.buy_count)
end

--商城购买次数请求
_G.CLPFShopQueryBuyCountReq = _G.class(ProtocolBase)
function _G.CLPFShopQueryBuyCountReq:__init(t)
    _G.CLPFShopQueryBuyCountReq.super:__init(t)
    self.mid = 2
    self.pid = 10
end

--商城购买次数回应
--total_item_len number 总购买次数数组长度
--total_item_array ShopBuyCountItem[100] 总购买次数数组
--today_item_len number 当天购买次数数组长度
--today_item_array ShopBuyCountItem[100] 当天购买次数数组
--today_real_goods_len number 当天兑换实物商品数组长度
--today_real_goods_array ShopBuyCountItem[100] 当天兑换实物商品数组
_G.CLPFShopQueryBuyCountAck = _G.class(ProtocolBase)
function _G.CLPFShopQueryBuyCountAck:__init(t)
    _G.CLPFShopQueryBuyCountAck.super:__init(t)
    self.mid = 2
    self.pid = 11
end
function _G.CLPFShopQueryBuyCountAck:deserialize(br)
    self.total_item_len = br:ReadInt32()
    self.total_item_array = {}
    for i=1,self.total_item_len do
        self.total_item_array[i] = CLPFShopBuyCountItem_deserialize(br)
    end
    self.today_item_len = br:ReadInt32()
    self.today_item_array = {}
    for i=1,self.today_item_len do
        self.today_item_array[i] = CLPFShopBuyCountItem_deserialize(br)
    end
    self.today_real_goods_len = br:ReadInt32()
    self.today_real_goods_array = {}
    for i=1,self.today_real_goods_len do
        self.today_real_goods_array[i] = CLPFShopBuyCountItem_deserialize(br)
    end
end
function _G.CLPFShopQueryBuyCountAck:serialize(bw)
    assert(not self.total_item_array or #self.total_item_array <= 100, "CLPFShopQueryBuyCountAck.total_item_array数组长度超过规定限制! 期望:100 实际:" .. #self.total_item_array)
    bw:WriteInt32(#(self.total_item_array or {}))
    for i=1,#(self.total_item_array or {}) do
        CLPFShopBuyCountItem_serialize(self.total_item_array[i], bw)
    end
    assert(not self.today_item_array or #self.today_item_array <= 100, "CLPFShopQueryBuyCountAck.today_item_array数组长度超过规定限制! 期望:100 实际:" .. #self.today_item_array)
    bw:WriteInt32(#(self.today_item_array or {}))
    for i=1,#(self.today_item_array or {}) do
        CLPFShopBuyCountItem_serialize(self.today_item_array[i], bw)
    end
    assert(not self.today_real_goods_array or #self.today_real_goods_array <= 100, "CLPFShopQueryBuyCountAck.today_real_goods_array数组长度超过规定限制! 期望:100 实际:" .. #self.today_real_goods_array)
    bw:WriteInt32(#(self.today_real_goods_array or {}))
    for i=1,#(self.today_real_goods_array or {}) do
        CLPFShopBuyCountItem_serialize(self.today_real_goods_array[i], bw)
    end
end

--商城购买请求
--shop_id number 商城购买项Id
_G.CLPFShopBuyReq = _G.class(ProtocolBase)
function _G.CLPFShopBuyReq:__init(t)
    _G.CLPFShopBuyReq.super:__init(t)
    self.mid = 2
    self.pid = 12
end
function _G.CLPFShopBuyReq:deserialize(br)
    self.shop_id = br:ReadInt32()
end
function _G.CLPFShopBuyReq:serialize(bw)
    bw:WriteInt32(self.shop_id)
end

--商城购买回应
--errcode number 0成功 1无此购买项 2资源不足 3购买次数已达上限 4vip等级不足 5系统错误
--item_len number 数组长度
--items ItemInfo[100] 物品数组
_G.CLPFShopBuyAck = _G.class(ProtocolBase)
function _G.CLPFShopBuyAck:__init(t)
    _G.CLPFShopBuyAck.super:__init(t)
    self.mid = 2
    self.pid = 13
end
function _G.CLPFShopBuyAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.item_len = br:ReadInt32()
    self.items = {}
    for i=1,self.item_len do
        self.items[i] = CLPFItemInfo_deserialize(br)
    end
end
function _G.CLPFShopBuyAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.items or #self.items <= 100, "CLPFShopBuyAck.items数组长度超过规定限制! 期望:100 实际:" .. #self.items)
    bw:WriteInt32(#(self.items or {}))
    for i=1,#(self.items or {}) do
        CLPFItemInfo_serialize(self.items[i], bw)
    end
end

--通用充值请求
--content_type number 购买内容类型 1商城充值 2购买月卡 3首充礼包 4每日充值 5投资炮倍 6出海保险
--content_id number 购买内容Id
--pay_mode number 支付渠道 1微信支付 2支付宝 3支付猫微信 4支付猫支付宝 5聚合微信 6聚合支付宝
_G.CLPFRechargeReq = _G.class(ProtocolBase)
function _G.CLPFRechargeReq:__init(t)
    _G.CLPFRechargeReq.super:__init(t)
    self.mid = 2
    self.pid = 14
end
function _G.CLPFRechargeReq:deserialize(br)
    self.content_type = br:ReadInt32()
    self.content_id = br:ReadInt32()
    self.pay_mode = br:ReadInt32()
end
function _G.CLPFRechargeReq:serialize(bw)
    bw:WriteInt32(self.content_type)
    bw:WriteInt32(self.content_id)
    bw:WriteInt32(self.pay_mode)
end

--通用充值回应
--errcode number 0成功 1无此购买项 2不支持的支付渠道 3购买次数已达上限 4vip等级不足 5系统错误
--pay_envir string[4096] 支付环境，json格式字符串
_G.CLPFRechargeAck = _G.class(ProtocolBase)
function _G.CLPFRechargeAck:__init(t)
    _G.CLPFRechargeAck.super:__init(t)
    self.mid = 2
    self.pid = 15
end
function _G.CLPFRechargeAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.pay_envir = br:ReadString()
end
function _G.CLPFRechargeAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteString(self.pay_envir, 4096)
end

--充值到账通知
--content_type number 购买内容类型 1商城充值 2购买月卡 3首充礼包 4每日充值 5投资炮倍 6出海保险
--content_id number 购买内容Id
--item_len number 获得物品数组长度
--items ItemInfo[100] 获得物品数组
_G.CLPFRechargeSuccessNtf = _G.class(ProtocolBase)
function _G.CLPFRechargeSuccessNtf:__init(t)
    _G.CLPFRechargeSuccessNtf.super:__init(t)
    self.mid = 2
    self.pid = 16
end
function _G.CLPFRechargeSuccessNtf:deserialize(br)
    self.content_type = br:ReadInt32()
    self.content_id = br:ReadInt32()
    self.item_len = br:ReadInt32()
    self.items = {}
    for i=1,self.item_len do
        self.items[i] = CLPFItemInfo_deserialize(br)
    end
end
function _G.CLPFRechargeSuccessNtf:serialize(bw)
    bw:WriteInt32(self.content_type)
    bw:WriteInt32(self.content_id)
    assert(not self.items or #self.items <= 100, "CLPFRechargeSuccessNtf.items数组长度超过规定限制! 期望:100 实际:" .. #self.items)
    bw:WriteInt32(#(self.items or {}))
    for i=1,#(self.items or {}) do
        CLPFItemInfo_serialize(self.items[i], bw)
    end
end

--排行榜玩家信息
--user_id number 玩家Id
--nickname string[32] 昵称
--gender number 性别 0保密 1男 2女
--head number 头像Id
--head_frame number 头像框Id
--level number 玩家等级
--vip_level number 玩家vip等级
--rank_value number 平台货币
local function CLPFRankPlayerInfo_deserialize(br)
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
local function CLPFRankPlayerInfo_serialize(t, bw)
    bw:WriteInt32(t.user_id)
    bw:WriteString(t.nickname, 32)
    bw:WriteInt32(t.gender)
    bw:WriteInt32(t.head)
    bw:WriteInt32(t.head_frame)
    bw:WriteInt32(t.level)
    bw:WriteInt32(t.vip_level)
    bw:WriteInt64(t.rank_value)
end

--获取排行榜请求
--rank_type number 排行榜类型 1金币榜 2弹头榜
_G.CLPFGetRankListReq = _G.class(ProtocolBase)
function _G.CLPFGetRankListReq:__init(t)
    _G.CLPFGetRankListReq.super:__init(t)
    self.mid = 2
    self.pid = 17
end
function _G.CLPFGetRankListReq:deserialize(br)
    self.rank_type = br:ReadInt32()
end
function _G.CLPFGetRankListReq:serialize(bw)
    bw:WriteInt32(self.rank_type)
end

--获取排行榜回应
--errcode number 0成功 1系统错误
--rank_len number 数组长度
--rank_rows RankPlayerInfo[100] 排行榜数据数组
_G.CLPFGetRankListAck = _G.class(ProtocolBase)
function _G.CLPFGetRankListAck:__init(t)
    _G.CLPFGetRankListAck.super:__init(t)
    self.mid = 2
    self.pid = 18
end
function _G.CLPFGetRankListAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.rank_len = br:ReadInt32()
    self.rank_rows = {}
    for i=1,self.rank_len do
        self.rank_rows[i] = CLPFRankPlayerInfo_deserialize(br)
    end
end
function _G.CLPFGetRankListAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.rank_rows or #self.rank_rows <= 100, "CLPFGetRankListAck.rank_rows数组长度超过规定限制! 期望:100 实际:" .. #self.rank_rows)
    bw:WriteInt32(#(self.rank_rows or {}))
    for i=1,#(self.rank_rows or {}) do
        CLPFRankPlayerInfo_serialize(self.rank_rows[i], bw)
    end
end

--玩家经验变化通知
--level_exp number 玩家等级经验
_G.CLPFLevelExpChangedNtf = _G.class(ProtocolBase)
function _G.CLPFLevelExpChangedNtf:__init(t)
    _G.CLPFLevelExpChangedNtf.super:__init(t)
    self.mid = 2
    self.pid = 19
end
function _G.CLPFLevelExpChangedNtf:deserialize(br)
    self.level_exp = br:ReadInt64()
end
function _G.CLPFLevelExpChangedNtf:serialize(bw)
    bw:WriteInt64(self.level_exp)
end

--玩家升级通知
--level number 玩家等级
--reward_len number 升级奖励物品数组长度
--reward_array ItemInfo[10] 升级奖励物品数组
_G.CLPFLevelUpNtf = _G.class(ProtocolBase)
function _G.CLPFLevelUpNtf:__init(t)
    _G.CLPFLevelUpNtf.super:__init(t)
    self.mid = 2
    self.pid = 20
end
function _G.CLPFLevelUpNtf:deserialize(br)
    self.level = br:ReadInt32()
    self.reward_len = br:ReadInt32()
    self.reward_array = {}
    for i=1,self.reward_len do
        self.reward_array[i] = CLPFItemInfo_deserialize(br)
    end
end
function _G.CLPFLevelUpNtf:serialize(bw)
    bw:WriteInt32(self.level)
    assert(not self.reward_array or #self.reward_array <= 10, "CLPFLevelUpNtf.reward_array数组长度超过规定限制! 期望:10 实际:" .. #self.reward_array)
    bw:WriteInt32(#(self.reward_array or {}))
    for i=1,#(self.reward_array or {}) do
        CLPFItemInfo_serialize(self.reward_array[i], bw)
    end
end

--Vip经验等级发生变化
--vip_level number 玩家vip等级
--vip_level_exp number 玩家vip等级经验
_G.CLPFVipExpChangedNtf = _G.class(ProtocolBase)
function _G.CLPFVipExpChangedNtf:__init(t)
    _G.CLPFVipExpChangedNtf.super:__init(t)
    self.mid = 2
    self.pid = 21
end
function _G.CLPFVipExpChangedNtf:deserialize(br)
    self.vip_level = br:ReadInt32()
    self.vip_level_exp = br:ReadInt64()
end
function _G.CLPFVipExpChangedNtf:serialize(bw)
    bw:WriteInt32(self.vip_level)
    bw:WriteInt64(self.vip_level_exp)
end

--修改昵称请求
--new_nickname string[32] 新的昵称
_G.CLPFModifyNicknameReq = _G.class(ProtocolBase)
function _G.CLPFModifyNicknameReq:__init(t)
    _G.CLPFModifyNicknameReq.super:__init(t)
    self.mid = 2
    self.pid = 22
end
function _G.CLPFModifyNicknameReq:deserialize(br)
    self.new_nickname = br:ReadString()
end
function _G.CLPFModifyNicknameReq:serialize(bw)
    bw:WriteString(self.new_nickname, 32)
end

--修改昵称回应
--errcode number 0成功 1格式不合法 2包含敏感字符 3昵称已存在 4钻石不足
_G.CLPFModifyNicknameAck = _G.class(ProtocolBase)
function _G.CLPFModifyNicknameAck:__init(t)
    _G.CLPFModifyNicknameAck.super:__init(t)
    self.mid = 2
    self.pid = 23
end
function _G.CLPFModifyNicknameAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLPFModifyNicknameAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--修改头像请求
--new_head number 新的头像Id
_G.CLPFModifyHeadReq = _G.class(ProtocolBase)
function _G.CLPFModifyHeadReq:__init(t)
    _G.CLPFModifyHeadReq.super:__init(t)
    self.mid = 2
    self.pid = 24
end
function _G.CLPFModifyHeadReq:deserialize(br)
    self.new_head = br:ReadInt32()
end
function _G.CLPFModifyHeadReq:serialize(bw)
    bw:WriteInt32(self.new_head)
end

--修改头像回应
--errcode number 0成功
_G.CLPFModifyHeadAck = _G.class(ProtocolBase)
function _G.CLPFModifyHeadAck:__init(t)
    _G.CLPFModifyHeadAck.super:__init(t)
    self.mid = 2
    self.pid = 25
end
function _G.CLPFModifyHeadAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLPFModifyHeadAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--查询签到请求
_G.CLPFQuerySignReq = _G.class(ProtocolBase)
function _G.CLPFQuerySignReq:__init(t)
    _G.CLPFQuerySignReq.super:__init(t)
    self.mid = 2
    self.pid = 26
end

--查询签到回应
--signed_count number 当月已签次数
--today_signed number 当天是否已签
--total_days number 当月总天数
_G.CLPFQuerySignAck = _G.class(ProtocolBase)
function _G.CLPFQuerySignAck:__init(t)
    _G.CLPFQuerySignAck.super:__init(t)
    self.mid = 2
    self.pid = 27
end
function _G.CLPFQuerySignAck:deserialize(br)
    self.signed_count = br:ReadInt32()
    self.today_signed = br:ReadUInt8()
    self.total_days = br:ReadInt32()
end
function _G.CLPFQuerySignAck:serialize(bw)
    bw:WriteInt32(self.signed_count)
    bw:WriteUInt8(self.today_signed)
    bw:WriteInt32(self.total_days)
end

--执行签到请求
_G.CLPFActSignReq = _G.class(ProtocolBase)
function _G.CLPFActSignReq:__init(t)
    _G.CLPFActSignReq.super:__init(t)
    self.mid = 2
    self.pid = 28
end

--执行签到回应
--errcode number 0成功 1重复签到 2配置表错误
--item_len number 数组长度
--items ItemInfo[10] 物品数组
_G.CLPFActSignAck = _G.class(ProtocolBase)
function _G.CLPFActSignAck:__init(t)
    _G.CLPFActSignAck.super:__init(t)
    self.mid = 2
    self.pid = 29
end
function _G.CLPFActSignAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.item_len = br:ReadInt32()
    self.items = {}
    for i=1,self.item_len do
        self.items[i] = CLPFItemInfo_deserialize(br)
    end
end
function _G.CLPFActSignAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.items or #self.items <= 10, "CLPFActSignAck.items数组长度超过规定限制! 期望:10 实际:" .. #self.items)
    bw:WriteInt32(#(self.items or {}))
    for i=1,#(self.items or {}) do
        CLPFItemInfo_serialize(self.items[i], bw)
    end
end

--查询vip抽奖请求
_G.CLPFQueryVipWheelReq = _G.class(ProtocolBase)
function _G.CLPFQueryVipWheelReq:__init(t)
    _G.CLPFQueryVipWheelReq.super:__init(t)
    self.mid = 2
    self.pid = 30
end

--查询vip抽奖回应
--used_count number 当天已抽奖次数
--total_count number 可抽奖总次数
_G.CLPFQueryVipWheelAck = _G.class(ProtocolBase)
function _G.CLPFQueryVipWheelAck:__init(t)
    _G.CLPFQueryVipWheelAck.super:__init(t)
    self.mid = 2
    self.pid = 31
end
function _G.CLPFQueryVipWheelAck:deserialize(br)
    self.used_count = br:ReadInt32()
    self.total_count = br:ReadInt32()
end
function _G.CLPFQueryVipWheelAck:serialize(bw)
    bw:WriteInt32(self.used_count)
    bw:WriteInt32(self.total_count)
end

--执行vip抽奖请求
_G.CLPFActVipWheelReq = _G.class(ProtocolBase)
function _G.CLPFActVipWheelReq:__init(t)
    _G.CLPFActVipWheelReq.super:__init(t)
    self.mid = 2
    self.pid = 32
end

--执行vip抽奖回应
--errcode number 0成功 1抽奖次数不足 2金币不足 3配置表错误
--reward_id number 随机到的结果奖项Id
--item_len number 数组长度
--items ItemInfo[10] 物品数组
_G.CLPFActVipWheelAck = _G.class(ProtocolBase)
function _G.CLPFActVipWheelAck:__init(t)
    _G.CLPFActVipWheelAck.super:__init(t)
    self.mid = 2
    self.pid = 33
end
function _G.CLPFActVipWheelAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.reward_id = br:ReadInt32()
    self.item_len = br:ReadInt32()
    self.items = {}
    for i=1,self.item_len do
        self.items[i] = CLPFItemInfo_deserialize(br)
    end
end
function _G.CLPFActVipWheelAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt32(self.reward_id)
    assert(not self.items or #self.items <= 10, "CLPFActVipWheelAck.items数组长度超过规定限制! 期望:10 实际:" .. #self.items)
    bw:WriteInt32(#(self.items or {}))
    for i=1,#(self.items or {}) do
        CLPFItemInfo_serialize(self.items[i], bw)
    end
end

--邮件信息结构
--id number 邮件唯一Id
--type number 邮件类型 1系统邮件 2好友赠送邮件
--title string[256] 邮件标题
--content string[512] 邮件内容
--item_len number 附件物品长度
--items ItemInfo[10] 附件物品数组
--state number 邮件状态 1未读 2已读 3已领取
--receive_time number 邮件接收时间戳
--expire_time number 邮件过期时间戳
local function CLPFMailInfo_deserialize(br)
    local r = {}
    r.id = br:ReadInt32()
    r.type = br:ReadInt8()
    r.title = br:ReadString()
    r.content = br:ReadString()
    r.item_len = br:ReadInt32()
    r.items = {}
    for i=1,r.item_len do
        r.items[i] = CLPFItemInfo_deserialize(br)
    end
    r.state = br:ReadInt8()
    r.receive_time = br:ReadUInt32()
    r.expire_time = br:ReadUInt32()
    return r
end
local function CLPFMailInfo_serialize(t, bw)
    bw:WriteInt32(t.id)
    bw:WriteInt8(t.type)
    bw:WriteString(t.title, 256)
    bw:WriteString(t.content, 512)
    assert(not t.items or #t.items <= 10, "CLPFMailInfo.items数组长度超过规定限制! 期望:10 实际:" .. #t.items)
    bw:WriteInt32(#(t.items or {}))
    for i=1,#(t.items or {}) do
        CLPFItemInfo_serialize(t.items[i], bw)
    end
    bw:WriteInt8(t.state)
    bw:WriteUInt32(t.receive_time)
    bw:WriteUInt32(t.expire_time)
end

--所有邮件Id请求
_G.CLPFMailQueryAllIdsReq = _G.class(ProtocolBase)
function _G.CLPFMailQueryAllIdsReq:__init(t)
    _G.CLPFMailQueryAllIdsReq.super:__init(t)
    self.mid = 2
    self.pid = 34
end

--所有邮件Id回应
--len number Id数组长度
--array number[1000] 邮件Id数组
_G.CLPFMailQueryAllIdsAck = _G.class(ProtocolBase)
function _G.CLPFMailQueryAllIdsAck:__init(t)
    _G.CLPFMailQueryAllIdsAck.super:__init(t)
    self.mid = 2
    self.pid = 35
end
function _G.CLPFMailQueryAllIdsAck:deserialize(br)
    self.len = br:ReadInt32()
    self.array = {}
    for i=1,self.len do
        self.array[i] = br:ReadInt32()
    end
end
function _G.CLPFMailQueryAllIdsAck:serialize(bw)
    assert(not self.array or #self.array <= 1000, "CLPFMailQueryAllIdsAck.array数组长度超过规定限制! 期望:1000 实际:" .. #self.array)
    bw:WriteInt32(#(self.array or {}))
    for i=1,#(self.array or {}) do
        bw:WriteInt32(self.array[i])
    end
end

--批量邮件内容请求
--len number Id数组长度
--array number[100] 邮件Id数组长度
--language string[16] 期望显示哪种语言？'CN'为中文
_G.CLPFMailBatchQueryContentReq = _G.class(ProtocolBase)
function _G.CLPFMailBatchQueryContentReq:__init(t)
    _G.CLPFMailBatchQueryContentReq.super:__init(t)
    self.mid = 2
    self.pid = 36
end
function _G.CLPFMailBatchQueryContentReq:deserialize(br)
    self.len = br:ReadInt32()
    self.array = {}
    for i=1,self.len do
        self.array[i] = br:ReadInt32()
    end
    self.language = br:ReadString()
end
function _G.CLPFMailBatchQueryContentReq:serialize(bw)
    assert(not self.array or #self.array <= 100, "CLPFMailBatchQueryContentReq.array数组长度超过规定限制! 期望:100 实际:" .. #self.array)
    bw:WriteInt32(#(self.array or {}))
    for i=1,#(self.array or {}) do
        bw:WriteInt32(self.array[i])
    end
    bw:WriteString(self.language, 16)
end

--批量邮件内容回应
--invalid_len number 无效Id数组长度
--invalid_array number[100] 无效Id数组
--result_len number 结果数组长度
--result_array MailInfo[100] 结果邮件数组
_G.CLPFMailBatchQueryContentAck = _G.class(ProtocolBase)
function _G.CLPFMailBatchQueryContentAck:__init(t)
    _G.CLPFMailBatchQueryContentAck.super:__init(t)
    self.mid = 2
    self.pid = 37
end
function _G.CLPFMailBatchQueryContentAck:deserialize(br)
    self.invalid_len = br:ReadInt32()
    self.invalid_array = {}
    for i=1,self.invalid_len do
        self.invalid_array[i] = br:ReadInt32()
    end
    self.result_len = br:ReadInt32()
    self.result_array = {}
    for i=1,self.result_len do
        self.result_array[i] = CLPFMailInfo_deserialize(br)
    end
end
function _G.CLPFMailBatchQueryContentAck:serialize(bw)
    assert(not self.invalid_array or #self.invalid_array <= 100, "CLPFMailBatchQueryContentAck.invalid_array数组长度超过规定限制! 期望:100 实际:" .. #self.invalid_array)
    bw:WriteInt32(#(self.invalid_array or {}))
    for i=1,#(self.invalid_array or {}) do
        bw:WriteInt32(self.invalid_array[i])
    end
    assert(not self.result_array or #self.result_array <= 100, "CLPFMailBatchQueryContentAck.result_array数组长度超过规定限制! 期望:100 实际:" .. #self.result_array)
    bw:WriteInt32(#(self.result_array or {}))
    for i=1,#(self.result_array or {}) do
        CLPFMailInfo_serialize(self.result_array[i], bw)
    end
end

--查看邮件请求
--mail_id number 邮件唯一Id
_G.CLPFMailAccessReq = _G.class(ProtocolBase)
function _G.CLPFMailAccessReq:__init(t)
    _G.CLPFMailAccessReq.super:__init(t)
    self.mid = 2
    self.pid = 38
end
function _G.CLPFMailAccessReq:deserialize(br)
    self.mail_id = br:ReadInt32()
end
function _G.CLPFMailAccessReq:serialize(bw)
    bw:WriteInt32(self.mail_id)
end

--查看邮件回应
--has_unread_mail number 是否还有未读邮件 1是0否
_G.CLPFMailAccessAck = _G.class(ProtocolBase)
function _G.CLPFMailAccessAck:__init(t)
    _G.CLPFMailAccessAck.super:__init(t)
    self.mid = 2
    self.pid = 39
end
function _G.CLPFMailAccessAck:deserialize(br)
    self.has_unread_mail = br:ReadInt8()
end
function _G.CLPFMailAccessAck:serialize(bw)
    bw:WriteInt8(self.has_unread_mail)
end

--领取邮件物品请求
--mail_id number 邮件唯一Id
_G.CLPFMailFetchItemReq = _G.class(ProtocolBase)
function _G.CLPFMailFetchItemReq:__init(t)
    _G.CLPFMailFetchItemReq.super:__init(t)
    self.mid = 2
    self.pid = 40
end
function _G.CLPFMailFetchItemReq:deserialize(br)
    self.mail_id = br:ReadInt32()
end
function _G.CLPFMailFetchItemReq:serialize(bw)
    bw:WriteInt32(self.mail_id)
end

--领取邮件物品回应
--errcode number 0成功 1邮件不存在 2邮件已被领取过
--item_len number 获得的物品数组长度
--items ItemInfo[10] 获得的物品数组
--has_unread_mail number 是否还有未读邮件 1是0否
_G.CLPFMailFetchItemAck = _G.class(ProtocolBase)
function _G.CLPFMailFetchItemAck:__init(t)
    _G.CLPFMailFetchItemAck.super:__init(t)
    self.mid = 2
    self.pid = 41
end
function _G.CLPFMailFetchItemAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.item_len = br:ReadInt32()
    self.items = {}
    for i=1,self.item_len do
        self.items[i] = CLPFItemInfo_deserialize(br)
    end
    self.has_unread_mail = br:ReadInt8()
end
function _G.CLPFMailFetchItemAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.items or #self.items <= 10, "CLPFMailFetchItemAck.items数组长度超过规定限制! 期望:10 实际:" .. #self.items)
    bw:WriteInt32(#(self.items or {}))
    for i=1,#(self.items or {}) do
        CLPFItemInfo_serialize(self.items[i], bw)
    end
    bw:WriteInt8(self.has_unread_mail)
end

--删除邮件请求
--remove_type number 删除类型 1删除指定邮件 2删除已读且无可领取附件的邮件 3清空所有邮件
--remove_len number 删除邮件数组长度
--remove_ids number[1000] 需要删除的邮件Id数组
_G.CLPFMailRemoveReq = _G.class(ProtocolBase)
function _G.CLPFMailRemoveReq:__init(t)
    _G.CLPFMailRemoveReq.super:__init(t)
    self.mid = 2
    self.pid = 42
end
function _G.CLPFMailRemoveReq:deserialize(br)
    self.remove_type = br:ReadInt8()
    self.remove_len = br:ReadInt32()
    self.remove_ids = {}
    for i=1,self.remove_len do
        self.remove_ids[i] = br:ReadInt32()
    end
end
function _G.CLPFMailRemoveReq:serialize(bw)
    bw:WriteInt8(self.remove_type)
    assert(not self.remove_ids or #self.remove_ids <= 1000, "CLPFMailRemoveReq.remove_ids数组长度超过规定限制! 期望:1000 实际:" .. #self.remove_ids)
    bw:WriteInt32(#(self.remove_ids or {}))
    for i=1,#(self.remove_ids or {}) do
        bw:WriteInt32(self.remove_ids[i])
    end
end

--删除邮件回应
--errcode number 0成功 1包含错误的邮件Id
--has_unread_mail number 是否还有未读邮件 1是0否
--removed_len number 已删除的邮件数组长度
--removed_ids number[1000] 已删除的邮件Id数组
_G.CLPFMailRemoveAck = _G.class(ProtocolBase)
function _G.CLPFMailRemoveAck:__init(t)
    _G.CLPFMailRemoveAck.super:__init(t)
    self.mid = 2
    self.pid = 43
end
function _G.CLPFMailRemoveAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.has_unread_mail = br:ReadInt8()
    self.removed_len = br:ReadInt32()
    self.removed_ids = {}
    for i=1,self.removed_len do
        self.removed_ids[i] = br:ReadInt32()
    end
end
function _G.CLPFMailRemoveAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt8(self.has_unread_mail)
    assert(not self.removed_ids or #self.removed_ids <= 1000, "CLPFMailRemoveAck.removed_ids数组长度超过规定限制! 期望:1000 实际:" .. #self.removed_ids)
    bw:WriteInt32(#(self.removed_ids or {}))
    for i=1,#(self.removed_ids or {}) do
        bw:WriteInt32(self.removed_ids[i])
    end
end

--邮件到来通知
--mail_info MailInfo 邮件信息
_G.CLPFMailArriveNtf = _G.class(ProtocolBase)
function _G.CLPFMailArriveNtf:__init(t)
    _G.CLPFMailArriveNtf.super:__init(t)
    self.mid = 2
    self.pid = 44
end
function _G.CLPFMailArriveNtf:deserialize(br)
    self.mail_info = CLPFMailInfo_deserialize(br)
end
function _G.CLPFMailArriveNtf:serialize(bw)
    CLPFMailInfo_serialize(self.mail_info, bw)
end

--公会信息结构
--id number 公会Id
--name string[64] 公会名称
--desc string[256] 公会宣言
--icon number 公会徽章
--level number 公会等级
--user_level_limit number 入会玩家等级限制
--vip_level_limit number 入会贵族等级限制
--allow_auto_join number 是否允许自动加入 1是0否
--member_count number 公会当前成员数量
--member_limit number 公会最大成员限制
--president_id number 会长Id
--president_name string[32] 会长昵称
local function CLPFGuildInfo_deserialize(br)
    local r = {}
    r.id = br:ReadInt32()
    r.name = br:ReadString()
    r.desc = br:ReadString()
    r.icon = br:ReadInt32()
    r.level = br:ReadInt32()
    r.user_level_limit = br:ReadInt32()
    r.vip_level_limit = br:ReadInt32()
    r.allow_auto_join = br:ReadInt8()
    r.member_count = br:ReadInt32()
    r.member_limit = br:ReadInt32()
    r.president_id = br:ReadInt32()
    r.president_name = br:ReadString()
    return r
end
local function CLPFGuildInfo_serialize(t, bw)
    bw:WriteInt32(t.id)
    bw:WriteString(t.name, 64)
    bw:WriteString(t.desc, 256)
    bw:WriteInt32(t.icon)
    bw:WriteInt32(t.level)
    bw:WriteInt32(t.user_level_limit)
    bw:WriteInt32(t.vip_level_limit)
    bw:WriteInt8(t.allow_auto_join)
    bw:WriteInt32(t.member_count)
    bw:WriteInt32(t.member_limit)
    bw:WriteInt32(t.president_id)
    bw:WriteString(t.president_name, 32)
end

--请求加入公会的信息结构
--user_id number 玩家Id
--nickname string[32] 昵称
--gender number 性别 0保密 1男 2女
--head number 头像Id
--head_frame number 头像框Id
--level number 玩家等级
--vip_level number 玩家vip等级
--request_time number 申请加入的时间戳
local function CLPFGuildJoinItem_deserialize(br)
    local r = {}
    r.user_id = br:ReadInt32()
    r.nickname = br:ReadString()
    r.gender = br:ReadInt32()
    r.head = br:ReadInt32()
    r.head_frame = br:ReadInt32()
    r.level = br:ReadInt32()
    r.vip_level = br:ReadInt32()
    r.request_time = br:ReadUInt32()
    return r
end
local function CLPFGuildJoinItem_serialize(t, bw)
    bw:WriteInt32(t.user_id)
    bw:WriteString(t.nickname, 32)
    bw:WriteInt32(t.gender)
    bw:WriteInt32(t.head)
    bw:WriteInt32(t.head_frame)
    bw:WriteInt32(t.level)
    bw:WriteInt32(t.vip_level)
    bw:WriteUInt32(t.request_time)
end

--公会成员信息结构
--user_id number 玩家Id
--nickname string[32] 昵称
--gender number 性别 0保密 1男 2女
--head number 头像Id
--head_frame number 头像框Id
--level number 玩家等级
--vip_level number 玩家vip等级
--job number 1会长 2管理员 3会员
--is_online number 是否在线 1是0否
--last_login_time number 最近登录时间戳
--contribute number 昨日贡献总金币数量
local function CLPFGuildMember_deserialize(br)
    local r = {}
    r.user_id = br:ReadInt32()
    r.nickname = br:ReadString()
    r.gender = br:ReadInt32()
    r.head = br:ReadInt32()
    r.head_frame = br:ReadInt32()
    r.level = br:ReadInt32()
    r.vip_level = br:ReadInt32()
    r.job = br:ReadInt8()
    r.is_online = br:ReadInt8()
    r.last_login_time = br:ReadUInt32()
    r.contribute = br:ReadInt64()
    return r
end
local function CLPFGuildMember_serialize(t, bw)
    bw:WriteInt32(t.user_id)
    bw:WriteString(t.nickname, 32)
    bw:WriteInt32(t.gender)
    bw:WriteInt32(t.head)
    bw:WriteInt32(t.head_frame)
    bw:WriteInt32(t.level)
    bw:WriteInt32(t.vip_level)
    bw:WriteInt8(t.job)
    bw:WriteInt8(t.is_online)
    bw:WriteUInt32(t.last_login_time)
    bw:WriteInt64(t.contribute)
end

--公会红包成员信息结构
--user_id number 玩家Id
--nickname string[32] 昵称
--gender number 性别 0保密 1男 2女
--head number 头像Id
--head_frame number 头像框Id
--level number 玩家等级
--vip_level number 玩家vip等级
--job number 1会长 2管理员 3会员
--is_online number 是否在线 1是0否
--grab_count number 今日领取红包次数
--total_grab_result number 今日共计抢到的金币数量
local function CLPFGuildRedpacketMember_deserialize(br)
    local r = {}
    r.user_id = br:ReadInt32()
    r.nickname = br:ReadString()
    r.gender = br:ReadInt32()
    r.head = br:ReadInt32()
    r.head_frame = br:ReadInt32()
    r.level = br:ReadInt32()
    r.vip_level = br:ReadInt32()
    r.job = br:ReadInt8()
    r.is_online = br:ReadInt8()
    r.grab_count = br:ReadInt32()
    r.total_grab_result = br:ReadInt64()
    return r
end
local function CLPFGuildRedpacketMember_serialize(t, bw)
    bw:WriteInt32(t.user_id)
    bw:WriteString(t.nickname, 32)
    bw:WriteInt32(t.gender)
    bw:WriteInt32(t.head)
    bw:WriteInt32(t.head_frame)
    bw:WriteInt32(t.level)
    bw:WriteInt32(t.vip_level)
    bw:WriteInt8(t.job)
    bw:WriteInt8(t.is_online)
    bw:WriteInt32(t.grab_count)
    bw:WriteInt64(t.total_grab_result)
end

--公会仓库物品信息
--item_id number 物品Id
--item_sub_id number 物品子Id
--item_count number 物品数量
local function CLPFGuildBagItem_deserialize(br)
    local r = {}
    r.item_id = br:ReadInt32()
    r.item_sub_id = br:ReadInt32()
    r.item_count = br:ReadInt64()
    return r
end
local function CLPFGuildBagItem_serialize(t, bw)
    bw:WriteInt32(t.item_id)
    bw:WriteInt32(t.item_sub_id)
    bw:WriteInt64(t.item_count)
end

--公会仓库日志信息
--user_id number 玩家Id
--nickname string[32] 玩家昵称
--type number 1存入 2取出
--item_id number 物品Id
--item_sub_id number 物品子Id
--item_count number 物品数量
--timestamp number 时间戳
local function CLPFGuildBagLog_deserialize(br)
    local r = {}
    r.user_id = br:ReadInt32()
    r.nickname = br:ReadString()
    r.type = br:ReadInt8()
    r.item_id = br:ReadInt32()
    r.item_sub_id = br:ReadInt32()
    r.item_count = br:ReadInt64()
    r.timestamp = br:ReadUInt32()
    return r
end
local function CLPFGuildBagLog_serialize(t, bw)
    bw:WriteInt32(t.user_id)
    bw:WriteString(t.nickname, 32)
    bw:WriteInt8(t.type)
    bw:WriteInt32(t.item_id)
    bw:WriteInt32(t.item_sub_id)
    bw:WriteInt64(t.item_count)
    bw:WriteUInt32(t.timestamp)
end

--创建公会请求
--name string[64] 公会名称
--icon number 公会徽章
--user_level_limit number 入会玩家等级限制
--vip_level_limit number 入会贵族等级限制
--allow_auto_join number 是否允许自动加入 1是0否
_G.CLPFGuildCreateReq = _G.class(ProtocolBase)
function _G.CLPFGuildCreateReq:__init(t)
    _G.CLPFGuildCreateReq.super:__init(t)
    self.mid = 2
    self.pid = 45
end
function _G.CLPFGuildCreateReq:deserialize(br)
    self.name = br:ReadString()
    self.icon = br:ReadInt32()
    self.user_level_limit = br:ReadInt32()
    self.vip_level_limit = br:ReadInt32()
    self.allow_auto_join = br:ReadInt8()
end
function _G.CLPFGuildCreateReq:serialize(bw)
    bw:WriteString(self.name, 64)
    bw:WriteInt32(self.icon)
    bw:WriteInt32(self.user_level_limit)
    bw:WriteInt32(self.vip_level_limit)
    bw:WriteInt8(self.allow_auto_join)
end

--创建公会回应
--errcode number 0成功 1已有公会 2公会名称非法 3公会名字已存在 4等级不足 5贵族等级不足 6钻石不足
--info GuildInfo 公会信息
_G.CLPFGuildCreateAck = _G.class(ProtocolBase)
function _G.CLPFGuildCreateAck:__init(t)
    _G.CLPFGuildCreateAck.super:__init(t)
    self.mid = 2
    self.pid = 46
end
function _G.CLPFGuildCreateAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.info = CLPFGuildInfo_deserialize(br)
end
function _G.CLPFGuildCreateAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    CLPFGuildInfo_serialize(self.info, bw)
end

--获取公会推荐列表请求
_G.CLPFGuildQueryRecommendListReq = _G.class(ProtocolBase)
function _G.CLPFGuildQueryRecommendListReq:__init(t)
    _G.CLPFGuildQueryRecommendListReq.super:__init(t)
    self.mid = 2
    self.pid = 47
end

--获取公会推荐列表回应
--len number 推荐列表数组长度
--array GuildInfo[6] 推荐列表数组
--join_flags_len number 申请加入标记数组长度
--join_flags_array number[6] 申请加入标记数组
_G.CLPFGuildQueryRecommendListAck = _G.class(ProtocolBase)
function _G.CLPFGuildQueryRecommendListAck:__init(t)
    _G.CLPFGuildQueryRecommendListAck.super:__init(t)
    self.mid = 2
    self.pid = 48
end
function _G.CLPFGuildQueryRecommendListAck:deserialize(br)
    self.len = br:ReadInt32()
    self.array = {}
    for i=1,self.len do
        self.array[i] = CLPFGuildInfo_deserialize(br)
    end
    self.join_flags_len = br:ReadInt32()
    self.join_flags_array = {}
    for i=1,self.join_flags_len do
        self.join_flags_array[i] = br:ReadInt8()
    end
end
function _G.CLPFGuildQueryRecommendListAck:serialize(bw)
    assert(not self.array or #self.array <= 6, "CLPFGuildQueryRecommendListAck.array数组长度超过规定限制! 期望:6 实际:" .. #self.array)
    bw:WriteInt32(#(self.array or {}))
    for i=1,#(self.array or {}) do
        CLPFGuildInfo_serialize(self.array[i], bw)
    end
    assert(not self.join_flags_array or #self.join_flags_array <= 6, "CLPFGuildQueryRecommendListAck.join_flags_array数组长度超过规定限制! 期望:6 实际:" .. #self.join_flags_array)
    bw:WriteInt32(#(self.join_flags_array or {}))
    for i=1,#(self.join_flags_array or {}) do
        bw:WriteInt8(self.join_flags_array[i])
    end
end

--搜索公会请求
--guild_id number 公会Id
_G.CLPFGuildSearchReq = _G.class(ProtocolBase)
function _G.CLPFGuildSearchReq:__init(t)
    _G.CLPFGuildSearchReq.super:__init(t)
    self.mid = 2
    self.pid = 49
end
function _G.CLPFGuildSearchReq:deserialize(br)
    self.guild_id = br:ReadInt32()
end
function _G.CLPFGuildSearchReq:serialize(bw)
    bw:WriteInt32(self.guild_id)
end

--搜索公会回应
--errcode number 0成功 1公会不存在
--info GuildInfo 公会详细信息
--join_flag number 申请加入标记
_G.CLPFGuildSearchAck = _G.class(ProtocolBase)
function _G.CLPFGuildSearchAck:__init(t)
    _G.CLPFGuildSearchAck.super:__init(t)
    self.mid = 2
    self.pid = 50
end
function _G.CLPFGuildSearchAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.info = CLPFGuildInfo_deserialize(br)
    self.join_flag = br:ReadInt8()
end
function _G.CLPFGuildSearchAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    CLPFGuildInfo_serialize(self.info, bw)
    bw:WriteInt8(self.join_flag)
end

--快速加入公会请求
_G.CLPFGuildQuickJoinReq = _G.class(ProtocolBase)
function _G.CLPFGuildQuickJoinReq:__init(t)
    _G.CLPFGuildQuickJoinReq.super:__init(t)
    self.mid = 2
    self.pid = 51
end

--快速加入公会回应
--errcode number 0成功 1已有公会 2当天退出过公会无法加入 3当日申请次数已达上限 4无满足条件的公会
--info GuildInfo 公会详细信息
_G.CLPFGuildQuickJoinAck = _G.class(ProtocolBase)
function _G.CLPFGuildQuickJoinAck:__init(t)
    _G.CLPFGuildQuickJoinAck.super:__init(t)
    self.mid = 2
    self.pid = 52
end
function _G.CLPFGuildQuickJoinAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.info = CLPFGuildInfo_deserialize(br)
end
function _G.CLPFGuildQuickJoinAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    CLPFGuildInfo_serialize(self.info, bw)
end

--加入公会请求
--guild_id number 申请加入的公会Id
_G.CLPFGuildJoinReq = _G.class(ProtocolBase)
function _G.CLPFGuildJoinReq:__init(t)
    _G.CLPFGuildJoinReq.super:__init(t)
    self.mid = 2
    self.pid = 53
end
function _G.CLPFGuildJoinReq:deserialize(br)
    self.guild_id = br:ReadInt32()
end
function _G.CLPFGuildJoinReq:serialize(bw)
    bw:WriteInt32(self.guild_id)
end

--加入公会回应
--errcode number 0申请成功 1已有公会 2公会不存在 3等级不满足 4贵族等级不满足 5公会成员已满 6重复申请 7当日主动退出不允许加入 8当日申请次数已达上限
_G.CLPFGuildJoinAck = _G.class(ProtocolBase)
function _G.CLPFGuildJoinAck:__init(t)
    _G.CLPFGuildJoinAck.super:__init(t)
    self.mid = 2
    self.pid = 54
end
function _G.CLPFGuildJoinAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLPFGuildJoinAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--获取公会申请列表请求
_G.CLPFGuildQueryJoinListReq = _G.class(ProtocolBase)
function _G.CLPFGuildQueryJoinListReq:__init(t)
    _G.CLPFGuildQueryJoinListReq.super:__init(t)
    self.mid = 2
    self.pid = 55
end

--获取公会申请列表回应
--errcode number 0成功 1没有公会 2权限不足
--item_len number 加入请求数组长度
--item_array GuildJoinItem[100] 加入请求数组
_G.CLPFGuildQueryJoinListAck = _G.class(ProtocolBase)
function _G.CLPFGuildQueryJoinListAck:__init(t)
    _G.CLPFGuildQueryJoinListAck.super:__init(t)
    self.mid = 2
    self.pid = 56
end
function _G.CLPFGuildQueryJoinListAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.item_len = br:ReadInt32()
    self.item_array = {}
    for i=1,self.item_len do
        self.item_array[i] = CLPFGuildJoinItem_deserialize(br)
    end
end
function _G.CLPFGuildQueryJoinListAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.item_array or #self.item_array <= 100, "CLPFGuildQueryJoinListAck.item_array数组长度超过规定限制! 期望:100 实际:" .. #self.item_array)
    bw:WriteInt32(#(self.item_array or {}))
    for i=1,#(self.item_array or {}) do
        CLPFGuildJoinItem_serialize(self.item_array[i], bw)
    end
end

--处理公会加入请求
--user_id number 玩家Id
--agree number 是否同意加入 1是0否
_G.CLPFGuildHandleJoinReq = _G.class(ProtocolBase)
function _G.CLPFGuildHandleJoinReq:__init(t)
    _G.CLPFGuildHandleJoinReq.super:__init(t)
    self.mid = 2
    self.pid = 57
end
function _G.CLPFGuildHandleJoinReq:deserialize(br)
    self.user_id = br:ReadInt32()
    self.agree = br:ReadInt32()
end
function _G.CLPFGuildHandleJoinReq:serialize(bw)
    bw:WriteInt32(self.user_id)
    bw:WriteInt32(self.agree)
end

--处理公会加入回应
--errcode number 0操作成功 1你没有公会 2权限不足 3找不到该申请纪录 4玩家等级不足 5玩家贵族等级不足 6公会成员已满 7玩家已加入其它公会
--new_member GuildMember 新加入的成员信息，只有当同意加入并操作成功时该字段才有意义
_G.CLPFGuildHandleJoinAck = _G.class(ProtocolBase)
function _G.CLPFGuildHandleJoinAck:__init(t)
    _G.CLPFGuildHandleJoinAck.super:__init(t)
    self.mid = 2
    self.pid = 58
end
function _G.CLPFGuildHandleJoinAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.new_member = CLPFGuildMember_deserialize(br)
end
function _G.CLPFGuildHandleJoinAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    CLPFGuildMember_serialize(self.new_member, bw)
end

--加入公会应答通知
--guild_id number 公会Id
--guild_name string[64] 公会名称
--user_id number 操作员玩家Id
--nickname string[32] 操作员玩家昵称
--agree number 是否同意加入公会 1是0否
_G.CLPFGuildJoinResponseNtf = _G.class(ProtocolBase)
function _G.CLPFGuildJoinResponseNtf:__init(t)
    _G.CLPFGuildJoinResponseNtf.super:__init(t)
    self.mid = 2
    self.pid = 59
end
function _G.CLPFGuildJoinResponseNtf:deserialize(br)
    self.guild_id = br:ReadInt32()
    self.guild_name = br:ReadString()
    self.user_id = br:ReadInt32()
    self.nickname = br:ReadString()
    self.agree = br:ReadInt32()
end
function _G.CLPFGuildJoinResponseNtf:serialize(bw)
    bw:WriteInt32(self.guild_id)
    bw:WriteString(self.guild_name, 64)
    bw:WriteInt32(self.user_id)
    bw:WriteString(self.nickname, 32)
    bw:WriteInt32(self.agree)
end

--新的申请加入公会的通知
--user_id number 申请人玩家Id
--nickname string[32] 申请人玩家昵称
_G.CLPFGuildNewJoinRequestNtf = _G.class(ProtocolBase)
function _G.CLPFGuildNewJoinRequestNtf:__init(t)
    _G.CLPFGuildNewJoinRequestNtf.super:__init(t)
    self.mid = 2
    self.pid = 60
end
function _G.CLPFGuildNewJoinRequestNtf:deserialize(br)
    self.user_id = br:ReadInt32()
    self.nickname = br:ReadString()
end
function _G.CLPFGuildNewJoinRequestNtf:serialize(bw)
    bw:WriteInt32(self.user_id)
    bw:WriteString(self.nickname, 32)
end

--获取公会信息请求
_G.CLPFGuildQueryInfoReq = _G.class(ProtocolBase)
function _G.CLPFGuildQueryInfoReq:__init(t)
    _G.CLPFGuildQueryInfoReq.super:__init(t)
    self.mid = 2
    self.pid = 61
end

--获取公会信息回应
--errcode number 0成功 1你没有公会
--info GuildInfo 公会详细信息
--members_len number 公会成员数量
--members_array GuildMember[100] 公会成员数组
--job number 你的职务
_G.CLPFGuildQueryInfoAck = _G.class(ProtocolBase)
function _G.CLPFGuildQueryInfoAck:__init(t)
    _G.CLPFGuildQueryInfoAck.super:__init(t)
    self.mid = 2
    self.pid = 62
end
function _G.CLPFGuildQueryInfoAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.info = CLPFGuildInfo_deserialize(br)
    self.members_len = br:ReadInt32()
    self.members_array = {}
    for i=1,self.members_len do
        self.members_array[i] = CLPFGuildMember_deserialize(br)
    end
    self.job = br:ReadInt32()
end
function _G.CLPFGuildQueryInfoAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    CLPFGuildInfo_serialize(self.info, bw)
    assert(not self.members_array or #self.members_array <= 100, "CLPFGuildQueryInfoAck.members_array数组长度超过规定限制! 期望:100 实际:" .. #self.members_array)
    bw:WriteInt32(#(self.members_array or {}))
    for i=1,#(self.members_array or {}) do
        CLPFGuildMember_serialize(self.members_array[i], bw)
    end
    bw:WriteInt32(self.job)
end

--修改公会信息请求
--name string[64] 公会名称
--desc string[256] 公会宣言
--icon number 公会徽章
--user_level_limit number 入会玩家等级限制
--vip_level_limit number 入会贵族等级限制
--allow_auto_join number 是否允许自动加入 1是0否
_G.CLPFGuildModifyInfoReq = _G.class(ProtocolBase)
function _G.CLPFGuildModifyInfoReq:__init(t)
    _G.CLPFGuildModifyInfoReq.super:__init(t)
    self.mid = 2
    self.pid = 63
end
function _G.CLPFGuildModifyInfoReq:deserialize(br)
    self.name = br:ReadString()
    self.desc = br:ReadString()
    self.icon = br:ReadInt32()
    self.user_level_limit = br:ReadInt32()
    self.vip_level_limit = br:ReadInt32()
    self.allow_auto_join = br:ReadInt8()
end
function _G.CLPFGuildModifyInfoReq:serialize(bw)
    bw:WriteString(self.name, 64)
    bw:WriteString(self.desc, 256)
    bw:WriteInt32(self.icon)
    bw:WriteInt32(self.user_level_limit)
    bw:WriteInt32(self.vip_level_limit)
    bw:WriteInt8(self.allow_auto_join)
end

--修改公会信息回应
--errcode number 0成功 1你没有公会 2权限不足 3公会名称已存在 4钻石不足 5修改次数已达上限
_G.CLPFGuildModifyInfoAck = _G.class(ProtocolBase)
function _G.CLPFGuildModifyInfoAck:__init(t)
    _G.CLPFGuildModifyInfoAck.super:__init(t)
    self.mid = 2
    self.pid = 64
end
function _G.CLPFGuildModifyInfoAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLPFGuildModifyInfoAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--修改成员职务请求
--user_id number 玩家Id
--job number 2管理员 3普通成员
_G.CLPFGuildModifyMemberJobReq = _G.class(ProtocolBase)
function _G.CLPFGuildModifyMemberJobReq:__init(t)
    _G.CLPFGuildModifyMemberJobReq.super:__init(t)
    self.mid = 2
    self.pid = 65
end
function _G.CLPFGuildModifyMemberJobReq:deserialize(br)
    self.user_id = br:ReadInt32()
    self.job = br:ReadInt8()
end
function _G.CLPFGuildModifyMemberJobReq:serialize(bw)
    bw:WriteInt32(self.user_id)
    bw:WriteInt8(self.job)
end

--修改成员职务回应
--errcode number 0成功 1你不是会长 2非法操作
_G.CLPFGuildModifyMemberJobAck = _G.class(ProtocolBase)
function _G.CLPFGuildModifyMemberJobAck:__init(t)
    _G.CLPFGuildModifyMemberJobAck.super:__init(t)
    self.mid = 2
    self.pid = 66
end
function _G.CLPFGuildModifyMemberJobAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLPFGuildModifyMemberJobAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--踢出成员请求
--id_len number 踢出的成员数组长度
--id_array number[100] 踢出的成员数组
_G.CLPFGuildKickMemberReq = _G.class(ProtocolBase)
function _G.CLPFGuildKickMemberReq:__init(t)
    _G.CLPFGuildKickMemberReq.super:__init(t)
    self.mid = 2
    self.pid = 67
end
function _G.CLPFGuildKickMemberReq:deserialize(br)
    self.id_len = br:ReadInt32()
    self.id_array = {}
    for i=1,self.id_len do
        self.id_array[i] = br:ReadInt32()
    end
end
function _G.CLPFGuildKickMemberReq:serialize(bw)
    assert(not self.id_array or #self.id_array <= 100, "CLPFGuildKickMemberReq.id_array数组长度超过规定限制! 期望:100 实际:" .. #self.id_array)
    bw:WriteInt32(#(self.id_array or {}))
    for i=1,#(self.id_array or {}) do
        bw:WriteInt32(self.id_array[i])
    end
end

--踢出成员回应
--errcode number 0成功 1你没有公会 2权限不足 3非法操作
_G.CLPFGuildKickMemberAck = _G.class(ProtocolBase)
function _G.CLPFGuildKickMemberAck:__init(t)
    _G.CLPFGuildKickMemberAck.super:__init(t)
    self.mid = 2
    self.pid = 68
end
function _G.CLPFGuildKickMemberAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLPFGuildKickMemberAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--被踢出通知
--user_id number 操作者玩家Id
--nickname string[32] 操作者玩家昵称
--job number 操作者职务 1会长 2管理员
_G.CLPFGuildKickMemberNtf = _G.class(ProtocolBase)
function _G.CLPFGuildKickMemberNtf:__init(t)
    _G.CLPFGuildKickMemberNtf.super:__init(t)
    self.mid = 2
    self.pid = 69
end
function _G.CLPFGuildKickMemberNtf:deserialize(br)
    self.user_id = br:ReadInt32()
    self.nickname = br:ReadString()
    self.job = br:ReadInt8()
end
function _G.CLPFGuildKickMemberNtf:serialize(bw)
    bw:WriteInt32(self.user_id)
    bw:WriteString(self.nickname, 32)
    bw:WriteInt8(self.job)
end

--自己退出公会请求
_G.CLPFGuildExitReq = _G.class(ProtocolBase)
function _G.CLPFGuildExitReq:__init(t)
    _G.CLPFGuildExitReq.super:__init(t)
    self.mid = 2
    self.pid = 70
end

--自己退出公会回应
--errcode number 0成功 1你没有公会 2会长需先设置管理员才能退出
_G.CLPFGuildExitAck = _G.class(ProtocolBase)
function _G.CLPFGuildExitAck:__init(t)
    _G.CLPFGuildExitAck.super:__init(t)
    self.mid = 2
    self.pid = 71
end
function _G.CLPFGuildExitAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLPFGuildExitAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--公会升级请求
_G.CLPFGuildUpgradeReq = _G.class(ProtocolBase)
function _G.CLPFGuildUpgradeReq:__init(t)
    _G.CLPFGuildUpgradeReq.super:__init(t)
    self.mid = 2
    self.pid = 72
end

--公会升级回应
--errcode number 0成功 1你不是会长 2钻石不足 3已满级
--guild_level number 升级后的公会等级
--guild_max_members number 升级后公会最大成员数量
_G.CLPFGuildUpgradeAck = _G.class(ProtocolBase)
function _G.CLPFGuildUpgradeAck:__init(t)
    _G.CLPFGuildUpgradeAck.super:__init(t)
    self.mid = 2
    self.pid = 73
end
function _G.CLPFGuildUpgradeAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.guild_level = br:ReadInt32()
    self.guild_max_members = br:ReadInt32()
end
function _G.CLPFGuildUpgradeAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt32(self.guild_level)
    bw:WriteInt32(self.guild_max_members)
end

--查询会长福利请求
_G.CLPFGuildQueryWelfareReq = _G.class(ProtocolBase)
function _G.CLPFGuildQueryWelfareReq:__init(t)
    _G.CLPFGuildQueryWelfareReq.super:__init(t)
    self.mid = 2
    self.pid = 74
end

--查询会长福利回应
--errcode number 0成功 1你不是会长
--contribute number 当前实时额度
--welfare number 福利金币总数量
--is_fetched number 0未领取 1已领取过
_G.CLPFGuildQueryWelfareAck = _G.class(ProtocolBase)
function _G.CLPFGuildQueryWelfareAck:__init(t)
    _G.CLPFGuildQueryWelfareAck.super:__init(t)
    self.mid = 2
    self.pid = 75
end
function _G.CLPFGuildQueryWelfareAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.contribute = br:ReadInt64()
    self.welfare = br:ReadInt64()
    self.is_fetched = br:ReadInt8()
end
function _G.CLPFGuildQueryWelfareAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt64(self.contribute)
    bw:WriteInt64(self.welfare)
    bw:WriteInt8(self.is_fetched)
end

--领取会长福利请求
_G.CLPFGuildFetchWelfareReq = _G.class(ProtocolBase)
function _G.CLPFGuildFetchWelfareReq:__init(t)
    _G.CLPFGuildFetchWelfareReq.super:__init(t)
    self.mid = 2
    self.pid = 76
end

--领取会长福利回应
--errcode number 0成功 1你不是会长 2已领取过
--welfare number 获得了多少金币
_G.CLPFGuildFetchWelfareAck = _G.class(ProtocolBase)
function _G.CLPFGuildFetchWelfareAck:__init(t)
    _G.CLPFGuildFetchWelfareAck.super:__init(t)
    self.mid = 2
    self.pid = 77
end
function _G.CLPFGuildFetchWelfareAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.welfare = br:ReadInt64()
end
function _G.CLPFGuildFetchWelfareAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt64(self.welfare)
end

--获取公会红包信息请求
_G.CLPFGuildQueryRedPacketInfoReq = _G.class(ProtocolBase)
function _G.CLPFGuildQueryRedPacketInfoReq:__init(t)
    _G.CLPFGuildQueryRedPacketInfoReq.super:__init(t)
    self.mid = 2
    self.pid = 78
end

--获取公会红包信息回应
--errcode number 0成功 1你没有公会
--today_pool number 今日累积奖池
--past_pool number 昨日累积奖池
--today_give_out number 今日已派发金币数
--left_packet_count number 剩余红包个数
--total_packet_count number 红包总数
--donate_num number 今日奖金鱼捐献次数
--grabed_count number 今日已抢红包次数
--left_grab_count number 剩余可抢红包次数
_G.CLPFGuildQueryRedPacketInfoAck = _G.class(ProtocolBase)
function _G.CLPFGuildQueryRedPacketInfoAck:__init(t)
    _G.CLPFGuildQueryRedPacketInfoAck.super:__init(t)
    self.mid = 2
    self.pid = 79
end
function _G.CLPFGuildQueryRedPacketInfoAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.today_pool = br:ReadInt64()
    self.past_pool = br:ReadInt64()
    self.today_give_out = br:ReadInt64()
    self.left_packet_count = br:ReadInt32()
    self.total_packet_count = br:ReadInt32()
    self.donate_num = br:ReadInt32()
    self.grabed_count = br:ReadInt32()
    self.left_grab_count = br:ReadInt32()
end
function _G.CLPFGuildQueryRedPacketInfoAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt64(self.today_pool)
    bw:WriteInt64(self.past_pool)
    bw:WriteInt64(self.today_give_out)
    bw:WriteInt32(self.left_packet_count)
    bw:WriteInt32(self.total_packet_count)
    bw:WriteInt32(self.donate_num)
    bw:WriteInt32(self.grabed_count)
    bw:WriteInt32(self.left_grab_count)
end

--获取公会红包排行榜请求
_G.CLPFGuildQueryRedPacketRankReq = _G.class(ProtocolBase)
function _G.CLPFGuildQueryRedPacketRankReq:__init(t)
    _G.CLPFGuildQueryRedPacketRankReq.super:__init(t)
    self.mid = 2
    self.pid = 80
end

--获取公会红包排行榜回应
--errcode number 0成功 1你没有公会
--rank_len number 排行榜成员数组长度
--rank_array GuildRedpacketMember[100] 排行榜成员数组
_G.CLPFGuildQueryRedPacketRankAck = _G.class(ProtocolBase)
function _G.CLPFGuildQueryRedPacketRankAck:__init(t)
    _G.CLPFGuildQueryRedPacketRankAck.super:__init(t)
    self.mid = 2
    self.pid = 81
end
function _G.CLPFGuildQueryRedPacketRankAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.rank_len = br:ReadInt32()
    self.rank_array = {}
    for i=1,self.rank_len do
        self.rank_array[i] = CLPFGuildRedpacketMember_deserialize(br)
    end
end
function _G.CLPFGuildQueryRedPacketRankAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.rank_array or #self.rank_array <= 100, "CLPFGuildQueryRedPacketRankAck.rank_array数组长度超过规定限制! 期望:100 实际:" .. #self.rank_array)
    bw:WriteInt32(#(self.rank_array or {}))
    for i=1,#(self.rank_array or {}) do
        CLPFGuildRedpacketMember_serialize(self.rank_array[i], bw)
    end
end

--公会抢红包请求
_G.CLPFGuildActRedPacketReq = _G.class(ProtocolBase)
function _G.CLPFGuildActRedPacketReq:__init(t)
    _G.CLPFGuildActRedPacketReq.super:__init(t)
    self.mid = 2
    self.pid = 82
end

--公会抢红包回应
--errcode number 0成功 1你没有公会 2次数不足 3奖池太小还不能领取红包
--grab_result number 获得的金币数量
_G.CLPFGuildActRedPacketAck = _G.class(ProtocolBase)
function _G.CLPFGuildActRedPacketAck:__init(t)
    _G.CLPFGuildActRedPacketAck.super:__init(t)
    self.mid = 2
    self.pid = 83
end
function _G.CLPFGuildActRedPacketAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.grab_result = br:ReadInt64()
end
function _G.CLPFGuildActRedPacketAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt64(self.grab_result)
end

--获取公会仓库信息请求
_G.CLPFGuildBagQueryInfoReq = _G.class(ProtocolBase)
function _G.CLPFGuildBagQueryInfoReq:__init(t)
    _G.CLPFGuildBagQueryInfoReq.super:__init(t)
    self.mid = 2
    self.pid = 84
end

--获取公会仓库信息回应
--errcode number 0成功 1你没有公会
--item_len number 仓库物品长度
--item_array GuildBagItem[100] 仓库物品数组
--log_len number 仓库日志长度
--log_array GuildBagLog[100] 仓库日志数组
_G.CLPFGuildBagQueryInfoAck = _G.class(ProtocolBase)
function _G.CLPFGuildBagQueryInfoAck:__init(t)
    _G.CLPFGuildBagQueryInfoAck.super:__init(t)
    self.mid = 2
    self.pid = 85
end
function _G.CLPFGuildBagQueryInfoAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.item_len = br:ReadInt32()
    self.item_array = {}
    for i=1,self.item_len do
        self.item_array[i] = CLPFGuildBagItem_deserialize(br)
    end
    self.log_len = br:ReadInt32()
    self.log_array = {}
    for i=1,self.log_len do
        self.log_array[i] = CLPFGuildBagLog_deserialize(br)
    end
end
function _G.CLPFGuildBagQueryInfoAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.item_array or #self.item_array <= 100, "CLPFGuildBagQueryInfoAck.item_array数组长度超过规定限制! 期望:100 实际:" .. #self.item_array)
    bw:WriteInt32(#(self.item_array or {}))
    for i=1,#(self.item_array or {}) do
        CLPFGuildBagItem_serialize(self.item_array[i], bw)
    end
    assert(not self.log_array or #self.log_array <= 100, "CLPFGuildBagQueryInfoAck.log_array数组长度超过规定限制! 期望:100 实际:" .. #self.log_array)
    bw:WriteInt32(#(self.log_array or {}))
    for i=1,#(self.log_array or {}) do
        CLPFGuildBagLog_serialize(self.log_array[i], bw)
    end
end

--获取公会仓库日志请求
--page_index number 每页数量100，请求第几页？
_G.CLPFGuildBagQueryLogReq = _G.class(ProtocolBase)
function _G.CLPFGuildBagQueryLogReq:__init(t)
    _G.CLPFGuildBagQueryLogReq.super:__init(t)
    self.mid = 2
    self.pid = 86
end
function _G.CLPFGuildBagQueryLogReq:deserialize(br)
    self.page_index = br:ReadInt32()
end
function _G.CLPFGuildBagQueryLogReq:serialize(bw)
    bw:WriteInt32(self.page_index)
end

--获取公会仓库日志回应
--errcode number 0成功 1你没有公会
--log_len number 日志数组长度
--log_array GuildBagLog[100] 仓库日志数组
_G.CLPFGuildBagQueryLogAck = _G.class(ProtocolBase)
function _G.CLPFGuildBagQueryLogAck:__init(t)
    _G.CLPFGuildBagQueryLogAck.super:__init(t)
    self.mid = 2
    self.pid = 87
end
function _G.CLPFGuildBagQueryLogAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.log_len = br:ReadInt32()
    self.log_array = {}
    for i=1,self.log_len do
        self.log_array[i] = CLPFGuildBagLog_deserialize(br)
    end
end
function _G.CLPFGuildBagQueryLogAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.log_array or #self.log_array <= 100, "CLPFGuildBagQueryLogAck.log_array数组长度超过规定限制! 期望:100 实际:" .. #self.log_array)
    bw:WriteInt32(#(self.log_array or {}))
    for i=1,#(self.log_array or {}) do
        CLPFGuildBagLog_serialize(self.log_array[i], bw)
    end
end

--公会仓库存入物品请求
--item ItemInfo 存入的物品
_G.CLPFGuildBagStoreItemReq = _G.class(ProtocolBase)
function _G.CLPFGuildBagStoreItemReq:__init(t)
    _G.CLPFGuildBagStoreItemReq.super:__init(t)
    self.mid = 2
    self.pid = 88
end
function _G.CLPFGuildBagStoreItemReq:deserialize(br)
    self.item = CLPFItemInfo_deserialize(br)
end
function _G.CLPFGuildBagStoreItemReq:serialize(bw)
    CLPFItemInfo_serialize(self.item, bw)
end

--公会仓库存入物品回应
--errcode number 0成功 1你没有公会 2不能存入该物品 3vip等级不足 4物品不足
--bag_log GuildBagLog 当前存入仓库动作所对应的日志
_G.CLPFGuildBagStoreItemAck = _G.class(ProtocolBase)
function _G.CLPFGuildBagStoreItemAck:__init(t)
    _G.CLPFGuildBagStoreItemAck.super:__init(t)
    self.mid = 2
    self.pid = 89
end
function _G.CLPFGuildBagStoreItemAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.bag_log = CLPFGuildBagLog_deserialize(br)
end
function _G.CLPFGuildBagStoreItemAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    CLPFGuildBagLog_serialize(self.bag_log, bw)
end

--公会仓库取出物品请求
--item ItemInfo 取出的物品
--user_id number 物品分配给谁？
_G.CLPFGuildBagFetchItemReq = _G.class(ProtocolBase)
function _G.CLPFGuildBagFetchItemReq:__init(t)
    _G.CLPFGuildBagFetchItemReq.super:__init(t)
    self.mid = 2
    self.pid = 90
end
function _G.CLPFGuildBagFetchItemReq:deserialize(br)
    self.item = CLPFItemInfo_deserialize(br)
    self.user_id = br:ReadInt32()
end
function _G.CLPFGuildBagFetchItemReq:serialize(bw)
    CLPFItemInfo_serialize(self.item, bw)
    bw:WriteInt32(self.user_id)
end

--公会仓库取出物品回应
--errcode number 0成功 1你不是会长 2玩家不在公会中 3物品数量不足
--bag_log GuildBagLog 当前取出仓库动作所对应的日志
_G.CLPFGuildBagFetchItemAck = _G.class(ProtocolBase)
function _G.CLPFGuildBagFetchItemAck:__init(t)
    _G.CLPFGuildBagFetchItemAck.super:__init(t)
    self.mid = 2
    self.pid = 91
end
function _G.CLPFGuildBagFetchItemAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.bag_log = CLPFGuildBagLog_deserialize(br)
end
function _G.CLPFGuildBagFetchItemAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    CLPFGuildBagLog_serialize(self.bag_log, bw)
end

--公会领取仓库物品通知
--item ItemInfo 领取的物品，仅用于显示
_G.CLPFGuildBagFetchItemNtf = _G.class(ProtocolBase)
function _G.CLPFGuildBagFetchItemNtf:__init(t)
    _G.CLPFGuildBagFetchItemNtf.super:__init(t)
    self.mid = 2
    self.pid = 92
end
function _G.CLPFGuildBagFetchItemNtf:deserialize(br)
    self.item = CLPFItemInfo_deserialize(br)
end
function _G.CLPFGuildBagFetchItemNtf:serialize(bw)
    CLPFItemInfo_serialize(self.item, bw)
end

--消息广播通知
--type number 消息类型，详细分类请查阅协议配置xml文件中的注释。
--content string[4096] json格式数组，数组中的字段内容根据type而不同
_G.CLPFMessageBroadcastNtf = _G.class(ProtocolBase)
function _G.CLPFMessageBroadcastNtf:__init(t)
    _G.CLPFMessageBroadcastNtf.super:__init(t)
    self.mid = 2
    self.pid = 93
end
function _G.CLPFMessageBroadcastNtf:deserialize(br)
    self.type = br:ReadInt32()
    self.content = br:ReadString()
end
function _G.CLPFMessageBroadcastNtf:serialize(bw)
    bw:WriteInt32(self.type)
    bw:WriteString(self.content, 4096)
end

--完成任务信息
--task_id number 任务Id
--achieve_num number 达成数量
local function CLPFTaskInfo_deserialize(br)
    local r = {}
    r.task_id = br:ReadInt32()
    r.achieve_num = br:ReadInt64()
    return r
end
local function CLPFTaskInfo_serialize(t, bw)
    bw:WriteInt32(t.task_id)
    bw:WriteInt64(t.achieve_num)
end

--查询任务请求
_G.CLPFTaskQueryReq = _G.class(ProtocolBase)
function _G.CLPFTaskQueryReq:__init(t)
    _G.CLPFTaskQueryReq.super:__init(t)
    self.mid = 2
    self.pid = 94
end

--查询任务回应
--task_info_len number 任务信息数组长度
--task_info_array TaskInfo[100] 任务信息数组
--finish_task_id_len number 已完成（并领取奖励）的任务Id数组长度
--finish_task_id_array number[100] 已完成（并领取奖励）的任务Id数组
--daily_active_value number 当前日活跃值
--weekly_active_value number 当前周活跃值
--finish_active_id_len number 已领取奖励的活跃值Id数组长度
--finish_active_id_array number[100] 已领取奖励的活跃值Id数组
_G.CLPFTaskQueryAck = _G.class(ProtocolBase)
function _G.CLPFTaskQueryAck:__init(t)
    _G.CLPFTaskQueryAck.super:__init(t)
    self.mid = 2
    self.pid = 95
end
function _G.CLPFTaskQueryAck:deserialize(br)
    self.task_info_len = br:ReadInt32()
    self.task_info_array = {}
    for i=1,self.task_info_len do
        self.task_info_array[i] = CLPFTaskInfo_deserialize(br)
    end
    self.finish_task_id_len = br:ReadInt32()
    self.finish_task_id_array = {}
    for i=1,self.finish_task_id_len do
        self.finish_task_id_array[i] = br:ReadInt32()
    end
    self.daily_active_value = br:ReadInt32()
    self.weekly_active_value = br:ReadInt32()
    self.finish_active_id_len = br:ReadInt32()
    self.finish_active_id_array = {}
    for i=1,self.finish_active_id_len do
        self.finish_active_id_array[i] = br:ReadInt32()
    end
end
function _G.CLPFTaskQueryAck:serialize(bw)
    assert(not self.task_info_array or #self.task_info_array <= 100, "CLPFTaskQueryAck.task_info_array数组长度超过规定限制! 期望:100 实际:" .. #self.task_info_array)
    bw:WriteInt32(#(self.task_info_array or {}))
    for i=1,#(self.task_info_array or {}) do
        CLPFTaskInfo_serialize(self.task_info_array[i], bw)
    end
    assert(not self.finish_task_id_array or #self.finish_task_id_array <= 100, "CLPFTaskQueryAck.finish_task_id_array数组长度超过规定限制! 期望:100 实际:" .. #self.finish_task_id_array)
    bw:WriteInt32(#(self.finish_task_id_array or {}))
    for i=1,#(self.finish_task_id_array or {}) do
        bw:WriteInt32(self.finish_task_id_array[i])
    end
    bw:WriteInt32(self.daily_active_value)
    bw:WriteInt32(self.weekly_active_value)
    assert(not self.finish_active_id_array or #self.finish_active_id_array <= 100, "CLPFTaskQueryAck.finish_active_id_array数组长度超过规定限制! 期望:100 实际:" .. #self.finish_active_id_array)
    bw:WriteInt32(#(self.finish_active_id_array or {}))
    for i=1,#(self.finish_active_id_array or {}) do
        bw:WriteInt32(self.finish_active_id_array[i])
    end
end

--领取任务奖励请求
--task_id number 任务Id
_G.CLPFTaskFetchTaskRewardsReq = _G.class(ProtocolBase)
function _G.CLPFTaskFetchTaskRewardsReq:__init(t)
    _G.CLPFTaskFetchTaskRewardsReq.super:__init(t)
    self.mid = 2
    self.pid = 96
end
function _G.CLPFTaskFetchTaskRewardsReq:deserialize(br)
    self.task_id = br:ReadInt32()
end
function _G.CLPFTaskFetchTaskRewardsReq:serialize(bw)
    bw:WriteInt32(self.task_id)
end

--领取任务奖励回应
--errcode number 0成功 1任务不存在 2任务目标未达成 3任务奖励已领取
--item_len number 数组长度
--items ItemInfo[100] 物品数组
_G.CLPFTaskFetchTaskRewardsAck = _G.class(ProtocolBase)
function _G.CLPFTaskFetchTaskRewardsAck:__init(t)
    _G.CLPFTaskFetchTaskRewardsAck.super:__init(t)
    self.mid = 2
    self.pid = 97
end
function _G.CLPFTaskFetchTaskRewardsAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.item_len = br:ReadInt32()
    self.items = {}
    for i=1,self.item_len do
        self.items[i] = CLPFItemInfo_deserialize(br)
    end
end
function _G.CLPFTaskFetchTaskRewardsAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.items or #self.items <= 100, "CLPFTaskFetchTaskRewardsAck.items数组长度超过规定限制! 期望:100 实际:" .. #self.items)
    bw:WriteInt32(#(self.items or {}))
    for i=1,#(self.items or {}) do
        CLPFItemInfo_serialize(self.items[i], bw)
    end
end

--领取活跃度奖励请求
--active_id number 活跃度奖励Id
_G.CLPFTaskFetchActiveRewardsReq = _G.class(ProtocolBase)
function _G.CLPFTaskFetchActiveRewardsReq:__init(t)
    _G.CLPFTaskFetchActiveRewardsReq.super:__init(t)
    self.mid = 2
    self.pid = 98
end
function _G.CLPFTaskFetchActiveRewardsReq:deserialize(br)
    self.active_id = br:ReadInt32()
end
function _G.CLPFTaskFetchActiveRewardsReq:serialize(bw)
    bw:WriteInt32(self.active_id)
end

--领取活跃度奖励回应
--errcode number 0成功 1不存在 2目标未达成 3已领取
--item_len number 数组长度
--items ItemInfo[100] 物品数组
_G.CLPFTaskFetchActiveRewardsAck = _G.class(ProtocolBase)
function _G.CLPFTaskFetchActiveRewardsAck:__init(t)
    _G.CLPFTaskFetchActiveRewardsAck.super:__init(t)
    self.mid = 2
    self.pid = 99
end
function _G.CLPFTaskFetchActiveRewardsAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.item_len = br:ReadInt32()
    self.items = {}
    for i=1,self.item_len do
        self.items[i] = CLPFItemInfo_deserialize(br)
    end
end
function _G.CLPFTaskFetchActiveRewardsAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.items or #self.items <= 100, "CLPFTaskFetchActiveRewardsAck.items数组长度超过规定限制! 期望:100 实际:" .. #self.items)
    bw:WriteInt32(#(self.items or {}))
    for i=1,#(self.items or {}) do
        CLPFItemInfo_serialize(self.items[i], bw)
    end
end

--完成的成就任务数据
--kind number 1累计登录 2捕鱼能手 3倍率大人 4竞技高手 5捕鱼累计获得金币
--count number 累计完成数量
local function CLPFTaskAchieveData_deserialize(br)
    local r = {}
    r.kind = br:ReadInt32()
    r.count = br:ReadInt64()
    return r
end
local function CLPFTaskAchieveData_serialize(t, bw)
    bw:WriteInt32(t.kind)
    bw:WriteInt64(t.count)
end

--获取成就任务信息请求
_G.CLPFTaskAchieveQueryInfoReq = _G.class(ProtocolBase)
function _G.CLPFTaskAchieveQueryInfoReq:__init(t)
    _G.CLPFTaskAchieveQueryInfoReq.super:__init(t)
    self.mid = 2
    self.pid = 100
end

--获取成就任务信息回应
--data_len number 任务数据数组长度
--data_array TaskAchieveData[100] 任务数据数组
--finish_id_len number 完成的任务Id数组长度
--finish_id_array number[100] 完成的任务Id数组
_G.CLPFTaskAchieveQueryInfoAck = _G.class(ProtocolBase)
function _G.CLPFTaskAchieveQueryInfoAck:__init(t)
    _G.CLPFTaskAchieveQueryInfoAck.super:__init(t)
    self.mid = 2
    self.pid = 101
end
function _G.CLPFTaskAchieveQueryInfoAck:deserialize(br)
    self.data_len = br:ReadInt32()
    self.data_array = {}
    for i=1,self.data_len do
        self.data_array[i] = CLPFTaskAchieveData_deserialize(br)
    end
    self.finish_id_len = br:ReadInt32()
    self.finish_id_array = {}
    for i=1,self.finish_id_len do
        self.finish_id_array[i] = br:ReadInt32()
    end
end
function _G.CLPFTaskAchieveQueryInfoAck:serialize(bw)
    assert(not self.data_array or #self.data_array <= 100, "CLPFTaskAchieveQueryInfoAck.data_array数组长度超过规定限制! 期望:100 实际:" .. #self.data_array)
    bw:WriteInt32(#(self.data_array or {}))
    for i=1,#(self.data_array or {}) do
        CLPFTaskAchieveData_serialize(self.data_array[i], bw)
    end
    assert(not self.finish_id_array or #self.finish_id_array <= 100, "CLPFTaskAchieveQueryInfoAck.finish_id_array数组长度超过规定限制! 期望:100 实际:" .. #self.finish_id_array)
    bw:WriteInt32(#(self.finish_id_array or {}))
    for i=1,#(self.finish_id_array or {}) do
        bw:WriteInt32(self.finish_id_array[i])
    end
end

--领取成就任务奖励请求
--task_achieve_id number 成就任务Id
_G.CLPFTaskAchieveFetchRewardReq = _G.class(ProtocolBase)
function _G.CLPFTaskAchieveFetchRewardReq:__init(t)
    _G.CLPFTaskAchieveFetchRewardReq.super:__init(t)
    self.mid = 2
    self.pid = 102
end
function _G.CLPFTaskAchieveFetchRewardReq:deserialize(br)
    self.task_achieve_id = br:ReadInt32()
end
function _G.CLPFTaskAchieveFetchRewardReq:serialize(bw)
    bw:WriteInt32(self.task_achieve_id)
end

--领取成就任务奖励回应
--errcode number 0成功 1任务不存在 2任务未达成 3奖励已领取
--item_len number 数组长度
--items ItemInfo[100] 物品数组
_G.CLPFTaskAchieveFetchRewardAck = _G.class(ProtocolBase)
function _G.CLPFTaskAchieveFetchRewardAck:__init(t)
    _G.CLPFTaskAchieveFetchRewardAck.super:__init(t)
    self.mid = 2
    self.pid = 103
end
function _G.CLPFTaskAchieveFetchRewardAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.item_len = br:ReadInt32()
    self.items = {}
    for i=1,self.item_len do
        self.items[i] = CLPFItemInfo_deserialize(br)
    end
end
function _G.CLPFTaskAchieveFetchRewardAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.items or #self.items <= 100, "CLPFTaskAchieveFetchRewardAck.items数组长度超过规定限制! 期望:100 实际:" .. #self.items)
    bw:WriteInt32(#(self.items or {}))
    for i=1,#(self.items or {}) do
        CLPFItemInfo_serialize(self.items[i], bw)
    end
end

--领取月卡奖励请求
_G.CLPFMonthCardFetchRewardReq = _G.class(ProtocolBase)
function _G.CLPFMonthCardFetchRewardReq:__init(t)
    _G.CLPFMonthCardFetchRewardReq.super:__init(t)
    self.mid = 2
    self.pid = 104
end

--领取月卡奖励回应
--errcode number 0成功 1请先购买月卡 2当日已领取 3领取次数不足
--item_len number 数组长度
--items ItemInfo[100] 物品数组
_G.CLPFMonthCardFetchRewardAck = _G.class(ProtocolBase)
function _G.CLPFMonthCardFetchRewardAck:__init(t)
    _G.CLPFMonthCardFetchRewardAck.super:__init(t)
    self.mid = 2
    self.pid = 105
end
function _G.CLPFMonthCardFetchRewardAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.item_len = br:ReadInt32()
    self.items = {}
    for i=1,self.item_len do
        self.items[i] = CLPFItemInfo_deserialize(br)
    end
end
function _G.CLPFMonthCardFetchRewardAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.items or #self.items <= 100, "CLPFMonthCardFetchRewardAck.items数组长度超过规定限制! 期望:100 实际:" .. #self.items)
    bw:WriteInt32(#(self.items or {}))
    for i=1,#(self.items or {}) do
        CLPFItemInfo_serialize(self.items[i], bw)
    end
end

--领取救济金请求
_G.CLPFReliefGoldFetchReq = _G.class(ProtocolBase)
function _G.CLPFReliefGoldFetchReq:__init(t)
    _G.CLPFReliefGoldFetchReq.super:__init(t)
    self.mid = 2
    self.pid = 106
end

--领取救济金回应
--errcode number 0成功 1当前金币不为零 2领取次数已达上限 3你的金库中还有金币不能领取救济金
--currency_delta number 领取到的救济金数量，仅用于显示
_G.CLPFReliefGoldFetchAck = _G.class(ProtocolBase)
function _G.CLPFReliefGoldFetchAck:__init(t)
    _G.CLPFReliefGoldFetchAck.super:__init(t)
    self.mid = 2
    self.pid = 107
end
function _G.CLPFReliefGoldFetchAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.currency_delta = br:ReadInt64()
end
function _G.CLPFReliefGoldFetchAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt64(self.currency_delta)
end

--摇数字获取信息请求
_G.CLPFShakeNumberQueryInfoReq = _G.class(ProtocolBase)
function _G.CLPFShakeNumberQueryInfoReq:__init(t)
    _G.CLPFShakeNumberQueryInfoReq.super:__init(t)
    self.mid = 2
    self.pid = 108
end

--摇数字获取信息回应
--shake_number_len number 已摇到的数字数组长度
--shake_number_array number[7] 已经摇到的数字数组，该数字由两部分组成：个位表示当天摇到的数字，十位为0表示当天尚未领取宝箱奖励，十位为1表示当天已领取宝箱奖励
--shake_number_act_flag number 当天是否已摇过数字 1是0否
--shake_number_fetched number 本轮是否已领取过奖励 1是0否
_G.CLPFShakeNumberQueryInfoAck = _G.class(ProtocolBase)
function _G.CLPFShakeNumberQueryInfoAck:__init(t)
    _G.CLPFShakeNumberQueryInfoAck.super:__init(t)
    self.mid = 2
    self.pid = 109
end
function _G.CLPFShakeNumberQueryInfoAck:deserialize(br)
    self.shake_number_len = br:ReadInt8()
    self.shake_number_array = {}
    for i=1,self.shake_number_len do
        self.shake_number_array[i] = br:ReadInt8()
    end
    self.shake_number_act_flag = br:ReadInt8()
    self.shake_number_fetched = br:ReadInt8()
end
function _G.CLPFShakeNumberQueryInfoAck:serialize(bw)
    assert(not self.shake_number_array or #self.shake_number_array <= 7, "CLPFShakeNumberQueryInfoAck.shake_number_array数组长度超过规定限制! 期望:7 实际:" .. #self.shake_number_array)
    bw:WriteInt8(#(self.shake_number_array or {}))
    for i=1,#(self.shake_number_array or {}) do
        bw:WriteInt8(self.shake_number_array[i])
    end
    bw:WriteInt8(self.shake_number_act_flag)
    bw:WriteInt8(self.shake_number_fetched)
end

--摇数字请求
_G.CLPFShakeNumberActReq = _G.class(ProtocolBase)
function _G.CLPFShakeNumberActReq:__init(t)
    _G.CLPFShakeNumberActReq.super:__init(t)
    self.mid = 2
    self.pid = 110
end

--摇数字回应
--errcode number 0成功 1今天已经摇过 2有奖励尚未领取
--number number 摇到的数字：0~9
_G.CLPFShakeNumberActAck = _G.class(ProtocolBase)
function _G.CLPFShakeNumberActAck:__init(t)
    _G.CLPFShakeNumberActAck.super:__init(t)
    self.mid = 2
    self.pid = 111
end
function _G.CLPFShakeNumberActAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.number = br:ReadInt8()
end
function _G.CLPFShakeNumberActAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt8(self.number)
end

--领取摇到的金币奖励请求
_G.CLPFShakeNumberFetchRewardReq = _G.class(ProtocolBase)
function _G.CLPFShakeNumberFetchRewardReq:__init(t)
    _G.CLPFShakeNumberFetchRewardReq.super:__init(t)
    self.mid = 2
    self.pid = 112
end

--领取摇到的金币奖励回应
--errcode number 0成功 1条件未达成 2已领取过
--currency_delta number 领取到的7日摇数字奖励金币数量，仅用于显示
_G.CLPFShakeNumberFetchRewardAck = _G.class(ProtocolBase)
function _G.CLPFShakeNumberFetchRewardAck:__init(t)
    _G.CLPFShakeNumberFetchRewardAck.super:__init(t)
    self.mid = 2
    self.pid = 113
end
function _G.CLPFShakeNumberFetchRewardAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.currency_delta = br:ReadInt64()
end
function _G.CLPFShakeNumberFetchRewardAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt64(self.currency_delta)
end

--领取摇到数字后的宝箱礼包请求
--day number 第几天的宝箱？范围：0-6
_G.CLPFShakeNumberFetchBoxRewardReq = _G.class(ProtocolBase)
function _G.CLPFShakeNumberFetchBoxRewardReq:__init(t)
    _G.CLPFShakeNumberFetchBoxRewardReq.super:__init(t)
    self.mid = 2
    self.pid = 114
end
function _G.CLPFShakeNumberFetchBoxRewardReq:deserialize(br)
    self.day = br:ReadInt32()
end
function _G.CLPFShakeNumberFetchBoxRewardReq:serialize(bw)
    bw:WriteInt32(self.day)
end

--领取摇到数字后的宝箱礼包回应
--errcode number 0成功 1参数非法 2当天还未摇数字 3已领取过
--item_len number 数组长度
--items ItemInfo[100] 物品数组，仅用于显示
_G.CLPFShakeNumberFetchBoxRewardAck = _G.class(ProtocolBase)
function _G.CLPFShakeNumberFetchBoxRewardAck:__init(t)
    _G.CLPFShakeNumberFetchBoxRewardAck.super:__init(t)
    self.mid = 2
    self.pid = 115
end
function _G.CLPFShakeNumberFetchBoxRewardAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.item_len = br:ReadInt32()
    self.items = {}
    for i=1,self.item_len do
        self.items[i] = CLPFItemInfo_deserialize(br)
    end
end
function _G.CLPFShakeNumberFetchBoxRewardAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.items or #self.items <= 100, "CLPFShakeNumberFetchBoxRewardAck.items数组长度超过规定限制! 期望:100 实际:" .. #self.items)
    bw:WriteInt32(#(self.items or {}))
    for i=1,#(self.items or {}) do
        CLPFItemInfo_serialize(self.items[i], bw)
    end
end

--查询每日充值请求
_G.CLPFRechargeDailyQueryReq = _G.class(ProtocolBase)
function _G.CLPFRechargeDailyQueryReq:__init(t)
    _G.CLPFRechargeDailyQueryReq.super:__init(t)
    self.mid = 2
    self.pid = 116
end

--查询每日充值回应
--finished_id_len number 已完成的每日充值id数组长度
--finished_id_array number[100] 已完成的每日充值id数组
_G.CLPFRechargeDailyQueryAck = _G.class(ProtocolBase)
function _G.CLPFRechargeDailyQueryAck:__init(t)
    _G.CLPFRechargeDailyQueryAck.super:__init(t)
    self.mid = 2
    self.pid = 117
end
function _G.CLPFRechargeDailyQueryAck:deserialize(br)
    self.finished_id_len = br:ReadInt32()
    self.finished_id_array = {}
    for i=1,self.finished_id_len do
        self.finished_id_array[i] = br:ReadInt32()
    end
end
function _G.CLPFRechargeDailyQueryAck:serialize(bw)
    assert(not self.finished_id_array or #self.finished_id_array <= 100, "CLPFRechargeDailyQueryAck.finished_id_array数组长度超过规定限制! 期望:100 实际:" .. #self.finished_id_array)
    bw:WriteInt32(#(self.finished_id_array or {}))
    for i=1,#(self.finished_id_array or {}) do
        bw:WriteInt32(self.finished_id_array[i])
    end
end

--福利猪获取信息请求
_G.CLPFWelfarePigQueryInfoReq = _G.class(ProtocolBase)
function _G.CLPFWelfarePigQueryInfoReq:__init(t)
    _G.CLPFWelfarePigQueryInfoReq.super:__init(t)
    self.mid = 2
    self.pid = 118
end

--福利猪获取信息回应
--welfare number 累积的总福利值
--expire_time number 总福利过期时间戳
--is_fetched number 今日是否已领取锤子碎片 1是0否
--is_broken number 今日是否已砸过罐子 1是0否
_G.CLPFWelfarePigQueryInfoAck = _G.class(ProtocolBase)
function _G.CLPFWelfarePigQueryInfoAck:__init(t)
    _G.CLPFWelfarePigQueryInfoAck.super:__init(t)
    self.mid = 2
    self.pid = 119
end
function _G.CLPFWelfarePigQueryInfoAck:deserialize(br)
    self.welfare = br:ReadInt32()
    self.expire_time = br:ReadUInt32()
    self.is_fetched = br:ReadInt8()
    self.is_broken = br:ReadInt8()
end
function _G.CLPFWelfarePigQueryInfoAck:serialize(bw)
    bw:WriteInt32(self.welfare)
    bw:WriteUInt32(self.expire_time)
    bw:WriteInt8(self.is_fetched)
    bw:WriteInt8(self.is_broken)
end

--福利猪领取每日锤子碎片请求
_G.CLPFWelfarePigFetchMaterialReq = _G.class(ProtocolBase)
function _G.CLPFWelfarePigFetchMaterialReq:__init(t)
    _G.CLPFWelfarePigFetchMaterialReq.super:__init(t)
    self.mid = 2
    self.pid = 120
end

--福利猪领取每日锤子碎片回应
--errcode number 0成功 1当日已领取
--item ItemInfo 领取到的物品信息
_G.CLPFWelfarePigFetchMaterialAck = _G.class(ProtocolBase)
function _G.CLPFWelfarePigFetchMaterialAck:__init(t)
    _G.CLPFWelfarePigFetchMaterialAck.super:__init(t)
    self.mid = 2
    self.pid = 121
end
function _G.CLPFWelfarePigFetchMaterialAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.item = CLPFItemInfo_deserialize(br)
end
function _G.CLPFWelfarePigFetchMaterialAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    CLPFItemInfo_serialize(self.item, bw)
end

--福利猪砸罐子请求
_G.CLPFWelfarePigBrokenReq = _G.class(ProtocolBase)
function _G.CLPFWelfarePigBrokenReq:__init(t)
    _G.CLPFWelfarePigBrokenReq.super:__init(t)
    self.mid = 2
    self.pid = 122
end

--福利猪砸罐子回应
--errcode number 0成功 1当日已砸过 2罐子中金币数量太少 3锤子碎片不足
--currency_delta number 获得的金币数量，仅用于显示
_G.CLPFWelfarePigBrokenAck = _G.class(ProtocolBase)
function _G.CLPFWelfarePigBrokenAck:__init(t)
    _G.CLPFWelfarePigBrokenAck.super:__init(t)
    self.mid = 2
    self.pid = 123
end
function _G.CLPFWelfarePigBrokenAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.currency_delta = br:ReadInt64()
end
function _G.CLPFWelfarePigBrokenAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt64(self.currency_delta)
end

--福利猪搜一搜请求
_G.CLPFWelfarePigSearchReq = _G.class(ProtocolBase)
function _G.CLPFWelfarePigSearchReq:__init(t)
    _G.CLPFWelfarePigSearchReq.super:__init(t)
    self.mid = 2
    self.pid = 124
end

--福利猪搜一搜回应
--errcode number 0搜索成功 1阶段不对 2很遗憾啥也没搜到
--currency_delta number 搜索到的罐子里砸开后获得的金币数量，仅用于显示
_G.CLPFWelfarePigSearchAck = _G.class(ProtocolBase)
function _G.CLPFWelfarePigSearchAck:__init(t)
    _G.CLPFWelfarePigSearchAck.super:__init(t)
    self.mid = 2
    self.pid = 125
end
function _G.CLPFWelfarePigSearchAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.currency_delta = br:ReadInt64()
end
function _G.CLPFWelfarePigSearchAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt64(self.currency_delta)
end

--查询投资炮倍信息请求
_G.CLPFInvestGunQueryInfoReq = _G.class(ProtocolBase)
function _G.CLPFInvestGunQueryInfoReq:__init(t)
    _G.CLPFInvestGunQueryInfoReq.super:__init(t)
    self.mid = 2
    self.pid = 126
end

--查询投资炮倍信息回应
--max_recharge_id number 已完成的最大投资充值Id，关联InvestGunRecharge.xlsx表主键
--max_gun_value number 已解锁的最大炮值
--finished_len number 已领取的解锁炮倍数组长度
--finished_array number[100] 已领取的解锁炮倍数组，对应InvestGunReward.xlsx表主键
_G.CLPFInvestGunQueryInfoAck = _G.class(ProtocolBase)
function _G.CLPFInvestGunQueryInfoAck:__init(t)
    _G.CLPFInvestGunQueryInfoAck.super:__init(t)
    self.mid = 2
    self.pid = 127
end
function _G.CLPFInvestGunQueryInfoAck:deserialize(br)
    self.max_recharge_id = br:ReadInt32()
    self.max_gun_value = br:ReadInt32()
    self.finished_len = br:ReadInt32()
    self.finished_array = {}
    for i=1,self.finished_len do
        self.finished_array[i] = br:ReadInt32()
    end
end
function _G.CLPFInvestGunQueryInfoAck:serialize(bw)
    bw:WriteInt32(self.max_recharge_id)
    bw:WriteInt32(self.max_gun_value)
    assert(not self.finished_array or #self.finished_array <= 100, "CLPFInvestGunQueryInfoAck.finished_array数组长度超过规定限制! 期望:100 实际:" .. #self.finished_array)
    bw:WriteInt32(#(self.finished_array or {}))
    for i=1,#(self.finished_array or {}) do
        bw:WriteInt32(self.finished_array[i])
    end
end

--领取投资炮倍奖励请求
--gun_value number 领取奖励的解锁炮倍
_G.CLPFInvestGunFetchRewardReq = _G.class(ProtocolBase)
function _G.CLPFInvestGunFetchRewardReq:__init(t)
    _G.CLPFInvestGunFetchRewardReq.super:__init(t)
    self.mid = 2
    self.pid = 128
end
function _G.CLPFInvestGunFetchRewardReq:deserialize(br)
    self.gun_value = br:ReadInt32()
end
function _G.CLPFInvestGunFetchRewardReq:serialize(bw)
    bw:WriteInt32(self.gun_value)
end

--领取投资炮倍奖励回应
--errcode number 0成功 1无此项 2炮值解锁条件不足 3充值条件不足 4奖励已领取
--item_len number 数组长度
--items ItemInfo[100] 物品数组，仅用于显示
_G.CLPFInvestGunFetchRewardAck = _G.class(ProtocolBase)
function _G.CLPFInvestGunFetchRewardAck:__init(t)
    _G.CLPFInvestGunFetchRewardAck.super:__init(t)
    self.mid = 2
    self.pid = 129
end
function _G.CLPFInvestGunFetchRewardAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.item_len = br:ReadInt32()
    self.items = {}
    for i=1,self.item_len do
        self.items[i] = CLPFItemInfo_deserialize(br)
    end
end
function _G.CLPFInvestGunFetchRewardAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.items or #self.items <= 100, "CLPFInvestGunFetchRewardAck.items数组长度超过规定限制! 期望:100 实际:" .. #self.items)
    bw:WriteInt32(#(self.items or {}))
    for i=1,#(self.items or {}) do
        CLPFItemInfo_serialize(self.items[i], bw)
    end
end

--查询出海保险信息请求
_G.CLPFInvestCostQueryInfoReq = _G.class(ProtocolBase)
function _G.CLPFInvestCostQueryInfoReq:__init(t)
    _G.CLPFInvestCostQueryInfoReq.super:__init(t)
    self.mid = 2
    self.pid = 130
end

--查询出海保险信息回应
--is_recharged number 是否已完成充值 1是0否
--total_currency_cost number 累计金币总消耗
--finished_len number 已领取的奖励Id数组长度
--finished_array number[100] 已领取的奖励Id数组，对应InvestCostReward.xlsx表主键
_G.CLPFInvestCostQueryInfoAck = _G.class(ProtocolBase)
function _G.CLPFInvestCostQueryInfoAck:__init(t)
    _G.CLPFInvestCostQueryInfoAck.super:__init(t)
    self.mid = 2
    self.pid = 131
end
function _G.CLPFInvestCostQueryInfoAck:deserialize(br)
    self.is_recharged = br:ReadInt8()
    self.total_currency_cost = br:ReadInt64()
    self.finished_len = br:ReadInt32()
    self.finished_array = {}
    for i=1,self.finished_len do
        self.finished_array[i] = br:ReadInt32()
    end
end
function _G.CLPFInvestCostQueryInfoAck:serialize(bw)
    bw:WriteInt8(self.is_recharged)
    bw:WriteInt64(self.total_currency_cost)
    assert(not self.finished_array or #self.finished_array <= 100, "CLPFInvestCostQueryInfoAck.finished_array数组长度超过规定限制! 期望:100 实际:" .. #self.finished_array)
    bw:WriteInt32(#(self.finished_array or {}))
    for i=1,#(self.finished_array or {}) do
        bw:WriteInt32(self.finished_array[i])
    end
end

--领取出海保险奖励请求
--reward_id number 奖励Id
_G.CLPFInvestCostFetchRewardReq = _G.class(ProtocolBase)
function _G.CLPFInvestCostFetchRewardReq:__init(t)
    _G.CLPFInvestCostFetchRewardReq.super:__init(t)
    self.mid = 2
    self.pid = 132
end
function _G.CLPFInvestCostFetchRewardReq:deserialize(br)
    self.reward_id = br:ReadInt32()
end
function _G.CLPFInvestCostFetchRewardReq:serialize(bw)
    bw:WriteInt32(self.reward_id)
end

--领取出海保险奖励回应
--errcode number 0成功 1无此项 2累计金币消耗不足 3尚未充值 4奖励已领取
--item_len number 数组长度
--items ItemInfo[100] 物品数组，仅用于显示
_G.CLPFInvestCostFetchRewardAck = _G.class(ProtocolBase)
function _G.CLPFInvestCostFetchRewardAck:__init(t)
    _G.CLPFInvestCostFetchRewardAck.super:__init(t)
    self.mid = 2
    self.pid = 133
end
function _G.CLPFInvestCostFetchRewardAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.item_len = br:ReadInt32()
    self.items = {}
    for i=1,self.item_len do
        self.items[i] = CLPFItemInfo_deserialize(br)
    end
end
function _G.CLPFInvestCostFetchRewardAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.items or #self.items <= 100, "CLPFInvestCostFetchRewardAck.items数组长度超过规定限制! 期望:100 实际:" .. #self.items)
    bw:WriteInt32(#(self.items or {}))
    for i=1,#(self.items or {}) do
        CLPFItemInfo_serialize(self.items[i], bw)
    end
end

--领取新手初始礼包请求
_G.CLPFFirstPackageFetchReq = _G.class(ProtocolBase)
function _G.CLPFFirstPackageFetchReq:__init(t)
    _G.CLPFFirstPackageFetchReq.super:__init(t)
    self.mid = 2
    self.pid = 134
end

--领取新手初始礼包回应
--errcode number 0成功 1已领取
--item_len number 数组长度
--items ItemInfo[100] 物品数组，仅用于显示
_G.CLPFFirstPackageFetchAck = _G.class(ProtocolBase)
function _G.CLPFFirstPackageFetchAck:__init(t)
    _G.CLPFFirstPackageFetchAck.super:__init(t)
    self.mid = 2
    self.pid = 135
end
function _G.CLPFFirstPackageFetchAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.item_len = br:ReadInt32()
    self.items = {}
    for i=1,self.item_len do
        self.items[i] = CLPFItemInfo_deserialize(br)
    end
end
function _G.CLPFFirstPackageFetchAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.items or #self.items <= 100, "CLPFFirstPackageFetchAck.items数组长度超过规定限制! 期望:100 实际:" .. #self.items)
    bw:WriteInt32(#(self.items or {}))
    for i=1,#(self.items or {}) do
        CLPFItemInfo_serialize(self.items[i], bw)
    end
end

--公告变动通知
_G.CLPFAnnouncementChangedNtf = _G.class(ProtocolBase)
function _G.CLPFAnnouncementChangedNtf:__init(t)
    _G.CLPFAnnouncementChangedNtf.super:__init(t)
    self.mid = 2
    self.pid = 136
end

--vip金币补足通知
--currency_delta number 补足的金币数量，仅用于显示
_G.CLPFVipFillUpCurrencyNtf = _G.class(ProtocolBase)
function _G.CLPFVipFillUpCurrencyNtf:__init(t)
    _G.CLPFVipFillUpCurrencyNtf.super:__init(t)
    self.mid = 2
    self.pid = 137
end
function _G.CLPFVipFillUpCurrencyNtf:deserialize(br)
    self.currency_delta = br:ReadInt64()
end
function _G.CLPFVipFillUpCurrencyNtf:serialize(bw)
    bw:WriteInt64(self.currency_delta)
end

--实物奖励兑换日志
--goods_id number 商品Id
--goods_name string[128] 商品名称
--real_name string[64] 真实姓名
--phone string[32] 联系电话
--address string[256] 联系地址
--state number 订单状态 0未发货 1已发货 2拒绝发货
--create_time number 创建订单时间戳
--process_time number 处理订单时间戳
local function CLPFRealGoodsExchangeLog_deserialize(br)
    local r = {}
    r.goods_id = br:ReadInt32()
    r.goods_name = br:ReadString()
    r.real_name = br:ReadString()
    r.phone = br:ReadString()
    r.address = br:ReadString()
    r.state = br:ReadInt32()
    r.create_time = br:ReadUInt32()
    r.process_time = br:ReadUInt32()
    return r
end
local function CLPFRealGoodsExchangeLog_serialize(t, bw)
    bw:WriteInt32(t.goods_id)
    bw:WriteString(t.goods_name, 128)
    bw:WriteString(t.real_name, 64)
    bw:WriteString(t.phone, 32)
    bw:WriteString(t.address, 256)
    bw:WriteInt32(t.state)
    bw:WriteUInt32(t.create_time)
    bw:WriteUInt32(t.process_time)
end

--查询常用的真实地址请求
_G.CLPFRealGoodsQueryAddressReq = _G.class(ProtocolBase)
function _G.CLPFRealGoodsQueryAddressReq:__init(t)
    _G.CLPFRealGoodsQueryAddressReq.super:__init(t)
    self.mid = 2
    self.pid = 138
end

--查询常用的真实地址回应
--errcode number 0成功 1不存在
--real_name string[64] 真实姓名
--phone string[32] 联系电话
--address string[256] 联系地址
_G.CLPFRealGoodsQueryAddressAck = _G.class(ProtocolBase)
function _G.CLPFRealGoodsQueryAddressAck:__init(t)
    _G.CLPFRealGoodsQueryAddressAck.super:__init(t)
    self.mid = 2
    self.pid = 139
end
function _G.CLPFRealGoodsQueryAddressAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.real_name = br:ReadString()
    self.phone = br:ReadString()
    self.address = br:ReadString()
end
function _G.CLPFRealGoodsQueryAddressAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteString(self.real_name, 64)
    bw:WriteString(self.phone, 32)
    bw:WriteString(self.address, 256)
end

--实物奖励下单请求
--goods_id number 实物商品Id
--real_name string[64] 真实姓名
--phone string[32] 联系电话
--address string[256] 联系地址
_G.CLPFRealGoodsCreateOrderReq = _G.class(ProtocolBase)
function _G.CLPFRealGoodsCreateOrderReq:__init(t)
    _G.CLPFRealGoodsCreateOrderReq.super:__init(t)
    self.mid = 2
    self.pid = 140
end
function _G.CLPFRealGoodsCreateOrderReq:deserialize(br)
    self.goods_id = br:ReadInt32()
    self.real_name = br:ReadString()
    self.phone = br:ReadString()
    self.address = br:ReadString()
end
function _G.CLPFRealGoodsCreateOrderReq:serialize(bw)
    bw:WriteInt32(self.goods_id)
    bw:WriteString(self.real_name, 64)
    bw:WriteString(self.phone, 32)
    bw:WriteString(self.address, 256)
end

--实物奖励下单回应
--errcode number 0成功 1信息填写不完整 2商品不存在 3vip等级不足 4购买次数已达上限 5资源不足
_G.CLPFRealGoodsCreateOrderAck = _G.class(ProtocolBase)
function _G.CLPFRealGoodsCreateOrderAck:__init(t)
    _G.CLPFRealGoodsCreateOrderAck.super:__init(t)
    self.mid = 2
    self.pid = 141
end
function _G.CLPFRealGoodsCreateOrderAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLPFRealGoodsCreateOrderAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--查询实物奖励兑换纪录请求
_G.CLPFRealGoodsQueryExchangeLogReq = _G.class(ProtocolBase)
function _G.CLPFRealGoodsQueryExchangeLogReq:__init(t)
    _G.CLPFRealGoodsQueryExchangeLogReq.super:__init(t)
    self.mid = 2
    self.pid = 142
end

--查询实物奖励兑换纪录回应
--log_len number 日志数组长度
--log_array RealGoodsExchangeLog[100] 日志数组
_G.CLPFRealGoodsQueryExchangeLogAck = _G.class(ProtocolBase)
function _G.CLPFRealGoodsQueryExchangeLogAck:__init(t)
    _G.CLPFRealGoodsQueryExchangeLogAck.super:__init(t)
    self.mid = 2
    self.pid = 143
end
function _G.CLPFRealGoodsQueryExchangeLogAck:deserialize(br)
    self.log_len = br:ReadInt32()
    self.log_array = {}
    for i=1,self.log_len do
        self.log_array[i] = CLPFRealGoodsExchangeLog_deserialize(br)
    end
end
function _G.CLPFRealGoodsQueryExchangeLogAck:serialize(bw)
    assert(not self.log_array or #self.log_array <= 100, "CLPFRealGoodsQueryExchangeLogAck.log_array数组长度超过规定限制! 期望:100 实际:" .. #self.log_array)
    bw:WriteInt32(#(self.log_array or {}))
    for i=1,#(self.log_array or {}) do
        CLPFRealGoodsExchangeLog_serialize(self.log_array[i], bw)
    end
end

--查询已完成的新手引导标记数组请求
_G.CLPFGuideDataQueryReq = _G.class(ProtocolBase)
function _G.CLPFGuideDataQueryReq:__init(t)
    _G.CLPFGuideDataQueryReq.super:__init(t)
    self.mid = 2
    self.pid = 144
end

--查询已完成的新手引导标记数组回应
--flag_len number 已完成的新手引导标记数组长度
--flag_array number[100] 已完成的新手引导标记数组
_G.CLPFGuideDataQueryAck = _G.class(ProtocolBase)
function _G.CLPFGuideDataQueryAck:__init(t)
    _G.CLPFGuideDataQueryAck.super:__init(t)
    self.mid = 2
    self.pid = 145
end
function _G.CLPFGuideDataQueryAck:deserialize(br)
    self.flag_len = br:ReadInt32()
    self.flag_array = {}
    for i=1,self.flag_len do
        self.flag_array[i] = br:ReadInt32()
    end
end
function _G.CLPFGuideDataQueryAck:serialize(bw)
    assert(not self.flag_array or #self.flag_array <= 100, "CLPFGuideDataQueryAck.flag_array数组长度超过规定限制! 期望:100 实际:" .. #self.flag_array)
    bw:WriteInt32(#(self.flag_array or {}))
    for i=1,#(self.flag_array or {}) do
        bw:WriteInt32(self.flag_array[i])
    end
end

--上报完成了某个新手引导标记
--flag number 新手引导完成标记
_G.CLPFGuideDataActRpt = _G.class(ProtocolBase)
function _G.CLPFGuideDataActRpt:__init(t)
    _G.CLPFGuideDataActRpt.super:__init(t)
    self.mid = 2
    self.pid = 146
end
function _G.CLPFGuideDataActRpt:deserialize(br)
    self.flag = br:ReadInt32()
end
function _G.CLPFGuideDataActRpt:serialize(bw)
    bw:WriteInt32(self.flag)
end

--客户端配置表发布通知
--md5 string[40] 最新配置表的md5，里面出现的字母大写
_G.CLPFClientConfigPublishNtf = _G.class(ProtocolBase)
function _G.CLPFClientConfigPublishNtf:__init(t)
    _G.CLPFClientConfigPublishNtf.super:__init(t)
    self.mid = 2
    self.pid = 147
end
function _G.CLPFClientConfigPublishNtf:deserialize(br)
    self.md5 = br:ReadString()
end
function _G.CLPFClientConfigPublishNtf:serialize(bw)
    bw:WriteString(self.md5, 40)
end

--子游戏在线人数信息
--group_id number 游戏Id
--service_id number 玩法Id
--online_count number 在线人数
local function CLPFSubGamesOnlineCountInfo_deserialize(br)
    local r = {}
    r.group_id = br:ReadInt32()
    r.service_id = br:ReadInt32()
    r.online_count = br:ReadInt32()
    return r
end
local function CLPFSubGamesOnlineCountInfo_serialize(t, bw)
    bw:WriteInt32(t.group_id)
    bw:WriteInt32(t.service_id)
    bw:WriteInt32(t.online_count)
end

--子游戏在线人数请求
_G.CLPFSubGamesOnlineCountReq = _G.class(ProtocolBase)
function _G.CLPFSubGamesOnlineCountReq:__init(t)
    _G.CLPFSubGamesOnlineCountReq.super:__init(t)
    self.mid = 2
    self.pid = 148
end

--子游戏在线人数回应
--array_len number 在线人数数组长度
--array SubGamesOnlineCountInfo[100] 在线人数数组
_G.CLPFSubGamesOnlineCountAck = _G.class(ProtocolBase)
function _G.CLPFSubGamesOnlineCountAck:__init(t)
    _G.CLPFSubGamesOnlineCountAck.super:__init(t)
    self.mid = 2
    self.pid = 149
end
function _G.CLPFSubGamesOnlineCountAck:deserialize(br)
    self.array_len = br:ReadUInt8()
    self.array = {}
    for i=1,self.array_len do
        self.array[i] = CLPFSubGamesOnlineCountInfo_deserialize(br)
    end
end
function _G.CLPFSubGamesOnlineCountAck:serialize(bw)
    assert(not self.array or #self.array <= 100, "CLPFSubGamesOnlineCountAck.array数组长度超过规定限制! 期望:100 实际:" .. #self.array)
    bw:WriteUInt8(#(self.array or {}))
    for i=1,#(self.array or {}) do
        CLPFSubGamesOnlineCountInfo_serialize(self.array[i], bw)
    end
end

--激活码领取奖励请求
--code string[20] 激活码
_G.CLPFCdkeyFetchRewardReq = _G.class(ProtocolBase)
function _G.CLPFCdkeyFetchRewardReq:__init(t)
    _G.CLPFCdkeyFetchRewardReq.super:__init(t)
    self.mid = 2
    self.pid = 150
end
function _G.CLPFCdkeyFetchRewardReq:deserialize(br)
    self.code = br:ReadString()
end
function _G.CLPFCdkeyFetchRewardReq:serialize(bw)
    bw:WriteString(self.code, 20)
end

--激活码领取奖励回应
--errcode number 0成功 1兑换码不存在 2兑换码已被领取 3同一类型的兑换码礼包每个玩家只能领取一次
--item_len number 数组长度
--items ItemInfo[100] 物品数组，仅用于显示
_G.CLPFCdkeyFetchRewardAck = _G.class(ProtocolBase)
function _G.CLPFCdkeyFetchRewardAck:__init(t)
    _G.CLPFCdkeyFetchRewardAck.super:__init(t)
    self.mid = 2
    self.pid = 151
end
function _G.CLPFCdkeyFetchRewardAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.item_len = br:ReadInt32()
    self.items = {}
    for i=1,self.item_len do
        self.items[i] = CLPFItemInfo_deserialize(br)
    end
end
function _G.CLPFCdkeyFetchRewardAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.items or #self.items <= 100, "CLPFCdkeyFetchRewardAck.items数组长度超过规定限制! 期望:100 实际:" .. #self.items)
    bw:WriteInt32(#(self.items or {}))
    for i=1,#(self.items or {}) do
        CLPFItemInfo_serialize(self.items[i], bw)
    end
end

--账号绑定状态请求
_G.CLPFAccountBindStateReq = _G.class(ProtocolBase)
function _G.CLPFAccountBindStateReq:__init(t)
    _G.CLPFAccountBindStateReq.super:__init(t)
    self.mid = 2
    self.pid = 152
end

--账号绑定状态回应
--errcode number 0成功
--bind_type_length number 绑定数组长度
--bind_type_array number[20] 绑定数组 1游客2手机号3QQ4微信5Facebook6GooglePlay7GameCenter
_G.CLPFAccountBindStateAck = _G.class(ProtocolBase)
function _G.CLPFAccountBindStateAck:__init(t)
    _G.CLPFAccountBindStateAck.super:__init(t)
    self.mid = 2
    self.pid = 153
end
function _G.CLPFAccountBindStateAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.bind_type_length = br:ReadInt8()
    self.bind_type_array = {}
    for i=1,self.bind_type_length do
        self.bind_type_array[i] = br:ReadInt8()
    end
end
function _G.CLPFAccountBindStateAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.bind_type_array or #self.bind_type_array <= 20, "CLPFAccountBindStateAck.bind_type_array数组长度超过规定限制! 期望:20 实际:" .. #self.bind_type_array)
    bw:WriteInt8(#(self.bind_type_array or {}))
    for i=1,#(self.bind_type_array or {}) do
        bw:WriteInt8(self.bind_type_array[i])
    end
end

--账号手机绑定请求
--phone string[16] 手机号码
--sms_app_key string[64] 短信验证的AppKey
--sms_zone string[10] 短信验证的区号
--sms_code string[10] 短信验证码
--password string[64] 登录密码，使用CA3加密，固定秘钥19357
--sms_channel number 0Mob渠道 1其他渠道
_G.CLPFAccountPhoneBindReq = _G.class(ProtocolBase)
function _G.CLPFAccountPhoneBindReq:__init(t)
    _G.CLPFAccountPhoneBindReq.super:__init(t)
    self.mid = 2
    self.pid = 154
end
function _G.CLPFAccountPhoneBindReq:deserialize(br)
    self.phone = br:ReadString()
    self.sms_app_key = br:ReadString()
    self.sms_zone = br:ReadString()
    self.sms_code = br:ReadString()
    self.password = br:ReadString()
    self.sms_channel = br:ReadInt32()
end
function _G.CLPFAccountPhoneBindReq:serialize(bw)
    bw:WriteString(self.phone, 16)
    bw:WriteString(self.sms_app_key, 64)
    bw:WriteString(self.sms_zone, 10)
    bw:WriteString(self.sms_code, 10)
    bw:WriteString(self.password, 64)
    bw:WriteInt32(self.sms_channel)
end

--账号手机绑定回应
--errcode number 0成功 1该账号已经绑定过手机号 2密码不合法 3输入的手机号码已绑定其他账号 4短信验证失败
_G.CLPFAccountPhoneBindAck = _G.class(ProtocolBase)
function _G.CLPFAccountPhoneBindAck:__init(t)
    _G.CLPFAccountPhoneBindAck.super:__init(t)
    self.mid = 2
    self.pid = 155
end
function _G.CLPFAccountPhoneBindAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLPFAccountPhoneBindAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--账号手机更换请求1
--phone string[16] 手机号码
--sms_app_key string[64] 短信验证的AppKey
--sms_zone string[10] 短信验证的区号
--sms_code string[10] 短信验证码
--sms_channel number 0Mob渠道 1其他渠道
_G.CLPFAccountPhoneChange1Req = _G.class(ProtocolBase)
function _G.CLPFAccountPhoneChange1Req:__init(t)
    _G.CLPFAccountPhoneChange1Req.super:__init(t)
    self.mid = 2
    self.pid = 156
end
function _G.CLPFAccountPhoneChange1Req:deserialize(br)
    self.phone = br:ReadString()
    self.sms_app_key = br:ReadString()
    self.sms_zone = br:ReadString()
    self.sms_code = br:ReadString()
    self.sms_channel = br:ReadInt32()
end
function _G.CLPFAccountPhoneChange1Req:serialize(bw)
    bw:WriteString(self.phone, 16)
    bw:WriteString(self.sms_app_key, 64)
    bw:WriteString(self.sms_zone, 10)
    bw:WriteString(self.sms_code, 10)
    bw:WriteInt32(self.sms_channel)
end

--账号手机更换回应1
--errcode number 0成功 1请先绑定手机号 2手机号与预留的不一致 3短信验证失败
_G.CLPFAccountPhoneChange1Ack = _G.class(ProtocolBase)
function _G.CLPFAccountPhoneChange1Ack:__init(t)
    _G.CLPFAccountPhoneChange1Ack.super:__init(t)
    self.mid = 2
    self.pid = 157
end
function _G.CLPFAccountPhoneChange1Ack:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLPFAccountPhoneChange1Ack:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--账号手机更换请求2
--new_phone string[16] 新手机号码
--sms_app_key string[64] 短信验证的AppKey
--sms_zone string[10] 短信验证的区号
--sms_code string[10] 短信验证码
--sms_channel number 0Mob渠道 1其他渠道
_G.CLPFAccountPhoneChange2Req = _G.class(ProtocolBase)
function _G.CLPFAccountPhoneChange2Req:__init(t)
    _G.CLPFAccountPhoneChange2Req.super:__init(t)
    self.mid = 2
    self.pid = 158
end
function _G.CLPFAccountPhoneChange2Req:deserialize(br)
    self.new_phone = br:ReadString()
    self.sms_app_key = br:ReadString()
    self.sms_zone = br:ReadString()
    self.sms_code = br:ReadString()
    self.sms_channel = br:ReadInt32()
end
function _G.CLPFAccountPhoneChange2Req:serialize(bw)
    bw:WriteString(self.new_phone, 16)
    bw:WriteString(self.sms_app_key, 64)
    bw:WriteString(self.sms_zone, 10)
    bw:WriteString(self.sms_code, 10)
    bw:WriteInt32(self.sms_channel)
end

--账号手机更换回应2
--errcode number 0成功 1状态不对 2原手机号和新手机号不能一致 3新手机号码已绑定其他账号 4短信验证失败
_G.CLPFAccountPhoneChange2Ack = _G.class(ProtocolBase)
function _G.CLPFAccountPhoneChange2Ack:__init(t)
    _G.CLPFAccountPhoneChange2Ack.super:__init(t)
    self.mid = 2
    self.pid = 159
end
function _G.CLPFAccountPhoneChange2Ack:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLPFAccountPhoneChange2Ack:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--账号统一绑定请求
--phone string[16] 手机号码
--sms_app_key string[64] 短信验证的AppKey
--sms_zone string[10] 短信验证的区号
--sms_code string[10] 短信验证码
--type number 绑定类型 1设备 3QQ 4微信 5Facebook 6GooglePlay 7GameCenter
--token string[256] 唯一标识串，使用CA3加密，固定秘钥：19357
--sms_channel number 0Mob渠道 1其他渠道
_G.CLPFAccountUniformBindReq = _G.class(ProtocolBase)
function _G.CLPFAccountUniformBindReq:__init(t)
    _G.CLPFAccountUniformBindReq.super:__init(t)
    self.mid = 2
    self.pid = 160
end
function _G.CLPFAccountUniformBindReq:deserialize(br)
    self.phone = br:ReadString()
    self.sms_app_key = br:ReadString()
    self.sms_zone = br:ReadString()
    self.sms_code = br:ReadString()
    self.type = br:ReadUInt8()
    self.token = br:ReadString()
    self.sms_channel = br:ReadInt32()
end
function _G.CLPFAccountUniformBindReq:serialize(bw)
    bw:WriteString(self.phone, 16)
    bw:WriteString(self.sms_app_key, 64)
    bw:WriteString(self.sms_zone, 10)
    bw:WriteString(self.sms_code, 10)
    bw:WriteUInt8(self.type)
    bw:WriteString(self.token, 256)
    bw:WriteInt32(self.sms_channel)
end

--账号统一绑定回应
--errcode number 0成功 1请先绑定手机 2手机号与预留的不一致 3绑定类型非法 4token非法 5已绑定该类型，请先解绑 6已关联其他账号 7短信验证失败
_G.CLPFAccountUniformBindAck = _G.class(ProtocolBase)
function _G.CLPFAccountUniformBindAck:__init(t)
    _G.CLPFAccountUniformBindAck.super:__init(t)
    self.mid = 2
    self.pid = 161
end
function _G.CLPFAccountUniformBindAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLPFAccountUniformBindAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--账号统一解绑请求
--phone string[16] 手机号码
--sms_app_key string[64] 短信验证的AppKey
--sms_zone string[10] 短信验证的区号
--sms_code string[10] 短信验证码
--type number 绑定类型 1设备 3QQ 4微信 5Facebook 6GooglePlay 7GameCenter
--sms_channel number 0Mob渠道 1其他渠道
_G.CLPFAccountUniformUnbindReq = _G.class(ProtocolBase)
function _G.CLPFAccountUniformUnbindReq:__init(t)
    _G.CLPFAccountUniformUnbindReq.super:__init(t)
    self.mid = 2
    self.pid = 162
end
function _G.CLPFAccountUniformUnbindReq:deserialize(br)
    self.phone = br:ReadString()
    self.sms_app_key = br:ReadString()
    self.sms_zone = br:ReadString()
    self.sms_code = br:ReadString()
    self.type = br:ReadUInt8()
    self.sms_channel = br:ReadInt32()
end
function _G.CLPFAccountUniformUnbindReq:serialize(bw)
    bw:WriteString(self.phone, 16)
    bw:WriteString(self.sms_app_key, 64)
    bw:WriteString(self.sms_zone, 10)
    bw:WriteString(self.sms_code, 10)
    bw:WriteUInt8(self.type)
    bw:WriteInt32(self.sms_channel)
end

--账号统一解绑回应
--errcode number 0成功 1请先绑定手机 2手机号与预留的不一致 3解绑类型非法 4还未绑定该类型 5短信验证失败
_G.CLPFAccountUniformUnbindAck = _G.class(ProtocolBase)
function _G.CLPFAccountUniformUnbindAck:__init(t)
    _G.CLPFAccountUniformUnbindAck.super:__init(t)
    self.mid = 2
    self.pid = 163
end
function _G.CLPFAccountUniformUnbindAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLPFAccountUniformUnbindAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--查询玩家昵称请求
--user_id number 玩家Id
_G.CLPFPlayerNicknameQueryReq = _G.class(ProtocolBase)
function _G.CLPFPlayerNicknameQueryReq:__init(t)
    _G.CLPFPlayerNicknameQueryReq.super:__init(t)
    self.mid = 2
    self.pid = 164
end
function _G.CLPFPlayerNicknameQueryReq:deserialize(br)
    self.user_id = br:ReadInt32()
end
function _G.CLPFPlayerNicknameQueryReq:serialize(bw)
    bw:WriteInt32(self.user_id)
end

--查询玩家昵称回应
--errcode number 0成功 1玩家不存在
--nickname string[32] 昵称
_G.CLPFPlayerNicknameQueryAck = _G.class(ProtocolBase)
function _G.CLPFPlayerNicknameQueryAck:__init(t)
    _G.CLPFPlayerNicknameQueryAck.super:__init(t)
    self.mid = 2
    self.pid = 165
end
function _G.CLPFPlayerNicknameQueryAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.nickname = br:ReadString()
end
function _G.CLPFPlayerNicknameQueryAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteString(self.nickname, 32)
end

--金库密码初始化请求 注意：该请求成功后，不用再进行密码验证功能了，可以直接进入金库界面
--password string[64] 金库初始密码，使用CA3加密，固定秘钥：19357
_G.CLPFBankPasswordInitReq = _G.class(ProtocolBase)
function _G.CLPFBankPasswordInitReq:__init(t)
    _G.CLPFBankPasswordInitReq.super:__init(t)
    self.mid = 2
    self.pid = 166
end
function _G.CLPFBankPasswordInitReq:deserialize(br)
    self.password = br:ReadString()
end
function _G.CLPFBankPasswordInitReq:serialize(bw)
    bw:WriteString(self.password, 64)
end

--金库密码初始化回应
--errcode number 0成功 1请先绑定手机 2已设置过初始密码 3密码不合法
_G.CLPFBankPasswordInitAck = _G.class(ProtocolBase)
function _G.CLPFBankPasswordInitAck:__init(t)
    _G.CLPFBankPasswordInitAck.super:__init(t)
    self.mid = 2
    self.pid = 167
end
function _G.CLPFBankPasswordInitAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLPFBankPasswordInitAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--金库密码验证请求
--password string[64] 金库密码，使用CA3加密，固定秘钥：19357
_G.CLPFBankPasswordVerifyReq = _G.class(ProtocolBase)
function _G.CLPFBankPasswordVerifyReq:__init(t)
    _G.CLPFBankPasswordVerifyReq.super:__init(t)
    self.mid = 2
    self.pid = 168
end
function _G.CLPFBankPasswordVerifyReq:deserialize(br)
    self.password = br:ReadString()
end
function _G.CLPFBankPasswordVerifyReq:serialize(bw)
    bw:WriteString(self.password, 64)
end

--金库密码验证回应
--errcode number 0成功 1请先绑定手机 2您还没设置过银行密码 3银行密码验证失败
_G.CLPFBankPasswordVerifyAck = _G.class(ProtocolBase)
function _G.CLPFBankPasswordVerifyAck:__init(t)
    _G.CLPFBankPasswordVerifyAck.super:__init(t)
    self.mid = 2
    self.pid = 169
end
function _G.CLPFBankPasswordVerifyAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLPFBankPasswordVerifyAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--金库密码修改请求
--origin_password string[64] 原密码，使用CA3加密，固定秘钥：19357
--new_password string[64] 新密码，使用CA3加密，固定秘钥：19357
_G.CLPFBankPasswordModifyReq = _G.class(ProtocolBase)
function _G.CLPFBankPasswordModifyReq:__init(t)
    _G.CLPFBankPasswordModifyReq.super:__init(t)
    self.mid = 2
    self.pid = 170
end
function _G.CLPFBankPasswordModifyReq:deserialize(br)
    self.origin_password = br:ReadString()
    self.new_password = br:ReadString()
end
function _G.CLPFBankPasswordModifyReq:serialize(bw)
    bw:WriteString(self.origin_password, 64)
    bw:WriteString(self.new_password, 64)
end

--金库密码修改回应
--errcode number 0成功 1请先绑定手机 2请先设置密码 3原密码不正确 4新密码不合法
_G.CLPFBankPasswordModifyAck = _G.class(ProtocolBase)
function _G.CLPFBankPasswordModifyAck:__init(t)
    _G.CLPFBankPasswordModifyAck.super:__init(t)
    self.mid = 2
    self.pid = 171
end
function _G.CLPFBankPasswordModifyAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLPFBankPasswordModifyAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--金库密码重置请求 注意：该请求成功后，不用再进行密码验证功能了，可以直接进入金库界面
--phone string[16] 手机号码
--sms_app_key string[64] 短信验证的AppKey
--sms_zone string[10] 短信验证的区号
--sms_code string[10] 短信验证码
--new_password string[64] 新密码，使用CA3加密，固定秘钥：19357
--sms_channel number 0Mob渠道 1其他渠道
_G.CLPFBankPasswordResetReq = _G.class(ProtocolBase)
function _G.CLPFBankPasswordResetReq:__init(t)
    _G.CLPFBankPasswordResetReq.super:__init(t)
    self.mid = 2
    self.pid = 172
end
function _G.CLPFBankPasswordResetReq:deserialize(br)
    self.phone = br:ReadString()
    self.sms_app_key = br:ReadString()
    self.sms_zone = br:ReadString()
    self.sms_code = br:ReadString()
    self.new_password = br:ReadString()
    self.sms_channel = br:ReadInt32()
end
function _G.CLPFBankPasswordResetReq:serialize(bw)
    bw:WriteString(self.phone, 16)
    bw:WriteString(self.sms_app_key, 64)
    bw:WriteString(self.sms_zone, 10)
    bw:WriteString(self.sms_code, 10)
    bw:WriteString(self.new_password, 64)
    bw:WriteInt32(self.sms_channel)
end

--金库密码重置回应
--errcode number 0成功 1请先绑定手机 2手机号与预留的不一致 3还未设置过密码 4新密码不合法 5短信验证失败
_G.CLPFBankPasswordResetAck = _G.class(ProtocolBase)
function _G.CLPFBankPasswordResetAck:__init(t)
    _G.CLPFBankPasswordResetAck.super:__init(t)
    self.mid = 2
    self.pid = 173
end
function _G.CLPFBankPasswordResetAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLPFBankPasswordResetAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--金库物品查询请求
_G.CLPFBankItemQueryReq = _G.class(ProtocolBase)
function _G.CLPFBankItemQueryReq:__init(t)
    _G.CLPFBankItemQueryReq.super:__init(t)
    self.mid = 2
    self.pid = 174
end

--金库物品查询回应
--errcode number 0成功 1权限不足
--item_len number 数组长度
--items ItemInfo[100] 物品数组
_G.CLPFBankItemQueryAck = _G.class(ProtocolBase)
function _G.CLPFBankItemQueryAck:__init(t)
    _G.CLPFBankItemQueryAck.super:__init(t)
    self.mid = 2
    self.pid = 175
end
function _G.CLPFBankItemQueryAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.item_len = br:ReadInt32()
    self.items = {}
    for i=1,self.item_len do
        self.items[i] = CLPFItemInfo_deserialize(br)
    end
end
function _G.CLPFBankItemQueryAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.items or #self.items <= 100, "CLPFBankItemQueryAck.items数组长度超过规定限制! 期望:100 实际:" .. #self.items)
    bw:WriteInt32(#(self.items or {}))
    for i=1,#(self.items or {}) do
        CLPFItemInfo_serialize(self.items[i], bw)
    end
end

--金库物品存入请求
--item ItemInfo 要存入的物品
_G.CLPFBankItemStoreReq = _G.class(ProtocolBase)
function _G.CLPFBankItemStoreReq:__init(t)
    _G.CLPFBankItemStoreReq.super:__init(t)
    self.mid = 2
    self.pid = 176
end
function _G.CLPFBankItemStoreReq:deserialize(br)
    self.item = CLPFItemInfo_deserialize(br)
end
function _G.CLPFBankItemStoreReq:serialize(bw)
    CLPFItemInfo_serialize(self.item, bw)
end

--金库物品存入回应
--errcode number 0成功 1权限不足 2参数无效 3资源数量不足
_G.CLPFBankItemStoreAck = _G.class(ProtocolBase)
function _G.CLPFBankItemStoreAck:__init(t)
    _G.CLPFBankItemStoreAck.super:__init(t)
    self.mid = 2
    self.pid = 177
end
function _G.CLPFBankItemStoreAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLPFBankItemStoreAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--金库物品取出请求
--item ItemInfo 要取出的物品
_G.CLPFBankItemFetchReq = _G.class(ProtocolBase)
function _G.CLPFBankItemFetchReq:__init(t)
    _G.CLPFBankItemFetchReq.super:__init(t)
    self.mid = 2
    self.pid = 178
end
function _G.CLPFBankItemFetchReq:deserialize(br)
    self.item = CLPFItemInfo_deserialize(br)
end
function _G.CLPFBankItemFetchReq:serialize(bw)
    CLPFItemInfo_serialize(self.item, bw)
end

--金库物品取出回应
--errcode number 0成功 1权限不足 2参数无效 3资源数量不足
_G.CLPFBankItemFetchAck = _G.class(ProtocolBase)
function _G.CLPFBankItemFetchAck:__init(t)
    _G.CLPFBankItemFetchAck.super:__init(t)
    self.mid = 2
    self.pid = 179
end
function _G.CLPFBankItemFetchAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLPFBankItemFetchAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--金库物品赠送请求
--user_id number 玩家Id
--item ItemInfo 要赠送的物品
_G.CLPFBankItemSendReq = _G.class(ProtocolBase)
function _G.CLPFBankItemSendReq:__init(t)
    _G.CLPFBankItemSendReq.super:__init(t)
    self.mid = 2
    self.pid = 180
end
function _G.CLPFBankItemSendReq:deserialize(br)
    self.user_id = br:ReadInt32()
    self.item = CLPFItemInfo_deserialize(br)
end
function _G.CLPFBankItemSendReq:serialize(bw)
    bw:WriteInt32(self.user_id)
    CLPFItemInfo_serialize(self.item, bw)
end

--金库物品赠送回应
--errcode number 0成功 1权限不足 2参数无效 3资源数量不足 4目标玩家不存在 5对方等级不足 6赠送的资源数量太少 7该物品不允许赠送 8您尚未达成赠送条件，请先充值 9目标玩家已达到接收最大上限
_G.CLPFBankItemSendAck = _G.class(ProtocolBase)
function _G.CLPFBankItemSendAck:__init(t)
    _G.CLPFBankItemSendAck.super:__init(t)
    self.mid = 2
    self.pid = 181
end
function _G.CLPFBankItemSendAck:deserialize(br)
    self.errcode = br:ReadInt8()
end
function _G.CLPFBankItemSendAck:serialize(bw)
    bw:WriteInt8(self.errcode)
end

--金库物品日志信息
--log_type number 1存入 2取出 3赠送 4接收
--item ItemInfo 物品信息
--refer_user_id number 关联的玩家Id
--refer_nickname string[32] 关联的玩家昵称
--timestamp number 时间戳
--unique_id number 唯一Id
local function CLPFBankItemLogInfo_deserialize(br)
    local r = {}
    r.log_type = br:ReadInt8()
    r.item = CLPFItemInfo_deserialize(br)
    r.refer_user_id = br:ReadInt32()
    r.refer_nickname = br:ReadString()
    r.timestamp = br:ReadUInt32()
    r.unique_id = br:ReadInt32()
    return r
end
local function CLPFBankItemLogInfo_serialize(t, bw)
    bw:WriteInt8(t.log_type)
    CLPFItemInfo_serialize(t.item, bw)
    bw:WriteInt32(t.refer_user_id)
    bw:WriteString(t.refer_nickname, 32)
    bw:WriteUInt32(t.timestamp)
    bw:WriteInt32(t.unique_id)
end

--金库物品日志查询请求
_G.CLPFBankItemLogQueryReq = _G.class(ProtocolBase)
function _G.CLPFBankItemLogQueryReq:__init(t)
    _G.CLPFBankItemLogQueryReq.super:__init(t)
    self.mid = 2
    self.pid = 182
end

--金库物品日志查询回应
--errcode number 0成功 1权限不足
--log_length number 日志数组长度
--log_array BankItemLogInfo[15] 日志数组
_G.CLPFBankItemLogQueryAck = _G.class(ProtocolBase)
function _G.CLPFBankItemLogQueryAck:__init(t)
    _G.CLPFBankItemLogQueryAck.super:__init(t)
    self.mid = 2
    self.pid = 183
end
function _G.CLPFBankItemLogQueryAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.log_length = br:ReadInt32()
    self.log_array = {}
    for i=1,self.log_length do
        self.log_array[i] = CLPFBankItemLogInfo_deserialize(br)
    end
end
function _G.CLPFBankItemLogQueryAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.log_array or #self.log_array <= 15, "CLPFBankItemLogQueryAck.log_array数组长度超过规定限制! 期望:15 实际:" .. #self.log_array)
    bw:WriteInt32(#(self.log_array or {}))
    for i=1,#(self.log_array or {}) do
        CLPFBankItemLogInfo_serialize(self.log_array[i], bw)
    end
end

--金库物品详细日志查询请求
--log_type number 日志类型 0全部 1存入 2取出 3赠送 4接收
--query_type number 查询类型 1查询大于指定Id的纪录 2查询小于指定Id的纪录
--refer_unique_id number 指定纪录Id
--order_type number 排序类型 1升序 2降序
--count number 请求数量，最大100条
_G.CLPFBankItemLogDetailQueryReq = _G.class(ProtocolBase)
function _G.CLPFBankItemLogDetailQueryReq:__init(t)
    _G.CLPFBankItemLogDetailQueryReq.super:__init(t)
    self.mid = 2
    self.pid = 184
end
function _G.CLPFBankItemLogDetailQueryReq:deserialize(br)
    self.log_type = br:ReadInt8()
    self.query_type = br:ReadInt8()
    self.refer_unique_id = br:ReadInt32()
    self.order_type = br:ReadInt8()
    self.count = br:ReadInt32()
end
function _G.CLPFBankItemLogDetailQueryReq:serialize(bw)
    bw:WriteInt8(self.log_type)
    bw:WriteInt8(self.query_type)
    bw:WriteInt32(self.refer_unique_id)
    bw:WriteInt8(self.order_type)
    bw:WriteInt32(self.count)
end

--金库物品详细日志查询回应
--errcode number 0成功 1权限不足 2日志类型错误 3查询类型错误 4排序类型参数错误 5查询数量参数错误
--log_length number 日志数组长度
--log_array BankItemLogInfo[100] 日志数组
_G.CLPFBankItemLogDetailQueryAck = _G.class(ProtocolBase)
function _G.CLPFBankItemLogDetailQueryAck:__init(t)
    _G.CLPFBankItemLogDetailQueryAck.super:__init(t)
    self.mid = 2
    self.pid = 185
end
function _G.CLPFBankItemLogDetailQueryAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.log_length = br:ReadInt32()
    self.log_array = {}
    for i=1,self.log_length do
        self.log_array[i] = CLPFBankItemLogInfo_deserialize(br)
    end
end
function _G.CLPFBankItemLogDetailQueryAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.log_array or #self.log_array <= 100, "CLPFBankItemLogDetailQueryAck.log_array数组长度超过规定限制! 期望:100 实际:" .. #self.log_array)
    bw:WriteInt32(#(self.log_array or {}))
    for i=1,#(self.log_array or {}) do
        CLPFBankItemLogInfo_serialize(self.log_array[i], bw)
    end
end

--子账号数量信息结构
--level number 代理等级
--amount number 数量
local function CLPFAgentSubAccountAmountInfo_deserialize(br)
    local r = {}
    r.level = br:ReadInt32()
    r.amount = br:ReadInt32()
    return r
end
local function CLPFAgentSubAccountAmountInfo_serialize(t, bw)
    bw:WriteInt32(t.level)
    bw:WriteInt32(t.amount)
end

--子账号贡献信息结构
--user_id number 玩家Id
--register_time number 注册时间戳
--nickname string[32] 昵称
--total_recharge number 充值总额，单位分
--total_contribution number 总贡献，客户端显示时先除以100
--today_recharge number 当天充值，单位分
--today_contribution number 当天贡献，客户端显示时先除以100
--agent_level number 代理级别
local function CLPFAgentContributionInfo_deserialize(br)
    local r = {}
    r.user_id = br:ReadInt32()
    r.register_time = br:ReadUInt32()
    r.nickname = br:ReadString()
    r.total_recharge = br:ReadInt32()
    r.total_contribution = br:ReadInt32()
    r.today_recharge = br:ReadInt32()
    r.today_contribution = br:ReadInt32()
    r.agent_level = br:ReadInt32()
    return r
end
local function CLPFAgentContributionInfo_serialize(t, bw)
    bw:WriteInt32(t.user_id)
    bw:WriteUInt32(t.register_time)
    bw:WriteString(t.nickname, 32)
    bw:WriteInt32(t.total_recharge)
    bw:WriteInt32(t.total_contribution)
    bw:WriteInt32(t.today_recharge)
    bw:WriteInt32(t.today_contribution)
    bw:WriteInt32(t.agent_level)
end

--子账号充值信息结构
--timestamp number 充值时间戳
--order_amount number 充值金额，单位分
local function CLPFAgentRechargeInfo_deserialize(br)
    local r = {}
    r.timestamp = br:ReadUInt32()
    r.order_amount = br:ReadInt32()
    return r
end
local function CLPFAgentRechargeInfo_serialize(t, bw)
    bw:WriteUInt32(t.timestamp)
    bw:WriteInt32(t.order_amount)
end

--查询我的推广信息请求
_G.CLPFAgentQueryInfoReq = _G.class(ProtocolBase)
function _G.CLPFAgentQueryInfoReq:__init(t)
    _G.CLPFAgentQueryInfoReq.super:__init(t)
    self.mid = 2
    self.pid = 186
end

--查询我的推广信息回应
--errcode number 0成功 1全民代理尚未开放
--url_params string[255] 我的推广链接附加参数信息
--current_assets number 我的当前点数，客户端需要除以100再显示
--today_assets number 当天获得的点数，客户端需要除以100再显示
--history_assets number 历史获得总点数，客户端需要除以100再显示
_G.CLPFAgentQueryInfoAck = _G.class(ProtocolBase)
function _G.CLPFAgentQueryInfoAck:__init(t)
    _G.CLPFAgentQueryInfoAck.super:__init(t)
    self.mid = 2
    self.pid = 187
end
function _G.CLPFAgentQueryInfoAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.url_params = br:ReadString()
    self.current_assets = br:ReadInt32()
    self.today_assets = br:ReadInt32()
    self.history_assets = br:ReadInt32()
end
function _G.CLPFAgentQueryInfoAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteString(self.url_params, 255)
    bw:WriteInt32(self.current_assets)
    bw:WriteInt32(self.today_assets)
    bw:WriteInt32(self.history_assets)
end

--查询子账号数量请求
_G.CLPFAgentQuerySubAccountAmountReq = _G.class(ProtocolBase)
function _G.CLPFAgentQuerySubAccountAmountReq:__init(t)
    _G.CLPFAgentQuerySubAccountAmountReq.super:__init(t)
    self.mid = 2
    self.pid = 188
end

--查询子账号数量回应
--errcode number 0成功 1全民代理尚未开放
--total_amount number 我下面的账号总数量
--data_length number 代理数量数组长度
--data_array AgentSubAccountAmountInfo[3] 代理数量数组
_G.CLPFAgentQuerySubAccountAmountAck = _G.class(ProtocolBase)
function _G.CLPFAgentQuerySubAccountAmountAck:__init(t)
    _G.CLPFAgentQuerySubAccountAmountAck.super:__init(t)
    self.mid = 2
    self.pid = 189
end
function _G.CLPFAgentQuerySubAccountAmountAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.total_amount = br:ReadInt32()
    self.data_length = br:ReadInt32()
    self.data_array = {}
    for i=1,self.data_length do
        self.data_array[i] = CLPFAgentSubAccountAmountInfo_deserialize(br)
    end
end
function _G.CLPFAgentQuerySubAccountAmountAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    bw:WriteInt32(self.total_amount)
    assert(not self.data_array or #self.data_array <= 3, "CLPFAgentQuerySubAccountAmountAck.data_array数组长度超过规定限制! 期望:3 实际:" .. #self.data_array)
    bw:WriteInt32(#(self.data_array or {}))
    for i=1,#(self.data_array or {}) do
        CLPFAgentSubAccountAmountInfo_serialize(self.data_array[i], bw)
    end
end

--查询子账号的贡献列表请求
--page_index number 查询第几页？每页20条
_G.CLPFAgentQueryContributionListReq = _G.class(ProtocolBase)
function _G.CLPFAgentQueryContributionListReq:__init(t)
    _G.CLPFAgentQueryContributionListReq.super:__init(t)
    self.mid = 2
    self.pid = 190
end
function _G.CLPFAgentQueryContributionListReq:deserialize(br)
    self.page_index = br:ReadInt32()
end
function _G.CLPFAgentQueryContributionListReq:serialize(bw)
    bw:WriteInt32(self.page_index)
end

--查询子账号的贡献列表回应
--errcode number 0成功 1全民代理尚未开放
--data_length number 子账号贡献数组长度
--data_array AgentContributionInfo[20] 子账号贡献数组
_G.CLPFAgentQueryContributionListAck = _G.class(ProtocolBase)
function _G.CLPFAgentQueryContributionListAck:__init(t)
    _G.CLPFAgentQueryContributionListAck.super:__init(t)
    self.mid = 2
    self.pid = 191
end
function _G.CLPFAgentQueryContributionListAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.data_length = br:ReadInt32()
    self.data_array = {}
    for i=1,self.data_length do
        self.data_array[i] = CLPFAgentContributionInfo_deserialize(br)
    end
end
function _G.CLPFAgentQueryContributionListAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.data_array or #self.data_array <= 20, "CLPFAgentQueryContributionListAck.data_array数组长度超过规定限制! 期望:20 实际:" .. #self.data_array)
    bw:WriteInt32(#(self.data_array or {}))
    for i=1,#(self.data_array or {}) do
        CLPFAgentContributionInfo_serialize(self.data_array[i], bw)
    end
end

--查询某个玩家详细贡献请求
--user_id number 要查询的玩家Id
--page_index number 请求第多少页数据？每页10条
_G.CLPFAgentQueryContributionUserReq = _G.class(ProtocolBase)
function _G.CLPFAgentQueryContributionUserReq:__init(t)
    _G.CLPFAgentQueryContributionUserReq.super:__init(t)
    self.mid = 2
    self.pid = 192
end
function _G.CLPFAgentQueryContributionUserReq:deserialize(br)
    self.user_id = br:ReadInt32()
    self.page_index = br:ReadInt32()
end
function _G.CLPFAgentQueryContributionUserReq:serialize(bw)
    bw:WriteInt32(self.user_id)
    bw:WriteInt32(self.page_index)
end

--查询某个玩家详细贡献回应
--errcode number 0成功 1全民代理尚未开放
--data_length number 详细贡献数组长度
--data_array AgentRechargeInfo[10] 详细贡献数组
_G.CLPFAgentQueryContributionUserAck = _G.class(ProtocolBase)
function _G.CLPFAgentQueryContributionUserAck:__init(t)
    _G.CLPFAgentQueryContributionUserAck.super:__init(t)
    self.mid = 2
    self.pid = 193
end
function _G.CLPFAgentQueryContributionUserAck:deserialize(br)
    self.errcode = br:ReadInt8()
    self.data_length = br:ReadInt32()
    self.data_array = {}
    for i=1,self.data_length do
        self.data_array[i] = CLPFAgentRechargeInfo_deserialize(br)
    end
end
function _G.CLPFAgentQueryContributionUserAck:serialize(bw)
    bw:WriteInt8(self.errcode)
    assert(not self.data_array or #self.data_array <= 10, "CLPFAgentQueryContributionUserAck.data_array数组长度超过规定限制! 期望:10 实际:" .. #self.data_array)
    bw:WriteInt32(#(self.data_array or {}))
    for i=1,#(self.data_array or {}) do
        CLPFAgentRechargeInfo_serialize(self.data_array[i], bw)
    end
end

--注册协议
ProtocolHelper.registerProtocol("CLPFLogoutReq", _G.CLPFLogoutReq, 2, 0)
ProtocolHelper.registerProtocol("CLPFResSyncNtf", _G.CLPFResSyncNtf, 2, 1)
ProtocolHelper.registerProtocol("CLPFResChangedNtf", _G.CLPFResChangedNtf, 2, 2)
ProtocolHelper.registerProtocol("CLPFItemGetListReq", _G.CLPFItemGetListReq, 2, 3)
ProtocolHelper.registerProtocol("CLPFItemGetListAck", _G.CLPFItemGetListAck, 2, 4)
ProtocolHelper.registerProtocol("CLPFItemUseReq", _G.CLPFItemUseReq, 2, 5)
ProtocolHelper.registerProtocol("CLPFItemUseAck", _G.CLPFItemUseAck, 2, 6)
ProtocolHelper.registerProtocol("CLPFItemCountChangeNtf", _G.CLPFItemCountChangeNtf, 2, 7)
ProtocolHelper.registerProtocol("CLPFItemBuyReq", _G.CLPFItemBuyReq, 2, 8)
ProtocolHelper.registerProtocol("CLPFItemBuyAck", _G.CLPFItemBuyAck, 2, 9)
ProtocolHelper.registerProtocol("CLPFShopQueryBuyCountReq", _G.CLPFShopQueryBuyCountReq, 2, 10)
ProtocolHelper.registerProtocol("CLPFShopQueryBuyCountAck", _G.CLPFShopQueryBuyCountAck, 2, 11)
ProtocolHelper.registerProtocol("CLPFShopBuyReq", _G.CLPFShopBuyReq, 2, 12)
ProtocolHelper.registerProtocol("CLPFShopBuyAck", _G.CLPFShopBuyAck, 2, 13)
ProtocolHelper.registerProtocol("CLPFRechargeReq", _G.CLPFRechargeReq, 2, 14)
ProtocolHelper.registerProtocol("CLPFRechargeAck", _G.CLPFRechargeAck, 2, 15)
ProtocolHelper.registerProtocol("CLPFRechargeSuccessNtf", _G.CLPFRechargeSuccessNtf, 2, 16)
ProtocolHelper.registerProtocol("CLPFGetRankListReq", _G.CLPFGetRankListReq, 2, 17)
ProtocolHelper.registerProtocol("CLPFGetRankListAck", _G.CLPFGetRankListAck, 2, 18)
ProtocolHelper.registerProtocol("CLPFLevelExpChangedNtf", _G.CLPFLevelExpChangedNtf, 2, 19)
ProtocolHelper.registerProtocol("CLPFLevelUpNtf", _G.CLPFLevelUpNtf, 2, 20)
ProtocolHelper.registerProtocol("CLPFVipExpChangedNtf", _G.CLPFVipExpChangedNtf, 2, 21)
ProtocolHelper.registerProtocol("CLPFModifyNicknameReq", _G.CLPFModifyNicknameReq, 2, 22)
ProtocolHelper.registerProtocol("CLPFModifyNicknameAck", _G.CLPFModifyNicknameAck, 2, 23)
ProtocolHelper.registerProtocol("CLPFModifyHeadReq", _G.CLPFModifyHeadReq, 2, 24)
ProtocolHelper.registerProtocol("CLPFModifyHeadAck", _G.CLPFModifyHeadAck, 2, 25)
ProtocolHelper.registerProtocol("CLPFQuerySignReq", _G.CLPFQuerySignReq, 2, 26)
ProtocolHelper.registerProtocol("CLPFQuerySignAck", _G.CLPFQuerySignAck, 2, 27)
ProtocolHelper.registerProtocol("CLPFActSignReq", _G.CLPFActSignReq, 2, 28)
ProtocolHelper.registerProtocol("CLPFActSignAck", _G.CLPFActSignAck, 2, 29)
ProtocolHelper.registerProtocol("CLPFQueryVipWheelReq", _G.CLPFQueryVipWheelReq, 2, 30)
ProtocolHelper.registerProtocol("CLPFQueryVipWheelAck", _G.CLPFQueryVipWheelAck, 2, 31)
ProtocolHelper.registerProtocol("CLPFActVipWheelReq", _G.CLPFActVipWheelReq, 2, 32)
ProtocolHelper.registerProtocol("CLPFActVipWheelAck", _G.CLPFActVipWheelAck, 2, 33)
ProtocolHelper.registerProtocol("CLPFMailQueryAllIdsReq", _G.CLPFMailQueryAllIdsReq, 2, 34)
ProtocolHelper.registerProtocol("CLPFMailQueryAllIdsAck", _G.CLPFMailQueryAllIdsAck, 2, 35)
ProtocolHelper.registerProtocol("CLPFMailBatchQueryContentReq", _G.CLPFMailBatchQueryContentReq, 2, 36)
ProtocolHelper.registerProtocol("CLPFMailBatchQueryContentAck", _G.CLPFMailBatchQueryContentAck, 2, 37)
ProtocolHelper.registerProtocol("CLPFMailAccessReq", _G.CLPFMailAccessReq, 2, 38)
ProtocolHelper.registerProtocol("CLPFMailAccessAck", _G.CLPFMailAccessAck, 2, 39)
ProtocolHelper.registerProtocol("CLPFMailFetchItemReq", _G.CLPFMailFetchItemReq, 2, 40)
ProtocolHelper.registerProtocol("CLPFMailFetchItemAck", _G.CLPFMailFetchItemAck, 2, 41)
ProtocolHelper.registerProtocol("CLPFMailRemoveReq", _G.CLPFMailRemoveReq, 2, 42)
ProtocolHelper.registerProtocol("CLPFMailRemoveAck", _G.CLPFMailRemoveAck, 2, 43)
ProtocolHelper.registerProtocol("CLPFMailArriveNtf", _G.CLPFMailArriveNtf, 2, 44)
ProtocolHelper.registerProtocol("CLPFGuildCreateReq", _G.CLPFGuildCreateReq, 2, 45)
ProtocolHelper.registerProtocol("CLPFGuildCreateAck", _G.CLPFGuildCreateAck, 2, 46)
ProtocolHelper.registerProtocol("CLPFGuildQueryRecommendListReq", _G.CLPFGuildQueryRecommendListReq, 2, 47)
ProtocolHelper.registerProtocol("CLPFGuildQueryRecommendListAck", _G.CLPFGuildQueryRecommendListAck, 2, 48)
ProtocolHelper.registerProtocol("CLPFGuildSearchReq", _G.CLPFGuildSearchReq, 2, 49)
ProtocolHelper.registerProtocol("CLPFGuildSearchAck", _G.CLPFGuildSearchAck, 2, 50)
ProtocolHelper.registerProtocol("CLPFGuildQuickJoinReq", _G.CLPFGuildQuickJoinReq, 2, 51)
ProtocolHelper.registerProtocol("CLPFGuildQuickJoinAck", _G.CLPFGuildQuickJoinAck, 2, 52)
ProtocolHelper.registerProtocol("CLPFGuildJoinReq", _G.CLPFGuildJoinReq, 2, 53)
ProtocolHelper.registerProtocol("CLPFGuildJoinAck", _G.CLPFGuildJoinAck, 2, 54)
ProtocolHelper.registerProtocol("CLPFGuildQueryJoinListReq", _G.CLPFGuildQueryJoinListReq, 2, 55)
ProtocolHelper.registerProtocol("CLPFGuildQueryJoinListAck", _G.CLPFGuildQueryJoinListAck, 2, 56)
ProtocolHelper.registerProtocol("CLPFGuildHandleJoinReq", _G.CLPFGuildHandleJoinReq, 2, 57)
ProtocolHelper.registerProtocol("CLPFGuildHandleJoinAck", _G.CLPFGuildHandleJoinAck, 2, 58)
ProtocolHelper.registerProtocol("CLPFGuildJoinResponseNtf", _G.CLPFGuildJoinResponseNtf, 2, 59)
ProtocolHelper.registerProtocol("CLPFGuildNewJoinRequestNtf", _G.CLPFGuildNewJoinRequestNtf, 2, 60)
ProtocolHelper.registerProtocol("CLPFGuildQueryInfoReq", _G.CLPFGuildQueryInfoReq, 2, 61)
ProtocolHelper.registerProtocol("CLPFGuildQueryInfoAck", _G.CLPFGuildQueryInfoAck, 2, 62)
ProtocolHelper.registerProtocol("CLPFGuildModifyInfoReq", _G.CLPFGuildModifyInfoReq, 2, 63)
ProtocolHelper.registerProtocol("CLPFGuildModifyInfoAck", _G.CLPFGuildModifyInfoAck, 2, 64)
ProtocolHelper.registerProtocol("CLPFGuildModifyMemberJobReq", _G.CLPFGuildModifyMemberJobReq, 2, 65)
ProtocolHelper.registerProtocol("CLPFGuildModifyMemberJobAck", _G.CLPFGuildModifyMemberJobAck, 2, 66)
ProtocolHelper.registerProtocol("CLPFGuildKickMemberReq", _G.CLPFGuildKickMemberReq, 2, 67)
ProtocolHelper.registerProtocol("CLPFGuildKickMemberAck", _G.CLPFGuildKickMemberAck, 2, 68)
ProtocolHelper.registerProtocol("CLPFGuildKickMemberNtf", _G.CLPFGuildKickMemberNtf, 2, 69)
ProtocolHelper.registerProtocol("CLPFGuildExitReq", _G.CLPFGuildExitReq, 2, 70)
ProtocolHelper.registerProtocol("CLPFGuildExitAck", _G.CLPFGuildExitAck, 2, 71)
ProtocolHelper.registerProtocol("CLPFGuildUpgradeReq", _G.CLPFGuildUpgradeReq, 2, 72)
ProtocolHelper.registerProtocol("CLPFGuildUpgradeAck", _G.CLPFGuildUpgradeAck, 2, 73)
ProtocolHelper.registerProtocol("CLPFGuildQueryWelfareReq", _G.CLPFGuildQueryWelfareReq, 2, 74)
ProtocolHelper.registerProtocol("CLPFGuildQueryWelfareAck", _G.CLPFGuildQueryWelfareAck, 2, 75)
ProtocolHelper.registerProtocol("CLPFGuildFetchWelfareReq", _G.CLPFGuildFetchWelfareReq, 2, 76)
ProtocolHelper.registerProtocol("CLPFGuildFetchWelfareAck", _G.CLPFGuildFetchWelfareAck, 2, 77)
ProtocolHelper.registerProtocol("CLPFGuildQueryRedPacketInfoReq", _G.CLPFGuildQueryRedPacketInfoReq, 2, 78)
ProtocolHelper.registerProtocol("CLPFGuildQueryRedPacketInfoAck", _G.CLPFGuildQueryRedPacketInfoAck, 2, 79)
ProtocolHelper.registerProtocol("CLPFGuildQueryRedPacketRankReq", _G.CLPFGuildQueryRedPacketRankReq, 2, 80)
ProtocolHelper.registerProtocol("CLPFGuildQueryRedPacketRankAck", _G.CLPFGuildQueryRedPacketRankAck, 2, 81)
ProtocolHelper.registerProtocol("CLPFGuildActRedPacketReq", _G.CLPFGuildActRedPacketReq, 2, 82)
ProtocolHelper.registerProtocol("CLPFGuildActRedPacketAck", _G.CLPFGuildActRedPacketAck, 2, 83)
ProtocolHelper.registerProtocol("CLPFGuildBagQueryInfoReq", _G.CLPFGuildBagQueryInfoReq, 2, 84)
ProtocolHelper.registerProtocol("CLPFGuildBagQueryInfoAck", _G.CLPFGuildBagQueryInfoAck, 2, 85)
ProtocolHelper.registerProtocol("CLPFGuildBagQueryLogReq", _G.CLPFGuildBagQueryLogReq, 2, 86)
ProtocolHelper.registerProtocol("CLPFGuildBagQueryLogAck", _G.CLPFGuildBagQueryLogAck, 2, 87)
ProtocolHelper.registerProtocol("CLPFGuildBagStoreItemReq", _G.CLPFGuildBagStoreItemReq, 2, 88)
ProtocolHelper.registerProtocol("CLPFGuildBagStoreItemAck", _G.CLPFGuildBagStoreItemAck, 2, 89)
ProtocolHelper.registerProtocol("CLPFGuildBagFetchItemReq", _G.CLPFGuildBagFetchItemReq, 2, 90)
ProtocolHelper.registerProtocol("CLPFGuildBagFetchItemAck", _G.CLPFGuildBagFetchItemAck, 2, 91)
ProtocolHelper.registerProtocol("CLPFGuildBagFetchItemNtf", _G.CLPFGuildBagFetchItemNtf, 2, 92)
ProtocolHelper.registerProtocol("CLPFMessageBroadcastNtf", _G.CLPFMessageBroadcastNtf, 2, 93)
ProtocolHelper.registerProtocol("CLPFTaskQueryReq", _G.CLPFTaskQueryReq, 2, 94)
ProtocolHelper.registerProtocol("CLPFTaskQueryAck", _G.CLPFTaskQueryAck, 2, 95)
ProtocolHelper.registerProtocol("CLPFTaskFetchTaskRewardsReq", _G.CLPFTaskFetchTaskRewardsReq, 2, 96)
ProtocolHelper.registerProtocol("CLPFTaskFetchTaskRewardsAck", _G.CLPFTaskFetchTaskRewardsAck, 2, 97)
ProtocolHelper.registerProtocol("CLPFTaskFetchActiveRewardsReq", _G.CLPFTaskFetchActiveRewardsReq, 2, 98)
ProtocolHelper.registerProtocol("CLPFTaskFetchActiveRewardsAck", _G.CLPFTaskFetchActiveRewardsAck, 2, 99)
ProtocolHelper.registerProtocol("CLPFTaskAchieveQueryInfoReq", _G.CLPFTaskAchieveQueryInfoReq, 2, 100)
ProtocolHelper.registerProtocol("CLPFTaskAchieveQueryInfoAck", _G.CLPFTaskAchieveQueryInfoAck, 2, 101)
ProtocolHelper.registerProtocol("CLPFTaskAchieveFetchRewardReq", _G.CLPFTaskAchieveFetchRewardReq, 2, 102)
ProtocolHelper.registerProtocol("CLPFTaskAchieveFetchRewardAck", _G.CLPFTaskAchieveFetchRewardAck, 2, 103)
ProtocolHelper.registerProtocol("CLPFMonthCardFetchRewardReq", _G.CLPFMonthCardFetchRewardReq, 2, 104)
ProtocolHelper.registerProtocol("CLPFMonthCardFetchRewardAck", _G.CLPFMonthCardFetchRewardAck, 2, 105)
ProtocolHelper.registerProtocol("CLPFReliefGoldFetchReq", _G.CLPFReliefGoldFetchReq, 2, 106)
ProtocolHelper.registerProtocol("CLPFReliefGoldFetchAck", _G.CLPFReliefGoldFetchAck, 2, 107)
ProtocolHelper.registerProtocol("CLPFShakeNumberQueryInfoReq", _G.CLPFShakeNumberQueryInfoReq, 2, 108)
ProtocolHelper.registerProtocol("CLPFShakeNumberQueryInfoAck", _G.CLPFShakeNumberQueryInfoAck, 2, 109)
ProtocolHelper.registerProtocol("CLPFShakeNumberActReq", _G.CLPFShakeNumberActReq, 2, 110)
ProtocolHelper.registerProtocol("CLPFShakeNumberActAck", _G.CLPFShakeNumberActAck, 2, 111)
ProtocolHelper.registerProtocol("CLPFShakeNumberFetchRewardReq", _G.CLPFShakeNumberFetchRewardReq, 2, 112)
ProtocolHelper.registerProtocol("CLPFShakeNumberFetchRewardAck", _G.CLPFShakeNumberFetchRewardAck, 2, 113)
ProtocolHelper.registerProtocol("CLPFShakeNumberFetchBoxRewardReq", _G.CLPFShakeNumberFetchBoxRewardReq, 2, 114)
ProtocolHelper.registerProtocol("CLPFShakeNumberFetchBoxRewardAck", _G.CLPFShakeNumberFetchBoxRewardAck, 2, 115)
ProtocolHelper.registerProtocol("CLPFRechargeDailyQueryReq", _G.CLPFRechargeDailyQueryReq, 2, 116)
ProtocolHelper.registerProtocol("CLPFRechargeDailyQueryAck", _G.CLPFRechargeDailyQueryAck, 2, 117)
ProtocolHelper.registerProtocol("CLPFWelfarePigQueryInfoReq", _G.CLPFWelfarePigQueryInfoReq, 2, 118)
ProtocolHelper.registerProtocol("CLPFWelfarePigQueryInfoAck", _G.CLPFWelfarePigQueryInfoAck, 2, 119)
ProtocolHelper.registerProtocol("CLPFWelfarePigFetchMaterialReq", _G.CLPFWelfarePigFetchMaterialReq, 2, 120)
ProtocolHelper.registerProtocol("CLPFWelfarePigFetchMaterialAck", _G.CLPFWelfarePigFetchMaterialAck, 2, 121)
ProtocolHelper.registerProtocol("CLPFWelfarePigBrokenReq", _G.CLPFWelfarePigBrokenReq, 2, 122)
ProtocolHelper.registerProtocol("CLPFWelfarePigBrokenAck", _G.CLPFWelfarePigBrokenAck, 2, 123)
ProtocolHelper.registerProtocol("CLPFWelfarePigSearchReq", _G.CLPFWelfarePigSearchReq, 2, 124)
ProtocolHelper.registerProtocol("CLPFWelfarePigSearchAck", _G.CLPFWelfarePigSearchAck, 2, 125)
ProtocolHelper.registerProtocol("CLPFInvestGunQueryInfoReq", _G.CLPFInvestGunQueryInfoReq, 2, 126)
ProtocolHelper.registerProtocol("CLPFInvestGunQueryInfoAck", _G.CLPFInvestGunQueryInfoAck, 2, 127)
ProtocolHelper.registerProtocol("CLPFInvestGunFetchRewardReq", _G.CLPFInvestGunFetchRewardReq, 2, 128)
ProtocolHelper.registerProtocol("CLPFInvestGunFetchRewardAck", _G.CLPFInvestGunFetchRewardAck, 2, 129)
ProtocolHelper.registerProtocol("CLPFInvestCostQueryInfoReq", _G.CLPFInvestCostQueryInfoReq, 2, 130)
ProtocolHelper.registerProtocol("CLPFInvestCostQueryInfoAck", _G.CLPFInvestCostQueryInfoAck, 2, 131)
ProtocolHelper.registerProtocol("CLPFInvestCostFetchRewardReq", _G.CLPFInvestCostFetchRewardReq, 2, 132)
ProtocolHelper.registerProtocol("CLPFInvestCostFetchRewardAck", _G.CLPFInvestCostFetchRewardAck, 2, 133)
ProtocolHelper.registerProtocol("CLPFFirstPackageFetchReq", _G.CLPFFirstPackageFetchReq, 2, 134)
ProtocolHelper.registerProtocol("CLPFFirstPackageFetchAck", _G.CLPFFirstPackageFetchAck, 2, 135)
ProtocolHelper.registerProtocol("CLPFAnnouncementChangedNtf", _G.CLPFAnnouncementChangedNtf, 2, 136)
ProtocolHelper.registerProtocol("CLPFVipFillUpCurrencyNtf", _G.CLPFVipFillUpCurrencyNtf, 2, 137)
ProtocolHelper.registerProtocol("CLPFRealGoodsQueryAddressReq", _G.CLPFRealGoodsQueryAddressReq, 2, 138)
ProtocolHelper.registerProtocol("CLPFRealGoodsQueryAddressAck", _G.CLPFRealGoodsQueryAddressAck, 2, 139)
ProtocolHelper.registerProtocol("CLPFRealGoodsCreateOrderReq", _G.CLPFRealGoodsCreateOrderReq, 2, 140)
ProtocolHelper.registerProtocol("CLPFRealGoodsCreateOrderAck", _G.CLPFRealGoodsCreateOrderAck, 2, 141)
ProtocolHelper.registerProtocol("CLPFRealGoodsQueryExchangeLogReq", _G.CLPFRealGoodsQueryExchangeLogReq, 2, 142)
ProtocolHelper.registerProtocol("CLPFRealGoodsQueryExchangeLogAck", _G.CLPFRealGoodsQueryExchangeLogAck, 2, 143)
ProtocolHelper.registerProtocol("CLPFGuideDataQueryReq", _G.CLPFGuideDataQueryReq, 2, 144)
ProtocolHelper.registerProtocol("CLPFGuideDataQueryAck", _G.CLPFGuideDataQueryAck, 2, 145)
ProtocolHelper.registerProtocol("CLPFGuideDataActRpt", _G.CLPFGuideDataActRpt, 2, 146)
ProtocolHelper.registerProtocol("CLPFClientConfigPublishNtf", _G.CLPFClientConfigPublishNtf, 2, 147)
ProtocolHelper.registerProtocol("CLPFSubGamesOnlineCountReq", _G.CLPFSubGamesOnlineCountReq, 2, 148)
ProtocolHelper.registerProtocol("CLPFSubGamesOnlineCountAck", _G.CLPFSubGamesOnlineCountAck, 2, 149)
ProtocolHelper.registerProtocol("CLPFCdkeyFetchRewardReq", _G.CLPFCdkeyFetchRewardReq, 2, 150)
ProtocolHelper.registerProtocol("CLPFCdkeyFetchRewardAck", _G.CLPFCdkeyFetchRewardAck, 2, 151)
ProtocolHelper.registerProtocol("CLPFAccountBindStateReq", _G.CLPFAccountBindStateReq, 2, 152)
ProtocolHelper.registerProtocol("CLPFAccountBindStateAck", _G.CLPFAccountBindStateAck, 2, 153)
ProtocolHelper.registerProtocol("CLPFAccountPhoneBindReq", _G.CLPFAccountPhoneBindReq, 2, 154)
ProtocolHelper.registerProtocol("CLPFAccountPhoneBindAck", _G.CLPFAccountPhoneBindAck, 2, 155)
ProtocolHelper.registerProtocol("CLPFAccountPhoneChange1Req", _G.CLPFAccountPhoneChange1Req, 2, 156)
ProtocolHelper.registerProtocol("CLPFAccountPhoneChange1Ack", _G.CLPFAccountPhoneChange1Ack, 2, 157)
ProtocolHelper.registerProtocol("CLPFAccountPhoneChange2Req", _G.CLPFAccountPhoneChange2Req, 2, 158)
ProtocolHelper.registerProtocol("CLPFAccountPhoneChange2Ack", _G.CLPFAccountPhoneChange2Ack, 2, 159)
ProtocolHelper.registerProtocol("CLPFAccountUniformBindReq", _G.CLPFAccountUniformBindReq, 2, 160)
ProtocolHelper.registerProtocol("CLPFAccountUniformBindAck", _G.CLPFAccountUniformBindAck, 2, 161)
ProtocolHelper.registerProtocol("CLPFAccountUniformUnbindReq", _G.CLPFAccountUniformUnbindReq, 2, 162)
ProtocolHelper.registerProtocol("CLPFAccountUniformUnbindAck", _G.CLPFAccountUniformUnbindAck, 2, 163)
ProtocolHelper.registerProtocol("CLPFPlayerNicknameQueryReq", _G.CLPFPlayerNicknameQueryReq, 2, 164)
ProtocolHelper.registerProtocol("CLPFPlayerNicknameQueryAck", _G.CLPFPlayerNicknameQueryAck, 2, 165)
ProtocolHelper.registerProtocol("CLPFBankPasswordInitReq", _G.CLPFBankPasswordInitReq, 2, 166)
ProtocolHelper.registerProtocol("CLPFBankPasswordInitAck", _G.CLPFBankPasswordInitAck, 2, 167)
ProtocolHelper.registerProtocol("CLPFBankPasswordVerifyReq", _G.CLPFBankPasswordVerifyReq, 2, 168)
ProtocolHelper.registerProtocol("CLPFBankPasswordVerifyAck", _G.CLPFBankPasswordVerifyAck, 2, 169)
ProtocolHelper.registerProtocol("CLPFBankPasswordModifyReq", _G.CLPFBankPasswordModifyReq, 2, 170)
ProtocolHelper.registerProtocol("CLPFBankPasswordModifyAck", _G.CLPFBankPasswordModifyAck, 2, 171)
ProtocolHelper.registerProtocol("CLPFBankPasswordResetReq", _G.CLPFBankPasswordResetReq, 2, 172)
ProtocolHelper.registerProtocol("CLPFBankPasswordResetAck", _G.CLPFBankPasswordResetAck, 2, 173)
ProtocolHelper.registerProtocol("CLPFBankItemQueryReq", _G.CLPFBankItemQueryReq, 2, 174)
ProtocolHelper.registerProtocol("CLPFBankItemQueryAck", _G.CLPFBankItemQueryAck, 2, 175)
ProtocolHelper.registerProtocol("CLPFBankItemStoreReq", _G.CLPFBankItemStoreReq, 2, 176)
ProtocolHelper.registerProtocol("CLPFBankItemStoreAck", _G.CLPFBankItemStoreAck, 2, 177)
ProtocolHelper.registerProtocol("CLPFBankItemFetchReq", _G.CLPFBankItemFetchReq, 2, 178)
ProtocolHelper.registerProtocol("CLPFBankItemFetchAck", _G.CLPFBankItemFetchAck, 2, 179)
ProtocolHelper.registerProtocol("CLPFBankItemSendReq", _G.CLPFBankItemSendReq, 2, 180)
ProtocolHelper.registerProtocol("CLPFBankItemSendAck", _G.CLPFBankItemSendAck, 2, 181)
ProtocolHelper.registerProtocol("CLPFBankItemLogQueryReq", _G.CLPFBankItemLogQueryReq, 2, 182)
ProtocolHelper.registerProtocol("CLPFBankItemLogQueryAck", _G.CLPFBankItemLogQueryAck, 2, 183)
ProtocolHelper.registerProtocol("CLPFBankItemLogDetailQueryReq", _G.CLPFBankItemLogDetailQueryReq, 2, 184)
ProtocolHelper.registerProtocol("CLPFBankItemLogDetailQueryAck", _G.CLPFBankItemLogDetailQueryAck, 2, 185)
ProtocolHelper.registerProtocol("CLPFAgentQueryInfoReq", _G.CLPFAgentQueryInfoReq, 2, 186)
ProtocolHelper.registerProtocol("CLPFAgentQueryInfoAck", _G.CLPFAgentQueryInfoAck, 2, 187)
ProtocolHelper.registerProtocol("CLPFAgentQuerySubAccountAmountReq", _G.CLPFAgentQuerySubAccountAmountReq, 2, 188)
ProtocolHelper.registerProtocol("CLPFAgentQuerySubAccountAmountAck", _G.CLPFAgentQuerySubAccountAmountAck, 2, 189)
ProtocolHelper.registerProtocol("CLPFAgentQueryContributionListReq", _G.CLPFAgentQueryContributionListReq, 2, 190)
ProtocolHelper.registerProtocol("CLPFAgentQueryContributionListAck", _G.CLPFAgentQueryContributionListAck, 2, 191)
ProtocolHelper.registerProtocol("CLPFAgentQueryContributionUserReq", _G.CLPFAgentQueryContributionUserReq, 2, 192)
ProtocolHelper.registerProtocol("CLPFAgentQueryContributionUserAck", _G.CLPFAgentQueryContributionUserAck, 2, 193)
