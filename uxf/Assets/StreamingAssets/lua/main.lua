--///////////////////[lua 模块的启动类]/////////////////////
LuaMgrMainUI = require "LuaUI/LuaMgrMainUI"
require "LuaUI/LuaMgrMessageUI"
require "LuaUI/LuaMgrBagUI"
require "LuaUI/UIRoot2D"



require "Type/UIDefault"
require "Common/LuaDebugUtil"
require "class"
require "Stack"
require "List"
require "Dictionary"
require 'Type/EventType'
Event = require "Common/events"


Main = {}

--不需要卸载的模块(true ：不需要卸载，false ： 需要卸载)
local whiteList = {
		--lua 系统的模块
		['string'] = true,
		['io'] = true,
		['pb'] = true,
		['os'] = true;
		['debug'] = true,
		['table'] = true,
		['package'] = true,
		['coroutine'] = true,
		['pack'] = true,

		--自己添加的模块
		--['LuaUI/LuaMgrMessageUI'] = true,
}

function Main.GameStart()

    --测试参数
	BaseUI.ShowUI(GameUIType.GameUI_MainUI,1,2,3,4,5,6,7,8,9,10);

    --[[
	for k,_ in pairs(package.loaded) do
		Log('--------------------package.loaded --> k : '..k);
	end
	--]]
	Log("GameStart lua--->>");
end


--卸载所有lua模块
function Main.ReLoad()
	UIRoot2D.Clear();

	for k,_ in pairs(package.loaded) do
		if not whiteList[k] then
			package.loaded[k] = nil;
		end
	end
end

