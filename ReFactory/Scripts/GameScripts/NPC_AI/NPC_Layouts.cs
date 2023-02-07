//==========================================================
//	This is a simple text file to guide the creation and modification of #ReFactory# NPCs and AI scripts.
//	For the time being, this is a general idea board and actual implementation may differ greatly
//	
//	
//==========================================================
//	Category Layouts and Definitions
//	NPCs have 2 main Categories, they all either fall under 'Humanoid' or 'Creature'.

//	Humanoids Examples: Towns People, Bandits, Cave People, Mermaids.

//	Creature Examples: Frog, Cat, Lizard, Giant Flying Beast, Mammoth, Mouse.
//	
//==========================================================

#region Optional Features for NPCs, and their intended use case

#region 
//	
//	
//	
//	
//	
#endregion

#region RequiredNPC: 
// **Should only be applied to 'Humanoid' unless explicitly stated otherwise.**
//	
//	
//	
//	
//	
//	
//	
//	
//	
//	
#endregion

#region 
//	
//	
//	
//	
//	
#endregion

#region 
//	
//	
//	
//	
//	
#endregion

#region 
//	
//	
//	
//	
//	
#endregion

#region 
//	
//	
//	
//	
//	
#endregion

#endregion

#region Base Traits

#region Basic Statistics
//	Health
//	Armour/Natural Damage Resistance
//	Unarmed Damage
//	Killable *Default True
//	Vision Range
//	Hearing Range
#endregion

#region Humanoid & Creature Personality 'Adoptible' Traits
//**In this sense, 'Adoptible' just means Non-Default for most traits**

//	Curiosity : Clamped float -100 to 100 (amount of 'Disruption' (Audio/Visual) before this NPC investigates. 100 means Extremely Curios. Negitive Numbers result in degrees of fleeing)
//	
//	
//	
//	
#endregion

#endregion

#region Humanoid

#region Traits/Features
//	Favorability (Towards the Player)
//	
//	
//	
#endregion

#region Information
//	Information Fields. Used for dialog trees, Quest pointers, NPC pointers.
//	Should include the general location of where/how the information is relavent or came to be.

#region Experience Based Knowledge
//	Things like Occupation or World Events this NPC took part in; generally before player interaction. (AKA 'Generated')

#region Example Jobs/Practices and World Events
//	====Jobs & Practices====
//	Lumber Jack, Excavator/Miner
//	Construction Worker
//	Hunter, Farmer, Scavenger
//	
//	
//	
//	
//	====World Events====
//	
//	
//	
//	
#endregion

#endregion

#region Situational Based Knowledge
//	Events that took place in the game, generally due directly or indirectly by player actions.

#region Example Actions or Scenarios that might occur
//	
//	
//	
//	
//	
#endregion

#endregion

#endregion

#region Skills

#region Knowledge/Occupation Based
//	
//	
//	
//	
#endregion

#region Situation Based
//	
//	
//	
//	
#endregion



#endregion

#region Locations
//
#endregion

#endregion



#region Creature

#region Traits/Features
// Natural Hostility : Clamped float 0-100, Default 25.0f
//
//
//
//
#endregion

#region
//
//
//
//
#endregion

#endregion