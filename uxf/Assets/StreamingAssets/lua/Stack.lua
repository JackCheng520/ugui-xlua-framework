require "class"

Stack = class()

function Stack:Ctor()
    self.stack_table = {}
    print(">>>>>Stack:Ctor<<<<<");
end

function Stack:Size()
    return self:Nums();
end

function Stack:Push(element)
     local size = self:Size()
     --print("stack  size :"..size);
     self.stack_table[size + 1] = element
end

function Stack:Pop()
     local size = self:Size()
     if self:IsEmpty() then
         print("Error: Stack is empty!")
         return
     end
     return table.remove(self.stack_table, size)
end

function Stack:IsEmpty()
     local size = self:Size()
     if size == 0 then
         return true
     end
     return false
end

function Stack:Clear()
    self.stack_table = nil;
    self.stack_table = {}
end

function Stack:Nums()
    if(self.stack_table == nil) then
    return 0;
    end
    local count = 0  
    for k,v in pairs(self.stack_table) do  
        count = count + 1  
    end 

    return count;
end

function Stack:ToString()
     local size = self:Size()

     if self:IsEmpty() then
         print("Error: Stack is empty!")
         return
     end

     local str = "{"..self.stack_table[size]
     size = size - 1
     while size > 0 do
         str = str..", "..self.stack_table[size]
         size = size - 1
     end
     str = str.."}"
     print(str)
end