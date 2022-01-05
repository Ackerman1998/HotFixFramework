--[[
UILoginConfig--Login窗口配置
]]

local UILoginConfig = {
	Name = UIWindowNames.UILoginServer,
	Layer = UILayers.NormalLayer,
	Model = require "UILoginServerModel",
	Ctrl = require "UILoginServerCtrl",
	View = require "UILoginServerView",
	PrefabPath = "UILoginServer",
}
return UILoginConfig