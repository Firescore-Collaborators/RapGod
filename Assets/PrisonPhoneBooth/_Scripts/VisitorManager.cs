using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using TMPro;


 namespace PrisonControl
 {
    [System.Serializable]
    public class VisitorReferences
    {
        public Transform visitor;
        public Transform inmate;
        public GameObject dialogueBox;
        public Transform susTransform;
        public TypewriterEffect text
        {
            get{
                return dialogueBox.transform.GetChild(0).GetComponent<TypewriterEffect>();
            }
        }
    }

    public class VisitorManager : MonoBehaviour
    {
        public List<VisitorSO> visitors = new List<VisitorSO>();
        [Expandable]
        public List<VisitorInmatePair> visitorInmatePairs = new List<VisitorInmatePair> ();
        public List<VisitorReferences> visitorReferences = new List<VisitorReferences>();

        public PlayPhasesControl mPlayPhaseControl;
        public int visitorIndex = 0;
        int VisitorIndex 
        {
            get{
                return visitorIndex;
            }
            set{
                visitorIndex = value;
                dialogueIndex = 0;
            }
        }
        int dialogueIndex;
        public float dialogueTime = 5f;

        bool checkBlend;

        public static event System.Action OnVisitorPicked;
        public static event System.Action OnPlayDialogue;
        public static event System.Action OnLevelComplete;
        public static event System.Action OnSuspiciousStart;
        public static event System.Action On2ndTextCompleted;
        public static event System.Action <int>OnTextCompleted;

        [SerializeField]
        private CopHandler copHandler;

        VisitorSO currentVisitor;

        [SerializeField]
        private Transform [] copPosIn;

        [SerializeField]
        private Transform copPos_out;

        [SerializeField]
        private Transform guestPos_out;

        [SerializeField]
        private GameObject [] chair;

        private List<int> visitor_animationUsed;
        private List<int> inmate_animationUsed;


        private string[] visitor_animations = { "sit_talk_loop1","sit_talk_loop2", "Sitting2" };
        private string[] inmate_animations = { "cry_sit_loop1", "Sitting_Upset2", "Sitting2" };

        [SerializeField]
        RespondMessage respondMsg;

        [SerializeField]
        private AudioManager audioManager;

        [Foldout("Audio Clips")]
        public AudioClip aud_arrest;

        private void Awake()
        {
            visitor_animationUsed = new List<int>();
            inmate_animationUsed = new List<int>();
        }

        public VisitorSO CurrentVisitor
        {
            get
            {
                return visitors[VisitorIndex];
            }
            set
            {
                currentVisitor = value;
            }
        }

        void OnEnable()
        {
            MiniGameStart();
            OnPlayDialogue += SpawnDialogueBox;
            UIElements.OnButtonClicked  += DialogueBoxOff;
            UIElements.OnSuspicious  += DialogueBoxOff;
            UIElements.OnAllow += NextVisitor;
            OnVisitorPicked += SetCurrentCamera;
            UIElements.OnAllow += PickVisitor; 
            UIElements.OnNextDialogue += NextDialogue;
            UIElements.OnNextDialogue += PlayDialogue;
            UIElements.OnArrestGuilty += OnArrestGuilty;
            UIElements.OnAllowedCorrect += OnAllowedCorrect;

            UIElements.OnArrest += NextVisitor;
            UIElements.OnArrest += Arrest;
            UIElements.OnSuspicious += OnSuspicious;
            OnSuspiciousStart += MoveDialogueBox;
            OnLevelComplete += LevelComplete;

            for (int j = 0; j < chair.Length; j++)
            {
                PlayAnim(chair[j].GetComponent<Animator>(), "ChairIdle", 0);
            }

            visitor_animationUsed.Clear();
            inmate_animationUsed.Clear();
        }

        void OnDisable()
        {
            OnPlayDialogue -= SpawnDialogueBox;
            UIElements.OnButtonClicked  -= DialogueBoxOff;
            UIElements.OnSuspicious  -= DialogueBoxOff;
            UIElements.OnAllow -= PickVisitor;
            UIElements.OnAllow -= NextVisitor;
            OnVisitorPicked -= SetCurrentCamera;
            UIElements.OnNextDialogue -= NextDialogue;
            UIElements.OnNextDialogue -= PlayDialogue;
            UIElements.OnArrestGuilty -= OnArrestGuilty;
            UIElements.OnAllowedCorrect -= OnAllowedCorrect;

            UIElements.OnArrest -= NextVisitor;
            UIElements.OnArrest -= Arrest;
            UIElements.OnSuspicious -= OnSuspicious;
            OnSuspiciousStart -= MoveDialogueBox;
            OnLevelComplete -= LevelComplete;
        }

        void MiniGameStart()
        {
            InitLevelInfo();
            InitVisitors();
            Timer.Delay(1.0f, () =>
            {
                PickVisitor();
            });
        }

        void Update()
        {
            if(checkBlend)
            {
                if(!CameraController.instance.isBlending)
                {
                    checkBlend = false;
                    PlayDialogue();
                }
            }
        }

        private void Reset() {
            VisitorIndex = 0;

            for(int i = 0; i < visitorReferences.Count; i++)
            {
                Destroy(visitorReferences[i].visitor.transform.GetChild(0).gameObject);
                Destroy(visitorReferences[i].inmate.transform.GetChild(0).gameObject);
                visitorReferences[i].visitor.transform.rotation = Quaternion.Euler(0,90,0);
            }

            for(int j = 0 ; j < chair.Length; j++)
            {
                PlayAnim(chair[j].GetComponent<Animator>(),"ChairIdle",0);
            }


        }
        /*void InitVisitors()
        {
            visitors.Clear();
            for(int i = 0; i < transform.childCount; i++)
            {
                visitors.Add(transform.GetChild(i).GetComponent<Visitor>());
            }
        }*/

        /*[Button]
        void PickVisitor()
        {
            if(visitors.Count <= 0) {
                OnLevelConmplete?.Invoke();
                return;
            }

            if(CurrentVisitor != null)
            {
                if(visitors.Count == 1) 
                {
                    CurrentVisitor = visitors[0];
                }
                else
                {
                    List<Visitor> randomList = new List<Visitor>();
                    for(int i = 0; i < visitors.Count; i++)
                    {
                        randomList.Add(visitors[i]);
                    }
                    randomList.Remove(CurrentVisitor);
                    CurrentVisitor = RandomVisitor(randomList);
                }
            }
            else{
                CurrentVisitor = RandomVisitor(visitors);
            }

            CurrentVisitor.OnVisitorPicked?.Invoke();
            Timer.Delay(0.1f, () =>
            {
                checkBlend = true;
            });
        }*/

        void InitLevelInfo()
        {
            visitorInmatePairs.Clear();
            visitors.Clear();
            for(int i = 0; i <mPlayPhaseControl.levels[Progress.Instance.CurrentLevel-1].GetVisitorInmatePairs.Count; i++ )
            {
                visitorInmatePairs.Add(mPlayPhaseControl.levels[Progress.Instance.CurrentLevel-1].GetVisitorInmatePairs[i]);
            }
            for(int j = 0; j < mPlayPhaseControl.levels[Progress.Instance.CurrentLevel-1].GetVisitorsInfo.Count; j++)
            {
                visitors.Add(mPlayPhaseControl.levels[Progress.Instance.CurrentLevel-1].GetVisitorsInfo[j]);
            }
        }
        void InitVisitors()
        {
            for(int i = 0; i < visitorInmatePairs.Count; i++)
            {
                GameObject visitor = Instantiate(visitorInmatePairs[i].visitor);
                GameObject inmate = Instantiate(visitorInmatePairs[i].inmate);
                ResetToParent(visitorReferences[i].visitor,visitor.transform);
                ResetToParent(visitorReferences[i].inmate,inmate.transform);

                int randVal = Random.Range(0, 3);
                if (randVal == 0)
                    PlayAnim(visitor.GetComponent<Animator>(), "sit_talk_loop1", 0);
                else if (randVal == 1)
                    PlayAnim(visitor.GetComponent<Animator>(), "sit_talk_loop2", 0);
                else 
                    PlayAnim(visitor.GetComponent<Animator>(), "Sitting2", 0);

                randVal = Random.Range(0, 3);

                if (randVal == 0)
                {
                    PlayAnim(inmate.GetComponent<Animator>(), "cry_sit_loop1", 0);
                }else if (randVal == 1)
                {
                    PlayAnim(inmate.GetComponent<Animator>(), "Sitting_Upset2", 0);
                }
                else if (randVal == 2)
                {
                    PlayAnim(inmate.GetComponent<Animator>(), "Sitting2", 0);
                }

                GetNextVisitorAnim(visitor);
                GetNextInmateAnim(inmate);
            }
        }
        

        void GetNextVisitorAnim(GameObject visitor)
        {
            int randVal;

            do
            {
                randVal = Random.Range(0, visitor_animations.Length);
                PlayAnim(visitor.GetComponent<Animator>(), visitor_animations[randVal], 0);
            }
            while (visitor_animationUsed.Contains(randVal));

            visitor_animationUsed.Add(randVal);
        }

        void GetNextInmateAnim(GameObject inmate)
        {
            int randVal;

            do
            {
                randVal = Random.Range(0, inmate_animations.Length);
                PlayAnim(inmate.GetComponent<Animator>(), inmate_animations[randVal], 0);
            }
            while (inmate_animationUsed.Contains(randVal));

            inmate_animationUsed.Add(randVal);
        }

        void OnSuspicious()
        {
            //CurrentVisitor.PlayAnim("Stand");
            PlayAnim(visitorReferences[VisitorIndex].visitor.transform.GetChild(0).GetComponent<Animator>(),"Stand", 0.2f);
            MoveCameraBack();

            chair[VisitorIndex].GetComponent<Animator>().SetTrigger("fall");

        }

        [Button]
        void PickVisitor()
        {
            if(VisitorIndex >= visitors.Count)
            {
                OnLevelComplete?.Invoke();
                return;
            }
            //Assing Current visitor


            OnVisitorPicked?.Invoke();

            //Assign if visior is guilty
            UIElements.instance.isGuilty = CurrentVisitor.isGuilty;

            //To check if camera is blended to position
            Timer.Delay(0.1f, () =>
            {
                checkBlend = true;
            });
        }

        void PlayDialogue()
        {
            //Check if current visitor dialogues over
        
            UIElements.instance.visitorOver = PlayCurrentDialogue();
        }

        void Arrest()
        {
            //Arrest Callback to go here
            Catch();

            //Pick next visitor after arrest
            Timer.Delay(2f, () =>
            {
                PickVisitor();
            });
        }

        void OnArrestGuilty(bool isGuilty)
        {
            if (isGuilty)
            {
                respondMsg.ShowCorrectMsg();
                print("Arrested guilty person");
                mPlayPhaseControl.correctAnswers++;
                Progress.Instance.IncreamentRating(3);
            }
            else
            {
                respondMsg.ShowWrongMsg();
                print("Arrested innocent person");
                Progress.Instance.DecreamentRating(3);

                if (visitorIndex == 2)
                {
                    Debug.Log("Bad decision");
                    Progress.Instance.WasBadDecision = true;
                }
            }
        }

        void OnAllowedCorrect(bool isGuilty)
        {
            if (!isGuilty)
            {
                respondMsg.ShowCorrectMsg();
                print("allowed innocent person");
                mPlayPhaseControl.correctAnswers++;
                Progress.Instance.IncreamentRating(3);
            }
            else
            {
                respondMsg.ShowWrongMsg();
                print("allowed guilty person");
                Progress.Instance.DecreamentRating(3);

                if (visitorIndex == 2)
                {
                    Progress.Instance.WasBadDecision = true;
                    Debug.Log("Bad decision");
                }
            }
        }

        void LevelComplete()
        {
            Timer.Delay(3, () =>
            {
                mPlayPhaseControl._OnPhaseFinished();
                Reset();
                print("Level Complete");
            });
        }

        [Button]
        void MoveCameraBack()
        {
            Transform currentCam = CameraController.instance.currentVCam.transform;
            Vector3 cameraZoomOut = currentCam.position - (currentCam.forward * 1f);
            LerpObjectPosition.instance.LerpObject(currentCam, cameraZoomOut, 0.5f, () =>
            {
                Transform toRotate = visitorReferences[VisitorIndex].visitor;
                Vector3 rotation = new Vector3(toRotate.rotation.eulerAngles.x,toRotate.rotation.eulerAngles.y+179f,toRotate.rotation.eulerAngles.z);
                LerpObjectRotation.instance.LerpObject(toRotate, Quaternion.Euler(rotation), 0.5f, () =>
                {
                    OnSuspiciousStart?.Invoke();
                });
            });
        }

        void MoveDialogueBox()
        {
            Transform toMove = visitorReferences[VisitorIndex].dialogueBox.transform;
            Transform target = visitorReferences[VisitorIndex].susTransform;
            toMove.transform.position = target.transform.position;
            toMove.transform.rotation = target.transform.rotation;
            toMove.transform.localScale = target.transform.localScale;
            toMove.gameObject.SetActive(true);
            //visitorReferences[VisitorIndex].text.FullText = CurrentVisitor.statementText;

            GetComponent<AudioSource>().clip = CurrentVisitor.aud_statement;
            GetComponent<AudioSource>().Play();

            visitorReferences[VisitorIndex].text.WholeText = CurrentVisitor.statementText;

            visitorReferences[VisitorIndex].text.ShowTextResponse(() =>
            {
                Debug.Log("text completed");
                OnTextCompleted?.Invoke(0);

            });

        }

        bool PlayCurrentDialogue()
        {
            OnPlayDialogue?.Invoke();
            return dialogueIndex >= CurrentVisitor.dialogues.Count-1? true : false;
        }
        void SetCurrentCamera()
        {
            CameraController.instance.SetCurrentCamera(VisitorIndex);
        }
    
        void SpawnDialogueBox()
        {
            visitorReferences[VisitorIndex].dialogueBox.SetActive(true);
            //visitorReferences[VisitorIndex].text.FullText = CurrentVisitor.dialogues[dialogueIndex];

            GetComponent<AudioSource>().clip = CurrentVisitor.aud_dialogs[dialogueIndex];
            GetComponent<AudioSource>().Play();

            visitorReferences[VisitorIndex].text.WholeText = CurrentVisitor.dialogues[dialogueIndex];

            visitorReferences[VisitorIndex].text.ShowTextResponse(() =>
            {
                Debug.Log("text completed "+ dialogueIndex);
                OnTextCompleted?.Invoke(dialogueIndex);
            });
        }

        void DialogueBoxOff()
        {
            visitorReferences[VisitorIndex].dialogueBox.SetActive(false);
        }

        void NextVisitor()
        {
            if(visitorIndex < 3)
                VisitorIndex++;
            Debug.Log("VisitorIndex "+ VisitorIndex);

        }

        void NextDialogue()
        {
            dialogueIndex++;
        }

        void ResetToParent(Transform parent, Transform curObj)
        {
            curObj.transform.parent = parent;
            curObj.transform.localPosition = Vector3.zero;
            curObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
          //  curObj.transform.localScale = Vector3.one;
        }

        void PlayAnim(Animator anim, string animName, float value)
        {
            anim.CrossFade(animName, value);
        }
        /*Visitor RandomVisitor(List<Visitor> visitors)
        {
            int randomIndex = Random.Range(0, visitors.Count);
            return visitors[randomIndex];
        }*/

        void Catch()
        {
            //Debug.Log("pppppppppppppp "+ visitorReferences[VisitorIndex - 1].visitor.transform.GetChild(0).name);
            visitorReferences[VisitorIndex - 1].visitor.transform.GetChild(0).GetComponent<Animator>().SetTrigger("scared");

            copHandler.ReturnSelectedAvatar().GetComponent<Animator>().SetTrigger("catch");

            audioManager.PlayAudio(aud_arrest);

            LerpObjectPosition2.instance.LerpObject(copHandler.ReturnSelectedAvatar().transform, copPosIn[visitorIndex - 1].position, 0.5f, () =>
            {
                Debug.Log("cop going in");

                Timer.Delay(0.5f, () =>
                {
                    ArrestOut();
                });
            });
        }

        void ArrestOut()
        {
            LerpObjectPosition2.instance.LerpObject(copHandler.ReturnSelectedAvatar().transform, copPos_out.position, 0.5f, () =>
            {
                Debug.Log("cop going out");
            });

            LerpObjectPosition.instance.LerpObject(visitorReferences[VisitorIndex - 1].visitor.transform.GetChild(0).transform, copPos_out.position, 0.5f, () =>
            {
                Debug.Log("guest going out");

                //Timer.Delay(0.5f, () =>
                //{
                //    _mPlayPhasesControl._OnPhaseFinished();
                //});

            });
        }

       
    }
}
