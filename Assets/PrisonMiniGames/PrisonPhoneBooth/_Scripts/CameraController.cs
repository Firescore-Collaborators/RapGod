using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using NaughtyAttributes;

public enum Cameras
{
    Visitor1,
    Visitor2,
    Visitor3,
    defaultCam
}

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Cameras currentCam;
    public CinemachineVirtualCamera currentVCam;
    [SerializeField] CinemachineVirtualCamera visitor1;
    [SerializeField] CinemachineVirtualCamera visitor2;
    [SerializeField] CinemachineVirtualCamera visitor3;
    [SerializeField] CinemachineVirtualCamera defaultCam;

    public List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera> ();

    CinemachineBrain brain;
    System.Action OnCameraChange;
    public bool isBlending;
    bool checkBlend;



    private void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }
    }
    private void Start() {
        
        brain = Camera.main.GetComponent<CinemachineBrain>();
    }

    private void Update() {
        isBlending = brain.IsBlending;

        if(checkBlend)
        {
            if(!isBlending)
            {
                checkBlend = false;
                OnCameraChange?.Invoke();
            }
        }
    }

    [Button]
    void CameraInit()
    {
        cameras.Clear();
        for(int i = 0; i < transform.childCount; i++)
        {
            cameras.Add(transform.GetChild(i).GetComponent<CinemachineVirtualCamera>());
        }
    }
    void SetCameraDefault()
    {
        for(int i = 0; i < cameras.Count; i++)
        {
            cameras[i].Priority = 1;
        }
    }

    public void SetCurrentCamera(Cameras cam,System.Action _OnCameraChange = null)
    {
        checkBlend = true;
        OnCameraChange = _OnCameraChange;
        SetCameraDefault();
        SetBlendSpeed(2);
        switch(cam)
        {
            case Cameras.Visitor1:
                visitor1.Priority = 2;
                currentVCam = visitor1;
                break;

            case Cameras.Visitor2:
                visitor2.Priority = 2;
                currentVCam = visitor2;
                break;
            
            case Cameras.Visitor3:
                visitor3.Priority = 2;
                currentVCam = visitor3;
                break;
            case Cameras.defaultCam:
                SetBlendSpeed(0);
                defaultCam.Priority = 2;
                currentVCam = defaultCam;
                break;
        }
        currentCam = cam;
    }

    public void SetCurrentCamera(int index)
    {
        SetCurrentCamera((Cameras)index);
    }

    [Button]
    public void SetCamera()
    {
        SetCurrentCamera(currentCam);
    }

    public void SetBlendSpeed(float speed)
    {
        brain.m_DefaultBlend.m_Time = speed;
    }

}
