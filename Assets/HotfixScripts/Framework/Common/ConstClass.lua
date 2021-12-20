--[[
ConstClass:静态类
]]

function ConstClass(classname,const_tb,super)
    assert(type(classname) == "string" and #classname > 0)
    local cls
    if super then
        cls = DeepCopy(super)
    else
        cls = {}
    end

    if const_tb then
		for i,v in pairs(const_tb) do
			cls[i] = v
		end
	end

    cls.__cname = classname
	cls.__tostring = function(self)
		return table.dump(self, true, 2)
	end
    return cls
end