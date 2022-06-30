using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    public static class ProgressUtils
    {
        public static bool IsLevelEnd(int level)
        {
            for (int i = 0; i < Config.LEVELS_PER_DAYEND.Length; i++)
            {
                if (level == Config.LEVELS_PER_DAYEND[i])
                {
                    return true;
                }
            }
            return false;
        }
        public static int GetMilestoneFromLevel(int level)
        {
            Debug.Assert(level > 0);
            --level;

            int levelTotal = 0;
            for (int milestone = 0; milestone < Config.LEVELS_PER_MILESTONE.Length; ++milestone)
            {
                if (level < levelTotal + Config.LEVELS_PER_MILESTONE[milestone])
                    return milestone + 1;

                levelTotal += Config.LEVELS_PER_MILESTONE[milestone];
            }

            int levelsPerMilestone = Config.LEVELS_PER_MILESTONE[Config.LEVELS_PER_MILESTONE.Length - 1];
            return (level - levelTotal) / levelsPerMilestone + Config.LEVELS_PER_MILESTONE.Length + 1;
        }

        public static int GetFirstLevelOfMilestone(int milestone)
        {
            Debug.Assert(milestone > 0);
            --milestone;

            int levelNumber = 1;
            for (int index = 0; index < milestone && index < Config.LEVELS_PER_MILESTONE.Length; ++index)
                levelNumber += Config.LEVELS_PER_MILESTONE[index];

            if (milestone > Config.LEVELS_PER_MILESTONE.Length)
                levelNumber += Config.LEVELS_PER_MILESTONE[Config.LEVELS_PER_MILESTONE.Length - 1] * (milestone - Config.LEVELS_PER_MILESTONE.Length);

            return levelNumber;
        }

        public static int GetLevelOfLastItemUnlock(int currentLevel)
        {
            Item item;
            int level;

            for (int index = 0; index < Config.ITEM_UNLOCK_LEVELS.Length; ++index)
            {
                (item, level) = Config.ITEM_UNLOCK_LEVELS[index];
                if (currentLevel == level)
                {
                    return level;
                }
                else if (level > currentLevel)
                {
                    if (index == 0)
                        return 1;

                    (item, level) = Config.ITEM_UNLOCK_LEVELS[index - 1];
                    return level;
                }
            }

            (item, level) = Config.ITEM_UNLOCK_LEVELS[Config.ITEM_UNLOCK_LEVELS.Length - 1];
            return level;
        }

        public static (Item, int /*levelNumber*/) GetNextItemToUnlock(int currentLevel, int levelMultiplier)
        {
            Item item;
            int level;

            for (int index = 0; index < Config.ITEM_UNLOCK_LEVELS.Length; ++index)
            {
                (item, level) = Config.ITEM_UNLOCK_LEVELS[index];
                if (level > currentLevel && levelMultiplier == 0)
                {
                    return (item, level);
                }
            }
            (item, level) = Config.ITEM_UNLOCK_LEVELS[Config.ITEM_UNLOCK_LEVELS.Length - 1];

            return (new Item(), level);
        }

        public static Item GetItemToUnlockOnLevel(int currentLevel, int levelMultiplier)
        {
            Debug.Assert(currentLevel > 0);

            foreach (var (item, level) in Config.ITEM_UNLOCK_LEVELS)
            {
                if (level == currentLevel + 1 && levelMultiplier == 0)
                    return item;
            }

            return new Item();
        }

        public static bool IsLastLevelInMilestone(int currentLevel)
        {
            int milestone = GetMilestoneFromLevel(currentLevel);
            int firstLevelOfNextMileStone = GetFirstLevelOfMilestone(milestone + 1);
            return (currentLevel == firstLevelOfNextMileStone - 1);
        }
    }
}
