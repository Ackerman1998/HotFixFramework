--[[
-- LoginScene
--]]
local LoginScene = BaseClass("LoginScene",BaseScene)

local function OnComplete(self)
    UIManager:GetInstance():OpenWindow(UIWindowNames.UILogin)
end

local function OnClose(self)
    UIManager:GetInstance():CloseWindow(UIWindowNames.UILogin)
end
LoginScene.OnComplete = OnComplete
LoginScene.OnClose = OnClose
return LoginScene
