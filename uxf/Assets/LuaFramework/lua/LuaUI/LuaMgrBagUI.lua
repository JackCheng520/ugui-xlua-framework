require "LuaUI/BaseUI"

LuaMgrBagUI=class(BaseUI)		

function LuaMgrBagUI:Ctor()	

end

function LuaMgrBagUI:SetParam()
	--self:super('SetParam','_abName',  '_assetName',  'Prefabs/UI/UIBag/BagUIPrefab',  true);
    self:super('SetParam', BaseUIType.Normal , UICollider.Normal, true, 'uibag', 'Prefabs/UI/UIBag/BagUIPrefab',  true);
end

function LuaMgrBagUI:OnStart()
    Log("LuaMgrBagUI:OnStart");
    local btnClose = self.uiprefab.transform:Find('btnClose').gameObject;
    CS.Game.LuaHelperUtil.AddButtonListener(btnClose,function ()
        Log("BagUI button OnClick callback ....");
        BaseUI.CloseUI(GameUIType.GameUI_BagUI);
    end);
end

function LuaMgrBagUI:Init()
    Log("LuaMgrBagUI:Init");
end

function LuaMgrBagUI:Disapper()
    Log("LuaMgrBagUI:Disapper");
    if self.uiprefab ~= nil then
        self.uiprefab:SetActive(false);
    end
end

function LuaMgrBagUI:Appear()
    Log("LuaMgrBagUI:Appear");
    if self.uiprefab ~= nil then
        self.uiprefab:SetActive(true);
    end
end

function LuaMgrBagUI:IsActive()
    return self.uiprefab.activeSelf;
end

function LuaMgrBagUI:ResetArgs( ... )
    
end


function LuaMgrBagUI:ToString()
	print(" uiType:"..self.uiType.." uiCollider:"..self.uiCollider.." isStack:"..tostring(self.isStack).." abName:"..self.abName.." assetName:"..self.assetName.." needComTitle:"..self.needComTitle.." uiPath:"..self.uiPath);
end
 