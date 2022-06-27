using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    [CreateAssetMenu(fileName = "CCTVMonitor", menuName = "PrisonControl/CCTVMonitor", order = 51)]
    public class CCTVMonitor_SO : ScriptableObject
    {
        public CCTV_Set [] pf_set;
    }
}
