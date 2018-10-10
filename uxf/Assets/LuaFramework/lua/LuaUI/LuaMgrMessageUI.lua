require "LuaUI/BaseUI"

LuaMgrMessageUI=class(BaseUI)		

function LuaMgrMessageUI:Ctor()	

end

function LuaMgrMessageUI:SetParam()
	self:super('SetParam', BaseUIType.Message , UICollider.Normal, false, 'uimessage', 'Prefabs/UI/UIMessage/MsgUIPrefab',  false);
end

function LuaMgrMessageUI:OnStart()
    Log("LuaMgrMessageUI:OnStart");
    local btnClose = self.uiprefab.transform:Find('btnClose').gameObject;
    CS.Game.LuaHelperUtil.AddButtonListener(btnClose,function ()
        BaseUI.CloseUI(GameUIType.GameUI_MessageBoxUI);
    end);

    local btnConfirm = self.uiprefab.transform:Find('cancelBtn').gameObject;
    CS.Game.LuaHelperUtil.AddButtonListener(btnConfirm,function ()
        BaseUI.ShowUI(GameUIType.GameUI_BagUI);
    end);
end

function LuaMgrMessageUI:Init()
    Log("LuaMgrMessageUI:Init");
    local title = self.uiprefab.transform:Find('title/value').gameObject;
    title.transform:GetComponent('Text').text = '测试在线更新lalalalalla';

end

function LuaMgrMessageUI:Disapper()
    Log("LuaMgrMessageUI:Disapper");
    if self.uiprefab ~= nil then
        self.uiprefab:SetActive(false);
    end
end

function LuaMgrMessageUI:Appear()
    Log("LuaMgrMessageUI:Appear");
    if self.uiprefab ~= nil then
        self.uiprefab:SetActive(true);
    end
end

function LuaMgrMessageUI:IsActive()
    return self.uiprefab.activeSelf;
end

function LuaMgrMessageUI:ResetArgs( ... )
    
end


function LuaMgrMessageUI:ToString()
	print(" uiType:"..self.uiType.." uiCollider:"..self.uiCollider.." isStack:"..tostring(self.isStack).." abName:"..self.abName.." assetName:"..self.assetName.." needComTitle:"..self.needComTitle.." uiPath:"..self.uiPath);
end
 