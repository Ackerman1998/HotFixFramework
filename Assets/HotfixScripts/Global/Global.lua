--[[Lua全局文件加载]]
Log = require "Tools.Log"
require "Framework.Common.BaseClass"
require "Framework.Common.ConstClass"
require "Framework.Common.DataClass"
require "TableUtil"
Vector2 = require "Vector2"
Mathf = require "Mathf"
Vector3 = require "Vector3"
SortingLayerNames = require "SortingLayerNames"
require "Object"
Singleton = require "Framework.Common.Singleton"
SceneConfig = require "SceneConfig"
ResourcesManager = require "ResourcesManager"
GameObjectPool = require "GameObjectPool"
UILayers = require "UILayers"

UIWindow = require "UIWindow"
UIUtil = require "UIUtil"
UIBaseComponent = require "UIBaseComponent"
UIText =require "UIText"
UISlider = require "UISlider"
UICanvas = require "UICanvas"
UIBaseContainer = require "UIBaseContainer"
UIBaseView = require "UIBaseView"
UILayer = require "UILayer"
SceneManager = require "SceneManager"
UIWindowNames = require "UI.UIWindowNames"
Messenger = require "Framework.Common.Messenger"
Updatable = require "Framework.Common.Updatable"
UpdateManager = require "Framework.Updater.UpdateManager"
UIConfig = require "UIConfig"

UIManager=require "UIManager"

require "Tools.LuaUtil"
LuaTest = require "Test.LuaTest"
Coroutine = require "Framework.Updater.Coroutine"

