--[[
--UI配置表
--]]
local UIModule = {
	-- 模块 = 模块配置表
	--UILogin = require "UI.UILogin.UILoginConfig",
	UILoading = require "UILoadingConfig",
	--UINoticeTip = require "UI.UINoticeTip.UINoticeTipConfig",
	--UITestMain = require "UI.UITestMain.UITestMainConfig",
	--UIBattle = require "UI.UIBattle.UIBattleConfig",
}

local UIConfig={}
--for _,ui_module in pairs(UIModule) do 
	for _,ui_config in pairs(UIModule) do
		local ui_name = ui_config.Name
		Log.Print("ui_name："..ui_name)
        assert(UIConfig.ui_name == nil, "Already exsits : "..ui_name)
		if ui_config.View then
			assert(ui_config.PrefabPath ~= nil and #ui_config.PrefabPath > 0, ui_name.." PrefabPath empty.")
		end
        UIConfig[ui_name]=ui_config
	end
--end


return ConstClass("UIConfig",UIConfig)
