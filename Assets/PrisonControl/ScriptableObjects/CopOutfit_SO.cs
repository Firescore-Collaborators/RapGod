using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;


namespace PrisonControl
{
    [CreateAssetMenu(fileName = "CopOutfit", menuName = "PrisonControl/Outfit", order = 51)]
    public class CopOutfit_SO : ScriptableObject
    {
        public OutFitData maleOutfit,femaleOutfit;
        
        [Serializable]
        public class OutFitData
        {
            public List<Outfit> outfits = new List<Outfit>();
        }

        [Serializable]
        public  class Outfit
        {
            public int id;
            public Mesh body,bottom,hair,hat,shoes,Tie,Top;
            public Material body_mat, bottom_mat, hair_mat, hat_mat, shoes_mat, Tie_mat, Top_mat;
        }

    }
}

