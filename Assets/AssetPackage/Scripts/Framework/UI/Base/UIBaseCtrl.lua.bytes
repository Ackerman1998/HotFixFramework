--[[
--UIBaseCtrl - ui 控制层基类
--主要用来实现游戏逻辑,修改数据
]]
local UIBaseCtrl = BaseClass("UIBaseCtrl")

local function _init(self,model)
   assert(model~=nil)
   self.model =model
end

local function _delete(self)
	self.model =nil
end

UIBaseCtrl._init = _init
UIBaseCtrl._delete = _delete
return UIBaseCtrl