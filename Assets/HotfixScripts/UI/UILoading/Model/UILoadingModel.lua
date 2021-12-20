--[[
加载页面UI Model层
]]
local UILoadingModel = BaseClass("UILoadingModel",UIBaseModel)
local base = UIBaseModel
--打开
local function OnEnable(self)
    base.OnEnable(self)
    self.value = 0
end
-- 关闭
local function OnDisable(self)
	base.OnDisable(self)
	self.value = 0
end

UILoadingModel.OnEnable = OnEnable
UILoadingModel.OnDisable = OnDisable

return UILoadingModel