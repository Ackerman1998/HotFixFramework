--[[
--UIUtil LUA-UI工具类    
--]]
local UIUtil =ConstClass("UIUtil",UIUtil)

local function FindTransform(trans,path)
    return trans:Find(path)
end
--查找组件
local function FindComponent(trans,ctype,path)
    assert(trans~=nil)
    assert(ctype~=nil)
    local transtarget = trans
    if path~=nil and #path>0 and type(path)=="string" then
        transtarget = trans:Find(path)
    end
    if transtarget==nil then
        return nil
    end
    local componet = transtarget:GetComponent(ctype)
    if componet~=nil then
        return componet
    end
    return transtarget:GetComponentInChildren(ctype)
end

UIUtil.FindTransform=FindTransform
UIUtil.FindComponent=FindComponent
return UIUtil