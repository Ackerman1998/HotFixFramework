--[[
--UI容器基类
-- 1、window.view是窗口最上层的容器类
--]]
local UIBaseContainer = BaseClass("UIBaseContainer",UIBaseComponent)

local base = UIBaseComponent
-- 创建
local function OnCreate(self)
	base.OnCreate(self)
	self.components = {}
	self.length = 0
end

-- 记录Transform下所有挂载过的脚本
local function AddNewRecordIfNeeded(self, name)
	if self.components[name] == nil then
		self.components[name] = {}
	end
end
--记录控件的实例
local function RecordComponent(self, name, component_class, component)
	assert(self.components[name][component_class] == nil, "Aready exist component_class : ", component_class.__cname)
	self.components[name][component_class]=component
end

-- 添加组件
local function AddComponent(self, component_target, var_arg, ...)
	assert(component_target._ctype==ClassType.class)
	local component_class =nil
	local component_instance = nil

	if type(var_arg)=="string" then 
		component_class = component_target
		component_instance = component_class.New(self,var_arg)
		component_instance:OnCreate(...)
	else

	end
	local name = component_instance:GetName()
	self:AddNewRecordIfNeeded(name)--存放/记录挂载过的控件
	return component_instance
end

local function GetComponent(self, name, component_class)
	local components = self.components[name]
	if components == nil then
		return nil
	end
	if component_class == nil then
		return nil
	else
		return components[component_class]
	end
end

UIBaseContainer.GetComponent=GetComponent
UIBaseContainer.RecordComponent=RecordComponent
UIBaseContainer.AddNewRecordIfNeeded=AddNewRecordIfNeeded
UIBaseContainer.OnCreate=OnCreate
UIBaseContainer.AddComponent=AddComponent

return UIBaseContainer