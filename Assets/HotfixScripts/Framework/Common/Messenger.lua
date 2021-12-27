--[[
--消息系统
-- Messenger.lua
--]]
local Messenger = BaseClass("Messenger")

local function _init(self)
	self.events = {}
end

local function _delete(self)
	self.events = nil	
	self.error_handle = nil
end

--添加消息监听
local function AddListener(self, e_type, e_listener, ...)
	local event = self.events[e_type]
	if event == nil then
		event = setmetatable({},{__mode="k"})
	end
	for k,v in pairs(event) do
		if k == e_listener then
			Log.Print(tostring(e_listener).."already exist...")
			return 
		end
	end
	event[e_listener] = setmetatable(SafePack(...),{__mode="kv"})
	self.events[e_type]=event
end
--执行监听的方法
local function Broadcast(self, e_type, ...)
	local event = self.events[e_type]
	if event==nil then
		return
	end
	for k,v in pairs(event) do
		local args = ConcatSafePack(v, SafePack(...))
		k(SafeUnpack(args))
	end
end
--移除监听
local function RemoveListener(self, e_type, e_listener)
	local event = self.events[e_type]
	if event == nil then
		return
	end

	event[e_listener] = nil
end
--清理Messenger
local function Cleanup(self)
	self.events = {}
end

Messenger._init = _init
Messenger._delete = _delete
Messenger.AddListener = AddListener
Messenger.Broadcast = Broadcast
Messenger.RemoveListener = RemoveListener
Messenger.Cleanup = Cleanup

return Messenger