--[[
-- UpdateManager.lua
-- 刷新:通过Unity Update LateUpdater FixedUpdate 来调用lua的方法
--]]
list = require "list"
Time = require "Time"
require "event"
local Messenger = require "Messenger" 
local UpdateManager=BaseClass("UpdateManager",Singleton)

--unity 三种刷新方法
local UpdateMsg = "Update"
local LateUpdateMsg = "LateUpdate"
local FixedUpdateMsg = "FixedUpdate"

local function _init(self)
	self.ui_message_center = Messenger.New()
	-- Update
	self._update_handle = nil
	-- LateUpdate
	self._lateupdate_handle = nil
	-- FixedUpdate
	self._fixedupdate_handle = nil
end

-- Update回调
local function UpdateHandle(self)
	--Log.Print("Update Run:")
	self.ui_message_center:Broadcast(UpdateMsg)
end

-- LateUpdate回调
local function LateUpdateHandle(self)
	--self.ui_message_center:Broadcast(LateUpdateMsg)
end

-- FixedUpdate回调
local function FixedUpdateHandle(self)
	--self.ui_message_center:Broadcast(FixedUpdateMsg)
end

local function _delete(self)
	
end

local function StartUp(self)
	--注册刷新方法
	-- Update
	self._update_handle = UpdateBeat:CreateListener(UpdateHandle,UpdateManager:GetInstance())
	-- LateUpdate
	self._lateupdate_handle = LateUpdateBeat:CreateListener(LateUpdateHandle,UpdateManager:GetInstance())
	-- FixedUpdate
	self._fixedupdate_handle = FixedUpdateBeat:CreateListener(FixedUpdateHandle,UpdateManager:GetInstance())
	UpdateBeat:AddListener(self._update_handle)
	LateUpdateBeat:AddListener(self._lateupdate_handle)
	FixedUpdateBeat:AddListener(self._fixedupdate_handle)

end
--添加--
local function AddUpdate(self,func)
	self.ui_message_center:AddListener(UpdateMsg,func)
end

local function AddLateUpdate(self,func)
	self.ui_message_center:AddListener(LateUpdateMsg,func)
end

local function AddFixUpdate(self,func)
	self.ui_message_center:AddListener(FixedUpdateMsg,func)
end
--添加--

--移除--
local function RemoveUpdate(self,func)
	self.ui_message_center:RemoveListener(UpdateMsg,func)
end

local function RemoveLateUpdate(self,func)
	self.ui_message_center:RemoveListener(LateUpdateMsg,func)
end

local function RemoveFixUpdate(self,func)
	self.ui_message_center:RemoveListener(FixedUpdateMsg,func)
end
--移除--


UpdateManager._init=_init
UpdateManager._delete=_delete
UpdateManager.StartUp=StartUp
UpdateManager.AddUpdate=AddUpdate
UpdateManager.AddLateUpdate=AddLateUpdate
UpdateManager.AddFixUpdate=AddFixUpdate
UpdateManager.RemoveUpdate=RemoveUpdate
UpdateManager.RemoveLateUpdate=RemoveLateUpdate
UpdateManager.RemoveFixUpdate=RemoveFixUpdate
return UpdateManager