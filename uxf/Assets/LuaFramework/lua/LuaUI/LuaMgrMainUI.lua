require "LuaUI/BaseUI"

local LuaMgrMainUI=class(BaseUI)
local this = LuaMgrMainUI;
local testBtn;
 
function LuaMgrMainUI:Ctor()

end

--设置参数，界面初始化（初始化不要放在Ctor 构造函数中）
function LuaMgrMainUI:SetParam()
	--self:super('SetParam','_abName',  '_assetName',  'Prefabs/UI/UIMain/MainUIPrefab',  true);
    self:super('SetParam', BaseUIType.Normal , UICollider.None, true, 'uimain', 'Prefabs/UI/UIMain/MainUIPrefab',  true);
end

function LuaMgrMainUI:OnStart()
    Log("LuaMgrMainUI:OnStart");

    testBtn = self.uiprefab.transform:Find('root/btnTest').gameObject;
    CS.Game.LuaHelperUtil.AddButtonListener(testBtn,function ()
    	Log("Test button OnClick callback ....");
    	BaseUI.ShowUI(GameUIType.GameUI_MessageBoxUI);
    end);


end

function LuaMgrMainUI:Init()
    Log("LuaMgrMainUI:Init 参数:");
    Log(self.args);


end

function LuaMgrMainUI:Disapper()
    Log("LuaMgrMainUI:Disapper");

    if self.uiprefab ~= nil then
        self.uiprefab:SetActive(false);
    end
end

function LuaMgrMainUI:Appear()
    Log("LuaMgrMainUI:Appear"..self.uiprefab.name);
    if self.uiprefab ~= nil then
        self.uiprefab:SetActive(true);
    end

    Log("----------------------------测试消息系统----------------------------")
    local callback = function( ... )
        local arg ={...};
        --Log(arg);
        Log('消息回调 TEST...............................');
        print(table.unpack(arg));
    end

    Event.AddListener("TEST",callback);

    Event.Brocast("TEST",100,2,3);
end



function LuaMgrMainUI:IsActive()
    return self.uiprefab.activeSelf;
end

function LuaMgrMainUI:ResetArgs( ... )
    
end


function LuaMgrMainUI:ToString()
	print(" uiType:"..self.uiType.." uiCollider:"..self.uiCollider.." isStack:"..tostring(self.isStack).." abName:"..self.abName.." assetName:"..self.assetName.." needComTitle:"..self.needComTitle.." uiPath:"..self.uiPath);
end

return LuaMgrMainUI;
 