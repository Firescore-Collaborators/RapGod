using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CellData : MonoBehaviour
{
    public List<GameObject> hiddenObjects = new List<GameObject>();
    public Transform lerpPostion;
    public CinemachineVirtualCamera gameCamera1, gameCamera2;
    public GameObject tutorial;
    public List<Texture2D> hiddenObj_icons = new List<Texture2D>();

}
