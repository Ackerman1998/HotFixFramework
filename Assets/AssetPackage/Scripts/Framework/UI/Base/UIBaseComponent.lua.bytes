--[[
-- UIBaseComponet.lua
--UI组件基类：所有UI组件继承这个类
--]]
local UIBaseComponent = BaseClass("UIBaseComponent", Updatable)
local base = Updatable
--构造函数
local function _init(self, holder, var_arg)
	-- 窗口view层脚本
	self.view = nil
	-- 持有者(父节点)
	self.holder = holder
	-- 脚本绑定的transform
	self.transform = nil
	-- transform对应的gameObject
	self.gameObject = nil
	-- trasnform对应的RectTransform
	self.rectTransform = nil
	-- 名字：Unity中获取Transform的名字是有GC的，而Lua侧组件大量使用了名字，所以这里缓存下
	self.__name = nil
	-- 绑定数据：在某些场景下可以提供诸多便利
	self.__bind_data = nil
	-- 可变类型参数，用于重载
	self.__var_arg = var_arg
	self:EnableUpdate(false)
end

local function GetName(self)
	return self.__name
end

local function EnableUpdate(self,active)

end

--析构函数
local function _delete(self)
	
end

local function OnCreate(self)
	assert(not IsNull(self.holder), "Err : holder nil!")
	assert(not IsNull(self.holder.transform), "Err : holder tansform nil!")
	
	-- 初始化view
	if self._class_type == UILayer then
		self.view = nil
	else
		local now_holder = self.holder
		while not IsNull(now_holder) do	
			if now_holder._class_type == UILayer then
				self.view = self
				break
			elseif not IsNull(now_holder.view) then
				self.view = now_holder.view
				break
			end
			now_holder = now_holder.holder
		end
		assert(not IsNull(self.view))
	end

	-- 初始化其它基本信息
	if type((self.__var_arg)) == "string" then
		--根据路径名获取
		self.transform = UIUtil.FindTransform(self.holder.transform,self.__var_arg)

		self.gameObject = self.transform.gameObject
	elseif type((self.__var_arg)) == "number" then
		self.transform = UIUtil.GetChild(self.holder.transform, self.__var_arg)
		self.gameObject = self.transform.gameObject
	elseif type((self.__var_arg)) == "userdata" then
		self.gameObject = self.__var_arg
		self.transform = gameObject.transform
	else

	end
	self.__name = self.gameObject.name
	self.rectTransform=UIUtil.FindComponent(self.transform,typeof(CS.UnityEngine.RectTransform))
	-- if self.rectTransform~=nil then
	-- 	
	-- 	self.rectTransform.offsetMax = Vector2.zero
	-- 	self.rectTransform.offsetMin = Vector2.zero
	-- 	self.rectTransform.localScale = Vector3.one
	-- 	self.rectTransform.localPosition = Vector3.zero
	-- end
	self.__var_arg = nil

end

-- 销毁
local function OnDestroy(self)
	if self.holder.OnComponentDestroy ~= nil then
		self.holder:OnComponentDestroy(self)
	end
	self.holder = nil
	self.transform = nil
	self.gameObject = nil
	self.rectTransform = nil
	self.__name = nil
	self.__bind_data = nil
end

UIBaseComponent._init = _init
UIBaseComponent._delete = _delete
UIBaseComponent.OnCreate = OnCreate
UIBaseComponent.OnDestroy = OnDestroy
UIBaseComponent.EnableUpdate = EnableUpdate
UIBaseComponent.GetName = GetName
return UIBaseComponent