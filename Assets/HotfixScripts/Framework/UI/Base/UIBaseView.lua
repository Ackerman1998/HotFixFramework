--[[
--UIBaseView - ui view层基类
]]
local UIBaseView = BaseClass("UIBaseView",UIBaseContainer)

local function _init(self,holder,var_arg,model,ctrl)
    assert(model~=nil)
    assert(ctrl~=nil)
    self.ctrl = ctrl
    self.model = model
    	-- 窗口画布
	self.canvas = nil
	-- 窗口基础order，窗口内添加的其它canvas设置的order都以它做偏移
	self.base_order = 0
end

-- 创建：资源加载完毕
local function OnCreate(self)
	base.OnCreate(self)
	-- 窗口画布
	self.canvas = self:AddComponent(UICanvas, "", 0)
	-- 回调管理，使其最长保持和View等同的生命周期
	self.__ui_callback = {}
	-- 初始化RectTransform
	self.rectTransform.offsetMax = Vector2.zero
	self.rectTransform.offsetMin = Vector2.zero
	self.rectTransform.localScale = Vector3.one
	self.rectTransform.localPosition = Vector3.zero
end

-- 打开：窗口显示
local function OnEnable(self)
	self.base_order = self.holder:PopWindowOrder()
	base.OnEnable(self)
	self:OnAddListener()
end

-- 注册消息
local function OnAddListener(self)
end

-- 注销消息
local function OnRemoveListener(self)
end

return UIBaseView