--FrameWork
_G.json = require "LuaUtil.dkjson"
require "LuaUtil.Functions"
require "LuaUtil.LuaDefines"

--Define
require "Defines.LocalDefines"

--Message
require "Message.MessageCenter"
MsgType = require "Message.MessageType"

--Module
require "Module.ModuleManager"
require "Module.MainModule"
require "Module.FishModule"

require "Module.Room.FishPath"
require "Module.Room.FishTimeConverter"
require "Module.Room.RoomBullet"
require "Module.Room.RoomFish"
require "Module.Room.RoomPlayer"

--Pool
require "Pool.PoolManager"

--Controller
require "Controller.NetController"
require "Controller.FishRoomController"

--Game
require "Game.FishUIController"
require "Game.FishSpawn"
require "Game.FishScript"
require "Game.BulletSpawn"
require "Game.BulletScript"
require "Game.LiveEff"

--UI
require "UI.LoadingUI"
require "UI.RoomUI"
require "UI.GameUI"
require "UI.LeaveUI"
require "UI.SettingUI"
require "UI.FishHandBookUI"

--Protocol
require "JBPROTO.ClientGT"
require "JBPROTO.ClientPF"
require "JBPROTO.ClientFishingMain"
require "JBPROTO.ClientFishingRoom"

--Tools
require "Tools.DynamicInfinityList"
require "Utils.SeatHelper"