--[[
加载页面UI Model层
]]
local UILoginModel = BaseClass("UILoginModel",UIBaseModel)
local base = UIBaseModel

local function _init(self)
    self.appversion_code = nil
    self.resversion_code=nil
end

--打开
local function OnEnable(self)
    base.OnEnable(self)
    --读取客户端AppVersionCode
    self.appversion_code = ResourcesManager:LoadAppVersionCode()
    --读取客户端ResVersionCode
end
-- 关闭
local function OnDisable(self)
	base.OnDisable(self)
	
end

UILoginModel.OnEnable = OnEnable
UILoginModel.OnDisable = OnDisable
UILoginModel._init = _init
return UILoginModel