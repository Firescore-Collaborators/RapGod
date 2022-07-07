using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PrisonControl
{
    public class ScoringUi : MonoBehaviour
    {
        [SerializeField]
        private Text txt_comment, txt_coins, txt_levelsCompleted;

        private int correctAnswers, totalAnswers;

        [SerializeField]
        private GameObject pf_coin, coinTargetPosition, coinsSpawnPos, btn_continue;

        private List<GameObject> moving_coins = new List<GameObject>();
        private float startTime, journeyLength;
        private float time, speed;
        private int winAmount;

        [SerializeField]
        private PlayPhasesControl _mPlayPhasesControl;

        [SerializeField]
        private RewardPhasesControl _mRewardPhasesControl;

        // If level increment at gameplay end then  level_subtractAmt = 0,
        // If level increment at reward end then  level_subtractAmt = 1,
        int level_subtractAmt;

        [SerializeField]
        private TransitionPanel transitionPanel;

        AudioSource audioSource;

        [SerializeField]
        AudioClip aud_levelEnd, aud_levelEndTransitionl, aud_boxCompleted;

        [SerializeField]
        private Slider rating_slider;
        private bool copRatingStarted;
        float slideVal;
        float tempVal;
        float lerpTime;
        float lerpSpeed;

        [SerializeField]
        private Image img_powder, progressBar, rays;
        [SerializeField]
        private Text txt_itemName, txt_percentage;

        [SerializeField]
        private AudioSource audio_progress;

        [SerializeField]
        private GameObject levelComplete, copRating, boxProgress, comment, cashEarned, btnNext;
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();

        }
        void OnEnable()
        {
            txt_levelsCompleted.text = "Level "+ (Progress.Instance.CurrentLevel - 1 )+ " Completed";
            lerpSpeed = 3;

            rating_slider.value = Progress.Instance.CopPrevRating / 100;
            slideVal = Progress.Instance.CopRating / 100;

            rays.gameObject.SetActive(false);

            copRatingStarted = true;

            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[0] == Level_SO.LevelTypes.IDCheck ||
                _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[0] == Level_SO.LevelTypes.LunchBox ||
                _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[0] == Level_SO.LevelTypes.BribeBox)
            {
                RenderSettings.fogDensity = 0.08f;
            }else
            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[0] == Level_SO.LevelTypes.JailEntry)
            {
                RenderSettings.fogDensity = 0.04f;
            }
            else
            {
                RenderSettings.fogDensity = 0;
            }

            audioSource.clip = aud_levelEnd;
            audioSource.Play();

            // Use this for incrementing level no at scoring
            level_subtractAmt = 1;
            winAmount = 100;

            correctAnswers = _mPlayPhasesControl.correctAnswers;

            HideData();

            StartCoroutine(Steps());
        }

        void ProgressSlider()
        {
            if (copRatingStarted == false)
                return;

            rating_slider.value = Mathf.Lerp(rating_slider.value, slideVal, lerpTime);

            if (rating_slider.value != slideVal)
            {
                lerpTime += Time.deltaTime / lerpSpeed;
            }
            else
            {
                copRatingStarted = false;
                lerpTime = 0;
                Progress.Instance.CopPrevRating = slideVal * 100;
                // lerp complete
            }
        }

        private void Update()
        {
            ProgressSlider();
        }

        void HideData()
        {
            levelComplete.SetActive(false);
            copRating.SetActive(false);
            boxProgress.SetActive(false);
            comment.SetActive(false);
            cashEarned.SetActive(false);
            btnNext.SetActive(false);
        }
        void ShowComment()
        {
            comment.SetActive(true);

            int level;

            if (Progress.Instance.CurrentLevel == 1)
            {
                level = 2;
            }
            else
            {
                level = Progress.Instance.CurrentLevel;
            }

            if (Progress.Instance.WasBadDecision)
                txt_comment.text = _mPlayPhasesControl.levels[level - 1 - level_subtractAmt].badResponse;
            else
            {
                if (Random.Range(0, 2) == 0)
                    txt_comment.text = _mPlayPhasesControl.levels[level - 1 - level_subtractAmt].goodResponse;
                else
                    txt_comment.text = _mPlayPhasesControl.levels[level - 1 - level_subtractAmt].commonResponse;
            }

        }

        IEnumerator Steps()
        {
            levelComplete.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            copRatingStarted = true;
            copRating.SetActive(true);

            yield return new WaitForSeconds(1f);

            UnlockInfo info = Progress.Instance.GetNextUnlockInfo(Progress.Instance.CurrentLevel - 1);

            if (info.nextItem.IsPunishment())
            {
                StartCoroutine(ShowBoxProgress(info));
                yield return new WaitForSeconds(1f);
            }

            ShowComment();

            yield return new WaitForSeconds(1);

            //SpawnCoins();

            yield return new WaitForSeconds(1f);

        //    btnNext.SetActive(true);
        }

        IEnumerator ShowBoxProgress(UnlockInfo info)
        {
            boxProgress.SetActive(true);

            float filAmt = info.unlockDoneProgressRatio;
            float filler = info.unlockProgressRatio;
            Text percentageText = txt_percentage.GetComponent<Text>();

            if (Progress.Instance.SFX_ON)
            {
                //     audio_progress.Play();
            }

            while (filler <= filAmt)
            {
                filler += 0.02f;
                if (filler >= 1)
                {
                    filler = 1;
                    percentageText.text = "" + (int)(filler * 100) + "%";
                    progressBar.fillAmount = filler;

                    rays.gameObject.SetActive(true);

                    audioSource.clip = aud_boxCompleted;
                    audioSource.Play();
                    break;
                }
                percentageText.text = "" + (int)(filler * 100) + "%";
                progressBar.fillAmount = filler;
                yield return new WaitForSeconds(0.05f);
            }
        }

        void FixedUpdate()
        {
            if (moving_coins.Count > 0)
            {
                float distCovered = (Time.time - startTime) * speed;
                float fracJourney = distCovered / journeyLength;

                for (int i = 0; i < moving_coins.Count; i++)
                {
                    //     moving_coins[i].transform.position = Bezier2(moving_coins[i].transform.position, coinTargetPosition1.transform.position, coinTargetPosition.transform.position, fracJourney);

                    moving_coins[i].transform.position = Vector3.Lerp(moving_coins[i].transform.position, coinTargetPosition.transform.position, fracJourney);
                    moving_coins[i].transform.localScale = Vector3.Lerp(moving_coins[i].transform.localScale, moving_coins[i].transform.localScale / 1.1f, fracJourney);

                    if (Vector3.Distance(moving_coins[i].transform.position, coinTargetPosition.transform.position) < 1)
                    {
                        Destroy(moving_coins[i]);
                        moving_coins.RemoveAt(i);

                        btn_continue.SetActive(true);

                        //    vibration.TriggerLightImpact();
                    }
                }
            }
            else
            {
                //obj_coinPanel.GetComponent<Animator>().SetBool("bounce", false);
            }
        }



        public void SpawnCoins()
        {
            txt_coins.text = "" + winAmount;

            cashEarned.SetActive(true);

            speed = 120;

            startTime = Time.time;
            StartCoroutine(SpawnCoinsAnimate());
        }

        IEnumerator SpawnCoinsAnimate()
        {
            yield return new WaitForSeconds(.3f);

            yield return new WaitForSeconds(.5f);

            //AudioController.instance.PlayAudio(aud_coins);

            journeyLength = Vector3.Distance(coinsSpawnPos.transform.position, coinTargetPosition.transform.position);

            Vector3 spawnPos;


            for (int i = 0; i < 10; i++)
            {
                spawnPos = new Vector3(coinsSpawnPos.transform.position.x, coinsSpawnPos.transform.position.y + 50, coinsSpawnPos.transform.position.z);
                GameObject coin_obj = Instantiate(pf_coin, spawnPos, Quaternion.identity, transform);
                coin_obj.transform.localScale = Vector3.one;
                moving_coins.Add(coin_obj);

                spawnPos = new Vector3(coinsSpawnPos.transform.position.x, coinsSpawnPos.transform.position.y - 50, coinsSpawnPos.transform.position.z);
                coin_obj = Instantiate(pf_coin, spawnPos, Quaternion.identity, transform);
                coin_obj.transform.localScale = Vector3.one;
                moving_coins.Add(coin_obj);

                //vibration.TriggerLightImpact();

                yield return new WaitForSeconds(0.1f);
                //obj_coinPanel.GetComponent<Animator>().SetBool("bounce", true);

            }

            //if (isRewardClaimed)
            //{
            //    isRewardClaimed = false;
            //    yield return new WaitForSeconds(1.3f);
            //    _rewardPhasesControl._OnPhaseComplete();
            //}

            //obj_coinPanel.GetComponent<Animator>().SetBool("bounce", false);

            Progress.Instance.Currency += winAmount;
        }

        public void Close()
        {
            //transitionPanel.DoTransition(() =>
            //{
            //    _mRewardPhasesControl._OnPhaseComplete();
            //}
            //);
            _mRewardPhasesControl._OnPhaseComplete();
        }
    }
}