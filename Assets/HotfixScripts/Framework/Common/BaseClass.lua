--[[
-- 继承实现
-- BaseClass.lua
--]]
--专门用来存放类的虚表
local _class={}
ClassType={
	class=1,
	instance=2,
}

function BaseClass(className,super)
	--Log.Print("BaseClass Run..." .. className)
	--生成一个类类型
	local class_type = {}
	class_type._init=false
	class_type._delete=false
	class_type._cname=className
	--是类还是实例
	class_type._ctype=ClassType.class
	class_type.super = super--父类
	class_type.New=function(...)--实例化方法
		--生成一个类的实例
		local obj ={}
		obj._class_type = class_type
		obj._ctype = ClassType.instance
		--生成的实例继承类所有的属性和方法
		setmetatable(obj,{
			__index = _class[class_type]
		})
		--调用初始化方法
		do
			local create--定义一个create方法
			create = function(c,...)
				if c.super then--如果有继承父类，则调用父类的create方法
					create(c.super,...)
				end
				if c._init then--调用它的init方法 
					c._init(obj,...)
				end
			end
			create(class_type,...)--调用这个方法
		end
		--注册delete方法
		obj.Delete = function(self)
			local now_super = self._class_type
			while now_super ~= nil do
				if now_super._delete then 
					now_super._delete(self)
				end
				now_super = now_super.super
			end
		end
		return obj--返回实例
    end
	--把这个类保存起来
	local vtbl = {}--存放这个类(表)的容器
	_class[class_type] = vtbl
	--当lua类中有方法，变量声明时，也会将这些存放到vtbl这个表中，最终也就是存到_class这个表中
	setmetatable(class_type,{
		__newindex=function(t,k,v)
			vtbl[k]=v
			--Log.Print("run newindex... key ="..k)
		end
		,
		__index=vtbl,--接下来可以在类中使用之前声明的方法和变量
	})
	--如果要继承父类
	if super then
		setmetatable(vtbl,{
			--将这个子类存放到要继承的父类的表中
			__index = function(t,k)
				--Log.Print("have base class：" .. className)
				local result = _class[super][k]
				--vtbl[k] = result
				return result
			end
		})
	end
	--最终返回这个类
	return class_type
end
