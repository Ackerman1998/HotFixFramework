--[[
-- Updatale.lua
--]]
local Updatable = BaseClass("Updatable")
-- 构造函数
local function _init(self)
	self:EnableUpdate(true)
end

-- 析构函数
local function _delete(self)
	self:EnableUpdate(false)
end

-- 是否启用更新
local function EnableUpdate(self, enable)
	
end

local function Run()
	Log.Print("Run...")
	--Log.Print("print:" .. Updatable._cname)
	Updatable.New();
end
Updatable.index =1
Updatable._init = _init
Updatable._delete = _delete
Updatable.EnableUpdate = EnableUpdate
Updatable.Run = Run

return Updatable