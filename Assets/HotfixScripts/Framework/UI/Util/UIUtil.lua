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
        -- if trans.name=="UILogin" then
        --     Log.Print(trans.name.."GetCompnent")
        --     Log.Print("component.gameObject.name: "..componet.gameObject.name)
        -- end
        return componet
    end
    return transtarget:GetComponentInChildren(ctype)
end
--查找组件--text
local function FindText(trans)
    local ui_text = trans:GetComponent(typeof(CS.UnityEngine.UI.Text))
    if ui_text~=nil then
        return ui_text
    else 
        return nil
    end
end
--查找组件--进度条
local function FindSlider(trans)
    local ui_Slider = trans:GetComponent(typeof(CS.UnityEngine.UI.Slider))
    if ui_Slider~=nil then
        return ui_Slider
    else 
        return nil
    end
end

local function FindButton(trans,path)
     local target_button = trans:GetComponent(typeof(CS.UnityEngine.UI.Button))
     if target_button~=nil then
        return target_button
    else 
        return nil
    end
end

UIUtil.FindTransform=FindTransform
UIUtil.FindComponent=FindComponent
UIUtil.FindText=FindText
UIUtil.FindSlider=FindSlider
UIUtil.FindButton=FindButton
return UIUtil