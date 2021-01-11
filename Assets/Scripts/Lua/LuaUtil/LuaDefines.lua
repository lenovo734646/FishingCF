--Debug.Log
log = CS.UnityEngine.Debug.Log
logWarning = CS.UnityEngine.Debug.LogWarning
logError = CS.UnityEngine.Debug.LogError

--UnityEngine
UnityEngine = CS.UnityEngine
Application = CS.UnityEngine.Application
Instantiate = CS.UnityEngine.Object.Instantiate
Destroy = CS.UnityEngine.Object.Destroy
GameObject = CS.UnityEngine.GameObject
Transform = CS.UnityEngine.Transform
Color = CS.UnityEngine.Color
TextAsset = CS.UnityEngine.TextAsset
Sprite = CS.UnityEngine.Sprite
Animator = CS.UnityEngine.Animator
Time = CS.UnityEngine.Time
SceneManager = CS.UnityEngine.SceneManagement.SceneManager
Math = CS.UnityEngine.Mathf
Vector3 = CS.UnityEngine.Vector3
Vector2 = CS.UnityEngine.Vector2
PlayerPrefs = CS.UnityEngine.PlayerPrefs
AudioClip = CS.UnityEngine.AudioClip
Quaternion = CS.UnityEngine.Quaternion
Renderer = CS.UnityEngine.Renderer
Collider = CS.UnityEngine.Collider
Input = CS.UnityEngine.Input
BoxCollider = CS.UnityEngine.BoxCollider

--UnityEngine.UI
Toggle = CS.UnityEngine.UI.Toggle
Text = CS.UnityEngine.UI.Text
Image = CS.UnityEngine.UI.Image
Button = CS.UnityEngine.UI.Button

--XLuaExtension
UpdateLuaBehaviour = CS.XLuaExtension.UpdateLuaBehaviour
UGUIClickLuaBehaviour = CS.XLuaExtension.UGUIClickLuaBehaviour
ColliderLuaBehaviour = CS.XLuaExtension.ColliderLuaBehaviour
DestroyLuaBehaviour = CS.XLuaExtension.DestroyLuaBehaviour

--CustomCSharp
Context = CS.Context
AssetConfig = CS.AssetConfig
Loader = CS.Context.Game.Loader
AppRoot = CS.AppRoot.Get()
AsyncImageDownload = CS.AsyncImageDownload.Instance

--Spine
SkeletonGraphic = CS.Spine.Unity.SkeletonGraphic

--Tweening
DOTween = CS.DG.Tweening.DOTween
Ease = CS.DG.Tweening.Ease
LoopType = CS.DG.Tweening.LoopType

--XiaoShi
SysDefines = CS.SysDefines
UIManager = CS.UIManager
GameController = CS.GameController
AudioManager = CS.AudioManager.Instance
ObjectPoolManager = CS.ObjectPoolManager.Instance
CoroutineController = CS.CoroutineController.Instance
UnityHelper = CS.UnityHelper
TimeHelper = CS.TimeHelper

--Global
event = require "LuaUtil/Event"
Timer = require "LuaUtil/Timer"
LuaClass = require "LuaUtil/LuaClass"
LuaBase = require "LuaUtil/LuaBase"

--Coroutine
cs_coroutine = require "Coroutine.cs_coroutine"
StartCoroutine = cs_coroutine.start
StopCoroutine = cs_coroutine.stop
StopAllCoroutine = cs_coroutine.stopAll
WaitForSeconds = function(time)
    coroutine.yield(UnityEngine.WaitForSeconds(time))
end
WaitForOneFrame = coroutine.yield

--GlobalPath
Path_Res = "Assets/Resources_HotUpdate/"
Path_UI = Path_Res .. "UIPrefab/"
Path_UI_Item = Path_Res .. "UIItem/"
Path_Audios = Path_Res .. "Audio/"
Path_Fish = Path_Res .. "Fish/"
Path_UIControl = Path_Res .. "UIControl/"
Path_Effect = "Assets/Effect/Prefabs/"

