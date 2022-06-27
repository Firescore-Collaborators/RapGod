using UnityEngine;
using UnityEngine.UI;


namespace PrisonControl
{
    public class ItemUnlockUi : MonoBehaviour
    {
        [SerializeField]
        private RewardPhasesControl _mRewardPhasesControl;

        [SerializeField]
        private Text txt_unlock;

        [SerializeField]
        private Image punishment_icon;

        [SerializeField]
        private AudioClip aud_spider, aud_spit, aud_lowBlow, aud_hammerHit, aud_chickenDance; 

        private void OnEnable()
        {
            Item unlockedItem = ProgressUtils.GetItemToUnlockOnLevel((Progress.Instance.CurrentLevel - 1), Progress.Instance.LevelMultiplier);
            txt_unlock.text = unlockedItem.punishment.ToString();

            punishment_icon.sprite = Resources.Load("PunishmentIcons/" + unlockedItem.punishment, typeof(Sprite)) as Sprite;

            Timer.Delay(1, () =>
            {
                if(unlockedItem.punishment ==  Punishment.SpiderBucket)
                    GetComponent<AudioSource>().clip = aud_spider;
                else if (unlockedItem.punishment == Punishment.Spit)
                    GetComponent<AudioSource>().clip = aud_spit;
                else if (unlockedItem.punishment == Punishment.LowBlow)
                    GetComponent<AudioSource>().clip = aud_lowBlow;
                else if (unlockedItem.punishment == Punishment.HammerHit)
                    GetComponent<AudioSource>().clip = aud_hammerHit;
                else if (unlockedItem.punishment == Punishment.ChickenDance)
                    GetComponent<AudioSource>().clip = aud_chickenDance;

                GetComponent<AudioSource>().Play();
            });

        }

        public void AudDelay()
        {
            
        }

        public void Close()
        {
            _mRewardPhasesControl._OnPhaseComplete();
        }
    }
}
