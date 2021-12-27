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

BaseScene._init=_init
BaseScene._delete=_delete
BaseScene.OnEnter=OnEnter
BaseScene.OnDestroy=OnDestroy

return BaseScene

