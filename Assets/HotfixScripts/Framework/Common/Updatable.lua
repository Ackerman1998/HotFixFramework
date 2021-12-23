--[[
-- Updatale.lua
--]]
local Updatable = BaseClass("Updatable")


--添加刷新方法
local function AddUpdate(self)
	
	if self.Update~=nil then 
		self._update_handle = self.Update
		UpdateManager:GetInstance():AddUpdate(self._update_handle)
	end
	if self.LateUpdate ~=nil then
		self._lateupdate_handle = self.LateUpdate
		UpdateManager:GetInstance():AddLateUpdate(self._lateupdate_handle)
	end
	if self.FixedUpdate ~=nil then
		self._fixedupdate_handle = self.FixedUpdate
		UpdateManager:GetInstance():AddFixUpdate(self._fixedupdate_handle)
	end
end

--移除
local function RemoveUpdate(self)
	if self._update_handle~=nil then 
		UpdateManager:GetInstance():RemoveUpdate(self._update_handle)
	end
	if self._lateupdate_handle ~=nil then
		UpdateManager:GetInstance():RmoveLateUpdate(self._lateupdate_handle)
	end
	if self._fixedupdate_handle ~=nil then
		UpdateManager:GetInstance():RmoveFixUpdate(self._fixedupdate_handle)
	end
end

-- 是否启用更新
local function EnableUpdate(self, enable)
	RemoveUpdate(self)
	if enable then
		AddUpdate(self)
	end
end

local function Run()
	Updatable.New();
end

-- 构造函数
local function _init(self)
	EnableUpdate(self,true)
end

-- 析构函数
local function _delete(self)
	EnableUpdate(self,false)
end

Updatable.RemoveUpdate = RemoveUpdate
Updatable.AddUpdate = AddUpdate
Updatable.index =1
Updatable.EnableUpdate = EnableUpdate
Updatable._init = _init
Updatable._delete = _delete

Updatable.Run = Run

return Updatable