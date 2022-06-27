using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PrisonControl
{
    public class PrisonYardScenario : MonoBehaviour
    {
        [HideInInspector]
        public int scenarioIndex;

        [SerializeField]
        private GameObject[]  prisoners;

        private int totalPrisoners, prisonersDied;

        [HideInInspector]
        public PrisonYardStep prisonYardStep;

        public bool isGuilty;

        private void Awake()
        {
            totalPrisoners = prisoners.Length;
        }
        public void Alert()
        {
            for(int i = 0; i < prisoners.Length; i++)
            {
                prisoners[i].GetComponent<PrisonYardAnimation>().HandsUp();
            }
        }

        public void BackToWork()
        {
            for (int i = 0; i < prisoners.Length; i++)
            {
                prisoners[i].GetComponent<PrisonYardAnimation>().BackToWork();
            }
        }

        public void OnPrisonerDied()
        {
            prisonersDied++;

            Debug.Log("prisonersDied "+ prisonersDied);

            if (prisonersDied >= totalPrisoners)
            {
                if(isGuilty)
                    prisonYardStep.MoveToNextScenario(true);
                else
                    prisonYardStep.MoveToNextScenario(false);
            }
        }
    }
}
