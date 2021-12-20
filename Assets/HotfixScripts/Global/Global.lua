--[[Lua全局文件加载]]
Log = require "Tools.Log"
require "Framework.Common.BaseClass"
require "Framework.Common.ConstClass"
require "Framework.Common.DataClass"
require "TableUtil"
SortingLayerNames = require "SortingLayerNames"
require "Object"

UILayers = require "UILayers"
UIUtil = require "UIUtil"
UIBaseComponent = require "UIBaseComponent"
UILayer = require "UILayer"
SceneManager = require "SceneManager"
UIWindowNames = require "UI.UIWindowNames"
Messenger = require "Framework.Common.Messenger"
Updatable = require "Framework.Common.Updatable"
Singleton = require "Framework.Common.Singleton"
UpdateManager = require "Framework.Updater.UpdateManager"
UIManager=require "UIManager"
require "Tools.LuaUtil"
LuaTest = require "Test.LuaTest"
Coroutine = require "Framework.Updater.Coroutine"

