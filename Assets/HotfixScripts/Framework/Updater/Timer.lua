--[[
-- Timer
-- 定时器
--]]
local Timer=BaseClass("Timer")

local function _init(self, delay, func, obj, one_shot, use_frame, unscaled)
    self.target = setmetatable({},{__mode="v"})
    if delay and func then
        self:Init(delay, func, obj, one_shot, use_frame, unscaled)
    end
end
--初始化定时器
local function Init(self, delay, func, obj, one_shot, use_frame, unscaled)
    --延时时长
    self.delay = delay
    --回调函数
    self.target.func = func
    --回传对象
    self.target.obj = obj
    --是否只作一次定时
    self.one_shot = one_shot
    --是否使用帧定时，反之为秒定时
    self.use_frame = use_frame
    --是否使用deltatime定时
    self.unscaled = unscaled
    -- 是否启用
	self.started = false
	-- 倒计时（剩余的时间）
	self.left = delay
	-- 是否结束
	self.over = false
	-- 传入对象是否为空
	self.objIsNull = obj and true or false
	-- 启动定时器时的帧数
	self.start_frame_count = Time.frameCount
end

local function Update(self,isFix)
    if not self.started or self.over then
        return
    end
    --是否结束了
    local timeover = false
    if self.use_frame then
        timeover = (Time.frameCount >= self.start_frame_count + self.delay)
    else 
        local deltaTime = nil
        if isFix then
            deltaTime=Time.fixedDeltaTime
        else 
            deltaTime=not self.unscaled and Time.deltaTime or Time.unscaledDeltaTime
        end
        self.left=self.left-deltaTime
        timeover = self.left<=0
    end

    if timeover then
    
        if self.target.func~=nil then
            if not self.one_shot then
              
            else 
                self.over = true
            end
            -- if self.over then
            --     Log.Print("one shot =false , over = true")
            -- else
            --     Log.Print("one shot =false , over = false")
            -- end
           
            local status,err
            if self.target.obj~=nil then
                status,err = pcall(self.target.func,self.target.obj)
            else 
                status,err = pcall(self.target.func)
            end
            if not status then
                self.over = true
                print(err)
            end
        else 
            self.over = true
        end
    end 

  
end

local function Start(self)
    if self.over then
		error("You can't start a overed timer, try add a new one!")
	end
	if not self.started then
		self.left = self.delay
		self.started = true
		self.start_frame_count = Time.frameCount
	end
end

local function Resume(self)
    self.started = true
end

local function Pause(self)
    self.started = false
end

local function Stop(self)
    self.left = 0
	self.one_shot = false
	self.target.func = nil
	self.target.obj = nil
	self.use_frame = false
	self.unscaled = false
	self.started = false
	self.over = true
end

local function IsOver(self)
    return self.over
end
Timer._init =_init
Timer.Init = Init
Timer.Update = Update
Timer.Start = Start
Timer.Resume = Resume
Timer.Pause = Pause
Timer.Stop = Stop
Timer.IsOver = IsOver
return Timer

