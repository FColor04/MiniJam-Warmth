-- Damage Types
DamageTypes = {
    Slash = {
        name = "Slash",
        color = {1, 0.5, 0},  -- Orange color
        modifier = 1.0,  -- Default damage modifier
        criticalChance = 0.05,  -- Chance to do a critical hit (5%)
        criticalDamage = 2.0,  -- Damage multiplier for critical hits
    },
    Blunt = {
        name = "Blunt",
        color = {0.5, 0.5, 1},  -- Blue-purple color
        modifier = 1.2,  -- 20% bonus damage to armor
        criticalChance = 0.02,  -- 2% chance to do a critical hit
        criticalDamage = 1.5,  -- 50% bonus damage for critical hits
    },
    Firearms = {
        name = "Firearms",
        color = {0.5, 1, 0},  -- Green-yellow color
        modifier = 1.0,  -- Default damage modifier
        criticalChance = 0.1,  -- 10% chance to do a critical hit
        criticalDamage = 2.0,  -- Double damage for critical hits
    },

-- Damage Effects
DamageEffects = {
    Normal = {
        name = "Normal",
        color = {1, 1, 1},  -- Pure white color
        modifier = 1.0,  -- Default damage modifier
        criticalChance = 0.05,  -- 5% chance to do a critical hit
        criticalDamage = 2.0,  -- Double damage for critical hits
    },
    Fire = {
        name = "Fire",
        color = {1, 0.25, 0},  -- Red-orange color
        modifier = 1.0,  -- Default damage modifier
        effect = "burn",  -- Causes a burn effect over time
        effectData = damageData.fire
    },
    Electric = {
        name = "Electric",
        color = {0.5, 0.5, 1},  -- Blue-purple color
        modifier = 1.0,  -- Default damage modifier
        effect = "stun",  -- Causes a stun effect over time
        effectData = damageData.electric
    },
    Poison = {
        name = "Poison",
        color = {0, 1, 0},  -- Green color
        modifier = 1.0,  -- Default damage modifier
        effect = "poison",  -- Causes a poison effect over time
        effectData = damageData.poison
    },
    Ice = {
        name = "Ice",
        color = {0, 0.75, 1},  -- Light blue color
        modifier = 1.0,  -- Default damage modifier
        effect = "slow",  -- Causes a slow effect over time
        effectData = {
            slowDuration = 5.0,  -- Duration of the slow effect in seconds
            slowFactor = 0.5,  -- The factor by which the target's speed is reduced (50%)
        },
    },
    Acid = {
        name = "Acid",
        color = {0.5, 1, 0},  -- Bright green-yellow color
        modifier = 1.0,  -- Default damage modifier
        effect = "corrode",  -- Causes a corrode effect over time
        effectData = damageData.acid
		}
        