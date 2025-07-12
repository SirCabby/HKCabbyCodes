using System.Collections.Generic;
using System.Linq;
using CabbyCodes.Flags.FlagInfo;

namespace CabbyCodes.Flags.FlagData
{
    /// <summary>
    /// Centralized hunter data structure providing access to all hunter target properties.
    /// </summary>
    public class HunterData
    {
        /// <summary>
        /// All hunter targets in the game with their properties.
        /// </summary>
        public static readonly Dictionary<string, HunterInfo> AllHunterTargets = new Dictionary<string, HunterInfo>
        {
            { "AbyssCrawler", new HunterInfo("AbyssCrawler", FlagInstances.killedAbyssCrawler, FlagInstances.killsAbyssCrawler) },
            { "AbyssTendril", new HunterInfo("AbyssTendril", FlagInstances.killedAbyssTendril, FlagInstances.killsAbyssTendril) },
            { "AcidFlyer", new HunterInfo("AcidFlyer", FlagInstances.killedAcidFlyer, FlagInstances.killsAcidFlyer) },
            { "AcidWalker", new HunterInfo("AcidWalker", FlagInstances.killedAcidWalker, FlagInstances.killsAcidWalker) },
            { "AngryBuzzer", new HunterInfo("AngryBuzzer", FlagInstances.killedAngryBuzzer, FlagInstances.killsAngryBuzzer) },
            { "BabyCentipede", new HunterInfo("BabyCentipede", FlagInstances.killedBabyCentipede, FlagInstances.killsBabyCentipede) },
            { "BeamMiner", new HunterInfo("BeamMiner", FlagInstances.killedBeamMiner, FlagInstances.killsBeamMiner) },
            { "BeeHatchling", new HunterInfo("BeeHatchling", FlagInstances.killedBeeHatchling, FlagInstances.killsBeeHatchling) },
            { "BeeStinger", new HunterInfo("BeeStinger", FlagInstances.killedBeeStinger, FlagInstances.killsBeeStinger) },
            { "BigBee", new HunterInfo("BigBee", FlagInstances.killedBigBee, FlagInstances.killsBigBee) },
            { "BigBuzzer", new HunterInfo("BigBuzzer", FlagInstances.killedBigBuzzer, FlagInstances.killsBigBuzzer) },
            { "BigCentipede", new HunterInfo("BigCentipede", FlagInstances.killedBigCentipede, FlagInstances.killsBigCentipede) },
            { "BigFly", new HunterInfo("BigFly", FlagInstances.killedBigFly, FlagInstances.killsBigFly) },
            { "BindingSeal", new HunterInfo("BindingSeal", FlagInstances.killedBindingSeal, FlagInstances.killsBindingSeal) },
            { "BlackKnight", new HunterInfo("BlackKnight", FlagInstances.killedBlackKnight, FlagInstances.killsBlackKnight) },
            { "BlobFlyer", new HunterInfo("BlobFlyer", FlagInstances.killedBlobFlyer, FlagInstances.killsBlobFlyer) },
            { "Blobble", new HunterInfo("Blobble", FlagInstances.killedBlobble, FlagInstances.killsBlobble) },
            { "Blocker", new HunterInfo("Blocker", FlagInstances.killedBlocker, FlagInstances.killsBlocker) },
            { "BlowFly", new HunterInfo("BlowFly", FlagInstances.killedBlowFly, FlagInstances.killsBlowFly) },
            { "Bouncer", new HunterInfo("Bouncer", FlagInstances.killedBouncer, FlagInstances.killsBouncer) },
            { "BurstingBouncer", new HunterInfo("BurstingBouncer", FlagInstances.killedBurstingBouncer, FlagInstances.killsBurstingBouncer) },
            { "BurstingZombie", new HunterInfo("BurstingZombie", FlagInstances.killedBurstingZombie, FlagInstances.killsBurstingZombie) },
            { "Buzzer", new HunterInfo("Buzzer", FlagInstances.killedBuzzer, FlagInstances.killsBuzzer) },
            { "CeilingDropper", new HunterInfo("CeilingDropper", FlagInstances.killedCeilingDropper, FlagInstances.killsCeilingDropper) },
            { "CentipedeHatcher", new HunterInfo("CentipedeHatcher", FlagInstances.killedCentipedeHatcher, FlagInstances.killsCentipedeHatcher) },
            { "Climber", new HunterInfo("Climber", FlagInstances.killedClimber, FlagInstances.killsClimber) },
            { "ColFlyingSentry", new HunterInfo("ColFlyingSentry", FlagInstances.killedColFlyingSentry, FlagInstances.killsColFlyingSentry) },
            { "ColHopper", new HunterInfo("ColHopper", FlagInstances.killedColHopper, FlagInstances.killsColHopper) },
            { "ColMiner", new HunterInfo("ColMiner", FlagInstances.killedColMiner, FlagInstances.killsColMiner) },
            { "ColMosquito", new HunterInfo("ColMosquito", FlagInstances.killedColMosquito, FlagInstances.killsColMosquito) },
            { "ColRoller", new HunterInfo("ColRoller", FlagInstances.killedColRoller, FlagInstances.killsColRoller) },
            { "ColShield", new HunterInfo("ColShield", FlagInstances.killedColShield, FlagInstances.killsColShield) },
            { "ColWorm", new HunterInfo("ColWorm", FlagInstances.killedColWorm, FlagInstances.killsColWorm) },
            { "Crawler", new HunterInfo("Crawler", FlagInstances.killedCrawler, FlagInstances.killsCrawler) },
            { "CrystalCrawler", new HunterInfo("CrystalCrawler", FlagInstances.killedCrystalCrawler, FlagInstances.killsCrystalCrawler) },
            { "CrystalFlyer", new HunterInfo("CrystalFlyer", FlagInstances.killedCrystalFlyer, FlagInstances.killsCrystalFlyer) },
            { "DreamGuard", new HunterInfo("DreamGuard", FlagInstances.killedDreamGuard, FlagInstances.killsDreamGuard) },
            { "Dummy", new HunterInfo("Dummy", FlagInstances.killedDummy, FlagInstances.killsDummy) },
            { "DungDefender", new HunterInfo("DungDefender", FlagInstances.killedDungDefender, FlagInstances.killsDungDefender) },
            { "EggSac", new HunterInfo("EggSac", FlagInstances.killedEggSac, FlagInstances.killsEggSac) },
            { "ElectricMage", new HunterInfo("ElectricMage", FlagInstances.killedElectricMage, FlagInstances.killsElectricMage) },
            { "FalseKnight", new HunterInfo("FalseKnight", FlagInstances.killedFalseKnight, FlagInstances.killsFalseKnight) },
            { "FatFluke", new HunterInfo("FatFluke", FlagInstances.killedFatFluke, FlagInstances.killsFatFluke) },
            { "FinalBoss", new HunterInfo("FinalBoss", FlagInstances.killedFinalBoss, FlagInstances.killsFinalBoss) },
            { "FlameBearerLarge", new HunterInfo("FlameBearerLarge", FlagInstances.killedFlameBearerLarge, FlagInstances.killsFlameBearerLarge) },
            { "FlameBearerMed", new HunterInfo("FlameBearerMed", FlagInstances.killedFlameBearerMed, FlagInstances.killsFlameBearerMed) },
            { "FlameBearerSmall", new HunterInfo("FlameBearerSmall", FlagInstances.killedFlameBearerSmall, FlagInstances.killsFlameBearerSmall) },
            { "FlipHopper", new HunterInfo("FlipHopper", FlagInstances.killedFlipHopper, FlagInstances.killsFlipHopper) },
            { "FlukeMother", new HunterInfo("FlukeMother", FlagInstances.killedFlukeMother, FlagInstances.killsFlukeMother) },
            { "Flukefly", new HunterInfo("Flukefly", FlagInstances.killedFlukefly, FlagInstances.killsFlukefly) },
            { "Flukeman", new HunterInfo("Flukeman", FlagInstances.killedFlukeman, FlagInstances.killsFlukeman) },
            { "FlyingSentryJavelin", new HunterInfo("FlyingSentryJavelin", FlagInstances.killedFlyingSentryJavelin, FlagInstances.killsFlyingSentryJavelin) },
            { "FlyingSentrySword", new HunterInfo("FlyingSentrySword", FlagInstances.killedFlyingSentrySword, FlagInstances.killsFlyingSentrySword) },
            { "FungCrawler", new HunterInfo("FungCrawler", FlagInstances.killedFungCrawler, FlagInstances.killsFungCrawler) },
            { "FungifiedZombie", new HunterInfo("FungifiedZombie", FlagInstances.killedFungifiedZombie, FlagInstances.killsFungifiedZombie) },
            { "FungoonBaby", new HunterInfo("FungoonBaby", FlagInstances.killedFungoonBaby, FlagInstances.killsFungoonBaby) },
            { "FungusFlyer", new HunterInfo("FungusFlyer", FlagInstances.killedFungusFlyer, FlagInstances.killsFungusFlyer) },
            { "GardenZombie", new HunterInfo("GardenZombie", FlagInstances.killedGardenZombie, FlagInstances.killsGardenZombie) },
            { "GhostAladar", new HunterInfo("GhostAladar", FlagInstances.killedGhostAladar, FlagInstances.killsGhostAladar) },
            { "GhostGalien", new HunterInfo("GhostGalien", FlagInstances.killedGhostGalien, FlagInstances.killsGhostGalien) },
            { "GhostHu", new HunterInfo("GhostHu", FlagInstances.killedGhostHu, FlagInstances.killsGhostHu) },
            { "GhostMarkoth", new HunterInfo("GhostMarkoth", FlagInstances.killedGhostMarkoth, FlagInstances.killsGhostMarkoth) },
            { "GhostMarmu", new HunterInfo("GhostMarmu", FlagInstances.killedGhostMarmu, FlagInstances.killsGhostMarmu) },
            { "GhostNoEyes", new HunterInfo("GhostNoEyes", FlagInstances.killedGhostNoEyes, FlagInstances.killsGhostNoEyes) },
            { "GhostXero", new HunterInfo("GhostXero", FlagInstances.killedGhostXero, FlagInstances.killsGhostXero) },
            { "GiantHopper", new HunterInfo("GiantHopper", FlagInstances.killedGiantHopper, FlagInstances.killsGiantHopper) },
            { "GodseekerMask", new HunterInfo("GodseekerMask", FlagInstances.killedGodseekerMask, FlagInstances.killsGodseekerMask) },
            { "GorgeousHusk", new HunterInfo("GorgeousHusk", FlagInstances.killedGorgeousHusk, FlagInstances.killsGorgeousHusk) },
            { "GrassHopper", new HunterInfo("GrassHopper", FlagInstances.killedGrassHopper, FlagInstances.killsGrassHopper) },
            { "GreatShieldZombie", new HunterInfo("GreatShieldZombie", FlagInstances.killedGreatShieldZombie, FlagInstances.killsGreatShieldZombie) },
            { "GreyPrince", new HunterInfo("GreyPrince", FlagInstances.killedGreyPrince, FlagInstances.killsGreyPrince) },
            { "Grimm", new HunterInfo("Grimm", FlagInstances.killedGrimm, FlagInstances.killsGrimm) },
            { "GrubMimic", new HunterInfo("GrubMimic", FlagInstances.killedGrubMimic, FlagInstances.killsGrubMimic) },
            { "Hatcher", new HunterInfo("Hatcher", FlagInstances.killedHatcher, FlagInstances.killsHatcher) },
            { "Hatchling", new HunterInfo("Hatchling", FlagInstances.killedHatchling, FlagInstances.killsHatchling) },
            { "HealthScuttler", new HunterInfo("HealthScuttler", FlagInstances.killedHealthScuttler, FlagInstances.killsHealthScuttler) },
            { "HeavyMantis", new HunterInfo("HeavyMantis", FlagInstances.killedHeavyMantis, FlagInstances.killsHeavyMantis) },
            { "HiveKnight", new HunterInfo("HiveKnight", FlagInstances.killedHiveKnight, FlagInstances.killsHiveKnight) },
            { "HollowKnight", new HunterInfo("HollowKnight", FlagInstances.killedHollowKnight, FlagInstances.killsHollowKnight) },
            { "HollowKnightPrime", new HunterInfo("HollowKnightPrime", FlagInstances.killedHollowKnightPrime, FlagInstances.killsHollowKnightPrime) },
            { "Hopper", new HunterInfo("Hopper", FlagInstances.killedHopper, FlagInstances.killsHopper) },
            { "Hornet", new HunterInfo("Hornet", FlagInstances.killedHornet, FlagInstances.killsHornet) },
            { "HunterMark", new HunterInfo("HunterMark", FlagInstances.killedHunterMark, FlagInstances.killsHunterMark) },
            { "InfectedKnight", new HunterInfo("InfectedKnight", FlagInstances.killedInfectedKnight, FlagInstances.killsInfectedKnight) },
            { "Inflater", new HunterInfo("Inflater", FlagInstances.killedInflater, FlagInstances.killsInflater) },
            { "JarCollector", new HunterInfo("JarCollector", FlagInstances.killedJarCollector, FlagInstances.killsJarCollector) },
            { "JellyCrawler", new HunterInfo("JellyCrawler", FlagInstances.killedJellyCrawler, FlagInstances.killsJellyCrawler) },
            { "Jellyfish", new HunterInfo("Jellyfish", FlagInstances.killedJellyfish, FlagInstances.killsJellyfish) },
            { "LaserBug", new HunterInfo("LaserBug", FlagInstances.killedLaserBug, FlagInstances.killsLaserBug) },
            { "LazyFlyer", new HunterInfo("LazyFlyer", FlagInstances.killedLazyFlyer, FlagInstances.killsLazyFlyer) },
            { "LesserMawlek", new HunterInfo("LesserMawlek", FlagInstances.killedLesserMawlek, FlagInstances.killsLesserMawlek) },
            { "LobsterLancer", new HunterInfo("LobsterLancer", FlagInstances.killedLobsterLancer, FlagInstances.killsLobsterLancer) },
            { "Mage", new HunterInfo("Mage", FlagInstances.killedMage, FlagInstances.killsMage) },
            { "MageBalloon", new HunterInfo("MageBalloon", FlagInstances.killedMageBalloon, FlagInstances.killsMageBalloon) },
            { "MageBlob", new HunterInfo("MageBlob", FlagInstances.killedMageBlob, FlagInstances.killsMageBlob) },
            { "MageKnight", new HunterInfo("MageKnight", FlagInstances.killedMageKnight, FlagInstances.killsMageKnight) },
            { "MageLord", new HunterInfo("MageLord", FlagInstances.killedMageLord, FlagInstances.killsMageLord) },
            { "Mantis", new HunterInfo("Mantis", FlagInstances.killedMantis, FlagInstances.killsMantis) },
            { "MantisFlyerChild", new HunterInfo("MantisFlyerChild", FlagInstances.killedMantisFlyerChild, FlagInstances.killsMantisFlyerChild) },
            { "MantisHeavyFlyer", new HunterInfo("MantisHeavyFlyer", FlagInstances.killedMantisHeavyFlyer, FlagInstances.killsMantisHeavyFlyer) },
            { "MantisLord", new HunterInfo("MantisLord", FlagInstances.killedMantisLord, FlagInstances.killsMantisLord) },
            { "Mawlek", new HunterInfo("Mawlek", FlagInstances.killedMawlek, FlagInstances.killsMawlek) },
            { "MawlekTurret", new HunterInfo("MawlekTurret", FlagInstances.killedMawlekTurret, FlagInstances.killsMawlekTurret) },
            { "MegaBeamMiner", new HunterInfo("MegaBeamMiner", FlagInstances.killedMegaBeamMiner, FlagInstances.killsMegaBeamMiner) },
            { "MegaJellyfish", new HunterInfo("MegaJellyfish", FlagInstances.killedMegaJellyfish, FlagInstances.killsMegaJellyfish) },
            { "MegaMossCharger", new HunterInfo("MegaMossCharger", FlagInstances.killedMegaMossCharger, FlagInstances.killsMegaMossCharger) },
            { "MenderBug", new HunterInfo("MenderBug", FlagInstances.killedMenderBug, FlagInstances.killsMenderBug) },
            { "MimicSpider", new HunterInfo("MimicSpider", FlagInstances.killedMimicSpider, FlagInstances.killsMimicSpider) },
            { "MinesCrawler", new HunterInfo("MinesCrawler", FlagInstances.killedMinesCrawler, FlagInstances.killsMinesCrawler) },
            { "MiniSpider", new HunterInfo("MiniSpider", FlagInstances.killedMiniSpider, FlagInstances.killsMiniSpider) },
            { "Mosquito", new HunterInfo("Mosquito", FlagInstances.killedMosquito, FlagInstances.killsMosquito) },
            { "MossCharger", new HunterInfo("MossCharger", FlagInstances.killedMossCharger, FlagInstances.killsMossCharger) },
            { "MossFlyer", new HunterInfo("MossFlyer", FlagInstances.killedMossFlyer, FlagInstances.killsMossFlyer) },
            { "MossKnight", new HunterInfo("MossKnight", FlagInstances.killedMossKnight, FlagInstances.killsMossKnight) },
            { "MossKnightFat", new HunterInfo("MossKnightFat", FlagInstances.killedMossKnightFat, FlagInstances.killsMossKnightFat) },
            { "MossWalker", new HunterInfo("MossWalker", FlagInstances.killedMossWalker, FlagInstances.killsMossWalker) },
            { "MossmanRunner", new HunterInfo("MossmanRunner", FlagInstances.killedMossmanRunner, FlagInstances.killsMossmanRunner) },
            { "MossmanShaker", new HunterInfo("MossmanShaker", FlagInstances.killedMossmanShaker, FlagInstances.killsMossmanShaker) },
            { "Mummy", new HunterInfo("Mummy", FlagInstances.killedMummy, FlagInstances.killsMummy) },
            { "MushroomBaby", new HunterInfo("MushroomBaby", FlagInstances.killedMushroomBaby, FlagInstances.killsMushroomBaby) },
            { "MushroomBrawler", new HunterInfo("MushroomBrawler", FlagInstances.killedMushroomBrawler, FlagInstances.killsMushroomBrawler) },
            { "MushroomRoller", new HunterInfo("MushroomRoller", FlagInstances.killedMushroomRoller, FlagInstances.killsMushroomRoller) },
            { "MushroomTurret", new HunterInfo("MushroomTurret", FlagInstances.killedMushroomTurret, FlagInstances.killsMushroomTurret) },
            { "NailBros", new HunterInfo("NailBros", FlagInstances.killedNailBros, FlagInstances.killsNailBros) },
            { "Nailsage", new HunterInfo("Nailsage", FlagInstances.killedNailsage, FlagInstances.killsNailsage) },
            { "NightmareGrimm", new HunterInfo("NightmareGrimm", FlagInstances.killedNightmareGrimm, FlagInstances.killsNightmareGrimm) },
            { "Oblobble", new HunterInfo("Oblobble", FlagInstances.killedOblobble, FlagInstances.killsOblobble) },
            { "OrangeBalloon", new HunterInfo("OrangeBalloon", FlagInstances.killedOrangeBalloon, FlagInstances.killsOrangeBalloon) },
            { "OrangeScuttler", new HunterInfo("OrangeScuttler", FlagInstances.killedOrangeScuttler, FlagInstances.killsOrangeScuttler) },
            { "Paintmaster", new HunterInfo("Paintmaster", FlagInstances.killedPaintmaster, FlagInstances.killsPaintmaster) },
            { "PalaceFly", new HunterInfo("PalaceFly", FlagInstances.killedPalaceFly, FlagInstances.killsPalaceFly) },
            { "PaleLurker", new HunterInfo("PaleLurker", FlagInstances.killedPaleLurker, FlagInstances.killsPaleLurker) },
            { "Pigeon", new HunterInfo("Pigeon", FlagInstances.killedPigeon, FlagInstances.killsPigeon) },
            { "PlantShooter", new HunterInfo("PlantShooter", FlagInstances.killedPlantShooter, FlagInstances.killsPlantShooter) },
            { "PrayerSlug", new HunterInfo("PrayerSlug", FlagInstances.killedPrayerSlug, FlagInstances.killsPrayerSlug) },
            { "Roller", new HunterInfo("Roller", FlagInstances.killedRoller, FlagInstances.killsRoller) },
            { "RoyalCoward", new HunterInfo("RoyalCoward", FlagInstances.killedRoyalCoward, FlagInstances.killsRoyalCoward) },
            { "RoyalDandy", new HunterInfo("RoyalDandy", FlagInstances.killedRoyalDandy, FlagInstances.killsRoyalDandy) },
            { "RoyalGuard", new HunterInfo("RoyalGuard", FlagInstances.killedRoyalGuard, FlagInstances.killsRoyalGuard) },
            { "RoyalPlumper", new HunterInfo("RoyalPlumper", FlagInstances.killedRoyalPlumper, FlagInstances.killsRoyalPlumper) },
            { "Sentry", new HunterInfo("Sentry", FlagInstances.killedSentry, FlagInstances.killsSentry) },
            { "SentryFat", new HunterInfo("SentryFat", FlagInstances.killedSentryFat, FlagInstances.killsSentryFat) },
            { "ShootSpider", new HunterInfo("ShootSpider", FlagInstances.killedShootSpider, FlagInstances.killsShootSpider) },
            { "Sibling", new HunterInfo("Sibling", FlagInstances.killedSibling, FlagInstances.killsSibling) },
            { "SlashSpider", new HunterInfo("SlashSpider", FlagInstances.killedSlashSpider, FlagInstances.killsSlashSpider) },
            { "SnapperTrap", new HunterInfo("SnapperTrap", FlagInstances.killedSnapperTrap, FlagInstances.killsSnapperTrap) },
            { "SpiderCorpse", new HunterInfo("SpiderCorpse", FlagInstances.killedSpiderCorpse, FlagInstances.killsSpiderCorpse) },
            { "SpiderFlyer", new HunterInfo("SpiderFlyer", FlagInstances.killedSpiderFlyer, FlagInstances.killsSpiderFlyer) },
            { "Spitter", new HunterInfo("Spitter", FlagInstances.killedSpitter, FlagInstances.killsSpitter) },
            { "SpittingZombie", new HunterInfo("SpittingZombie", FlagInstances.killedSpittingZombie, FlagInstances.killsSpittingZombie) },
            { "SuperSpitter", new HunterInfo("SuperSpitter", FlagInstances.killedSuperSpitter, FlagInstances.killsSuperSpitter) },
            { "TraitorLord", new HunterInfo("TraitorLord", FlagInstances.killedTraitorLord, FlagInstances.killsTraitorLord) },
            { "VoidIdol_1", new HunterInfo("VoidIdol_1", FlagInstances.killedVoidIdol_1, FlagInstances.killsVoidIdol_1) },
            { "VoidIdol_2", new HunterInfo("VoidIdol_2", FlagInstances.killedVoidIdol_2, FlagInstances.killsVoidIdol_2) },
            { "VoidIdol_3", new HunterInfo("VoidIdol_3", FlagInstances.killedVoidIdol_3, FlagInstances.killsVoidIdol_3) },
            { "WhiteDefender", new HunterInfo("WhiteDefender", FlagInstances.killedWhiteDefender, FlagInstances.killsWhiteDefender) },
            { "WhiteRoyal", new HunterInfo("WhiteRoyal", FlagInstances.killedWhiteRoyal, FlagInstances.killsWhiteRoyal) },
            { "Worm", new HunterInfo("Worm", FlagInstances.killedWorm, FlagInstances.killsWorm) },
            { "ZapBug", new HunterInfo("ZapBug", FlagInstances.killedZapBug, FlagInstances.killsZapBug) },
            { "ZombieBarger", new HunterInfo("ZombieBarger", FlagInstances.killedZombieBarger, FlagInstances.killsZombieBarger) },
            { "ZombieGuard", new HunterInfo("ZombieGuard", FlagInstances.killedZombieGuard, FlagInstances.killsZombieGuard) },
            { "ZombieHive", new HunterInfo("ZombieHive", FlagInstances.killedZombieHive, FlagInstances.killsZombieHive) },
            { "ZombieHornhead", new HunterInfo("ZombieHornhead", FlagInstances.killedZombieHornhead, FlagInstances.killsZombieHornhead) },
            { "ZombieLeaper", new HunterInfo("ZombieLeaper", FlagInstances.killedZombieLeaper, FlagInstances.killsZombieLeaper) },
            { "ZombieMiner", new HunterInfo("ZombieMiner", FlagInstances.killedZombieMiner, FlagInstances.killsZombieMiner) },
            { "ZombieRunner", new HunterInfo("ZombieRunner", FlagInstances.killedZombieRunner, FlagInstances.killsZombieRunner) },
            { "ZombieShield", new HunterInfo("ZombieShield", FlagInstances.killedZombieShield, FlagInstances.killsZombieShield) },
            { "Zote", new HunterInfo("Zote", FlagInstances.killedZote, FlagInstances.killsZote) },
            { "ZotelingBalloon", new HunterInfo("ZotelingBalloon", FlagInstances.killedZotelingBalloon, FlagInstances.killsZotelingBalloon) },
            { "ZotelingBuzzer", new HunterInfo("ZotelingBuzzer", FlagInstances.killedZotelingBuzzer, FlagInstances.killsZotelingBuzzer) },
            { "ZotelingHopper", new HunterInfo("ZotelingHopper", FlagInstances.killedZotelingHopper, FlagInstances.killsZotelingHopper) }
        };

        /// <summary>
        /// Gets hunter target information by enemy name.
        /// </summary>
        /// <param name="enemyName">The enemy name</param>
        /// <returns>The hunter target information</returns>
        public static HunterInfo GetHunterTarget(string enemyName)
        {
            if (AllHunterTargets.TryGetValue(enemyName, out HunterInfo hunterInfo))
            {
                return hunterInfo;
            }
            throw new System.ArgumentOutOfRangeException(nameof(enemyName), $"Hunter target '{enemyName}' does not exist");
        }

        /// <summary>
        /// Gets all hunter targets as a list.
        /// </summary>
        /// <returns>List of all hunter targets</returns>
        public static List<HunterInfo> GetAllHunterTargets()
        {
            return AllHunterTargets.Values.ToList();
        }

        /// <summary>
        /// Gets all hunter target names as a list.
        /// </summary>
        /// <returns>List of all hunter target names</returns>
        public static List<string> GetAllHunterTargetNames()
        {
            return AllHunterTargets.Keys.ToList();
        }
    }
} 