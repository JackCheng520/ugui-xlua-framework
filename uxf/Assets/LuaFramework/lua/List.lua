require "class"

List = class();

function List:Ctor()
    self.list_table = {}
    print(">>>>>List:Ctor<<<<<");
end


function List:Add(_item)
    table.insert(self.list_table, _item);
end
 
function List:Clear()
    local count = self:Count();
    for i=count,1,-1 do
        table.remove(self.list_table);
    end
end
 
function List:Contains(_item)
    local count = self:Count();
    for i=1,count do
        if self.list_table[i] == _item then
            return true;
        end
    end
    return false;
end
 
function List:Count()
    local count = 0;
    for k in pairs(self.list_table) do
        count = count + 1;
    end
    return count;
end
 
function List:Find(_predicate)
    if (_predicate == nil or type(_predicate) ~= 'function') then
        print('_predicate is invalid!');
        return
    end
    local count = self:Count();
    for i=1,count do
        if _predicate(self.list_table[i]) then 
            return self.list_table[i] ;
        end
    end
    return nil;
end
 
function List:Foreach(_action)
    if (_action == nil or type(_action) ~= 'function') then
        print('_action is invalid!');
        return
    end
    local count = self:Count();
    for i=1,count do
        _action(self.list_table[i]);
    end
end
 
function List:IndexOf(_item)
    local count = self:Count();
    for i=1,count do
        if self.list_table[i] == _item then
            return i;
        end
    end
    return 0;
end
 
function List:LastIndexOf(_item)
    local count = self:Count();
    for i=count,1,-1 do
        if self.list_table[i] == _item then
            return i;
        end
    end
    return 0;
end
 
function List:Insert(_index, _item)
    table.insert(self.list_table, _index, _item);
end
 
function List:ItemType()
    return self.itemType;
end
 
function List:Remove(_item)
    local idx = self:LastIndexOf(_item);
    if (idx > 0) then
        table.remove(self.list_table, idx);
        --self:Remove(_item)
    end
end

function List:GetTop()
    local count = self:Count();
    return self.list_table[count];
end
 
function List:RemoveAt(_index)
    if(_index<1) then
        print("_index is out of range _index:".._index);
        return;
    end
    table.remove(self.list_table, _index);
end
 
function List:Sort(_comparison)
    if (_comparison ~= nil and type(_comparison) ~= 'function') then
        print('_comparison is invalid');
        return;
    end
    if func == nil then
        table.sort(self.list_table);
    else
        table.sort(self.list_table, func);
    end
end

function List:ToString()

     local str = "{";

     for k,v in pairs(self.list_table) do
        str = str.." k:"..k..", v:"..v.."\n" ;
     end
    
     str = str.."}"
     print(str)
end