using System.Collections.Generic;
using UnityEngine;

public static class AllHeroTypes 
{
    public static List<HeroType> HeroType = new List<HeroType> { global::HeroType.Knight, global::HeroType.Archer, global::HeroType.Dwarf, global::HeroType.Mage };
    public static List<HeroCardType> HeroCards = new List<HeroCardType> { global::HeroCardType.KnightCard, global::HeroCardType.ArcherCard, global::HeroCardType.DwarfCard, global::HeroCardType.MageCard };
    public static List<HeroType> GetHeroTypes()
    {
        HeroType.Add(new HeroType());
        HeroType.Add(global::HeroType.Archer);
        HeroType.Add(global::HeroType.Dwarf);
        HeroType.Add(global::HeroType.Mage);
        return HeroType;
    } 
}
