require "class"


BaseUI=class()	

-->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

function BaseUI:Ctor()
    Log("BaseUI ctor ");
end

function BaseUI:SetParam(_uiType, _uiCollider,  _isStack,  _abName,  _uipath,  _needComTitle)
	Log("BaseUI SetParam ");

    self.uiType = _uiType;
    self.uiCollider = _uiCollider;
    self.isStack = _isStack;
    self.abName = _abName;
    self.needComTitle = _needComTitle;
    self.uiPath = _uipath;
    self.uiprefab = nil;
end

--[[
function BaseUI:SetParam( _abName,  _assetName,  _uipath,  _needComTitle )	
	print("BaseUI SetParam 2");

	self.uiType = BaseUIType.Normal;
    self.uiCollider = UICollider.Normal;
    self.isStack = true;
    self.abName = _abName;
    self.assetName = _assetName;
    self.needComTitle = _needComTitle;
    self.uiPath = _uipath;	
    self.uiprefab = nil;
end
--]]
-->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
--加载ui prefab
function BaseUI:LoadUI( ... )
    --添加参数
    local param = {...};
    if param ~= nil and #param > 0 then
        if self.args ~= nil then
            self.args = nil;
        end
        self.args = param;
    end

    --初始化ui 框架
    UIRoot2D.Init(function()
        Log("BaseUI:LoadUI ...");
        if self.uiprefab == nil then
            CS.Game.ResUtil.GetAssetsPrefab(self.abName,self.uiPath,function ( ... )
                Log("BaseUI:LoadUICallBack");
                arg={...};
                Log(arg);
                self.uiprefab = CS.UnityEngine.GameObject.Instantiate(arg[1][0]);
                self:SetUIPrefab(true);
            end);
        else
            self:SetUIPrefab(false);
        end
    end);
    
end

--设置ui prefab
function BaseUI:SetUIPrefab(_isFirst)
    if _isFirst then
        self:SetUILayer();
        self:SetUIType();
        self:SetUICollider();
        
        self:OnStart();
    end
    self:SetComTitle();
    self:Init();
    self:Appear();

    BaseUI.PushUI(self);

    BaseUI.HideOldUIs();

end

--设置ui layer层
function BaseUI:SetUILayer()
    CS.Game.LuaHelperUtil.SetGameObjectAndChildLayer(self.uiprefab,'UI');
end

--设置ui 挂载点
function BaseUI:SetUIType()
    if self.uiType == BaseUIType.Normal then
        self.uiprefab.transform:SetParent(UIRoot2D.GetNormalRoot(), false);
    elseif self.uiType == BaseUIType.Message then
        self.uiprefab.transform:SetParent(UIRoot2D.GetMessageRoot(), false);
    else
        self.uiprefab.transform:SetParent(UIRoot2D.GetRoot(), false);
    end
    self.uiprefab.transform.localScale = CS.UnityEngine.Vector3.one;
end

--设置ui碰撞层
function BaseUI:SetUICollider()
    if self.uiCollider == UICollider.None then
        return;
    end

    --加载hide ok回调
    local callback = function(...)
        local arg = {...};

        if arg == nil or #arg <= 0 then
            return;
        end

        --Log('BaseUI:SetUICollider 参数长度 ：'..#arg);
        local uiHide = CS.UnityEngine.GameObject.Instantiate(arg[1][0]);
        uiHide.transform:SetParent(self.uiprefab.transform, false);
        uiHide.transform:SetAsFirstSibling();

        local hideRectTran = uiHide.transform:GetComponent('RectTransform');
        hideRectTran.anchoredPosition3D = CS.UnityEngine.Vector3.zero;
        hideRectTran.sizeDelta = CS.UnityEngine.Vector2.zero;
        hideRectTran.anchorMin = CS.UnityEngine.Vector2.zero;
        hideRectTran.anchorMax = CS.UnityEngine.Vector2.one;
        hideRectTran.pivot = CS.UnityEngine.Vector2.one * 0.5;


        if self.uiCollider == UICollider.WithGrayBg then
            local image = uiHide:GetComponent('Image');
            c = image.color;
            c.a = 0.5;
            image.color = c;
        end


        uiHide:SetActive(true);
    end

    --加载hide 背景
    if self.uiCollider == UICollider.Normal then
        CS.Game.ResUtil.GetAssetsPrefab('uiroot','Prefabs/UI/UIRoot/uiHide',callback);
    elseif self.uiCollider == UICollider.WithBg then
        CS.Game.ResUtil.GetAssetsPrefab('uiroot','Prefabs/UI/UIRoot/uiHideNoBg',callback);
    elseif self.uiCollider == UICollider.WithGrayBg then
        CS.Game.ResUtil.GetAssetsPrefab('uiroot','Prefabs/UI/UIRoot/uiHideWithGrayBg',callback);
    end
end

--设置公共title
function BaseUI:SetComTitle()
    
end

-->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
--第一次加载预设
function BaseUI:OnStart()
    Log('BaseUI:OnStart');
end

--初始化
function BaseUI:Init()
    
end

--隐藏ui
function BaseUI:Disapper()
    if self.uiprefab ~= nil then
        self.uiprefab:SetActive(false);
    end
end

--显示ui
function BaseUI:Appear()
    if self.uiprefab ~= nil then
        self.uiprefab:SetActive(true);
    end
end

--是否是显示状态
function BaseUI:IsActive()
    return false;
end

--重置参数
function BaseUI:ResetArgs( ... )
    self.args = nil;
    self.args = {...};
end

--打印消息
function BaseUI:ToString()
	print(" uiType:"..self.uiType.." uiCollider:"..self.uiCollider.." isStack:"..tostring(self.isStack).." abName:"..self.abName.." assetName:"..self.assetName.." needComTitle:"..self.needComTitle.." uiPath:"..self.uiPath);
end


-->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

local uiDic;
local uiStack;
local uiNotStack

--打开ui界面
function BaseUI.ShowUI(_uiKey , ...)
    if uiDic == nil then
        uiDic = Dictionary.New();
    end

    local _ui ;
    if uiDic:ContainsKey(_uiKey) then
        _ui = uiDic:GetValue(_uiKey);
    else
        _ui = BaseUI.CreateUI(_uiKey);
        _ui:SetParam();
        uiDic:Add(_uiKey,_ui);
    end

    if _ui == nil then
        Log("BaseUI.ShowUI ui is nil ...");
        return;
    end
    
    _ui.name = _uiKey;
    _ui:LoadUI(...);

end

--关闭堆栈打开界面
function BaseUI.ClearStackShowUI(_uiKey,...)
    local mainui;
    for _,v in pairs(uiDic) do
        if v ~= nil then
            v:Disapper();
            if v.name == GameUIType.GameUI_MainUI then
               mainui = v;
            end
        end
    end

    uiStack:Clear();
    uiNotStack:Clear();

    if mainui ~= nil then
        uiStack:Add(mainui);
    end

    BaseUI.ShowUI(_uiKey,...);
end

--关闭ui界面
function BaseUI.CloseUI(_uiKey)
    if uiDic:ContainsKey(_uiKey) then
        
        local _ui = uiDic:GetValue(_uiKey);
        if _ui ~= nil then
            if _ui.isStack then
            --处理入栈ui
                if uiStack ~= nil then
                    local uiIndex = uiStack:IndexOf(_ui);
                    if uiIndex ~= 0 then
                        uiStack:RemoveAt(uiIndex);
                    end
                    local count = uiStack:Count();
                    if count > 0 then
                    
                        local topUI = uiStack:GetTop();
                        --if (topUI.args != null && topUI.args.Length > 0)
                         --   ShowUI(topUI.name, topUI, null, topUI.args);
                        --else
                            BaseUI.ShowUI(topUI.name);
                    end


                end
            
            else
            --处理非入栈ui

            end

            _ui:Disapper();
        end

        return;
    end
end

--将ui界面压栈
function BaseUI.PushUI(_ui)
    if uiStack == nil then
        uiStack = List.New();
    end

    if uiNotStack == nil then
        uiNotStack = List.New();
    end

    if _ui == nil then
        Log("BaseUI.PushUI ui is nil");
        return;
    end

    if not _ui.isStack then
        uiNotStack:Add(_ui);
        return;
    end

    local uiIndex = uiStack:IndexOf(_ui);

    if uiIndex ~= 0 then
        uiStack:RemoveAt(uiIndex);

    end
    uiStack:Add(_ui);

end

--关闭所有ui界面
function BaseUI.HideOldUIs()
    local count = uiStack:Count();
    if count <= 1 then
        return;
    end

    local topUI = uiStack:GetTop();
    if topUI ~= nil and topUI.isStack then
        for i=count-1,1,-1 do
            uiStack.list_table[i]:Disapper();
        end
    end

    --隐藏所有非入栈ui
    count = uiNotStack:Count();
    for i=count,1,-1 do
        uiNotStack.list_table[i]:Disapper();
    end
    
end

--注册ui界面
function BaseUI.RegisterUI(_uiKey)
    if uiDic == nil then
        uiDic = Dictionary.New();
    end

    local _ui ;
    if uiDic:ContainsKey(_uiKey) then
        _ui = uiDic:GetValue(_uiKey);
    else
        _ui = BaseUI.CreateUI(_uiKey);
        _ui:SetParam();
        uiDic:Add(_uiKey,_ui);
    end

    _ui.name = _uiKey;
end

--获取ui界面
function BaseUI.GetUI(_uiKey)
    local _ui;
    if uiDic:ContainsKey(_uiKey) then
        _ui = uiDic:GetValue(_uiKey);
    end
    return _ui;
end

--创建UI，有新的ui类型都要加在这里
function BaseUI.CreateUI(_uiKey)
    local _ui;
    if _uiKey == GameUIType.GameUI_MainUI then
        _ui = LuaMgrMainUI.New();

    elseif _uiKey == GameUIType.GameUI_MessageBoxUI then
        _ui = LuaMgrMessageUI.New();

    elseif _uiKey == GameUIType.GameUI_BagUI then
        _ui = LuaMgrBagUI.New();
    end
    return _ui;
end

--关闭所有界面
function BaseUI.HideAllUIs()
    for k,v in pairs(uiDic) do
        if v ~= nil then
            v:Disapper();
        end
    end
end

--清空
function BaseUI.Clear()
    BaseUI.HideAllUIs();
    uiStack:Clear();
    uiNotStack:Clear();
end
