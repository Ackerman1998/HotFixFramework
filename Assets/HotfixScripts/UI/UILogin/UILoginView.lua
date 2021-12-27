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
	self.btn_login_text:SetText("开始")
	self.btn_login = self:AddComponent(UIButton,btn_login_path)
	print("click 1111...")
	self.btn_login:SetOnClick(function()
		print("click button ...")
	end)
	print("click 2222...")

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