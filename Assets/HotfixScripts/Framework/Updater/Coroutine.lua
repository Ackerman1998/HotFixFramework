--[[
-- coroutine
-- 协程拓展 
--]]
--coroutine池子
local coroutine_pool={}
local action_map = {}
local action_pool = {}
--回收
local function _RecycleCoroutine(cor)
	if not coroutine.status(cor) == "suspended" then
		error("recycle coroutine failed , coroutine not suspended...")
	end
	table.insert(coroutine_pool,cor)
end
--构造
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
		print("get coroutine from copool"..#coroutine_pool)
		co = table.remove(coroutine_pool)
	else
		print("get coroutine from create new co")
		co = coroutine.create(_GenCoroutine)
	end
	return co
end
--回收action
local function __RecycleAction(action)
	action.co = false
	action.timer = false
	action.func = false
	action.args = false
	action.result = false
	table.insert(action_pool, action)
end
--生成action实例(参数包含协程，计时器，回调等)
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
--local function _ResumeCoroutine(co,func,...)
local function _ResumeCoroutine(co,callback,...)
	print("coroutine.resume run...+")
	coroutine.resume(co,callback,...)
	-- local resume_ret = nil
	-- if func ~= nil then
	-- 	resume_ret = SafePack(coroutine.resume(co, func, ...))
	-- else
	-- 	resume_ret = SafePack(coroutine.resume(co, ...))
	-- end
	-- local flag, msg = resume_ret[1], resume_ret[2]
	-- if not flag then
	-- 	print(msg.."\n"..debug.traceback(co))
	-- elseif resume_ret.n > 1 then
	-- 	table.remove(resume_ret, 1)
	-- else
	-- 	resume_ret = nil
	-- end

	-- return flag, resume_ret
end
--结束时执行的方法
local function __Action(action, abort, ...)
	assert(action.timer)
	if not action.func then
		abort = true
	end

	--这里做Timer.shotone=false的判断
	if  not abort and action.func then
		if action.args and #action.args>0 then
			abort = (action.func(SafeUnpack(action.args))==action.result)
		else
			abort = (action.func()==action.result)
		end
	end

	if abort then
		action.timer:Stop()
		action_map[action.co]=nil
		_ResumeCoroutine(action.co,...)
		__RecycleAction(action)
	end
end

local function _CheckAsyncOperation(co,asyncOperation,callback)
	if callback~=nil then
		callback(co,asyncOperation.progress)
	end
	return asyncOperation.isDone
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
--等待异步完成，执行回调
local function waitforasyncop(asyncOperation,callback)
	assert(asyncOperation)
	local co = coroutine.running() or error ("coroutine.waitforasyncop must be run in coroutine")
	local timer = TimeManager:GetInstance():GetCoTimer()
	local action =__GetAction(co,timer,_CheckAsyncOperation,SafePack(co,asyncOperation,callback),true)
	timer:Init(1, __Action, action, false,true)
	timer:Start()
	action_map[co] = action
	return coroutine.yield()
end

local function yieldstart(func,callback)
	
end

local function start(callback,...)
	print("coroutine.start start run...+")
	local co = _GetCoroutine()
	_ResumeCoroutine(co,callback,...)
	return co
end

-------------------------------外部调用方法--------------------------------------

coroutine.waitforasyncop = waitforasyncop
coroutine.waitforseconds = waitforseconds
coroutine.waitforframes = waitforframes
coroutine.start = start

