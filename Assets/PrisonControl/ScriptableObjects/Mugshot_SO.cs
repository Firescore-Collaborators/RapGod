using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    [CreateAssetMenu(fileName = "Mugshot", menuName = "PrisonControl/Mugshot", order = 51)]
    public class Mugshot_SO : ScriptableObject
    {
        public int totalPrisnors;

        public Prisoner_SO[] prisoners;

        public string[] positiveResponse;
        public string[] negetiveResponse;

        public AnimationList[] positiveAnim;
        public AnimationList[] negetiveAnim;

    }
}
