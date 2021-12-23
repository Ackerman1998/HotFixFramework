--[[
-- coroutine 拓展
--]]
--coroutine池子
local coroutine_pool={}

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
		Log.Print("callback run over , 111")
		_RecycleCoroutine(coroutine.running())
	 	args = SafePack(coroutine.yield())
	end
	Log.Print("callback run over , 222")
end

--获取
local function _GetCoroutine()
	--先不做缓存
	local co =nil
	co = coroutine.create(_GenCoroutine)
	return co
end
--继续
local function _ResumeCoroutine(co,callback,...)
	coroutine.resume(co,callback,...)
end

--等待times秒 类似于 yield return new WaitingForSeconds(1f)
local function waitforseconds(times)
	--local co = coroutine.running()
end

local function start(callback,...)
	local co = _GetCoroutine()
	_ResumeCoroutine(co,callback)
	return co
end


--Log.Print("resume2:"..coroutine.resume(cor,200))

coroutine.waitforseconds = waitforseconds
coroutine.start = start

