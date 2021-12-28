--[[
--UILogin View层
--]]

local UILoginView = BaseClass("UILoginView",UIBaseView)
local base = UIBaseView
local btn_login_path = "Content/LoginBtn"
local btn_login_text_path ="Content/LoginBtn/Text"


local function OnCreate(self)
	base.OnCreate(self)
	--初始化
	self.btn_login_text = self:AddComponent(UIText,btn_login_text_path)
	self.btn_login = self:AddComponent(UIButton,btn_login_path)
	self.btn_login:SetOnClick(function()
		self.ctrl:Button_Start()
	end)

end

local function OnEnable(self)
	base.OnEnable(self)
end

local function Update(self)

end

local function OnDestroy(self)

	base.OnDestroy(self)
end

UILoginView.OnCreate = OnCreate
UILoginView.OnEnable = OnEnable
UILoginView.Update = Update
UILoginView.OnDestroy = OnDestroy
return UILoginView