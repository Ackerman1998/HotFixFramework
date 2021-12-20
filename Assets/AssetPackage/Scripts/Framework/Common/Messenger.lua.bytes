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

--添加监听
local function AddListener(self, e_type, e_listener, ...)
	local event = self.events[e_type]
	if event == nil then
		event = setmetatable({},{__mode="k"})
	end
	--判断是否已经添加过了
	for k,v in pairs(event) do
		if k==e_listener then
			error("Aready cotains listener : "..tostring(e_listener))
			return 
		end
	end
	event[e_listener] = setmetatable(SafePack(...), {__mode = "kv"}) 
	self.events[e_type]=event
end
--发送消息
local function Broadcast(self, e_type, ...)
	local event = self.events[e_type]
	if event == nil then
		return
	end
	for k,v in pairs(event) do
		assert(k ~= nil)
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