Rect = CS.UnityEngine.Rect

DynamicInfinityList = class(LuaBase, {
    CellSize, --单元格尺寸（宽，高）
    SpacingSize, --单元格间隙（水平，垂直）
    ColumnCount, --列数(必须大于等于1)
    ItemPrefab, --动态格子prefab
    ItemScript, --动态格子绑定Lua脚本
    HideItemCount, --隐藏的动态格子数(必须大于等于1)
    
    itemCount, --渲染格子数
    maskSize, --蒙板尺寸
    maskRect, --蒙板矩形
    rectParent, --父节点
    itemList, --渲染脚本集合
    rectDic, --渲染格子字典
    dataProviders, --数据来源
    hasInited, --是否完成初始化
})

function DynamicInfinityList:__init()
    self.hasInited = false
    
    UpdateLuaBehaviour.Bind(self.gameObject, self)
    
    self.rectParent = self.transform:GetComponent("RectTransform")
    self.maskSize = self.transform.parent:GetComponent("RectTransform").sizeDelta
end

function DynamicInfinityList:Update()
    if self.hasInited then self:UpdateItem() end

    local num = (1920/1080)/(CS.UnityEngine.Screen.height/CS.UnityEngine.Screen.width)
    if (num) < 1  then
        self.rectParent.parent.parent.localScale = Vector3(num,1,1)
    end
end

function DynamicInfinityList:InitList(params)
    if self.hasInited then return end
    
    self.CellSize = params.CellSize
    self.SpacingSize = params.SpacingSize
    self.ColumnCount = params.ColumnCount ~= nil and params.ColumnCount or 1
    self.ItemPrefab = params.ItemPrefab
    self.ItemScript = params.ItemScript
    self.HideItemCount = params.HideItemCount ~= nil and params.HideItemCount or 1
    
    self.itemCount = self.ColumnCount * (math.ceil(self.maskSize.y / self:GetBlockSizeY()) + self.HideItemCount)
    self:updateDynamicRects(self.itemCount)
    self.itemList = {}
    self.itemPreList = {}
    for i = 1, self.itemCount do
        local child = Instantiate(self.ItemPrefab).transform
        child:SetParent(self.rectParent.transform)
        child.localRotation = Quaternion.identity
        child.localScale = Vector3.one
        --child.localScale = Vector3(1920/CS.UnityEngine.Screen.height,1,1)
        child.localPosition = Vector3.zero
        child.gameObject.layer = self.rectParent.gameObject.layer
        local dfItem = self.ItemScript(LuaClass(child.gameObject))
        table.insert(self.itemList, dfItem)
        dfItem:SetDRect(self.rectDic[i])
        child.gameObject:SetActive(false)
        self:updateChildTranformPos(child, i)
    end
    self:setRectParentSize(self.itemCount)
    self.hasInited = true
end

function DynamicInfinityList:SetDataProvider(datas)
    self:updateDynamicRects(#datas)
    self:setRectParentSize(#datas)
    self.dataProviders = datas
    self:clearAllItemListDr()
end

function DynamicInfinityList:GetDataProvider()
    return self.dataProviders
end

function DynamicInfinityList:RefreshDataProvider()
    if self.dataProviders == nil then logError("dataProviders 为空！请先使用SetDataProvider") end
    self:updateDynamicRects(#self.dataProviders)
    self:setRectParentSize(#self.dataProviders)
    self:clearAllItemListDr()
end

function DynamicInfinityList:setRectParentSize(count)
    self.rectParent.sizeDelta = Vector2(self.rectParent.sizeDelta.x, math.ceil(count / self.ColumnCount) * self:GetBlockSizeY())
    self.maskRect = Rect(0, -self.maskSize.y, self.maskSize.x, self.maskSize.y)
end

function DynamicInfinityList:updateChildTranformPos(child, index)
    index = index - 1
    local row = math.floor(index / self.ColumnCount)
    local column = math.fmod(index, self.ColumnCount)
    local v2Pos = Vector2(column * self:GetBlockSizeX(), -self.CellSize.y - row * self:GetBlockSizeY())
    local childRect = child:GetComponent("RectTransform")
    childRect.anchoredPosition3D = Vector3.zero
    childRect.anchoredPosition = v2Pos
end

function DynamicInfinityList:GetBlockSizeY()
    return self.CellSize.y + self.SpacingSize.y
end

function DynamicInfinityList:GetBlockSizeX()
    return self.CellSize.x + self.SpacingSize.x
end

function DynamicInfinityList:updateDynamicRects(count)
    self.rectDic = {}
    for i = 1, count do
        local row = math.floor(i-1 / self.ColumnCount)
        local column = math.fmod(i-1, self.ColumnCount)
        local dRect = DynamicRect(column * self:GetBlockSizeX(), -row * self:GetBlockSizeY() - self.CellSize.y, self.CellSize.x, self.CellSize.y, i)
        self.rectDic[i] = dRect
    end
end

function DynamicInfinityList:Reset()
    self.rectParent.anchoredPosition = Vector2(self.rectParent.anchoredPosition.x, 0)
end

function DynamicInfinityList:clearAllItemListDr()
    if self.itemList ~= nil then
        for k, v in ipairs(self.itemList) do
            v:SetDRect(nil)
        end
    end
end

function DynamicInfinityList:UpdateItem()
    self.maskRect.y = -self.maskSize.y - self.rectParent.anchoredPosition.y
    local inOverlaps = {}
    for k, rect in ipairs(self.rectDic) do
        if rect:Overlaps(self.maskRect) then
            inOverlaps[rect.Index] = rect
        end
    end
    for k, item in ipairs(self.itemList) do
        if item.DRect ~= nil and inOverlaps[item.DRect.Index] == nil then
            item:SetDRect(nil)
        end
    end
    local i = 0
    for k, rect in pairs(inOverlaps) do
        if self:getDynamicItem(rect) == nil then
            local item = self:getNullDynamicItem()
            item:SetDRect(rect)
            self:updateChildTranformPos(item.transform, rect.Index)
            if self.dataProviders ~= nil and rect.Index <= #self.dataProviders then
                i = i + 1
                item:SetData(self.dataProviders[rect.Index])
            end
        end
    end
end

function DynamicInfinityList:getNullDynamicItem()
    for k, item in ipairs(self.itemList) do
        if item.DRect == nil then
            return item
        end
    end
end

function DynamicInfinityList:getDynamicItem(rect)
    for k, item in ipairs(self.itemList) do
        if item.DRect ~= nil then
            if rect.Index == item.DRect.Index then
                return item
            end
        end
    end
    return nil
end

function DynamicInfinityList:GetItemSpriteList()
    if self.itemList ~= nil then
        return self.itemList
    end
end 



DynamicRect = class(nil, {
    mRect, --矩形数据
    Index, --格子索引
})

function DynamicRect:__init(x, y, width, height, index)
    self.Index = index
    self.mRect = Rect(x, y, width, height)
end

function DynamicRect:Overlaps(otherRect)
    return self.mRect:Overlaps(otherRect)
end



DynamicInfinityItem = class(LuaBase, {
    DRect, --动态矩形
    Data, --格子数据
})

function DynamicInfinityItem:SetData(data)
-- body
end

