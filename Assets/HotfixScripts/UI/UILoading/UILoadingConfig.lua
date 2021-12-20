--[[
UILoading--Loading窗口配置
]]

local UILoading = {
	Name = UIWindowNames.UILoading,
	Layer = UILayers.TopLayer,
	Model = require "UI.UILoading.Model.UILoadingModel",
	Ctrl = nil,
	View = require "UI.UILoading.View.UILoadingView",
	PrefabPath = "UI/Prefabs/View/UILoading.prefab",
}

return UILoading