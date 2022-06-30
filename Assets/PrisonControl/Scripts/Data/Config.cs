using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    public static class Config
    {
        public static readonly int[] LEVELS_PER_MILESTONE =
        {
            3, 3, 4, 5 // 5, 5, 5...
        };

        // Must be in ascending order!
        public static readonly (Item, int)[] ITEM_UNLOCK_LEVELS =
        {
            (new Item(Punishment.SpiderBucket), 6),
            (new Item(Punishment.Spit), 10),
            (new Item(Punishment.LowBlow), 16),
            (new Item(Punishment.HammerHit), 26),
            (new Item(Punishment.ChickenDance), 37),
            (new Item(Punishment.JumpingJack), 67)
        };

        public static readonly int[] LEVELS_PER_DAYEND =
        {
            3, 5 , 8 , 11 , 15 , 18 , 21 , 25 , 28 , 32 , 36 , 43 , 47 , 50 , 53 , 58 ,61 , 65 , 68 , 69
        };

        public static readonly int[] INTERIOR_PRICES =
        {
            100, 200, 300
        };

        public static readonly int[] WALL_PRICES =
        {
            150, 250, 350
        };

        public static readonly int[] FLOOR_PRICES =
        {
            200, 300, 400
        };
    }
}
