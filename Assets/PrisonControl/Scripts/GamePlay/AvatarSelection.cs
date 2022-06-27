using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    public class AvatarSelection : MonoBehaviour
    {
        [SerializeField]
        private Avatar avatar1, avatar2;

        [SerializeField]
        AvatarSelectionUi mAvatarUi;

        private void Awake()
        {
            mAvatarUi._OnAvatarNext += OnAvatarNext;
            mAvatarUi._OnAvatarPrevious += OnAvatarPrevious;
        }

        private void OnDestroy()
        {
            mAvatarUi._OnAvatarNext -= OnAvatarNext;
            mAvatarUi._OnAvatarPrevious -= OnAvatarPrevious;
        }

        public void SelectAvatar(int id)
        {
            if (id == 0)
            {
                avatar1.MoveAhead();
                avatar2.MoveBack();
            }
            else
            {
                avatar2.MoveAhead();
                avatar1.MoveBack();
            }

        }

        public void OnAvatarSelected(int id)
        {
            if (id == 0)
            {
                avatar2.GetComponent<BoxCollider>().enabled = false;
                avatar1.Celebrate();
                avatar2.WalkOut();
            }
            else
            {
                avatar1.GetComponent<BoxCollider>().enabled = false;
                avatar2.Celebrate();
                avatar1.WalkOut();
            }
        }

        public void OnAvatarNext()
        {
            Debug.Log("---Progress.Instance.AvatarGender "+ Progress.Instance.AvatarGender);
            if (Progress.Instance.AvatarGender == 0)
            {
                avatar1.OnAvatarNext();
            }
            else 
            {
                avatar2.OnAvatarNext();
            }
        }

        public void OnAvatarPrevious()
        {
            if (Progress.Instance.AvatarGender == 0)
            {
                avatar1.OnAvatarPrevious();
            }
            else if (Progress.Instance.AvatarGender == 1)
            {
                avatar2.OnAvatarPrevious();
            }
        }
    }
}

