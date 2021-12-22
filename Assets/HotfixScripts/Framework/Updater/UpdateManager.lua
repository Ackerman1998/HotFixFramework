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
	Log.Print("调用:")
	self.ui_message_center:Broadcast(UpdateMsgName)
end

-- LateUpdate回调
local function LateUpdateHandle(self)
	--self.ui_message_center:Broadcast(LateUpdateMsgName)
end

-- FixedUpdate回调
local function FixedUpdateHandle(self)
	--self.ui_message_center:Broadcast(FixedUpdateMsgName)
end

local function _delete(self)
	
end

local function StartUp(self)
	--注册

	-- Update
	self._update_handle = UpdateBeat:CreateListener(UpdateHandle,UpdateManager:GetInstance())
	-- LateUpdate
	self._lateupdate_handle = LateUpdateBeat:CreateListener(LateUpdateHandle,UpdateManager:GetInstance())
	-- FixedUpdate
	self._fixedupdate_handle = FixedUpdateBeat:CreateListener(FixedUpdateHandle,UpdateManager:GetInstance())
	Log.Print("register startup")
	UpdateBeat:AddListener(self._update_handle)
	LateUpdateBeat:AddListener(self._lateupdate_handle)
	FixedUpdateBeat:AddListener(self._fixedupdate_handle)

end

UpdateManager._init=_init
UpdateManager._delete=_delete
UpdateManager.StartUp=StartUp
return UpdateManager