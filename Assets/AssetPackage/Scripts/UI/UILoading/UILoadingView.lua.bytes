--[[
--UILoading View层
--]]

local UILoadingView = BaseClass("UILoadingView",UIBaseView)
local base = UIBaseView
local loading_text_path = "Content/LoadText"
local loading_slider_path = "Content/GameSlider"

local function OnCreate(self)
	base.OnCreate(self)
	-- 初始化各个组件
	self.loading_text = self:AddComponent(UIText, loading_text_path)
	self.loading_text:SetText("Loading...")
	self.loading_slider = self:AddComponent(UISlider, loading_slider_path)
	self.loading_slider:SetValue(0.0)
	self.testvalue = 111
	
end

local function OnEnable(self)
	base.OnEnable(self)
end

local function Update(self)
	--print("testvalue:"..self.testvalue)
	--刷新Loading页的进度条
	self.loading_slider:SetValue(self.model.value)
end

local function OnDestroy(self)
	base.OnDestroy(self)
end

UILoadingView.OnCreate = OnCreate
UILoadingView.OnEnable = OnEnable
UILoadingView.Update = Update
UILoadingView.OnDestroy = OnDestroy

return UILoadingView