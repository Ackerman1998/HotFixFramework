--[[
--UILogin View层
--]]

local UILoginServerView = BaseClass("UILoginServerView",UIBaseView)
local base = UIBaseView
local back_btn = "MaskContent/Bg/BackBtn"
local ok_btn = "MaskContent/Bg/OkBtn"

local function OnCreate(self)
	base.OnCreate(self)
	self.back_button = self:AddComponent(UIButton,back_btn)
	self.ok_button = self:AddComponent(UIButton,ok_btn)
	self.back_button:SetOnClick(function()
		self.ctrl:ButtonClose()
	end)
	self.ok_button:SetOnClick(function()
		self.ctrl:ButtonClose()
	end)
end

local function OnEnable(self)
	base.OnEnable(self)
end

local function Update(self)

end

local function OnDestroy(self)
	self.back_button=nil
	self.ok_button=nil
	base.OnDestroy(self)
end

UILoginServerView.OnCreate = OnCreate
UILoginServerView.OnEnable = OnEnable
UILoginServerView.Update = Update
UILoginServerView.OnDestroy = OnDestroy
return UILoginServerView