--[[
-- DataClass.lua
-- 结构体
--]]
function DataClass(classname,data_tb,super)
	assert(type(classname) == "string" and #classname > 0)
	local cls
	--copy to super
	if super then
        cls = DeepCopy(super)
    else
        cls = {}
    end
	--拷贝数据表
	if data_tb then
		for i,v in pairs(data_tb) do
			cls[i] = v
		end
	end

	cls.super = super
    cls.__cname = classname

	function cls.New(...)
		local data = DeepCopy(cls)
		local ret_data
		data.New = nil
		ret_data = setmetatable(data,data)
		do
			local args = {...}
			local create
			create = function(c,...)
				if c.super then
					create(c.super,...)
				end
				if c.__init then
					c.__init(ret_data, ...)
				end
			end
			if #args > 0 then
				create(cls, ...)
			end
		end
		return ret_data
	end

	return cls
end