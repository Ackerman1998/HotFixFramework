--[[
-- 单例
-- Singleton.lua
--]]
local Singleton = BaseClass("Singleton")
local function _init(self)
    assert(rawget(self._class_type,"Instance")==nil,self._class_type._cname.." to create singleton twice!")
	rawset(self._class_type,"Instance",self)
end

local function _delete(self)
	Log.Print("Singleton _delete run...")
end
--获取实例
local function GetInstance(self)
	if rawget(self,"Instance")==nil then
		rawset(self,"Instance",self.New())
	end
	assert(self.Instance~=nil)
	return self.Instance
end


Singleton._init=_init
Singleton._delete=_delete
Singleton.GetInstance=GetInstance

return Singleton