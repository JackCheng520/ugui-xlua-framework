require "class"

local root;
local normalRoot;
local messageRoot;
local canvasRoot;
local camera;
local eventSystem;
local graphicRaycaster;

UIRoot2D = class()


function UIRoot2D.Init(_initOkCallBack)
	local uiRoot = CS.UnityEngine.GameObject.Find("UIRoot2D");
	if uiRoot == nil then
		CS.Game.ResUtil.GetAssetsPrefab('uiroot','Prefabs/UI/UIRoot/UIRoot2D',function ( ... )
	        Log("UIRoot2D:Init");
	        arg = {...};
	        uiRoot = CS.UnityEngine.GameObject.Instantiate(arg[1]);
	        uiRoot.name = "UIRoot2D";
            uiRoot.transform.position = CS.UnityEngine.Vector3.zero;
            uiRoot.transform.localScale = CS.UnityEngine.Vector3.one;
            uiRoot.transform.localRotation = CS.UnityEngine.Quaternion.identity;

            root = uiRoot;

            camera = uiRoot.transform:Find("UICamera"):GetComponent('Camera');
            if camera == nil then
            
                c = uiRoot.transform:Find("UICamera").gameObject;
                camera = c.GetComponent('Camera');
            end
            camera.depth = 10;

            canvasRoot = uiRoot.transform:Find("Canvas"):GetComponent('Canvas');

            normalRoot = uiRoot.transform:Find("Canvas/normalLayer"):GetComponent('RectTransform');
            UIRoot2D.SetRectTransformAnchors(normalRoot);
            normalRoot.localPosition = CS.UnityEngine.Vector3(0, 0, 1000);

            messageRoot = uiRoot.transform:Find("Canvas/messageLayer"):GetComponent('RectTransform');
            UIRoot2D.SetRectTransformAnchors(messageRoot);
            messageRoot.localPosition = CS.UnityEngine.Vector3.zero;

            eventSystem = uiRoot.transform:Find("EventSystem"):GetComponent('EventSystem');
            graphicRaycaster = {};
            graphicRaycaster.normal = normalRoot:GetComponent('GraphicRaycaster');
            graphicRaycaster.message = messageRoot:GetComponent('GraphicRaycaster');

            
            CS.Game.LuaHelperUtil.AddComponent(uiRoot,'Game.UI.GameEventListeners');

			_initOkCallBack();

	    end);
		
	else
			camera = uiRoot.transform:Find("UICamera"):GetComponent('Camera');
            if camera == nil then
            
                c = uiRoot.transform:Find("UICamera").gameObject;
                camera = c.GetComponent('Camera');
            end
            camera.depth = 10;

            if canvasRoot == nil then
            	canvasRoot = uiRoot.transform:Find("Canvas"):GetComponent('Canvas');
        	end

        	if normalRoot == nil then
	            normalRoot = uiRoot.transform:Find("Canvas/normalLayer"):GetComponent('RectTransform');
	            UIRoot2D.SetRectTransformAnchors(normalRoot);
	            normalRoot.localPosition = CS.UnityEngine.Vector3(0, 0, 1000);
        	end

        	if messageRoot == nil then
	            messageRoot = uiRoot.transform:Find("Canvas/messageLayer"):GetComponent('RectTransform');
	            UIRoot2D.SetRectTransformAnchors(messageRoot);
	            messageRoot.localPosition = CS.UnityEngine.Vector3.zero;
        	end

        	if eventSystem == nil then
	            eventSystem = uiRoot.transform:Find("EventSystem"):GetComponent('EventSystem');
	            graphicRaycaster = {};
	            graphicRaycaster.normal = normalRoot:GetComponent('GraphicRaycaster');
	            graphicRaycaster.message = messageRoot:GetComponent('GraphicRaycaster');
        	end
            
            CS.Game.LuaHelperUtil.AddComponent(uiRoot,'Game.UI.GameEventListeners');

		_initOkCallBack();
	end

	
end

function UIRoot2D.Clear()
    Log('###### UIRoot2D.Clear001 ######');
    local normalLayer = CS.UnityEngine.GameObject.Find("UIRoot2D/Canvas/normalLayer").gameObject;

    CS.Game.LuaHelperUtil.RemoveAllUnParentChild(normalLayer);
    local messageLayer = CS.UnityEngine.GameObject.Find("UIRoot2D/Canvas/messageLayer").gameObject;

    CS.Game.LuaHelperUtil.RemoveAllUnParentChild(messageLayer);
    Log('###### UIRoot2D.Clear ######');
end



function UIRoot2D.SetRectTransformAnchors( _rect )
	_rect.anchorMin = CS.UnityEngine.Vector2.zero;
    _rect.anchorMax = CS.UnityEngine.Vector2.one;
    _rect.pivot = CS.UnityEngine.Vector2.one * 0.5;
    _rect.anchoredPosition3D = CS.UnityEngine.Vector3.zero;
    _rect.sizeDelta = CS.UnityEngine.Vector2.zero;
end

function UIRoot2D.MountOn2DUIRootNormal(_obj)
        
            --设置ui挂载点（临时）
            UIRoot2D.Init(function ( )
            	if _obj ~= nil then
                    _obj.transform:SetParent(normalRoot, false);
                    _obj.transform.localScale = CS.UnityEngine.Vector3.one;
                    _obj.transform.localPosition = CS.UnityEngine.Vector3.zero;
                end
            end);
end


function UIRoot2D.MountOn2DUIRootMessage(_obj)

    --设置ui挂载点（临时）
    UIRoot2D.Init(function ( )
    	if _obj ~= nil then
            _obj.transform:SetParent(messageRoot, false);
            _obj.transform.localScale = CS.UnityEngine.Vector3.one;
            _obj.transform.localPosition = CS.UnityEngine.Vector3.zero;
        end
    end);
end

function UIRoot2D.GetNormalRoot()
	return normalRoot;
end

function UIRoot2D.GetMessageRoot()
	return messageRoot;
end

function UIRoot2D.GetRoot()
	return root;
end