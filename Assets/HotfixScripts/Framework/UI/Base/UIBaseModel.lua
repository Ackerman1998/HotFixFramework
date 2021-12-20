--[[
UI Model基类    
]]

local UIBaseModel = BaseClass("UIBaseModel")

local function _init(self,ui_name)
    self._ui_callback = {}
	self._data_callback = {}
	self._ui_name = ui_name
	self:OnCreate()
end
--析构函数
local function _delete(self)
    self:OnDestroy()

    self._ui_callback = nil
	self._data_callback = nil
	self._ui_name = nil
end

-- 创建：变量定义，初始化，消息注册
-- 注意：窗口生命周期内保持的成员变量放这
local function OnCreate(self)
end

-- 打开：刷新数据模型
-- 注意：窗口关闭时可以清理的成员变量放这
local function OnEnable(self, ...)
end

-- 关闭
-- 注意：必须清理OnEnable中声明的变量
local function OnDisable(self)
end

-- 销毁
-- 注意：必须清理OnCreate中声明的变量
local function OnDestroy(self)
end

-- 注册消息
local function OnAddListener(self)
end

-- 注销消息
local function OnRemoveListener(self)
end
--激活
local function Activate(self,...)
    self:OnAddListener()
    self:OnEnable(...)
end
--关闭
local function Deactivate(self)
	self:OnRemoveListener()
	self:OnDisable()
end

UIBaseModel._init =_init
UIBaseModel._delete = _delete
UIBaseModel.OnCreate = OnCreate
UIBaseModel.OnEnable = OnEnable
UIBaseModel.OnDisable = OnDisable
UIBaseModel.OnDestroy = OnDestroy
UIBaseModel.OnAddListener = OnAddListener
UIBaseModel.OnRemoveListener = OnRemoveListener
return UIBaseModel