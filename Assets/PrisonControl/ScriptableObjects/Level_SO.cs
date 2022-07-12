﻿using System.Collections;
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

        public enum LevelTypes
        {
            rapBattle,
            Narration,
            ProfileDp,
            HandShake,
            AgentSelect,
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

    }
}

