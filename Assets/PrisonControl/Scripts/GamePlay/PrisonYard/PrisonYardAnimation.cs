using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PrisonControl
{
    public class PrisonYardAnimation : MonoBehaviour
    {
        public string animation_name;

        [HideInInspector]
        public float totalHealth, health;

        [SerializeField]
        private Image healthBar;

        bool isDead;

        public bool isCrawling;

        PrisonYardScenario prisonYardScenario;

        private void Awake()
        {
            GetComponent<Animator>().Play(animation_name);
            prisonYardScenario = transform.parent.GetComponent<PrisonYardScenario>();
        }
        void Start()
        {
            healthBar.transform.parent.parent.gameObject.SetActive(false);

            health = 8;
            totalHealth = health;
        }

        private void Update()
        {
            if (isDead)
                return;

            healthBar.fillAmount = (health / totalHealth);

            if (health <= 0)
            {
                Death();
            }
        }

        public void HandsUp()
        {
            //if (isCrawling) {
            //    GetComponent<Animator>().Play("ClimbingUpWallHalt");
            //}
            //else
            //{
            //    GetComponent<Animator>().Play("HandsUp");
            //}
        }

        public void BackToWork()
        {
            GetComponent<Animator>().Play(animation_name);
        }

        void Death()
        {
            GetComponent<BoxCollider>().enabled = false;
            isDead = true;
            GetComponent<Animator>().applyRootMotion = true;

            GetComponent<Animator>().SetTrigger("death");
            healthBar.transform.parent.parent.gameObject.SetActive(false);

            prisonYardScenario.OnPrisonerDied();
        }

        public void GetHit()
        {
            if (isDead)
                return;
            healthBar.transform.parent.parent.gameObject.SetActive(true);
            health--;

            int randNo = Random.Range(1, 3);
            GetComponent<Animator>().Play("BulletHit" + randNo);
        }
    }
}
