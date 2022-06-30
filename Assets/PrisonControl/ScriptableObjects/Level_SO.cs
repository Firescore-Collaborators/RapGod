using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;


namespace PrisonControl
{
    [CreateAssetMenu(fileName = "New Level", menuName = "PrisonControl/Level", order = 51)]
    public class Level_SO : ScriptableObject
    {
        [Foldout("Prison")]
        [SerializeField]
        private int totalMiniLevels;
        [SerializeField]
        private LevelTypes[] levelTypes;

        [Foldout("Prison")]
        [SerializeField]
        private List<IDCheck_SO> iDCheckInfo;

        [Foldout("Prison")]
        [SerializeField]
        private List<LunchBox_SO> lunchBoxInfo;

        [Foldout("Prison")]
        [SerializeField]
        private List<SuitcaseBribe_SO> bribeBoxInfo;

        [Foldout("Prison")]
        [SerializeField]
        [Expandable]
        private List<VisitorSO> vistorsInfo;

        [Foldout("Prison")]
        [SerializeField]
        [Expandable]
        private List<VisitorInmatePair> visitorInmatePairs;

        [Foldout("Prison")]
        [SerializeField]
        private Interrogation_SO interrogation_SO;

        [Foldout("Prison")]
        [SerializeField]
        private Warden_SO warden_so;

        [Foldout("Prison")]
        [SerializeField]
        private List<JailEntry_SO> jailEntry_so;

        [Foldout("Prison")]
        [SerializeField]
        private List<FoodPlating_SO> foodPlating_SO;

        [Foldout("Prison")]
        [SerializeField]
        private Mugshot_SO mugshot_so;

        [Foldout("Prison")]
        [SerializeField]
        private PrisonYard_SO prisonYard_so;

        [Foldout("Prison")]
        [SerializeField]
        private CCTVMonitor_SO cCTVMonitor_SO;

        [Foldout("Prison")]
        [SerializeField]
        private SlapAndRun_SO slapAndRun_SO;

        [Foldout("Prison")]
        [SerializeField]
        private CellCheck_SO cellChec_SO;

        [Foldout("Prison")]
        [SerializeField]
        private Guest_SO[] guests;

        [Foldout("RapBattle")]
        [Expandable]
        [SerializeField]
        private RapBattleDataSO rapBattle_SO;

        [Foldout("RapBattle")]
        [Expandable]
        [SerializeField]
        private NarrationSO narration_SO;

        public string badResponse;
        public string goodResponse;
        public string commonResponse;

        public enum LevelTypes
        {
            rapBattle,
            Narration,
            IDCheck,
            LunchBox,   
            BribeBox,
            PhoneBooth,
            Interrogation,
            Warden,
            JailEntry,
            MugShot,
            FoodPlating,
            PrisonYard,
            CCTVMonitor,
            CellCheck,
            SlapAndRun
        }

        public int TotalMiniLevels
        {
            get
            {
                return totalMiniLevels;
            }
        }

        public LevelTypes[] GetLevelTypes
        {
            get
            {
                return levelTypes;
            }
        }

        public List<IDCheck_SO> GetIDCheckInfo
        {
            get
            {
                return iDCheckInfo;
            }
        }

        public List<LunchBox_SO> GetLunchBoxInfo
        {
            get
            {
                return lunchBoxInfo;
            }
        }

        public List<SuitcaseBribe_SO> GetBribeBoxInfo
        {
            get
            {
                return bribeBoxInfo;
            }
        }



        public Interrogation_SO GetInterrogationInfo
        {
            get
            {
                return interrogation_SO;
            }
        }

        public List<VisitorInmatePair> GetVisitorInmatePairs
        {
            get
            {
                return visitorInmatePairs;
            }
        }

        public List<VisitorSO> GetVisitorsInfo
        {
            get
            {
                return vistorsInfo;
            }
        }

        public Warden_SO GetWardenInfo
        {
            get
            {
                return warden_so;
            }
        }

        public List<JailEntry_SO> GetJailEntryInfo
        {
            get
            {
                return jailEntry_so;
            }
        }

        public List<FoodPlating_SO> GetFoodPlatingInfo
        {
            get
            {
                return foodPlating_SO;
            }
        }

        public Mugshot_SO GetMugShotInfo
        {
            get
            {
                return mugshot_so;
            }
        }

        public PrisonYard_SO GetPrisonYardInfo
        {
            get
            {
                return prisonYard_so;
            }
        }


        public CCTVMonitor_SO GetCCTVMonitorInfo
        {
            get
            {
                return cCTVMonitor_SO;
            }
        }

        public SlapAndRun_SO GetSlapAndRunInfo
        {
            get
            {
                return slapAndRun_SO;
            }
        }

        public CellCheck_SO GetCellCheckInfo
        {
            get
            {
                return cellChec_SO;
            }
        }

        
        public Guest_SO[] Guests
        {
            get
            {
                return guests;
            }
        }

        public RapBattleDataSO GetRapBattleSO
        {
            get
            {
                return rapBattle_SO;
            }
        }

        public NarrationSO GetNarrationSO
        {
            get
            {
                return narration_SO;
            }
        }
    }
}

