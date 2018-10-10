require "class"

Dictionary = class()

function Dictionary:Ctor()
    self.dic_table = {}
    self.keyList = {}
    print(">>>>>Dictionary:Ctor<<<<<");
    --print(self.dic_table);
end

function Dictionary:Add(_key, _value)
    if self.dic_table[_key] == nil then
        self.dic_table[_key] = _value;
        table.insert(self.keyList, _key);
    else
        self.dic_table[_key] = _value;
    end
end
 
function Dictionary:Clear()
    local count = self:Count();
    for i=count,1,-1 do
        self.dic_table[self.keyList[i]] = nil;
        table.remove(self.keyList);
    end
end
 
function Dictionary:ContainsKey(_key)
    local count = self:Count();
    for i=1,count do
        if self.keyList[i] == _key then
            return true;
        end
    end
    return false;
end
 
function Dictionary:ContainsValue(_value)
    local count = self:Count();
    for i=1,count do
        if self.dic_table[self.keyList[i]] == _value then
            return true;
        end
    end
    return false;
end
 
function Dictionary:Count()

    local count = 0;
    for k in pairs(self.keyList) do
        count = count + 1;
    end
    return count;
end
 
function Dictionary:Iter()
    local i = 0;
    local n = self:Count();
    return function ()
        i = i + 1;
        if i <= n then
            return self.keyList[i];
        end
        return nil;
    end
end
 
function Dictionary:Remove(_key)
    if self:ContainsKey(_key) then
        local count = self:Count()
        for i=1,count do
            if self.keyList[i] == _key then
                table.remove(self.keyList, i)
                break
            end
        end
        self.dic_table[_key] = nil;
    end
end

function Dictionary:GetValue(_key)
    for k,v in pairs(self.dic_table) do
        if k == _key then
            return v;
        end
    end
end
 

function Dictionary:ToString()

     local str = "{";

     for k,v in pairs(self.dic_table) do
        str = str.." k:"..k..", v:"..v.."\n" ;
     end
    
     str = str.."}"
     print(str)
end
