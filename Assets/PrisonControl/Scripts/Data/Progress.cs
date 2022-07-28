using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    public struct UnlockInfo
    {
        public float unlockProgressRatio;
        public Item nextItem;
        public float unlockDoneProgressRatio;
    }

    public class Progress
    {
        public event System.Action OnCurrencyChanged;
        public event System.Action OnCurrentLevelChanged;
        public event System.Action OnCurrentMilestoneChanged;
        public event System.Action OnSalonUpgraded;

        public int Currency
        {
            get => mData.currency;

            set
            {
                Debug.Assert(value >= 0);
                mData.currency = value;
                Save();
                OnCurrencyChanged?.Invoke();
            }
        }

        public bool CanAfford(int price)
        {
            return (Currency >= price);
        }

        //RapBattle
        public string SocialHandleName{
            get => mData.socialHandleName;
            set
            {
                mData.socialHandleName = value;
                Save();
            }
        }

        public string DisplayPic
        {
            get => mData.displayPic;
            set
            {
                mData.displayPic = value;
                Save();
            }
        }
        public Vector2 DisplayPicSize
        {
            get => mData.dpSize;
            set
            {
                mData.dpSize = value;
                Save();
            }
        }
        public int FollowerCount
        {
            get => mData.followers;
            set
            {
                mData.followers = value;
                Save();
            }
        }

        public bool TapSmashTut
        {
            get => mData.tapSmashTut;
            set
            {
                mData.tapSmashTut = value;
                Save();
            }
        }

        public int CurrentLevel
        {
            get => mData.currentLevel;

            set
            {
                int lastMilestone = CurrentMilestone;

                mData.currentLevel = value;
                Save();

                OnCurrentLevelChanged?.Invoke();
                if (CurrentMilestone != lastMilestone)
                    OnCurrentMilestoneChanged?.Invoke();
            }
        }

        public int LevelMultiplier
        {
            get => mData.level_multiplier;

            set
            {
                mData.level_multiplier = value;
                Save();
            }
        }

        public bool IsMugShotTutDone
        {
            get => mData.isMugShotTutDone;

            set
            {
                mData.isMugShotTutDone = value;
                Save();
            }
        }

        public bool IsfoodPlatingTutDone
        {
            get => mData.isfoodPlatingTutDone;

            set
            {
                mData.isfoodPlatingTutDone = value;
                Save();
            }
        }

        public bool IsIdStepTutDone
        {
            get => mData.isIdStepTutDone;

            set
            {
                mData.isIdStepTutDone = value;
                Save();
            }
        }

        public bool IsLunchBoxStepTutDone
        {
            get => mData.isLunchBoxStepTutDone;

            set
            {
                mData.isLunchBoxStepTutDone = value;
                Save();
            }
        }

        public bool IsBribeStepTutDone
        {
            get => mData.isBribeStepTutDone;

            set
            {
                mData.isBribeStepTutDone = value;
                Save();
            }
        }

        public bool IsAudioExperianceDone
        {
            get => mData.isAudioExperianceDone;

            set
            {
                mData.isAudioExperianceDone = value;
                Save();
            }
        }

        
        public int CurrentMilestone
        {
            get => ProgressUtils.GetMilestoneFromLevel(CurrentLevel);
        }

        public UnlockInfo GetNextUnlockInfo(int level)
        {
            int lowerBound = ProgressUtils.GetLevelOfLastItemUnlock(level);
            var (item, upperBound) = ProgressUtils.GetNextItemToUnlock(level, LevelMultiplier);

            float unlockProgress = 0f;
            float unlockDoneProgress = 0f;

            if (upperBound > lowerBound)
            {
                unlockProgress = (float)(level - lowerBound) / (float)(upperBound - lowerBound);
                unlockDoneProgress = (float)((level + 1) - lowerBound) / (float)(upperBound - lowerBound);
            }

            return new UnlockInfo
            {
                unlockProgressRatio = unlockProgress,
                nextItem = item,
                unlockDoneProgressRatio = unlockDoneProgress
            };
        }

        public ItemStatus GetNailShapeStatus(Punishment nailShape)
        {
            foreach (var (item, level) in Config.ITEM_UNLOCK_LEVELS)
            {
                if (item.IsPunishment() && item.punishment == nailShape)
                    return (CurrentLevel < level) ? ItemStatus.Locked : ItemStatus.Unlocked;
            }

            Debug.Assert(false);
            return ItemStatus.Locked;
        }

        public ItemStatus GetPowderStatus(Powder powder)
        {
            foreach (var (item, level) in Config.ITEM_UNLOCK_LEVELS)
            {
                if (item.IsPowder() && item.Powder == powder)
                    return (CurrentLevel < level) ? ItemStatus.Locked : ItemStatus.Unlocked;
            }

            Debug.Assert(false);
            return ItemStatus.Locked;
        }

        public bool SFX_ON
        {
            get => mData.sfx_on;

            set
            {
                mData.sfx_on = value;
                Save();
            }
        }

        public bool HapticsStatus
        {
            get => mData.haptics_on;

            set
            {
                mData.haptics_on = value;
                Save();
            }
        }
        public int InteriorLevel
        {
            get => mData.interiorLevel;

            set
            {
                Debug.Assert(value > 0);
                mData.interiorLevel = value;
                Save();
                OnSalonUpgraded?.Invoke();
            }
        }

        public int WallLevel
        {
            get => mData.wallLevel;

            set
            {
                Debug.Assert(value > 0);
                mData.wallLevel = value;
                Save();
                OnSalonUpgraded?.Invoke();
            }
        }

        public int FloorLevel
        {
            get => mData.floorLevel;

            set
            {
                Debug.Assert(value > 0);
                mData.floorLevel = value;
                Save();
                OnSalonUpgraded?.Invoke();
            }
        }

        public int AvatarGender
        {
            get => mData.avatarGender;

            set
            {
                Debug.Assert(value == 0 || value == 1);
                mData.avatarGender = value;
                Save();
            }
        }

        public int AvatarType
        {
            get => mData.avatarType;

            set
            {
                Debug.Assert(value > 0 && value < 4);
                mData.avatarType = value;
                Save();
            }
        }

        public bool IsAvatarSelected
        {
            get => mData.isAvatarSelected;

            set
            {
                mData.isAvatarSelected = value;
                Save();
            }
        }

        public void IncreamentRating(int val)
        {
            mData.cop_rating += val;
            Save();
        }

        public void DecreamentRating(int val)
        {
            mData.cop_rating -= val;
            Save();
        }

        public float CopRating
        {
            get => mData.cop_rating;

            set
            {
                mData.cop_rating = value;
                Save();
            }
        }

        public float CopPrevRating
        {
            get => mData.cop_prev_rating;

            set
            {
                mData.cop_prev_rating = value;
                Save();
            }
        }


        public bool WasBadDecision
        {
            get => mData.wasBadDecision;

            set
            {
                mData.wasBadDecision = value;
                Save();
            }
        }

        public int GetInteriorPrice()
        {
            return GetUpgradePrice(InteriorLevel, Config.INTERIOR_PRICES);
        }

        public int GetWallPrice()
        {
            return GetUpgradePrice(WallLevel, Config.WALL_PRICES);
        }

        public int GetFloorPrice()
        {
            return GetUpgradePrice(FloorLevel, Config.FLOOR_PRICES);
        }

        public string PATH
        {
            get => mData.data_path;
        }

        private static int GetUpgradePrice(int currentUpgradeLevel, int[] prices)
        {
            Debug.Assert(currentUpgradeLevel > 0);
            --currentUpgradeLevel;

            if (currentUpgradeLevel < prices.Length)
                return prices[currentUpgradeLevel];
            else
                return prices[prices.Length - 1];
        }

        public static Progress Instance
        {
            get
            {
                if (smInstance == null)
                    smInstance = new Progress();

                return smInstance;
            }
        }

        private Progress()
        {
            mData = JsonUtility.FromJson<ProgressData>(PlayerPrefs.GetString(PROGRESS_KEY, "{}"));
        }

        private void Save()
        {
            PlayerPrefs.SetString(PROGRESS_KEY, JsonUtility.ToJson(mData));
        }

        private ProgressData mData;
        private static Progress smInstance;

        private const string PROGRESS_KEY = "progress";
    }

    class ProgressData
    {
        public int currency;
        public int currentLevel = 1;
        public int level_multiplier = 0;

        //RapBattle
        public string socialHandleName = "UpcomingRapper";
        public int followers = 0;
        public string displayPic = string.Empty;
        public Vector2 dpSize = new Vector2(1, 1);

        public bool tapSmashTut = false;

        // Salon upgrade level
        public int interiorLevel = 1;
        public int wallLevel = 1;
        public int floorLevel = 1;

        public int avatarGender = 1;       // 0 - Female, 1 - Male
        public int avatarType = 1;
        public bool isAvatarSelected = false;

        public bool sfx_on = true;
        public bool haptics_on = true;

        public string data_path = Application.persistentDataPath;

        public bool isMugShotTutDone;
        public bool isfoodPlatingTutDone;
        public bool isIdStepTutDone, isLunchBoxStepTutDone, isBribeStepTutDone, isAudioExperianceDone;

        public float cop_rating = 50f;
        public float cop_prev_rating = 50f;

        public bool wasBadDecision;
    }
}
