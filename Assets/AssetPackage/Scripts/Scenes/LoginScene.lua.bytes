--[[
-- LoginScene
--]]
local LoginScene = BaseClass("LoginScene",BaseScene)

local function OnComplete(self)
    print("open login ui success111...")
    UIManager:GetInstance():OpenWindow(UIWindowNames.UILogin)
    print("open login ui success333...")
end

local function OnClose(self)
    UIManager:GetInstance():CloseWindow(UIWindowNames.UILogin)
end
LoginScene.OnComplete = OnComplete
LoginScene.OnClose = OnClose
return LoginScene
