--[[
-- coroutine 拓展
--]]
--coroutine池子
local coroutine_pool={}

local action_map = {}

local action_pool = {}

local function _RecycleCoroutine(cor)
	if not coroutine.status(cor) == "suspended" then
		error("recycle coroutine failed , coroutine not suspended...")
	end
	table.insert(coroutine_pool,cor)
end

local function _GenCoroutine(callback,...)
	local args = SafePack(...)
	if callback~=nil then
		local ret = SafePack(callback(SafeUnpack(args)))
		_RecycleCoroutine(coroutine.running())
	 	args = SafePack(coroutine.yield())
	end
end

--获取
local function _GetCoroutine()
	local co =nil
	if #coroutine_pool>0 then
		co = table.remove(coroutine_pool)
	else
		co = coroutine.create(_GenCoroutine)
	end
	return co
end

local function __RecycleAction(action)
	action.co = false
	action.timer = false
	action.func = false
	action.args = false
	action.result = false
	table.insert(action_pool, action)
end

local function __GetAction(co, timer, func, args, result)
	local action = nil
	if table.length(action_pool) > 0 then
		action = table.remove(action_pool)
	else
		action = {false, false, false, false, false}
	end
	action.co = co and co or false
	action.timer = timer and timer or false
	action.func = func and func or false
	action.args = args and args or false
	action.result = result and result or false
	return action
end

--继续
local function _ResumeCoroutine(co,callback,...)
	coroutine.resume(co,callback,...)
end

local function __Action(action, abort, ...)
	assert(action.timer)
	if not action.func then
		abort = true
	end

	if abort then
		action.timer:Stop()
		action_map[action.co]=nil
		_ResumeCoroutine(action.co,...)
		__RecycleAction(action)
	end
end



-------------------------------外部调用方法--------------------------------------

--等待times秒 类似于 yield return new WaitingForSeconds(1f)
local function waitforseconds(times)
	local co = coroutine.running()
	local timer = TimeManager:GetInstance():GetCoTimer()
	local action =__GetAction(co,timer)
	timer:Init(times, __Action, action, true)
	timer:Start()
	action_map[co] = action
	return coroutine.yield()
end
--等frameNum帧
local function waitforframes(frameNum)
	local co = coroutine.running()
	local timer = TimeManager:GetInstance():GetCoTimer()
	local action =__GetAction(co,timer)
	timer:Init(frameNum, __Action, action, true,true)
	timer:Start()
	action_map[co] = action
	return coroutine.yield()
end

local function start(callback,...)
	local co = _GetCoroutine()
	print(co)
	_ResumeCoroutine(co,callback)
	return co
end


--Log.Print("resume2:"..coroutine.resume(cor,200))

coroutine.waitforseconds = waitforseconds
coroutine.waitforframes = waitforframes
coroutine.start = start

