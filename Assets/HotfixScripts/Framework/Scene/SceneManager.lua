--[[
-- SceneManager.lua
--]]
local SceneManager = BaseClass("SceneManger",Singleton)
--构造函数
local function _init(self)
	--当前所处的场景
	self.current_scene = nil
	--是否繁忙
	self.busing = false
	--存放场景容器
	self.scenes = {}
end
--析构函数
local function _delete(self)

end
--切场景
local function CoInnerSwitchScene(self, scene_config)
	--打开loading页
	local uimgr_instance = UIManager:GetInstance()
	local window = uimgr_instance:OpenWindow(UIWindowNames.UILoading)
	
	local window_model = window.Model
	window_model.value = 0
	coroutine.waitforframes(1)
	--if self.current_scene~=nil then
		--clear current scene
	--end
	
	coroutine.waitforframes(1)
	window_model.value = window_model.value+0.1

	--clear ui layer
	uimgr_instance:DestroyWindowExceptLayer(UILayers.TopLayer)
	coroutine.waitforframes(1)
	window_model.value = window_model.value+0.1
	--clear gameobjectpool
	GameObjectPool:GetInstance():Cleanup(true)
	coroutine.waitforframes(1)
	window_model.value = window_model.value + 0.1
	--enter loading scene
	local sceneMgr = CS.UnityEngine.SceneManagement.SceneManager
	sceneMgr.LoadScene(SceneConfig.LoadingScene.Level)
	coroutine.waitforframes(1)
	window_model.value = window_model.value + 0.1
	--clear gc
	collectgarbage("collect")
	CS.System.GC.Collect()
	collectgarbage("collect")
	CS.System.GC.Collect()
	--init scene
	local login_scene =self.scenes[scene_config.Name]
	if login_scene==nil then
		login_scene = scene_config.Type.New(scene_config)
		self.scenes[scene_config.Name] = login_scene
	end
	coroutine.waitforframes(1)
	window_model.value = window_model.value + 0.1
	--async load scene
	local model_CurrentValue = window_model.value
	coroutine.waitforasyncop(sceneMgr.LoadSceneAsync(scene_config.Level),function(co,progress)
		window_model.value = model_CurrentValue + 0.2*progress
	end)
	
	window_model.value = window_model.value + 0.1
	coroutine.waitforseconds(0.5)
	--load login ui
	window_model.value = 1
	coroutine.waitforseconds(0.1)
	login_scene:OnComplete()
	uimgr_instance:CloseWindow(UIWindowNames.UILoading)
end

local function SwitchScene(self,scene_config)
	assert(scene_config ~= LaunchScene and scene_config ~= LoadingScene)
	--assert(scene_config.Type ~= nil)
	if self.busing then
		return
	end
	self.busing=true
	coroutine.start(CoInnerSwitchScene,self,scene_config)
end
SceneManager._init=_init
SceneManager.SwitchScene=SwitchScene
SceneManager.CoInnerSwitchScene=CoInnerSwitchScene
SceneManager._delete=_delete

return SceneManager

