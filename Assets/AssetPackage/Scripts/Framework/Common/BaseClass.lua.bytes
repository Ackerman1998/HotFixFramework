--[[
-- BaseClass.lua
--]]
--专门用来存放类的虚表
local _class={}
ClassType={
	class=1,
	instance=2,
}

function BaseClass(className,super)
	Log.Print("BaseClass Run..." .. className)
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


--[[
--保存类类型的虚表
--_class的结构如下：_class = { Class_A = vtbl_A, Class_B = vtbl_B}
local _class = {}
 
 --BaseClass(super)
 --函数功能：创建一个super类的子类类型
 --子类类型实现了New方法、设置了当前类的元表（包括__index函数和__newindex函数，用于索引操作，该操作实现了对虚表的索引）
 --如果是继承自父类，那么虚表的元表的索引方法将会在父类super对应的vtbl中去查找键值
 
function BaseClass(super)
	-- 生成一个类类型,	实际上存放类信息
	local class_type = {}
	
	-- 在创建对象的时候自动调用
	-- 默认的两个属性
	--class_type.__init = false
	class_type.__delete = false
	class_type.super = super
	
	--!!!看此方法时，先跳过，看完BaseClass的其他定义后，再来看此"成员函数
	-- 创建接口
	-- 子类型class_type创建实例对象的方法
	class_type.New = function(...)
		-- 生成一个类对象
		local obj = {}
		obj._class_type = class_type
		-- 在初始化之前注册基类方法
		-- 即引入基类class_type的虚表vtbl
		setmetatable(obj, { __index = _class[class_type] })
		-- 注册一个delete方法
		if(super and not super.DeleteMe) then
			obj.DeleteMe = function(obj_self)
				local now_super = obj_self._class_type 
				while now_super ~= nil do	
					if now_super.__delete then
						now_super.__delete(obj_self)
					end
					now_super = now_super.super
				end
			end
		end
		-- 调用初始化方法
		class_type.__init(obj, ...)
		
		return obj
	end
	--class_type类型的虚表
	local vtbl = {}
	--在类类型的虚表中，为当前的类类型添加虚表，比如使用Layer = Layer or BaseClass() 创建了一个子类Layer，那么，此时类类型虚表中，会添加这么一项：_class = { Class_A = vtbl_A, Class_B = vtbl_B, Layer = {}}
	_class[class_type] = vtbl
	-- 设置class_type的元表，给出访问索引函数和赋值索引函数
	-- 从赋值索引函数__newindex上可以看出，如果要往class_type类型的中添加属性，会在其虚表vtbl中实现这种键值的添加
	-- 比如Layer = BaseClass()后，通过Layer.position = {x = 0.0, y = 0.0} 往Layer中添加一个键值，此时该键值会添加在Layer类型所对应的虚表中，
	此时虚表变为：_class = { Class_A = vtbl_A, Class_B = vtbl_B, Layer = { position = {x = 0.0, y = 0.0} }}
	-- 于是这么做的好处就现象出来了，类型Layer自身的键值不会发生改变，因此可以做为一个“稳定”的基类去派生子类，而所有的变化，都体现在Layer所对应的虚表vtbl_Layer中，这样的设计，封装了变化，也便于继承的实现
	setmetatable(class_type, {__newindex =
		function(t,k,v)
			vtbl[k] = v
		end
		, 
		__index = vtbl, --For call parent method
	})
	--如果是通过super派生子类，那么vtbl的访问索引将在super的vtbl中查找
	if super then
		setmetatable(vtbl, {__index =
			function(t,k)
				local ret = _class[super][k]
				--do not do accept, make hot update work right!
				--vtbl[k] = ret
				return ret
			end
		})
	end
 
	return class_type
end
--[[
总结
BaseClass.lua提供了一个从已有类派生子类的方法。

为每个类型提供一个虚表（_class中存储这些虚表）
创建一个新的table（这里表现为class_type）作为新的类类型，
申明并定义class_type的属性class_type.__delete  class_type.super；
声明并定义class_type的方法class_type.New
配置class_type类型对应的虚表：vtbl
设置class_type的元表的访问索引和赋值索引
处理class_type类型从父类继承的属性和方法（通过设置class_type的虚表vtbl的元表访问索引来实现）
特别值得注意的是class_type的New方法——该方法表示从class_type类型创建一个实例obj，并调用class_type的__init方法来初始化obj对象；
（也就是说当使用Layer = Layer or BaseClass() 创建一个新类型的时候，需要实现Layer:__init(...)方法；
创建一个table(实际中是local obj = {})
设置obj对象元表，其访问索引方法__index 从class_type的虚表vtbl中查找；
注册了一个delete方法，通过自底向上的方式析构对象；
使用__init方法来初始化；

]]