-- Weapon Base Class
Weapon = {}
function Weapon:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
end

function Weapon:addDamageType(damageType)
    self.damageTypes = self.damageTypes or {}
    table.insert(self.damageTypes, damageType)
end

function Weapon:getDamageTypeColor()
    if self.damageTypes then
        for _, damageType in ipairs(self.damageTypes) do
            local color = damageTypes[damageType].color
            if color then
                return color
            end
        end
    end
    return damageTypes.normal.color
end

function Weapon:getDamageEffects()
    local effects = {}
    if self.damageTypes then
        for _, damageType in ipairs(self.damageTypes) do
            local effect = damageTypes[damageType].effect
            if effect then
                table.insert(effects, effect)
            end
        end
    end
    return effects
end

function Weapon:getDamageData()
    local data = {}
    if self.damageTypes then
        for _, damageType in ipairs(self.damageTypes) do
            local damage = damageData[damageType]
            if damage then
                data[damageType] = damage
            end
        end
    end
    return data
end