function __TRACKBACK__(errorMsg)
    local track_text = debug.traceback(tostring(errorMsg))
    logError(track_text)
    return false;
end

function default_ctor(self, ...)
    local obj = {}
    setmetatable(obj, self)
    if obj.__init then
        obj:__init(...)
    end
    return obj
end

function class(super, cls)
    if not cls then cls = {} end
    
    local mt = {}
    if super then
        setmetatable(mt, super)
        cls.super = super
    end
    
    mt.__index = mt
    mt.__call = function(self, ...)
        if self.New then
            return self:New(...)
        else
            return default_ctor(self, ...)
        end
    end
    setmetatable(cls, mt)
    cls.__index = cls
    return cls
end

--import function
function import(moduleName, currentModuleName)
    local currentModuleNameParts
    local moduleFullName = moduleName
    local offset = 1
    
    while true do
        if string.byte(moduleName, offset) ~= 46 then -- .
            moduleFullName = string.sub(moduleName, offset)
            if currentModuleNameParts and #currentModuleNameParts > 0 then
                moduleFullName = table.concat(currentModuleNameParts, ".") .. "." .. moduleFullName
            end
            break
        end
        offset = offset + 1
        
        if not currentModuleNameParts then
            if not currentModuleName then
                local n, v = debug.getlocal(3, 1)
                currentModuleName = v
            end
            
            currentModuleNameParts = string.split(currentModuleName, ".")
        end
        table.remove(currentModuleNameParts, #currentModuleNameParts)
    end
    
    return require(moduleFullName)
end

functional = {}

function functional.bind(func, count, ...)
    local args_origin = {...}
    return function(...)
        local args = {...}
        local num = table.maxn(args)
        for i = num, 1, -1 do
            args[i + count] = args[i]
        end
        for i = 1, count do
            args[i] = args_origin[i]
        end
        return func(table.unpack(args))
    end
end

function functional.bindself(self, fname)
    return functional.bind1(self[fname], self)
end

function functional.bind1(func, obj1)
    return function(...)
        return func(obj1, ...)
    end
end

function functional.bind2(func, obj1, obj2)
    return function(...)
        return func(obj1, obj2, ...)
    end
end

function table.maxn(tbl)
    local max = nil
    local count = 0
    for k, v in pairs(tbl) do
        if type(k) == "number" then
            if max then
                if k >= 0 and k > max then max = k end
            else
                if k >= 0 then max = k end
            end
        end
        count = count + 1
    end
    if count == 0 then max = 0 end
    return max
end

function table.contains(tbl, value)
    for k, v in pairs(tbl) do
        if v == value then
            return k
        end
    end
    return false
end

function table.removebyvalue(array, value, removeall)
    local c, i, max = 0, 1, #array
    while i <= max do
        if array[i] == value then
            table.remove(array, i)
            c = c + 1
            i = i - 1
            max = max - 1
            if not removeall then break end
        end
        i = i + 1
    end
    return c
end

function table.removekvpair(pair, value, removeall)
    for k, v in pairs(pair) do
        if v == value then
            pair[k] = nil
            if not removeall then break end
        end
    end
end

function table.intersect(tblA, tblB)
    local tblT = {}
    for k, v in ipairs(tblA) do
        if table.contains(tblB, v) then
            table.insert(tblT, v)
        end
    end
    return tblT
end

function table.find(tbl, func)
    local ret
    for k, v in ipairs(tbl) do
        if func(v) then
            ret = v
        end
    end
    return ret
end

function table.findArray(tbl, func)
    local tblR = {}
    for k, v in ipairs(tbl) do
        if func(v) then
            table.insert(tblR, v)
        end
    end
    return tblR
end

function table.findMap(tbl, func)
    local tblR = {}
    for k, v in ipairs(tbl) do
        if func(v) then
            tblR[k] = v
        end
    end
    return tblR
end

function table.count(tbl)
    local values = table.values(tbl)
    return #values
end

function table.keys(tbl)
    local keys = {}
    for k, v in pairs(tbl) do
        --log("table.keys:"..k)
        table.insert(keys, k)
    end
    return keys
end

function table.values(tbl)
    local values = {}
    for k, v in pairs(tbl) do
        if v ~= nil then
            table.insert(values, v)
        end
    end
    return values
end

function table.clone(tbl)
    local lookup_table = {}
    local function copy(obj)
        if type(obj) ~= "table" then
            return obj
        elseif lookup_table[obj] then
            return lookup_table[obj]
        end
        local new_table = {}
        lookup_table[obj] = new_table
        for k, v in pairs(obj) do
            new_table[copy(k)] = copy(v)
        end
        return setmetatable(new_table, getmetatable(obj))
    end
    return copy(tbl)
end

function table.print(tbl, level, filteDefault)
    local msg = ""
    filteDefault = filteDefault or true --默认过滤关键字（DeleteMe, _class_type）
    level = level or 1
    local indent_str = ""
    for i = 1, level do
      indent_str = indent_str.."  "
    end
  
    print(indent_str .. "{")
    for k,v in pairs(tbl) do
      if filteDefault then
        if k ~= "_class_type" and k ~= "DeleteMe" then
          local item_str = string.format("%s%s = %s", indent_str .. " ",tostring(k), tostring(v))
          print(item_str)
          if type(v) == "table" then
            table.print(v, level + 1)
          end
        end
      else
        local item_str = string.format("%s%s = %s", indent_str .. " ",tostring(k), tostring(v))
        print(item_str)
        if type(v) == "table" then
            table.print(v, level + 1)
        end
      end
    end
    print(indent_str .. "}")
end

function string.split(input, delimiter)
    input = tostring(input)
    delimiter = tostring(delimiter)
    if delimiter == '' then return false end
    local pos, arr = 0, {}
    for st, sp in function() return string.find(input, delimiter, pos, true) end do
        table.insert(arr, string.sub(input, pos, st - 1))
        pos = sp + 1
    end
    table.insert(arr, string.sub(input, pos))
    return arr
end

function string.isNullOrEmpty(str)
    return not str or str == ""
end

function string.indexof(str, searchStr)
    local startIndex = string.find(str, searchStr, 1, true)
    return startIndex
end

function string.lastindexof(str, searchStr)
    local lastindex = nil
    local p = string.find(str, searchStr, 1, true)
    lastindex = p
    while p do
        p = string.find(str, searchStr, p + 1, true)
        if p then
            lastindex = p
        end
    end
    return lastindex
end
