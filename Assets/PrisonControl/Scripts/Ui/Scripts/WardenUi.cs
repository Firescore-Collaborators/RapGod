using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace PrisonControl
{
    public class WardenUi : MonoBehaviour
    {
        public GameObject step1_panel, step2_panel, step3_panel, takeSnapShotPanel, snapShotPreviewPanel;

        public Action conversationStarted;
        public Action<bool> step1Response;
        public Action<bool> step2Response;
        public Action<bool> step3Response;

        public Action<bool> showWardenAnim;
        public Action onMugshotStarted;
        public Action onTinderBioStarted;
        public Action onTinderChatStarted;

        public Action onMugshotInitiated;
        public Action onPoseClicked;
        public Action onDanceClicked;
        public Action onMugshotDone;

        public Action onWardenReward;

        public Action spawnTool;
        public Action <int> endLevel;

        [SerializeField]
        private Text[] txt_positiveBtn;

        [SerializeField]
        private Text[] txt_negetiveBtn;

        [SerializeField]
        private TypewriterEffect[] txt_conversation;

        [SerializeField]
        private TypewriterEffect txt_negConversation;

        private Warden_SO wardenInfo;

        public int curr_conversation;

        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private AudioClip aud_camShutter;

        [SerializeField]
        private RawImage rawImg_snapShot;

        [SerializeField]
        private GameObject camFlash;


        public void Setup(Warden_SO _wardenInfo)
        {
            curr_conversation = 0;
            wardenInfo = _wardenInfo;
            for (int i = 0; i < 3; i++)
            {
                //txt_conversation[i].text = wardenInfo.conversation[i];
                txt_positiveBtn[i].text = wardenInfo.positiveResponse[i];
                txt_negetiveBtn[i].text = wardenInfo.negetiveResponse[i];
            }

            Invoke("ShowConversation1", 1);
        }

        // Conversation Response1
        public void Step1Btn1(bool isPositive)
        {
            if (isPositive)
                Progress.Instance.IncreamentRating(3);
            else
                Progress.Instance.DecreamentRating(3);

            step1_panel.SetActive(false);
            txt_conversation[0].transform.parent.gameObject.SetActive(false);


            if (wardenInfo.wardenGameType == Warden_SO.WardenGameType.TinderChat)
                onWardenReward?.Invoke();

            if (isPositive)
            {
                audioSource.clip = wardenInfo.aud_positiveResponse[0];
                audioSource.Play();

                Timer.Delay(audioSource.clip.length + 0.5f, () =>
                {
                    ShowConversation2();
                });
            }
            else
            {
                audioSource.clip = wardenInfo.aud_negetiveResponse[0];
                audioSource.Play();

                Timer.Delay(audioSource.clip.length + 0.5f, () =>
                {
                    ShowNegetiveConversation();
                    step1Response?.Invoke(isPositive);
                });
            }
        }

        // Conversation Response2
        public void Step2Btn1(bool isPositive)
        {
            if (isPositive)
                Progress.Instance.IncreamentRating(3);
            else
                Progress.Instance.DecreamentRating(3);

            step2_panel.SetActive(false);
            txt_conversation[1].transform.parent.gameObject.SetActive(false);

            if (isPositive)
            {
                audioSource.clip = wardenInfo.aud_positiveResponse[1];
                audioSource.Play();

                Timer.Delay(audioSource.clip.length + 0.5f, () =>
                {
                    if (wardenInfo.wardenGameType == Warden_SO.WardenGameType.Mugshot)
                    {
                        onMugshotStarted?.Invoke();
                        Invoke("ActivateSnapshotUi", 1.3f);
                    }
                    else if (wardenInfo.wardenGameType == Warden_SO.WardenGameType.TinderChat)
                    {
                        onTinderChatStarted?.Invoke();
                    }
                    else if (wardenInfo.wardenGameType == Warden_SO.WardenGameType.TinderBio)
                    {
                        onTinderBioStarted?.Invoke();
                    }
                    else
                    {
                        ShowConversation3();
                    }
                });
            }
            else
            {
                audioSource.clip = wardenInfo.aud_negetiveResponse[1];
                audioSource.Play();

                Timer.Delay(audioSource.clip.length + 0.5f, () =>
                {
                    if (wardenInfo.wardenGameType == Warden_SO.WardenGameType.Mugshot)
                    {
                        onMugshotStarted?.Invoke();
                        Invoke("ActivateSnapshotUi", 1.3f);
                    }
                    else if (wardenInfo.wardenGameType == Warden_SO.WardenGameType.TinderChat)
                    {
                        onTinderChatStarted?.Invoke();
                    }
                    else if (wardenInfo.wardenGameType == Warden_SO.WardenGameType.TinderBio)
                    {
                        onTinderBioStarted?.Invoke();
                    }
                    else
                    {
                        step2Response?.Invoke(isPositive);
                        ShowNegetiveConversation();
                    }
                });
            }
        }


        // Conversation Response3
        public void Step3Btn1(bool isPositive)
        {
            if (isPositive)
                Progress.Instance.IncreamentRating(3);
            else
                Progress.Instance.DecreamentRating(3);

            step3_panel.SetActive(false);
            txt_conversation[2].transform.parent.gameObject.SetActive(false);

            if (!isPositive)
            {
                if(wardenInfo.punishmentType == Punishment.None)
                {
                    audioSource.clip = wardenInfo.aud_negetiveResponse[2];
                    audioSource.Play();
                    Timer.Delay(audioSource.clip.length + 0.5f, () =>
                    {
                        ShowNegetiveConversation();
                        step3Response?.Invoke(isPositive);
                    });
                }
                else
                {
                    ShowNegetiveConversation();
                    step3Response?.Invoke(isPositive);

                }
            }
            else
            {
                audioSource.clip = wardenInfo.aud_positiveResponse[2];
                audioSource.Play();

                Timer.Delay(audioSource.clip.length + 0.5f, () =>
                {
                    step3Response?.Invoke(isPositive);
                });
                curr_conversation++;
            }

        }
        public void ShowConversation1()
        {
            txt_conversation[0].transform.parent.gameObject.SetActive(true);
            conversationStarted?.Invoke();
            txt_conversation[0].WholeText = wardenInfo.conversation[0];
            txt_conversation[0].ShowTextResponse(() =>
            {
                step1_panel.SetActive(true);
            });

            audioSource.clip = wardenInfo.aud_positiveConv[0];
            audioSource.Play();
        }

        public void ShowConversation2()
        {
            curr_conversation++;
            txt_conversation[1].transform.parent.gameObject.SetActive(true);
            txt_conversation[1].WholeText = wardenInfo.conversation[1];
            txt_conversation[1].ShowTextResponse(() =>
            {
                step2_panel.SetActive(true);
            });
            showWardenAnim?.Invoke(true);
            audioSource.clip = wardenInfo.aud_positiveConv[1];
            audioSource.Play();
        }

        public void ShowConversation3()
        {
            // Check for tool spawn condition
            spawnTool?.Invoke();

            curr_conversation++;
            txt_conversation[2].transform.parent.gameObject.SetActive(true);
            txt_conversation[2].WholeText = wardenInfo.conversation[2];
            txt_conversation[2].ShowTextResponse(() =>
            {
                step3_panel.SetActive(true);
            });
            showWardenAnim?.Invoke(true);
            audioSource.clip = wardenInfo.aud_positiveConv[2];
            audioSource.Play();
        }


        public void ShowNegetiveConversation()
        {
            txt_negConversation.transform.parent.gameObject.SetActive(true);

            txt_negConversation.WholeText = wardenInfo.negetive_conversation[curr_conversation];

            audioSource.clip = wardenInfo.aud_negetiveConv[curr_conversation];
            audioSource.Play();

            showWardenAnim?.Invoke(false);

            txt_negConversation.ShowTextResponse(() =>
            {
                if (curr_conversation == 0) {

                    Timer.Delay(2, () =>
                    {
                        txt_negConversation.transform.parent.gameObject.SetActive(false);
                        ShowConversation2();
                    });
                }
                else if (curr_conversation == 1)
                {
                    Timer.Delay(2, () =>
                    {
                        txt_negConversation.transform.parent.gameObject.SetActive(false);
                        ShowConversation3();
                    });
                }
                else if (curr_conversation == 2)
                {
                    Timer.Delay(2, () =>
                    {
                        txt_negConversation.transform.parent.gameObject.SetActive(false);

                    });
                }
            });
        }

        public void TakeMugshot()
        {
            camFlash.SetActive(true);
            audioSource.clip = aud_camShutter;
            audioSource.Play();

            onMugshotInitiated?.Invoke();
        }

        public void MugshotPose()
        {
            onPoseClicked?.Invoke();
        }

        public void DancePose()
        {
            onDanceClicked?.Invoke();
        }
        

        public void Recapture()
        {
            snapShotPreviewPanel.SetActive(false);
            ActivateSnapshotUi();
        }

        public void CloseMugShot()
        {
            snapShotPreviewPanel.SetActive(false);
            onMugshotDone?.Invoke();
        }

        public void ShowSnapShot(byte[] bytes)
        {
            takeSnapShotPanel.SetActive(false);
            snapShotPreviewPanel.SetActive(true);

            var texture = new Texture2D(73, 73);
            texture.LoadImage(bytes);

            rawImg_snapShot.texture = texture;
        }

        void ActivateSnapshotUi()
        {
            takeSnapShotPanel.SetActive(true);
        }

    }
}
