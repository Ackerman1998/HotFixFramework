--[[
-- UpdateManager.lua
绑定在UIRoot游戏物体上
--]]
local UIManager = BaseClass("UIManager", Singleton)

-- UIRoot路径
local UIRootPath = "UIRoot"
-- EventSystem路径
local EventSystemPath = "EventSystem"
-- UICamera路径
local UICameraPath = UIRootPath.."/UICamera"

local function _init(self)
    -- 所有存活的窗体
	self.windows = {}
	-- 所有可用的层级(存放的是UI层级的gameobject)
	self.layers = {}
	-- 保持Model
	self.keep_model = {}
	-- 窗口记录队列
	self.__window_stack = {}
	-- 是否启用记录
	self.__enable_record = true

	-- 初始化组件
	self.gameObject = CS.UnityEngine.GameObject.Find(UIRootPath)
	self.transform = self.gameObject.transform
	self.camera_ui = CS.UnityEngine.GameObject.Find(UICameraPath)
	self.UICamera = self.camera_ui:GetComponent(typeof(CS.UnityEngine.Camera))
	CS.UnityEngine.Object.DontDestroyOnLoad(self.gameObject)
	local event_system = CS.UnityEngine.GameObject.Find(EventSystemPath)
	CS.UnityEngine.Object.DontDestroyOnLoad(event_system)

	assert(not IsNull(self.transform))
	assert(not IsNull(self.UICamera))
	--过滤
	local layers = table.choose(UILayers,function(k,v)
		return type(v)=="table" and v.OrderInLayer ~= nil and v.Name ~= nil and type(v.Name) == "string" and #v.Name > 0
	end)

	--初始化ui层级 生成配置中所有层级的gameobject
	table.walksort(layers,function(k,v)
		return layers[k].OrderInLayer<layers[v].OrderInLayer
	end,function(k,v)
		local obj = CS.UnityEngine.GameObject(v.Name)
		local tran = obj.transform
		tran:SetParent(self.transform)--设置父节点
		local new_layer = UILayer.New(self,v.Name)
		new_layer:OnCreate(v)
		self.layers[v.Name] = new_layer
	end)
end

local function _delete(self)
	
end

local function StartUp(self)
	
end

local function InitWindow(self, ui_name, window)
	--先获取UI的配置表
	local ui_config = UIConfig
	assert(config, "No window named : "..ui_name..".You should add it to UIConfig first!")

	local layer = self.layers[config.Layer.Name]
	assert(layer, "No layer named : "..config.Layer.Name..".You should create it first!")

	window.Name = ui_name
	if self.keep_model[ui_name] then
		window.Model = self.keep_model[ui_name]
	elseif ui_config.Model then
		window.Model =ui_config.Model.New(ui_name)
	end

	if ui_config.Ctrl then
		window.Ctrl = ui_config.Ctrl
	end

	if ui_config.View then
		window.View = ui_config.View:New(layer,window.Name,window.Model,window.Ctrl)
	end

	window.Active =false
	window.Layer = layer
	window.PrefabPath = ui_config.PrefabPath

	return window
end

local function GetWindow(self, ui_name, active)
	local window=self.windows[ui_name]
	if window==nil then
		return nil
	elseif  active~=nil and active~=window.Active then
		return nil
	end
	return window
end
--激活
local function ActivateWindow(self,window,...)
	window.Model:Activate(...)
	window.View:SetActive(true)
end

--反激活窗口
local function Deactivate(self, window)
	window.Model:Deactivate()
	window.View:SetActive(false)
end

--打开目标窗口
local function InnerOpenWindow(self, target, ...)
	assert(target)
	assert(target.Model)
	assert(target.Ctrl)
	assert(target.View)
	assert(target.Active == false, "You should close window before open again!")

	target.Active=true
	local has_view = target.View
	local has_prefab_res = target.PrefabPath~=nil and #target.PrefabPath>0
	local has_gameobject = target.View.gameObject~=nil
	local has_result = has_view and has_prefab_res and has_gameobject
	if has_result then
		ActivateWindow(self,target,...)
	elseif not target.IsLoading then
		target.IsLoading = true
		local params = SafePack(...)
		GameObjectPool:GetInstance():GetGameObjectAsync(target.PrefabPath,function(go)
			if IsNull(go) then
				return
			end

			local tran_go = go.transform
			tran_go:SetParent(target.Layer.transform)
			tran_go.name =target.name
			target.IsLoading=false
			target.View:OnCreate()
			if target.Active then
				ActivateWindow(self, target, SafeUnpack(params))
			end
		end)
	end
end
--关闭窗口
local function InnerCloseWindow(self, target)
	assert(target)
	assert(target.Model)
	assert(target.Ctrl)
	assert(target.View)
	if target.Active then
		Deactivate(self,target)
		target.Active=false
	end
end

local function OpenWindow(self, ui_name, ...)
    local target = self:GetWindow(ui_name)
    if not target then
        --nil，实例化
        local window = UIWindow.New()
        self.windows[ui_name] = window
		target = InitWindow(self,ui_name,window)
    end
	InnerCloseWindow(self,target)
	InnerOpenWindow(self,target,...)
end

UIManager._init=_init
UIManager._delete=_delete
UIManager.StartUp=StartUp
UIManager.OpenWindow=OpenWindow
UIManager.InnerCloseWindow=InnerCloseWindow
UIManager.InnerOpenWindow=InnerOpenWindow
UIManager.Deactivate=Deactivate
UIManager.ActivateWindow=ActivateWindow
UIManager.GetWindow=GetWindow
UIManager.InitWindow=InitWindow
return UIManager