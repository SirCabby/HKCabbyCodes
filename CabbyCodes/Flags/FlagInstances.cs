using CabbyCodes.Scenes;

namespace CabbyCodes.Flags
{
    public static class FlagInstances
    {
        #region Bosses
            public static readonly FlagDef aladarSlugDefeated = new FlagDef("aladarSlugDefeated", null, false, "PlayerData_Int", "Warrior's Grave Gorb (0-2). 1 = Defeated, 2 = Claimed Reward");
            public static readonly FlagDef Crossroads_09__Mawlek_Body = new FlagDef("Mawlek Body", SceneInstances.Crossroads_09, false, "PersistentBoolData", "Brooding Mawlek defeated");
            public static readonly FlagDef defeatedMantisLords = new FlagDef("defeatedMantisLords", null, false, "PlayerData_Bool", "Mantis Lords defeated");
            public static readonly FlagDef falseKnightDefeated = new FlagDef("falseKnightDefeated", null, false, "PlayerData_Bool", "False Knight defeated");
            public static readonly FlagDef elderHuDefeated = new FlagDef("elderHuDefeated", null, false, "PlayerData_Int", "Warrior's Grave Elder Hu (0-2). 1 = Defeated, 2 = Claimed Reward");
            public static readonly FlagDef falseKnightDreamDefeated = new FlagDef("falseKnightDreamDefeated", null, false, "PlayerData_Bool", "Failed Knight defeated");
            public static readonly FlagDef galienDefeated = new FlagDef("galienDefeated", null, false, "PlayerData_Int", "Warrior's Grave Galien (0-2). 1 = Defeated, 2 = Claimed Reward");
            public static readonly FlagDef hornet1Defeated = new FlagDef("hornet1Defeated", null, false, "PlayerData_Bool", "Hornet defeated at Greenpath");
            public static readonly FlagDef noEyesDefeated = new FlagDef("noEyesDefeated", null, false, "PlayerData_Int", "Warrior's Grave No Eyes (0-2). 1 = Defeated, 2 = Claimed Reward");
            public static readonly FlagDef xeroDefeated = new FlagDef("xeroDefeated", null, false, "PlayerData_Int", "Warrior's Grave Xero (0-2). 1 = Defeated, 2 = Claimed Reward");
        #endregion

        #region Charms
            public static readonly FlagDef charmsOwned =        new FlagDef("charmsOwned",      null, false, "PlayerData_Int", "Charms owned count");
            public static readonly FlagDef charmSlots =         new FlagDef("charmSlots",       null, false, "PlayerData_Int", "Notches");
            public static readonly FlagDef charmSlotsFilled =   new FlagDef("charmSlotsFilled", null, false, "PlayerData_Int", "Charm notches in use");

            #region Charms
                public static readonly FlagDef gotCharm_1 = new FlagDef("gotCharm_1", null, false, "PlayerData_Bool", "Gathering Swarm");
                public static readonly FlagDef gotCharm_2 = new FlagDef("gotCharm_2", null, false, "PlayerData_Bool", "Wayward Compass");
                public static readonly FlagDef gotCharm_3 = new FlagDef("gotCharm_3", null, false, "PlayerData_Bool", "Stalwart Shell");
                public static readonly FlagDef gotCharm_4 = new FlagDef("gotCharm_4", null, false, "PlayerData_Bool", "Soul Catcher");
                public static readonly FlagDef gotCharm_5 = new FlagDef("gotCharm_5", null, false, "PlayerData_Bool", "Baldur Shell");
                public static readonly FlagDef gotCharm_6 = new FlagDef("gotCharm_6", null, false, "PlayerData_Bool", "Fury of the Fallen");
                public static readonly FlagDef gotCharm_7 = new FlagDef("gotCharm_7", null, false, "PlayerData_Bool", "Quick Focus");
                public static readonly FlagDef gotCharm_8 = new FlagDef("gotCharm_8", null, false, "PlayerData_Bool", "Lifeblood Heart");
                public static readonly FlagDef gotCharm_9 = new FlagDef("gotCharm_9", null, false, "PlayerData_Bool", "Lifeblood Core");
                public static readonly FlagDef gotCharm_10 = new FlagDef("gotCharm_10", null, false, "PlayerData_Bool", "Defender's Crest");
                public static readonly FlagDef gotCharm_11 = new FlagDef("gotCharm_11", null, false, "PlayerData_Bool", "Flukenest");
                public static readonly FlagDef gotCharm_12 = new FlagDef("gotCharm_12", null, false, "PlayerData_Bool", "Thorns of Agony");
                public static readonly FlagDef gotCharm_13 = new FlagDef("gotCharm_13", null, false, "PlayerData_Bool", "Mark of Pride");
                public static readonly FlagDef gotCharm_14 = new FlagDef("gotCharm_14", null, false, "PlayerData_Bool", "Steady Body");
                public static readonly FlagDef gotCharm_15 = new FlagDef("gotCharm_15", null, false, "PlayerData_Bool", "Heavy Blow");
                public static readonly FlagDef gotCharm_16 = new FlagDef("gotCharm_16", null, false, "PlayerData_Bool", "Sharp Shadow");
                public static readonly FlagDef gotCharm_17 = new FlagDef("gotCharm_17", null, false, "PlayerData_Bool", "Spore Shroom");
                public static readonly FlagDef gotCharm_18 = new FlagDef("gotCharm_18", null, false, "PlayerData_Bool", "Longnail");
                public static readonly FlagDef gotCharm_19 = new FlagDef("gotCharm_19", null, false, "PlayerData_Bool", "Shaman Stone");
                public static readonly FlagDef gotCharm_20 = new FlagDef("gotCharm_20", null, false, "PlayerData_Bool", "Soul Catcher");
                public static readonly FlagDef gotCharm_21 = new FlagDef("gotCharm_21", null, false, "PlayerData_Bool", "Soul Eater");
                public static readonly FlagDef gotCharm_22 = new FlagDef("gotCharm_22", null, false, "PlayerData_Bool", "Glowing Womb");
                public static readonly FlagDef gotCharm_23 = new FlagDef("gotCharm_23", null, false, "PlayerData_Bool", "Fragile Heart");
                public static readonly FlagDef gotCharm_24 = new FlagDef("gotCharm_24", null, false, "PlayerData_Bool", "Fragile Greed");
                public static readonly FlagDef gotCharm_25 = new FlagDef("gotCharm_25", null, false, "PlayerData_Bool", "Fragile Strength");
                public static readonly FlagDef gotCharm_26 = new FlagDef("gotCharm_26", null, false, "PlayerData_Bool", "Nailmaster's Glory");
                public static readonly FlagDef gotCharm_27 = new FlagDef("gotCharm_27", null, false, "PlayerData_Bool", "Joni's Blessing");
                public static readonly FlagDef gotCharm_28 = new FlagDef("gotCharm_28", null, false, "PlayerData_Bool", "Shape of Unn");
                public static readonly FlagDef gotCharm_29 = new FlagDef("gotCharm_29", null, false, "PlayerData_Bool", "Hiveblood");
                public static readonly FlagDef gotCharm_30 = new FlagDef("gotCharm_30", null, false, "PlayerData_Bool", "Dream Wielder");
                public static readonly FlagDef gotCharm_31 = new FlagDef("gotCharm_31", null, false, "PlayerData_Bool", "Dashmaster");
                public static readonly FlagDef gotCharm_32 = new FlagDef("gotCharm_32", null, false, "PlayerData_Bool", "Quickslash");
                public static readonly FlagDef gotCharm_33 = new FlagDef("gotCharm_33", null, false, "PlayerData_Bool", "Spell Twister");
                public static readonly FlagDef gotCharm_34 = new FlagDef("gotCharm_34", null, false, "PlayerData_Bool", "Deep Focus");
                public static readonly FlagDef gotCharm_35 = new FlagDef("gotCharm_35", null, false, "PlayerData_Bool", "Grubberfly's Elegy");
                public static readonly FlagDef gotCharm_36 = new FlagDef("gotCharm_36", null, false, "PlayerData_Bool", "Kingsoul / Void Heart");
                public static readonly FlagDef gotCharm_37 = new FlagDef("gotCharm_37", null, false, "PlayerData_Bool", "Sprintmaster");
                public static readonly FlagDef gotCharm_38 = new FlagDef("gotCharm_38", null, false, "PlayerData_Bool", "Dreamshield");
                public static readonly FlagDef gotCharm_39 = new FlagDef("gotCharm_39", null, false, "PlayerData_Bool", "Weaversong");
                public static readonly FlagDef gotCharm_40 = new FlagDef("gotCharm_40", null, false, "PlayerData_Bool", "Grimmchild / Carefree Melody");
            #endregion

            #region Charm Cost
                public static readonly FlagDef charmCost_1 = new FlagDef("charmCost_1", null, false, "PlayerData_Int", gotCharm_1.ReadableName + " Cost");
                public static readonly FlagDef charmCost_2 = new FlagDef("charmCost_2", null, false, "PlayerData_Int", gotCharm_2.ReadableName + " Cost");
                public static readonly FlagDef charmCost_3 = new FlagDef("charmCost_3", null, false, "PlayerData_Int", gotCharm_3.ReadableName + " Cost");
                public static readonly FlagDef charmCost_4 = new FlagDef("charmCost_4", null, false, "PlayerData_Int", gotCharm_4.ReadableName + " Cost");
                public static readonly FlagDef charmCost_5 = new FlagDef("charmCost_5", null, false, "PlayerData_Int", gotCharm_5.ReadableName + " Cost");
                public static readonly FlagDef charmCost_6 = new FlagDef("charmCost_6", null, false, "PlayerData_Int", gotCharm_6.ReadableName + " Cost");
                public static readonly FlagDef charmCost_7 = new FlagDef("charmCost_7", null, false, "PlayerData_Int", gotCharm_7.ReadableName + " Cost");
                public static readonly FlagDef charmCost_8 = new FlagDef("charmCost_8", null, false, "PlayerData_Int", gotCharm_8.ReadableName + " Cost");
                public static readonly FlagDef charmCost_9 = new FlagDef("charmCost_9", null, false, "PlayerData_Int", gotCharm_9.ReadableName + " Cost");
                public static readonly FlagDef charmCost_10 = new FlagDef("charmCost_10", null, false, "PlayerData_Int", gotCharm_10.ReadableName + " Cost");
                public static readonly FlagDef charmCost_11 = new FlagDef("charmCost_11", null, false, "PlayerData_Int", gotCharm_11.ReadableName + " Cost");
                public static readonly FlagDef charmCost_12 = new FlagDef("charmCost_12", null, false, "PlayerData_Int", gotCharm_12.ReadableName + " Cost");
                public static readonly FlagDef charmCost_13 = new FlagDef("charmCost_13", null, false, "PlayerData_Int", gotCharm_13.ReadableName + " Cost");
                public static readonly FlagDef charmCost_14 = new FlagDef("charmCost_14", null, false, "PlayerData_Int", gotCharm_14.ReadableName + " Cost");
                public static readonly FlagDef charmCost_15 = new FlagDef("charmCost_15", null, false, "PlayerData_Int", gotCharm_15.ReadableName + " Cost");
                public static readonly FlagDef charmCost_16 = new FlagDef("charmCost_16", null, false, "PlayerData_Int", gotCharm_16.ReadableName + " Cost");
                public static readonly FlagDef charmCost_17 = new FlagDef("charmCost_17", null, false, "PlayerData_Int", gotCharm_17.ReadableName + " Cost");
                public static readonly FlagDef charmCost_18 = new FlagDef("charmCost_18", null, false, "PlayerData_Int", gotCharm_18.ReadableName + " Cost");
                public static readonly FlagDef charmCost_19 = new FlagDef("charmCost_19", null, false, "PlayerData_Int", gotCharm_19.ReadableName + " Cost");
                public static readonly FlagDef charmCost_20 = new FlagDef("charmCost_20", null, false, "PlayerData_Int", gotCharm_20.ReadableName + " Cost");
                public static readonly FlagDef charmCost_21 = new FlagDef("charmCost_21", null, false, "PlayerData_Int", gotCharm_21.ReadableName + " Cost");
                public static readonly FlagDef charmCost_22 = new FlagDef("charmCost_22", null, false, "PlayerData_Int", gotCharm_22.ReadableName + " Cost");
                public static readonly FlagDef charmCost_23 = new FlagDef("charmCost_23", null, false, "PlayerData_Int", gotCharm_23.ReadableName + " Cost");
                public static readonly FlagDef charmCost_24 = new FlagDef("charmCost_24", null, false, "PlayerData_Int", gotCharm_24.ReadableName + " Cost");
                public static readonly FlagDef charmCost_25 = new FlagDef("charmCost_25", null, false, "PlayerData_Int", gotCharm_25.ReadableName + " Cost");
                public static readonly FlagDef charmCost_26 = new FlagDef("charmCost_26", null, false, "PlayerData_Int", gotCharm_26.ReadableName + " Cost");
                public static readonly FlagDef charmCost_27 = new FlagDef("charmCost_27", null, false, "PlayerData_Int", gotCharm_27.ReadableName + " Cost");
                public static readonly FlagDef charmCost_28 = new FlagDef("charmCost_28", null, false, "PlayerData_Int", gotCharm_28.ReadableName + " Cost");
                public static readonly FlagDef charmCost_29 = new FlagDef("charmCost_29", null, false, "PlayerData_Int", gotCharm_29.ReadableName + " Cost");
                public static readonly FlagDef charmCost_30 = new FlagDef("charmCost_30", null, false, "PlayerData_Int", gotCharm_30.ReadableName + " Cost");
                public static readonly FlagDef charmCost_31 = new FlagDef("charmCost_31", null, false, "PlayerData_Int", gotCharm_31.ReadableName + " Cost");
                public static readonly FlagDef charmCost_32 = new FlagDef("charmCost_32", null, false, "PlayerData_Int", gotCharm_32.ReadableName + " Cost");
                public static readonly FlagDef charmCost_33 = new FlagDef("charmCost_33", null, false, "PlayerData_Int", gotCharm_33.ReadableName + " Cost");
                public static readonly FlagDef charmCost_34 = new FlagDef("charmCost_34", null, false, "PlayerData_Int", gotCharm_34.ReadableName + " Cost");
                public static readonly FlagDef charmCost_35 = new FlagDef("charmCost_35", null, false, "PlayerData_Int", gotCharm_35.ReadableName + " Cost");
                public static readonly FlagDef charmCost_36 = new FlagDef("charmCost_36", null, false, "PlayerData_Int", gotCharm_36.ReadableName + " Cost");
                public static readonly FlagDef charmCost_37 = new FlagDef("charmCost_37", null, false, "PlayerData_Int", gotCharm_37.ReadableName + " Cost");
                public static readonly FlagDef charmCost_38 = new FlagDef("charmCost_38", null, false, "PlayerData_Int", gotCharm_38.ReadableName + " Cost");
                public static readonly FlagDef charmCost_39 = new FlagDef("charmCost_39", null, false, "PlayerData_Int", gotCharm_39.ReadableName + " Cost");
                public static readonly FlagDef charmCost_40 = new FlagDef("charmCost_40", null, false, "PlayerData_Int", gotCharm_40.ReadableName + " Cost");
            #endregion

            #region Charm Equipped
                public static readonly FlagDef equippedCharm_1 = new FlagDef("equippedCharm_1", null, false, "PlayerData_Bool", gotCharm_1.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_2 = new FlagDef("equippedCharm_2", null, false, "PlayerData_Bool", gotCharm_2.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_3 = new FlagDef("equippedCharm_3", null, false, "PlayerData_Bool", gotCharm_3.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_4 = new FlagDef("equippedCharm_4", null, false, "PlayerData_Bool", gotCharm_4.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_5 = new FlagDef("equippedCharm_5", null, false, "PlayerData_Bool", gotCharm_5.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_6 = new FlagDef("equippedCharm_6", null, false, "PlayerData_Bool", gotCharm_6.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_7 = new FlagDef("equippedCharm_7", null, false, "PlayerData_Bool", gotCharm_7.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_8 = new FlagDef("equippedCharm_8", null, false, "PlayerData_Bool", gotCharm_8.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_9 = new FlagDef("equippedCharm_9", null, false, "PlayerData_Bool", gotCharm_9.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_10 = new FlagDef("equippedCharm_10", null, false, "PlayerData_Bool", gotCharm_10.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_11 = new FlagDef("equippedCharm_11", null, false, "PlayerData_Bool", gotCharm_11.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_12 = new FlagDef("equippedCharm_12", null, false, "PlayerData_Bool", gotCharm_12.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_13 = new FlagDef("equippedCharm_13", null, false, "PlayerData_Bool", gotCharm_13.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_14 = new FlagDef("equippedCharm_14", null, false, "PlayerData_Bool", gotCharm_14.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_15 = new FlagDef("equippedCharm_15", null, false, "PlayerData_Bool", gotCharm_15.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_16 = new FlagDef("equippedCharm_16", null, false, "PlayerData_Bool", gotCharm_16.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_17 = new FlagDef("equippedCharm_17", null, false, "PlayerData_Bool", gotCharm_17.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_18 = new FlagDef("equippedCharm_18", null, false, "PlayerData_Bool", gotCharm_18.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_19 = new FlagDef("equippedCharm_19", null, false, "PlayerData_Bool", gotCharm_19.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_20 = new FlagDef("equippedCharm_20", null, false, "PlayerData_Bool", gotCharm_20.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_21 = new FlagDef("equippedCharm_21", null, false, "PlayerData_Bool", gotCharm_21.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_22 = new FlagDef("equippedCharm_22", null, false, "PlayerData_Bool", gotCharm_22.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_23 = new FlagDef("equippedCharm_23", null, false, "PlayerData_Bool", gotCharm_23.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_24 = new FlagDef("equippedCharm_24", null, false, "PlayerData_Bool", gotCharm_24.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_25 = new FlagDef("equippedCharm_25", null, false, "PlayerData_Bool", gotCharm_25.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_26 = new FlagDef("equippedCharm_26", null, false, "PlayerData_Bool", gotCharm_26.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_27 = new FlagDef("equippedCharm_27", null, false, "PlayerData_Bool", gotCharm_27.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_28 = new FlagDef("equippedCharm_28", null, false, "PlayerData_Bool", gotCharm_28.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_29 = new FlagDef("equippedCharm_29", null, false, "PlayerData_Bool", gotCharm_29.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_30 = new FlagDef("equippedCharm_30", null, false, "PlayerData_Bool", gotCharm_30.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_31 = new FlagDef("equippedCharm_31", null, false, "PlayerData_Bool", gotCharm_31.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_32 = new FlagDef("equippedCharm_32", null, false, "PlayerData_Bool", gotCharm_32.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_33 = new FlagDef("equippedCharm_33", null, false, "PlayerData_Bool", gotCharm_33.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_34 = new FlagDef("equippedCharm_34", null, false, "PlayerData_Bool", gotCharm_34.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_35 = new FlagDef("equippedCharm_35", null, false, "PlayerData_Bool", gotCharm_35.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_36 = new FlagDef("equippedCharm_36", null, false, "PlayerData_Bool", gotCharm_36.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_37 = new FlagDef("equippedCharm_37", null, false, "PlayerData_Bool", gotCharm_37.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_38 = new FlagDef("equippedCharm_38", null, false, "PlayerData_Bool", gotCharm_38.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_39 = new FlagDef("equippedCharm_39", null, false, "PlayerData_Bool", gotCharm_39.ReadableName + " Equipped");
                public static readonly FlagDef equippedCharm_40 = new FlagDef("equippedCharm_40", null, false, "PlayerData_Bool", gotCharm_40.ReadableName + " Equipped");
            #endregion

            #region Charm Status
                public static readonly FlagDef brokenCharm_23 = new FlagDef("brokenCharm_23", null, false, "PlayerData_Bool", gotCharm_23.ReadableName + " Broken");
                public static readonly FlagDef brokenCharm_24 = new FlagDef("brokenCharm_24", null, false, "PlayerData_Bool", gotCharm_24.ReadableName + " Broken");
                public static readonly FlagDef brokenCharm_25 = new FlagDef("brokenCharm_25", null, false, "PlayerData_Bool", gotCharm_25.ReadableName + " Broken");
                public static readonly FlagDef canOvercharm = new FlagDef("canOvercharm", null, false, "PlayerData_Bool");
                public static readonly FlagDef fragileGreed_unbreakable = new FlagDef("fragileGreed_unbreakable", null, false, "PlayerData_Bool", gotCharm_24.ReadableName + " Unbreakable");
                public static readonly FlagDef fragileHealth_unbreakable = new FlagDef("fragileHealth_unbreakable", null, false, "PlayerData_Bool", gotCharm_23.ReadableName + " Unbreakable");
                public static readonly FlagDef fragileStrength_unbreakable = new FlagDef("fragileStrength_unbreakable", null, false, "PlayerData_Bool", gotCharm_25.ReadableName + " Unbreakable");
                public static readonly FlagDef gaveFragileGreed = new FlagDef("gaveFragileGreed", null, false, "PlayerData_Bool", "Divine has Greed charm");
                public static readonly FlagDef gaveFragileHeart = new FlagDef("gaveFragileHeart", null, false, "PlayerData_Bool", "Divine has Heart charm");
                public static readonly FlagDef gaveFragileStrength = new FlagDef("gaveFragileStrength", null, false, "PlayerData_Bool", "Divine has Strength charm");
                // Requires royalCharmState = 4
                public static readonly FlagDef gotShadeCharm = new FlagDef("gotShadeCharm", null, false, "PlayerData_Bool");
                public static readonly FlagDef grimmChildLevel = new FlagDef("grimmChildLevel", null, false, "PlayerData_Int");
                public static readonly FlagDef grimmchildAwoken = new FlagDef("grimmchildAwoken", null, false, "PlayerData_Bool");
                public static readonly FlagDef pooedFragileGreed = new FlagDef("pooedFragileGreed", null, false, "PlayerData_Bool", "Divine pooed Greed charm");
                public static readonly FlagDef pooedFragileHeart = new FlagDef("pooedFragileHeart", null, false, "PlayerData_Bool", "Divine pooed Heart charm");
                public static readonly FlagDef pooedFragileStrength = new FlagDef("pooedFragileStrength", null, false, "PlayerData_Bool", "Divine pooed Strength charm");
                // 0 = missing, 1|2 = parts, 3 = Kingsoul, 4: Void Heart and gotShadeCharm = true, else false
                public static readonly FlagDef royalCharmState = new FlagDef("royalCharmState", null, false, "PlayerData_Int", "Kingsoul state"); 
            #endregion

        #endregion

        #region Currency
            public static readonly FlagDef dreamOrbs =  new FlagDef("dreamOrbs",    null, false, "PlayerData_Int", "Dream Essence (0-9999)");
            public static readonly FlagDef geo =        new FlagDef("geo",          null, false, "PlayerData_Int", "Geo (0-999999)");
        #endregion

        #region In-Game Cheats
            public static readonly FlagDef infiniteAirJump =    new FlagDef("infiniteAirJump",  null, false, "PlayerData_Bool", "Infinite Air Jump");
            public static readonly FlagDef isInvincible =       new FlagDef("isInvincible",     null, false, "PlayerData_Bool", "Invulnerability");
        #endregion

        #region Environment Updates
            public static readonly FlagDef aladarPinned = new FlagDef("aladarPinned", null, false, "PlayerData_Bool", "Gorb pinned");
            public static readonly FlagDef bankerAccountPurchased = new FlagDef("bankerAccountPurchased", null, false, "PlayerData_Bool", "Opened Bank Account");
            public static readonly FlagDef bankerBalance = new FlagDef("bankerBalance", null, false, "PlayerData_Int", "Balance stored in bank");
            public static readonly FlagDef crossroadsInfected = new FlagDef("crossroadsInfected", null, false, "PlayerData_Bool", "Crossroads Infected");
            public static readonly FlagDef crossroadsMawlekWall = new FlagDef("crossroadsMawlekWall", null, false, "PlayerData_Bool", "Brooding Mawlek Wall opened");
            public static readonly FlagDef deepnestBridgeCollapsed = new FlagDef("deepnestBridgeCollapsed", null, false, "PlayerData_Bool", "Deepnest bridge collapsed");
            public static readonly FlagDef deepnestWall = new FlagDef("deepnestWall", null, false, "PlayerData_Bool", "Opened wall between Fungus2 and Deepnest");
            public static readonly FlagDef defeatedDoubleBlockers = new FlagDef("defeatedDoubleBlockers", null, false, "PlayerData_Bool", "Defeated double blockers in " + SceneInstances.Fungus1_28.ReadableName);
            public static readonly FlagDef galienPinned = new FlagDef("galienPinned", null, false, "PlayerData_Bool", "Warrior Galien pinned on map");
            public static readonly FlagDef huPinned = new FlagDef("huPinned", null, false, "PlayerData_Bool", "Warrior Elder Hu pinned");
            public static readonly FlagDef jijiDoorUnlocked = new FlagDef("jijiDoorUnlocked", null, false, "PlayerData_Bool", "Jiji's door opened");
            public static readonly FlagDef megaMossChargerDefeated = new FlagDef("megaMossChargerDefeated", null, false, "PlayerData_Bool", "Massive Moss Charger defeated");
            public static readonly FlagDef menderSignBroken = new FlagDef("menderSignBroken", null, false, "PlayerData_Bool", "Broke Mender sign");
            public static readonly FlagDef menderState = new FlagDef("menderState", null, false, "PlayerData_Int", "Mender is aware of broken sign (0-1?)");
            public static readonly FlagDef nightmareLanternAppeared = new FlagDef("nightmareLanternAppeared", null, false, "PlayerData_Bool", "Nightmare Lantern appeared");
            public static readonly FlagDef nightmareLanternLit = new FlagDef("nightmareLanternLit", null, false, "PlayerData_Bool", "Nightmare Lantern lit");
            public static readonly FlagDef noEyesPinned = new FlagDef("noEyesPinned", null, false, "PlayerData_Bool", "Warrior No Eyes pinned");
            public static readonly FlagDef openedMapperShop = new FlagDef("openedMapperShop", null, false, "PlayerData_Bool", "Map Shop opened");
            public static readonly FlagDef openedTownBuilding = new FlagDef("openedTownBuilding", null, false, "PlayerData_Bool", "Town Stag Station building opened");
            public static readonly FlagDef openedTramLower = new FlagDef("openedTramLower", null, false, "PlayerData_Bool", "Opened lower tram");
            public static readonly FlagDef queensStationNonDisplay = new FlagDef("queensStationNonDisplay", null, false, "PlayerData_Bool", "???");
            public static readonly FlagDef shamanPillar = new FlagDef("shamanPillar", null, false, "PlayerData_Bool", "Snail Shaman door to Crossroads west opened");
            public static readonly FlagDef spiderCapture = new FlagDef("spiderCapture", null, false, "PlayerData_Bool", "Captured by spiders in Distant Village");
            public static readonly FlagDef steppedBeyondBridge = new FlagDef("steppedBeyondBridge", null, false, "PlayerData_Bool", "Made it past the collapsing bridge to Deepnest");
            public static readonly FlagDef xeroPinned = new FlagDef("xeroPinned", null, false, "PlayerData_Bool", "Warrior Xero pinned on map");

            #region Categorized Scene Flags
                #region Abyss
                    // Abyss_17
                    public static readonly FlagDef Abyss_17__One_Way_Wall = new FlagDef("One Way Wall", SceneInstances.Abyss_17, false, "PersistentBoolData", "Opened wall back to entrance");
                    public static readonly FlagDef Abyss_17__Battle_Scene_Ore = new FlagDef("Battle Scene Ore", SceneInstances.Abyss_17, false, "PersistentBoolData", "Lesser Mawlek battle before pale ore");

                    // Abyss_18
                    public static readonly FlagDef Abyss_18__Toll_Machine_Bench = new FlagDef("Toll Machine Bench", SceneInstances.Abyss_18, false, "PersistentBoolData", "Toll bench opened");
                #endregion

                #region Cliffs
                    // Cliffs_01
                    public static readonly FlagDef Cliffs_01__Breakable_Wall = new FlagDef("Breakable Wall", SceneInstances.Cliffs_01, false, "PersistentBoolData", "Open wall to Wanderer's Journal");
                    public static readonly FlagDef Cliffs_01__Breakable_Wall_grimm = new FlagDef("Breakable Wall grimm", SceneInstances.Cliffs_01, false, "PersistentBoolData", "Open wall to Nightmare Lantern");
                    public static readonly FlagDef Cliffs_01__One_Way_Wall = new FlagDef("One Way Wall", SceneInstances.Cliffs_01, false, "PersistentBoolData", "Broke wall back out from Nightmare Lantern");

                    // Cliffs_05
                    public static readonly FlagDef Cliffs_05__Ghost_NPC_Joni = new FlagDef("Ghost NPC Joni", SceneInstances.Cliffs_05, false, "PersistentBoolData", "Claimed Joni's spirit");
                #endregion

                #region Crossroads
                    // Crossroads_03
                    public static readonly FlagDef Crossroads_03__Break_Wall_2 = new FlagDef("Break Wall 2", SceneInstances.Crossroads_03, false, "PersistentBoolData", "Break wall to Grub");
                    public static readonly FlagDef Crossroads_03__Toll_Gate_Switch = new FlagDef("Toll Gate Switch", SceneInstances.Crossroads_03, false, "PersistentBoolData", "Open backtrack gate");

                    // Crossroads_04
                    public static readonly FlagDef Crossroads_04__Battle_Scene = new FlagDef("Battle Scene", SceneInstances.Crossroads_04, false, "PersistentBoolData", "Gruz Mother Defeated");
                    public static readonly FlagDef Crossroads_04__Break_Floor_1 = new FlagDef("Break Floor 1", SceneInstances.Crossroads_04, false, "PersistentBoolData", "Opened floor back to entrance");

                    // Crossroads_06
                    public static readonly FlagDef Crossroads_06__Gate_Switch = new FlagDef("Gate Switch", SceneInstances.Crossroads_06, false, "PersistentBoolData", "Opened path back to West Crossroads");

                    // Crossroads_07
                    public static readonly FlagDef Crossroads_07__Tute_Door_1 = new FlagDef("Tute Door 1", SceneInstances.Crossroads_07, false, "PersistentBoolData", "Broke wall to Brooding Mawlek");

                    // Crossroads_08
                    public static readonly FlagDef Crossroads_08__Battle_Scene = new FlagDef("Battle Scene", SceneInstances.Crossroads_08, false, "PersistentBoolData", "Completed Spitter Battle");
                    public static readonly FlagDef Crossroads_08__Break_Wall_2 = new FlagDef("Break Wall 2", SceneInstances.Crossroads_08, false, "PersistentBoolData", "Broke wall to Geo Rock");

                    // Crossroads_09
                    public static readonly FlagDef Crossroads_09__Break_Floor_1 = new FlagDef("Break Floor 1", SceneInstances.Crossroads_09, false, "PersistentBoolData", "Broke one-way wall to exit");

                    // Crossroads_10
                    public static readonly FlagDef Crossroads_10__Breakable_Wall = new FlagDef("Breakable Wall", SceneInstances.Crossroads_10, false, "PersistentBoolData", "Broke wall to Maggots");
                    public static readonly FlagDef Crossroads_10__Chest = new FlagDef("Chest", SceneInstances.Crossroads_10, false, "PersistentBoolData", "Opened Geo Chest");

                    // Crossroads_11_alt
                    public static readonly FlagDef Crossroads_11_alt__Blocker = new FlagDef("Blocker", SceneInstances.Crossroads_11_alt, false, "PersistentBoolData", "Blocker killed");

                    // Crossroads_13
                    public static readonly FlagDef Crossroads_13__Break_Floor_1 = new FlagDef("Break Floor 1", SceneInstances.Crossroads_13, false, "PersistentBoolData", "Break wall back from Mask Shard secret");

                    // Crossroads_21
                    public static readonly FlagDef Crossroads_21__Breakable_Wall = new FlagDef("Breakable Wall", SceneInstances.Crossroads_21, false, "PersistentBoolData", "Break wall to Glowing Womb");

                    // Crossroads_22
                    public static readonly FlagDef Crossroads_22__Battle_Scene = new FlagDef("Battle Scene", SceneInstances.Crossroads_22, false, "PersistentBoolData", "Hatcher Battle");

                    // Crossroads_36
                    public static readonly FlagDef Crossroads_36__Collapser_Small = new FlagDef("Collapser Small", SceneInstances.Crossroads_36, false, "PersistentBoolData", "Broke collapsing floor to Brooding Mawlek");
                    public static readonly FlagDef Crossroads_36__Collapser_Small_1 = new FlagDef("Collapser Small 1", SceneInstances.Crossroads_36, false, "PersistentBoolData", "Broke backtrack collapsing floor");

                    // Crossroads_ShamanTemple
                    public static readonly FlagDef Crossroads_ShamanTemple__Blocker = new FlagDef("Blocker", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData", "Blocker killed");
                    public static readonly FlagDef Crossroads_ShamanTemple__Bone_Gate = new FlagDef("Bone Gate", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData", "Opened gate to return to entrance");
                #endregion

                #region Deepnest
                    // Deepnest_01
                    public static readonly FlagDef Deepnest_01__Fungus_Break_Floor = new FlagDef("Fungus Break Floor", SceneInstances.Deepnest_01, false, "PersistentBoolData", "Deepnest bridge collapsed");
                    public static readonly FlagDef Deepnest_01__One_Way_Wall = new FlagDef("One Way Wall", SceneInstances.Deepnest_01, false, "PersistentBoolData", "Opened wall back to topside");

                    // Deepnest_01b
                    public static readonly FlagDef Deepnest_01b__One_Way_Wall = new FlagDef("One Way Wall", SceneInstances.Deepnest_01b, false, "PersistentBoolData", "Broke wall to go back topside");

                    // Deepnest_02
                    public static readonly FlagDef Deepnest_02__Breakable_Wall = new FlagDef("Breakable Wall", SceneInstances.Deepnest_02, false, "PersistentBoolData", "Opened wall to grubs");
                    public static readonly FlagDef Deepnest_02__Collapser_Small = new FlagDef("Collapser Small", SceneInstances.Deepnest_02, false, "PersistentBoolData", "Trap floor collapsed");

                    // Deepnest_03
                    public static readonly FlagDef Deepnest_03__Collapser_Small = new FlagDef("Collapser Small", SceneInstances.Deepnest_03, false, "PersistentBoolData", "Collapsed floor trap opened");

                    // Deepnest_14
                    public static readonly FlagDef Deepnest_14__Collapser_Small = new FlagDef("Collapser Small", SceneInstances.Deepnest_14, false, "PersistentBoolData", "Collapsed floor open back to " + SceneInstances.Deepnest_01b.ReadableName);

                    // Deepnest_16
                    public static readonly FlagDef Deepnest_16__Collapser_Small = new FlagDef("Collapser Small", SceneInstances.Deepnest_16, false, "PersistentBoolData", "Collapsible floor trap");
                    public static readonly FlagDef Deepnest_16__Collapser_Small_1 = new FlagDef("Collapser Small (1)", SceneInstances.Deepnest_16, false, "PersistentBoolData", "Collapsible floor opened back to Deepnest side");

                    // Deepnest_26
                    public static readonly FlagDef Deepnest_26__Ruins_Lever = new FlagDef("Ruins Lever", SceneInstances.Deepnest_26, false, "PersistentBoolData", "Opened gate to Ruins");

                    // Deepnest_26b
                    public static readonly FlagDef Deepnest_26b__Ruins_Lever_Remade = new FlagDef("Ruins Lever Remade", SceneInstances.Deepnest_26b, false, "PersistentBoolData", "Open gate back to " + SceneInstances.Deepnest_26.ReadableName);

                    // Deepnest_30
                    public static readonly FlagDef Deepnest_30__Collapser_Small = new FlagDef("Collapser Small", SceneInstances.Deepnest_30, false, "PersistentBoolData", "Collapsed secret floor");
                    public static readonly FlagDef Deepnest_30__Collapser_Small_1 = new FlagDef("Collapser Small (1)", SceneInstances.Deepnest_30, false, "PersistentBoolData", "Collapsed floor trap 1");
                    public static readonly FlagDef Deepnest_30__Collapser_Small_2 = new FlagDef("Collapser Small (2)", SceneInstances.Deepnest_30, false, "PersistentBoolData", "Collapsed floor progress 2");
                    public static readonly FlagDef Deepnest_30__Collapser_Small_3 = new FlagDef("Collapser Small (3)", SceneInstances.Deepnest_30, false, "PersistentBoolData", "Collapsed floor progress 1");
                    public static readonly FlagDef Deepnest_30__Collapser_Small_top = new FlagDef("Collapser Small top", SceneInstances.Deepnest_30, false, "PersistentBoolData", "Collapsed floor trap 1");

                    // Deepnest_33
                    public static readonly FlagDef Deepnest_33__Battle_Scene_v2 = new FlagDef("Battle Scene v2", SceneInstances.Deepnest_33, false, "PersistentBoolData", "Corpse Creeper battle");
                    public static readonly FlagDef Deepnest_33__Collapser_Small = new FlagDef("Collapser Small", SceneInstances.Deepnest_33, false, "PersistentBoolData", "Collapsed floor to Zote");

                    // Deepnest_38
                    public static readonly FlagDef Deepnest_38__Collapser_Small = new FlagDef("Collapser Small", SceneInstances.Deepnest_38, false, "PersistentBoolData", "Opened floor back to entrance");
                    public static readonly FlagDef Deepnest_38__Breakable_Wall = new FlagDef("Breakable Wall", SceneInstances.Deepnest_38, false, "PersistentBoolData", "Broke wall to soul totem");

                    // Deepnest_39
                    public static readonly FlagDef Deepnest_39__Collapser_Small = new FlagDef("Collapser Small", SceneInstances.Deepnest_39, false, "PersistentBoolData", "Collapsible floor trap 3");
                    public static readonly FlagDef Deepnest_39__Collapser_Small_1 = new FlagDef("Collapser Small (1)", SceneInstances.Deepnest_39, false, "PersistentBoolData", "Collapsible floor trap 2");
                    public static readonly FlagDef Deepnest_39__Collapser_Small_2 = new FlagDef("Collapser Small (2)", SceneInstances.Deepnest_39, false, "PersistentBoolData", "Collapsible floor trap 1");
                    public static readonly FlagDef Deepnest_39__Collapser_Small_3 = new FlagDef("Collapser Small (3)", SceneInstances.Deepnest_39, false, "PersistentBoolData", "Collapsible floor trap 4");
                    public static readonly FlagDef Deepnest_39__Collapser_Small_4 = new FlagDef("Collapser Small (4)", SceneInstances.Deepnest_39, false, "PersistentBoolData", "Collapsible floor trap 5");
                    public static readonly FlagDef Deepnest_39__Collapser_Small_5 = new FlagDef("Collapser Small (5)", SceneInstances.Deepnest_39, false, "PersistentBoolData", "Collapsible floor trap 6");
                    public static readonly FlagDef Deepnest_39__Collapser_Small_6 = new FlagDef("Collapser Small (6)", SceneInstances.Deepnest_39, false, "PersistentBoolData", "Collapsible floor trap 7");
                    public static readonly FlagDef Deepnest_39__Collapser_Small_7 = new FlagDef("Collapser Small (7)", SceneInstances.Deepnest_39, false, "PersistentBoolData", "Collapsible floor return from Grub");
                    public static readonly FlagDef Deepnest_39__One_Way_Wall = new FlagDef("One Way Wall", SceneInstances.Deepnest_39, false, "PersistentBoolData", "Broke floor back from geo rock secret");
                    public static readonly FlagDef Deepnest_39__Shiny_ItemClone = new FlagDef("Shiny Item(Clone)", SceneInstances.Deepnest_39, false, "PersistentBoolData", "Rancid Egg");
                    public static readonly FlagDef Deepnest_39__Egg_Sac = new FlagDef("Egg Sac", SceneInstances.Deepnest_39, false, "PersistentBoolData", "Egg sac");

                    // Deepnest_41
                    public static readonly FlagDef Deepnest_41__Collapser_Small = new FlagDef("Collapser Small", SceneInstances.Deepnest_41, false, "PersistentBoolData", "Collapsible floor back to entrance");
                    public static readonly FlagDef Deepnest_41__Collapser_Small_1 = new FlagDef("Collapser Small (1)", SceneInstances.Deepnest_41, false, "PersistentBoolData", "Collapsible floor trap 1");
                    public static readonly FlagDef Deepnest_41__Collapser_Small_2 = new FlagDef("Collapser Small (2)", SceneInstances.Deepnest_41, false, "PersistentBoolData", "Collapsible floor progression");
                    public static readonly FlagDef Deepnest_41__Collapser_Small_4 = new FlagDef("Collapser Small (4)", SceneInstances.Deepnest_41, false, "PersistentBoolData", "Collapsible floor trap 1");
                    public static readonly FlagDef Deepnest_41__One_Way_Wall = new FlagDef("One Way Wall", SceneInstances.Deepnest_41, false, "PersistentBoolData", "Break wall back to entrance 1");
                    public static readonly FlagDef Deepnest_41__One_Way_Wall_1 = new FlagDef("One Way Wall (1)", SceneInstances.Deepnest_41, false, "PersistentBoolData", "Break wall back to entrance 1");
                    public static readonly FlagDef Deepnest_41__Breakable_Wall = new FlagDef("Breakable Wall", SceneInstances.Deepnest_41, false, "PersistentBoolData", "Broke wall to Midwife");
                    public static readonly FlagDef Deepnest_41__One_Way_Wall_2 = new FlagDef("One Way Wall (2)", SceneInstances.Deepnest_41, false, "PersistentBoolData", "Opened floor back to topside");

                    // Deepnest_42
                    public static readonly FlagDef Deepnest_42__Plank_Solid_1 = new FlagDef("Plank Solid 1", SceneInstances.Deepnest_42, false, "PersistentBoolData");
                    public static readonly FlagDef Deepnest_42__Plank_Solid_1_1 = new FlagDef("Plank Solid 1 (1)", SceneInstances.Deepnest_42, false, "PersistentBoolData");
                    public static readonly FlagDef Deepnest_42__Plank_Solid_1_2 = new FlagDef("Plank Solid 1 (2)", SceneInstances.Deepnest_42, false, "PersistentBoolData");

                    // Deepnest_44
                    public static readonly FlagDef Deepnest_44__One_Way_Wall = new FlagDef("One Way Wall", SceneInstances.Deepnest_44, false, "PersistentBoolData");
                    public static readonly FlagDef Deepnest_44__Shiny_Item_Stand = new FlagDef("Shiny Item Stand", SceneInstances.Deepnest_44, false, "PersistentBoolData");

                    // Deepnest_Spider_Town
                    public static readonly FlagDef Deepnest_Spider_Town__Collapser_Small_10 = new FlagDef("Collapser Small (10)", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData", "Collapsible floor out from secrets");
                    public static readonly FlagDef Deepnest_Spider_Town__Collapser_Small_11 = new FlagDef("Collapser Small (11)", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData", "Collapsible floor to exit");
                    public static readonly FlagDef Deepnest_Spider_Town__Collapser_Small_3 = new FlagDef("Collapser Small (3)", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData", "Collapsible floor trap 1");
                    public static readonly FlagDef Deepnest_Spider_Town__Collapser_Small_5 = new FlagDef("Collapser Small (5)", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData", "Collapsible floor trap 2");
                    public static readonly FlagDef Deepnest_Spider_Town__Collapser_Small_6 = new FlagDef("Collapser Small (6)", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData", "Collapsible floor to nowhere 2");
                    public static readonly FlagDef Deepnest_Spider_Town__Collapser_Small_7 = new FlagDef("Collapser Small (7)", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData", "Open floor to Geo Rocks");
                    public static readonly FlagDef Deepnest_Spider_Town__Collapser_Small_8 = new FlagDef("Collapser Small (8)", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData", "Collapsible floor to nowhere 1");
                    public static readonly FlagDef Deepnest_Spider_Town__Collapser_Small_9 = new FlagDef("Collapser Small (9)", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData", "Collapsible floor to secrets");
                    public static readonly FlagDef Deepnest_Spider_Town__Egg_Sac = new FlagDef("Egg Sac", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData", "Rancid Egg Sack");
                    public static readonly FlagDef Deepnest_Spider_Town__Shiny_ItemClone = new FlagDef("Shiny Item(Clone)", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData", "Rancid Egg");
                #endregion

                #region Deepnest East
                    
                #endregion

                #region Fungus 1
                    // Fungus1_04
                    public static readonly FlagDef Fungus1_04__Break_Floor_1 = new FlagDef("Break Floor 1", SceneInstances.Fungus1_04, false, "PersistentBoolData", "Broke floor back to entrance");

                    // Fungus1_06
                    public static readonly FlagDef Fungus1_06__Vine_Platform = new FlagDef("Vine Platform", SceneInstances.Fungus1_06, false, "PersistentBoolData");
                    public static readonly FlagDef Fungus1_06__Vine_Platform_1 = new FlagDef("Vine Platform (1)", SceneInstances.Fungus1_06, false, "PersistentBoolData");
                    public static readonly FlagDef Fungus1_06__Vine_Platform_2 = new FlagDef("Vine Platform (2)", SceneInstances.Fungus1_06, false, "PersistentBoolData");
                    public static readonly FlagDef Fungus1_06__Vine_Platform_3 = new FlagDef("Vine Platform (3)", SceneInstances.Fungus1_06, false, "PersistentBoolData");
                    public static readonly FlagDef Fungus1_06__Vine_Platform_4 = new FlagDef("Vine Platform (4)", SceneInstances.Fungus1_06, false, "PersistentBoolData");
                    public static readonly FlagDef Fungus1_06__Vine_Platform_5 = new FlagDef("Vine Platform (5)", SceneInstances.Fungus1_06, false, "PersistentBoolData");
                    public static readonly FlagDef Fungus1_06__Vine_Platform_6 = new FlagDef("Vine Platform (6)", SceneInstances.Fungus1_06, false, "PersistentBoolData");

                    // Fungus1_07
                    public static readonly FlagDef Fungus1_07__Vine_Platform = new FlagDef("Vine Platform", SceneInstances.Fungus1_07, false, "PersistentBoolData");
                    public static readonly FlagDef Fungus1_07__Vine_Platform_1 = new FlagDef("Vine Platform (1)", SceneInstances.Fungus1_07, false, "PersistentBoolData");
                    public static readonly FlagDef Fungus1_07__Vine_Platform_2 = new FlagDef("Vine Platform (2)", SceneInstances.Fungus1_07, false, "PersistentBoolData");
                    public static readonly FlagDef Fungus1_07__Vine_Platform_3 = new FlagDef("Vine Platform (3)", SceneInstances.Fungus1_07, false, "PersistentBoolData");

                    // Fungus1_09
                    public static readonly FlagDef Fungus1_09__Vine_Platform = new FlagDef("Vine Platform", SceneInstances.Fungus1_09, false, "PersistentBoolData");

                    // Fungus1_13
                    public static readonly FlagDef Fungus1_13__Chest = new FlagDef("Chest", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                    public static readonly FlagDef Fungus1_13__Vine_Platform = new FlagDef("Vine Platform", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                    public static readonly FlagDef Fungus1_13__Vine_Platform_1 = new FlagDef("Vine Platform (1)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                    public static readonly FlagDef Fungus1_13__Vine_Platform_2 = new FlagDef("Vine Platform (2)", SceneInstances.Fungus1_13, false, "PersistentBoolData");

                    // Fungus1_14
                    public static readonly FlagDef Fungus1_14__Vine_Platform = new FlagDef("Vine Platform", SceneInstances.Fungus1_14, false, "PersistentBoolData");

                    // Fungus1_21
                    public static readonly FlagDef Fungus1_21__Vine_Platform_2 = new FlagDef("Vine Platform (2)", SceneInstances.Fungus1_21, false, "PersistentBoolData");

                    // Fungus1_22
                    public static readonly FlagDef Fungus1_22__Breakable_Wall = new FlagDef("Breakable Wall", SceneInstances.Fungus1_22, false, "PersistentBoolData", "Break wall to Trinket");
                    public static readonly FlagDef Fungus1_22__Gate_Switch = new FlagDef("Gate Switch", SceneInstances.Fungus1_22, false, "PersistentBoolData", "Open gate to lower exit");
                    public static readonly FlagDef Fungus1_22__Vine_Platform = new FlagDef("Vine Platform", SceneInstances.Fungus1_22, false, "PersistentBoolData");
                    public static readonly FlagDef Fungus1_22__Vine_Platform_1 = new FlagDef("Vine Platform (1)", SceneInstances.Fungus1_22, false, "PersistentBoolData");

                    // Fungus1_28
                    public static readonly FlagDef Fungus1_28__Blocker_1 = new FlagDef("Blocker 1", SceneInstances.Fungus1_28, false, "PersistentBoolData", "Blocker 1 killed");
                    public static readonly FlagDef Fungus1_28__Blocker_2 = new FlagDef("Blocker 2", SceneInstances.Fungus1_28, false, "PersistentBoolData", "Blocker 2 killed");
                    public static readonly FlagDef Fungus1_28__Chest = new FlagDef("Chest", SceneInstances.Fungus1_28, false, "PersistentBoolData", "Opened chest");
                    public static readonly FlagDef Fungus1_28__One_Way_Wall = new FlagDef("One Way Wall", SceneInstances.Fungus1_28, false, "PersistentBoolData", "Open wall back to entrance");
                    public static readonly FlagDef Fungus1_28__One_Way_Wall_1 = new FlagDef("One Way Wall (1)", SceneInstances.Fungus1_28, false, "PersistentBoolData", "Open wall to upper area");

                    // Fungus1_31
                    public static readonly FlagDef Fungus1_31__Toll_Gate_Machine = new FlagDef("Toll Gate Machine", SceneInstances.Fungus1_31, false, "PersistentBoolData", "Opened gate to connect top and bottom of room");

                    // Fungus1_32
                    public static readonly FlagDef Fungus1_32__Battle_Scene_v2 = new FlagDef("Battle Scene v2", SceneInstances.Fungus1_32, false, "PersistentBoolData", "Moss Knight Battle");
                #endregion

                #region Fungus 2
                    // Fungus2_01
                    public static readonly FlagDef Fungus2_01__Ruins_Lever = new FlagDef("Ruins Lever", SceneInstances.Fungus2_01, false, "PersistentBoolData", "Open door to mask shard");

                    // Fungus2_04
                    public static readonly FlagDef Fungus2_04__Mantis_Lever = new FlagDef("Mantis Lever", SceneInstances.Fungus2_04, false, "PersistentBoolData", "Open door to Wanderer's Journal");
                    public static readonly FlagDef Fungus2_04__Mantis_Lever_1 = new FlagDef("Mantis Lever (1)", SceneInstances.Fungus2_04, false, "PersistentBoolData", "Opened upper gate");

                    // Fungus2_05
                    public static readonly FlagDef Fungus2_05__Battle_Scene_v2 = new FlagDef("Battle Scene v2", SceneInstances.Fungus2_05, false, "PersistentBoolData", "Shrumal Ogre battle");

                    // Fungus2_10
                    public static readonly FlagDef Fungus2_10__One_Way_Wall = new FlagDef("One Way Wall", SceneInstances.Fungus2_10, false, "PersistentBoolData", "Broke wall back to entrance");

                    // Fungus2_11
                    public static readonly FlagDef Fungus2_18__Mantis_Lever = new FlagDef("Mantis Lever", SceneInstances.Fungus2_18, false, "PersistentBoolData", "Opened door");

                    // Fungus2_14
                    public static readonly FlagDef Fungus2_14__Mantis_Lever = new FlagDef("Mantis Lever", SceneInstances.Fungus2_14, false, "PersistentBoolData", "Opened door to Mantis Claw");
                    public static readonly FlagDef Fungus2_14__Mantis_Lever_1 = new FlagDef("Mantis Lever (1)", SceneInstances.Fungus2_14, false, "PersistentBoolData", "Opened door back to entrance");

                    // Fungus2_15
                    public static readonly FlagDef Fungus2_15__Mantis_Lever = new FlagDef("Mantis Lever", SceneInstances.Fungus2_15, false, "PersistentBoolData", "Open door back to top");
                    public static readonly FlagDef Fungus2_15__Mantis_Lever_1 = new FlagDef("Mantis Lever (1)", SceneInstances.Fungus2_15, false, "PersistentBoolData", "Open left door second from top");
                    public static readonly FlagDef Fungus2_15__Mantis_Lever_2 = new FlagDef("Mantis Lever (2)", SceneInstances.Fungus2_15, false, "PersistentBoolData", "Open left door third from top");
                    public static readonly FlagDef Fungus2_15__Mantis_Lever_3 = new FlagDef("Mantis Lever (3)", SceneInstances.Fungus2_15, false, "PersistentBoolData", "Open right door second from top");
                    public static readonly FlagDef Fungus2_15__Mantis_Lever_4 = new FlagDef("Mantis Lever (4)", SceneInstances.Fungus2_15, false, "PersistentBoolData", "Open right top door");

                    // Fungus2_20
                    public static readonly FlagDef Fungus2_20__One_Way_Wall = new FlagDef("One Way Wall", SceneInstances.Fungus2_20, false, "PersistentBoolData", "Broke wall back to entrance");
                    public static readonly FlagDef Fungus2_20__Breakable_Wall_Waterways = new FlagDef("Breakable Wall Waterways", SceneInstances.Fungus2_20, false, "PersistentBoolData", "Opened wall to Deepnest");

                    // Fungus2_23
                    public static readonly FlagDef Fungus2_23__Collapser_Small = new FlagDef("Collapser Small", SceneInstances.Fungus2_23, false, "PersistentBoolData", "Collapisble floor opened for Bretta escape");

                    // Fungus2_31
                    public static readonly FlagDef Fungus2_31__Mantis_Chest = new FlagDef("Mantis Chest", SceneInstances.Fungus2_31, false, "PersistentBoolData");
                    public static readonly FlagDef Fungus2_31__Mantis_Chest_1 = new FlagDef("Mantis Chest (1)", SceneInstances.Fungus2_31, false, "PersistentBoolData");
                    public static readonly FlagDef Fungus2_31__Mantis_Chest_2 = new FlagDef("Mantis Chest (2)", SceneInstances.Fungus2_31, false, "PersistentBoolData");

                #endregion

                #region Mines
                    // Mines_33
                    public static readonly FlagDef Mines_33__One_Way_Wall = new FlagDef("One Way Wall", SceneInstances.Mines_33, false, "PersistentBoolData", "Opened wall to exit pit");
                    public static readonly FlagDef Mines_33__Toll_Gate_Machine = new FlagDef("Toll Gate Machine", SceneInstances.Mines_33, false, "PersistentBoolData", "Paid toll to open gate");
                #endregion

                #region Resting Grounds
                    // RestingGrounds_09
                    public static readonly FlagDef RestingGrounds_09__Ruins_Lever = new FlagDef("Ruins Lever", SceneInstances.RestingGrounds_09, false, "PersistentBoolData", "Opened Resting Grounds Stag Station");
                #endregion

                #region Rooms
                    // Room_Town_Stag_Station
                    public static readonly FlagDef Room_Town_Stag_Station__Gate_Switch = new FlagDef("Gate Switch", SceneInstances.Room_Town_Stag_Station, false, "PersistentBoolData", "Opened Town Stag Station door");
                #endregion

                #region Ruins 2
                    // Ruins_Bathhouse
                    public static readonly FlagDef Ruins_Bathhouse__Breakable_Wall = new FlagDef("Breakable Wall", SceneInstances.Ruins_Bathhouse, false, "PersistentBoolData", "Opened wall to " + SceneInstances.Ruins2_10_b.ReadableName);
                    public static readonly FlagDef Ruins_Bathhouse__Ghost_NPC = new FlagDef("Ghost NPC", SceneInstances.Ruins_Bathhouse, false, "PersistentBoolData", "Collected Ghost Marissa with dreamnail");

                    // Ruins_Elevator
                    public static readonly FlagDef Ruins_Elevator__Ghost_NPC = new FlagDef("Ghost NPC", SceneInstances.Ruins_Elevator, false, "PersistentBoolData", "Collected Ghost Poggy Thorax with dreamnail");
                    public static readonly FlagDef Ruins_Elevator__One_Way_Wall = new FlagDef("One Way Wall", SceneInstances.Ruins_Elevator, false, "PersistentBoolData", "Open wall back from Wanderer's Journal");
                    public static readonly FlagDef Ruins_Elevator__Shiny_Item = new FlagDef("Shiny Item", SceneInstances.Ruins_Elevator, false, "PersistentBoolData", "Rancid Egg");

                    // Ruins1_05b
                    public static readonly FlagDef Ruins1_05b__Ruins_Lever_1 = new FlagDef("Ruins Lever 1", SceneInstances.Ruins1_05b, false, "PersistentBoolData", "Open gate to lower area");

                    // Ruins1_27
                    public static readonly FlagDef Ruins1_27__Ruins_Lever = new FlagDef("Ruins Lever", SceneInstances.Ruins1_27, false, "PersistentBoolData", "Open gate to west area");

                    // Ruins2_09
                    public static readonly FlagDef Ruins2_09__Battle_Scene = new FlagDef("Battle Scene", SceneInstances.Ruins2_09, false, "PersistentBoolData", "Royal bugs battle");
                #endregion

                #region Town
                    public static readonly FlagDef Town__Door_Destroyer = new FlagDef("Door Destroyer", SceneInstances.Town, false, "PersistentBoolData", "Door broken open from King's Pass");
                    public static readonly FlagDef Town__Gravedigger_NPC = new FlagDef("Gravedigger NPC", SceneInstances.Town, false, "PersistentBoolData", "Collected Ghost Gravedigger with dreamnail");
                #endregion

                #region Tutorial_01
                    // Tutorial_01
                    public static readonly FlagDef Tutorial_01__Break_Floor_1 = new FlagDef("Break Floor 1", SceneInstances.Tutorial_01, false, "PersistentBoolData", "Broke floor back up to top");
                    public static readonly FlagDef Tutorial_01__Collapser_Tute_01 = new FlagDef("Collapser Tute 01", SceneInstances.Tutorial_01, false, "PersistentBoolData", "Fell through the collaspable floor");
                    public static readonly FlagDef Tutorial_01__Door = new FlagDef("Door", SceneInstances.Tutorial_01, false, "PersistentBoolData", "Broke door open to Dirtmouth");
                    public static readonly FlagDef Tutorial_01__Tute_Door_1 = new FlagDef("Tute Door 1", SceneInstances.Tutorial_01, false, "PersistentBoolData", "Intro Door 1");
                    public static readonly FlagDef Tutorial_01__Tute_Door_2 = new FlagDef("Tute Door 2", SceneInstances.Tutorial_01, false, "PersistentBoolData", "Intro Door 2");
                    public static readonly FlagDef Tutorial_01__Tute_Door_3 = new FlagDef("Tute Door 3", SceneInstances.Tutorial_01, false, "PersistentBoolData", "Intro Door 3");
                    public static readonly FlagDef Tutorial_01__Tute_Door_4 = new FlagDef("Tute Door 4", SceneInstances.Tutorial_01, false, "PersistentBoolData", "Intro Door 4");
                    public static readonly FlagDef Tutorial_01__Tute_Door_5 = new FlagDef("Tute Door 5", SceneInstances.Tutorial_01, false, "PersistentBoolData", "Intro Door 5");
                    public static readonly FlagDef Tutorial_01__Tute_Door_6 = new FlagDef("Tute Door 6", SceneInstances.Tutorial_01, false, "PersistentBoolData", "Intro Door 6");
                    public static readonly FlagDef Tutorial_01__Tute_Door_7 = new FlagDef("Tute Door 7", SceneInstances.Tutorial_01, false, "PersistentBoolData", "Intro Door 7");
                #endregion

            #endregion

            #region Dream Plants
                // Abyss_01
                public static readonly FlagDef Abyss_01__Dream_Plant = new FlagDef("Dream Plant", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb = new FlagDef("Dream Plant Orb", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_1 = new FlagDef("Dream Plant Orb (1)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_10 = new FlagDef("Dream Plant Orb (10)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_11 = new FlagDef("Dream Plant Orb (11)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_12 = new FlagDef("Dream Plant Orb (12)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_13 = new FlagDef("Dream Plant Orb (13)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_14 = new FlagDef("Dream Plant Orb (14)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_15 = new FlagDef("Dream Plant Orb (15)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_16 = new FlagDef("Dream Plant Orb (16)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_17 = new FlagDef("Dream Plant Orb (17)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_18 = new FlagDef("Dream Plant Orb (18)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_19 = new FlagDef("Dream Plant Orb (19)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_2 = new FlagDef("Dream Plant Orb (2)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_20 = new FlagDef("Dream Plant Orb (20)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_21 = new FlagDef("Dream Plant Orb (21)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_22 = new FlagDef("Dream Plant Orb (22)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_23 = new FlagDef("Dream Plant Orb (23)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_24 = new FlagDef("Dream Plant Orb (24)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_25 = new FlagDef("Dream Plant Orb (25)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_26 = new FlagDef("Dream Plant Orb (26)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_27 = new FlagDef("Dream Plant Orb (27)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_28 = new FlagDef("Dream Plant Orb (28)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_29 = new FlagDef("Dream Plant Orb (29)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_3 = new FlagDef("Dream Plant Orb (3)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_30 = new FlagDef("Dream Plant Orb (30)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_31 = new FlagDef("Dream Plant Orb (31)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_32 = new FlagDef("Dream Plant Orb (32)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_33 = new FlagDef("Dream Plant Orb (33)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_34 = new FlagDef("Dream Plant Orb (34)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_4 = new FlagDef("Dream Plant Orb (4)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_5 = new FlagDef("Dream Plant Orb (5)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_6 = new FlagDef("Dream Plant Orb (6)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_7 = new FlagDef("Dream Plant Orb (7)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_8 = new FlagDef("Dream Plant Orb (8)", SceneInstances.Abyss_01, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_01__Dream_Plant_Orb_9 = new FlagDef("Dream Plant Orb (9)", SceneInstances.Abyss_01, false, "PersistentBoolData");

                // Cliffs_01
                public static readonly FlagDef Cliffs_01__Dream_Plant = new FlagDef("Dream Plant", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_11 = new FlagDef("Dream Plant Orb (11)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_13 = new FlagDef("Dream Plant Orb (13)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_14 = new FlagDef("Dream Plant Orb (14)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_15 = new FlagDef("Dream Plant Orb (15)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_16 = new FlagDef("Dream Plant Orb (16)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_17 = new FlagDef("Dream Plant Orb (17)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_18 = new FlagDef("Dream Plant Orb (18)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_19 = new FlagDef("Dream Plant Orb (19)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_2 = new FlagDef("Dream Plant Orb (2)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_22 = new FlagDef("Dream Plant Orb (22)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_24 = new FlagDef("Dream Plant Orb (24)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_25 = new FlagDef("Dream Plant Orb (25)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_26 = new FlagDef("Dream Plant Orb (26)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_27 = new FlagDef("Dream Plant Orb (27)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_28 = new FlagDef("Dream Plant Orb (28)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_29 = new FlagDef("Dream Plant Orb (29)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_3 = new FlagDef("Dream Plant Orb (3)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_30 = new FlagDef("Dream Plant Orb (30)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_31 = new FlagDef("Dream Plant Orb (31)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_33 = new FlagDef("Dream Plant Orb (33)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_35 = new FlagDef("Dream Plant Orb (35)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_36 = new FlagDef("Dream Plant Orb (36)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_38 = new FlagDef("Dream Plant Orb (38)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_39 = new FlagDef("Dream Plant Orb (39)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_4 = new FlagDef("Dream Plant Orb (4)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_41 = new FlagDef("Dream Plant Orb (41)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_42 = new FlagDef("Dream Plant Orb (42)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_45 = new FlagDef("Dream Plant Orb (45)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_47 = new FlagDef("Dream Plant Orb (47)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_48 = new FlagDef("Dream Plant Orb (48)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_50 = new FlagDef("Dream Plant Orb (50)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_52 = new FlagDef("Dream Plant Orb (52)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_53 = new FlagDef("Dream Plant Orb (53)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_55 = new FlagDef("Dream Plant Orb (55)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_56 = new FlagDef("Dream Plant Orb (56)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_57 = new FlagDef("Dream Plant Orb (57)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_58 = new FlagDef("Dream Plant Orb (58)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_59 = new FlagDef("Dream Plant Orb (59)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_6 = new FlagDef("Dream Plant Orb (6)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_61 = new FlagDef("Dream Plant Orb (61)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_63 = new FlagDef("Dream Plant Orb (63)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_64 = new FlagDef("Dream Plant Orb (64)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_66 = new FlagDef("Dream Plant Orb (66)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_67 = new FlagDef("Dream Plant Orb (67)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_8 = new FlagDef("Dream Plant Orb (8)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                public static readonly FlagDef Cliffs_01__Dream_Plant_Orb_9 = new FlagDef("Dream Plant Orb (9)", SceneInstances.Cliffs_01, false, "PersistentBoolData");

                // Crossroads_07
                public static readonly FlagDef Crossroads_07__Dream_Plant = new FlagDef("Dream Plant", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb = new FlagDef("Dream Plant Orb", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_1 = new FlagDef("Dream Plant Orb (1)", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_10 = new FlagDef("Dream Plant Orb (10)", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_11 = new FlagDef("Dream Plant Orb (11)", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_12 = new FlagDef("Dream Plant Orb (12)", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_13 = new FlagDef("Dream Plant Orb (13)", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_14 = new FlagDef("Dream Plant Orb (14)", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_15 = new FlagDef("Dream Plant Orb (15)", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_16 = new FlagDef("Dream Plant Orb (16)", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_17 = new FlagDef("Dream Plant Orb (17)", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_18 = new FlagDef("Dream Plant Orb (18)", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_19 = new FlagDef("Dream Plant Orb (19)", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_2 = new FlagDef("Dream Plant Orb (2)", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_20 = new FlagDef("Dream Plant Orb (20)", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_21 = new FlagDef("Dream Plant Orb (21)", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_22 = new FlagDef("Dream Plant Orb (22)", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_23 = new FlagDef("Dream Plant Orb (23)", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_24 = new FlagDef("Dream Plant Orb (24)", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_25 = new FlagDef("Dream Plant Orb (25)", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_26 = new FlagDef("Dream Plant Orb (26)", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_27 = new FlagDef("Dream Plant Orb (27)", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_28 = new FlagDef("Dream Plant Orb (28)", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_3 = new FlagDef("Dream Plant Orb (3)", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_4 = new FlagDef("Dream Plant Orb (4)", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_5 = new FlagDef("Dream Plant Orb (5)", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_6 = new FlagDef("Dream Plant Orb (6)", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_7 = new FlagDef("Dream Plant Orb (7)", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_8 = new FlagDef("Dream Plant Orb (8)", SceneInstances.Crossroads_07, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_07__Dream_Plant_Orb_9 = new FlagDef("Dream Plant Orb (9)", SceneInstances.Crossroads_07, false, "PersistentBoolData");

                // Crossroads_ShamanTemple
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant = new FlagDef("Dream Plant", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_29 = new FlagDef("Dream Plant Orb (29)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_30 = new FlagDef("Dream Plant Orb (30)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_31 = new FlagDef("Dream Plant Orb (31)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_32 = new FlagDef("Dream Plant Orb (32)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_33 = new FlagDef("Dream Plant Orb (33)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_34 = new FlagDef("Dream Plant Orb (34)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_35 = new FlagDef("Dream Plant Orb (35)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_36 = new FlagDef("Dream Plant Orb (36)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_37 = new FlagDef("Dream Plant Orb (37)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_38 = new FlagDef("Dream Plant Orb (38)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_39 = new FlagDef("Dream Plant Orb (39)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_40 = new FlagDef("Dream Plant Orb (40)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_41 = new FlagDef("Dream Plant Orb (41)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb = new FlagDef("Dream Plant Orb", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_1 = new FlagDef("Dream Plant Orb (1)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_10 = new FlagDef("Dream Plant Orb (10)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_11 = new FlagDef("Dream Plant Orb (11)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_12 = new FlagDef("Dream Plant Orb (12)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_13 = new FlagDef("Dream Plant Orb (13)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_14 = new FlagDef("Dream Plant Orb (14)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_15 = new FlagDef("Dream Plant Orb (15)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_16 = new FlagDef("Dream Plant Orb (16)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_17 = new FlagDef("Dream Plant Orb (17)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_18 = new FlagDef("Dream Plant Orb (18)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_19 = new FlagDef("Dream Plant Orb (19)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_2 = new FlagDef("Dream Plant Orb (2)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_20 = new FlagDef("Dream Plant Orb (20)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_21 = new FlagDef("Dream Plant Orb (21)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_22 = new FlagDef("Dream Plant Orb (22)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_23 = new FlagDef("Dream Plant Orb (23)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_24 = new FlagDef("Dream Plant Orb (24)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_25 = new FlagDef("Dream Plant Orb (25)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_26 = new FlagDef("Dream Plant Orb (26)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_27 = new FlagDef("Dream Plant Orb (27)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_28 = new FlagDef("Dream Plant Orb (28)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_3 = new FlagDef("Dream Plant Orb (3)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_4 = new FlagDef("Dream Plant Orb (4)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_5 = new FlagDef("Dream Plant Orb (5)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_6 = new FlagDef("Dream Plant Orb (6)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_7 = new FlagDef("Dream Plant Orb (7)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_8 = new FlagDef("Dream Plant Orb (8)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_ShamanTemple__Dream_Plant_Orb_9 = new FlagDef("Dream Plant Orb (9)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData");

                // Dreamnest_39
                public static readonly FlagDef Deepnest_39__Dream_Plant = new FlagDef("Dream Plant", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb = new FlagDef("Dream Plant Orb", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_1 = new FlagDef("Dream Plant Orb (1)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_10 = new FlagDef("Dream Plant Orb (10)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_11 = new FlagDef("Dream Plant Orb (11)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_12 = new FlagDef("Dream Plant Orb (12)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_13 = new FlagDef("Dream Plant Orb (13)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_14 = new FlagDef("Dream Plant Orb (14)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_15 = new FlagDef("Dream Plant Orb (15)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_16 = new FlagDef("Dream Plant Orb (16)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_17 = new FlagDef("Dream Plant Orb (17)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_18 = new FlagDef("Dream Plant Orb (18)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_19 = new FlagDef("Dream Plant Orb (19)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_2 = new FlagDef("Dream Plant Orb (2)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_20 = new FlagDef("Dream Plant Orb (20)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_21 = new FlagDef("Dream Plant Orb (21)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_22 = new FlagDef("Dream Plant Orb (22)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_23 = new FlagDef("Dream Plant Orb (23)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_24 = new FlagDef("Dream Plant Orb (24)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_25 = new FlagDef("Dream Plant Orb (25)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_26 = new FlagDef("Dream Plant Orb (26)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_27 = new FlagDef("Dream Plant Orb (27)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_28 = new FlagDef("Dream Plant Orb (28)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_29 = new FlagDef("Dream Plant Orb (29)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_3 = new FlagDef("Dream Plant Orb (3)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_30 = new FlagDef("Dream Plant Orb (30)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_31 = new FlagDef("Dream Plant Orb (31)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_32 = new FlagDef("Dream Plant Orb (32)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_33 = new FlagDef("Dream Plant Orb (33)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_34 = new FlagDef("Dream Plant Orb (34)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_35 = new FlagDef("Dream Plant Orb (35)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_36 = new FlagDef("Dream Plant Orb (36)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_37 = new FlagDef("Dream Plant Orb (37)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_38 = new FlagDef("Dream Plant Orb (38)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_39 = new FlagDef("Dream Plant Orb (39)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_4 = new FlagDef("Dream Plant Orb (4)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_40 = new FlagDef("Dream Plant Orb (40)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_41 = new FlagDef("Dream Plant Orb (41)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_42 = new FlagDef("Dream Plant Orb (42)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_43 = new FlagDef("Dream Plant Orb (43)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_44 = new FlagDef("Dream Plant Orb (44)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_5 = new FlagDef("Dream Plant Orb (5)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_6 = new FlagDef("Dream Plant Orb (6)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_7 = new FlagDef("Dream Plant Orb (7)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_8 = new FlagDef("Dream Plant Orb (8)", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Dream_Plant_Orb_9 = new FlagDef("Dream Plant Orb (9)", SceneInstances.Deepnest_39, false, "PersistentBoolData");

                // Fungus1_13
                public static readonly FlagDef Fungus1_13__Dream_Plant = new FlagDef("Dream Plant", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb = new FlagDef("Dream Plant Orb", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_1 = new FlagDef("Dream Plant Orb (1)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_10 = new FlagDef("Dream Plant Orb (10)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_11 = new FlagDef("Dream Plant Orb (11)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_12 = new FlagDef("Dream Plant Orb (12)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_13 = new FlagDef("Dream Plant Orb (13)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_14 = new FlagDef("Dream Plant Orb (14)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_15 = new FlagDef("Dream Plant Orb (15)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_16 = new FlagDef("Dream Plant Orb (16)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_17 = new FlagDef("Dream Plant Orb (17)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_18 = new FlagDef("Dream Plant Orb (18)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_19 = new FlagDef("Dream Plant Orb (19)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_2 = new FlagDef("Dream Plant Orb (2)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_20 = new FlagDef("Dream Plant Orb (20)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_21 = new FlagDef("Dream Plant Orb (21)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_22 = new FlagDef("Dream Plant Orb (22)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_23 = new FlagDef("Dream Plant Orb (23)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_24 = new FlagDef("Dream Plant Orb (24)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_25 = new FlagDef("Dream Plant Orb (25)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_26 = new FlagDef("Dream Plant Orb (26)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_27 = new FlagDef("Dream Plant Orb (27)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_28 = new FlagDef("Dream Plant Orb (28)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_29 = new FlagDef("Dream Plant Orb (29)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_3 = new FlagDef("Dream Plant Orb (3)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_30 = new FlagDef("Dream Plant Orb (30)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_31 = new FlagDef("Dream Plant Orb (31)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_32 = new FlagDef("Dream Plant Orb (32)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_33 = new FlagDef("Dream Plant Orb (33)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_34 = new FlagDef("Dream Plant Orb (34)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_35 = new FlagDef("Dream Plant Orb (35)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_36 = new FlagDef("Dream Plant Orb (36)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_37 = new FlagDef("Dream Plant Orb (37)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_38 = new FlagDef("Dream Plant Orb (38)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_39 = new FlagDef("Dream Plant Orb (39)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_4 = new FlagDef("Dream Plant Orb (4)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_40 = new FlagDef("Dream Plant Orb (40)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_41 = new FlagDef("Dream Plant Orb (41)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_42 = new FlagDef("Dream Plant Orb (42)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_43 = new FlagDef("Dream Plant Orb (43)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_5 = new FlagDef("Dream Plant Orb (5)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_6 = new FlagDef("Dream Plant Orb (6)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_7 = new FlagDef("Dream Plant Orb (7)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_8 = new FlagDef("Dream Plant Orb (8)", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Dream_Plant_Orb_9 = new FlagDef("Dream Plant Orb (9)", SceneInstances.Fungus1_13, false, "PersistentBoolData");

                // Fungus2_17
                public static readonly FlagDef Fungus2_17__Dream_Plant = new FlagDef("Dream Plant", SceneInstances.Fungus2_17, false, "PersistentBoolData");
                public static readonly FlagDef Fungus2_17__Dream_Plant_Orb = new FlagDef("Dream Plant Orb", SceneInstances.Fungus2_17, false, "PersistentBoolData");
                public static readonly FlagDef Fungus2_17__Dream_Plant_Orb_1 = new FlagDef("Dream Plant Orb (1)", SceneInstances.Fungus2_17, false, "PersistentBoolData");
                public static readonly FlagDef Fungus2_17__Dream_Plant_Orb_10 = new FlagDef("Dream Plant Orb (10)", SceneInstances.Fungus2_17, false, "PersistentBoolData");
                public static readonly FlagDef Fungus2_17__Dream_Plant_Orb_11 = new FlagDef("Dream Plant Orb (11)", SceneInstances.Fungus2_17, false, "PersistentBoolData");
                public static readonly FlagDef Fungus2_17__Dream_Plant_Orb_12 = new FlagDef("Dream Plant Orb (12)", SceneInstances.Fungus2_17, false, "PersistentBoolData");
                public static readonly FlagDef Fungus2_17__Dream_Plant_Orb_13 = new FlagDef("Dream Plant Orb (13)", SceneInstances.Fungus2_17, false, "PersistentBoolData");
                public static readonly FlagDef Fungus2_17__Dream_Plant_Orb_14 = new FlagDef("Dream Plant Orb (14)", SceneInstances.Fungus2_17, false, "PersistentBoolData");
                public static readonly FlagDef Fungus2_17__Dream_Plant_Orb_15 = new FlagDef("Dream Plant Orb (15)", SceneInstances.Fungus2_17, false, "PersistentBoolData");
                public static readonly FlagDef Fungus2_17__Dream_Plant_Orb_16 = new FlagDef("Dream Plant Orb (16)", SceneInstances.Fungus2_17, false, "PersistentBoolData");
                public static readonly FlagDef Fungus2_17__Dream_Plant_Orb_17 = new FlagDef("Dream Plant Orb (17)", SceneInstances.Fungus2_17, false, "PersistentBoolData");
                public static readonly FlagDef Fungus2_17__Dream_Plant_Orb_2 = new FlagDef("Dream Plant Orb (2)", SceneInstances.Fungus2_17, false, "PersistentBoolData");
                public static readonly FlagDef Fungus2_17__Dream_Plant_Orb_3 = new FlagDef("Dream Plant Orb (3)", SceneInstances.Fungus2_17, false, "PersistentBoolData");
                public static readonly FlagDef Fungus2_17__Dream_Plant_Orb_4 = new FlagDef("Dream Plant Orb (4)", SceneInstances.Fungus2_17, false, "PersistentBoolData");
                public static readonly FlagDef Fungus2_17__Dream_Plant_Orb_5 = new FlagDef("Dream Plant Orb (5)", SceneInstances.Fungus2_17, false, "PersistentBoolData");
                public static readonly FlagDef Fungus2_17__Dream_Plant_Orb_6 = new FlagDef("Dream Plant Orb (6)", SceneInstances.Fungus2_17, false, "PersistentBoolData");
                public static readonly FlagDef Fungus2_17__Dream_Plant_Orb_7 = new FlagDef("Dream Plant Orb (7)", SceneInstances.Fungus2_17, false, "PersistentBoolData");
                public static readonly FlagDef Fungus2_17__Dream_Plant_Orb_8 = new FlagDef("Dream Plant Orb (8)", SceneInstances.Fungus2_17, false, "PersistentBoolData");
                public static readonly FlagDef Fungus2_17__Dream_Plant_Orb_9 = new FlagDef("Dream Plant Orb (9)", SceneInstances.Fungus2_17, false, "PersistentBoolData");

                // RestingGrounds_05
                public static readonly FlagDef RestingGrounds_05__Dream_Plant = new FlagDef("Dream Plant", SceneInstances.RestingGrounds_05, false, "PersistentBoolData");
                public static readonly FlagDef RestingGrounds_05__Dream_Plant_Orb = new FlagDef("Dream Plant Orb", SceneInstances.RestingGrounds_05, false, "PersistentBoolData");
                public static readonly FlagDef RestingGrounds_05__Dream_Plant_Orb_1 = new FlagDef("Dream Plant Orb (1)", SceneInstances.RestingGrounds_05, false, "PersistentBoolData");
                public static readonly FlagDef RestingGrounds_05__Dream_Plant_Orb_10 = new FlagDef("Dream Plant Orb (10)", SceneInstances.RestingGrounds_05, false, "PersistentBoolData");
                public static readonly FlagDef RestingGrounds_05__Dream_Plant_Orb_11 = new FlagDef("Dream Plant Orb (11)", SceneInstances.RestingGrounds_05, false, "PersistentBoolData");
                public static readonly FlagDef RestingGrounds_05__Dream_Plant_Orb_12 = new FlagDef("Dream Plant Orb (12)", SceneInstances.RestingGrounds_05, false, "PersistentBoolData");
                public static readonly FlagDef RestingGrounds_05__Dream_Plant_Orb_13 = new FlagDef("Dream Plant Orb (13)", SceneInstances.RestingGrounds_05, false, "PersistentBoolData");
                public static readonly FlagDef RestingGrounds_05__Dream_Plant_Orb_14 = new FlagDef("Dream Plant Orb (14)", SceneInstances.RestingGrounds_05, false, "PersistentBoolData");
                public static readonly FlagDef RestingGrounds_05__Dream_Plant_Orb_15 = new FlagDef("Dream Plant Orb (15)", SceneInstances.RestingGrounds_05, false, "PersistentBoolData");
                public static readonly FlagDef RestingGrounds_05__Dream_Plant_Orb_16 = new FlagDef("Dream Plant Orb (16)", SceneInstances.RestingGrounds_05, false, "PersistentBoolData");
                public static readonly FlagDef RestingGrounds_05__Dream_Plant_Orb_17 = new FlagDef("Dream Plant Orb (17)", SceneInstances.RestingGrounds_05, false, "PersistentBoolData");
                public static readonly FlagDef RestingGrounds_05__Dream_Plant_Orb_18 = new FlagDef("Dream Plant Orb (18)", SceneInstances.RestingGrounds_05, false, "PersistentBoolData");
                public static readonly FlagDef RestingGrounds_05__Dream_Plant_Orb_19 = new FlagDef("Dream Plant Orb (19)", SceneInstances.RestingGrounds_05, false, "PersistentBoolData");
                public static readonly FlagDef RestingGrounds_05__Dream_Plant_Orb_2 = new FlagDef("Dream Plant Orb (2)", SceneInstances.RestingGrounds_05, false, "PersistentBoolData");
                public static readonly FlagDef RestingGrounds_05__Dream_Plant_Orb_3 = new FlagDef("Dream Plant Orb (3)", SceneInstances.RestingGrounds_05, false, "PersistentBoolData");
                public static readonly FlagDef RestingGrounds_05__Dream_Plant_Orb_4 = new FlagDef("Dream Plant Orb (4)", SceneInstances.RestingGrounds_05, false, "PersistentBoolData");
                public static readonly FlagDef RestingGrounds_05__Dream_Plant_Orb_5 = new FlagDef("Dream Plant Orb (5)", SceneInstances.RestingGrounds_05, false, "PersistentBoolData");
                public static readonly FlagDef RestingGrounds_05__Dream_Plant_Orb_6 = new FlagDef("Dream Plant Orb (6)", SceneInstances.RestingGrounds_05, false, "PersistentBoolData");
                public static readonly FlagDef RestingGrounds_05__Dream_Plant_Orb_7 = new FlagDef("Dream Plant Orb (7)", SceneInstances.RestingGrounds_05, false, "PersistentBoolData");
                public static readonly FlagDef RestingGrounds_05__Dream_Plant_Orb_8 = new FlagDef("Dream Plant Orb (8)", SceneInstances.RestingGrounds_05, false, "PersistentBoolData");
                public static readonly FlagDef RestingGrounds_05__Dream_Plant_Orb_9 = new FlagDef("Dream Plant Orb (9)", SceneInstances.RestingGrounds_05, false, "PersistentBoolData");

            #endregion

            #region Geo Rocks
                public static readonly FlagDef Abyss_01__Geo_Rock_1 = new FlagDef("Geo Rock 1", SceneInstances.Abyss_01, false, "GeoRockData");
                public static readonly FlagDef Abyss_01__Geo_Rock_2 = new FlagDef("Geo Rock 2", SceneInstances.Abyss_01, false, "GeoRockData");
                public static readonly FlagDef Abyss_01__Geo_Rock_2_1 = new FlagDef("Geo Rock 2 (1)", SceneInstances.Abyss_01, false, "GeoRockData");
                public static readonly FlagDef Abyss_02__Geo_Rock_Deepnest = new FlagDef("Geo Rock Deepnest", SceneInstances.Abyss_02, false, "GeoRockData");
                public static readonly FlagDef Abyss_02__Geo_Rock_Deepnest_1 = new FlagDef("Geo Rock Deepnest (1)", SceneInstances.Abyss_02, false, "GeoRockData");
                public static readonly FlagDef Abyss_02__Geo_Rock_Deepnest_2 = new FlagDef("Geo Rock Deepnest (2)", SceneInstances.Abyss_02, false, "GeoRockData");
                public static readonly FlagDef Abyss_06_Core__Geo_Rock_Abyss = new FlagDef("Geo Rock Abyss", SceneInstances.Abyss_06_Core, false, "GeoRockData");
                public static readonly FlagDef Abyss_06_Core__Geo_Rock_Abyss_1 = new FlagDef("Geo Rock Abyss (1)", SceneInstances.Abyss_06_Core, false, "GeoRockData");
                public static readonly FlagDef Abyss_18__Geo_Rock_Abyss = new FlagDef("Geo Rock Abyss", SceneInstances.Abyss_18, false, "GeoRockData");
                public static readonly FlagDef Cliffs_01__Geo_Rock_2_1 = new FlagDef("Geo Rock 2 (1)", SceneInstances.Cliffs_01, false, "GeoRockData");
                public static readonly FlagDef Cliffs_01__Geo_Rock_2_2 = new FlagDef("Geo Rock 2 (2)", SceneInstances.Cliffs_01, false, "GeoRockData");
                public static readonly FlagDef Cliffs_01__Geo_Rock_2_3 = new FlagDef("Geo Rock 2 (3)", SceneInstances.Cliffs_01, false, "GeoRockData");
                public static readonly FlagDef Cliffs_01__Geo_Rock_2_4 = new FlagDef("Geo Rock 2 (4)", SceneInstances.Cliffs_01, false, "GeoRockData");
                public static readonly FlagDef Cliffs_02__Geo_Rock_1 = new FlagDef("Geo Rock 1", SceneInstances.Cliffs_02, false, "GeoRockData");
                public static readonly FlagDef Cliffs_02__Geo_Rock_1_1 = new FlagDef("Geo Rock 1 (1)", SceneInstances.Cliffs_02, false, "GeoRockData");
                public static readonly FlagDef Crossroads_01__Geo_Rock_2 = new FlagDef("Geo Rock 2", SceneInstances.Crossroads_01, false, "GeoRockData");
                public static readonly FlagDef Crossroads_05__Geo_Rock_1 = new FlagDef("Geo Rock 1", SceneInstances.Crossroads_05, false, "GeoRockData");
                public static readonly FlagDef Crossroads_07__Geo_Rock_1 = new FlagDef("Geo Rock 1", SceneInstances.Crossroads_07, false, "GeoRockData");
                public static readonly FlagDef Crossroads_07__Geo_Rock_1_1 = new FlagDef("Geo Rock 1 (1)", SceneInstances.Crossroads_07, false, "GeoRockData");
                public static readonly FlagDef Crossroads_07__Geo_Rock_1_2 = new FlagDef("Geo Rock 1 (2)", SceneInstances.Crossroads_07, false, "GeoRockData");
                public static readonly FlagDef Crossroads_08__Geo_Rock_1 = new FlagDef("Geo Rock 1", SceneInstances.Crossroads_08, false, "GeoRockData");
                public static readonly FlagDef Crossroads_08__Geo_Rock_1_1 = new FlagDef("Geo Rock 1 (1)", SceneInstances.Crossroads_08, false, "GeoRockData");
                public static readonly FlagDef Crossroads_08__Geo_Rock_1_2 = new FlagDef("Geo Rock 1 (2)", SceneInstances.Crossroads_08, false, "GeoRockData");
                public static readonly FlagDef Crossroads_08__Geo_Rock_1_3 = new FlagDef("Geo Rock 1 (3)", SceneInstances.Crossroads_08, false, "GeoRockData");
                public static readonly FlagDef Crossroads_10__Geo_Rock_1 = new FlagDef("Geo Rock 1", SceneInstances.Crossroads_10, false, "GeoRockData");
                public static readonly FlagDef Crossroads_12__Geo_Rock_2 = new FlagDef("Geo Rock 2", SceneInstances.Crossroads_12, false, "GeoRockData");
                public static readonly FlagDef Crossroads_13__Geo_Rock_1 = new FlagDef("Geo Rock 1", SceneInstances.Crossroads_13, false, "GeoRockData");
                public static readonly FlagDef Crossroads_13__Geo_Rock_2 = new FlagDef("Geo Rock 2", SceneInstances.Crossroads_13, false, "GeoRockData");
                public static readonly FlagDef Crossroads_16__Geo_Rock_2 = new FlagDef("Geo Rock 2", SceneInstances.Crossroads_16, false, "GeoRockData");
                public static readonly FlagDef Crossroads_18__Geo_Rock_1 = new FlagDef("Geo Rock 1", SceneInstances.Crossroads_18, false, "GeoRockData");
                public static readonly FlagDef Crossroads_18__Geo_Rock_2 = new FlagDef("Geo Rock 2", SceneInstances.Crossroads_18, false, "GeoRockData");
                public static readonly FlagDef Crossroads_18__Geo_Rock_3 = new FlagDef("Geo Rock 3", SceneInstances.Crossroads_18, false, "GeoRockData");
                public static readonly FlagDef Crossroads_19__Geo_Rock_1 = new FlagDef("Geo Rock 1", SceneInstances.Crossroads_19, false, "GeoRockData");
                public static readonly FlagDef Crossroads_21__Geo_Rock_1 = new FlagDef("Geo Rock 1", SceneInstances.Crossroads_21, false, "GeoRockData");
                public static readonly FlagDef Crossroads_27__Geo_Rock_1 = new FlagDef("Geo Rock 1", SceneInstances.Crossroads_27, false, "GeoRockData");
                public static readonly FlagDef Crossroads_36__Geo_Rock_1 = new FlagDef("Geo Rock 1", SceneInstances.Crossroads_36, false, "GeoRockData");
                public static readonly FlagDef Crossroads_42__Geo_Rock_1 = new FlagDef("Geo Rock 1", SceneInstances.Crossroads_42, false, "GeoRockData");
                public static readonly FlagDef Crossroads_42__Geo_Rock_2 = new FlagDef("Geo Rock 2", SceneInstances.Crossroads_42, false, "GeoRockData");
                public static readonly FlagDef Crossroads_46__Geo_Rock_1 = new FlagDef("Geo Rock 1", SceneInstances.Crossroads_46, false, "GeoRockData");
                public static readonly FlagDef Crossroads_52__Geo_Rock_1 = new FlagDef("Geo Rock 1", SceneInstances.Crossroads_52, false, "GeoRockData");
                public static readonly FlagDef Crossroads_52__Geo_Rock_2 = new FlagDef("Geo Rock 2", SceneInstances.Crossroads_52, false, "GeoRockData");
                public static readonly FlagDef Crossroads_ShamanTemple__Geo_Rock_1 = new FlagDef("Geo Rock 1", SceneInstances.Crossroads_ShamanTemple, false, "GeoRockData");
                public static readonly FlagDef Crossroads_ShamanTemple__Geo_Rock_2 = new FlagDef("Geo Rock 2", SceneInstances.Crossroads_ShamanTemple, false, "GeoRockData");
                public static readonly FlagDef Crossroads_ShamanTemple__Geo_Rock_2_1 = new FlagDef("Geo Rock 2 (1)", SceneInstances.Crossroads_ShamanTemple, false, "GeoRockData");
                public static readonly FlagDef Deepnest_01__Geo_Rock_1 = new FlagDef("Geo Rock 1", SceneInstances.Deepnest_01, false, "GeoRockData");
                public static readonly FlagDef Deepnest_01__Geo_Rock_2 = new FlagDef("Geo Rock 2", SceneInstances.Deepnest_01, false, "GeoRockData");
                public static readonly FlagDef Deepnest_02__Geo_Rock_Deepnest = new FlagDef("Geo Rock Deepnest", SceneInstances.Deepnest_02, false, "GeoRockData");
                public static readonly FlagDef Deepnest_02__Geo_Rock_Deepnest_1 = new FlagDef("Geo Rock Deepnest (1)", SceneInstances.Deepnest_02, false, "GeoRockData");
                public static readonly FlagDef Deepnest_03__Geo_Rock_Deepnest = new FlagDef("Geo Rock Deepnest", SceneInstances.Deepnest_03, false, "GeoRockData");
                public static readonly FlagDef Deepnest_03__Geo_Rock_Deepnest_1 = new FlagDef("Geo Rock Deepnest (1)", SceneInstances.Deepnest_03, false, "GeoRockData");
                public static readonly FlagDef Deepnest_03__Geo_Rock_Deepnest_2 = new FlagDef("Geo Rock Deepnest (2)", SceneInstances.Deepnest_03, false, "GeoRockData");
                public static readonly FlagDef Deepnest_16__Geo_Rock_Deepnest = new FlagDef("Geo Rock Deepnest", SceneInstances.Deepnest_16, false, "GeoRockData");
                public static readonly FlagDef Deepnest_16__Geo_Rock_Deepnest_1 = new FlagDef("Geo Rock Deepnest (1)", SceneInstances.Deepnest_16, false, "GeoRockData");
                public static readonly FlagDef Deepnest_16__Geo_Rock_Deepnest_2 = new FlagDef("Geo Rock Deepnest (2)", SceneInstances.Deepnest_16, false, "GeoRockData");
                public static readonly FlagDef Deepnest_16__Geo_Rock_Deepnest_3 = new FlagDef("Geo Rock Deepnest (3)", SceneInstances.Deepnest_16, false, "GeoRockData");
                public static readonly FlagDef Deepnest_16__Geo_Rock_Deepnest_4 = new FlagDef("Geo Rock Deepnest (4)", SceneInstances.Deepnest_16, false, "GeoRockData");
                public static readonly FlagDef Deepnest_35__Geo_Rock_Deepnest = new FlagDef("Geo Rock Deepnest", SceneInstances.Deepnest_35, false, "GeoRockData");
                public static readonly FlagDef Deepnest_35__Geo_Rock_Deepnest_1 = new FlagDef("Geo Rock Deepnest (1)", SceneInstances.Deepnest_35, false, "GeoRockData");
                public static readonly FlagDef Deepnest_37__Geo_Rock_Deepnest = new FlagDef("Geo Rock Deepnest", SceneInstances.Deepnest_37, false, "GeoRockData");
                public static readonly FlagDef Deepnest_37__Geo_Rock_Deepnest_1 = new FlagDef("Geo Rock Deepnest (1)", SceneInstances.Deepnest_37, false, "GeoRockData");
                public static readonly FlagDef Deepnest_39__Geo_Rock_Deepnest = new FlagDef("Geo Rock Deepnest", SceneInstances.Deepnest_39, false, "GeoRockData");
                public static readonly FlagDef Deepnest_39__Geo_Rock_Deepnest_1 = new FlagDef("Geo Rock Deepnest (1)", SceneInstances.Deepnest_39, false, "GeoRockData");
                public static readonly FlagDef Deepnest_39__Geo_Rock_Deepnest_2 = new FlagDef("Geo Rock Deepnest (2)", SceneInstances.Deepnest_39, false, "GeoRockData");
                public static readonly FlagDef Deepnest_East_01__Geo_Rock_Deepnest = new FlagDef("Geo Rock Deepnest", SceneInstances.Deepnest_East_01, false, "GeoRockData");
                public static readonly FlagDef Deepnest_East_01__Geo_Rock_Deepnest_1 = new FlagDef("Geo Rock Deepnest (1)", SceneInstances.Deepnest_East_01, false, "GeoRockData");
                public static readonly FlagDef Deepnest_Spider_Town__Geo_Rock_Deepnest = new FlagDef("Geo Rock Deepnest", SceneInstances.Deepnest_Spider_Town, false, "GeoRockData");
                public static readonly FlagDef Deepnest_Spider_Town__Geo_Rock_Deepnest_1 = new FlagDef("Geo Rock Deepnest (1)", SceneInstances.Deepnest_Spider_Town, false, "GeoRockData");
                public static readonly FlagDef Deepnest_Spider_Town__Geo_Rock_Deepnest_2 = new FlagDef("Geo Rock Deepnest (2)", SceneInstances.Deepnest_Spider_Town, false, "GeoRockData");
                public static readonly FlagDef Deepnest_Spider_Town__Geo_Rock_Deepnest_3 = new FlagDef("Geo Rock Deepnest (3)", SceneInstances.Deepnest_Spider_Town, false, "GeoRockData");
                public static readonly FlagDef Deepnest_Spider_Town__Geo_Rock_Deepnest_4 = new FlagDef("Geo Rock Deepnest (4)", SceneInstances.Deepnest_Spider_Town, false, "GeoRockData");
                public static readonly FlagDef Deepnest_Spider_Town__Geo_Rock_Deepnest_5 = new FlagDef("Geo Rock Deepnest (5)", SceneInstances.Deepnest_Spider_Town, false, "GeoRockData");
                public static readonly FlagDef Deepnest_Spider_Town__Geo_Rock_Deepnest_6 = new FlagDef("Geo Rock Deepnest (6)", SceneInstances.Deepnest_Spider_Town, false, "GeoRockData");
                public static readonly FlagDef Deepnest_Spider_Town__Geo_Rock_Deepnest_7 = new FlagDef("Geo Rock Deepnest (7)", SceneInstances.Deepnest_Spider_Town, false, "GeoRockData");
                public static readonly FlagDef Fungus1_01__Geo_Rock_Green_Path_01 = new FlagDef("Geo Rock Green Path 01", SceneInstances.Fungus1_01, false, "GeoRockData");
                public static readonly FlagDef Fungus1_01b__Geo_Rock_Green_Path_01 = new FlagDef("Geo Rock Green Path 01", SceneInstances.Fungus1_01b, false, "GeoRockData");
                public static readonly FlagDef Fungus1_02__Geo_Rock_Green_Path_01 = new FlagDef("Geo Rock Green Path 01", SceneInstances.Fungus1_02, false, "GeoRockData");
                public static readonly FlagDef Fungus1_02__Geo_Rock_Green_Path_01_1 = new FlagDef("Geo Rock Green Path 01 (1)", SceneInstances.Fungus1_02, false, "GeoRockData");
                public static readonly FlagDef Fungus1_03__Geo_Rock_Green_Path_01 = new FlagDef("Geo Rock Green Path 01", SceneInstances.Fungus1_03, false, "GeoRockData");
                public static readonly FlagDef Fungus1_03__Geo_Rock_Green_Path_01_1 = new FlagDef("Geo Rock Green Path 01 (1)", SceneInstances.Fungus1_03, false, "GeoRockData");
                public static readonly FlagDef Fungus1_03__Geo_Rock_Green_Path_01_2 = new FlagDef("Geo Rock Green Path 01 (2)", SceneInstances.Fungus1_03, false, "GeoRockData");
                public static readonly FlagDef Fungus1_04__Geo_Rock_Green_Path_01 = new FlagDef("Geo Rock Green Path 01", SceneInstances.Fungus1_04, false, "GeoRockData");
                public static readonly FlagDef Fungus1_05__Geo_Rock_2 = new FlagDef("Geo Rock 2", SceneInstances.Fungus1_05, false, "GeoRockData");
                public static readonly FlagDef Fungus1_07__Geo_Rock_Green_Path_01 = new FlagDef("Geo Rock Green Path 01", SceneInstances.Fungus1_07, false, "GeoRockData");
                public static readonly FlagDef Fungus1_10__Geo_Rock_Green_Path_01 = new FlagDef("Geo Rock Green Path 01", SceneInstances.Fungus1_10, false, "GeoRockData");
                public static readonly FlagDef Fungus1_12__Geo_Rock_Green_Path_01 = new FlagDef("Geo Rock Green Path 01", SceneInstances.Fungus1_12, false, "GeoRockData");
                public static readonly FlagDef Fungus1_12__Geo_Rock_Green_Path_01_1 = new FlagDef("Geo Rock Green Path 01 (1)", SceneInstances.Fungus1_12, false, "GeoRockData");
                public static readonly FlagDef Fungus1_12__Geo_Rock_Green_Path_02 = new FlagDef("Geo Rock Green Path 02", SceneInstances.Fungus1_12, false, "GeoRockData");
                public static readonly FlagDef Fungus2_14__Geo_Rock_1 = new FlagDef("Geo Rock 1", SceneInstances.Fungus2_14, false, "GeoRockData");
                public static readonly FlagDef Fungus2_14__Geo_Rock_2 = new FlagDef("Geo Rock 2", SceneInstances.Fungus2_14, false, "GeoRockData");
                public static readonly FlagDef Fungus2_14__Geo_Rock_2_1 = new FlagDef("Geo Rock 2 (1)", SceneInstances.Fungus2_14, false, "GeoRockData");
                public static readonly FlagDef Fungus2_14__Geo_Rock_2_2 = new FlagDef("Geo Rock 2 (2)", SceneInstances.Fungus2_14, false, "GeoRockData");
                public static readonly FlagDef Fungus2_14__Geo_Rock_2_3 = new FlagDef("Geo Rock 2 (3)", SceneInstances.Fungus2_14, false, "GeoRockData");
                public static readonly FlagDef Fungus1_19__Geo_Rock_Green_Path_01 = new FlagDef("Geo Rock Green Path 01", SceneInstances.Fungus1_19, false, "GeoRockData");
                public static readonly FlagDef Fungus1_21__Geo_Rock_Green_Path_02 = new FlagDef("Geo Rock Green Path 02", SceneInstances.Fungus1_21, false, "GeoRockData");
                public static readonly FlagDef Fungus1_21__Geo_Rock_Green_Path_02_1 = new FlagDef("Geo Rock Green Path 02 (1)", SceneInstances.Fungus1_21, false, "GeoRockData");
                public static readonly FlagDef Fungus1_21__Geo_Rock_Green_Path_02_2 = new FlagDef("Geo Rock Green Path 02 (2)", SceneInstances.Fungus1_21, false, "GeoRockData");
                public static readonly FlagDef Fungus1_22__Geo_Rock_Green_Path_01 = new FlagDef("Geo Rock Green Path 01", SceneInstances.Fungus1_22, false, "GeoRockData");
                public static readonly FlagDef Fungus1_22__Geo_Rock_Green_Path_01_1 = new FlagDef("Geo Rock Green Path 01 (1)", SceneInstances.Fungus1_22, false, "GeoRockData");
                public static readonly FlagDef Fungus2_25__Geo_Rock_Deepnest = new FlagDef("Geo Rock Deepnest", SceneInstances.Fungus2_25, false, "GeoRockData");
                public static readonly FlagDef Fungus2_25__Geo_Rock_Deepnest_1 = new FlagDef("Geo Rock Deepnest (1)", SceneInstances.Fungus2_25, false, "GeoRockData");
                public static readonly FlagDef Fungus2_25__Geo_Rock_Deepnest_2 = new FlagDef("Geo Rock Deepnest (2)", SceneInstances.Fungus2_25, false, "GeoRockData");
                public static readonly FlagDef Fungus1_28__Geo_Rock_1_2 = new FlagDef("Geo Rock 1 (2)", SceneInstances.Fungus1_28, false, "GeoRockData");
                public static readonly FlagDef Fungus1_28__Geo_Rock_2 = new FlagDef("Geo Rock 2", SceneInstances.Fungus1_28, false, "GeoRockData");
                public static readonly FlagDef Fungus1_29__Geo_Rock_Green_Path_01 = new FlagDef("Geo Rock Green Path 01", SceneInstances.Fungus1_29, false, "GeoRockData");
                public static readonly FlagDef Fungus1_31__Geo_Rock_1_1 = new FlagDef("Geo Rock 1 (1)", SceneInstances.Fungus1_31, false, "GeoRockData");
                public static readonly FlagDef Fungus1_31__Geo_Rock_2_1 = new FlagDef("Geo Rock 2 (1)", SceneInstances.Fungus1_31, false, "GeoRockData");
                public static readonly FlagDef Fungus1_31__Geo_Rock_Green_Path_01 = new FlagDef("Geo Rock Green Path 01", SceneInstances.Fungus1_31, false, "GeoRockData");
                public static readonly FlagDef Fungus2_04__Geo_Rock_Fung_01 = new FlagDef("Geo Rock Fung 01", SceneInstances.Fungus2_04, false, "GeoRockData");
                public static readonly FlagDef Fungus2_08__Geo_Rock_Fung_01 = new FlagDef("Geo Rock Fung 01", SceneInstances.Fungus2_08, false, "GeoRockData");
                public static readonly FlagDef Fungus2_10__Geo_Rock_Fung_01 = new FlagDef("Geo Rock Fung 01", SceneInstances.Fungus2_10, false, "GeoRockData");
                public static readonly FlagDef Fungus2_11__Geo_Rock_Fung_01 = new FlagDef("Geo Rock Fung 01", SceneInstances.Fungus2_11, false, "GeoRockData");
                public static readonly FlagDef Fungus2_11__Geo_Rock_Fung_02 = new FlagDef("Geo Rock Fung 02", SceneInstances.Fungus2_11, false, "GeoRockData");
                public static readonly FlagDef Fungus2_13__Geo_Rock_Fung_01 = new FlagDef("Geo Rock Fung 01", SceneInstances.Fungus2_13, false, "GeoRockData");
                public static readonly FlagDef Fungus2_13__Geo_Rock_Fung_01_1 = new FlagDef("Geo Rock Fung 01 (1)", SceneInstances.Fungus2_13, false, "GeoRockData");
                public static readonly FlagDef Fungus2_13__Geo_Rock_Fung_02 = new FlagDef("Geo Rock Fung 02", SceneInstances.Fungus2_13, false, "GeoRockData");
                public static readonly FlagDef Fungus2_15__Geo_Rock_1 = new FlagDef("Geo Rock 1", SceneInstances.Fungus2_15, false, "GeoRockData");
                public static readonly FlagDef Fungus2_15__Geo_Rock_1_1 = new FlagDef("Geo Rock 1 (1)", SceneInstances.Fungus2_15, false, "GeoRockData");
                public static readonly FlagDef Fungus2_18__Geo_Rock_Fung_01 = new FlagDef("Geo Rock Fung 01", SceneInstances.Fungus2_18, false, "GeoRockData");
                public static readonly FlagDef Fungus2_18__Geo_Rock_Fung_01_1 = new FlagDef("Geo Rock Fung 01 (1)", SceneInstances.Fungus2_18, false, "GeoRockData");
                public static readonly FlagDef Fungus2_18__Geo_Rock_Fung_02_1 = new FlagDef("Geo Rock Fung 02 (1)", SceneInstances.Fungus2_18, false, "GeoRockData");
                public static readonly FlagDef Fungus2_18__Geo_Rock_Fung_02_2 = new FlagDef("Geo Rock Fung 02 (2)", SceneInstances.Fungus2_18, false, "GeoRockData");
                public static readonly FlagDef Fungus2_21__Geo_Rock_1 = new FlagDef("Geo Rock 1", SceneInstances.Fungus2_21, false, "GeoRockData");
                public static readonly FlagDef Fungus2_29__Geo_Rock_Fung_01 = new FlagDef("Geo Rock Fung 01", SceneInstances.Fungus2_29, false, "GeoRockData");
                public static readonly FlagDef Fungus3_03__Geo_Rock_Green_Path_01 = new FlagDef("Geo Rock Green Path 01", SceneInstances.Fungus3_03, false, "GeoRockData");
                public static readonly FlagDef Fungus3_39__Geo_Rock_Green_Path_01 = new FlagDef("Geo Rock Green Path 01", SceneInstances.Fungus3_39, false, "GeoRockData");
                public static readonly FlagDef Fungus3_39__Geo_Rock_Green_Path_01_1 = new FlagDef("Geo Rock Green Path 01 (1)", SceneInstances.Fungus3_39, false, "GeoRockData");
                public static readonly FlagDef Mines_02__Geo_Rock_1 = new FlagDef("Geo Rock 1", SceneInstances.Mines_02, false, "GeoRockData");
                public static readonly FlagDef Mines_02__Geo_Rock_Mine = new FlagDef("Geo Rock Mine", SceneInstances.Mines_02, false, "GeoRockData");
                public static readonly FlagDef Mines_02__Geo_Rock_Mine_1 = new FlagDef("Geo Rock Mine (1)", SceneInstances.Mines_02, false, "GeoRockData");
                public static readonly FlagDef Mines_04__Geo_Rock_Mine = new FlagDef("Geo Rock Mine", SceneInstances.Mines_04, false, "GeoRockData");
                public static readonly FlagDef Mines_04__Geo_Rock_Mine_1 = new FlagDef("Geo Rock Mine (1)", SceneInstances.Mines_04, false, "GeoRockData");
                public static readonly FlagDef Mines_33__Geo_Rock_2 = new FlagDef("Geo Rock 2", SceneInstances.Mines_33, false, "GeoRockData");
                public static readonly FlagDef Mines_33__Geo_Rock_2_1 = new FlagDef("Geo Rock 2 (1)", SceneInstances.Mines_33, false, "GeoRockData");
                public static readonly FlagDef Mines_33__Geo_Rock_2_2 = new FlagDef("Geo Rock 2 (2)", SceneInstances.Mines_33, false, "GeoRockData");
                public static readonly FlagDef Ruins_Elevator__Geo_Rock_City_1 = new FlagDef("Geo Rock City 1", SceneInstances.Ruins_Elevator, false, "GeoRockData");
                public static readonly FlagDef Ruins1_03__Geo_Rock_City_1 = new FlagDef("Geo Rock City 1", SceneInstances.Ruins1_03, false, "GeoRockData");
                public static readonly FlagDef Ruins1_05b__Geo_Rock_City_1_1 = new FlagDef("Geo Rock City 1 (1)", SceneInstances.Ruins1_05b, false, "GeoRockData");
                public static readonly FlagDef Ruins2_05__Geo_Rock_City_1 = new FlagDef("Geo Rock City 1", SceneInstances.Ruins2_05, false, "GeoRockData");
                public static readonly FlagDef Ruins2_06__Geo_Rock_City_1 = new FlagDef("Geo Rock City 1", SceneInstances.Ruins2_06, false, "GeoRockData");
                public static readonly FlagDef Tutorial_01__Geo_Rock_1 = new FlagDef("Geo Rock 1", SceneInstances.Tutorial_01, false, "GeoRockData");
                public static readonly FlagDef Tutorial_01__Geo_Rock_2 = new FlagDef("Geo Rock 2", SceneInstances.Tutorial_01, false, "GeoRockData");
                public static readonly FlagDef Tutorial_01__Geo_Rock_3 = new FlagDef("Geo Rock 3", SceneInstances.Tutorial_01, false, "GeoRockData");
                public static readonly FlagDef Tutorial_01__Geo_Rock_4 = new FlagDef("Geo Rock 4", SceneInstances.Tutorial_01, false, "GeoRockData");
                public static readonly FlagDef Tutorial_01__Geo_Rock_5 = new FlagDef("Geo Rock 5", SceneInstances.Tutorial_01, false, "GeoRockData");
            #endregion

            #region Shop State
                public static readonly FlagDef slyRancidEgg = new FlagDef("slyRancidEgg", null, false, "PlayerData_Bool", "Bought Sly's Rancid Egg");
            #endregion

            #region Stag Stations
                public static readonly FlagDef stationsOpened = new FlagDef("stationsOpened", null, false, "PlayerData_Int", "Stag Stations Opened (0-?)");

                // Individual Stations
                public static readonly FlagDef openedCrossroads = new FlagDef("openedCrossroads", null, false, "PlayerData_Bool", "Crossroads Stag Station");
                public static readonly FlagDef openedFungalWastes = new FlagDef("openedFungalWastes", null, false, "PlayerData_Bool", "Queen's Station Stag Station");
                public static readonly FlagDef openedRestingGrounds = new FlagDef("openedRestingGrounds", null, false, "PlayerData_Bool", "Resting Grounds Stag Station");
                public static readonly FlagDef openedGreenpath = new FlagDef("openedGreenpath", null, false, "PlayerData_Bool", "Greenpath Stag Station");
                public static readonly FlagDef openedDeepnest = new FlagDef("openedDeepnest", null, false, "PlayerData_Bool", "Deepnest Stag Station");
                public static readonly FlagDef openedRuins2 = new FlagDef("openedRuins2", null, false, "PlayerData_Bool", "King's Station Stag Station");
            #endregion

        #endregion

        #region Game State
            public static readonly FlagDef currentArea = new FlagDef("currentArea", null, false, "PlayerData_Int",  "Current Area");
            public static readonly FlagDef isFirstGame = new FlagDef("isFirstGame", null, false, "PlayerData_Bool");
            public static readonly FlagDef permadeathMode = new FlagDef("permadeathMode", null, false, "PlayerData_Int",  "Steel Soul Mode (Permadeath)");
            public static readonly FlagDef playTime = new FlagDef("playTime", null, false, "PlayerData_Single");
            public static readonly FlagDef profileID = new FlagDef("profileID", null, false, "PlayerData_Int", "Save slot of current game");
            public static readonly FlagDef unlockedCompletionRate = new FlagDef("unlockedCompletionRate", null, false, "PlayerData_Bool", "World Sense");
        #endregion

        #region Grub Bottles
            /// <summary>
            /// Related Flags
            /// </summary>
            /// 
            public static readonly FlagDef grubsCollected = new FlagDef("grubsCollected", null, false, "PlayerData_Int", "Total Grubs Collected");
            public static readonly FlagDef grubRewards = new FlagDef("grubRewards", null, false, "PlayerData_Int", "Grub Rewards Received");
            // List of scenes that grubs were found in
            public static readonly FlagDef scenesGrubRescued = new FlagDef("scenesGrubRescued", null, false, "PlayerData_List", "Scenes where Grubs were rescued");

            #region Individual Bottles
                public static readonly FlagDef Abyss_17__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Abyss_17, false, "PersistentBoolData");
                public static readonly FlagDef Abyss_19__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Abyss_19, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_03__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Crossroads_03, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_05__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Crossroads_05, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_31__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Crossroads_31, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_35__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Crossroads_35, false, "PersistentBoolData");
                public static readonly FlagDef Crossroads_48__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Crossroads_48, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_03__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Deepnest_03, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_31__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Deepnest_31, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_36__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Deepnest_36, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_39__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Deepnest_39, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_East_11__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Deepnest_East_11, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_East_14__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Deepnest_East_14, false, "PersistentBoolData");
                public static readonly FlagDef Deepnest_Spider_Town__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_06__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Fungus1_06, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_07__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Fungus1_07, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_13__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Fungus1_13, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_21__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Fungus1_21, false, "PersistentBoolData");
                public static readonly FlagDef Fungus1_28__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Fungus1_28, false, "PersistentBoolData");
                public static readonly FlagDef Fungus2_18__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Fungus2_18, false, "PersistentBoolData");
                public static readonly FlagDef Fungus2_20__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Fungus2_20, false, "PersistentBoolData");
                public static readonly FlagDef Fungus3_10__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Fungus3_10, false, "PersistentBoolData");
                public static readonly FlagDef Fungus3_22__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Fungus3_22, false, "PersistentBoolData");
                public static readonly FlagDef Fungus3_47__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Fungus3_47, false, "PersistentBoolData");
                public static readonly FlagDef Fungus3_48__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Fungus3_48, false, "PersistentBoolData");
                public static readonly FlagDef Hive_03__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Hive_03, false, "PersistentBoolData");
                public static readonly FlagDef Hive_04__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Hive_04, false, "PersistentBoolData");
                public static readonly FlagDef Mines_03__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Mines_03, false, "PersistentBoolData");
                public static readonly FlagDef Mines_04__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Mines_04, false, "PersistentBoolData");
                public static readonly FlagDef Mines_16__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Mines_16, false, "PersistentBoolData");
                public static readonly FlagDef Mines_19__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Mines_19, false, "PersistentBoolData");
                public static readonly FlagDef Mines_24__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Mines_24, false, "PersistentBoolData");
                public static readonly FlagDef Mines_31__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Mines_31, false, "PersistentBoolData");
                public static readonly FlagDef Mines_35__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Mines_35, false, "PersistentBoolData");
                public static readonly FlagDef RestingGrounds_10__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.RestingGrounds_10, false, "PersistentBoolData");
                public static readonly FlagDef Ruins1_05__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Ruins1_05, false, "PersistentBoolData");
                public static readonly FlagDef Ruins1_32__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Ruins1_32, false, "PersistentBoolData");
                public static readonly FlagDef Ruins2_03__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Ruins2_03, false, "PersistentBoolData");
                public static readonly FlagDef Ruins2_07__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Ruins2_07, false, "PersistentBoolData");
                public static readonly FlagDef Ruins2_11__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Ruins2_11, false, "PersistentBoolData");
                public static readonly FlagDef Ruins_House_01__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Ruins_House_01, false, "PersistentBoolData");
                public static readonly FlagDef Waterways_04__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Waterways_04, false, "PersistentBoolData");
                public static readonly FlagDef Waterways_13__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Waterways_13, false, "PersistentBoolData");
                public static readonly FlagDef Waterways_14__Grub_Bottle = new FlagDef("Grub Bottle", SceneInstances.Waterways_14, false, "PersistentBoolData");
            #endregion

        #endregion

        #region Hunter
            public static readonly FlagDef journalEntriesCompleted = new FlagDef("journalEntriesCompleted", null, false, "PlayerData_Int", "Journal Entries Completed, updates at bench");
            public static readonly FlagDef journalEntriesTotal = new FlagDef("journalEntriesTotal", null, false, "PlayerData_Int", "Journal Entries Total, updates at bench");
            public static readonly FlagDef journalNotesCompleted = new FlagDef("journalNotesCompleted", null, false, "PlayerData_Int", "Journal Notes Completed, updates at bench");

            #region Killed Enemy
                public static readonly FlagDef killedAbyssCrawler = new FlagDef("killedAbyssCrawler", null, false, "PlayerData_Bool",                   "Shadow Creeper");
                public static readonly FlagDef killedAbyssTendril = new FlagDef("killedAbyssTendril", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedAcidFlyer = new FlagDef("killedAcidFlyer", null, false, "PlayerData_Bool",                         "Duranda");
                public static readonly FlagDef killedAcidWalker = new FlagDef("killedAcidWalker", null, false, "PlayerData_Bool",                       "Durandoo");
                public static readonly FlagDef killedAngryBuzzer = new FlagDef("killedAngryBuzzer", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedBabyCentipede = new FlagDef("killedBabyCentipede", null, false, "PlayerData_Bool",                 "Dirtcarver");
                public static readonly FlagDef killedBeamMiner = new FlagDef("killedBeamMiner", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedBeeHatchling = new FlagDef("killedBeeHatchling", null, false, "PlayerData_Bool",                   "Hiveling");
                public static readonly FlagDef killedBeeStinger = new FlagDef("killedBeeStinger", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedBigBee = new FlagDef("killedBigBee", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedBigBuzzer = new FlagDef("killedBigBuzzer", null, false, "PlayerData_Bool",                         "Vengefly King");
                public static readonly FlagDef killedBigCentipede = new FlagDef("killedBigCentipede", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedBigFly = new FlagDef("killedBigFly", null, false, "PlayerData_Bool",                               "Gruz Mother");
                public static readonly FlagDef killedBindingSeal = new FlagDef("killedBindingSeal", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedBlackKnight = new FlagDef("killedBlackKnight", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedBlobFlyer = new FlagDef("killedBlobFlyer", null, false, "PlayerData_Bool",                         "Obble");
                public static readonly FlagDef killedBlobble = new FlagDef("killedBlobble", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedBlocker = new FlagDef("killedBlocker", null, false, "PlayerData_Bool",                             "Elder Baldur");
                public static readonly FlagDef killedBlowFly = new FlagDef("killedBlowFly", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedBouncer = new FlagDef("killedBouncer", null, false, "PlayerData_Bool",                             "Gruzzer");
                public static readonly FlagDef killedBurstingBouncer = new FlagDef("killedBurstingBouncer", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedBurstingZombie = new FlagDef("killedBurstingZombie", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedBuzzer = new FlagDef("killedBuzzer", null, false, "PlayerData_Bool",                               "Vengefly");
                public static readonly FlagDef killedCeilingDropper = new FlagDef("killedCeilingDropper", null, false, "PlayerData_Bool",               "Belfly");
                public static readonly FlagDef killedCentipedeHatcher = new FlagDef("killedCentipedeHatcher", null, false, "PlayerData_Bool",           "Carver Hatcher");
                public static readonly FlagDef killedClimber = new FlagDef("killedClimber", null, false, "PlayerData_Bool",                             "Tiktik");
                public static readonly FlagDef killedColFlyingSentry = new FlagDef("killedColFlyingSentry", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedColHopper = new FlagDef("killedColHopper", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedColMiner = new FlagDef("killedColMiner", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedColMosquito = new FlagDef("killedColMosquito", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedColRoller = new FlagDef("killedColRoller", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedColShield = new FlagDef("killedColShield", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedColWorm = new FlagDef("killedColWorm", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedCrawler = new FlagDef("killedCrawler", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedCrystalCrawler = new FlagDef("killedCrystalCrawler", null, false, "PlayerData_Bool",               "Glimback");
                public static readonly FlagDef killedCrystalFlyer = new FlagDef("killedCrystalFlyer", null, false, "PlayerData_Bool",                   "Crystal Hunter");
                public static readonly FlagDef killedDreamGuard = new FlagDef("killedDreamGuard", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedDummy = new FlagDef("killedDummy", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedDungDefender = new FlagDef("killedDungDefender", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedEggSac = new FlagDef("killedEggSac", null, false, "PlayerData_Bool",                               "Bluggsac");
                public static readonly FlagDef killedElectricMage = new FlagDef("killedElectricMage", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedFalseKnight = new FlagDef("killedFalseKnight", null, false, "PlayerData_Bool",                     "False Knight");
                public static readonly FlagDef killedFatFluke = new FlagDef("killedFatFluke", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedFinalBoss = new FlagDef("killedFinalBoss", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedFlameBearerLarge = new FlagDef("killedFlameBearerLarge", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedFlameBearerMed = new FlagDef("killedFlameBearerMed", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedFlameBearerSmall = new FlagDef("killedFlameBearerSmall", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedFlipHopper = new FlagDef("killedFlipHopper", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedFlukeMother = new FlagDef("killedFlukeMother", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedFlukefly = new FlagDef("killedFlukefly", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedFlukeman = new FlagDef("killedFlukeman", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedFlyingSentryJavelin = new FlagDef("killedFlyingSentryJavelin", null, false, "PlayerData_Bool",     "Lance Sentry");
                public static readonly FlagDef killedFlyingSentrySword = new FlagDef("killedFlyingSentrySword", null, false, "PlayerData_Bool",         "Winged Sentry");
                public static readonly FlagDef killedFungCrawler = new FlagDef("killedFungCrawler", null, false, "PlayerData_Bool",                     "Ambloom");
                public static readonly FlagDef killedFungifiedZombie = new FlagDef("killedFungifiedZombie", null, false, "PlayerData_Bool",             "Fungified Husk");
                public static readonly FlagDef killedFungoonBaby = new FlagDef("killedFungoonBaby", null, false, "PlayerData_Bool",                     "Fungling");
                public static readonly FlagDef killedFungusFlyer = new FlagDef("killedFungusFlyer", null, false, "PlayerData_Bool",                     "Fungoon");
                public static readonly FlagDef killedGardenZombie = new FlagDef("killedGardenZombie", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedGhostAladar = new FlagDef("killedGhostAladar", null, false, "PlayerData_Bool",                     "Gorb");
                public static readonly FlagDef killedGhostGalien = new FlagDef("killedGhostGalien", null, false, "PlayerData_Bool",                     "Galien");
                public static readonly FlagDef killedGhostHu = new FlagDef("killedGhostHu", null, false, "PlayerData_Bool",                             "Elder Hu");
                public static readonly FlagDef killedGhostMarkoth = new FlagDef("killedGhostMarkoth", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedGhostMarmu = new FlagDef("killedGhostMarmu", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedGhostNoEyes = new FlagDef("killedGhostNoEyes", null, false, "PlayerData_Bool",                     "No Eyes");
                public static readonly FlagDef killedGhostXero = new FlagDef("killedGhostXero", null, false, "PlayerData_Bool",                         "Xero");
                public static readonly FlagDef killedGiantHopper = new FlagDef("killedGiantHopper", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedGodseekerMask = new FlagDef("killedGodseekerMask", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedGorgeousHusk = new FlagDef("killedGorgeousHusk", null, false, "PlayerData_Bool",                   "Gorgeous Husk");
                public static readonly FlagDef killedGrassHopper = new FlagDef("killedGrassHopper", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedGreatShieldZombie = new FlagDef("killedGreatShieldZombie", null, false, "PlayerData_Bool",         "Great Husk Sentry");
                public static readonly FlagDef killedGreyPrince = new FlagDef("killedGreyPrince", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedGrimm = new FlagDef("killedGrimm", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedGrubMimic = new FlagDef("killedGrubMimic", null, false, "PlayerData_Bool",                         "Grub Mimic");
                public static readonly FlagDef killedHatcher = new FlagDef("killedHatcher", null, false, "PlayerData_Bool",                             "Aspid Mother");
                public static readonly FlagDef killedHatchling = new FlagDef("killedHatchling", null, false, "PlayerData_Bool",                         "Aspid Hatchling");
                public static readonly FlagDef killedHealthScuttler = new FlagDef("killedHealthScuttler", null, false, "PlayerData_Bool",               "Lifeseed");
                public static readonly FlagDef killedHeavyMantis = new FlagDef("killedHeavyMantis", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedHiveKnight = new FlagDef("killedHiveKnight", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedHollowKnight = new FlagDef("killedHollowKnight", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedHollowKnightPrime = new FlagDef("killedHollowKnightPrime", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedHopper = new FlagDef("killedHopper", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedHornet = new FlagDef("killedHornet", null, false, "PlayerData_Bool",                               "Hornet");
                public static readonly FlagDef killedHunterMark = new FlagDef("killedHunterMark", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedInfectedKnight = new FlagDef("killedInfectedKnight", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedInflater = new FlagDef("killedInflater", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedJarCollector = new FlagDef("killedJarCollector", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedJellyCrawler = new FlagDef("killedJellyCrawler", null, false, "PlayerData_Bool",                   "Uoma");
                public static readonly FlagDef killedJellyfish = new FlagDef("killedJellyfish", null, false, "PlayerData_Bool",                         "Ooma");
                public static readonly FlagDef killedLaserBug = new FlagDef("killedLaserBug", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedLazyFlyer = new FlagDef("killedLazyFlyer", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedLesserMawlek = new FlagDef("killedLesserMawlek", null, false, "PlayerData_Bool",                   "Lesser Mawlek");
                public static readonly FlagDef killedLobsterLancer = new FlagDef("killedLobsterLancer", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedMage = new FlagDef("killedMage", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedMageBalloon = new FlagDef("killedMageBalloon", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedMageBlob = new FlagDef("killedMageBlob", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedMageKnight = new FlagDef("killedMageKnight", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedMageLord = new FlagDef("killedMageLord", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedMantis = new FlagDef("killedMantis", null, false, "PlayerData_Bool",                               "Mantis Warrior");
                public static readonly FlagDef killedMantisFlyerChild = new FlagDef("killedMantisFlyerChild", null, false, "PlayerData_Bool",           "Mantis Youth");
                public static readonly FlagDef killedMantisHeavyFlyer = new FlagDef("killedMantisHeavyFlyer", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedMantisLord = new FlagDef("killedMantisLord", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedMawlek = new FlagDef("killedMawlek", null, false, "PlayerData_Bool",                               "Brooding Mawlek");
                public static readonly FlagDef killedMawlekTurret = new FlagDef("killedMawlekTurret", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedMegaBeamMiner = new FlagDef("killedMegaBeamMiner", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedMegaJellyfish = new FlagDef("killedMegaJellyfish", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedMegaMossCharger = new FlagDef("killedMegaMossCharger", null, false, "PlayerData_Bool",             "Massive Moss Charger");
                public static readonly FlagDef killedMenderBug = new FlagDef("killedMenderBug", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedMimicSpider = new FlagDef("killedMimicSpider", null, false, "PlayerData_Bool",                     "Deepling");
                public static readonly FlagDef killedMinesCrawler = new FlagDef("killedMinesCrawler", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedMiniSpider = new FlagDef("killedMiniSpider", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedMosquito = new FlagDef("killedMosquito", null, false, "PlayerData_Bool",                           "Squit");
                public static readonly FlagDef killedMossCharger = new FlagDef("killedMossCharger", null, false, "PlayerData_Bool",                     "Moss Charger");
                public static readonly FlagDef killedMossFlyer = new FlagDef("killedMossFlyer", null, false, "PlayerData_Bool",                         "Mossfly");
                public static readonly FlagDef killedMossKnight = new FlagDef("killedMossKnight", null, false, "PlayerData_Bool",                       "Moss Knight");
                public static readonly FlagDef killedMossKnightFat = new FlagDef("killedMossKnightFat", null, false, "PlayerData_Bool",                 "Mossy Vagabond");
                public static readonly FlagDef killedMossWalker = new FlagDef("killedMossWalker", null, false, "PlayerData_Bool",                       "Mosscreep");
                public static readonly FlagDef killedMossmanRunner = new FlagDef("killedMossmanRunner", null, false, "PlayerData_Bool",                 "Mosskin");
                public static readonly FlagDef killedMossmanShaker = new FlagDef("killedMossmanShaker", null, false, "PlayerData_Bool",                 "Volatile Mosskin");
                public static readonly FlagDef killedMummy = new FlagDef("killedMummy", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedMushroomBaby = new FlagDef("killedMushroomBaby", null, false, "PlayerData_Bool",                   "Shrumeling");
                public static readonly FlagDef killedMushroomBrawler = new FlagDef("killedMushroomBrawler", null, false, "PlayerData_Bool",             "Shrumal Ogre");
                public static readonly FlagDef killedMushroomRoller = new FlagDef("killedMushroomRoller", null, false, "PlayerData_Bool",               "Shrumal Warrior");
                public static readonly FlagDef killedMushroomTurret = new FlagDef("killedMushroomTurret", null, false, "PlayerData_Bool",               "Sporg");
                public static readonly FlagDef killedNailBros = new FlagDef("killedNailBros", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedNailsage = new FlagDef("killedNailsage", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedNightmareGrimm = new FlagDef("killedNightmareGrimm", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedOblobble = new FlagDef("killedOblobble", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedOrangeBalloon = new FlagDef("killedOrangeBalloon", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedOrangeScuttler = new FlagDef("killedOrangeScuttler", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedPaintmaster = new FlagDef("killedPaintmaster", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedPalaceFly = new FlagDef("killedPalaceFly", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedPaleLurker = new FlagDef("killedPaleLurker", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedPigeon = new FlagDef("killedPigeon", null, false, "PlayerData_Bool",                               "Maskfly");
                public static readonly FlagDef killedPlantShooter = new FlagDef("killedPlantShooter", null, false, "PlayerData_Bool",                   "Gulka");
                public static readonly FlagDef killedPrayerSlug = new FlagDef("killedPrayerSlug", null, false, "PlayerData_Bool",                       "Maggot");
                public static readonly FlagDef killedRoller = new FlagDef("killedRoller", null, false, "PlayerData_Bool",                               "Baldur");
                public static readonly FlagDef killedRoyalCoward = new FlagDef("killedRoyalCoward", null, false, "PlayerData_Bool",                     "Cowardly Husk");
                public static readonly FlagDef killedRoyalDandy = new FlagDef("killedRoyalDandy", null, false, "PlayerData_Bool",                       "Husk Dandy");
                public static readonly FlagDef killedRoyalGuard = new FlagDef("killedRoyalGuard", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedRoyalPlumper = new FlagDef("killedRoyalPlumper", null, false, "PlayerData_Bool",                   "Gluttonous Husk");
                public static readonly FlagDef killedSentry = new FlagDef("killedSentry", null, false, "PlayerData_Bool",                               "Husk Sentry");
                public static readonly FlagDef killedSentryFat = new FlagDef("killedSentryFat", null, false, "PlayerData_Bool",                         "Heavy Sentry");
                public static readonly FlagDef killedShootSpider = new FlagDef("killedShootSpider", null, false, "PlayerData_Bool",                     "Deephunter");
                public static readonly FlagDef killedSibling = new FlagDef("killedSibling", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedSlashSpider = new FlagDef("killedSlashSpider", null, false, "PlayerData_Bool",                     "Stalking Devout");
                public static readonly FlagDef killedSnapperTrap = new FlagDef("killedSnapperTrap", null, false, "PlayerData_Bool",                     "Fool Eater");
                public static readonly FlagDef killedSpiderCorpse = new FlagDef("killedSpiderCorpse", null, false, "PlayerData_Bool",                   "Corpse Creeper");
                public static readonly FlagDef killedSpiderFlyer = new FlagDef("killedSpiderFlyer", null, false, "PlayerData_Bool",                     "Little Weaver");
                public static readonly FlagDef killedSpitter = new FlagDef("killedSpitter", null, false, "PlayerData_Bool",                             "Aspid Hunter");
                public static readonly FlagDef killedSpittingZombie = new FlagDef("killedSpittingZombie", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedSuperSpitter = new FlagDef("killedSuperSpitter", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedTraitorLord = new FlagDef("killedTraitorLord", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedVoidIdol_1 = new FlagDef("killedVoidIdol_1", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedVoidIdol_2 = new FlagDef("killedVoidIdol_2", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedVoidIdol_3 = new FlagDef("killedVoidIdol_3", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedWhiteDefender = new FlagDef("killedWhiteDefender", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedWhiteRoyal = new FlagDef("killedWhiteRoyal", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedWorm = new FlagDef("killedWorm", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedZapBug = new FlagDef("killedZapBug", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedZombieBarger = new FlagDef("killedZombieBarger", null, false, "PlayerData_Bool",                   "Husk Bully");
                public static readonly FlagDef killedZombieGuard = new FlagDef("killedZombieGuard", null, false, "PlayerData_Bool",                     "Husk Guard");
                public static readonly FlagDef killedZombieHive = new FlagDef("killedZombieHive", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedZombieHornhead = new FlagDef("killedZombieHornhead", null, false, "PlayerData_Bool",               "Husk Hornhead");
                public static readonly FlagDef killedZombieLeaper = new FlagDef("killedZombieLeaper", null, false, "PlayerData_Bool",                   "Leaping Husk");
                public static readonly FlagDef killedZombieMiner = new FlagDef("killedZombieMiner", null, false, "PlayerData_Bool",                     "Husk Miner");
                public static readonly FlagDef killedZombieRunner = new FlagDef("killedZombieRunner", null, false, "PlayerData_Bool",                   "Wandering Husk");
                public static readonly FlagDef killedZombieShield = new FlagDef("killedZombieShield", null, false, "PlayerData_Bool",                   "Husk Warrior");
                public static readonly FlagDef killedZote = new FlagDef("killedZote", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedZotelingBalloon = new FlagDef("killedZotelingBalloon", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedZotelingBuzzer = new FlagDef("killedZotelingBuzzer", null, false, "PlayerData_Bool");
                public static readonly FlagDef killedZotelingHopper = new FlagDef("killedZotelingHopper", null, false, "PlayerData_Bool");
            #endregion

            #region Kills of Enemy
                public static readonly FlagDef killsAbyssCrawler = new FlagDef("killsAbyssCrawler", null, false, "PlayerData_Int");
                public static readonly FlagDef killsAbyssTendril = new FlagDef("killsAbyssTendril", null, false, "PlayerData_Int");
                public static readonly FlagDef killsAcidFlyer = new FlagDef("killsAcidFlyer", null, false, "PlayerData_Int");
                public static readonly FlagDef killsAcidWalker = new FlagDef("killsAcidWalker", null, false, "PlayerData_Int");
                public static readonly FlagDef killsAngryBuzzer = new FlagDef("killsAngryBuzzer", null, false, "PlayerData_Int");
                public static readonly FlagDef killsBabyCentipede = new FlagDef("killsBabyCentipede", null, false, "PlayerData_Int");
                public static readonly FlagDef killsBeamMiner = new FlagDef("killsBeamMiner", null, false, "PlayerData_Int");
                public static readonly FlagDef killsBeeHatchling = new FlagDef("killsBeeHatchling", null, false, "PlayerData_Int");
                public static readonly FlagDef killsBeeStinger = new FlagDef("killsBeeStinger", null, false, "PlayerData_Int");
                public static readonly FlagDef killsBigBee = new FlagDef("killsBigBee", null, false, "PlayerData_Int");
                public static readonly FlagDef killsBigBuzzer = new FlagDef("killsBigBuzzer", null, false, "PlayerData_Int");
                public static readonly FlagDef killsBigCentipede = new FlagDef("killsBigCentipede", null, false, "PlayerData_Int");
                public static readonly FlagDef killsBigFly = new FlagDef("killsBigFly", null, false, "PlayerData_Int");
                public static readonly FlagDef killsBindingSeal = new FlagDef("killsBindingSeal", null, false, "PlayerData_Int");
                public static readonly FlagDef killsBlackKnight = new FlagDef("killsBlackKnight", null, false, "PlayerData_Int");
                public static readonly FlagDef killsBlobFlyer = new FlagDef("killsBlobFlyer", null, false, "PlayerData_Int");
                public static readonly FlagDef killsBlobble = new FlagDef("killsBlobble", null, false, "PlayerData_Int");
                public static readonly FlagDef killsBlocker = new FlagDef("killsBlocker", null, false, "PlayerData_Int");
                public static readonly FlagDef killsBlowFly = new FlagDef("killsBlowFly", null, false, "PlayerData_Int");
                public static readonly FlagDef killsBouncer = new FlagDef("killsBouncer", null, false, "PlayerData_Int");
                public static readonly FlagDef killsBurstingBouncer = new FlagDef("killsBurstingBouncer", null, false, "PlayerData_Int");
                public static readonly FlagDef killsBurstingZombie = new FlagDef("killsBurstingZombie", null, false, "PlayerData_Int");
                public static readonly FlagDef killsBuzzer = new FlagDef("killsBuzzer", null, false, "PlayerData_Int");
                public static readonly FlagDef killsCeilingDropper = new FlagDef("killsCeilingDropper", null, false, "PlayerData_Int");
                public static readonly FlagDef killsCentipedeHatcher = new FlagDef("killsCentipedeHatcher", null, false, "PlayerData_Int");
                public static readonly FlagDef killsClimber = new FlagDef("killsClimber", null, false, "PlayerData_Int");
                public static readonly FlagDef killsColFlyingSentry = new FlagDef("killsColFlyingSentry", null, false, "PlayerData_Int");
                public static readonly FlagDef killsColHopper = new FlagDef("killsColHopper", null, false, "PlayerData_Int");
                public static readonly FlagDef killsColMiner = new FlagDef("killsColMiner", null, false, "PlayerData_Int");
                public static readonly FlagDef killsColMosquito = new FlagDef("killsColMosquito", null, false, "PlayerData_Int");
                public static readonly FlagDef killsColRoller = new FlagDef("killsColRoller", null, false, "PlayerData_Int");
                public static readonly FlagDef killsColShield = new FlagDef("killsColShield", null, false, "PlayerData_Int");
                public static readonly FlagDef killsColWorm = new FlagDef("killsColWorm", null, false, "PlayerData_Int");
                public static readonly FlagDef killsCrawler = new FlagDef("killsCrawler", null, false, "PlayerData_Int");
                public static readonly FlagDef killsCrystalCrawler = new FlagDef("killsCrystalCrawler", null, false, "PlayerData_Int");
                public static readonly FlagDef killsCrystalFlyer = new FlagDef("killsCrystalFlyer", null, false, "PlayerData_Int");
                public static readonly FlagDef killsDreamGuard = new FlagDef("killsDreamGuard", null, false, "PlayerData_Int");
                public static readonly FlagDef killsDummy = new FlagDef("killsDummy", null, false, "PlayerData_Int");
                public static readonly FlagDef killsDungDefender = new FlagDef("killsDungDefender", null, false, "PlayerData_Int");
                public static readonly FlagDef killsEggSac = new FlagDef("killsEggSac", null, false, "PlayerData_Int");
                public static readonly FlagDef killsElectricMage = new FlagDef("killsElectricMage", null, false, "PlayerData_Int");
                public static readonly FlagDef killsFalseKnight = new FlagDef("killsFalseKnight", null, false, "PlayerData_Int");
                public static readonly FlagDef killsFatFluke = new FlagDef("killsFatFluke", null, false, "PlayerData_Int");
                public static readonly FlagDef killsFinalBoss = new FlagDef("killsFinalBoss", null, false, "PlayerData_Int");
                public static readonly FlagDef killsFlameBearerLarge = new FlagDef("killsFlameBearerLarge", null, false, "PlayerData_Int");
                public static readonly FlagDef killsFlameBearerMed = new FlagDef("killsFlameBearerMed", null, false, "PlayerData_Int");
                public static readonly FlagDef killsFlameBearerSmall = new FlagDef("killsFlameBearerSmall", null, false, "PlayerData_Int");
                public static readonly FlagDef killsFlipHopper = new FlagDef("killsFlipHopper", null, false, "PlayerData_Int");
                public static readonly FlagDef killsFlukeMother = new FlagDef("killsFlukeMother", null, false, "PlayerData_Int");
                public static readonly FlagDef killsFlukefly = new FlagDef("killsFlukefly", null, false, "PlayerData_Int");
                public static readonly FlagDef killsFlukeman = new FlagDef("killsFlukeman", null, false, "PlayerData_Int");
                public static readonly FlagDef killsFlyingSentryJavelin = new FlagDef("killsFlyingSentryJavelin", null, false, "PlayerData_Int");
                public static readonly FlagDef killsFlyingSentrySword = new FlagDef("killsFlyingSentrySword", null, false, "PlayerData_Int");
                public static readonly FlagDef killsFungCrawler = new FlagDef("killsFungCrawler", null, false, "PlayerData_Int");
                public static readonly FlagDef killsFungifiedZombie = new FlagDef("killsFungifiedZombie", null, false, "PlayerData_Int");
                public static readonly FlagDef killsFungoonBaby = new FlagDef("killsFungoonBaby", null, false, "PlayerData_Int");
                public static readonly FlagDef killsFungusFlyer = new FlagDef("killsFungusFlyer", null, false, "PlayerData_Int");
                public static readonly FlagDef killsGardenZombie = new FlagDef("killsGardenZombie", null, false, "PlayerData_Int");
                public static readonly FlagDef killsGhostAladar = new FlagDef("killsGhostAladar", null, false, "PlayerData_Int");
                public static readonly FlagDef killsGhostGalien = new FlagDef("killsGhostGalien", null, false, "PlayerData_Int");
                public static readonly FlagDef killsGhostHu = new FlagDef("killsGhostHu", null, false, "PlayerData_Int");
                public static readonly FlagDef killsGhostMarkoth = new FlagDef("killsGhostMarkoth", null, false, "PlayerData_Int");
                public static readonly FlagDef killsGhostMarmu = new FlagDef("killsGhostMarmu", null, false, "PlayerData_Int");
                public static readonly FlagDef killsGhostNoEyes = new FlagDef("killsGhostNoEyes", null, false, "PlayerData_Int");
                public static readonly FlagDef killsGhostXero = new FlagDef("killsGhostXero", null, false, "PlayerData_Int");
                public static readonly FlagDef killsGiantHopper = new FlagDef("killsGiantHopper", null, false, "PlayerData_Int");
                public static readonly FlagDef killsGodseekerMask = new FlagDef("killsGodseekerMask", null, false, "PlayerData_Int");
                public static readonly FlagDef killsGorgeousHusk = new FlagDef("killsGorgeousHusk", null, false, "PlayerData_Int");
                public static readonly FlagDef killsGrassHopper = new FlagDef("killsGrassHopper", null, false, "PlayerData_Int");
                public static readonly FlagDef killsGreatShieldZombie = new FlagDef("killsGreatShieldZombie", null, false, "PlayerData_Int");
                public static readonly FlagDef killsGreyPrince = new FlagDef("killsGreyPrince", null, false, "PlayerData_Int");
                public static readonly FlagDef killsGrimm = new FlagDef("killsGrimm", null, false, "PlayerData_Int");
                public static readonly FlagDef killsGrubMimic = new FlagDef("killsGrubMimic", null, false, "PlayerData_Int");
                public static readonly FlagDef killsHatcher = new FlagDef("killsHatcher", null, false, "PlayerData_Int");
                public static readonly FlagDef killsHatchling = new FlagDef("killsHatchling", null, false, "PlayerData_Int");
                public static readonly FlagDef killsHealthScuttler = new FlagDef("killsHealthScuttler", null, false, "PlayerData_Int");
                public static readonly FlagDef killsHeavyMantis = new FlagDef("killsHeavyMantis", null, false, "PlayerData_Int");
                public static readonly FlagDef killsHiveKnight = new FlagDef("killsHiveKnight", null, false, "PlayerData_Int");
                public static readonly FlagDef killsHollowKnight = new FlagDef("killsHollowKnight", null, false, "PlayerData_Int");
                public static readonly FlagDef killsHollowKnightPrime = new FlagDef("killsHollowKnightPrime", null, false, "PlayerData_Int");
                public static readonly FlagDef killsHopper = new FlagDef("killsHopper", null, false, "PlayerData_Int");
                public static readonly FlagDef killsHornet = new FlagDef("killsHornet", null, false, "PlayerData_Int");
                public static readonly FlagDef killsHunterMark = new FlagDef("killsHunterMark", null, false, "PlayerData_Int");
                public static readonly FlagDef killsInfectedKnight = new FlagDef("killsInfectedKnight", null, false, "PlayerData_Int");
                public static readonly FlagDef killsInflater = new FlagDef("killsInflater", null, false, "PlayerData_Int");
                public static readonly FlagDef killsJarCollector = new FlagDef("killsJarCollector", null, false, "PlayerData_Int");
                public static readonly FlagDef killsJellyCrawler = new FlagDef("killsJellyCrawler", null, false, "PlayerData_Int");
                public static readonly FlagDef killsJellyfish = new FlagDef("killsJellyfish", null, false, "PlayerData_Int");
                public static readonly FlagDef killsLaserBug = new FlagDef("killsLaserBug", null, false, "PlayerData_Int");
                public static readonly FlagDef killsLazyFlyer = new FlagDef("killsLazyFlyer", null, false, "PlayerData_Int");
                public static readonly FlagDef killsLesserMawlek = new FlagDef("killsLesserMawlek", null, false, "PlayerData_Int");
                public static readonly FlagDef killsLobsterLancer = new FlagDef("killsLobsterLancer", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMage = new FlagDef("killsMage", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMageBalloon = new FlagDef("killsMageBalloon", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMageBlob = new FlagDef("killsMageBlob", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMageKnight = new FlagDef("killsMageKnight", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMageLord = new FlagDef("killsMageLord", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMantis = new FlagDef("killsMantis", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMantisFlyerChild = new FlagDef("killsMantisFlyerChild", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMantisHeavyFlyer = new FlagDef("killsMantisHeavyFlyer", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMantisLord = new FlagDef("killsMantisLord", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMawlek = new FlagDef("killsMawlek", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMawlekTurret = new FlagDef("killsMawlekTurret", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMegaBeamMiner = new FlagDef("killsMegaBeamMiner", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMegaJellyfish = new FlagDef("killsMegaJellyfish", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMegaMossCharger = new FlagDef("killsMegaMossCharger", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMenderBug = new FlagDef("killsMenderBug", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMimicSpider = new FlagDef("killsMimicSpider", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMinesCrawler = new FlagDef("killsMinesCrawler", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMiniSpider = new FlagDef("killsMiniSpider", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMosquito = new FlagDef("killsMosquito", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMossCharger = new FlagDef("killsMossCharger", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMossFlyer = new FlagDef("killsMossFlyer", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMossKnight = new FlagDef("killsMossKnight", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMossKnightFat = new FlagDef("killsMossKnightFat", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMossWalker = new FlagDef("killsMossWalker", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMossmanRunner = new FlagDef("killsMossmanRunner", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMossmanShaker = new FlagDef("killsMossmanShaker", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMummy = new FlagDef("killsMummy", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMushroomBaby = new FlagDef("killsMushroomBaby", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMushroomBrawler = new FlagDef("killsMushroomBrawler", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMushroomRoller = new FlagDef("killsMushroomRoller", null, false, "PlayerData_Int");
                public static readonly FlagDef killsMushroomTurret = new FlagDef("killsMushroomTurret", null, false, "PlayerData_Int");
                public static readonly FlagDef killsNailBros = new FlagDef("killsNailBros", null, false, "PlayerData_Int");
                public static readonly FlagDef killsNailsage = new FlagDef("killsNailsage", null, false, "PlayerData_Int");
                public static readonly FlagDef killsNightmareGrimm = new FlagDef("killsNightmareGrimm", null, false, "PlayerData_Int");
                public static readonly FlagDef killsOblobble = new FlagDef("killsOblobble", null, false, "PlayerData_Int");
                public static readonly FlagDef killsOrangeBalloon = new FlagDef("killsOrangeBalloon", null, false, "PlayerData_Int");
                public static readonly FlagDef killsOrangeScuttler = new FlagDef("killsOrangeScuttler", null, false, "PlayerData_Int");
                public static readonly FlagDef killsPaintmaster = new FlagDef("killsPaintmaster", null, false, "PlayerData_Int");
                public static readonly FlagDef killsPalaceFly = new FlagDef("killsPalaceFly", null, false, "PlayerData_Int");
                public static readonly FlagDef killsPaleLurker = new FlagDef("killsPaleLurker", null, false, "PlayerData_Int");
                public static readonly FlagDef killsPigeon = new FlagDef("killsPigeon", null, false, "PlayerData_Int");
                public static readonly FlagDef killsPlantShooter = new FlagDef("killsPlantShooter", null, false, "PlayerData_Int");
                public static readonly FlagDef killsPrayerSlug = new FlagDef("killsPrayerSlug", null, false, "PlayerData_Int");
                public static readonly FlagDef killsRoller = new FlagDef("killsRoller", null, false, "PlayerData_Int");
                public static readonly FlagDef killsRoyalCoward = new FlagDef("killsRoyalCoward", null, false, "PlayerData_Int");
                public static readonly FlagDef killsRoyalDandy = new FlagDef("killsRoyalDandy", null, false, "PlayerData_Int");
                public static readonly FlagDef killsRoyalGuard = new FlagDef("killsRoyalGuard", null, false, "PlayerData_Int");
                public static readonly FlagDef killsRoyalPlumper = new FlagDef("killsRoyalPlumper", null, false, "PlayerData_Int");
                public static readonly FlagDef killsSentry = new FlagDef("killsSentry", null, false, "PlayerData_Int");
                public static readonly FlagDef killsSentryFat = new FlagDef("killsSentryFat", null, false, "PlayerData_Int");
                public static readonly FlagDef killsShootSpider = new FlagDef("killsShootSpider", null, false, "PlayerData_Int");
                public static readonly FlagDef killsSibling = new FlagDef("killsSibling", null, false, "PlayerData_Int");
                public static readonly FlagDef killsSlashSpider = new FlagDef("killsSlashSpider", null, false, "PlayerData_Int");
                public static readonly FlagDef killsSnapperTrap = new FlagDef("killsSnapperTrap", null, false, "PlayerData_Int");
                public static readonly FlagDef killsSpiderCorpse = new FlagDef("killsSpiderCorpse", null, false, "PlayerData_Int");
                public static readonly FlagDef killsSpiderFlyer = new FlagDef("killsSpiderFlyer", null, false, "PlayerData_Int");
                public static readonly FlagDef killsSpitter = new FlagDef("killsSpitter", null, false, "PlayerData_Int");
                public static readonly FlagDef killsSpittingZombie = new FlagDef("killsSpittingZombie", null, false, "PlayerData_Int");
                public static readonly FlagDef killsSuperSpitter = new FlagDef("killsSuperSpitter", null, false, "PlayerData_Int");
                public static readonly FlagDef killsTraitorLord = new FlagDef("killsTraitorLord", null, false, "PlayerData_Int");
                public static readonly FlagDef killsVoidIdol_1 = new FlagDef("killsVoidIdol_1", null, false, "PlayerData_Int");
                public static readonly FlagDef killsVoidIdol_2 = new FlagDef("killsVoidIdol_2", null, false, "PlayerData_Int");
                public static readonly FlagDef killsVoidIdol_3 = new FlagDef("killsVoidIdol_3", null, false, "PlayerData_Int");
                public static readonly FlagDef killsWhiteDefender = new FlagDef("killsWhiteDefender", null, false, "PlayerData_Int");
                public static readonly FlagDef killsWhiteRoyal = new FlagDef("killsWhiteRoyal", null, false, "PlayerData_Int");
                public static readonly FlagDef killsWorm = new FlagDef("killsWorm", null, false, "PlayerData_Int");
                public static readonly FlagDef killsZapBug = new FlagDef("killsZapBug", null, false, "PlayerData_Int");
                public static readonly FlagDef killsZombieBarger = new FlagDef("killsZombieBarger", null, false, "PlayerData_Int");
                public static readonly FlagDef killsZombieGuard = new FlagDef("killsZombieGuard", null, false, "PlayerData_Int");
                public static readonly FlagDef killsZombieHive = new FlagDef("killsZombieHive", null, false, "PlayerData_Int");
                public static readonly FlagDef killsZombieHornhead = new FlagDef("killsZombieHornhead", null, false, "PlayerData_Int");
                public static readonly FlagDef killsZombieLeaper = new FlagDef("killsZombieLeaper", null, false, "PlayerData_Int");
                public static readonly FlagDef killsZombieMiner = new FlagDef("killsZombieMiner", null, false, "PlayerData_Int");
                public static readonly FlagDef killsZombieRunner = new FlagDef("killsZombieRunner", null, false, "PlayerData_Int");
                public static readonly FlagDef killsZombieShield = new FlagDef("killsZombieShield", null, false, "PlayerData_Int");
                public static readonly FlagDef killsZote = new FlagDef("killsZote", null, false, "PlayerData_Int");
                public static readonly FlagDef killsZotelingBalloon = new FlagDef("killsZotelingBalloon", null, false, "PlayerData_Int");
                public static readonly FlagDef killsZotelingBuzzer = new FlagDef("killsZotelingBuzzer", null, false, "PlayerData_Int");
                public static readonly FlagDef killsZotelingHopper = new FlagDef("killsZotelingHopper", null, false, "PlayerData_Int");
            #endregion

            #region Kills Enemy New Data
                public static readonly FlagDef newDataAbyssCrawler = new FlagDef("newDataAbyssCrawler", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataAbyssTendril = new FlagDef("newDataAbyssTendril", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataAcidFlyer = new FlagDef("newDataAcidFlyer", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataAcidWalker = new FlagDef("newDataAcidWalker", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataAngryBuzzer = new FlagDef("newDataAngryBuzzer", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataBabyCentipede = new FlagDef("newDataBabyCentipede", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataBeamMiner = new FlagDef("newDataBeamMiner", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataBeeHatchling = new FlagDef("newDataBeeHatchling", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataBeeStinger = new FlagDef("newDataBeeStinger", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataBigBee = new FlagDef("newDataBigBee", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataBigBuzzer = new FlagDef("newDataBigBuzzer", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataBigCentipede = new FlagDef("newDataBigCentipede", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataBigFly = new FlagDef("newDataBigFly", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataBindingSeal = new FlagDef("newDataBindingSeal", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataBlackKnight = new FlagDef("newDataBlackKnight", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataBlobFlyer = new FlagDef("newDataBlobFlyer", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataBlobble = new FlagDef("newDataBlobble", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataBlocker = new FlagDef("newDataBlocker", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataBlowFly = new FlagDef("newDataBlowFly", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataBouncer = new FlagDef("newDataBouncer", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataBurstingBouncer = new FlagDef("newDataBurstingBouncer", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataBurstingZombie = new FlagDef("newDataBurstingZombie", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataBuzzer = new FlagDef("newDataBuzzer", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataCeilingDropper = new FlagDef("newDataCeilingDropper", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataCentipedeHatcher = new FlagDef("newDataCentipedeHatcher", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataClimber = new FlagDef("newDataClimber", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataColFlyingSentry = new FlagDef("newDataColFlyingSentry", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataColHopper = new FlagDef("newDataColHopper", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataColMiner = new FlagDef("newDataColMiner", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataColMosquito = new FlagDef("newDataColMosquito", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataColRoller = new FlagDef("newDataColRoller", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataColShield = new FlagDef("newDataColShield", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataColWorm = new FlagDef("newDataColWorm", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataCrawler = new FlagDef("newDataCrawler", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataCrystalCrawler = new FlagDef("newDataCrystalCrawler", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataCrystalFlyer = new FlagDef("newDataCrystalFlyer", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataDreamGuard = new FlagDef("newDataDreamGuard", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataDummy = new FlagDef("newDataDummy", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataDungDefender = new FlagDef("newDataDungDefender", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataEggSac = new FlagDef("newDataEggSac", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataElectricMage = new FlagDef("newDataElectricMage", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataFalseKnight = new FlagDef("newDataFalseKnight", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataFatFluke = new FlagDef("newDataFatFluke", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataFinalBoss = new FlagDef("newDataFinalBoss", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataFlameBearerLarge = new FlagDef("newDataFlameBearerLarge", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataFlameBearerMed = new FlagDef("newDataFlameBearerMed", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataFlameBearerSmall = new FlagDef("newDataFlameBearerSmall", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataFlipHopper = new FlagDef("newDataFlipHopper", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataFlukeMother = new FlagDef("newDataFlukeMother", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataFlukefly = new FlagDef("newDataFlukefly", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataFlukeman = new FlagDef("newDataFlukeman", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataFlyingSentryJavelin = new FlagDef("newDataFlyingSentryJavelin", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataFlyingSentrySword = new FlagDef("newDataFlyingSentrySword", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataFungCrawler = new FlagDef("newDataFungCrawler", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataFungifiedZombie = new FlagDef("newDataFungifiedZombie", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataFungoonBaby = new FlagDef("newDataFungoonBaby", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataFungusFlyer = new FlagDef("newDataFungusFlyer", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataGardenZombie = new FlagDef("newDataGardenZombie", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataGhostAladar = new FlagDef("newDataGhostAladar", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataGhostGalien = new FlagDef("newDataGhostGalien", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataGhostHu = new FlagDef("newDataGhostHu", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataGhostMarkoth = new FlagDef("newDataGhostMarkoth", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataGhostMarmu = new FlagDef("newDataGhostMarmu", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataGhostNoEyes = new FlagDef("newDataGhostNoEyes", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataGhostXero = new FlagDef("newDataGhostXero", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataGiantHopper = new FlagDef("newDataGiantHopper", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataGodseekerMask = new FlagDef("newDataGodseekerMask", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataGorgeousHusk = new FlagDef("newDataGorgeousHusk", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataGrassHopper = new FlagDef("newDataGrassHopper", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataGreatShieldZombie = new FlagDef("newDataGreatShieldZombie", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataGreyPrince = new FlagDef("newDataGreyPrince", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataGrimm = new FlagDef("newDataGrimm", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataGrubMimic = new FlagDef("newDataGrubMimic", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataHatcher = new FlagDef("newDataHatcher", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataHatchling = new FlagDef("newDataHatchling", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataHealthScuttler = new FlagDef("newDataHealthScuttler", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataHeavyMantis = new FlagDef("newDataHeavyMantis", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataHiveKnight = new FlagDef("newDataHiveKnight", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataHollowKnight = new FlagDef("newDataHollowKnight", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataHollowKnightPrime = new FlagDef("newDataHollowKnightPrime", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataHopper = new FlagDef("newDataHopper", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataHornet = new FlagDef("newDataHornet", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataHunterMark = new FlagDef("newDataHunterMark", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataInfectedKnight = new FlagDef("newDataInfectedKnight", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataInflater = new FlagDef("newDataInflater", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataJarCollector = new FlagDef("newDataJarCollector", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataJellyCrawler = new FlagDef("newDataJellyCrawler", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataJellyfish = new FlagDef("newDataJellyfish", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataLaserBug = new FlagDef("newDataLaserBug", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataLazyFlyer = new FlagDef("newDataLazyFlyer", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataLesserMawlek = new FlagDef("newDataLesserMawlek", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataLobsterLancer = new FlagDef("newDataLobsterLancer", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMage = new FlagDef("newDataMage", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMageBalloon = new FlagDef("newDataMageBalloon", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMageBlob = new FlagDef("newDataMageBlob", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMageKnight = new FlagDef("newDataMageKnight", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMageLord = new FlagDef("newDataMageLord", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMantis = new FlagDef("newDataMantis", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMantisFlyerChild = new FlagDef("newDataMantisFlyerChild", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMantisHeavyFlyer = new FlagDef("newDataMantisHeavyFlyer", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMantisLord = new FlagDef("newDataMantisLord", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMawlek = new FlagDef("newDataMawlek", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMawlekTurret = new FlagDef("newDataMawlekTurret", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMegaBeamMiner = new FlagDef("newDataMegaBeamMiner", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMegaJellyfish = new FlagDef("newDataMegaJellyfish", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMegaMossCharger = new FlagDef("newDataMegaMossCharger", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMenderBug = new FlagDef("newDataMenderBug", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMimicSpider = new FlagDef("newDataMimicSpider", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMinesCrawler = new FlagDef("newDataMinesCrawler", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMiniSpider = new FlagDef("newDataMiniSpider", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMosquito = new FlagDef("newDataMosquito", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMossCharger = new FlagDef("newDataMossCharger", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMossFlyer = new FlagDef("newDataMossFlyer", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMossKnight = new FlagDef("newDataMossKnight", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMossKnightFat = new FlagDef("newDataMossKnightFat", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMossWalker = new FlagDef("newDataMossWalker", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMossmanRunner = new FlagDef("newDataMossmanRunner", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMossmanShaker = new FlagDef("newDataMossmanShaker", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMummy = new FlagDef("newDataMummy", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMushroomBaby = new FlagDef("newDataMushroomBaby", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMushroomBrawler = new FlagDef("newDataMushroomBrawler", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMushroomRoller = new FlagDef("newDataMushroomRoller", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataMushroomTurret = new FlagDef("newDataMushroomTurret", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataNailBros = new FlagDef("newDataNailBros", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataNailsage = new FlagDef("newDataNailsage", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataNightmareGrimm = new FlagDef("newDataNightmareGrimm", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataOblobble = new FlagDef("newDataOblobble", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataOrangeBalloon = new FlagDef("newDataOrangeBalloon", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataOrangeScuttler = new FlagDef("newDataOrangeScuttler", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataPaintmaster = new FlagDef("newDataPaintmaster", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataPalaceFly = new FlagDef("newDataPalaceFly", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataPaleLurker = new FlagDef("newDataPaleLurker", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataPigeon = new FlagDef("newDataPigeon", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataPlantShooter = new FlagDef("newDataPlantShooter", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataPrayerSlug = new FlagDef("newDataPrayerSlug", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataRoller = new FlagDef("newDataRoller", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataRoyalCoward = new FlagDef("newDataRoyalCoward", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataRoyalDandy = new FlagDef("newDataRoyalDandy", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataRoyalGuard = new FlagDef("newDataRoyalGuard", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataRoyalPlumper = new FlagDef("newDataRoyalPlumper", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataSentry = new FlagDef("newDataSentry", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataSentryFat = new FlagDef("newDataSentryFat", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataShootSpider = new FlagDef("newDataShootSpider", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataSibling = new FlagDef("newDataSibling", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataSlashSpider = new FlagDef("newDataSlashSpider", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataSnapperTrap = new FlagDef("newDataSnapperTrap", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataSpiderCorpse = new FlagDef("newDataSpiderCorpse", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataSpiderFlyer = new FlagDef("newDataSpiderFlyer", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataSpitter = new FlagDef("newDataSpitter", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataSpittingZombie = new FlagDef("newDataSpittingZombie", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataSuperSpitter = new FlagDef("newDataSuperSpitter", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataTraitorLord = new FlagDef("newDataTraitorLord", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataVoidIdol_1 = new FlagDef("newDataVoidIdol_1", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataVoidIdol_2 = new FlagDef("newDataVoidIdol_2", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataVoidIdol_3 = new FlagDef("newDataVoidIdol_3", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataWhiteDefender = new FlagDef("newDataWhiteDefender", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataWhiteRoyal = new FlagDef("newDataWhiteRoyal", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataWorm = new FlagDef("newDataWorm", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataZapBug = new FlagDef("newDataZapBug", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataZombieBarger = new FlagDef("newDataZombieBarger", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataZombieGuard = new FlagDef("newDataZombieGuard", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataZombieHive = new FlagDef("newDataZombieHive", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataZombieHornhead = new FlagDef("newDataZombieHornhead", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataZombieLeaper = new FlagDef("newDataZombieLeaper", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataZombieMiner = new FlagDef("newDataZombieMiner", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataZombieRunner = new FlagDef("newDataZombieRunner", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataZombieShield = new FlagDef("newDataZombieShield", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataZote = new FlagDef("newDataZote", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataZotelingBalloon = new FlagDef("newDataZotelingBalloon", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataZotelingBuzzer = new FlagDef("newDataZotelingBuzzer", null, false, "PlayerData_Bool");
                public static readonly FlagDef newDataZotelingHopper = new FlagDef("newDataZotelingHopper", null, false, "PlayerData_Bool");
            #endregion

        #endregion

        #region Inventory
            // Item State
            public static readonly FlagDef heartPieceCollected = new FlagDef("heartPieceCollected", null, false, "PlayerData_Bool", "Collected first Mask Shard");
            public static readonly FlagDef spareMarkers_b = new FlagDef("spareMarkers_b", null, false, "PlayerData_Int", "Used Blue Markers");
            public static readonly FlagDef spareMarkers_r = new FlagDef("spareMarkers_r", null, false, "PlayerData_Int", "Used Red Markers");
            public static readonly FlagDef spareMarkers_w = new FlagDef("spareMarkers_w", null, false, "PlayerData_Int", "Used White Markers");
            public static readonly FlagDef spareMarkers_y = new FlagDef("spareMarkers_y", null, false, "PlayerData_Int", "Used Yellow Markers");
            public static readonly FlagDef vesselFragmentCollected = new FlagDef("vesselFragmentCollected", null, false, "PlayerData_Bool", "Completed a new Vessel");
            public static readonly FlagDef vesselFragments = new FlagDef("vesselFragments", null, false, "PlayerData_Int", "Vessel Fragments Completed (0-2)");
            public static readonly FlagDef xunFlowerBroken = new FlagDef("xunFlowerBroken", null, false, "PlayerData_Bool", "Ruined Flower");

            // Upgrade Parts
            public static readonly FlagDef heartPieces = new FlagDef("heartPieces", null, false, "PlayerData_Int", "Mask Shards (0-4)");
            public static readonly FlagDef ore = new FlagDef("ore", null, false, "PlayerData_Int", "Pale Ore (0-6)");

            // Consumeables
            public static readonly FlagDef rancidEggs = new FlagDef("rancidEggs", null, false, "PlayerData_Int", "Rancid Eggs (0-80)");
            public static readonly FlagDef simpleKeys = new FlagDef("simpleKeys", null, false, "PlayerData_Int", "Simple Keys (0-4)");

            #region Items
                public static readonly FlagDef hasCharm =           new FlagDef("hasCharm",         null, false, "PlayerData_Bool", "Has a Charm");
                public static readonly FlagDef hasCityKey =         new FlagDef("hasCityKey",       null, false, "PlayerData_Bool", "City Crest");
                public static readonly FlagDef hasGodfinder =       new FlagDef("hasGodfinder",     null, false, "PlayerData_Bool", "Godtuner");
                public static readonly FlagDef hasHuntersMark =     new FlagDef("hasHuntersMark",   null, false, "PlayerData_Bool", "Hunter's Mark");
                public static readonly FlagDef hasJournal =         new FlagDef("hasJournal",       null, false, "PlayerData_Bool", "Hunter's Journal");
                public static readonly FlagDef hasKingsBrand =      new FlagDef("hasKingsBrand",    null, false, "PlayerData_Bool", "King's Brand");
                public static readonly FlagDef hasLantern =         new FlagDef("hasLantern",       null, false, "PlayerData_Bool", "Lumafly Lantern");
                public static readonly FlagDef hasLoveKey =         new FlagDef("hasLoveKey",       null, false, "PlayerData_Bool", "Love Key");
                public static readonly FlagDef hasMap =             new FlagDef("hasMap",           null, false, "PlayerData_Bool", "Has any Map");
                public static readonly FlagDef hasMarker =          new FlagDef("hasMarker",        null, false, "PlayerData_Bool", "Has a marker");
                public static readonly FlagDef hasMarker_b =        new FlagDef("hasMarker_b",      null, false, "PlayerData_Bool", "Scarab Marker");
                public static readonly FlagDef hasMarker_r =        new FlagDef("hasMarker_r",      null, false, "PlayerData_Bool", "Shell Marker");
                public static readonly FlagDef hasMarker_w =        new FlagDef("hasMarker_w",      null, false, "PlayerData_Bool", "Gleaming Marker");
                public static readonly FlagDef hasMarker_y =        new FlagDef("hasMarker_y",      null, false, "PlayerData_Bool", "Token Marker");
                public static readonly FlagDef hasMenderKey =       new FlagDef("hasMenderKey",     null, false, "PlayerData_Bool");
                public static readonly FlagDef hasPin =             new FlagDef("hasPin",           null, false, "PlayerData_Bool", "Has any pin");
                public static readonly FlagDef hasPinBench =        new FlagDef("hasPinBench",      null, false, "PlayerData_Bool", "Bench Pin");
                public static readonly FlagDef hasPinBlackEgg =     new FlagDef("hasPinBlackEgg",   null, false, "PlayerData_Bool", "Black Egg Temple Pin");
                public static readonly FlagDef hasPinCocoon =       new FlagDef("hasPinCocoon",     null, false, "PlayerData_Bool", "Cocoon Pin");
                public static readonly FlagDef hasPinDreamPlant =   new FlagDef("hasPinDreamPlant", null, false, "PlayerData_Bool", "Whispering Root Pin");
                public static readonly FlagDef hasPinGhost =        new FlagDef("hasPinGhost",      null, false, "PlayerData_Bool", "Warrior's Grave Pin");
                public static readonly FlagDef hasPinGrub =         new FlagDef("hasPinGrub",       null, false, "PlayerData_Bool", "Collector's Map");
                public static readonly FlagDef hasPinGuardian =     new FlagDef("hasPinGuardian",   null, false, "PlayerData_Bool", "Dreamers Pin");
                public static readonly FlagDef hasPinShop =         new FlagDef("hasPinShop",       null, false, "PlayerData_Bool", "Vendor Pin");
                public static readonly FlagDef hasPinSpa =          new FlagDef("hasPinSpa",        null, false, "PlayerData_Bool", "Hot Spring Pin");
                public static readonly FlagDef hasPinStag =         new FlagDef("hasPinStag",       null, false, "PlayerData_Bool", "Stag Station Pin");
                public static readonly FlagDef hasPinTram =         new FlagDef("hasPinTram",       null, false, "PlayerData_Bool", "Tram Pin");
                public static readonly FlagDef hasQuill =           new FlagDef("hasQuill",         null, false, "PlayerData_Bool", "Quill");
                public static readonly FlagDef hasSlykey =          new FlagDef("hasSlykey",        null, false, "PlayerData_Bool", "Shopkeeper's Key");
                public static readonly FlagDef hasSpaKey =          new FlagDef("hasSpaKey",        null, false, "PlayerData_Bool");
                public static readonly FlagDef hasTramPass =        new FlagDef("hasTramPass",      null, false, "PlayerData_Bool", "Tram Pass");
                public static readonly FlagDef hasWaterwaysKey =    new FlagDef("hasWaterwaysKey",  null, false, "PlayerData_Bool");
                public static readonly FlagDef hasWhiteKey =        new FlagDef("hasWhiteKey",      null, false, "PlayerData_Bool", "Elegant Key");
                public static readonly FlagDef hasXunFlower =       new FlagDef("hasXunFlower",     null, false, "PlayerData_Bool", "Delicate Flower");
            #endregion

            #region Abilities
                public static readonly FlagDef dreamNailUpgraded =  new FlagDef("dreamNailUpgraded",    null, false, "PlayerData_Bool", "Awoken Dream Nail");
                public static readonly FlagDef fireballLevel =      new FlagDef("fireballLevel",        null, false, "PlayerData_Int",  "Vengeful Spirit / Shade Soul");
                public static readonly FlagDef hasAcidArmour =      new FlagDef("hasAcidArmour",        null, false, "PlayerData_Bool", "Isma's Tear");
                public static readonly FlagDef hasAllNailArts =     new FlagDef("hasAllNailArts",       null, false, "PlayerData_Bool", "Has all Nail Arts");
                public static readonly FlagDef hasCyclone =         new FlagDef("hasCyclone",           null, false, "PlayerData_Bool", "Cyclone Slash");
                public static readonly FlagDef hasDash =            new FlagDef("hasDash",              null, false, "PlayerData_Bool", "Mothwing Cloak");
                public static readonly FlagDef hasDoubleJump =      new FlagDef("hasDoubleJump",        null, false, "PlayerData_Bool", "Monarch Wings");
                public static readonly FlagDef hasDashSlash =       new FlagDef("hasDashSlash",         null, false, "PlayerData_Bool", "Great Slash");
                public static readonly FlagDef hasDreamGate =       new FlagDef("hasDreamGate",         null, false, "PlayerData_Bool", "Dream Gate");
                public static readonly FlagDef hasDreamNail =       new FlagDef("hasDreamNail",         null, false, "PlayerData_Bool", "Dream Nail");
                public static readonly FlagDef hasNailArt =         new FlagDef("hasNailArt",           null, false, "PlayerData_Bool", "Has a Nail Art");
                public static readonly FlagDef hasShadowDash =      new FlagDef("hasShadowDash",        null, false, "PlayerData_Bool", "Shade Cloak");
                public static readonly FlagDef hasSpell =           new FlagDef("hasSpell",             null, false, "PlayerData_Bool", "Got first spell from Shaman");
                public static readonly FlagDef hasSuperDash =       new FlagDef("hasSuperDash",         null, false, "PlayerData_Bool", "Crystal Heart");
                public static readonly FlagDef hasUpwardSlash =     new FlagDef("hasUpwardSlash",       null, false, "PlayerData_Bool", "Dash Slash");
                public static readonly FlagDef hasWalljump =        new FlagDef("hasWalljump",          null, false, "PlayerData_Bool", "Mantis Claw");
                public static readonly FlagDef quakeLevel =         new FlagDef("quakeLevel",           null, false, "PlayerData_Int",  "Desolate Dive / Descending Dark");
                public static readonly FlagDef salubraBlessing =    new FlagDef("salubraBlessing",      null, false, "PlayerData_Bool", "Salubra's Blessing");
                public static readonly FlagDef screamLevel =        new FlagDef("screamLevel",          null, false, "PlayerData_Int",  "Howling Wraiths / Abyss Shriek");
            #endregion

            #region Maps
                public static readonly FlagDef mapAbyss = new FlagDef("mapAbyss", null, false, "PlayerData_Bool");
                public static readonly FlagDef mapAllRooms = new FlagDef("mapAllRooms", null, false, "PlayerData_Bool");
                public static readonly FlagDef mapCity = new FlagDef("mapCity", null, false, "PlayerData_Bool");
                public static readonly FlagDef mapCliffs = new FlagDef("mapCliffs", null, false, "PlayerData_Bool");
                public static readonly FlagDef mapCrossroads = new FlagDef("mapCrossroads", null, false, "PlayerData_Bool");
                public static readonly FlagDef mapDeepnest = new FlagDef("mapDeepnest", null, false, "PlayerData_Bool");
                public static readonly FlagDef mapDirtmouth = new FlagDef("mapDirtmouth", null, false, "PlayerData_Bool");
                public static readonly FlagDef mapFogCanyon = new FlagDef("mapFogCanyon", null, false, "PlayerData_Bool");
                public static readonly FlagDef mapFungalWastes = new FlagDef("mapFungalWastes", null, false, "PlayerData_Bool");
                public static readonly FlagDef mapGreenpath = new FlagDef("mapGreenpath", null, false, "PlayerData_Bool");
                public static readonly FlagDef mapKeyPref = new FlagDef("mapKeyPref", null, false, "PlayerData_Int");
                public static readonly FlagDef mapMines = new FlagDef("mapMines", null, false, "PlayerData_Bool");
                public static readonly FlagDef mapOutskirts = new FlagDef("mapOutskirts", null, false, "PlayerData_Bool");
                public static readonly FlagDef mapRestingGrounds = new FlagDef("mapRestingGrounds", null, false, "PlayerData_Bool");
                public static readonly FlagDef mapRoyalGardens = new FlagDef("mapRoyalGardens", null, false, "PlayerData_Bool");
                public static readonly FlagDef mapWaterways = new FlagDef("mapWaterways", null, false, "PlayerData_Bool");
            #endregion

            #region Mask Shards (heart pieces)
                public static readonly FlagDef Crossroads_09__Heart_Piece = new FlagDef("Heart Piece", SceneInstances.Crossroads_09, false, "PersistentBoolData", "Brooding Mawlek");
                public static readonly FlagDef Crossroads_13__Heart_Piece = new FlagDef("Heart Piece", SceneInstances.Crossroads_13, false, "PersistentBoolData", SceneInstances.Crossroads_13.ReadableName);
                public static readonly FlagDef Fungus1_36__Heart_Piece = new FlagDef("Heart Piece", SceneInstances.Fungus1_36, false, "PersistentBoolData", SceneInstances.Fungus1_36.ReadableName);
                public static readonly FlagDef Fungus2_01__Heart_Piece = new FlagDef("Heart Piece", SceneInstances.Fungus2_01, false, "PersistentBoolData", SceneInstances.Fungus2_01.ReadableName);
                public static readonly FlagDef slyShellFrag1 = new FlagDef("slyShellFrag1", null, false, "PlayerData_Bool", "Bought Mask Shard 1 from Sly");
                public static readonly FlagDef slyShellFrag2 = new FlagDef("slyShellFrag2", null, false, "PlayerData_Bool", "Bought Mask Shard 2 from Sly");
                public static readonly FlagDef slyShellFrag3 = new FlagDef("slyShellFrag3", null, false, "PlayerData_Bool", "Bought Mask Shard 3 from Sly");
                public static readonly FlagDef slyShellFrag4 = new FlagDef("slyShellFrag4", null, false, "PlayerData_Bool", "Bought Mask Shard 4 from Sly");
            #endregion

            #region Vessel Fragments
                public static readonly FlagDef slyVesselFrag1 = new FlagDef("slyVesselFrag1", null, false, "PlayerData_Bool", "Bought Vessel Fragment 1 from Sly");
                public static readonly FlagDef slyVesselFrag2 = new FlagDef("slyVesselFrag2", null, false, "PlayerData_Bool", "Bought Vessel Fragment 2 from Sly");
                public static readonly FlagDef slyVesselFrag3 = new FlagDef("slyVesselFrag3", null, false, "PlayerData_Bool", "Bought Vessel Fragment 3 from Sly");
                public static readonly FlagDef slyVesselFrag4 = new FlagDef("slyVesselFrag4", null, false, "PlayerData_Bool", "Bought Vessel Fragment 4 from Sly");
                public static readonly FlagDef Fungus1_13__Vessel_Fragment = new FlagDef("Vessel Fragment", SceneInstances.Fungus1_13, false, "PersistentBoolData", SceneInstances.Fungus1_13.ReadableName);
                public static readonly FlagDef Deepnest_38__Vessel_Fragment = new FlagDef("Vessel Fragment", SceneInstances.Deepnest_38, false, "PersistentBoolData", SceneInstances.Deepnest_38.ReadableName);
                public static readonly FlagDef Ruins2_09__Vessel_Fragment = new FlagDef("Vessel Fragment", SceneInstances.Ruins2_09, false, "PersistentBoolData", SceneInstances.Ruins2_09.ReadableName);
            #endregion

            #region Nail
                public static readonly FlagDef honedNail = new FlagDef("honedNail", null, false, "PlayerData_Bool", "Upgraded Nail");
                public static readonly FlagDef nailSmithUpgrades =  new FlagDef("nailSmithUpgrades",    null, false, "PlayerData_Int",  "Nail Upgrades (0-4)");
            #endregion

            #region Notches
                public static readonly FlagDef salubraNotch1 = new FlagDef("salubraNotch1", null, false, "PlayerData_Bool", "Salubra Notch 1");
                public static readonly FlagDef salubraNotch2 = new FlagDef("salubraNotch2", null, false, "PlayerData_Bool", "Salubra Notch 2");
                public static readonly FlagDef salubraNotch3 = new FlagDef("salubraNotch3", null, false, "PlayerData_Bool", "Salubra Notch 3");
                public static readonly FlagDef salubraNotch4 = new FlagDef("salubraNotch4", null, false, "PlayerData_Bool", "Salubra Notch 4");
                public static readonly FlagDef notchShroomOgres = new FlagDef("notchShroomOgres", null, false, "PlayerData_Bool", "Shrumal Ogre battle notch");
            #endregion

            #region Simple Keys
                public static readonly FlagDef slySimpleKey = new FlagDef("slySimpleKey", null, false, "PlayerData_Bool", "Got Sly's Simple Key");
            #endregion

            #region Pale Ore
                public static readonly FlagDef Abyss_17__Shiny_Item_Stand = new FlagDef("Shiny Item Stand", SceneInstances.Abyss_17, false, "PersistentBoolData", "Pale Ore " + SceneInstances.Abyss_17.ReadableName);
            #endregion

            #region Trinkets
                public static readonly FlagDef foundTrinket1 =      new FlagDef("foundTrinket1",    null, false, "PlayerData_Bool");
                public static readonly FlagDef foundTrinket2 =      new FlagDef("foundTrinket2",    null, false, "PlayerData_Bool");
                public static readonly FlagDef foundTrinket3 =      new FlagDef("foundTrinket3",    null, false, "PlayerData_Bool");
                public static readonly FlagDef foundTrinket4 =      new FlagDef("foundTrinket4",    null, false, "PlayerData_Bool");
                public static readonly FlagDef noTrinket1 =         new FlagDef("noTrinket1",       null, false, "PlayerData_Bool");
                public static readonly FlagDef noTrinket2 =         new FlagDef("noTrinket2",       null, false, "PlayerData_Bool");
                public static readonly FlagDef noTrinket3 =         new FlagDef("noTrinket3",       null, false, "PlayerData_Bool");
                public static readonly FlagDef noTrinket4 =         new FlagDef("noTrinket4",       null, false, "PlayerData_Bool");
                public static readonly FlagDef soldTrinket1 =       new FlagDef("soldTrinket1",     null, false, "PlayerData_Int");
                public static readonly FlagDef soldTrinket2 =       new FlagDef("soldTrinket2",     null, false, "PlayerData_Int");
                public static readonly FlagDef soldTrinket3 =       new FlagDef("soldTrinket3",     null, false, "PlayerData_Int");
                public static readonly FlagDef soldTrinket4 =       new FlagDef("soldTrinket4",     null, false, "PlayerData_Int");
                public static readonly FlagDef trinket1 =           new FlagDef("trinket1",         null, false, "PlayerData_Int",  "Wanderer's Journals (0-14)");
                public static readonly FlagDef trinket2 =           new FlagDef("trinket2",         null, false, "PlayerData_Int",  "Hallownest Seals (0-17)");
                public static readonly FlagDef trinket3 =           new FlagDef("trinket3",         null, false, "PlayerData_Int",  "King's Idols (0-8)");
                public static readonly FlagDef trinket4 =           new FlagDef("trinket4",         null, false, "PlayerData_Int",  "Arcane Eggs (0-4)");

                #region Trinket 1
                    public static readonly FlagDef Fungus1_22__Shiny_Item = new FlagDef("Shiny Item", SceneInstances.Fungus1_22, false, "PersistentBoolData");
                    public static readonly FlagDef Fungus2_17__Shiny_Item = new FlagDef("Shiny Item", SceneInstances.Fungus2_17, false, "PersistentBoolData");
                    public static readonly FlagDef Cliffs_01__Shiny_Item_1 = new FlagDef("Shiny Item (1)", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                    public static readonly FlagDef Fungus2_04__Shiny_Item = new FlagDef("Shiny Item", SceneInstances.Fungus2_04, false, "PersistentBoolData");
                    public static readonly FlagDef Abyss_02__Shiny_Item = new FlagDef("Shiny Item", SceneInstances.Abyss_02, false, "PersistentBoolData");
                    public static readonly FlagDef Ruins_Elevator__Shiny_Item_1 = new FlagDef("Shiny Item (1)", SceneInstances.Ruins_Elevator, false, "PersistentBoolData");
                    public static readonly FlagDef Ruins2_05__Shiny_Item = new FlagDef("Shiny Item", SceneInstances.Ruins2_05, false, "PersistentBoolData");
                #endregion

                #region Trinket 2
                    public static readonly FlagDef Fungus2_03__Shiny_Item = new FlagDef("Shiny Item", SceneInstances.Fungus2_03, false, "PersistentBoolData");
                    public static readonly FlagDef Fungus3_30__Shiny_Item = new FlagDef("Shiny Item", SceneInstances.Fungus3_30, false, "PersistentBoolData");
                    public static readonly FlagDef Deepnest_Spider_Town__Shiny_Item = new FlagDef("Shiny Item", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData");
                    public static readonly FlagDef Fungus2_31__Shiny_Item = new FlagDef("Shiny Item", SceneInstances.Fungus2_31, false, "PersistentBoolData");
                    public static readonly FlagDef Ruins2_08__Shiny_Item = new FlagDef("Shiny Item", SceneInstances.Ruins2_08, false, "PersistentBoolData");
                    public static readonly FlagDef Ruins1_03__Shiny_Item = new FlagDef("Shiny Item", SceneInstances.Ruins1_03, false, "PersistentBoolData");
                #endregion

                #region Trinket 3
                    public static readonly FlagDef Cliffs_01__Shiny_Item = new FlagDef("Shiny Item", SceneInstances.Cliffs_01, false, "PersistentBoolData");
                    public static readonly FlagDef Deepnest_33__Shiny_Item = new FlagDef("Shiny Item", SceneInstances.Deepnest_33, false, "PersistentBoolData");
                #endregion

                #region Trinket 4

                #endregion

            #endregion

        #endregion

        #region NPC Status
            public static readonly FlagDef ALADAR_encountered = new FlagDef("ALADAR_encountered", null, false, "PlayerData_Bool", "Met Gorb");
            // 0 = not rescued, 1 = ?
            public static readonly FlagDef brettaPosition = new FlagDef("brettaPosition", null, false, "PlayerData_Int", "Bretta's current location (0-?)");
            public static readonly FlagDef brettaRescued = new FlagDef("brettaRescued", null, false, "PlayerData_Bool", "Rescued Bretta");
            public static readonly FlagDef corn_abyssEncountered = new FlagDef("corn_abyssEncountered", null, false, "PlayerData_Bool", "Met Cornifer at Abyss");
            public static readonly FlagDef corn_cliffsEncountered = new FlagDef("corn_cliffsEncountered", null, false, "PlayerData_Bool", "Met Cornifer at Cliffs");
            public static readonly FlagDef corn_cliffsLeft = new FlagDef("corn_cliffsLeft", null, false, "PlayerData_Bool", "Cornifer left Cliffs");
            public static readonly FlagDef corn_crossroadsLeft = new FlagDef("corn_crossroadsLeft", null, false, "PlayerData_Bool", "Cornifer left Crossroads, after defeating False Knight");
            public static readonly FlagDef corn_deepnestEncountered = new FlagDef("corn_deepnestEncountered", null, false, "PlayerData_Bool", "Encountered Cornifer at Deepnest");
            public static readonly FlagDef corn_deepnestLeft = new FlagDef("corn_deepnestLeft", null, false, "PlayerData_Bool", "Cornifer left Deepnest");
            public static readonly FlagDef corn_deepnestMet1 = new FlagDef("corn_deepnestMet1", null, false, "PlayerData_Bool", "Talked with Cornifer at Deepnest");
            public static readonly FlagDef corn_fungalWastesEncountered = new FlagDef("corn_fungalWastesEncountered", null, false, "PlayerData_Bool", "Met Cornifer at Fungal Wastes");
            public static readonly FlagDef corn_fungalWastesLeft = new FlagDef("corn_fungalWastesLeft", null, false, "PlayerData_Bool", "Cornifer left Fungal Wastes, after getting Mantis Claw");
            public static readonly FlagDef corn_greenpathEncountered = new FlagDef("corn_greenpathEncountered", null, false, "PlayerData_Bool", "Met Cornifer at Greenpath");
            public static readonly FlagDef corn_greenpathLeft = new FlagDef("corn_greenpathLeft", null, false, "PlayerData_Bool", "Cornifer left Greenpath, after defeating Hornet");
            public static readonly FlagDef corniferIntroduced = new FlagDef("corniferIntroduced", null, false, "PlayerData_Bool", "Cornifer Introduced");
            public static readonly FlagDef Crossroads_45__Zombie_Myla = new FlagDef("Zombie Myla", SceneInstances.Crossroads_45, false, "PersistentBoolData", "Myla Zombied");
            public static readonly FlagDef divineInTown = new FlagDef("divineInTown", null, false, "PlayerData_Bool", "Divine in Dirtmouth");
            public static readonly FlagDef elderbugConvoGrimm = new FlagDef("elderbugConvoGrimm", null, false, "PlayerData_Bool", "Elderbug Grimm Conversation");
            public static readonly FlagDef elderbugHistory = new FlagDef("elderbugHistory", null, false, "PlayerData_Int", "Elderbug History Conversation (0-1?)");
            public static readonly FlagDef elderbugHistory1 = new FlagDef("elderbugHistory1", null, false, "PlayerData_Bool");
            public static readonly FlagDef elderbugHistory2 = new FlagDef("elderbugHistory2", null, false, "PlayerData_Bool");
            public static readonly FlagDef elderbugHistory3 = new FlagDef("elderbugHistory3", null, false, "PlayerData_Bool");
            public static readonly FlagDef elderbugSpeechEggTemple = new FlagDef("elderbugSpeechEggTemple", null, false, "PlayerData_Bool", "Elderbug speech Egg Temple");
            public static readonly FlagDef elderbugSpeechKingsPass = new FlagDef("elderbugSpeechKingsPass", null, false, "PlayerData_Bool", "Elderbug speech King's Pass");
            public static readonly FlagDef elderbugSpeechMapShop = new FlagDef("elderbugSpeechMapShop", null, false, "PlayerData_Bool", "Elderbug speech Map Shop");
            public static readonly FlagDef elderbugSpeechSly = new FlagDef("elderbugSpeechSly", null, false, "PlayerData_Bool", "Elderbug speech Sly");
            public static readonly FlagDef elderbugSpeechStation = new FlagDef("elderbugSpeechStation", null, false, "PlayerData_Bool", "Elderbug speech Stag Stations");
            public static readonly FlagDef GALIEN_encountered = new FlagDef("GALIEN_encountered", null, false, "PlayerData_Bool", "Met Galien");
            public static readonly FlagDef hornetCityBridge_completed = new FlagDef("hornetCityBridge_completed", null, false, "PlayerData_Bool", "Hornet seen at City bridge");
            public static readonly FlagDef hornetCityBridge_ready = new FlagDef("hornetCityBridge_ready", null, false, "PlayerData_Bool", "Hornet at City bridge");
            public static readonly FlagDef hornetDenEncounter = new FlagDef("hornetDenEncounter", null, false, "PlayerData_Bool", "Hornet encountered in Distant Village after Herrah's death");
            public static readonly FlagDef hornetFountainEncounter = new FlagDef("hornetFountainEncounter", null, false, "PlayerData_Bool", "Hornet encountered at fountain in City of Tears");
            public static readonly FlagDef hornetFung = new FlagDef("hornetFung", null, false, "PlayerData_Int", "Hornet seen in " + SceneInstances.Fungus2_10.ReadableName);
            public static readonly FlagDef hornetGreenpath = new FlagDef("hornetGreenpath", null, false, "PlayerData_Int", "Saw Hornet at Greenpath (0-4)");
            public static readonly FlagDef HU_encountered = new FlagDef("HU_encountered", null, false, "PlayerData_Bool", "Met Elder Hu");
            public static readonly FlagDef hunterRoared = new FlagDef("hunterRoared", null, false, "PlayerData_Bool", "Hunter Roared");
            public static readonly FlagDef jijiMet = new FlagDef("jijiMet", null, false, "PlayerData_Bool", "Met Jiji");
            public static readonly FlagDef marmOutside = new FlagDef("marmOutside", null, false, "PlayerData_Bool", "Lemm at Fountain Square");
            public static readonly FlagDef marmOutsideConvo = new FlagDef("marmOutsideConvo", null, false, "PlayerData_Bool", "Talked with Lemm at Fountain");
            public static readonly FlagDef maskmakerConvo1 = new FlagDef("maskmakerConvo1", null, false, "PlayerData_Bool", "Mask Maker conversation 1");
            public static readonly FlagDef maskmakerConvo2 = new FlagDef("maskmakerConvo2", null, false, "PlayerData_Bool", "Mask Maker conversation 2");
            public static readonly FlagDef maskmakerMet = new FlagDef("maskmakerMet", null, false, "PlayerData_Bool", "Met Mask Maker");
            public static readonly FlagDef metBanker = new FlagDef("metBanker", null, false, "PlayerData_Bool", "Met Millibelle the Banker");
            public static readonly FlagDef metBrum = new FlagDef("metBrum", null, false, "PlayerData_Bool", "Met Brumm");
            public static readonly FlagDef metCharmSlug = new FlagDef("metCharmSlug", null, false, "PlayerData_Bool", "Met Salubra");
            public static readonly FlagDef metCloth = new FlagDef("metCloth", null, false, "PlayerData_Bool", "Met Cloth");
            public static readonly FlagDef metCornifer = new FlagDef("metCornifer", null, false, "PlayerData_Bool", "Met Cornifer");
            public static readonly FlagDef metDivine = new FlagDef("metDivine", null, false, "PlayerData_Bool", "Met Divine");
            public static readonly FlagDef metElderbug = new FlagDef("metElderbug", null, false, "PlayerData_Bool", "Met Elderbug");
            public static readonly FlagDef metGiraffe = new FlagDef("metGiraffe", null, false, "PlayerData_Bool", "Met Willoh");
            public static readonly FlagDef metGrimm = new FlagDef("metGrimm", null, false, "PlayerData_Bool", "Met Grimm");
            public static readonly FlagDef metHunter = new FlagDef("metHunter", null, false, "PlayerData_Bool", "Met Hunter");
            public static readonly FlagDef metIselda = new FlagDef("metIselda", null, false, "PlayerData_Bool", "Met Iselda");
            public static readonly FlagDef metLegEater = new FlagDef("metLegEater", null, false, "PlayerData_Bool", "Met Leg Eater");
            public static readonly FlagDef metMiner = new FlagDef("metMiner", null, false, "PlayerData_Bool", "Met Myla the Miner");
            public static readonly FlagDef metMoth = new FlagDef("metMoth", null, false, "PlayerData_Bool", "Met the Seer");
            public static readonly FlagDef metNailmasterMato = new FlagDef("metNailmasterMato", null, false, "PlayerData_Bool", "Met Nailmaster Mato");
            public static readonly FlagDef metNailsmith = new FlagDef("metNailsmith", null, false, "PlayerData_Bool", "Met Nailsmith");
            public static readonly FlagDef metQuirrel = new FlagDef("metQuirrel", null, false, "PlayerData_Bool", "Met Quirrel");
            public static readonly FlagDef metSlyShop = new FlagDef("metSlyShop", null, false, "PlayerData_Bool", "Met Sly at Shop");
            public static readonly FlagDef metStag = new FlagDef("metStag", null, false, "PlayerData_Bool", "Met Stag");
            public static readonly FlagDef midwifeConvo1 = new FlagDef("midwifeConvo1", null, false, "PlayerData_Bool", "Midwife conversation 1");
            public static readonly FlagDef midwifeMet = new FlagDef("midwifeMet", null, false, "PlayerData_Bool", "Met Midwife");
            public static readonly FlagDef mossCultist = new FlagDef("mossCultist", null, false, "PlayerData_Int", "Spoke with Moss Prophet (0-2)");
            public static readonly FlagDef NOEYES_encountered = new FlagDef("NOEYES_encountered", null, false, "PlayerData_Bool", "Met No Eyes");
            public static readonly FlagDef paidLegEater = new FlagDef("paidLegEater", null, false, "PlayerData_Bool", "Paid Leg Eater");
            public static readonly FlagDef quirrelEggTemple = new FlagDef("quirrelEggTemple", null, false, "PlayerData_Int", "Talked with Quirrel about Egg Temple (0-4)");
            public static readonly FlagDef quirrelLeftEggTemple = new FlagDef("quirrelLeftEggTemple", null, false, "PlayerData_Bool", "Quirrel left Egg Temple, triggers after False Knight defeated");
            public static readonly FlagDef quirrelLeftStation = new FlagDef("quirrelLeftStation", null, false, "PlayerData_Bool", "Quirrel left Queen's Station, triggered when entering Queen's Station");
            public static readonly FlagDef quirrelSlugShrine = new FlagDef("quirrelSlugShrine", null, false, "PlayerData_Int", "Talked with Quirrel at Slug Shrine (0-3)");
            public static readonly FlagDef quirrelSpaEncountered = new FlagDef("quirrelSpaEncountered", null, false, "PlayerData_Bool", "Met Quirrel at Deepnest spa");
            public static readonly FlagDef quirrelSpaReady = new FlagDef("quirrelSpaReady", null, false, "PlayerData_Bool", "Quirrel at Deepnest Spa");
            public static readonly FlagDef savedCloth = new FlagDef("savedCloth", null, false, "PlayerData_Bool", "Saved Cloth from Mawleks");
            public static readonly FlagDef shaman = new FlagDef("shaman", null, false, "PlayerData_Int", "Talked with Snail Shaman (0-6)");
            public static readonly FlagDef slyRescued = new FlagDef("slyRescued", null, false, "PlayerData_Bool", "Rescued Sly");
            public static readonly FlagDef stagConvoTram = new FlagDef("stagConvoTram", null, false, "PlayerData_Bool", "Stag discusses tram pass");
            public static readonly FlagDef stagRemember1 = new FlagDef("stagRemember1", null, false, "PlayerData_Bool", "Stag memories 1 after opening enough stations");
            public static readonly FlagDef tisoEncounteredTown = new FlagDef("tisoEncounteredTown", null, false, "PlayerData_Bool", "Met Tiso in Dirtmouth");
            public static readonly FlagDef troupeInTown = new FlagDef("troupeInTown", null, false, "PlayerData_Bool", "Nightmare Troupe in Dirtmouth");
            public static readonly FlagDef XERO_encountered = new FlagDef("XERO_encountered", null, false, "PlayerData_Bool", "Met Xero");
            public static readonly FlagDef zote = new FlagDef("zote", null, false, "PlayerData_Int", "Zote conversations (0-2)");
            public static readonly FlagDef zoteRescuedBuzzer = new FlagDef("zoteRescuedBuzzer", null, false, "PlayerData_Bool", "Zote rescued from Vengefly King");
            public static readonly FlagDef zoteRescuedDeepnest = new FlagDef("zoteRescuedDeepnest", null, false, "PlayerData_Bool", "Zoe rescued from web in Deepnest");
            public static readonly FlagDef zoteTownConvo = new FlagDef("zoteTownConvo", null, false, "PlayerData_Int", "Spoke with Zote in Dirtmouth (0-2)");
        #endregion

        #region Player Status
            public static readonly FlagDef beamDamage = new FlagDef("beamDamage", null, false, "PlayerData_Int", "");
            public static readonly FlagDef geoPool = new FlagDef("geoPool", null, false, "PlayerData_Int", "Geo stored on Shade Corpse");
            public static readonly FlagDef health = new FlagDef("health", null, false, "PlayerData_Int");
            public static readonly FlagDef healthBlue = new FlagDef("healthBlue", null, false, "PlayerData_Int", "Blue extra hit points");
            public static readonly FlagDef maxHealth = new FlagDef("maxHealth", null, false, "PlayerData_Int");
            public static readonly FlagDef maxHealthBase = new FlagDef("maxHealthBase", null, false, "PlayerData_Int",  "Max Health (5-9)");
            public static readonly FlagDef maxHealthCap = new FlagDef("maxHealthCap", null, false, "PlayerData_Int");
            public static readonly FlagDef maxMP = new FlagDef("maxMP", null, false, "PlayerData_Int");
            public static readonly FlagDef MPCharge = new FlagDef("MPCharge", null, false, "PlayerData_Int");
            public static readonly FlagDef MPReserveCap = new FlagDef("MPReserveCap", null, false, "PlayerData_Int");
            public static readonly FlagDef MPReserveMax = new FlagDef("MPReserveMax", null, false, "PlayerData_Int", "Added soul reserves from vessels");
            public static readonly FlagDef nailDamage = new FlagDef("nailDamage", null, false, "PlayerData_Int", "Nail Damage");
            public static readonly FlagDef soulLimited = new FlagDef("soulLimited", null, false, "PlayerData_Bool", "Soul Limited from active Death Shade");
        #endregion

        #region Progression
            public static readonly FlagDef guardiansDefeated = new FlagDef("guardiansDefeated", null, false, "PlayerData_Int", "Dreamers defeated (0-3)");
            public static readonly FlagDef maskToBreak = new FlagDef("maskToBreak", null, false, "PlayerData_Int", "Dreamer Seals broken (0-3)");

            public static readonly FlagDef hegemolDefeated = new FlagDef("hegemolDefeated", null, false, "PlayerData_Bool", "Broke Herrah defeated");
            public static readonly FlagDef maskBrokenHegemol = new FlagDef("maskBrokenHegemol", null, false, "PlayerData_Bool", "Broke Herrah the Beast's Seal");
        #endregion

        #region Visited Locations
            public static readonly FlagDef eggTempleVisited = new FlagDef("eggTempleVisited", null, false, "PlayerData_Bool");
            public static readonly FlagDef enteredDreamWorld = new FlagDef("enteredDreamWorld", null, false, "PlayerData_Bool");
            public static readonly FlagDef visitedAbyss = new FlagDef("visitedAbyss", null, false, "PlayerData_Bool");
            public static readonly FlagDef visitedAbyssLower = new FlagDef("visitedAbyssLower", null, false, "PlayerData_Bool");
            public static readonly FlagDef visitedCliffs = new FlagDef("visitedCliffs", null, false, "PlayerData_Bool");
            public static readonly FlagDef visitedCrossroads = new FlagDef("visitedCrossroads", null, false, "PlayerData_Bool");
            public static readonly FlagDef visitedCrossroadsInfected = new FlagDef("visitedCrossroadsInfected", null, false, "PlayerData_Bool");
            public static readonly FlagDef visitedDeepnest = new FlagDef("visitedDeepnest", null, false, "PlayerData_Bool");
            public static readonly FlagDef visitedDeepnestSpa = new FlagDef("visitedDeepnestSpa", null, false, "PlayerData_Bool");
            public static readonly FlagDef visitedDirtmouth = new FlagDef("visitedDirtmouth", null, false, "PlayerData_Bool");
            public static readonly FlagDef visitedFogCanyon = new FlagDef("visitedFogCanyon", null, false, "PlayerData_Bool");
            public static readonly FlagDef visitedFungus = new FlagDef("visitedFungus", null, false, "PlayerData_Bool");
            public static readonly FlagDef visitedGodhome = new FlagDef("visitedGodhome", null, false, "PlayerData_Bool");
            public static readonly FlagDef visitedGreenpath = new FlagDef("visitedGreenpath", null, false, "PlayerData_Bool");
            public static readonly FlagDef visitedHive = new FlagDef("visitedHive", null, false, "PlayerData_Bool");
            public static readonly FlagDef visitedMines = new FlagDef("visitedMines", null, false, "PlayerData_Bool");
            public static readonly FlagDef visitedMines10 = new FlagDef("visitedMines10", null, false, "PlayerData_Bool");
            public static readonly FlagDef visitedOutskirts = new FlagDef("visitedOutskirts", null, false, "PlayerData_Bool");
            public static readonly FlagDef visitedRestingGrounds = new FlagDef("visitedRestingGrounds", null, false, "PlayerData_Bool");
            public static readonly FlagDef visitedRoyalGardens = new FlagDef("visitedRoyalGardens", null, false, "PlayerData_Bool");
            public static readonly FlagDef visitedRuins = new FlagDef("visitedRuins", null, false, "PlayerData_Bool");
            public static readonly FlagDef visitedWaterways = new FlagDef("visitedWaterways", null, false, "PlayerData_Bool");
            public static readonly FlagDef visitedWhitePalace = new FlagDef("visitedWhitePalace", null, false, "PlayerData_Bool");
        #endregion

        #region UNCATEGORIZED SCENE FLAGS
            // Abyss_03_c
            public static readonly FlagDef Abyss_03_c__Breakable_Wall = new FlagDef("Breakable Wall", SceneInstances.Abyss_03_c, false, "PersistentBoolData");

            // Cliffs_02
            public static readonly FlagDef Cliffs_02__One_Way_Wall = new FlagDef("One Way Wall", SceneInstances.Cliffs_02, false, "PersistentBoolData");

            // Crossroads_01
            public static readonly FlagDef Crossroads_01__Shiny_Item = new FlagDef("Shiny Item", SceneInstances.Crossroads_01, false, "PersistentBoolData");

            // Crossroads_03
            public static readonly FlagDef Crossroads_03__Break_Floor_1 = new FlagDef("Break Floor 1", SceneInstances.Crossroads_03, false, "PersistentBoolData");

            // Crossroads_07
            public static readonly FlagDef Crossroads_07__Breakable_Wall_Silhouette = new FlagDef("Breakable Wall_Silhouette", SceneInstances.Crossroads_07, false, "PersistentBoolData");
            public static readonly FlagDef Crossroads_07__Remasker = new FlagDef("Remasker", SceneInstances.Crossroads_07, false, "PersistentBoolData");

            // Crossroads_10
            public static readonly FlagDef Crossroads_10__Prayer_Slug = new FlagDef("Prayer Slug", SceneInstances.Crossroads_10, false, "PersistentBoolData");
            public static readonly FlagDef Crossroads_10__Prayer_Slug_1 = new FlagDef("Prayer Slug (1)", SceneInstances.Crossroads_10, false, "PersistentBoolData");

            // Crossroads_18
            public static readonly FlagDef Crossroads_18__Breakable_Wall_Waterways = new FlagDef("Breakable Wall Waterways", SceneInstances.Crossroads_18, false, "PersistentBoolData");
            public static readonly FlagDef Crossroads_18__Inverse_Remasker = new FlagDef("Inverse Remasker", SceneInstances.Crossroads_18, false, "PersistentBoolData");

            // Crossroads_27
            public static readonly FlagDef Crossroads_21__Collapser_Small = new FlagDef("Collapser Small", SceneInstances.Crossroads_21, false, "PersistentBoolData");

            // Crossroads_33
            public static readonly FlagDef Crossroads_33__Shiny = new FlagDef("Shiny", SceneInstances.Crossroads_33, false, "PersistentBoolData");

            // Crossroads_38
            public static readonly FlagDef Crossroads_38__Reward_10 = new FlagDef("Reward 10", SceneInstances.Crossroads_38, false, "PersistentBoolData");
            public static readonly FlagDef Crossroads_38__Reward_16 = new FlagDef("Reward 16", SceneInstances.Crossroads_38, false, "PersistentBoolData");
            public static readonly FlagDef Crossroads_38__Reward_23 = new FlagDef("Reward 23", SceneInstances.Crossroads_38, false, "PersistentBoolData");
            public static readonly FlagDef Crossroads_38__Reward_31 = new FlagDef("Reward 31", SceneInstances.Crossroads_38, false, "PersistentBoolData");
            public static readonly FlagDef Crossroads_38__Reward_38 = new FlagDef("Reward 38", SceneInstances.Crossroads_38, false, "PersistentBoolData");
            public static readonly FlagDef Crossroads_38__Reward_46 = new FlagDef("Reward 46", SceneInstances.Crossroads_38, false, "PersistentBoolData");
            public static readonly FlagDef Crossroads_38__Reward_5 = new FlagDef("Reward 5", SceneInstances.Crossroads_38, false, "PersistentBoolData");

            // Deepnest_01
            public static readonly FlagDef Deepnest_01__Breakable_Wall = new FlagDef("Breakable Wall", SceneInstances.Deepnest_01, false, "PersistentBoolData");

            // Deepnest_03
            public static readonly FlagDef Deepnest_03__Breakable_Wall = new FlagDef("Breakable Wall", SceneInstances.Deepnest_03, false, "PersistentBoolData");

            // Deepnest_16
            public static readonly FlagDef Deepnest_16__Shiny_Item = new FlagDef("Shiny Item", SceneInstances.Deepnest_16, false, "PersistentBoolData");

            // Deepnest_26
            public static readonly FlagDef Deepnest_26__Shiny_Item = new FlagDef("Shiny Item", SceneInstances.Deepnest_26, false, "PersistentBoolData");

            // Deepnest_39
            public static readonly FlagDef Deepnest_39__Breakable_Wall = new FlagDef("Breakable Wall", SceneInstances.Deepnest_39, false, "PersistentBoolData");
            public static readonly FlagDef Deepnest_39__One_Way_Wall_1 = new FlagDef("One Way Wall (1)", SceneInstances.Deepnest_39, false, "PersistentBoolData");

            // Deepnest_East_01
            public static readonly FlagDef Deepnest_East_01__One_Way_Wall = new FlagDef("One Way Wall", SceneInstances.Deepnest_East_01, false, "PersistentBoolData");

            // Deepnest_East_09
            public static readonly FlagDef Deepnest_East_09__One_Way_Wall = new FlagDef("One Way Wall", SceneInstances.Deepnest_East_09, false, "PersistentBoolData");

            // Deepnest_Spider_Town
            public static readonly FlagDef Deepnest_Spider_Town__Collapser_Small_12 = new FlagDef("Collapser Small (12)", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData");
            public static readonly FlagDef Deepnest_Spider_Town__Collapser_Small_4 = new FlagDef("Collapser Small (4)", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData");
            public static readonly FlagDef Deepnest_Spider_Town__one_way_permanent = new FlagDef("one way permanent", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData");
            public static readonly FlagDef Deepnest_Spider_Town__One_Way_Wall = new FlagDef("One Way Wall", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData");

            // Fungus1_08
            public static readonly FlagDef Fungus1_08__Break_Floor_1 = new FlagDef("Break Floor 1", SceneInstances.Fungus1_08, false, "PersistentBoolData");
            public static readonly FlagDef Fungus1_08__Break_Floor_1_1 = new FlagDef("Break Floor 1 (1)", SceneInstances.Fungus1_08, false, "PersistentBoolData");

            // Fungus1_19
            public static readonly FlagDef Fungus1_19__Hornet_Activate = new FlagDef("Hornet_Activate", SceneInstances.Fungus1_19, false, "PersistentBoolData");

            // Fungus1_21
            public static readonly FlagDef Fungus1_21__Battle_Scene = new FlagDef("Battle Scene", SceneInstances.Fungus1_21, false, "PersistentBoolData");

            // Fungus1_31
            public static readonly FlagDef Fungus1_31__Breakable_Wall = new FlagDef("Breakable Wall", SceneInstances.Fungus1_31, false, "PersistentBoolData");
            public static readonly FlagDef Fungus1_31__Toll_Gate_Machine_1 = new FlagDef("Toll Gate Machine (1)", SceneInstances.Fungus1_31, false, "PersistentBoolData");

            // Fungus1_32
            public static readonly FlagDef Fungus1_32__Breakable_Wall = new FlagDef("Breakable Wall", SceneInstances.Fungus1_32, false, "PersistentBoolData");

            // Fungus2_15
            public static readonly FlagDef Fungus2_15__Breakable_Wall = new FlagDef("Breakable Wall", SceneInstances.Fungus2_15, false, "PersistentBoolData");

            // Fungus2_18
            public static readonly FlagDef Fungus2_18__Shiny = new FlagDef("Shiny", SceneInstances.Fungus2_18, false, "PersistentBoolData");

            // Fungus2_25
            public static readonly FlagDef Fungus2_25__Collapser_Small = new FlagDef("Collapser Small", SceneInstances.Fungus2_25, false, "PersistentBoolData");
            public static readonly FlagDef Fungus2_25__Collapser_Small_1 = new FlagDef("Collapser Small (1)", SceneInstances.Fungus2_25, false, "PersistentBoolData");
            public static readonly FlagDef Fungus2_25__Heart_Piece = new FlagDef("Heart Piece", SceneInstances.Fungus2_25, false, "PersistentBoolData");

            // Fungus2_29
            public static readonly FlagDef Fungus2_29__Break_Floor_1 = new FlagDef("Break Floor 1", SceneInstances.Fungus2_29, false, "PersistentBoolData");
            public static readonly FlagDef Fungus2_29__Shiny_Item = new FlagDef("Shiny Item", SceneInstances.Fungus2_29, false, "PersistentBoolData");

            // Fungus2_34
            public static readonly FlagDef Fungus2_34__Shiny_Item = new FlagDef("Shiny Item", SceneInstances.Fungus2_34, false, "PersistentBoolData");

            // Fungus3_39
            public static readonly FlagDef Fungus3_39__Battle_Scene = new FlagDef("Battle Scene", SceneInstances.Fungus3_39, false, "PersistentBoolData");
            public static readonly FlagDef Fungus3_39__One_Way_Wall = new FlagDef("One Way Wall", SceneInstances.Fungus3_39, false, "PersistentBoolData");
            public static readonly FlagDef Fungus3_39__Shiny_Item_Stand = new FlagDef("Shiny Item Stand", SceneInstances.Fungus3_39, false, "PersistentBoolData");

            // Fungus3_44
            public static readonly FlagDef Fungus3_44__One_Way_Wall = new FlagDef("One Way Wall", SceneInstances.Fungus3_44, false, "PersistentBoolData");
            public static readonly FlagDef Fungus3_44__Ruins_Lever = new FlagDef("Ruins Lever", SceneInstances.Fungus3_44, false, "PersistentBoolData");

            // Mines_01
            public static readonly FlagDef Mines_01__Egg_Sac = new FlagDef("Egg Sac", SceneInstances.Mines_01, false, "PersistentBoolData");
            public static readonly FlagDef Mines_01__mine_1_quake_floor = new FlagDef("mine_1_quake_floor", SceneInstances.Mines_01, false, "PersistentBoolData");

            // Mines_04
            public static readonly FlagDef Mines_04__Mines_Lever = new FlagDef("Mines Lever", SceneInstances.Mines_04, false, "PersistentBoolData");

            // Mines_33
            public static readonly FlagDef Mines_33__Toll_Gate_Machine_1 = new FlagDef("Toll Gate Machine (1)", SceneInstances.Mines_33, false, "PersistentBoolData");

            // RestingGrounds_05
            public static readonly FlagDef RestingGrounds_05__Quake_Floor = new FlagDef("Quake Floor", SceneInstances.RestingGrounds_05, false, "PersistentBoolData");

            // Ruins1_04
            public static readonly FlagDef Ruins1_04__One_Way_Wall = new FlagDef("One Way Wall", SceneInstances.Ruins1_04, false, "PersistentBoolData");

            // Ruins2_01_b
            public static readonly FlagDef Ruins2_01_b__Battle_Scene = new FlagDef("Battle Scene", SceneInstances.Ruins2_01_b, false, "PersistentBoolData");

            // Town
            public static readonly FlagDef Town__Interact_Reminder = new FlagDef("Interact Reminder", SceneInstances.Town, false, "PersistentBoolData");
            public static readonly FlagDef Town__Mines_Lever = new FlagDef("Mines Lever", SceneInstances.Town, false, "PersistentBoolData");
        #endregion

        #region UNCATEGORIZED GLOBAL FLAGS
            // Global
            public static readonly FlagDef MARKOTH_encountered = new FlagDef("MARKOTH_encountered", null, false, "PlayerData_Bool");
            public static readonly FlagDef MUMCAT_encountered = new FlagDef("MUMCAT_encountered", null, false, "PlayerData_Bool");
            public static readonly FlagDef abyssGateOpened = new FlagDef("abyssGateOpened", null, false, "PlayerData_Bool");
            public static readonly FlagDef abyssLighthouse = new FlagDef("abyssLighthouse", null, false, "PlayerData_Bool");
            public static readonly FlagDef allBelieverTabletsDestroyed = new FlagDef("allBelieverTabletsDestroyed", null, false, "PlayerData_Bool");
            public static readonly FlagDef atMapPrompt = new FlagDef("atMapPrompt", null, false, "PlayerData_Bool");
            public static readonly FlagDef awardAllAchievements = new FlagDef("awardAllAchievements", null, false, "PlayerData_Bool");
            public static readonly FlagDef backerCredits = new FlagDef("backerCredits", null, false, "PlayerData_Bool");
            public static readonly FlagDef bankerDeclined = new FlagDef("bankerDeclined", null, false, "PlayerData_Bool");
            public static readonly FlagDef bankerSpaMet = new FlagDef("bankerSpaMet", null, false, "PlayerData_Bool");
            public static readonly FlagDef bankerTheft = new FlagDef("bankerTheft", null, false, "PlayerData_Int");
            public static readonly FlagDef bankerTheftCheck = new FlagDef("bankerTheftCheck", null, false, "PlayerData_Bool");
            public static readonly FlagDef bathHouseOpened = new FlagDef("bathHouseOpened", null, false, "PlayerData_Bool");
            public static readonly FlagDef bathHouseWall = new FlagDef("bathHouseWall", null, false, "PlayerData_Bool");
            public static readonly FlagDef betaEnd = new FlagDef("betaEnd", null, false, "PlayerData_Bool");
            public static readonly FlagDef bigCatHitTail = new FlagDef("bigCatHitTail", null, false, "PlayerData_Bool");
            public static readonly FlagDef bigCatHitTailConvo = new FlagDef("bigCatHitTailConvo", null, false, "PlayerData_Bool");
            public static readonly FlagDef bigCatKingsBrandConvo = new FlagDef("bigCatKingsBrandConvo", null, false, "PlayerData_Bool");
            public static readonly FlagDef bigCatMeet = new FlagDef("bigCatMeet", null, false, "PlayerData_Bool");
            public static readonly FlagDef bigCatShadeConvo = new FlagDef("bigCatShadeConvo", null, false, "PlayerData_Bool");
            public static readonly FlagDef bigCatTalk1 = new FlagDef("bigCatTalk1", null, false, "PlayerData_Bool");
            public static readonly FlagDef bigCatTalk2 = new FlagDef("bigCatTalk2", null, false, "PlayerData_Bool");
            public static readonly FlagDef bigCatTalk3 = new FlagDef("bigCatTalk3", null, false, "PlayerData_Bool");
            public static readonly FlagDef blizzardEnded = new FlagDef("blizzardEnded", null, false, "PlayerData_Bool");
            public static readonly FlagDef blocker1Defeated = new FlagDef("blocker1Defeated", null, false, "PlayerData_Bool");
            public static readonly FlagDef blocker2Defeated = new FlagDef("blocker2Defeated", null, false, "PlayerData_Bool");
            public static readonly FlagDef blockerHits = new FlagDef("blockerHits", null, false, "PlayerData_Int");
            public static readonly FlagDef blueRoomActivated = new FlagDef("blueRoomActivated", null, false, "PlayerData_Bool");
            public static readonly FlagDef blueRoomDoorUnlocked = new FlagDef("blueRoomDoorUnlocked", null, false, "PlayerData_Bool");
            public static readonly FlagDef blueVineDoor = new FlagDef("blueVineDoor", null, false, "PlayerData_Bool");
            public static readonly FlagDef bossDoorCageUnlocked = new FlagDef("bossDoorCageUnlocked", null, false, "PlayerData_Bool");
            public static readonly FlagDef bossDoorEntranceTextSeen = new FlagDef("bossDoorEntranceTextSeen", null, false, "PlayerData_Int");
            public static readonly FlagDef bossRushMode = new FlagDef("bossRushMode", null, false, "PlayerData_Bool");
            public static readonly FlagDef bossStatueTargetLevel = new FlagDef("bossStatueTargetLevel", null, false, "PlayerData_Int");
            public static readonly FlagDef brettaLeftTown = new FlagDef("brettaLeftTown", null, false, "PlayerData_Bool");
            public static readonly FlagDef brettaSeenBed = new FlagDef("brettaSeenBed", null, false, "PlayerData_Bool");
            public static readonly FlagDef brettaSeenBedDiary = new FlagDef("brettaSeenBedDiary", null, false, "PlayerData_Bool");
            public static readonly FlagDef brettaSeenBench = new FlagDef("brettaSeenBench", null, false, "PlayerData_Bool");
            public static readonly FlagDef brettaSeenBenchDiary = new FlagDef("brettaSeenBenchDiary", null, false, "PlayerData_Bool");
            public static readonly FlagDef brettaState = new FlagDef("brettaState", null, false, "PlayerData_Int");
            public static readonly FlagDef brokeMinersWall = new FlagDef("brokeMinersWall", null, false, "PlayerData_Bool");
            public static readonly FlagDef brokenMageWindow = new FlagDef("brokenMageWindow", null, false, "PlayerData_Bool");
            public static readonly FlagDef brokenMageWindowGlass = new FlagDef("brokenMageWindowGlass", null, false, "PlayerData_Bool");
            public static readonly FlagDef brummBrokeBrazier = new FlagDef("brummBrokeBrazier", null, false, "PlayerData_Bool");
            public static readonly FlagDef canBackDash = new FlagDef("canBackDash", null, false, "PlayerData_Bool");
            public static readonly FlagDef canDash = new FlagDef("canDash", null, false, "PlayerData_Bool");
            public static readonly FlagDef canShadowDash = new FlagDef("canShadowDash", null, false, "PlayerData_Bool");
            public static readonly FlagDef canSuperDash = new FlagDef("canSuperDash", null, false, "PlayerData_Bool");
            public static readonly FlagDef canWallJump = new FlagDef("canWallJump", null, false, "PlayerData_Bool");
            public static readonly FlagDef city2_sewerDoor = new FlagDef("city2_sewerDoor", null, false, "PlayerData_Bool");
            public static readonly FlagDef cityBridge1 = new FlagDef("cityBridge1", null, false, "PlayerData_Bool");
            public static readonly FlagDef cityBridge2 = new FlagDef("cityBridge2", null, false, "PlayerData_Bool");
            public static readonly FlagDef cityGateClosed = new FlagDef("cityGateClosed", null, false, "PlayerData_Bool");
            public static readonly FlagDef cityLift1 = new FlagDef("cityLift1", null, false, "PlayerData_Bool");
            public static readonly FlagDef cityLift1_isUp = new FlagDef("cityLift1_isUp", null, false, "PlayerData_Bool");
            public static readonly FlagDef cityLift2 = new FlagDef("cityLift2", null, false, "PlayerData_Bool");
            public static readonly FlagDef cityLift2_isUp = new FlagDef("cityLift2_isUp", null, false, "PlayerData_Bool");
            public static readonly FlagDef clothEncounteredQueensGarden = new FlagDef("clothEncounteredQueensGarden", null, false, "PlayerData_Bool");
            public static readonly FlagDef clothEnteredTramRoom = new FlagDef("clothEnteredTramRoom", null, false, "PlayerData_Bool");
            public static readonly FlagDef clothGhostSpoken = new FlagDef("clothGhostSpoken", null, false, "PlayerData_Bool");
            public static readonly FlagDef clothInTown = new FlagDef("clothInTown", null, false, "PlayerData_Bool");
            public static readonly FlagDef clothKilled = new FlagDef("clothKilled", null, false, "PlayerData_Bool");
            public static readonly FlagDef clothLeftTown = new FlagDef("clothLeftTown", null, false, "PlayerData_Bool");
            public static readonly FlagDef collectorDefeated = new FlagDef("collectorDefeated", null, false, "PlayerData_Bool");
            public static readonly FlagDef colosseumBronzeCompleted = new FlagDef("colosseumBronzeCompleted", null, false, "PlayerData_Bool");
            public static readonly FlagDef colosseumBronzeOpened = new FlagDef("colosseumBronzeOpened", null, false, "PlayerData_Bool");
            public static readonly FlagDef colosseumGoldCompleted = new FlagDef("colosseumGoldCompleted", null, false, "PlayerData_Bool");
            public static readonly FlagDef colosseumGoldOpened = new FlagDef("colosseumGoldOpened", null, false, "PlayerData_Bool");
            public static readonly FlagDef colosseumSilverCompleted = new FlagDef("colosseumSilverCompleted", null, false, "PlayerData_Bool");
            public static readonly FlagDef colosseumSilverOpened = new FlagDef("colosseumSilverOpened", null, false, "PlayerData_Bool");
            public static readonly FlagDef completedQuakeArea = new FlagDef("completedQuakeArea", null, false, "PlayerData_Bool");
            public static readonly FlagDef completedRGDreamPlant = new FlagDef("completedRGDreamPlant", null, false, "PlayerData_Bool");
            public static readonly FlagDef corn_abyssLeft = new FlagDef("corn_abyssLeft", null, false, "PlayerData_Bool");
            public static readonly FlagDef corn_cityEncountered = new FlagDef("corn_cityEncountered", null, false, "PlayerData_Bool");
            public static readonly FlagDef corn_cityLeft = new FlagDef("corn_cityLeft", null, false, "PlayerData_Bool");
            public static readonly FlagDef corn_crossroadsEncountered = new FlagDef("corn_crossroadsEncountered", null, false, "PlayerData_Bool");
            public static readonly FlagDef corn_deepnestMet2 = new FlagDef("corn_deepnestMet2", null, false, "PlayerData_Bool");
            public static readonly FlagDef corn_fogCanyonEncountered = new FlagDef("corn_fogCanyonEncountered", null, false, "PlayerData_Bool");
            public static readonly FlagDef corn_fogCanyonLeft = new FlagDef("corn_fogCanyonLeft", null, false, "PlayerData_Bool");
            public static readonly FlagDef corn_minesEncountered = new FlagDef("corn_minesEncountered", null, false, "PlayerData_Bool");
            public static readonly FlagDef corn_minesLeft = new FlagDef("corn_minesLeft", null, false, "PlayerData_Bool");
            public static readonly FlagDef corn_outskirtsEncountered = new FlagDef("corn_outskirtsEncountered", null, false, "PlayerData_Bool");
            public static readonly FlagDef corn_outskirtsLeft = new FlagDef("corn_outskirtsLeft", null, false, "PlayerData_Bool");
            public static readonly FlagDef corn_royalGardensEncountered = new FlagDef("corn_royalGardensEncountered", null, false, "PlayerData_Bool");
            public static readonly FlagDef corn_royalGardensLeft = new FlagDef("corn_royalGardensLeft", null, false, "PlayerData_Bool");
            public static readonly FlagDef corn_waterwaysEncountered = new FlagDef("corn_waterwaysEncountered", null, false, "PlayerData_Bool");
            public static readonly FlagDef corn_waterwaysLeft = new FlagDef("corn_waterwaysLeft", null, false, "PlayerData_Bool");
            public static readonly FlagDef cornifer = new FlagDef("cornifer", null, false, "PlayerData_Int");
            public static readonly FlagDef corniferAtHome = new FlagDef("corniferAtHome", null, false, "PlayerData_Bool");
            public static readonly FlagDef cultistTransformed = new FlagDef("cultistTransformed", null, false, "PlayerData_Bool");
            public static readonly FlagDef deepnest26b_switch = new FlagDef("deepnest26b_switch", null, false, "PlayerData_Bool");
            public static readonly FlagDef defeatedDungDefender = new FlagDef("defeatedDungDefender", null, false, "PlayerData_Bool");
            public static readonly FlagDef defeatedMegaBeamMiner = new FlagDef("defeatedMegaBeamMiner", null, false, "PlayerData_Bool");
            public static readonly FlagDef defeatedMegaBeamMiner2 = new FlagDef("defeatedMegaBeamMiner2", null, false, "PlayerData_Bool");
            public static readonly FlagDef defeatedMegaJelly = new FlagDef("defeatedMegaJelly", null, false, "PlayerData_Bool");
            public static readonly FlagDef defeatedNightmareGrimm = new FlagDef("defeatedNightmareGrimm", null, false, "PlayerData_Bool");
            public static readonly FlagDef destroyedNightmareLantern = new FlagDef("destroyedNightmareLantern", null, false, "PlayerData_Bool");
            public static readonly FlagDef divineEatenConvos = new FlagDef("divineEatenConvos", null, false, "PlayerData_Int");
            public static readonly FlagDef divineFinalConvo = new FlagDef("divineFinalConvo", null, false, "PlayerData_Bool");
            public static readonly FlagDef dreamMothConvo1 = new FlagDef("dreamMothConvo1", null, false, "PlayerData_Bool");
            public static readonly FlagDef dreamOrbsSpent = new FlagDef("dreamOrbsSpent", null, false, "PlayerData_Int");
            public static readonly FlagDef dreamReward1 = new FlagDef("dreamReward1", null, false, "PlayerData_Bool");
            public static readonly FlagDef dreamReward2 = new FlagDef("dreamReward2", null, false, "PlayerData_Bool");
            public static readonly FlagDef dreamReward3 = new FlagDef("dreamReward3", null, false, "PlayerData_Bool");
            public static readonly FlagDef dreamReward4 = new FlagDef("dreamReward4", null, false, "PlayerData_Bool");
            public static readonly FlagDef dreamReward5 = new FlagDef("dreamReward5", null, false, "PlayerData_Bool");
            public static readonly FlagDef dreamReward5b = new FlagDef("dreamReward5b", null, false, "PlayerData_Bool");
            public static readonly FlagDef dreamReward6 = new FlagDef("dreamReward6", null, false, "PlayerData_Bool");
            public static readonly FlagDef dreamReward7 = new FlagDef("dreamReward7", null, false, "PlayerData_Bool");
            public static readonly FlagDef dreamReward8 = new FlagDef("dreamReward8", null, false, "PlayerData_Bool");
            public static readonly FlagDef dreamReward9 = new FlagDef("dreamReward9", null, false, "PlayerData_Bool");
            public static readonly FlagDef dungDefenderAwakeConvo = new FlagDef("dungDefenderAwakeConvo", null, false, "PlayerData_Bool");
            public static readonly FlagDef dungDefenderAwoken = new FlagDef("dungDefenderAwoken", null, false, "PlayerData_Bool");
            public static readonly FlagDef dungDefenderCharmConvo = new FlagDef("dungDefenderCharmConvo", null, false, "PlayerData_Bool");
            public static readonly FlagDef dungDefenderConvo1 = new FlagDef("dungDefenderConvo1", null, false, "PlayerData_Bool");
            public static readonly FlagDef dungDefenderConvo2 = new FlagDef("dungDefenderConvo2", null, false, "PlayerData_Bool");
            public static readonly FlagDef dungDefenderConvo3 = new FlagDef("dungDefenderConvo3", null, false, "PlayerData_Bool");
            public static readonly FlagDef dungDefenderEncounterReady = new FlagDef("dungDefenderEncounterReady", null, false, "PlayerData_Bool");
            public static readonly FlagDef dungDefenderIsmaConvo = new FlagDef("dungDefenderIsmaConvo", null, false, "PlayerData_Bool");
            public static readonly FlagDef dungDefenderLeft = new FlagDef("dungDefenderLeft", null, false, "PlayerData_Bool");
            public static readonly FlagDef dungDefenderSleeping = new FlagDef("dungDefenderSleeping", null, false, "PlayerData_Bool");
            public static readonly FlagDef dungDefenderWallBroken = new FlagDef("dungDefenderWallBroken", null, false, "PlayerData_Bool");
            public static readonly FlagDef duskKnightDefeated = new FlagDef("duskKnightDefeated", null, false, "PlayerData_Bool");
            public static readonly FlagDef elderbug = new FlagDef("elderbug", null, false, "PlayerData_Int");
            public static readonly FlagDef elderbugBrettaLeft = new FlagDef("elderbugBrettaLeft", null, false, "PlayerData_Bool");
            public static readonly FlagDef elderbugGaveFlower = new FlagDef("elderbugGaveFlower", null, false, "PlayerData_Bool");
            public static readonly FlagDef elderbugNymmConvo = new FlagDef("elderbugNymmConvo", null, false, "PlayerData_Bool");
            public static readonly FlagDef elderbugReintro = new FlagDef("elderbugReintro", null, false, "PlayerData_Bool");
            public static readonly FlagDef elderbugRequestedFlower = new FlagDef("elderbugRequestedFlower", null, false, "PlayerData_Bool");
            public static readonly FlagDef elderbugSpeechBretta = new FlagDef("elderbugSpeechBretta", null, false, "PlayerData_Bool");
            public static readonly FlagDef elderbugSpeechFinalBossDoor = new FlagDef("elderbugSpeechFinalBossDoor", null, false, "PlayerData_Bool");
            public static readonly FlagDef elderbugSpeechInfectedCrossroads = new FlagDef("elderbugSpeechInfectedCrossroads", null, false, "PlayerData_Bool");
            public static readonly FlagDef elderbugSpeechJiji = new FlagDef("elderbugSpeechJiji", null, false, "PlayerData_Bool");
            public static readonly FlagDef elderbugSpeechMinesLift = new FlagDef("elderbugSpeechMinesLift", null, false, "PlayerData_Bool");
            public static readonly FlagDef elderbugTroupeLeftConvo = new FlagDef("elderbugTroupeLeftConvo", null, false, "PlayerData_Bool");
            public static readonly FlagDef emilitiaKingsBrandConvo = new FlagDef("emilitiaKingsBrandConvo", null, false, "PlayerData_Bool");
            public static readonly FlagDef encounteredGatekeeper = new FlagDef("encounteredGatekeeper", null, false, "PlayerData_Bool");
            public static readonly FlagDef encounteredHornet = new FlagDef("encounteredHornet", null, false, "PlayerData_Bool");
            public static readonly FlagDef encounteredMegaJelly = new FlagDef("encounteredMegaJelly", null, false, "PlayerData_Bool");
            public static readonly FlagDef encounteredMimicSpider = new FlagDef("encounteredMimicSpider", null, false, "PlayerData_Bool");
            public static readonly FlagDef enteredGGAtrium = new FlagDef("enteredGGAtrium", null, false, "PlayerData_Bool");
            public static readonly FlagDef enteredMantisLordArea = new FlagDef("enteredMantisLordArea", null, false, "PlayerData_Bool");
            public static readonly FlagDef environmentTypeDefault = new FlagDef("environmentTypeDefault", null, false, "PlayerData_Int");
            public static readonly FlagDef extendedGramophone = new FlagDef("extendedGramophone", null, false, "PlayerData_Bool");
            public static readonly FlagDef extraFlowerAppear = new FlagDef("extraFlowerAppear", null, false, "PlayerData_Bool");
            public static readonly FlagDef falseKnightGhostDeparted = new FlagDef("falseKnightGhostDeparted", null, false, "PlayerData_Bool");
            public static readonly FlagDef falseKnightOrbsCollected = new FlagDef("falseKnightOrbsCollected", null, false, "PlayerData_Bool");
            public static readonly FlagDef falseKnightWallBroken = new FlagDef("falseKnightWallBroken", null, false, "PlayerData_Bool");
            public static readonly FlagDef falseKnightWallRepaired = new FlagDef("falseKnightWallRepaired", null, false, "PlayerData_Bool");
            public static readonly FlagDef fatGrubKing = new FlagDef("fatGrubKing", null, false, "PlayerData_Bool");
            public static readonly FlagDef fillJournal = new FlagDef("fillJournal", null, false, "PlayerData_Bool");
            public static readonly FlagDef finalBossDoorUnlocked = new FlagDef("finalBossDoorUnlocked", null, false, "PlayerData_Bool");
            public static readonly FlagDef finalGrubRewardCollected = new FlagDef("finalGrubRewardCollected", null, false, "PlayerData_Bool");
            public static readonly FlagDef flamesCollected = new FlagDef("flamesCollected", null, false, "PlayerData_Int");
            public static readonly FlagDef flamesRequired = new FlagDef("flamesRequired", null, false, "PlayerData_Int");
            public static readonly FlagDef flukeMotherDefeated = new FlagDef("flukeMotherDefeated", null, false, "PlayerData_Bool");
            public static readonly FlagDef flukeMotherEncountered = new FlagDef("flukeMotherEncountered", null, false, "PlayerData_Bool");
            public static readonly FlagDef focusMP_amount = new FlagDef("focusMP_amount", null, false, "PlayerData_Int");
            public static readonly FlagDef foughtGrimm = new FlagDef("foughtGrimm", null, false, "PlayerData_Bool");
            public static readonly FlagDef foundGhostCoin = new FlagDef("foundGhostCoin", null, false, "PlayerData_Bool");
            public static readonly FlagDef fountainGeo = new FlagDef("fountainGeo", null, false, "PlayerData_Int");
            public static readonly FlagDef fountainVesselSummoned = new FlagDef("fountainVesselSummoned", null, false, "PlayerData_Bool");
            public static readonly FlagDef gaveSlykey = new FlagDef("gaveSlykey", null, false, "PlayerData_Bool");
            public static readonly FlagDef ghostCoins = new FlagDef("ghostCoins", null, false, "PlayerData_Int");
            public static readonly FlagDef giantBuzzerDefeated = new FlagDef("giantBuzzerDefeated", null, false, "PlayerData_Bool");
            public static readonly FlagDef giantFlyDefeated = new FlagDef("giantFlyDefeated", null, false, "PlayerData_Bool");
            public static readonly FlagDef givenEmilitiaFlower = new FlagDef("givenEmilitiaFlower", null, false, "PlayerData_Bool");
            public static readonly FlagDef givenGodseekerFlower = new FlagDef("givenGodseekerFlower", null, false, "PlayerData_Bool");
            public static readonly FlagDef givenOroFlower = new FlagDef("givenOroFlower", null, false, "PlayerData_Bool");
            public static readonly FlagDef givenWhiteLadyFlower = new FlagDef("givenWhiteLadyFlower", null, false, "PlayerData_Bool");
            public static readonly FlagDef gladeDoorOpened = new FlagDef("gladeDoorOpened", null, false, "PlayerData_Bool");
            public static readonly FlagDef gladeGhostsKilled = new FlagDef("gladeGhostsKilled", null, false, "PlayerData_Int");
            public static readonly FlagDef godseekerSpokenAwake = new FlagDef("godseekerSpokenAwake", null, false, "PlayerData_Bool");
            public static readonly FlagDef godseekerUnlocked = new FlagDef("godseekerUnlocked", null, false, "PlayerData_Bool");
            public static readonly FlagDef godseekerWaterwaysSeenState = new FlagDef("godseekerWaterwaysSeenState", null, false, "PlayerData_Int");
            public static readonly FlagDef godseekerWaterwaysSpoken1 = new FlagDef("godseekerWaterwaysSpoken1", null, false, "PlayerData_Bool");
            public static readonly FlagDef godseekerWaterwaysSpoken2 = new FlagDef("godseekerWaterwaysSpoken2", null, false, "PlayerData_Bool");
            public static readonly FlagDef godseekerWaterwaysSpoken3 = new FlagDef("godseekerWaterwaysSpoken3", null, false, "PlayerData_Bool");
            public static readonly FlagDef gotBrummsFlame = new FlagDef("gotBrummsFlame", null, false, "PlayerData_Bool");
            public static readonly FlagDef gotGrimmNotch = new FlagDef("gotGrimmNotch", null, false, "PlayerData_Bool");
            public static readonly FlagDef gotKingFragment = new FlagDef("gotKingFragment", null, false, "PlayerData_Bool");
            public static readonly FlagDef gotLurkerKey = new FlagDef("gotLurkerKey", null, false, "PlayerData_Bool");
            public static readonly FlagDef gotQueenFragment = new FlagDef("gotQueenFragment", null, false, "PlayerData_Bool");
            public static readonly FlagDef gotSlyCharm = new FlagDef("gotSlyCharm", null, false, "PlayerData_Bool");
            public static readonly FlagDef greyPrinceDefeated = new FlagDef("greyPrinceDefeated", null, false, "PlayerData_Bool");
            public static readonly FlagDef greyPrinceDefeats = new FlagDef("greyPrinceDefeats", null, false, "PlayerData_Int");
            public static readonly FlagDef greyPrinceOrbsCollected = new FlagDef("greyPrinceOrbsCollected", null, false, "PlayerData_Bool");
            public static readonly FlagDef heartPieceMax = new FlagDef("heartPieceMax", null, false, "PlayerData_Bool");
            public static readonly FlagDef hornetAbyssEncounter = new FlagDef("hornetAbyssEncounter", null, false, "PlayerData_Bool");
            public static readonly FlagDef hornetOutskirtsDefeated = new FlagDef("hornetOutskirtsDefeated", null, false, "PlayerData_Bool");
            public static readonly FlagDef hornet_f19 = new FlagDef("hornet_f19", null, false, "PlayerData_Bool");
            public static readonly FlagDef hornheadVinePlat = new FlagDef("hornheadVinePlat", null, false, "PlayerData_Bool");
            public static readonly FlagDef hunterRewardOffered = new FlagDef("hunterRewardOffered", null, false, "PlayerData_Bool");
            public static readonly FlagDef huntersMarkOffered = new FlagDef("huntersMarkOffered", null, false, "PlayerData_Bool");
            public static readonly FlagDef ignoredMoth = new FlagDef("ignoredMoth", null, false, "PlayerData_Bool");
            public static readonly FlagDef infectedKnightDreamDefeated = new FlagDef("infectedKnightDreamDefeated", null, false, "PlayerData_Bool");
            public static readonly FlagDef infectedKnightEncountered = new FlagDef("infectedKnightEncountered", null, false, "PlayerData_Bool");
            public static readonly FlagDef infectedKnightOrbsCollected = new FlagDef("infectedKnightOrbsCollected", null, false, "PlayerData_Bool");
            public static readonly FlagDef invinciTest = new FlagDef("invinciTest", null, false, "PlayerData_Bool");
            public static readonly FlagDef iseldaConvo1 = new FlagDef("iseldaConvo1", null, false, "PlayerData_Bool");
            public static readonly FlagDef iseldaConvoGrimm = new FlagDef("iseldaConvoGrimm", null, false, "PlayerData_Bool");
            public static readonly FlagDef iseldaCorniferHomeConvo = new FlagDef("iseldaCorniferHomeConvo", null, false, "PlayerData_Bool");
            public static readonly FlagDef iseldaNymmConvo = new FlagDef("iseldaNymmConvo", null, false, "PlayerData_Bool");
            public static readonly FlagDef jijiGrimmConvo = new FlagDef("jijiGrimmConvo", null, false, "PlayerData_Bool");
            public static readonly FlagDef jijiShadeCharmConvo = new FlagDef("jijiShadeCharmConvo", null, false, "PlayerData_Bool");
            public static readonly FlagDef jijiShadeOffered = new FlagDef("jijiShadeOffered", null, false, "PlayerData_Bool");
            public static readonly FlagDef jinnConvo1 = new FlagDef("jinnConvo1", null, false, "PlayerData_Bool");
            public static readonly FlagDef jinnConvo2 = new FlagDef("jinnConvo2", null, false, "PlayerData_Bool");
            public static readonly FlagDef jinnConvo3 = new FlagDef("jinnConvo3", null, false, "PlayerData_Bool");
            public static readonly FlagDef jinnConvoKingBrand = new FlagDef("jinnConvoKingBrand", null, false, "PlayerData_Bool");
            public static readonly FlagDef jinnConvoShadeCharm = new FlagDef("jinnConvoShadeCharm", null, false, "PlayerData_Bool");
            public static readonly FlagDef jinnEggsSold = new FlagDef("jinnEggsSold", null, false, "PlayerData_Int");
            public static readonly FlagDef joniHealthBlue = new FlagDef("joniHealthBlue", null, false, "PlayerData_Int");
            public static readonly FlagDef legEaterBoughtConvo = new FlagDef("legEaterBoughtConvo", null, false, "PlayerData_Bool");
            public static readonly FlagDef legEaterBrokenConvo = new FlagDef("legEaterBrokenConvo", null, false, "PlayerData_Bool");
            public static readonly FlagDef legEaterConvo1 = new FlagDef("legEaterConvo1", null, false, "PlayerData_Bool");
            public static readonly FlagDef legEaterConvo2 = new FlagDef("legEaterConvo2", null, false, "PlayerData_Bool");
            public static readonly FlagDef legEaterConvo3 = new FlagDef("legEaterConvo3", null, false, "PlayerData_Bool");
            public static readonly FlagDef legEaterDungConvo = new FlagDef("legEaterDungConvo", null, false, "PlayerData_Bool");
            public static readonly FlagDef legEaterGoldConvo = new FlagDef("legEaterGoldConvo", null, false, "PlayerData_Bool");
            public static readonly FlagDef legEaterInfectedCrossroadConvo = new FlagDef("legEaterInfectedCrossroadConvo", null, false, "PlayerData_Bool");
            public static readonly FlagDef legEaterLeft = new FlagDef("legEaterLeft", null, false, "PlayerData_Bool");
            public static readonly FlagDef liftArrival = new FlagDef("liftArrival", null, false, "PlayerData_Bool");
            public static readonly FlagDef littleFoolMet = new FlagDef("littleFoolMet", null, false, "PlayerData_Bool");
            public static readonly FlagDef lurienDefeated = new FlagDef("lurienDefeated", null, false, "PlayerData_Bool");
            public static readonly FlagDef mageLordDefeated = new FlagDef("mageLordDefeated", null, false, "PlayerData_Bool");
            public static readonly FlagDef mageLordDreamDefeated = new FlagDef("mageLordDreamDefeated", null, false, "PlayerData_Bool");
            public static readonly FlagDef mageLordEncountered = new FlagDef("mageLordEncountered", null, false, "PlayerData_Bool");
            public static readonly FlagDef mageLordEncountered_2 = new FlagDef("mageLordEncountered_2", null, false, "PlayerData_Bool");
            public static readonly FlagDef mageLordOrbsCollected = new FlagDef("mageLordOrbsCollected", null, false, "PlayerData_Bool");
            public static readonly FlagDef markothDefeated = new FlagDef("markothDefeated", null, false, "PlayerData_Int");
            public static readonly FlagDef markothPinned = new FlagDef("markothPinned", null, false, "PlayerData_Bool");
            public static readonly FlagDef marmConvo1 = new FlagDef("marmConvo1", null, false, "PlayerData_Bool");
            public static readonly FlagDef marmConvo2 = new FlagDef("marmConvo2", null, false, "PlayerData_Bool");
            public static readonly FlagDef marmConvo3 = new FlagDef("marmConvo3", null, false, "PlayerData_Bool");
            public static readonly FlagDef marmConvoNailsmith = new FlagDef("marmConvoNailsmith", null, false, "PlayerData_Bool");
            public static readonly FlagDef maskBrokenLurien = new FlagDef("maskBrokenLurien", null, false, "PlayerData_Bool");
            public static readonly FlagDef maskBrokenMonomon = new FlagDef("maskBrokenMonomon", null, false, "PlayerData_Bool");
            public static readonly FlagDef maskmakerKingsBrand = new FlagDef("maskmakerKingsBrand", null, false, "PlayerData_Bool");
            public static readonly FlagDef maskmakerShadowDash = new FlagDef("maskmakerShadowDash", null, false, "PlayerData_Bool");
            public static readonly FlagDef maskmakerUnmasked1 = new FlagDef("maskmakerUnmasked1", null, false, "PlayerData_Bool");
            public static readonly FlagDef maskmakerUnmasked2 = new FlagDef("maskmakerUnmasked2", null, false, "PlayerData_Bool");
            public static readonly FlagDef matoConvoOro = new FlagDef("matoConvoOro", null, false, "PlayerData_Bool");
            public static readonly FlagDef matoConvoSheo = new FlagDef("matoConvoSheo", null, false, "PlayerData_Bool");
            public static readonly FlagDef matoConvoSly = new FlagDef("matoConvoSly", null, false, "PlayerData_Bool");
            public static readonly FlagDef mawlekDefeated = new FlagDef("mawlekDefeated", null, false, "PlayerData_Bool");
            public static readonly FlagDef menderDoorOpened = new FlagDef("menderDoorOpened", null, false, "PlayerData_Bool");
            public static readonly FlagDef metEmilitia = new FlagDef("metEmilitia", null, false, "PlayerData_Bool");
            public static readonly FlagDef metJinn = new FlagDef("metJinn", null, false, "PlayerData_Bool");
            public static readonly FlagDef metNailmasterOro = new FlagDef("metNailmasterOro", null, false, "PlayerData_Bool");
            public static readonly FlagDef metNailmasterSheo = new FlagDef("metNailmasterSheo", null, false, "PlayerData_Bool");
            public static readonly FlagDef metQueen = new FlagDef("metQueen", null, false, "PlayerData_Bool");
            public static readonly FlagDef metRelicDealer = new FlagDef("metRelicDealer", null, false, "PlayerData_Bool");
            public static readonly FlagDef metRelicDealerShop = new FlagDef("metRelicDealerShop", null, false, "PlayerData_Bool");
            public static readonly FlagDef metXun = new FlagDef("metXun", null, false, "PlayerData_Bool");
            public static readonly FlagDef midwifeConvo2 = new FlagDef("midwifeConvo2", null, false, "PlayerData_Bool");
            public static readonly FlagDef midwifeWeaverlingConvo = new FlagDef("midwifeWeaverlingConvo", null, false, "PlayerData_Bool");
            public static readonly FlagDef mineLiftOpened = new FlagDef("mineLiftOpened", null, false, "PlayerData_Bool");
            public static readonly FlagDef miner = new FlagDef("miner", null, false, "PlayerData_Int");
            public static readonly FlagDef minerEarly = new FlagDef("minerEarly", null, false, "PlayerData_Int");
            public static readonly FlagDef monomonDefeated = new FlagDef("monomonDefeated", null, false, "PlayerData_Bool");
            public static readonly FlagDef mothDeparted = new FlagDef("mothDeparted", null, false, "PlayerData_Bool");
            public static readonly FlagDef mrMushroomState = new FlagDef("mrMushroomState", null, false, "PlayerData_Int");
            public static readonly FlagDef mumCaterpillarDefeated = new FlagDef("mumCaterpillarDefeated", null, false, "PlayerData_Int");
            public static readonly FlagDef mumCaterpillarPinned = new FlagDef("mumCaterpillarPinned", null, false, "PlayerData_Bool");
            public static readonly FlagDef nailRange = new FlagDef("nailRange", null, false, "PlayerData_Int");
            public static readonly FlagDef nailsmithCliff = new FlagDef("nailsmithCliff", null, false, "PlayerData_Bool");
            public static readonly FlagDef nailsmithConvoArt = new FlagDef("nailsmithConvoArt", null, false, "PlayerData_Bool");
            public static readonly FlagDef nailsmithCorpseAppeared = new FlagDef("nailsmithCorpseAppeared", null, false, "PlayerData_Bool");
            public static readonly FlagDef nailsmithKillSpeech = new FlagDef("nailsmithKillSpeech", null, false, "PlayerData_Bool");
            public static readonly FlagDef nailsmithKilled = new FlagDef("nailsmithKilled", null, false, "PlayerData_Bool");
            public static readonly FlagDef nailsmithSheo = new FlagDef("nailsmithSheo", null, false, "PlayerData_Bool");
            public static readonly FlagDef nailsmithSpared = new FlagDef("nailsmithSpared", null, false, "PlayerData_Bool");
            public static readonly FlagDef newDatTraitorLord = new FlagDef("newDatTraitorLord", null, false, "PlayerData_Bool");
            public static readonly FlagDef notchFogCanyon = new FlagDef("notchFogCanyon", null, false, "PlayerData_Bool");
            public static readonly FlagDef nymmCharmConvo = new FlagDef("nymmCharmConvo", null, false, "PlayerData_Bool");
            public static readonly FlagDef nymmFinalConvo = new FlagDef("nymmFinalConvo", null, false, "PlayerData_Bool");
            public static readonly FlagDef nymmInTown = new FlagDef("nymmInTown", null, false, "PlayerData_Bool");
            public static readonly FlagDef nymmMissedEggOpen = new FlagDef("nymmMissedEggOpen", null, false, "PlayerData_Bool");
            public static readonly FlagDef nymmSpoken = new FlagDef("nymmSpoken", null, false, "PlayerData_Bool");
            public static readonly FlagDef oneWayArchive = new FlagDef("oneWayArchive", null, false, "PlayerData_Bool");
            public static readonly FlagDef openedBlackEggDoor = new FlagDef("openedBlackEggDoor", null, false, "PlayerData_Bool");
            public static readonly FlagDef openedBlackEggPath = new FlagDef("openedBlackEggPath", null, false, "PlayerData_Bool");
            public static readonly FlagDef openedCityGate = new FlagDef("openedCityGate", null, false, "PlayerData_Bool");
            public static readonly FlagDef openedGardensStagStation = new FlagDef("openedGardensStagStation", null, false, "PlayerData_Bool");
            public static readonly FlagDef openedHiddenStation = new FlagDef("openedHiddenStation", null, false, "PlayerData_Bool");
            public static readonly FlagDef openedLoveDoor = new FlagDef("openedLoveDoor", null, false, "PlayerData_Bool");
            public static readonly FlagDef openedMageDoor = new FlagDef("openedMageDoor", null, false, "PlayerData_Bool");
            public static readonly FlagDef openedMageDoor_v2 = new FlagDef("openedMageDoor_v2", null, false, "PlayerData_Bool");
            public static readonly FlagDef openedPalaceGrounds = new FlagDef("openedPalaceGrounds", null, false, "PlayerData_Bool");
            public static readonly FlagDef openedRestingGrounds02 = new FlagDef("openedRestingGrounds02", null, false, "PlayerData_Bool");
            public static readonly FlagDef openedRoyalGardens = new FlagDef("openedRoyalGardens", null, false, "PlayerData_Bool");
            public static readonly FlagDef openedRuins1 = new FlagDef("openedRuins1", null, false, "PlayerData_Bool");
            public static readonly FlagDef openedSlyShop = new FlagDef("openedSlyShop", null, false, "PlayerData_Bool");
            public static readonly FlagDef openedStagNest = new FlagDef("openedStagNest", null, false, "PlayerData_Bool");
            public static readonly FlagDef openedTown = new FlagDef("openedTown", null, false, "PlayerData_Bool");
            public static readonly FlagDef openedTramRestingGrounds = new FlagDef("openedTramRestingGrounds", null, false, "PlayerData_Bool");
            public static readonly FlagDef openedWaterwaysManhole = new FlagDef("openedWaterwaysManhole", null, false, "PlayerData_Bool");
            public static readonly FlagDef ordealAchieved = new FlagDef("ordealAchieved", null, false, "PlayerData_Bool");
            public static readonly FlagDef oroConvoMato = new FlagDef("oroConvoMato", null, false, "PlayerData_Bool");
            public static readonly FlagDef oroConvoSheo = new FlagDef("oroConvoSheo", null, false, "PlayerData_Bool");
            public static readonly FlagDef oroConvoSly = new FlagDef("oroConvoSly", null, false, "PlayerData_Bool");
            public static readonly FlagDef outskirtsWall = new FlagDef("outskirtsWall", null, false, "PlayerData_Bool");
            public static readonly FlagDef overcharmed = new FlagDef("overcharmed", null, false, "PlayerData_Bool");
            public static readonly FlagDef queenConvo_grimm1 = new FlagDef("queenConvo_grimm1", null, false, "PlayerData_Bool");
            public static readonly FlagDef queenConvo_grimm2 = new FlagDef("queenConvo_grimm2", null, false, "PlayerData_Bool");
            public static readonly FlagDef queenDung1 = new FlagDef("queenDung1", null, false, "PlayerData_Bool");
            public static readonly FlagDef queenDung2 = new FlagDef("queenDung2", null, false, "PlayerData_Bool");
            public static readonly FlagDef queenHornet = new FlagDef("queenHornet", null, false, "PlayerData_Bool");
            public static readonly FlagDef queenTalk1 = new FlagDef("queenTalk1", null, false, "PlayerData_Bool");
            public static readonly FlagDef queenTalk2 = new FlagDef("queenTalk2", null, false, "PlayerData_Bool");
            public static readonly FlagDef queenTalkExtra = new FlagDef("queenTalkExtra", null, false, "PlayerData_Bool");
            public static readonly FlagDef queuedGodfinderIcon = new FlagDef("queuedGodfinderIcon", null, false, "PlayerData_Bool");
            public static readonly FlagDef quirrelArchiveEncountered = new FlagDef("quirrelArchiveEncountered", null, false, "PlayerData_Bool");
            public static readonly FlagDef quirrelCityEncountered = new FlagDef("quirrelCityEncountered", null, false, "PlayerData_Bool");
            public static readonly FlagDef quirrelCityLeft = new FlagDef("quirrelCityLeft", null, false, "PlayerData_Bool");
            public static readonly FlagDef quirrelEpilogueCompleted = new FlagDef("quirrelEpilogueCompleted", null, false, "PlayerData_Bool");
            public static readonly FlagDef quirrelMantisEncountered = new FlagDef("quirrelMantisEncountered", null, false, "PlayerData_Bool");
            public static readonly FlagDef quirrelMines = new FlagDef("quirrelMines", null, false, "PlayerData_Int");
            public static readonly FlagDef quirrelMinesEncountered = new FlagDef("quirrelMinesEncountered", null, false, "PlayerData_Bool");
            public static readonly FlagDef quirrelMinesLeft = new FlagDef("quirrelMinesLeft", null, false, "PlayerData_Bool");
            public static readonly FlagDef quirrelRuins = new FlagDef("quirrelRuins", null, false, "PlayerData_Int");
            public static readonly FlagDef ranAway = new FlagDef("ranAway", null, false, "PlayerData_Bool");
            public static readonly FlagDef refusedLegEater = new FlagDef("refusedLegEater", null, false, "PlayerData_Bool");
            public static readonly FlagDef restingGroundsCryptWall = new FlagDef("restingGroundsCryptWall", null, false, "PlayerData_Bool");
            public static readonly FlagDef ruins1_5_tripleDoor = new FlagDef("ruins1_5_tripleDoor", null, false, "PlayerData_Bool");
            public static readonly FlagDef salubraConvoCombo = new FlagDef("salubraConvoCombo", null, false, "PlayerData_Bool");
            public static readonly FlagDef salubraConvoOvercharm = new FlagDef("salubraConvoOvercharm", null, false, "PlayerData_Bool");
            public static readonly FlagDef salubraConvoTruth = new FlagDef("salubraConvoTruth", null, false, "PlayerData_Bool");
            public static readonly FlagDef savedByHornet = new FlagDef("savedByHornet", null, false, "PlayerData_Bool");
            public static readonly FlagDef sawWoundedQuirrel = new FlagDef("sawWoundedQuirrel", null, false, "PlayerData_Bool");
            public static readonly FlagDef scaredFlukeHermitEncountered = new FlagDef("scaredFlukeHermitEncountered", null, false, "PlayerData_Bool");
            public static readonly FlagDef scaredFlukeHermitReturned = new FlagDef("scaredFlukeHermitReturned", null, false, "PlayerData_Bool");
            public static readonly FlagDef seenColosseumTitle = new FlagDef("seenColosseumTitle", null, false, "PlayerData_Bool");
            public static readonly FlagDef seenDoor4Finale = new FlagDef("seenDoor4Finale", null, false, "PlayerData_Bool");
            public static readonly FlagDef seenGGWastes = new FlagDef("seenGGWastes", null, false, "PlayerData_Bool");
            public static readonly FlagDef shadeFireballLevel = new FlagDef("shadeFireballLevel", null, false, "PlayerData_Int");
            public static readonly FlagDef shadeHealth = new FlagDef("shadeHealth", null, false, "PlayerData_Int");
            public static readonly FlagDef shadeMP = new FlagDef("shadeMP", null, false, "PlayerData_Int");
            public static readonly FlagDef shadeQuakeLevel = new FlagDef("shadeQuakeLevel", null, false, "PlayerData_Int");
            public static readonly FlagDef shadeScreamLevel = new FlagDef("shadeScreamLevel", null, false, "PlayerData_Int");
            public static readonly FlagDef shadeSpecialType = new FlagDef("shadeSpecialType", null, false, "PlayerData_Int");
            public static readonly FlagDef shamanFireball2Convo = new FlagDef("shamanFireball2Convo", null, false, "PlayerData_Bool");
            public static readonly FlagDef shamanQuake2Convo = new FlagDef("shamanQuake2Convo", null, false, "PlayerData_Bool");
            public static readonly FlagDef shamanQuakeConvo = new FlagDef("shamanQuakeConvo", null, false, "PlayerData_Bool");
            public static readonly FlagDef shamanScream2Convo = new FlagDef("shamanScream2Convo", null, false, "PlayerData_Bool");
            public static readonly FlagDef shamanScreamConvo = new FlagDef("shamanScreamConvo", null, false, "PlayerData_Bool");
            public static readonly FlagDef sheoConvoMato = new FlagDef("sheoConvoMato", null, false, "PlayerData_Bool");
            public static readonly FlagDef sheoConvoNailsmith = new FlagDef("sheoConvoNailsmith", null, false, "PlayerData_Bool");
            public static readonly FlagDef sheoConvoOro = new FlagDef("sheoConvoOro", null, false, "PlayerData_Bool");
            public static readonly FlagDef sheoConvoSly = new FlagDef("sheoConvoSly", null, false, "PlayerData_Bool");
            public static readonly FlagDef showGeoUI = new FlagDef("showGeoUI", null, false, "PlayerData_Bool");
            public static readonly FlagDef slugEncounterComplete = new FlagDef("slugEncounterComplete", null, false, "PlayerData_Bool");
            public static readonly FlagDef slyBeta = new FlagDef("slyBeta", null, false, "PlayerData_Bool");
            public static readonly FlagDef slyConvoGrimm = new FlagDef("slyConvoGrimm", null, false, "PlayerData_Bool");
            public static readonly FlagDef slyConvoMapper = new FlagDef("slyConvoMapper", null, false, "PlayerData_Bool");
            public static readonly FlagDef slyConvoNailArt = new FlagDef("slyConvoNailArt", null, false, "PlayerData_Bool");
            public static readonly FlagDef slyConvoNailHoned = new FlagDef("slyConvoNailHoned", null, false, "PlayerData_Bool");
            public static readonly FlagDef slyNotch1 = new FlagDef("slyNotch1", null, false, "PlayerData_Bool");
            public static readonly FlagDef slyNotch2 = new FlagDef("slyNotch2", null, false, "PlayerData_Bool");
            public static readonly FlagDef slyNymmConvo = new FlagDef("slyNymmConvo", null, false, "PlayerData_Bool");
            public static readonly FlagDef spaBugsEncountered = new FlagDef("spaBugsEncountered", null, false, "PlayerData_Bool");
            public static readonly FlagDef stagConvoTiso = new FlagDef("stagConvoTiso", null, false, "PlayerData_Bool");
            public static readonly FlagDef stagEggInspected = new FlagDef("stagEggInspected", null, false, "PlayerData_Bool");
            public static readonly FlagDef stagHopeConvo = new FlagDef("stagHopeConvo", null, false, "PlayerData_Bool");
            public static readonly FlagDef stagRemember2 = new FlagDef("stagRemember2", null, false, "PlayerData_Bool");
            public static readonly FlagDef stagRemember3 = new FlagDef("stagRemember3", null, false, "PlayerData_Bool");
            public static readonly FlagDef summonedMonomon = new FlagDef("summonedMonomon", null, false, "PlayerData_Bool");
            public static readonly FlagDef tisoDead = new FlagDef("tisoDead", null, false, "PlayerData_Bool");
            public static readonly FlagDef tisoEncounteredBench = new FlagDef("tisoEncounteredBench", null, false, "PlayerData_Bool");
            public static readonly FlagDef tisoEncounteredColosseum = new FlagDef("tisoEncounteredColosseum", null, false, "PlayerData_Bool");
            public static readonly FlagDef tisoEncounteredLake = new FlagDef("tisoEncounteredLake", null, false, "PlayerData_Bool");
            public static readonly FlagDef tisoShieldConvo = new FlagDef("tisoShieldConvo", null, false, "PlayerData_Bool");
            public static readonly FlagDef tollBenchAbyss = new FlagDef("tollBenchAbyss", null, false, "PlayerData_Bool");
            public static readonly FlagDef tollBenchCity = new FlagDef("tollBenchCity", null, false, "PlayerData_Bool");
            public static readonly FlagDef tollBenchQueensGardens = new FlagDef("tollBenchQueensGardens", null, false, "PlayerData_Bool");
            public static readonly FlagDef tramOpenedCrossroads = new FlagDef("tramOpenedCrossroads", null, false, "PlayerData_Bool");
            public static readonly FlagDef tramOpenedDeepnest = new FlagDef("tramOpenedDeepnest", null, false, "PlayerData_Bool");
            public static readonly FlagDef tramRestingGroundsPosition = new FlagDef("tramRestingGroundsPosition", null, false, "PlayerData_Int");
            public static readonly FlagDef tukDungEgg = new FlagDef("tukDungEgg", null, false, "PlayerData_Bool");
            public static readonly FlagDef tukEggPrice = new FlagDef("tukEggPrice", null, false, "PlayerData_Int");
            public static readonly FlagDef tukMet = new FlagDef("tukMet", null, false, "PlayerData_Bool");
            public static readonly FlagDef unchainedHollowKnight = new FlagDef("unchainedHollowKnight", null, false, "PlayerData_Bool");
            public static readonly FlagDef unlockedNewBossStatue = new FlagDef("unlockedNewBossStatue", null, false, "PlayerData_Bool");
            public static readonly FlagDef usedWhiteKey = new FlagDef("usedWhiteKey", null, false, "PlayerData_Bool");
            public static readonly FlagDef vesselFragStagNest = new FlagDef("vesselFragStagNest", null, false, "PlayerData_Bool");
            public static readonly FlagDef vesselFragmentMax = new FlagDef("vesselFragmentMax", null, false, "PlayerData_Bool");
            public static readonly FlagDef watcherChandelier = new FlagDef("watcherChandelier", null, false, "PlayerData_Bool");
            public static readonly FlagDef waterwaysAcidDrained = new FlagDef("waterwaysAcidDrained", null, false, "PlayerData_Bool");
            public static readonly FlagDef waterwaysGate = new FlagDef("waterwaysGate", null, false, "PlayerData_Bool");
            public static readonly FlagDef whiteDefenderDefeated = new FlagDef("whiteDefenderDefeated", null, false, "PlayerData_Bool");
            public static readonly FlagDef whiteDefenderDefeats = new FlagDef("whiteDefenderDefeats", null, false, "PlayerData_Int");
            public static readonly FlagDef whiteDefenderOrbsCollected = new FlagDef("whiteDefenderOrbsCollected", null, false, "PlayerData_Bool");
            public static readonly FlagDef whitePalace05_lever = new FlagDef("whitePalace05_lever", null, false, "PlayerData_Bool");
            public static readonly FlagDef whitePalaceMidWarp = new FlagDef("whitePalaceMidWarp", null, false, "PlayerData_Bool");
            public static readonly FlagDef whitePalaceOrb_1 = new FlagDef("whitePalaceOrb_1", null, false, "PlayerData_Bool");
            public static readonly FlagDef whitePalaceOrb_2 = new FlagDef("whitePalaceOrb_2", null, false, "PlayerData_Bool");
            public static readonly FlagDef whitePalaceOrb_3 = new FlagDef("whitePalaceOrb_3", null, false, "PlayerData_Bool");
            public static readonly FlagDef whitePalaceSecretRoomVisited = new FlagDef("whitePalaceSecretRoomVisited", null, false, "PlayerData_Bool");
            public static readonly FlagDef xunFailedConvo1 = new FlagDef("xunFailedConvo1", null, false, "PlayerData_Bool");
            public static readonly FlagDef xunFailedConvo2 = new FlagDef("xunFailedConvo2", null, false, "PlayerData_Bool");
            public static readonly FlagDef xunFlowerBrokeTimes = new FlagDef("xunFlowerBrokeTimes", null, false, "PlayerData_Int");
            public static readonly FlagDef xunFlowerGiven = new FlagDef("xunFlowerGiven", null, false, "PlayerData_Bool");
            public static readonly FlagDef xunRewardGiven = new FlagDef("xunRewardGiven", null, false, "PlayerData_Bool");
            public static readonly FlagDef zoteDead = new FlagDef("zoteDead", null, false, "PlayerData_Bool");
            public static readonly FlagDef zoteDeathPos = new FlagDef("zoteDeathPos", null, false, "PlayerData_Int");
            public static readonly FlagDef zoteDefeated = new FlagDef("zoteDefeated", null, false, "PlayerData_Bool");
            public static readonly FlagDef zoteLeftCity = new FlagDef("zoteLeftCity", null, false, "PlayerData_Bool");
            public static readonly FlagDef zotePrecept = new FlagDef("zotePrecept", null, false, "PlayerData_Int");
            public static readonly FlagDef zoteSpokenCity = new FlagDef("zoteSpokenCity", null, false, "PlayerData_Bool");
            public static readonly FlagDef zoteSpokenColosseum = new FlagDef("zoteSpokenColosseum", null, false, "PlayerData_Bool");
            public static readonly FlagDef zoteStatueWallBroken = new FlagDef("zoteStatueWallBroken", null, false, "PlayerData_Bool");
            public static readonly FlagDef zoteTrappedDeepnest = new FlagDef("zoteTrappedDeepnest", null, false, "PlayerData_Bool");

            public static readonly FlagDef bossDoorStateTier1 = new FlagDef("bossDoorStateTier1", null, false, "PlayerData_Completion");
            public static readonly FlagDef bossDoorStateTier2 = new FlagDef("bossDoorStateTier2", null, false, "PlayerData_Completion");
            public static readonly FlagDef bossDoorStateTier3 = new FlagDef("bossDoorStateTier3", null, false, "PlayerData_Completion");
            public static readonly FlagDef bossDoorStateTier4 = new FlagDef("bossDoorStateTier4", null, false, "PlayerData_Completion");
            public static readonly FlagDef bossDoorStateTier5 = new FlagDef("bossDoorStateTier5", null, false, "PlayerData_Completion");
            public static readonly FlagDef bossReturnEntryGate = new FlagDef("bossReturnEntryGate", null, false, "PlayerData_String");
            public static readonly FlagDef completionPercent = new FlagDef("completionPercent", null, false, "PlayerData_Single");
            public static readonly FlagDef completionPercentage = new FlagDef("completionPercentage", null, false, "PlayerData_Single", "Completion Percentage, updates at bench");
            public static readonly FlagDef currentBossSequence = new FlagDef("currentBossSequence", null, false, "PlayerData_BossSequenceData");
            public static readonly FlagDef currentBossStatueCompletionKey = new FlagDef("currentBossStatueCompletionKey", null, false, "PlayerData_String");
            public static readonly FlagDef dreamgateMapPos = new FlagDef("dreamgateMapPos", null, false, "PlayerData_Vector3");
            public static readonly FlagDef dreamGateScene = new FlagDef("dreamGateScene", null, false, "PlayerData_String");
            public static readonly FlagDef dreamGateX = new FlagDef("dreamGateX", null, false, "PlayerData_Single");
            public static readonly FlagDef dreamGateY = new FlagDef("dreamGateY", null, false, "PlayerData_Single");
            public static readonly FlagDef dreamReturnScene = new FlagDef("dreamReturnScene", null, false, "PlayerData_String");
            public static readonly FlagDef equippedCharms = new FlagDef("equippedCharms", null, false, "PlayerData_List`1");
            public static readonly FlagDef placedMarkers_b = new FlagDef("placedMarkers_b", null, false, "PlayerData_List`1");
            public static readonly FlagDef placedMarkers_r = new FlagDef("placedMarkers_r", null, false, "PlayerData_List`1");
            public static readonly FlagDef placedMarkers_w = new FlagDef("placedMarkers_w", null, false, "PlayerData_List`1");
            public static readonly FlagDef placedMarkers_y = new FlagDef("placedMarkers_y", null, false, "PlayerData_List`1");
            public static readonly FlagDef playerStory = new FlagDef("playerStory", null, false, "PlayerData_List`1");
            public static readonly FlagDef playerStoryOutput = new FlagDef("playerStoryOutput", null, false, "PlayerData_String");
            public static readonly FlagDef scenesEncounteredBench = new FlagDef("scenesEncounteredBench", null, false, "PlayerData_List`1");
            public static readonly FlagDef scenesEncounteredCocoon = new FlagDef("scenesEncounteredCocoon", null, false, "PlayerData_List`1");
            public static readonly FlagDef scenesEncounteredDreamPlant = new FlagDef("scenesEncounteredDreamPlant", null, false, "PlayerData_List`1");
            public static readonly FlagDef scenesEncounteredDreamPlantC = new FlagDef("scenesEncounteredDreamPlantC", null, false, "PlayerData_List`1");
            public static readonly FlagDef scenesFlameCollected = new FlagDef("scenesFlameCollected", null, false, "PlayerData_List`1");
            public static readonly FlagDef scenesMapped = new FlagDef("scenesMapped", null, false, "PlayerData_List`1");
            public static readonly FlagDef scenesVisited = new FlagDef("scenesVisited", null, false, "PlayerData_List`1");
            public static readonly FlagDef shadeMapPos = new FlagDef("shadeMapPos", null, false, "PlayerData_Vector3");
            public static readonly FlagDef shadeMapZone = new FlagDef("shadeMapZone", null, false, "PlayerData_String");
            public static readonly FlagDef shadePositionX = new FlagDef("shadePositionX", null, false, "PlayerData_Single");
            public static readonly FlagDef shadePositionY = new FlagDef("shadePositionY", null, false, "PlayerData_Single");
            public static readonly FlagDef shadeScene = new FlagDef("shadeScene", null, false, "PlayerData_String");
            public static readonly FlagDef statueStateBrokenVessel = new FlagDef("statueStateBrokenVessel", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateBroodingMawlek = new FlagDef("statueStateBroodingMawlek", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateCollector = new FlagDef("statueStateCollector", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateCrystalGuardian1 = new FlagDef("statueStateCrystalGuardian1", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateCrystalGuardian2 = new FlagDef("statueStateCrystalGuardian2", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateDungDefender = new FlagDef("statueStateDungDefender", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateElderHu = new FlagDef("statueStateElderHu", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateFailedChampion = new FlagDef("statueStateFailedChampion", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateFalseKnight = new FlagDef("statueStateFalseKnight", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateFlukemarm = new FlagDef("statueStateFlukemarm", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateGalien = new FlagDef("statueStateGalien", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateGodTamer = new FlagDef("statueStateGodTamer", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateGorb = new FlagDef("statueStateGorb", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateGreyPrince = new FlagDef("statueStateGreyPrince", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateGrimm = new FlagDef("statueStateGrimm", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateGruzMother = new FlagDef("statueStateGruzMother", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateHiveKnight = new FlagDef("statueStateHiveKnight", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateHollowKnight = new FlagDef("statueStateHollowKnight", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateHornet1 = new FlagDef("statueStateHornet1", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateHornet2 = new FlagDef("statueStateHornet2", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateLostKin = new FlagDef("statueStateLostKin", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateMageKnight = new FlagDef("statueStateMageKnight", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateMantisLords = new FlagDef("statueStateMantisLords", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateMantisLordsExtra = new FlagDef("statueStateMantisLordsExtra", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateMarkoth = new FlagDef("statueStateMarkoth", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateMarmu = new FlagDef("statueStateMarmu", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateMegaMossCharger = new FlagDef("statueStateMegaMossCharger", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateNailmasters = new FlagDef("statueStateNailmasters", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateNightmareGrimm = new FlagDef("statueStateNightmareGrimm", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateNoEyes = new FlagDef("statueStateNoEyes", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateNosk = new FlagDef("statueStateNosk", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateNoskHornet = new FlagDef("statueStateNoskHornet", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateOblobbles = new FlagDef("statueStateOblobbles", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStatePaintmaster = new FlagDef("statueStatePaintmaster", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateRadiance = new FlagDef("statueStateRadiance", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateSly = new FlagDef("statueStateSly", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateSoulMaster = new FlagDef("statueStateSoulMaster", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateSoulTyrant = new FlagDef("statueStateSoulTyrant", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateTraitorLord = new FlagDef("statueStateTraitorLord", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateUumuu = new FlagDef("statueStateUumuu", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateVengefly = new FlagDef("statueStateVengefly", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateWatcherKnights = new FlagDef("statueStateWatcherKnights", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateWhiteDefender = new FlagDef("statueStateWhiteDefender", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateXero = new FlagDef("statueStateXero", null, false, "PlayerData_Completion");
            public static readonly FlagDef statueStateZote = new FlagDef("statueStateZote", null, false, "PlayerData_Completion");
            public static readonly FlagDef unlockedBossScenes = new FlagDef("unlockedBossScenes", null, false, "PlayerData_List`1");
            public static readonly FlagDef version = new FlagDef("version", null, false, "PlayerData_String");
        #endregion

        /// <summary>
        /// Centralized list of unused flags that should be ignored during monitoring.
        /// These flags are defined but not actively used by the mod, so they should not
        /// trigger notifications when their values change.
        /// </summary>
        public static readonly FlagDef[] UnusedFlags = new FlagDef[]
        {
            #region Player Flags
                new FlagDef("atBench", null, false, "PlayerData_Bool", "Currently sitting at a bench"),
                new FlagDef("charmBenchMsg", null, false, "PlayerData_Bool", "Got Charm message from resting at bench after getting a charm"),
                new FlagDef("currentInvPane", null, false, "PlayerData_Int", "Inventory Pane Selected"),
                new FlagDef("damagedBlue", null, false, "PlayerData_Bool", "Last damage took a blue health orb"),
                new FlagDef("disablePause", null, false, "PlayerData_Bool", "Pause is disabled"),
                new FlagDef("dreamNailConvo", null, false, "PlayerData_Bool"),
                new FlagDef("dreamerScene1", null, false, "PlayerData_Bool", "Dreamer scene 1 seen"),
                new FlagDef("elderbugFirstCall", null, false, "PlayerData_Bool", "Elderbug noticed Knight"),
                new FlagDef("enteredTutorialFirstTime", null, false, "PlayerData_Bool", "Entered game start"),
                new FlagDef("environmentType", null, false, "PlayerData_Int", "Environment Type: 0 = ground, 4 = platform"),
                new FlagDef("falseKnightFirstPlop", null, false, "PlayerData_Bool", "False Knight fell first time"),
                new FlagDef("firstGeo", null, false, "PlayerData_Bool", "Found first Geo"),
                new FlagDef("gMap_doorMapZone", null, false, "PlayerData_String"),
                new FlagDef("gMap_doorOriginOffsetX", null, false, "PlayerData_Single"),
                new FlagDef("gMap_doorOriginOffsetY", null, false, "PlayerData_Single"),
                new FlagDef("gMap_doorScene", null, false, "PlayerData_String"),
                new FlagDef("gMap_doorSceneHeight", null, false, "PlayerData_Single"),
                new FlagDef("gMap_doorSceneWidth", null, false, "PlayerData_Single"),
                new FlagDef("gMap_doorX", null, false, "PlayerData_Single"),
                new FlagDef("gMap_doorY", null, false, "PlayerData_Single"),
                new FlagDef("hazardRespawnFacingRight", null, false, "PlayerData_Bool"),
                new FlagDef("hazardRespawnLocation", null, false, "PlayerData_Vector3"),
                new FlagDef("kingsStationNonDisplay", null, false, "PlayerData_Bool"),
                new FlagDef("lastJournalItem", null, false, "PlayerData_Int", "Last Journey Entry looked at"),
                new FlagDef("mapZone", null, false, "PlayerData_MapZone"),
                new FlagDef("megaMossChargerEncountered", null, false, "PlayerData_Bool", "Massive Moss Charger encountered"),
                new FlagDef("MPReserve", null, false, "PlayerData_Int", "Soul in reserves"),
                new FlagDef("newCharm_1", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_10", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_11", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_12", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_13", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_14", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_15", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_16", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_17", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_18", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_19", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_2", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_20", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_21", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_22", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_23", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_24", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_25", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_26", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_27", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_28", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_29", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_3", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_30", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_31", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_32", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_33", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_34", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_35", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_36", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_37", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_38", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_39", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_4", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_40", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_5", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_6", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_7", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_8", null, false, "PlayerData_Bool"),
                new FlagDef("newCharm_9", null, false, "PlayerData_Bool"),
                new FlagDef("nextScene", null, false, "PlayerData_String"),
                new FlagDef("openingCreditsPlayed", null, false, "PlayerData_Bool", "Opening Credits Played on Game Start"),
                new FlagDef("previousDarkness", null, false, "PlayerData_Int"),
                new FlagDef("prevHealth", null, false, "PlayerData_Int"),
                new FlagDef("promptFocus", null, false, "PlayerData_Bool", "Took damage after seeing Focus Tablet"),
                new FlagDef("respawnFacingRight", null, false, "PlayerData_Bool"),
                new FlagDef("respawnMarkerName", null, false, "PlayerData_String"),
                new FlagDef("respawnScene", null, false, "PlayerData_String"),
                new FlagDef("respawnType", null, false, "PlayerData_Int"),
                new FlagDef("seenDreamNailPrompt", null, false, "PlayerData_Bool"),
                new FlagDef("seenFocusTablet", null, false, "PlayerData_Bool", "Passed by Focus Tablet"),
                new FlagDef("seenJournalMsg", null, false, "PlayerData_Bool", "Seen Journal message for new enemy"),
                new FlagDef("showHealthUI", null, false, "PlayerData_Bool", "Show Health UI"),
                new FlagDef("seenHunterMsg", null, false, "PlayerData_Bool"),
                new FlagDef("stagPosition", null, false, "PlayerData_Int", "Location of Stag"),
                new FlagDef("tramLowerPosition", null, false, "PlayerData_Int", "Lower tram's current position"),
                new FlagDef("travelling", null, false, "PlayerData_Bool", "Riding the Stag"),
            #endregion

            #region Scene Flags
                #region Abyss
                    // Abyss_01
                    new FlagDef("Inverse Remasker", SceneInstances.Abyss_01, false, "PersistentBoolData"),
                    new FlagDef("Ruins Flying Sentry", SceneInstances.Abyss_01, false, "PersistentBoolData"),
                    new FlagDef("Ruins Flying Sentry (1)", SceneInstances.Abyss_01, false, "PersistentBoolData"),
                    new FlagDef("Secret Sound Region", SceneInstances.Abyss_01, false, "PersistentBoolData"),

                    // Abyss_02
                    new FlagDef("Ruins Flying Sentry", SceneInstances.Abyss_02, false, "PersistentBoolData"),
                    new FlagDef("Ruins Flying Sentry (1)", SceneInstances.Abyss_02, false, "PersistentBoolData"),
                    new FlagDef("Ruins Flying Sentry Javelin", SceneInstances.Abyss_02, false, "PersistentBoolData"),
                    new FlagDef("Ruins Flying Sentry Javelin (1)", SceneInstances.Abyss_02, false, "PersistentBoolData"),
                    new FlagDef("Ruins Sentry 1", SceneInstances.Abyss_02, false, "PersistentBoolData"),
                    new FlagDef("Zombie Barger", SceneInstances.Abyss_02, false, "PersistentBoolData"),
                    new FlagDef("Zombie Barger (1)", SceneInstances.Abyss_02, false, "PersistentBoolData"),
                    new FlagDef("Zombie Hornhead", SceneInstances.Abyss_02, false, "PersistentBoolData"),
                    new FlagDef("Flamebearer Spawn", SceneInstances.Abyss_02, false, "PersistentBoolData"),

                    // Abyss_04
                    new FlagDef("Soul Totem mini_horned", SceneInstances.Abyss_04, false, "PersistentIntData"),
                    new FlagDef("wish inverse remask", SceneInstances.Abyss_04, false, "PersistentBoolData"),
                    new FlagDef("wish_remask", SceneInstances.Abyss_04, false, "PersistentBoolData"),
                    new FlagDef("wish_secret sound", SceneInstances.Abyss_04, false, "PersistentBoolData"),

                    // Abyss_06
                    new FlagDef("Remasker", SceneInstances.Abyss_06_Core, false, "PersistentBoolData"),
                    new FlagDef("Remasker (1)", SceneInstances.Abyss_06_Core, false, "PersistentBoolData"),

                    // Abyss_17
                    new FlagDef("Ceiling Dropper", SceneInstances.Abyss_17, false, "PersistentBoolData"),
                    new FlagDef("Ceiling Dropper (1)", SceneInstances.Abyss_17, false, "PersistentBoolData"),
                    new FlagDef("Ceiling Dropper (2)", SceneInstances.Abyss_17, false, "PersistentBoolData"),
                    new FlagDef("Ceiling Dropper (3)", SceneInstances.Abyss_17, false, "PersistentBoolData"),
                    new FlagDef("Lesser Mawlek", SceneInstances.Abyss_17, false, "PersistentBoolData"),
                    new FlagDef("Lesser Mawlek (1)", SceneInstances.Abyss_17, false, "PersistentBoolData"),
                    new FlagDef("Lesser Mawlek 1", SceneInstances.Abyss_17, false, "PersistentBoolData"),
                    new FlagDef("Lesser Mawlek (2)", SceneInstances.Abyss_17, false, "PersistentBoolData"),
                    new FlagDef("Lesser Mawlek 2", SceneInstances.Abyss_17, false, "PersistentBoolData"),
                    new FlagDef("Lesser Mawlek (3)", SceneInstances.Abyss_17, false, "PersistentBoolData"),
                    new FlagDef("Lesser Mawlek (4)", SceneInstances.Abyss_17, false, "PersistentBoolData"),
                    new FlagDef("Lesser Mawlek (8)", SceneInstances.Abyss_17, false, "PersistentBoolData"),
                    new FlagDef("Quake Floor", SceneInstances.Abyss_17, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Abyss_17, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask (1)", SceneInstances.Abyss_17, false, "PersistentBoolData"),
                #endregion

                #region Cliffs
                    // Cliffs_01
                    new FlagDef("Soul Totem mini_two_horned", SceneInstances.Cliffs_01, false, "PersistentIntData"),

                    // Cliffs_02
                    new FlagDef("Inverse Remasker", SceneInstances.Cliffs_02, false, "PersistentBoolData"),
                    new FlagDef("Remasker", SceneInstances.Cliffs_02, false, "PersistentBoolData"),
                    new FlagDef("Remasker (1)", SceneInstances.Cliffs_02, false, "PersistentBoolData"),
                    new FlagDef("Remasker (2)", SceneInstances.Cliffs_02, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask (2)", SceneInstances.Cliffs_02, false, "PersistentBoolData"),
                    new FlagDef("Soul Totem 5", SceneInstances.Cliffs_02, false, "PersistentIntData"),
                    new FlagDef("Zombie Barger", SceneInstances.Cliffs_02, false, "PersistentBoolData"),
                    new FlagDef("Zombie Hornhead", SceneInstances.Cliffs_02, false, "PersistentBoolData"),

                    // Cliffs_04
                    new FlagDef("Quake Floor", SceneInstances.Cliffs_04, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Cliffs_04, false, "PersistentBoolData"),
                    new FlagDef("Soul Totem mini_horned", SceneInstances.Cliffs_04, false, "PersistentIntData"),
                    new FlagDef("Zombie Barger", SceneInstances.Cliffs_04, false, "PersistentBoolData"),
                    new FlagDef("Zombie Hornhead", SceneInstances.Cliffs_04, false, "PersistentBoolData"),
                    new FlagDef("Zombie Leaper", SceneInstances.Cliffs_04, false, "PersistentBoolData"),
                    new FlagDef("Zombie Leaper (1)", SceneInstances.Cliffs_04, false, "PersistentBoolData"),
                    new FlagDef("Zombie Leaper (2)", SceneInstances.Cliffs_04, false, "PersistentBoolData"),

                    // Cliffs_05
                    new FlagDef("Ghost Activator", SceneInstances.Cliffs_05, false, "PersistentBoolData"),
                    new FlagDef("Remasker", SceneInstances.Cliffs_05, false, "PersistentBoolData"),
                    new FlagDef("Shiny Item Stand", SceneInstances.Cliffs_05, false, "PersistentBoolData"),

                    // Cliffs_06
                    new FlagDef("Inverse Remasker", SceneInstances.Cliffs_06, false, "PersistentBoolData"),
                    new FlagDef("Secret Sound Region", SceneInstances.Cliffs_06, false, "PersistentBoolData"),
                #endregion

                #region Crossroads
                    // Crossroads_01
                    new FlagDef("Secret Mask", SceneInstances.Crossroads_01, false, "PersistentBoolData"),
                    new FlagDef("Zombie Runner", SceneInstances.Crossroads_01, false, "PersistentBoolData"),
                    new FlagDef("Zombie Runner 1", SceneInstances.Crossroads_01, false, "PersistentBoolData"),

                    // Crossroads_04
                    new FlagDef("CamLock Destroyer", SceneInstances.Crossroads_04, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Crossroads_04, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask (1)", SceneInstances.Crossroads_04, false, "PersistentBoolData"),
                    new FlagDef("Zombie Barger", SceneInstances.Crossroads_04, false, "PersistentBoolData"),
                    new FlagDef("Zombie Hornhead", SceneInstances.Crossroads_04, false, "PersistentBoolData"),
                    new FlagDef("Zombie Runner", SceneInstances.Crossroads_04, false, "PersistentBoolData"),

                    // Crossroads_05
                    new FlagDef("Zombie Runner", SceneInstances.Crossroads_05, false, "PersistentBoolData"),

                    // Crossroads_06
                    new FlagDef("Raising Pillar", SceneInstances.Crossroads_06, false, "PersistentBoolData", "Opened passage back to West Crossroads"),

                    // Crossroads_08
                    new FlagDef("break_wall_masks", SceneInstances.Crossroads_08, false, "PersistentBoolData"),

                    // Crossroads_09
                    new FlagDef("Battle Scene", SceneInstances.Crossroads_09, false, "PersistentBoolData", "Defeated Brooding Mawlek Scene"),

                    // Crossroads_10
                    new FlagDef("Battle Scene", SceneInstances.Crossroads_10, false, "PersistentBoolData", "Defeated False Knight"),
                    new FlagDef("Shiny Item", SceneInstances.Crossroads_10, false, "PersistentBoolData", "Got City Key"),
                    new FlagDef("Zombie Barger", SceneInstances.Crossroads_10, false, "PersistentBoolData"),
                    new FlagDef("Zombie Hornhead", SceneInstances.Crossroads_10, false, "PersistentBoolData"),
                    new FlagDef("Zombie Runner", SceneInstances.Crossroads_10, false, "PersistentBoolData"),

                    // Crossroads_11_alt
                    new FlagDef("Battle Scene", SceneInstances.Crossroads_11_alt, false, "PersistentBoolData"),

                    // Crossroads_13
                    new FlagDef("Zombie Barger", SceneInstances.Crossroads_13, false, "PersistentBoolData"),

                    // Crossroads_15
                    new FlagDef("Zombie Shield", SceneInstances.Crossroads_15, false, "PersistentBoolData"),
                    new FlagDef("Zombie Shield 1", SceneInstances.Crossroads_15, false, "PersistentBoolData"),

                    // Crossroads_16
                    new FlagDef("Zombie Hornhead", SceneInstances.Crossroads_16, false, "PersistentBoolData"),

                    // Crossroads_18
                    new FlagDef("Soul Totem mini_horned", SceneInstances.Crossroads_18, true, "PersistentIntData"),

                    // Crossroads_19
                    new FlagDef("Hatcher", SceneInstances.Crossroads_19, true, "PersistentBoolData"),
                    new FlagDef("Zombie Leaper", SceneInstances.Crossroads_19, true, "PersistentBoolData", "Killed Zombie Leaper"),
                    new FlagDef("Soul Totem mini_two_horned", SceneInstances.Crossroads_19, false, "PersistentIntData"),

                    // Crossroads_21
                    new FlagDef("Secret Mask (1)", SceneInstances.Crossroads_21, false, "PersistentBoolData"),
                    new FlagDef("Zombie Barger (1)", SceneInstances.Crossroads_21, false, "PersistentBoolData"),

                    // Crossroads_22
                    new FlagDef("Hatcher", SceneInstances.Crossroads_22, false, "PersistentBoolData"),
                    new FlagDef("Shiny Item", SceneInstances.Crossroads_22, false, "PersistentBoolData"),

                    // Crossroads_25
                    new FlagDef("Soul Totem mini_two_horned", SceneInstances.Crossroads_25, false, "PersistentIntData"),

                    // Crossroads_27
                    new FlagDef("Hatcher", SceneInstances.Crossroads_27, false, "PersistentBoolData"),
                    new FlagDef("Hatcher 1", SceneInstances.Crossroads_27, false, "PersistentBoolData"),
                    new FlagDef("Hatcher 2", SceneInstances.Crossroads_27, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Crossroads_21, false, "PersistentBoolData"),
                    new FlagDef("Zombie Barger", SceneInstances.Crossroads_21, false, "PersistentBoolData"),
                    new FlagDef("Zombie Guard", SceneInstances.Crossroads_21, false, "PersistentBoolData"),
                    new FlagDef("Zombie Leaper", SceneInstances.Crossroads_21, false, "PersistentBoolData"),
                    new FlagDef("Zombie Runner", SceneInstances.Crossroads_21, false, "PersistentBoolData"),

                    // Crossroads_35
                    new FlagDef("Hatcher", SceneInstances.Crossroads_35, true, "PersistentBoolData", "Killed Hatcher"),
                    new FlagDef("Remasker", SceneInstances.Crossroads_35, false, "PersistentBoolData"),
                    new FlagDef("Soul Totem mini_horned", SceneInstances.Crossroads_35, true, "PersistentIntData"),

                    // Crossroads_36
                    new FlagDef("Force Hard Landing", SceneInstances.Crossroads_36, false, "PersistentBoolData", "Took hard landing after collapse floor"),
                    new FlagDef("Mask Bottom", SceneInstances.Crossroads_36, false, "PersistentBoolData", "Remove darkness from the lower area"),
                    new FlagDef("Reminder Look Down", SceneInstances.Crossroads_36, false, "PersistentBoolData", "Reminded to Look Down"),
                    new FlagDef("Secret Mask", SceneInstances.Crossroads_36, false, "PersistentBoolData", "Remove darkness to backtrack to entrance"),
                    new FlagDef("Soul Totem 4", SceneInstances.Crossroads_36, false, "PersistentIntData"),

                    // Crossroads_39
                    new FlagDef("Zombie Hornhead 1", SceneInstances.Crossroads_39, false, "PersistentBoolData"),
                    new FlagDef("Zombie Runner", SceneInstances.Crossroads_39, false, "PersistentBoolData"),
                    new FlagDef("Zombie Runner 1", SceneInstances.Crossroads_39, false, "PersistentBoolData"),

                    // Crossroads_40
                    new FlagDef("Zombie Hornhead", SceneInstances.Crossroads_40, false, "PersistentBoolData"),
                    new FlagDef("Zombie Leaper", SceneInstances.Crossroads_40, false, "PersistentBoolData"),
                    new FlagDef("Zombie Leaper 1", SceneInstances.Crossroads_40, false, "PersistentBoolData"),
                    new FlagDef("Zombie Runner 2", SceneInstances.Crossroads_40, false, "PersistentBoolData"),

                    // Crossroads_45
                    new FlagDef("Soul Totem 5", SceneInstances.Crossroads_45, false, "PersistentIntData"),

                    // Crossroads_48
                    new FlagDef("Zombie Guard", SceneInstances.Crossroads_48, false, "PersistentBoolData"),

                    // Crossroads_55
                    new FlagDef("Quake Floor", SceneInstances.Crossroads_52, false, "PersistentBoolData"),
                    new FlagDef("Remasker", SceneInstances.Crossroads_52, false, "PersistentBoolData"),
                    new FlagDef("Remasker (1)", SceneInstances.Crossroads_52, false, "PersistentBoolData"),

                    // Crossroads_ShamanTemple
                    new FlagDef("Soul Totem 2", SceneInstances.Crossroads_ShamanTemple, false, "PersistentIntData"),
                    new FlagDef("Reminder Cast (1)", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData"),
                    new FlagDef("Death Respawn Trigger 1", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData"),
                    new FlagDef("Health Cocoon", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData"),
                    new FlagDef("Remasker", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData"),
                    new FlagDef("Zombie Runner", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData"),
                    new FlagDef("Shiny Item", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData", "Got Soul Catcher Charm"),
                    new FlagDef("Battle Scene", SceneInstances.Crossroads_ShamanTemple, false, "PersistentBoolData"),
                #endregion

                #region Deepnest            
                    // Deepnest_01
                    new FlagDef("Secret Mask", SceneInstances.Deepnest_01, false, "PersistentBoolData"),

                    // Deepnest_01b
                    new FlagDef("Inverse Remasker", SceneInstances.Deepnest_01b, false, "PersistentBoolData"),
                    new FlagDef("Inverse Remasker (1)", SceneInstances.Deepnest_01b, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Deepnest_01b, false, "PersistentBoolData"),

                    // Deepnest_02
                    new FlagDef("Secret Mask", SceneInstances.Deepnest_02, false, "PersistentBoolData"),

                    // Deepnest_03
                    new FlagDef("Secret Mask (1)", SceneInstances.Deepnest_03, false, "PersistentBoolData"),
                    new FlagDef("Zombie Hornhead Sp", SceneInstances.Deepnest_03, false, "PersistentBoolData"),
                    new FlagDef("Zombie Hornhead Sp (2)", SceneInstances.Deepnest_03, false, "PersistentBoolData"),
                    new FlagDef("Zombie Hornhead Sp (3)", SceneInstances.Deepnest_03, false, "PersistentBoolData"),
                    new FlagDef("Zombie Hornhead Sp (4)", SceneInstances.Deepnest_03, false, "PersistentBoolData"),
                    new FlagDef("Zombie Runner Sp", SceneInstances.Deepnest_03, false, "PersistentBoolData"),
                    new FlagDef("Zombie Runner Sp (2)", SceneInstances.Deepnest_03, false, "PersistentBoolData"),
                    new FlagDef("Zombie Runner Sp (5)", SceneInstances.Deepnest_03, false, "PersistentBoolData"),

                    // Deepnest_10
                    new FlagDef("Soul Totem 1", SceneInstances.Deepnest_10, false, "PersistentIntData"),

                    // Deepnest_14
                    new FlagDef("Inverse Remasker", SceneInstances.Deepnest_14, false, "PersistentBoolData"),
                    new FlagDef("Inverse Remasker (1)", SceneInstances.Deepnest_14, false, "PersistentBoolData"),

                    // Deepnest_16
                    new FlagDef("Inverse Remasker", SceneInstances.Deepnest_16, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Deepnest_16, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask (1)", SceneInstances.Deepnest_16, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask (3)", SceneInstances.Deepnest_16, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask bot left", SceneInstances.Deepnest_16, false, "PersistentBoolData"),

                    // Deepnest_26
                    new FlagDef("Centipede Hatcher", SceneInstances.Deepnest_26, false, "PersistentBoolData"),
                    new FlagDef("Centipede Hatcher (2)", SceneInstances.Deepnest_26, false, "PersistentBoolData"),
                    new FlagDef("Health Cocoon", SceneInstances.Deepnest_26, false, "PersistentBoolData"),
                    new FlagDef("Inverse Remasker", SceneInstances.Deepnest_26, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask (1)", SceneInstances.Deepnest_26, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask (2)", SceneInstances.Deepnest_26, false, "PersistentBoolData"),
                    new FlagDef("Secret Sound Region", SceneInstances.Deepnest_26, false, "PersistentBoolData"),

                    // Deepnest_26b
                    new FlagDef("Centipede Hatcher (4)", SceneInstances.Deepnest_26b, false, "PersistentBoolData"),
                    new FlagDef("Centipede Hatcher (5)", SceneInstances.Deepnest_26b, false, "PersistentBoolData"),
                    new FlagDef("Centipede Hatcher (7)", SceneInstances.Deepnest_26b, false, "PersistentBoolData"),
                    new FlagDef("Centipede Hatcher (9)", SceneInstances.Deepnest_26b, false, "PersistentBoolData"),
                    new FlagDef("mask tram left front", SceneInstances.Deepnest_26b, false, "PersistentBoolData"),
                    new FlagDef("tram_inverse mask", SceneInstances.Deepnest_26b, false, "PersistentBoolData"),

                    // Deepnest_30
                    new FlagDef("Mimic Spider Fake4", SceneInstances.Deepnest_30, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Deepnest_30, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask (1)", SceneInstances.Deepnest_30, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask (3)", SceneInstances.Deepnest_30, false, "PersistentBoolData"),

                    // Deepnest_33
                    new FlagDef("Secret Mask", SceneInstances.Deepnest_33, false, "PersistentBoolData"),
                    new FlagDef("Zombie Hornhead Sp", SceneInstances.Deepnest_33, false, "PersistentBoolData"),
                    new FlagDef("Zombie Hornhead Sp (1)", SceneInstances.Deepnest_33, false, "PersistentBoolData"),
                    new FlagDef("Zombie Hornhead Sp (2)", SceneInstances.Deepnest_33, false, "PersistentBoolData"),
                    new FlagDef("Zombie Runner Sp (1)", SceneInstances.Deepnest_33, false, "PersistentBoolData"),

                    // Deepnest_34
                    new FlagDef("Secret Mask", SceneInstances.Deepnest_34, false, "PersistentBoolData"),
                    new FlagDef("Slash Spider", SceneInstances.Deepnest_34, false, "PersistentBoolData"),
                    new FlagDef("Zombie Hornhead Sp", SceneInstances.Deepnest_34, false, "PersistentBoolData"),
                    new FlagDef("Zombie Runner Sp", SceneInstances.Deepnest_34, false, "PersistentBoolData"),
                    new FlagDef("Zombie Runner Sp (1)", SceneInstances.Deepnest_34, false, "PersistentBoolData"),

                    // Deepnest_35
                    new FlagDef("Zombie Hornhead Sp", SceneInstances.Deepnest_35, false, "PersistentBoolData"),
                    new FlagDef("Zombie Hornhead Sp (2)", SceneInstances.Deepnest_35, false, "PersistentBoolData"),
                    new FlagDef("Zombie Runner Sp", SceneInstances.Deepnest_35, false, "PersistentBoolData"),

                    // Deepnest_36
                    new FlagDef("Grub Mimic 1", SceneInstances.Deepnest_36, false, "PersistentBoolData"),
                    new FlagDef("Grub Mimic 2", SceneInstances.Deepnest_36, false, "PersistentBoolData"),
                    new FlagDef("Grub Mimic 3", SceneInstances.Deepnest_36, false, "PersistentBoolData"),
                    new FlagDef("Grub Mimic Bottle", SceneInstances.Deepnest_36, false, "PersistentBoolData"),
                    new FlagDef("Grub Mimic Bottle (1)", SceneInstances.Deepnest_36, false, "PersistentBoolData"),
                    new FlagDef("Grub Mimic Bottle (2)", SceneInstances.Deepnest_36, false, "PersistentBoolData"),

                    // Deepnest_38
                    new FlagDef("Secret Mask", SceneInstances.Deepnest_38, false, "PersistentBoolData"),
                    new FlagDef("Soul Totem 4", SceneInstances.Deepnest_38, false, "PersistentIntData"),

                    // Deepnest_39
                    new FlagDef("Remasker", SceneInstances.Deepnest_39, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Deepnest_39, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask (1)", SceneInstances.Deepnest_39, false, "PersistentBoolData"),
                    new FlagDef("Slash Spider", SceneInstances.Deepnest_39, false, "PersistentBoolData"),
                    new FlagDef("Slash Spider (2)", SceneInstances.Deepnest_39, false, "PersistentBoolData"),
                    new FlagDef("Slash Spider (3)", SceneInstances.Deepnest_39, false, "PersistentBoolData"),
                    new FlagDef("Slash Spider (4)", SceneInstances.Deepnest_39, false, "PersistentBoolData"),

                    // Deepnest_40
                    new FlagDef("Health Cocoon", SceneInstances.Deepnest_40, false, "PersistentBoolData"),

                    // Deepnest_41
                    new FlagDef("Secret Mask (2)", SceneInstances.Deepnest_41, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask temp", SceneInstances.Deepnest_41, false, "PersistentBoolData"),
                    new FlagDef("Slash Spider", SceneInstances.Deepnest_41, false, "PersistentBoolData"),

                    // Deepnest_42
                    new FlagDef("Secret Mask", SceneInstances.Deepnest_42, false, "PersistentBoolData"),
                    new FlagDef("Soul Totem mini_two_horned", SceneInstances.Deepnest_42, false, "PersistentIntData"),

                    // Deepnest_44
                    new FlagDef("Inverse Remasker", SceneInstances.Deepnest_44, false, "PersistentBoolData"),
                    new FlagDef("Remasker", SceneInstances.Deepnest_44, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Deepnest_44, false, "PersistentBoolData"),
                    new FlagDef("Secret Sound Region", SceneInstances.Deepnest_44, false, "PersistentBoolData"),

                    // Deepnest_Spider_Town
                    new FlagDef("hack jump secret remask", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData"),
                    new FlagDef("Inverse Remasker", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData"),
                    new FlagDef("Inverse Remasker (1)", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData"),
                    new FlagDef("Inverse Remasker (2)", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData"),
                    new FlagDef("remask_store_room", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData"),
                    new FlagDef("Remasker", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData"),
                    new FlagDef("Remasker (1)", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData"),
                    new FlagDef("Remasker (2)", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData"),
                    new FlagDef("Remasker (3)", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData"),
                    new FlagDef("Remasker (4)", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData"),
                    new FlagDef("Remasker bar", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData"),
                    new FlagDef("Secret Sound Region", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData"),
                    new FlagDef("Slash Spider", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData"),
                    new FlagDef("Slash Spider (1)", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData"),
                    new FlagDef("Slash Spider (2)", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData"),
                    new FlagDef("Slash Spider (3)", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData"),
                    new FlagDef("Slash Spider (4)", SceneInstances.Deepnest_Spider_Town, false, "PersistentBoolData"),
                    new FlagDef("Soul Totem 5", SceneInstances.Deepnest_Spider_Town, false, "PersistentIntData"),
                #endregion

                #region Deepnest East
                    // Deepnest_East_01
                    new FlagDef("Blow Fly", SceneInstances.Deepnest_East_01, false, "PersistentBoolData"),
                    new FlagDef("Blow Fly (1)", SceneInstances.Deepnest_East_01, false, "PersistentBoolData"),
                    new FlagDef("Blow Fly (2)", SceneInstances.Deepnest_East_01, false, "PersistentBoolData"),
                    new FlagDef("Blow Fly (3)", SceneInstances.Deepnest_East_01, false, "PersistentBoolData"),
                    new FlagDef("Blow Fly (4)", SceneInstances.Deepnest_East_01, false, "PersistentBoolData"),
                    new FlagDef("Blow Fly (5)", SceneInstances.Deepnest_East_01, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Deepnest_East_01, false, "PersistentBoolData"),
                    new FlagDef("Soul Totem mini_two_horned", SceneInstances.Deepnest_East_01, false, "PersistentIntData"),

                    // Deepnest_East_09
                    new FlagDef("Ceiling Dropper", SceneInstances.Deepnest_East_09, false, "PersistentBoolData"),
                    new FlagDef("Ceiling Dropper (1)", SceneInstances.Deepnest_East_09, false, "PersistentBoolData"),
                    new FlagDef("Ceiling Dropper (2)", SceneInstances.Deepnest_East_09, false, "PersistentBoolData"),
                    new FlagDef("Ceiling Dropper (3)", SceneInstances.Deepnest_East_09, false, "PersistentBoolData"),
                    new FlagDef("Ceiling Dropper (4)", SceneInstances.Deepnest_East_09, false, "PersistentBoolData"),
                    new FlagDef("Ceiling Dropper (5)", SceneInstances.Deepnest_East_09, false, "PersistentBoolData"),
                    new FlagDef("Ceiling Dropper (6)", SceneInstances.Deepnest_East_09, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Deepnest_East_09, false, "PersistentBoolData"),
                #endregion

                #region Dirtmouth
                    // Town
                    new FlagDef("Death Respawn Trigger", SceneInstances.Town, false, "PersistentBoolData"),
                    new FlagDef("Death Respawn Trigger 1", SceneInstances.Town, false, "PersistentBoolData"),
                    new FlagDef("Interact Reminder", SceneInstances.Town, false, "PersistentBoolData"),

                    // Tutorial_01
                    new FlagDef("Health Cocoon", SceneInstances.Tutorial_01, true, "PersistentBoolData", "Broke Lifeblood Cocoon"),
                    new FlagDef("Initial Fall Impact", SceneInstances.Tutorial_01, false, "PersistentBoolData", "Player fell hard on game start"),
                    new FlagDef("Interact Reminder", SceneInstances.Tutorial_01, false, "PersistentBoolData", "Told how to Focus"),
                    new FlagDef("Chest", SceneInstances.Tutorial_01, false, "PersistentBoolData"),
                    new FlagDef("Inverse Remasker", SceneInstances.Tutorial_01, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Tutorial_01, false, "PersistentBoolData"),
                    new FlagDef("Secret Sound Region", SceneInstances.Tutorial_01, false, "PersistentBoolData"),
                    new FlagDef("Secret Sound Region (1)", SceneInstances.Tutorial_01, false, "PersistentBoolData"),
                    new FlagDef("fury charm_remask", SceneInstances.Tutorial_01, false, "PersistentBoolData"),
                    new FlagDef("inverse_remask_right", SceneInstances.Tutorial_01, false, "PersistentBoolData"),
                    new FlagDef("Shiny Item (1)", SceneInstances.Tutorial_01, false, "PersistentBoolData", "Got Charm 6"),
                #endregion

                #region Fungus 1
                    // Fungus1_01
                    new FlagDef("Mossman_Runner", SceneInstances.Fungus1_01, false, "PersistentBoolData"),
                    new FlagDef("Mossman_Shaker", SceneInstances.Fungus1_01, false, "PersistentBoolData"),
                    new FlagDef("Mossman_Shaker (1)", SceneInstances.Fungus1_01, false, "PersistentBoolData"),
                    new FlagDef("Plant Trap", SceneInstances.Fungus1_01, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Fungus1_01, false, "PersistentBoolData"),

                    // Fungus1_02
                    new FlagDef("Mossman_Shaker", SceneInstances.Fungus1_02, false, "PersistentBoolData"),

                    // Fungus1_03
                    new FlagDef("Plant Trap", SceneInstances.Fungus1_03, false, "PersistentBoolData"),
                    new FlagDef("Zombie Barger", SceneInstances.Fungus1_03, false, "PersistentBoolData"),
                    new FlagDef("Zombie Hornhead", SceneInstances.Fungus1_03, false, "PersistentBoolData"),
                    new FlagDef("Zombie Runner", SceneInstances.Fungus1_03, false, "PersistentBoolData"),
                    new FlagDef("Zombie Runner (1)", SceneInstances.Fungus1_03, false, "PersistentBoolData"),

                    // Fungus1_04
                    new FlagDef("Camera Locks Boss", SceneInstances.Fungus1_04, false, "PersistentBoolData"),
                    new FlagDef("Reminder Look Down", SceneInstances.Fungus1_04, false, "PersistentBoolData"),
                    new FlagDef("secret mask 2", SceneInstances.Fungus1_04, false, "PersistentBoolData"),
                    new FlagDef("Shiny Item", SceneInstances.Fungus1_04, false, "PersistentBoolData"),

                    // Fungus1_05
                    new FlagDef("Mossman_Runner", SceneInstances.Fungus1_05, false, "PersistentBoolData"),
                    new FlagDef("Mossman_Shaker", SceneInstances.Fungus1_05, false, "PersistentBoolData"),
                    new FlagDef("Mossman_Shaker (1)", SceneInstances.Fungus1_05, false, "PersistentBoolData"),

                    // Fungus1_06
                    new FlagDef("Mossman_Runner", SceneInstances.Fungus1_06, false, "PersistentBoolData"),
                    new FlagDef("Mossman_Shaker", SceneInstances.Fungus1_06, false, "PersistentBoolData"),
                    new FlagDef("Mossman_Shaker (1)", SceneInstances.Fungus1_06, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Fungus1_06, false, "PersistentBoolData"),
                    new FlagDef("Secret sounder", SceneInstances.Fungus1_06, false, "PersistentBoolData"),

                    // Fungus1_07
                    new FlagDef("Mossman_Runner (1)", SceneInstances.Fungus1_07, false, "PersistentBoolData"),
                    new FlagDef("Mossman_Runner", SceneInstances.Fungus1_07, false, "PersistentBoolData"),
                    new FlagDef("Remasker", SceneInstances.Fungus1_07, false, "PersistentBoolData"),
                    new FlagDef("Soul Totem mini_horned", SceneInstances.Fungus1_07, false, "PersistentIntData"),
                    
                    // Fungus1_08
                    new FlagDef("Secret Mask", SceneInstances.Fungus1_08, false, "PersistentBoolData"),
                    new FlagDef("Shiny Item", SceneInstances.Fungus1_08, false, "PersistentBoolData"),

                    // Fungus1_10
                    new FlagDef("Moss Charger", SceneInstances.Fungus1_10, false, "PersistentBoolData"),
                    new FlagDef("Moss Charger (1)", SceneInstances.Fungus1_10, false, "PersistentBoolData"),
                    new FlagDef("Moss Charger 1 (1)", SceneInstances.Fungus1_10, false, "PersistentBoolData"),
                    new FlagDef("Moss Charger 1 (2)", SceneInstances.Fungus1_10, false, "PersistentBoolData"),
                    new FlagDef("Flamebearer Spawn", SceneInstances.Fungus1_10, false, "PersistentBoolData"),
                    new FlagDef("Shiny Item", SceneInstances.Fungus1_10, false, "PersistentBoolData"),

                    // Fungus1_11
                    new FlagDef("Acid Walker", SceneInstances.Fungus1_11, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Fungus1_11, false, "PersistentBoolData"),
                    new FlagDef("Shiny Item", SceneInstances.Fungus1_11, false, "PersistentBoolData"),

                    // Fungus1_12
                    new FlagDef("Acid Walker", SceneInstances.Fungus1_12, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Fungus1_12, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask sounder", SceneInstances.Fungus1_12, false, "PersistentBoolData"),

                    // Fungus1_13
                    new FlagDef("Acid Walker", SceneInstances.Fungus1_13, false, "PersistentBoolData"),
                    new FlagDef("Acid Walker (1)", SceneInstances.Fungus1_13, false, "PersistentBoolData"),
                    new FlagDef("Acid Walker (2)", SceneInstances.Fungus1_13, false, "PersistentBoolData"),
                    new FlagDef("Acid Walker (3)", SceneInstances.Fungus1_13, false, "PersistentBoolData"),
                    new FlagDef("Acid Walker (4)", SceneInstances.Fungus1_13, false, "PersistentBoolData"),
                    new FlagDef("Acid Walker (5)", SceneInstances.Fungus1_13, false, "PersistentBoolData"),

                    // Fungus1_14
                    new FlagDef("Shiny Item", SceneInstances.Fungus1_14, false, "PersistentBoolData"),

                    // Fungus1_17
                    new FlagDef("Moss Charger", SceneInstances.Fungus1_17, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Fungus1_17, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask (1)", SceneInstances.Fungus1_17, false, "PersistentBoolData"),

                    // Fungus1_19
                    new FlagDef("Mossman_Shaker", SceneInstances.Fungus1_19, false, "PersistentBoolData"),
                    new FlagDef("Plant Trap", SceneInstances.Fungus1_19, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Fungus1_19, false, "PersistentBoolData"),

                    // Fungus1_21
                    new FlagDef("Moss Charger", SceneInstances.Fungus1_21, false, "PersistentBoolData"),
                    new FlagDef("Moss Charger (1)", SceneInstances.Fungus1_21, false, "PersistentBoolData"),
                    new FlagDef("Moss Charger (2)", SceneInstances.Fungus1_21, false, "PersistentBoolData"),
                    new FlagDef("Moss Knight", SceneInstances.Fungus1_21, false, "PersistentBoolData"),
                    new FlagDef("Moss Knight (1)", SceneInstances.Fungus1_21, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Fungus1_21, false, "PersistentBoolData"),

                    // Fungus1_22
                    new FlagDef("Mossman_Shaker", SceneInstances.Fungus1_22, false, "PersistentBoolData"),
                    new FlagDef("Mossman_Shaker (1)", SceneInstances.Fungus1_22, false, "PersistentBoolData"),
                    new FlagDef("Mossman_Shaker (2)", SceneInstances.Fungus1_22, false, "PersistentBoolData"),
                    new FlagDef("Plant Trap", SceneInstances.Fungus1_22, false, "PersistentBoolData"),
                    new FlagDef("Plant Trap (1)", SceneInstances.Fungus1_22, false, "PersistentBoolData"),
                    new FlagDef("Plant Trap (2)", SceneInstances.Fungus1_22, false, "PersistentBoolData"),
                    new FlagDef("Plant Trap (3)", SceneInstances.Fungus1_22, false, "PersistentBoolData"),
                    new FlagDef("Plant Trap (4)", SceneInstances.Fungus1_22, false, "PersistentBoolData"),
                    new FlagDef("Plant Trap (5)", SceneInstances.Fungus1_22, false, "PersistentBoolData"),

                    // Fungus1_26
                    new FlagDef("Inverse Remasker", SceneInstances.Fungus1_26, false, "PersistentBoolData"),
                    new FlagDef("Moss Knight", SceneInstances.Fungus1_26, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Fungus1_26, false, "PersistentBoolData"),
                    new FlagDef("secret sound", SceneInstances.Fungus1_26, false, "PersistentBoolData"),

                    // Fungus1_28
                    new FlagDef("mask_01", SceneInstances.Fungus1_28, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Fungus1_28, false, "PersistentBoolData"),
                    new FlagDef("Shiny Item", SceneInstances.Fungus1_28, false, "PersistentBoolData"),

                    // Fungus1_29
                    new FlagDef("Soul Totem mini_horned", SceneInstances.Fungus1_29, false, "PersistentIntData"),

                    // Fungus1_30
                    new FlagDef("Soul Totem mini_horned", SceneInstances.Fungus1_30, false, "PersistentIntData"),

                    // Fungus1_31
                    new FlagDef("Mossman_Runner", SceneInstances.Fungus1_31, false, "PersistentBoolData"),
                    new FlagDef("Mossman_Shaker", SceneInstances.Fungus1_31, false, "PersistentBoolData"),
                    new FlagDef("Mossman_Shaker (1)", SceneInstances.Fungus1_31, false, "PersistentBoolData"),

                    // Fungus1_32
                    new FlagDef("Health Cocoon", SceneInstances.Fungus1_32, false, "PersistentBoolData"),
                    new FlagDef("Moss Knight B", SceneInstances.Fungus1_32, false, "PersistentBoolData"),
                    new FlagDef("Moss Knight C", SceneInstances.Fungus1_32, false, "PersistentBoolData"),

                    // Fungus1_34
                    new FlagDef("Plant Trap", SceneInstances.Fungus1_34, false, "PersistentBoolData"),
                    new FlagDef("Zombie Hornhead", SceneInstances.Fungus1_34, false, "PersistentBoolData"),
                    new FlagDef("Zombie Leaper", SceneInstances.Fungus1_34, false, "PersistentBoolData"),
                    new FlagDef("Zombie Runner", SceneInstances.Fungus1_34, false, "PersistentBoolData"),

                    // Fungus1_36
                    new FlagDef("Remasker", SceneInstances.Fungus1_36, false, "PersistentBoolData"),
                #endregion

                #region Fungus 2
                    // Fungus2_01
                    new FlagDef("Remasker", SceneInstances.Fungus2_01, false, "PersistentBoolData"),

                    // Fungus2_03
                    new FlagDef("Fungus Flyer", SceneInstances.Fungus2_03, false, "PersistentBoolData"),
                    new FlagDef("Fungus Flyer (1)", SceneInstances.Fungus2_03, false, "PersistentBoolData"),
                    new FlagDef("Fungus Flyer (2)", SceneInstances.Fungus2_03, false, "PersistentBoolData"),
                    new FlagDef("Zombie Fungus B", SceneInstances.Fungus2_03, false, "PersistentBoolData"),
                    new FlagDef("Zombie Hornhead", SceneInstances.Fungus2_03, false, "PersistentBoolData"),

                    // Fungus2_05
                    new FlagDef("Shiny Item", SceneInstances.Fungus2_05, false, "PersistentBoolData", "Shrumal Ogre battle notch"),
                    new FlagDef("Mushroom Brawler", SceneInstances.Fungus2_05, false, "PersistentBoolData"),

                    // Fungus2_06
                    new FlagDef("Fungus Flyer", SceneInstances.Fungus2_06, false, "PersistentBoolData"),
                    new FlagDef("Fungus Flyer (1)", SceneInstances.Fungus2_06, false, "PersistentBoolData"),
                    new FlagDef("Fungus Flyer (2)", SceneInstances.Fungus2_06, false, "PersistentBoolData"),
                    new FlagDef("Fungus Flyer (3)", SceneInstances.Fungus2_06, false, "PersistentBoolData"),

                    // Fungus2_07
                    new FlagDef("Mushroom Roller", SceneInstances.Fungus2_07, false, "PersistentBoolData"),
                    new FlagDef("Mushroom Roller (1)", SceneInstances.Fungus2_07, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Fungus2_07, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask (1)", SceneInstances.Fungus2_07, false, "PersistentBoolData"),

                    // Fungus2_08
                    new FlagDef("Fungus Flyer", SceneInstances.Fungus2_08, false, "PersistentBoolData"),

                    // Fungus2_10
                    new FlagDef("Secret Mask", SceneInstances.Fungus2_10, false, "PersistentBoolData"),
                    new FlagDef("Soul Totem mini_horned", SceneInstances.Fungus2_10, false, "PersistentIntData"),
                    new FlagDef("Zombie Fungus A", SceneInstances.Fungus2_10, false, "PersistentBoolData"),
                    new FlagDef("Zombie Fungus B", SceneInstances.Fungus2_10, false, "PersistentBoolData"),

                    // Fungus2_11
                    new FlagDef("Fungus Flyer", SceneInstances.Fungus2_11, false, "PersistentBoolData"),

                    // Fungus2_12
                    new FlagDef("Mantis", SceneInstances.Fungus2_12, false, "PersistentBoolData"),
                    new FlagDef("Mantis (1)", SceneInstances.Fungus2_12, false, "PersistentBoolData"),
                    new FlagDef("Mantis (2)", SceneInstances.Fungus2_12, false, "PersistentBoolData"),

                    // Fungus2_13
                    new FlagDef("Mantis", SceneInstances.Fungus2_13, false, "PersistentBoolData"),
                    new FlagDef("Mantis (1)", SceneInstances.Fungus2_13, false, "PersistentBoolData"),
                    new FlagDef("Mantis (2)", SceneInstances.Fungus2_13, false, "PersistentBoolData"),
                    new FlagDef("Mantis (3)", SceneInstances.Fungus2_13, false, "PersistentBoolData"),
                    new FlagDef("Mantis (4)", SceneInstances.Fungus2_13, false, "PersistentBoolData"),

                    // Fungus2_14
                    new FlagDef("Gate Mantis", SceneInstances.Fungus2_14, false, "PersistentBoolData"),
                    new FlagDef("Shiny Item Stand", SceneInstances.Fungus2_14, false, "PersistentBoolData"),
                    new FlagDef("Mantis (1)", SceneInstances.Fungus2_14, false, "PersistentBoolData"),
                    new FlagDef("Mantis (2)", SceneInstances.Fungus2_14, false, "PersistentBoolData"),

                    // Fungus2_15
                    new FlagDef("Health Cocoon", SceneInstances.Fungus2_15, false, "PersistentBoolData"),
                    new FlagDef("Mantis", SceneInstances.Fungus2_15, false, "PersistentBoolData"),
                    new FlagDef("Mantis (1)", SceneInstances.Fungus2_15, false, "PersistentBoolData"),
                    new FlagDef("Mantis (2)", SceneInstances.Fungus2_15, false, "PersistentBoolData"),

                    // Fungus2_18
                    new FlagDef("Fungus Flyer", SceneInstances.Fungus2_18, false, "PersistentBoolData"),
                    new FlagDef("Zombie Fungus A", SceneInstances.Fungus2_18, false, "PersistentBoolData"),

                    // Fungus2_20
                    new FlagDef("Secret Mask", SceneInstances.Fungus2_20, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask (1)", SceneInstances.Fungus2_20, false, "PersistentBoolData"),
                    new FlagDef("Shiny Item Stand", SceneInstances.Fungus2_20, false, "PersistentBoolData"),

                    // Fungus2_21
                    new FlagDef("Quake Floor", SceneInstances.Fungus2_21, false, "PersistentBoolData"),
                    new FlagDef("Remasker", SceneInstances.Fungus2_21, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Fungus2_21, false, "PersistentBoolData"),
                    new FlagDef("secret sound", SceneInstances.Fungus2_21, false, "PersistentBoolData"),
                    new FlagDef("Soul Totem 5", SceneInstances.Fungus2_21, false, "PersistentIntData"),

                    // Fungus2_23
                    new FlagDef("Mushroom Roller", SceneInstances.Fungus2_23, false, "PersistentBoolData"),
                    new FlagDef("Mushroom Roller (1)", SceneInstances.Fungus2_23, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Fungus2_23, false, "PersistentBoolData"),
                    new FlagDef("Shiny Item Stand", SceneInstances.Fungus2_23, false, "PersistentBoolData"),

                    // Fungus2_28
                    new FlagDef("Mushroom Roller", SceneInstances.Fungus2_28, false, "PersistentBoolData"),
                    new FlagDef("Mushroom Roller (1)", SceneInstances.Fungus2_28, false, "PersistentBoolData"),
                    new FlagDef("Mushroom Roller (2)", SceneInstances.Fungus2_28, false, "PersistentBoolData"),
                    new FlagDef("Mushroom Roller (3)", SceneInstances.Fungus2_28, false, "PersistentBoolData"),

                    // Fungus2_29
                    new FlagDef("Fungus Flyer", SceneInstances.Fungus2_29, false, "PersistentBoolData"),
                    new FlagDef("Fungus Flyer (1)", SceneInstances.Fungus2_29, false, "PersistentBoolData"),
                    new FlagDef("Mushroom Brawler", SceneInstances.Fungus2_29, false, "PersistentBoolData"),
                    new FlagDef("Mushroom Brawler (1)", SceneInstances.Fungus2_29, false, "PersistentBoolData"),
                    new FlagDef("Mushroom Roller", SceneInstances.Fungus2_29, false, "PersistentBoolData"),
                    new FlagDef("Mushroom Roller (1)", SceneInstances.Fungus2_29, false, "PersistentBoolData"),
                    new FlagDef("Mushroom Roller (2)", SceneInstances.Fungus2_29, false, "PersistentBoolData"),
                    new FlagDef("Mushroom Roller (3)", SceneInstances.Fungus2_29, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Fungus2_29, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask (1)", SceneInstances.Fungus2_29, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask (2)", SceneInstances.Fungus2_29, false, "PersistentBoolData"),
                    new FlagDef("Soul Totem mini_horned", SceneInstances.Fungus2_29, false, "PersistentIntData"),

                    // Fungus2_31
                    new FlagDef("Inverse Remasker", SceneInstances.Fungus2_31, false, "PersistentBoolData"),
                    new FlagDef("Mantis", SceneInstances.Fungus2_31, false, "PersistentBoolData"),
                    new FlagDef("Remasker (1)", SceneInstances.Fungus2_31, false, "PersistentBoolData"),
                    new FlagDef("Remasker (2)", SceneInstances.Fungus2_31, false, "PersistentBoolData"),
                    new FlagDef("Shiny Item Charm", SceneInstances.Fungus2_31, false, "PersistentBoolData"),

                    // Fungus2_34
                    new FlagDef("Remasker", SceneInstances.Fungus2_34, false, "PersistentBoolData"),
                    new FlagDef("Remasker (1)", SceneInstances.Fungus2_34, false, "PersistentBoolData"),
                    new FlagDef("secret sound", SceneInstances.Fungus2_34, false, "PersistentBoolData"),
                #endregion

                #region Fungus 3
                    // Fungus3_01
                    new FlagDef("Jellyfish", SceneInstances.Fungus3_01, false, "PersistentBoolData"),
                    new FlagDef("Jellyfish 1", SceneInstances.Fungus3_01, false, "PersistentBoolData"),
                    new FlagDef("Jellyfish 2", SceneInstances.Fungus3_01, false, "PersistentBoolData"),
                    new FlagDef("Jellyfish 3", SceneInstances.Fungus3_01, false, "PersistentBoolData"),
                    new FlagDef("Jellyfish 4", SceneInstances.Fungus3_01, false, "PersistentBoolData"),
                    new FlagDef("Jellyfish 6", SceneInstances.Fungus3_01, false, "PersistentBoolData"),

                    // Fungus3_02
                    new FlagDef("Jellyfish", SceneInstances.Fungus3_02, false, "PersistentBoolData"),
                    new FlagDef("Jellyfish (1)", SceneInstances.Fungus3_02, false, "PersistentBoolData"),
                    new FlagDef("Jellyfish (2)", SceneInstances.Fungus3_02, false, "PersistentBoolData"),
                    new FlagDef("Jellyfish (3)", SceneInstances.Fungus3_02, false, "PersistentBoolData"),
                    new FlagDef("Jellyfish (4)", SceneInstances.Fungus3_02, false, "PersistentBoolData"),

                    // Fungus3_25
                    new FlagDef("Jellyfish", SceneInstances.Fungus3_25, false, "PersistentBoolData"),
                    new FlagDef("Jellyfish (1)", SceneInstances.Fungus3_25, false, "PersistentBoolData"),
                    new FlagDef("Jellyfish (2)", SceneInstances.Fungus3_25, false, "PersistentBoolData"),
                    new FlagDef("Jellyfish (3)", SceneInstances.Fungus3_25, false, "PersistentBoolData"),
                    new FlagDef("Jellyfish (4)", SceneInstances.Fungus3_25, false, "PersistentBoolData"),
                    new FlagDef("Jellyfish (5)", SceneInstances.Fungus3_25, false, "PersistentBoolData"),

                    // Fungus3_30
                    new FlagDef("Health Cocoon", SceneInstances.Fungus3_30, false, "PersistentBoolData"),

                    // Fungus3_39
                    new FlagDef("Acid Walker", SceneInstances.Fungus3_39, false, "PersistentBoolData"),
                    new FlagDef("Mantis Heavy", SceneInstances.Fungus3_39, false, "PersistentBoolData"),
                    new FlagDef("Mantis Heavy (1)", SceneInstances.Fungus3_39, false, "PersistentBoolData"),
                    new FlagDef("Moss Knight Fat", SceneInstances.Fungus3_39, false, "PersistentBoolData"),
                    new FlagDef("Moss Knight Fat (1)", SceneInstances.Fungus3_39, false, "PersistentBoolData"),
                    new FlagDef("Moss Knight Fat (2)", SceneInstances.Fungus3_39, false, "PersistentBoolData"),

                    // Fungus3_44
                    new FlagDef("Secret Mask", SceneInstances.Fungus3_44, false, "PersistentBoolData"),
                #endregion

                #region Mines
                    // Mines_02
                    new FlagDef("Zombie Miner 1", SceneInstances.Mines_02, false, "PersistentBoolData"),
                    new FlagDef("Zombie Miner 1 (1)", SceneInstances.Mines_02, false, "PersistentBoolData"),

                    // Mines_04
                    new FlagDef("Crystal Flyer", SceneInstances.Mines_04, false, "PersistentBoolData"),
                    new FlagDef("Crystal Flyer (1)", SceneInstances.Mines_04, false, "PersistentBoolData"),
                    new FlagDef("Crystal Flyer (2)", SceneInstances.Mines_04, false, "PersistentBoolData"),
                    new FlagDef("Crystal Flyer (3)", SceneInstances.Mines_04, false, "PersistentBoolData"),
                    new FlagDef("Zombie Miner 1", SceneInstances.Mines_04, false, "PersistentBoolData"),
                    new FlagDef("Zombie Miner 1 (1)", SceneInstances.Mines_04, false, "PersistentBoolData"),

                    // Mines_07
                    new FlagDef("Crystal Flyer", SceneInstances.Mines_07, false, "PersistentBoolData"),
                    new FlagDef("Crystal Flyer (2)", SceneInstances.Mines_07, false, "PersistentBoolData"),

                    // Mines_28
                    new FlagDef("Soul Totem 5", SceneInstances.Mines_28, false, "PersistentIntData"),

                    // Mines_29
                    new FlagDef("Shiny Item", SceneInstances.Mines_29, false, "PersistentBoolData"),

                    // Mines_33
                    new FlagDef("Secret Mask", SceneInstances.Mines_33, false, "PersistentBoolData"),
                #endregion

                #region Resting Grounds
                    // RestingGrounds_05
                    new FlagDef("Soul Totem 4", SceneInstances.RestingGrounds_05, false, "PersistentIntData"),

                    // RestingGrounds_07
                    new FlagDef("Remasker", SceneInstances.RestingGrounds_07, false, "PersistentBoolData"),

                    // RestingGrounds_09
                    new FlagDef("Shiny", SceneInstances.RestingGrounds_09, false, "PersistentBoolData"),
                #endregion

                #region Rooms
                    // Dream_01_False_Knight
                    new FlagDef("Battle Scene", SceneInstances.Dream_01_False_Knight, false, "PersistentBoolData"),

                    // Grimm_Divine
                    new FlagDef("Poo Greed", SceneInstances.Grimm_Divine, false, "PersistentBoolData", "Divine pooed the greed charm"),
                    new FlagDef("Poo Heart", SceneInstances.Grimm_Divine, false, "PersistentBoolData", "Divine pooed the heart charm"),
                    new FlagDef("Poo Strength", SceneInstances.Grimm_Divine, false, "PersistentBoolData", "Divine pooed the strength charm"),

                    // Grimm_Main_Tent
                    new FlagDef("Remasker", SceneInstances.Grimm_Main_Tent, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Grimm_Main_Tent, false, "PersistentBoolData"),

                    // Room_nailmaster
                    new FlagDef("Remasker", SceneInstances.Room_nailmaster, false, "PersistentBoolData"),
                    new FlagDef("Remasker (1)", SceneInstances.Room_nailmaster, false, "PersistentBoolData"),
                    new FlagDef("Remasker (2)", SceneInstances.Room_nailmaster, false, "PersistentBoolData"),

                    // Room_Tram
                    new FlagDef("gramaphone", SceneInstances.Room_Tram, false, "PersistentBoolData"),
                    new FlagDef("gramaphone (1)", SceneInstances.Room_Tram, false, "PersistentBoolData"),
                #endregion

                #region Ruins 2
                    // Ruins_Bathhouse
                    new FlagDef("Remasker", SceneInstances.Ruins_Bathhouse, false, "PersistentBoolData"),

                    // Ruins_Elevator
                    new FlagDef("Remasker", SceneInstances.Ruins_Elevator, false, "PersistentBoolData"),
                    new FlagDef("Remasker (1)", SceneInstances.Ruins_Elevator, false, "PersistentBoolData"),
                    new FlagDef("Remasker (2)", SceneInstances.Ruins_Elevator, false, "PersistentBoolData"),
                    new FlagDef("Remasker (3)", SceneInstances.Ruins_Elevator, false, "PersistentBoolData"),
                    new FlagDef("Ruins Lift", SceneInstances.Ruins_Elevator, false, "PersistentIntData"),
                    new FlagDef("Secret Sound Region", SceneInstances.Ruins_Elevator, false, "PersistentBoolData"),
                    new FlagDef("Secret Sound Region (1)", SceneInstances.Ruins_Elevator, false, "PersistentBoolData"),

                    // Ruins_House_02
                    new FlagDef("Gorgeous Husk", SceneInstances.Ruins_House_02, false, "PersistentBoolData"),
                    new FlagDef("Inverse Remasker", SceneInstances.Ruins_House_02, false, "PersistentBoolData"),
                    new FlagDef("Inverse Remasker (1)", SceneInstances.Ruins_House_02, false, "PersistentBoolData"),
                    new FlagDef("Inverse Remasker (2)", SceneInstances.Ruins_House_02, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie 1 (1)", SceneInstances.Ruins_House_02, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie Coward", SceneInstances.Ruins_House_02, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie Coward (1)", SceneInstances.Ruins_House_02, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie Fat", SceneInstances.Ruins_House_02, false, "PersistentBoolData"),
                    new FlagDef("Secret Sound Region", SceneInstances.Ruins_House_02, false, "PersistentBoolData"),

                    // Ruins1_03
                    new FlagDef("Ruins Flying Sentry", SceneInstances.Ruins1_03, false, "PersistentBoolData"),
                    new FlagDef("Ruins Flying Sentry (2)", SceneInstances.Ruins1_03, false, "PersistentBoolData"),
                    new FlagDef("Ruins Flying Sentry (3)", SceneInstances.Ruins1_03, false, "PersistentBoolData"),
                    new FlagDef("Ruins Lift 1", SceneInstances.Ruins1_03, false, "PersistentIntData"),
                    new FlagDef("Ruins Sentry 1", SceneInstances.Ruins1_03, false, "PersistentBoolData"),
                    new FlagDef("Ruins Sentry 1 (1)", SceneInstances.Ruins1_03, false, "PersistentBoolData"),
                    new FlagDef("Ruins Sentry 1 (2)", SceneInstances.Ruins1_03, false, "PersistentBoolData"),
                    new FlagDef("Ruins Sentry 1 (3)", SceneInstances.Ruins1_03, false, "PersistentBoolData"),
                    new FlagDef("Zombie Hornhead", SceneInstances.Ruins1_03, false, "PersistentBoolData"),
                    new FlagDef("Zombie Hornhead (1)", SceneInstances.Ruins1_03, false, "PersistentBoolData"),
                    new FlagDef("Zombie Leaper", SceneInstances.Ruins1_03, false, "PersistentBoolData"),

                    // Ruins1_04
                    new FlagDef("Inverse Remasker", SceneInstances.Ruins1_04, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Ruins1_04, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask (1)", SceneInstances.Ruins1_04, false, "PersistentBoolData"),

                    // Ruins1_05b
                    new FlagDef("Ruins Lift 2", SceneInstances.Ruins1_05b, false, "PersistentIntData"),

                    // Ruins1_27
                    new FlagDef("Remasker", SceneInstances.Ruins1_27, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie 1", SceneInstances.Ruins1_27, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie 1 (1)", SceneInstances.Ruins1_27, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie Coward", SceneInstances.Ruins1_27, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie Coward (1)", SceneInstances.Ruins1_27, false, "PersistentBoolData"),

                    // Ruins2_01_b
                    new FlagDef("Quake Floor Glass", SceneInstances.Ruins2_01_b, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie 1", SceneInstances.Ruins2_01_b, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie Coward", SceneInstances.Ruins2_01_b, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie Fat", SceneInstances.Ruins2_01_b, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie Fat (1)", SceneInstances.Ruins2_01_b, false, "PersistentBoolData"),
                    new FlagDef("Ruins Lift", SceneInstances.Ruins2_01_b, false, "PersistentIntData"),
                    new FlagDef("Ruins Lift (1)", SceneInstances.Ruins2_01_b, false, "PersistentIntData"),

                    // Ruins2_04
                    new FlagDef("Great Shield Zombie", SceneInstances.Ruins2_04, false, "PersistentBoolData"),
                    new FlagDef("Great Shield Zombie (1)", SceneInstances.Ruins2_04, false, "PersistentBoolData"),
                    new FlagDef("Great Shield Zombie (2)", SceneInstances.Ruins2_04, false, "PersistentBoolData"),
                    new FlagDef("Great Shield Zombie (3)", SceneInstances.Ruins2_04, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie 1", SceneInstances.Ruins2_04, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie 1 (1)", SceneInstances.Ruins2_04, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie Coward", SceneInstances.Ruins2_04, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie Coward (1)", SceneInstances.Ruins2_04, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie Fat", SceneInstances.Ruins2_04, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie Fat (1)", SceneInstances.Ruins2_04, false, "PersistentBoolData"),

                    // Ruins2_05
                    new FlagDef("Royal Zombie 1", SceneInstances.Ruins2_05, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie 1 (1)", SceneInstances.Ruins2_05, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie 1 (2)", SceneInstances.Ruins2_05, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie Coward (1)", SceneInstances.Ruins2_05, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie Coward (2)", SceneInstances.Ruins2_05, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie Coward (3)", SceneInstances.Ruins2_05, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie Fat", SceneInstances.Ruins2_05, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie Fat (1)", SceneInstances.Ruins2_05, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie Fat (2)", SceneInstances.Ruins2_05, false, "PersistentBoolData"),
                    new FlagDef("Ruins Flying Sentry", SceneInstances.Ruins2_05, false, "PersistentBoolData"),
                    new FlagDef("Ruins Lift", SceneInstances.Ruins2_05, false, "PersistentIntData"),
                    new FlagDef("Ruins Lift (1)", SceneInstances.Ruins2_05, false, "PersistentIntData"),
                    new FlagDef("Ruins Sentry 1", SceneInstances.Ruins2_05, false, "PersistentBoolData"),
                    new FlagDef("Ruins Sentry 1 (2)", SceneInstances.Ruins2_05, false, "PersistentBoolData"),
                    new FlagDef("Ruins Sentry 1 (3)", SceneInstances.Ruins2_05, false, "PersistentBoolData"),
                    new FlagDef("Ruins Sentry Fat", SceneInstances.Ruins2_05, false, "PersistentBoolData"),
                    new FlagDef("Ruins Sentry Fat (1)", SceneInstances.Ruins2_05, false, "PersistentBoolData"),
                    new FlagDef("Ruins Sentry 1 (1)", SceneInstances.Ruins2_05, false, "PersistentBoolData"),

                    // Ruins2_06
                    new FlagDef("remask", SceneInstances.Ruins2_06, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie 1 (1)", SceneInstances.Ruins2_06, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie Fat", SceneInstances.Ruins2_06, false, "PersistentBoolData"),
                    new FlagDef("Ruins Flying Sentry Javelin", SceneInstances.Ruins2_06, false, "PersistentBoolData"),
                    new FlagDef("Secret Mask", SceneInstances.Ruins2_06, false, "PersistentBoolData"),
                    new FlagDef("Zombie Hornhead", SceneInstances.Ruins2_06, false, "PersistentBoolData"),

                    // Ruins2_07
                    new FlagDef("Ceiling Dropper", SceneInstances.Ruins2_07, false, "PersistentBoolData"),
                    new FlagDef("Ceiling Dropper (1)", SceneInstances.Ruins2_07, false, "PersistentBoolData"),
                    new FlagDef("Ceiling Dropper (2)", SceneInstances.Ruins2_07, false, "PersistentBoolData"),

                    // Ruins2_09
                    new FlagDef("Royal Zombie 1", SceneInstances.Ruins2_09, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie Coward", SceneInstances.Ruins2_09, false, "PersistentBoolData"),
                    new FlagDef("Royal Zombie Fat", SceneInstances.Ruins2_09, false, "PersistentBoolData"),
                #endregion

            #endregion
        };
    }
}