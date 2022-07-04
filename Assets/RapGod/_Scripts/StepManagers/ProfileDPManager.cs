using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using System;

namespace PrisonControl
{
    public class ProfileDPManager : MonoBehaviour
    {
        [SerializeField]
        private PlayPhasesControl _mPlayPhasesControl;

        [SerializeField]
        [Foldout("Text")]
        private Text handleText, followerText, followerIncreaseText, option1, option2;

        [SerializeField]
        [Foldout("Button")]
        private Button option1Button, option2Button, nextButton;

        [SerializeField]
        [Foldout("Image")]
        private Image profileImage;
        [SerializeField]
        private TypewriterEffect typeWriter;

        [SerializeField]
        private GameObject profileListPanel, statusBody, optionPanel, postedStamp, followersIncreasePanel;

        [SerializeField]
        private ProfileDpSO profileDpSO;
        float currentFollowerCount;
        string postTextDefault = "Type your status...";


        void OnEnable()
        {
            InitLevelData();
            Init();
            
        }

        void Init()
        {
            MainCameraController.instance.SetCurrentCamera("SocialMedia", 0);
        }

        void InitLevelData()
        {
            handleText.text = "@" + Progress.Instance.SocialHandleName;
            followerText.text = Progress.Instance.FollowerCount + " Followers";
            LoadDp();
            profileDpSO = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetProfileDpSO;
            AssingDPToUI();
            currentFollowerCount = Progress.Instance.FollowerCount;
            //Enabling dp selection panel
            Timer.Delay(1, () =>
            {
                profileListPanel.transform.parent.gameObject.SetActive(true);
            });
        }

        void AssingDPToUI()
        {
            for (int i = 0; i < profileListPanel.transform.childCount; i++)
            {
                Texture2D tex = profileDpSO.profileDpList[i];
                Sprite dp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                profileListPanel.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = dp;
            }
        }

        [Button]
        public void SaveDp()
        {
            nextButton.gameObject.SetActive(false);
            profileListPanel.transform.parent.gameObject.SetActive(false);
            Texture2D tex = profileImage.sprite.texture;
            Progress.Instance.DisplayPicSize = new Vector2(tex.width, tex.height);
            Progress.Instance.DisplayPic = ConvertTextureToString(tex);
            Timer.Delay(1, () =>
            {
                AssingPostOptions();
            });
        }

        [Button]
        void LoadDp()
        {
            if (Progress.Instance.DisplayPic == string.Empty) { return; }

            Texture2D texture = new Texture2D((int)Progress.Instance.DisplayPicSize.x, (int)Progress.Instance.DisplayPicSize.y);
            Sprite img = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            profileImage.sprite = img;
            LoadTextureFromString(Progress.Instance.DisplayPic, texture);
        }

        string ConvertTextureToString(Texture2D texture)
        {
            Byte[] bytes = texture.EncodeToPNG();
            return Convert.ToBase64String(bytes);
        }

        void LoadTextureFromString(string loadString, Texture2D texture)
        {
            byte[] bytes = Convert.FromBase64String(loadString);
            texture.LoadImage(bytes);
        }

        public void OnDpSelectClick(Image img)
        {
            profileImage.sprite = img.sprite;
            nextButton.gameObject.SetActive(true);
        }

        [Button]
        public void ClearSavedImage()
        {
            Progress.Instance.DisplayPic = string.Empty;
        }

        //Post Gameplay
        void AssingPostOptions()
        {
            option1.text = profileDpSO.option1;
            option2.text = profileDpSO.option2;
            optionPanel.SetActive(true);
        }

        public void TypePost(bool positive)
        {
            typeWriter.WholeText = positive ? option1.text : option2.text;
            typeWriter.ShowTextResponse(() =>
            {
                Posted(positive);
            });
            optionPanel.SetActive(false);
        }

        void Posted(bool positive)
        {
            Timer.Delay(1.0f, () =>
            {
                handleText.transform.parent.gameObject.SetActive(false);
                followerText.transform.parent.gameObject.SetActive(false);
                statusBody.SetActive(false);
                postedStamp.SetActive(true);

                //New followers
                Timer.Delay(1.0f, () =>
                {
                    postedStamp.SetActive(false);
                    followersIncreasePanel.SetActive(true);
                    AddFollowerCount(positive);
                    IncreaseFollowerCount(Progress.Instance.FollowerCount);
                });
            });
        }

        void IncreaseFollowerCount(float changedValue)
        {
            LerpFloatValue.instance.LerpValue(currentFollowerCount, changedValue, 3.0f, (value) =>
            {
                followerIncreaseText.text = ((int)value) + "";
            }, () =>
            {
                Timer.Delay(1.0f, () =>
                {
                    LevelEnd();
                });
            });
        }

        void AddFollowerCount(bool positive)
        {
            Progress.Instance.FollowerCount += positive ? profileDpSO.followerCount.followerPositive : profileDpSO.followerCount.followerNegative;
        }

        [Button]
        void ClearFolloweCount()
        {
            Progress.Instance.FollowerCount = 0;
        }

        void LevelEnd()
        {
            handleText.transform.parent.gameObject.SetActive(true);
            followerText.transform.parent.gameObject.SetActive(true);
            statusBody.SetActive(true);
            followersIncreasePanel.SetActive(false);
            typeWriter.unity_text.text = postTextDefault;

            _mPlayPhasesControl._OnPhaseFinished();
        }

    }

}
