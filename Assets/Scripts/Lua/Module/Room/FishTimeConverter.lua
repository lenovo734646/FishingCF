FishTimeConverter = class(nil, {
    configList,
    innerList,
    cacheIndex,
    isDirty,
})

function FishTimeConverter:__init()
    self.configList = {}
    self.innerList = {}
    self.cacheIndex = -1
    self.isDirty = false
end

function FishTimeConverter:reset()
    self.configList = {}
    self.innerList = {}
    self.cacheIndex = -1
    self.isDirty = false
end

function FishTimeConverter:convertRealTimeToFishTime(realTime)
    if self.isDirty then self:_tidy() end
    
    local t = self:_findInnerByRealTime(realTime)
    return t ~= nil and (t.FishTime + (realTime - t.RealTime) * t.Rate) or realTime
end

function FishTimeConverter:convertFishTimeToRealTime(fishTime)
    if self.isDirty then self:_tidy() end
    
    local t = self:_findInnerByFishTime(fishTime)
    return t ~= nil and (t.RealTime + (fishTime - t.FishTime) / t.Rate) or fishTime
end

function FishTimeConverter:addTimeRate(realTime, durationInMillionSeconds, rateInTenThousand, reason)
    table.insert(self.configList, FishTimeRateConfig({
        RealTime = realTime,
        DurationInMillionSeconds = durationInMillionSeconds,
        RateInTenThousand = rateInTenThousand,
        Reason = reason
    }))
    self.isDirty = true
end

function FishTimeConverter:getTimeRate(realTime)
    local t = self:_findInnerByRealTime(realTime)
    return t ~= nil and t.Rate or 1
end

function FishTimeConverter:getReason(realTime)
    local t = self:_findInnerByRealTime(realTime)
    return t ~= nil and t.Reason or 0
end

function FishTimeConverter:_findInnerByRealTime(realTime)
    local r = nil
    if #self.innerList > 0 then
        if self.cacheIndex >= 0 and self.cacheIndex < #self.innerList then
            local t = self.innerList[self.cacheIndex + 1]
            if realTime >= t.RealTime and (#self.innerList == self.cacheIndex + 1 or realTime < self.innerList[self.cacheIndex + 2].RealTime) then
                r = t
            end
        end
        if r == nil then
            self.cacheIndex = -1
            for k, v in pairs(self.innerList) do
                if realTime < v.RealTime then break end
                self.cacheIndex = self.cacheIndex + 1
            end
            if self.cacheIndex >= 0 then
                r = self.innerList[self.cacheIndex + 1]
            end
        end
    end
    return r
end

function FishTimeConverter:_findInnerByFishTime(fishTime)
    local r = nil
    if #self.innerList > 0 then
        local index = -1
        for k, v in pairs(self.innerList) do
            if fishTime < v.FishTime then break end
            index = index + 1
        end
        if index >= 0 then
            r = self.innerList[index + 1]
        end
    end
    return r
end

local TEN_THOUSAND = 10000
function FishTimeConverter:_tidy()
    if not self.isDirty then return end
    
    self.isDirty = false
    self.innerList = {}
    self.cacheIndex = -1
    
    table.sort(self.configList, function(a, b)
        return a.RealTime < b.RealTime
    end)
    
    local tmpConfigList = {}
    for k, v in pairs(self.configList) do
        if #tmpConfigList == 0 then
            table.insert(tmpConfigList, FishTimeRateConfig({
                RealTime = v.RealTime,
                DurationInMillionSeconds = v.DurationInMillionSeconds,
                RateInTenThousand = v.RateInTenThousand,
                Reason = v.Reason,
            }))
        else
            local tback = tmpConfigList[#tmpConfigList]
            local endRealTime = tback.RealTime + tback.DurationInMillionSeconds
            if endRealTime < v.RealTime then
                if tback.RateInTenThousand == TEN_THOUSAND then
                    tback.DurationInMillionSeconds = v.RealTime - tback.RealTime
                else
                    table.insert(tmpConfigList, FishTimeRateConfig({
                        RealTime = endRealTime,
                        DurationInMillionSeconds = v.RealTime - endRealTime,
                        RateInTenThousand = TEN_THOUSAND,
                        Reason = v.Reason,
                    }))
                end
            else
                tback.DurationInMillionSeconds = v.RealTime - tback.RealTime
            end
            table.insert(tmpConfigList, FishTimeRateConfig({
                RealTime = v.RealTime,
                DurationInMillionSeconds = v.DurationInMillionSeconds,
                RateInTenThousand = v.RateInTenThousand,
                Reason = v.Reason,
            }))
        end
    end
    
    for k, v in ipairs(tmpConfigList) do
        if #self.innerList == 0 then
            table.insert(self.innerList, FishTimeRateInner({
                RealTime = v.RealTime,
                FishTime = v.RealTime,
                Rate = v.RateInTenThousand / TEN_THOUSAND,
                Reason = v.Reason,
            }))
        else
            local tback = self.innerList[#self.innerList]
            table.insert(self.innerList, FishTimeRateInner({
                RealTime = v.RealTime,
                FishTime = tback.FishTime + (v.RealTime - tback.RealTime) * tback.Rate,
                Rate = v.RateInTenThousand / TEN_THOUSAND,
                Reason = v.Reason,
            }))
        end
    end
    if tmpConfigList[#tmpConfigList].RateInTenThousand ~= TEN_THOUSAND then
        local tlast = self.innerList[#self.innerList]
        table.insert(self.innerList, FishTimeRateInner({
            RealTime = tlast.RealTime + tmpConfigList[#tmpConfigList].DurationInMillionSeconds,
            FishTime = tlast.FishTime + tmpConfigList[#tmpConfigList].DurationInMillionSeconds * tlast.Rate,
            Rate = 1,
            Reason = 0,
        }))
    end
end



FishTimeRateConfig = class(nil, {
    RealTime,
    DurationInMillionSeconds,
    RateInTenThousand,
    Reason
})

function FishTimeRateConfig:__init(params)
    self.RealTime = params.RealTime
    self.DurationInMillionSeconds = params.DurationInMillionSeconds
    self.RateInTenThousand = params.RateInTenThousand
    self.Reason = params.Reason
end



FishTimeRateInner = class(nil, {
    RealTime,
    FishTime,
    Rate,
    Reason
})

function FishTimeRateInner:__init(params)
    self.RealTime = params.RealTime
    self.FishTime = params.FishTime
    self.Rate = params.Rate
    self.Reason = params.Reason
end
