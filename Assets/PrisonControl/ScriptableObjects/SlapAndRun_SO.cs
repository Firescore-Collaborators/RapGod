using UnityEngine;

namespace PrisonControl
{
    [CreateAssetMenu(fileName = "SlapAndRun", menuName = "PrisonControl/SlapAndRun", order = 51)]
    public class SlapAndRun_SO : ScriptableObject
    {
        public GameObject pf_level;

        public string wardenText;
    }
}

