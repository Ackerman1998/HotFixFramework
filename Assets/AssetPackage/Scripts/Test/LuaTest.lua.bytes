--[[
--LuaTest.lua
--]]
local LuaTest={}
local TestFunc1 = "TestFunc1"
local function TestVoid(index)
	Log.Print("TestVoid Run".. index)
end
local function TestFunction()
	local msg = Messenger.New()
	msg:AddListener("TestFunc1",TestVoid)
	msg:Broadcast("TestFunc1",1)
end
LuaTest.TestFunction = TestFunction
return LuaTest