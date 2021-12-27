--[[
-- SceneManager.lua
--]]
local BaseScene = BaseClass("BaseScene")

local function _init(self)
	-- 场景配置
	self.scene_config = scene_config
	-- 预加载资源：资源路径、资源类型
	self.preload_resources = {}
	-- 预加载GameObject：资源路径、实例化个数
	self.preload_prefab = {}
end
--进入
local function OnEnter(self)
end

local function _delete(self)
	self:OnDestroy()
end

--释放
local function OnDestroy(self)
	self.scene_config = nil
	self.preload_resources = nil
end
--pre load asset
--[[path:资源路径，res_type:资源类型,inst_count:实例化个数(当res_type=GameObject)]]
local function AddPreLoadAssets(self, path, res_type, inst_count)
	assert(res_type~=nil)
	assert(#res_type>0)
	if res_type == typeof(CS.UnityEngine.GameObject) then
		self.preload_prefab[path] = inst_count
	else 
		self.preload_prefab[path] = res_type
	end
end
--开始加载
local function StartLoadAssets(self)
	local res_count = table.count(self.preload_resources)
	local prefab_count = table.count(self.preload_prefab)
	local total_count = res_count + prefab_count
	if total_count <= 0 then
		return coroutine.yieldbreak()
	end
	
end

local function OnComplete(self)
    
end

local function OnClose(self)
    
end


BaseScene._init=_init
BaseScene._delete=_delete
BaseScene.OnEnter=OnEnter
BaseScene.OnDestroy=OnDestroy
BaseScene.OnComplete = OnComplete
BaseScene.OnClose = OnClose
return BaseScene

