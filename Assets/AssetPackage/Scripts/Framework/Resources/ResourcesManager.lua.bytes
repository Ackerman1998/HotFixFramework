--[[
-- 资源管理系统
--]]

local ResourcesManager = BaseClass("ResourcesManager", Singleton)
local AssetBundleManager = CS.AssetBundles.AssetBundleManager.Instance
local AssetBundleUtility = CS.AssetBundles.AssetBundleUtility

-- 是否有加载任务正在进行
local function IsProsessRunning(self)
	return AssetBundleManager.IsProsessRunning
end

-- 设置常驻包
-- 注意：
-- 1、公共包（被2个或者2个其它AB包所依赖的包）底层会自动设置常驻包
-- 2、任何情况下不想被卸载的非公共包（如Lua脚本）需要手动设置常驻包
local function SetAssetBundleResident(self, path, resident)
	local assetbundleName = AssetBundleUtility.AssetBundlePathToAssetBundleName(path)
	resident = resident and true or false
	AssetBundleManager:SetAssetBundleResident(assetbundleName, resident)
end

-- 异步加载AssetBundle：回调形式
local function LoadAssetBundleAsync(self, path, callback, ...)
	assert(path ~= nil and type(path) == "string" and #path > 0, "path err : "..path)
	assert(callback ~= nil and type(callback) == "function", "Need to provide a function as callback")
	local args = SafePack(...)
	coroutine.start(function()
		local assetbundle = self:CoLoadAssetBundleAsync(path, nil)
		callback(SafeUnpack(args))
	end)
end

-- 异步加载AssetBundle：协程形式
local function CoLoadAssetBundleAsync(self, path, progress_callback)
	assert(path ~= nil and type(path) == "string" and #path > 0, "path err : "..path)
	local assetbundleName = AssetBundleUtility.AssetBundlePathToAssetBundleName(path)
	local loader = AssetBundleManager:LoadAssetBundleAsync(assetbundleName)
	coroutine.waitforasyncop(loader, progress_callback)
    loader:Dispose()
end

-- 异步加载Asset：回调形式
local function LoadAsync(self, path, res_type, callback, ...)
	assert(path ~= nil and type(path) == "string" and #path > 0, "path err : "..path)
	assert(callback ~= nil and type(callback) == "function", "Need to provide a function as callback")
	local args = SafePack(nil, ...)
	print("Resources.LoadAsync start:")
    coroutine.start(function()
		print("Resources.LoadAsync:"..path)
		local asset = self:CoLoadAsync(path, res_type, nil)
		args[1] = asset
		callback(SafeUnpack(args))
		--print("Resources.LoadAsync ok:"..path)
	end)
end

-- 异步加载Asset
local function CoLoadAsync(self, path, res_type, progress_callback)
	assert(path ~= nil and type(path) == "string" and #path > 0, "path err : "..path)
	print("CoLoadAsync LoadAssetAsync:"..path)
	local loader = CS.LuaCallCsharpFunc.LoadAssetAsync(path)
	coroutine.waitforasyncop(loader,progress_callback)
	local asset = loader.asset
	loader:Dispose()
	print("loader is dispose,Resources.LoadAsync ok:"..path)
	--local asset = CS.LuaCallCsharpFunc.GetAsset(path)
	return asset
end
--加载本地Text文件中的字符串(例如：Appversion,ResVersion)
local function LoadTextForLocalFile(self,fileName)
	print("start load text local :"..fileName)
	local appversion_code = nil
	local loader =  CS.LuaCallCsharpFunc.GetResourceAsync(fileName)
	coroutine.waitforasyncop(loader,nil)
	appversion_code = loader.text
	loader:Dispose()
	print("start load text local ok:"..fileName)
	return appversion_code
end

-- 清理资源：切换场景时调用
local function Cleanup(self)
	AssetBundleManager:ClearAssetsCache()
	AssetBundleManager:UnloadAllUnusedResidentAssetBundles()
	
	-- TODO：Lua脚本要重新加载，暂时吧，后面缓缓策略
	local luaAssetbundleName = CS.XLuaManager.Instance.AssetbundleName
	AssetBundleManager:AddAssetbundleAssetsCache(luaAssetbundleName)
end

ResourcesManager.IsProsessRunning = IsProsessRunning
ResourcesManager.SetAssetBundleResident = SetAssetBundleResident
ResourcesManager.LoadAssetBundleAsync = LoadAssetBundleAsync
ResourcesManager.CoLoadAssetBundleAsync = CoLoadAssetBundleAsync
ResourcesManager.LoadAsync = LoadAsync
ResourcesManager.CoLoadAsync = CoLoadAsync
ResourcesManager.Cleanup = Cleanup
ResourcesManager.LoadTextForLocalFile = LoadTextForLocalFile
return ResourcesManager
