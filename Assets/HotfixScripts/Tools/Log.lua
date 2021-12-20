--[[
-- 游戏Log工具
--]]
Log = {}
local function Print(msg)
	CS.UnityEngine.Debug.Log(msg)
end
local function PrintError(msg)
	CS.UnityEngine.Debug.LogError(msg)
end
Log.Print = Print
Log.PrintError = PrintError
return Log