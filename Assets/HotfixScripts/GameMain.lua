--[[
--GameMain.lua 游戏Lua启动入口
--]]
GameMain={}
require "Global.Global"--加载全局
function foo(...)  
	print("dd " .. select('#', ...))
	for i = 1, select('#', ...) do 
		local arg = select(i, ...); 
		--print("arg", arg);  
	end  
end  
--测试协程方法
local function TestCoroutine()
	local co = coroutine.start(function(index)
		Log.Print("Test Coroutine... index:"..index)
		return 1,2
	end,1)
	print(coroutine.status(co))
	coroutine.resume(co,1)
	print(coroutine.status(co))
end

local function Initilize()
	--在这里加载ui ab包
end

local function EnterGame()
	SceneManager:GetInstance():SwitchScene(SceneConfig.LoginScene)
end


local function TestDeepCopy()
	local nums = 100
	local testnum ={}
	-- print(type(testnum))
    -- testnum = DeepCopy(nums)
	-- print(testnum)
	-- print(type(testnum))
	local a= {
		{b=1},
		{c=3},
		print("tst"),
		{x=1},
		cc=1,
		{y=1},
	}
	print("type(a[1])"..type(a[1]))
	print("type(a[3])"..type(a[3]))
	print("**********************************")
	for k,v in pairs(a) do
		print("key:" .. k )
	end
	print("**********************************")
	print(testnum[nums] == nil)
	-- if testnum[nums] then
	-- 	print("true...")
	-- else
	-- 	print("false...")
	-- end
end

local function Start()
	Log.Print("GameMain Start...")
	--update_mgr = UpdateManager:New()
	--update_mgr_get = UpdateManager:GetInstance()
	--update_mgr:StartUp()
	--update_mgr_get:StartUp()
	--Log.Print("update_mgr==update_mgr_get:"..update_mgr==update_mgr_get)
	--update_mgr2 = UpdateManager:New()
    --UpdateManager._init()
	UpdateManager:GetInstance():StartUp()
	UIManager:GetInstance():StartUp()
	--foo(1,2,3,4)
	--TestCoroutine()
	coroutine.start(function()
		Log.Print("******************************Enter Game ...")
		EnterGame()
	end)
	
	--LuaTest.TestFunction()
	--TestDeepCopy()
end



GameMain.Start=Start