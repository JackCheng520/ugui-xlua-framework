function Log(_log)
	if type(_log) == 'table' then
		local str = '{';
		for k,v in pairs(_log) do
			str = str ..tostring(v)..' , ';
			if type(v) == 'table' then
				Log(v);
			end
		end
		str = str.. '}';
		print(str);
	else
		print(_log);
	end

end

function LogError(_logError)
	CS.UnityEngine.Debug.LogError(_logError);
end
