using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

namespace PrisonControl
{
    public class CCTVMonitorStep : MonoBehaviour
    {
        [SerializeField]
        private PlayPhasesControl _mPlayPhasesControl;

        [SerializeField]
        private CameraManager cameraManager;

        private CCTVMonitor_SO cCTVMonitor_SO;

        public int currentSet;

        [SerializeField]
        private List<CCTV_Set> list_sets;

        [SerializeField]
        private CCTVMonitorUI cCTVMonitorUI;

        [SerializeField]
        private CinemachineBrain mainCam, cctvCam;

        [SerializeField]
        private CinemachineVirtualCamera camCCTV2;

        [SerializeField]
        private GameObject fadeOut;

        [SerializeField]
        private GameObject pf_passwrdManager, panelPasswordmanager, RecordingFrame;

        private PasswordManager curr_passwordManager;

        [SerializeField]
        private Text txt_description;

        [SerializeField]
        private RespondMessage respondMessage;

        private void Awake()
        {
            list_sets = new List<CCTV_Set>();

            cCTVMonitorUI._OnPassClicked += OnPassClicked;
            cCTVMonitorUI._OnSuspiciousClicked += OnSuspiciousClicked;

            cCTVMonitorUI._OnIgnoreClicked += OnIgnoreClicked;
            cCTVMonitorUI._OnPunishmentClicked += OnPunishmentClicked;

        }

        private void OnDestroy()
        {
            cCTVMonitorUI._OnPassClicked -= OnPassClicked;
            cCTVMonitorUI._OnSuspiciousClicked -= OnSuspiciousClicked;

            cCTVMonitorUI._OnIgnoreClicked -= OnIgnoreClicked;
            cCTVMonitorUI._OnPunishmentClicked -= OnPunishmentClicked;
        }

        private void OnEnable()
        {
            RecordingFrame.SetActive(false);

            currentSet = 0;

            cameraManager.ActivateCamCCTV1(0);

            curr_passwordManager = Instantiate(pf_passwrdManager, panelPasswordmanager.transform).GetComponent<PasswordManager>();
            curr_passwordManager._OnPasswordDone += OnPasswordDone;

            StartStep();
        }

        void OnPasswordDone()
        {
            RecordingFrame.SetActive(true);
            RemoveCurrPasswordManager();
            StartMonitor();
        }

        public void StartStep()
        {
            StartCoroutine(Steps());
        }

        IEnumerator Steps()
        {
            for (int i = 0; i < list_sets.Count; i++)
            {
                Destroy(list_sets[i].gameObject);
            }

            list_sets.Clear();

            cameraManager.ActivateCamCCTV1(0);

            cCTVMonitor_SO = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetCCTVMonitorInfo;

            for (int i = 0; i < cCTVMonitor_SO.pf_set.Length; i++)
            {
                GameObject obj = Instantiate(cCTVMonitor_SO.pf_set[i].gameObject);
                obj.SetActive(false);
                list_sets.Add(obj.GetComponent<CCTV_Set>());
            }

            yield return new WaitForSeconds(1);

            cameraManager.ActivateCamCCTV2(1);

        }

        void StartMonitor()
        {
            //DeActivate main cam cinebrain and activate cctv cam cinebrain
            mainCam.m_DefaultBlend.m_Time = 0;
            cctvCam.m_DefaultBlend.m_Time = 0;

            cCTVMonitorUI.ShowDecisionPanel();
            mainCam.enabled = false;
            cctvCam.enabled = true;

            cctvCam.gameObject.SetActive(true);

            string punishment = list_sets[currentSet].GetComponent<CCTV_Set>().punishment.ToString();

            cCTVMonitorUI.punishmentText.text = punishment.ToString();
            cCTVMonitorUI.img_emoji.sprite = Resources.Load("PunishmentEmoji/" + punishment, typeof(Sprite)) as Sprite;

            txt_description.text = list_sets[currentSet].GetComponent<CCTV_Set>().description.ToString();

            list_sets[currentSet].gameObject.SetActive(true);
            list_sets[currentSet].virtual_cam.gameObject.SetActive(true);
            list_sets[currentSet].StartAnim();
        }

        void OnPassClicked()
        {
            if (list_sets[currentSet].isGuilty)
            {
                //if (currentSet == list_sets.Count)
                //{
                //    Progress.Instance.WasBadDecision = true;
                //    Debug.Log("Bad decision");
                //}
                Progress.Instance.DecreamentRating(2);
                Debug.Log("ShowWrongMsg");
                respondMessage.ShowWrongMsg();
            }
            else
            {
                Progress.Instance.IncreamentRating(2);
                Debug.Log("ShowCorrectMsg");
                respondMessage.ShowCorrectMsg();
            }

            Timer.Delay(1, () =>
            {
                Pass();
            });
        }

        void Pass()
        {
            currentSet++;

            if (currentSet >= list_sets.Count)
            {
                Timer.Delay(2, () =>
                {
                    for (int i = 0; i < list_sets.Count; i++)
                    {
                        Destroy(list_sets[i].gameObject);
                    }
                    list_sets.Clear();

                    _mPlayPhasesControl._OnPhaseFinished();
                });
            }
            else
            {
                list_sets[currentSet - 1].virtual_cam.gameObject.SetActive(false);
                list_sets[currentSet - 1].gameObject.SetActive(false);

                //   Destroy(list_sets[currentSet - 1].gameObject);

                list_sets[currentSet].gameObject.SetActive(true);
                list_sets[currentSet].virtual_cam.gameObject.SetActive(true);
                list_sets[currentSet].StartAnim();

                string punishment = list_sets[currentSet].GetComponent<CCTV_Set>().punishment.ToString();

                cCTVMonitorUI.punishmentText.text = punishment.ToString();
                cCTVMonitorUI.img_emoji.sprite = Resources.Load("PunishmentEmoji/" + punishment, typeof(Sprite)) as Sprite;
                txt_description.text = list_sets[currentSet].GetComponent<CCTV_Set>().description.ToString();

                cCTVMonitorUI.ShowDecisionPanel();
            }
        }

        void RemoveCurrPasswordManager() {
            curr_passwordManager._OnPasswordDone -= OnPasswordDone;
            Destroy(curr_passwordManager.gameObject);
        }


        void OnSuspiciousClicked()
        {
            list_sets[currentSet].ChangeFOV();
            fadeOut.SetActive(true);
            mainCam.enabled = true;
            cctvCam.enabled = false;
        }

        void OnIgnoreClicked()
        {
            Debug.Log("currentSet "+ currentSet);
            if (list_sets[currentSet].isGuilty)
            {
                respondMessage.ShowWrongMsg();
                Debug.Log("ShowWrongMsg");

                Progress.Instance.DecreamentRating(2);
                //if (currentSet == list_sets.Count)
                //{
                //    Progress.Instance.WasBadDecision = true;
                //    Debug.Log("Bad decision");
                //}
            }
            else
            {
                respondMessage.ShowCorrectMsg();
                Progress.Instance.IncreamentRating(2);
                Debug.Log("ShowCorrectMsg");
            }

            Timer.Delay(1, () =>
            {
                MoveToNext();

                Timer.Delay(1, () =>
                {
                    Pass();
                });
            });
        }

        void MoveToNext()
        {
            fadeOut.SetActive(true);
            camCCTV2.Priority = 11;
            list_sets[currentSet].ResetFOV();
            Timer.Delay(0.2f, () =>
            {
                mainCam.enabled = false;
                cctvCam.enabled = true;

                ResetCamPriority();
                list_sets[currentSet].virtual_cam.GetComponent<CinemachineVirtualCamera>().Priority = 11;
            });
        }

        void OnPunishmentClicked()
        {
            if (list_sets[currentSet].isGuilty)
            {
                respondMessage.ShowCorrectMsg();
                Debug.Log("ShowCorrectMsg");
                Progress.Instance.IncreamentRating(2);
            }
            else
            {
                respondMessage.ShowWrongMsg();
                Debug.Log("ShowWrongMsg");
                Progress.Instance.DecreamentRating(2);
                if (currentSet == list_sets.Count)
                {
                    Progress.Instance.WasBadDecision = true;
                    Debug.Log("Bad decision");
                }
            }

            list_sets[currentSet].PlayAnim();

            Invoke("MoveToNext", 1);

            Timer.Delay(3, () =>
            {
                Pass();
            });
        }

        void ResetCamPriority()
        {
            camCCTV2.Priority = 10;

            //for (int i = 0; i < list_sets.Count; i++)
            //{
            //    list_sets[i].virtual_cam.GetComponent<CinemachineVirtualCamera>().Priority = 10; ;
            //}

        }
    }
}