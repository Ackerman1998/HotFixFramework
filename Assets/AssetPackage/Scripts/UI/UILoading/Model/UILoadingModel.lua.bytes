--[[
加载页面UI Model层
]]
local UILoadingModel = BaseClass("UILoadingModel",UIBaseModel)
local base = UIBaseModel

local function _init(self)
    print("UILoadingModel inint")
end

--打开
local function OnEnable(self)
    print("UILoadingModel OnEnable")
    base.OnEnable(self)
    self.value = 0
end
-- 关闭
local function OnDisable(self)
    print("UILoadingModel OnDisable")
	base.OnDisable(self)
	self.value = 0
end

UILoadingModel.OnEnable = OnEnable
UILoadingModel.OnDisable = OnDisable
UILoadingModel._init = _init
return UILoadingModel