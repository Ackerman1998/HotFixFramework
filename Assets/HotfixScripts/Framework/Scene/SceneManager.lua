--[[
-- SceneManager.lua
--]]
local SceneManager = BaseClass("SceneManger",Singleton)
--构造函数
local function _init(self)
	self.busing = false
end
--析构函数
local function _delete(self)

end
--切场景
local function CoInnerSwitchScene(self, scene_config)
	--打开loading页
	local uimgr_instance = UIManager:GetInstance()
	local window = uimgr_instance:OpenWindow(UIWindowNames.UILoading)
	Log.Print("Loading ui create success：")
	local window_model = window.Model
	Log.Print("Loading ui model.value："..window_model.value)
	window_model.value = 0
	coroutine.waitforseconds(1)
	window_model.value = window_model.value+0.1
	coroutine.waitforseconds(1)
	window_model.value = window_model.value+0.1
	coroutine.waitforseconds(1)
	window_model.value = window_model.value+0.1
	coroutine.waitforseconds(1)
	window_model.value = window_model.value+0.1
	
end

local function SwitchScene(self,scene_config)
	assert(scene_config ~= LaunchScene and scene_config ~= LoadingScene)
	--assert(scene_config.Type ~= nil)
	if self.busing then
		return
	end
	self.busing=true
	--coroutine.start(CoInnerSwitchScene,self,scene_config)
end
SceneManager._init=_init
SceneManager.SwitchScene=SwitchScene
SceneManager.CoInnerSwitchScene=CoInnerSwitchScene
SceneManager._delete=_delete

return SceneManager

