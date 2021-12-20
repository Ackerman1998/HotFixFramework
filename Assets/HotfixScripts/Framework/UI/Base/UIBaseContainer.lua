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

-- 如果必要，创建新的记录，对应Unity下一个Transform下所有挂载脚本的记录表
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
-- 1、直接添加Lua侧组件：inst:AddComponent(ComponentTypeClass, luaComponentInst)
-- 2、指定Lua侧组件类型和必要参数，新建组件并添加，多种重载方式：
--    A）inst:AddComponent(ComponentTypeClass, relative_path)
--    B）inst:AddComponent(ComponentTypeClass, child_index)
--    C）inst:AddComponent(ComponentTypeClass, unity_gameObject)
local function AddComponent(self, component_target, var_arg, ...)
	assert(component_target._ctype==ClassType.class)
	local component_class =nil
	local component_instance = nil
	if type(var_arg)=="string" then 
		component_class = component_target
		component_instance = component_class:New(self,var_arg)
		component_instance:OnCreate(...)
	else

	end
	local name = component_instance:GetName()
	AddNewRecordIfNeeded(name)--存放/记录挂载过的控件

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

return UIBaseContainer