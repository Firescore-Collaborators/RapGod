using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    public enum Punishment
    {
        None,
        Slap,
        Taser,
        JumpingJack,
        PushUps,
        ChickenDance,
        HammerHit,
        LowBlow,
        SpiderBucket,
        Spit,
    }

    public enum Powder
    {
        PowderA,
        PowderB,
        PowderC,
        PowderD,
        PowderE,
        PowderF
    }

    public enum Interior
    {
        InteriorA,
        InteriorB,
        InteriorC,
        InteriorD,
        InteriorE,
        InteriorF
    }

    public enum Wall
    {
        WallA,
        WallB,
        WallC,
        WallD,
        WallE,
        WallF
    }

    public enum Floor
    {
        FloorA,
        FloorB,
        FloorC,
        FloorD,
        FloorE,
        FloorF
    }

    public struct Item
    {
        public Item(Punishment punishment)
        {
            mPunishment = punishment;
            mPowder = null;
        }

        public Item(Powder powder)
        {
            mPowder = powder;
            mPunishment = null;
        }

        public bool IsValid() => IsPunishment() || IsPowder();

        public bool IsPunishment() => mPunishment.HasValue;
        public Punishment punishment => mPunishment.Value;

        public bool IsPowder() => mPowder.HasValue;
        public Powder Powder => mPowder.Value;

        private Punishment? mPunishment;
        private Powder? mPowder;
    }
}
