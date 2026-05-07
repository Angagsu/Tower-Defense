using System.Collections.Generic;


public static class AllHeroAndTowerTypes 
{
    public static List<HeroType> HeroTypes = new List<HeroType> 
    {
        global::HeroType.Knight,
        global::HeroType.Archer,
        global::HeroType.Dwarf,
        global::HeroType.Mage,
        // global::HeroType.Unknown,
        // global::HeroType.ComingSoon
    };

    public static List<HeroCardType> HeroCards = new List<HeroCardType> 
    {
        global::HeroCardType.KnightCard,
        global::HeroCardType.ArcherCard,
        global::HeroCardType.DwarfCard,
        global::HeroCardType.MageCard,
        // global::HeroCardType.Unknown,
        // global::HeroCardType.ComingSoon
    };

    public static List<TowerType> TowerTypes = new List<TowerType>
    {
        global::TowerType.Arrow,
        global::TowerType.Defender,
        global::TowerType.Golem,
        global::TowerType.Ice,
        global::TowerType.Fire,
        global::TowerType.Lightning,
        //global::TowerType.Unknown,
        //global::TowerType.ComingSoon
    };

    public static List<TowerCardType> TowerCards = new List<TowerCardType> 
    {
        global :: TowerCardType.Common,
        global :: TowerCardType.Epic,
        global :: TowerCardType.Mythic,
        global :: TowerCardType.Rare
    };
}
