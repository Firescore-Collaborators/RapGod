using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

namespace PrisonControl
{
    public class Avatar : MonoBehaviour
    {
        // Avatar Gender 0 - Female, 1 - Male
        public int GenderID;

        [SerializeField]
        private AvatarSelectionState avatarSelectionState;

        [SerializeField]
        private GameObject [] uniform;

        public void OnAvatarNext()
        {
            Progress.Instance.AvatarType++;
            SetUniform();
        }

        public void OnAvatarPrevious()
        {
            Progress.Instance.AvatarType--;
            SetUniform();
        }
        private void OnMouseDown()
        {
            //if (!IsMouseOverUI())
            //{
                avatarSelectionState.OnAvatarClicked(GenderID);
            //}
        }

        public void SetUniform()
        {
            foreach (GameObject obj in uniform)
            {
                obj.SetActive(false);
            }

            Debug.Log("Progress.Instance.AvatarType "+ Progress.Instance.AvatarType);
            uniform[Progress.Instance.AvatarType - 1].SetActive(true);
        }

        public bool IsMouseOverUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        public void MoveAhead()
        {
            GetComponent<Animator>().SetBool("walk", true);
        }

        public void MoveBack()
        {
            GetComponent<Animator>().SetBool("walk", false);
        }

        public void Celebrate()
        {
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<Animator>().SetBool("celebrate", true);
        }

        public void WalkOut()
        {
            GetComponent<Animator>().SetBool("walkOut", true);
        }

        public void ArrestOut()
        {
          //  copManager.ArrestOut();
        }
    }
}
