--[[
加载页面UI Model层
]]
local UILoginModel = BaseClass("UILoginModel")
local base = UILoginModel

local function _init(self)
end

--打开
local function OnEnable(self)
    base.OnEnable(self)
  
end
-- 关闭
local function OnDisable(self)
	base.OnDisable(self)
	
end

UILoginModel.OnEnable = OnEnable
UILoginModel.OnDisable = OnDisable
UILoginModel._init = _init
return UILoginModel