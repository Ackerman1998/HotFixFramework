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
        -- if trans.name=="UILoading" then
        --     Log.Print(trans.name.."GetCompnent")
        --     Log.Print("component.gameObject.name: "..componet.gameObject.name)
        -- end
        return componet
    end
    return transtarget:GetComponentInChildren(ctype)
end

local function FindText(trans)
    local ui_text = trans:GetComponent(typeof(CS.UnityEngine.UI.Text))
    if ui_text~=nil then
        return ui_text
    else 
        return nil
    end
end

UIUtil.FindTransform=FindTransform
UIUtil.FindComponent=FindComponent
UIUtil.FindText=FindText
return UIUtil