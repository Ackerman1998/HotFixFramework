--[[
--UILogin View层
--]]

local UILoginView = BaseClass("UILoginView",UIBaseView)
local base = UIBaseView
local btn_login_path = "Content/LoginBtn"
local btn_login_text_path ="Content/LoginBtn/Text"
local btn_loginServer_path = "Content/SelectBtn"
local appversion_text_path = "BgContent/AppVersion"
local resversion_text_path = "BgContent/ResVersion"
local function OnCreate(self)
	base.OnCreate(self)
	--初始化
	self.btn_login_text = self:AddComponent(UIText,btn_login_text_path)
	self.btn_login_text:SetText("登陆")
	self.btn_login = self:AddComponent(UIButton,btn_login_path)
	self.btn_login:SetOnClick(function()
		self.ctrl:Button_Start()
	end)
	self.btn_selectServer = self:AddComponent(UIButton,btn_loginServer_path)
	self.btn_selectServer:SetOnClick(function()
		self.ctrl:Button_SelectServer()
	end)
	self.text_appversioncode = self:AddComponent(UIText,appversion_text_path)
	self.text_appversioncode:SetText("AppVersion："..self.model.appversion_code)

	self.text_resversioncode = self:AddComponent(UIText,resversion_text_path)
	self.text_resversioncode:SetText("ResVersion："..self.model.resversion_code)
end

local function OnEnable(self)
	base.OnEnable(self)
end

local function Update(self)

end

local function OnDestroy(self)
	self.btn_login_text=nil
	self.btn_login=nil
	self.btn_selectServer=nil
	self.text_appversioncode=nil
	self.text_resversioncode=nil
	base.OnDestroy(self)
end

UILoginView.OnCreate = OnCreate
UILoginView.OnEnable = OnEnable
UILoginView.Update = Update
UILoginView.OnDestroy = OnDestroy
return UILoginView