--[[
-- TimeManager
-- 定时管理器
--]]
local TimeManager=BaseClass("TimeManager",Singleton)
function _init(self)
    self._update_handle = nil
    self._coUpdate_handle = nil

    --存放定时器的池子
    self.timer_pool = {}

    self.__coupdate_timer = {}

    self.__coupdate_toadd={}
end

local function DelayRecycle(self, timers)
    for k,v in pairs(timers) do
        if k:IsOver() then
            k:Stop()
            table.insert(self.timer_pool,k)
            timers[k]=nil
        end
    end
end

local function UpdateHandle(self)

end
--coUpdate
local function CoUpdateHandle(self)
    for timer,_ in pairs(self.__coupdate_toadd) do
		self.__coupdate_timer[timer] = true
		self.__coupdate_toadd[timer] = nil
	end
	for timer,_ in pairs(self.__coupdate_timer) do
		timer:Update(false)
	end
	DelayRecycle(self, self.__coupdate_timer)
end

--从这里启动
local function StartUp(self)
    --创建监听
    self._update_handle = UpdateBeat:CreateListener(UpdateHandle,TimeManager:GetInstance())
    self._coUpdate_handle = CoUpdateBeat:CreateListener(CoUpdateHandle,TimeManager:GetInstance())
    --添加监听
    UpdateBeat:AddListener(self._update_handle)
    CoUpdateBeat:AddListener(self._coUpdate_handle)
end
--获取定时器
local function GetTimer(self, delay, func, obj, one_shot, use_frame, unscaled)
    local timer = nil
    if #self.timer_pool>0 then
        timer = table.remove(self.timer_pool)
        timer:Init(delay, func, obj, one_shot, use_frame, unscaled)
    else
        timer = Timer.New(delay, func, obj, one_shot, use_frame, unscaled)
    end
    return timer
end

local function GetCoTimer(self, delay, func, obj, one_shot, use_frame, unscaled)
    local timer = GetTimer(self, delay, func, obj, one_shot, use_frame, unscaled)
    self.__coupdate_toadd[timer] = true
    return timer
end

function _delete(self)

end
TimeManager._init =_init
TimeManager.StartUp=StartUp
TimeManager.GetCoTimer=GetCoTimer
return TimeManager

