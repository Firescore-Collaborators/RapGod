using UnityEngine;

namespace PrisonControl
{
    public class CCTV_Animation : MonoBehaviour
    {
        public string animation_name;

        public void PlayDefaultAnim()
        {
            GetComponent<Animator>().Play(animation_name);
        }

        public void PlayAnim(string anim)
        {
            GetComponent<Animator>().applyRootMotion = true;
            GetComponent<Animator>().Play(anim);
        }
    }
}
