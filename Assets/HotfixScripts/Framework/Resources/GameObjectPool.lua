--[[
--GameObjectPool : 游戏物体池子(缓存)
--]]
local GameObjectPool = BaseClass("GameObjectPool",Singleton)
local __cacheTransRoot = nil
local __goPool = {}
local __instCache = {}
local function _init(self)
    local gameObjectRoot = CS.UnityEngine.GameObject.Find("GameObjectCacheRoot")
    if gameObjectRoot==nil then
        gameObjectRoot = CS.UnityEngine.GameObject("GameObjectCacheRoot")
        CS.UnityEngine.Object.DontDestroyOnLoad(gameObjectRoot)
    end
    __cacheTransRoot=gameObjectRoot.transform
end
--从缓存池子中取
local function TryGetFromCache(self, path)
	if not self:CheckHasCached(path) then
		return nil
	end
	
	local cachedInst = __instCache[path]
	if cachedInst ~= nil and table.length(cachedInst) > 0 then
		local inst = table.remove(cachedInst)
		assert(not IsNull(inst), "Something wrong, there gameObject instance in cache is null!")
		return inst
	end
	
	local pooledGo = __goPool[path]
	if not IsNull(pooledGo) then
		local inst = CS.UnityEngine.GameObject.Instantiate(pooledGo)
		return inst
	end
	
	return nil
end

-- 激活GameObject
local function InitInst(obj)
	if not IsNull(obj) then
		obj:SetActive(true)
	end
end

local function CheckHasCached(self, path)
    assert(path ~= nil and type(path) == "string" and #path > 0, "path err : "..path)
	--assert(string.endswith(path, ".prefab", true), "GameObject must be prefab : "..path)
	
	local cachedInst = __instCache[path]
	if cachedInst ~= nil and table.length(cachedInst) > 0 then
		return true
	end
	
	local pooledGo = __goPool[path]
	return not IsNull(pooledGo)
end

-- 缓存并实例化GameObject
local function CacheAndInstGameObject(self, path, go, inst_count)
	assert(not IsNull(go))
	assert(inst_count == nil or type(inst_count) == "number" and inst_count >= 0)
	
	__goPool[path] = go
	if inst_count ~= nil and inst_count > 0 then
		local cachedInst = __instCache[path] or {}
		for i = 1, inst_count do
			local inst = CS.UnityEngine.GameObject.Instantiate(go)
			inst.transform:SetParent(__cacheTransRoot)
			inst:SetActive(false)
			table.insert(cachedInst, inst)
		end
		__instCache[path] = cachedInst
	end
end

local function PreLoadGameObjectAsync(self, path, inst_count, callback, ...)
    assert(inst_count == nil or type(inst_count) == "number" and inst_count >= 0)
	if self:CheckHasCached(path) then
		if callback then
			callback(...)
		end
		return
	end

    local args = SafePack(...)
    ResourcesManager:GetInstance():LoadAsync(path,typeof(CS.UnityEngine.GameObject),function(go)
        if not IsNull(go) then
			CacheAndInstGameObject(self, path, go, inst_count)
		end
		
		if callback then
			callback(SafeUnpack(args))
		end
    end)
end

--异步获取游戏物体
local function GetGameObjectAsync(self, path, callback, ...)
    local go = TryGetFromCache(self, path)
    if not IsNull(go) then
        InitInst(go)
        callback(go,...)
		return
    end

    PreLoadGameObjectAsync(self,path,1,function(callback, ...)
        local go = TryGetFromCache(self, path)
        InitInst(go)
        callback(go,...)
    end, callback, ...)
end

-- 回收
local function RecycleGameObject(self, path, inst)
	assert(not IsNull(inst))
	assert(path ~= nil and type(path) == "string" and #path > 0, "path err : "..path)
	--assert(string.endswith(path, ".prefab", true), "GameObject must be prefab : "..path)
	
	inst.transform:SetParent(__cacheTransRoot)
	inst:SetActive(false)
	local cachedInst = __instCache[path] or {}
	table.insert(cachedInst, inst)
	__instCache[path] = cachedInst
end

-- 清理缓存
local function Cleanup(self, includePooledGo)
	for _,cachedInst in pairs(__instCache) do
		for _,inst in pairs(cachedInst) do
			if not IsNull(inst) then
				CS.UnityEngine.GameObject.Destroy(inst)
			end
		end
	end
	__instCache = {}
	
	if includePooledGo then
		__goPool = {}
	end
end

GameObjectPool._init = _init
GameObjectPool.CheckHasCached = CheckHasCached
GameObjectPool.TryGetFromCache = TryGetFromCache
GameObjectPool.PreLoadGameObjectAsync = PreLoadGameObjectAsync
GameObjectPool.CoPreLoadGameObjectAsync = CoPreLoadGameObjectAsync
GameObjectPool.GetGameObjectAsync = GetGameObjectAsync
GameObjectPool.CoGetGameObjectAsync = CoGetGameObjectAsync
GameObjectPool.RecycleGameObject = RecycleGameObject
GameObjectPool.Cleanup = Cleanup
return GameObjectPool