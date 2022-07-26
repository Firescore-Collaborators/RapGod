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
        [SerializeField]
        private LevelTypes[] levelTypes;

        [Foldout("RapBattle")]
        [Expandable]
        [SerializeField]
        private RapBattleDataSO rapBattle_SO;

        [Foldout("RapBattle")]
        [Expandable]
        [SerializeField]
        private NarrationSO narration_SO;

        [Foldout("RapBattle")]
        [Expandable]
        [SerializeField]
        private ProfileDpSO profileDp_SO;

        [Foldout("RapBattle")]
        [Expandable]
        [SerializeField]
        private HandShakeSO handShake_SO;

        [Foldout("RapBattle")]
        [Expandable]
        [SerializeField]
        private AgentsList_SO agentsList_SO;

        [Foldout("RapBattle")]
        [SerializeField]
        private GameObject signaturePrefab;

        [Foldout("RapBattle")]
        [SerializeField]
        private LyricDataSO lyricList;

        [Foldout("RapBattle")]
        [SerializeField]
        private TilesSO tiles_SO;

        [Foldout("RapBattle")]
        [SerializeField]
        private SliderSOList equalizer_SO;

        public enum LevelTypes
        {
            rapBattle,
            Narration,
            ProfileDp,
            HandShake,
            AgentSelect,
            Signature,
            Lyrics,
            BeatSort,
            Equalizer,
            GirlAudition
        }

        public RapBattleDataSO GetRapBattleSO
        {
            get
            {
                return rapBattle_SO;
            }
        }

        public LevelTypes[] GetLevelTypes
        {
            get
            {
                return levelTypes;
            }
        }
        public NarrationSO GetNarrationSO
        {
            get
            {
                return narration_SO;
            }
        }

        public ProfileDpSO GetProfileDpSO
        {
            get
            {
                return profileDp_SO;
            }
        }

        public HandShakeSO GetHandShakeSO
        {
            get
            {
                return handShake_SO;
            }
        }

        public AgentsList_SO GetAgentsListSO
        {
            get
            {
                return agentsList_SO;
            }
        }

        public GameObject GetSignaturePrefab
        {
            get
            {
                return signaturePrefab;
            }
        }

        public LyricDataSO GetLyricList
        {
            get
            {
                return lyricList;
            }
        }
        
        public TilesSO GetTilesSO
        {
            get
            {
                return tiles_SO;
            }
        }

        public SliderSOList GetEqualizerSO
        {
            get
            {
                return equalizer_SO;
            }
        } 
    }
}

