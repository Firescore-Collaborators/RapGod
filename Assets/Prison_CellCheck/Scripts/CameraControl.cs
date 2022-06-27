using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using NaughtyAttributes;

public class CameraControl : MonoBehaviour
{
    //public static CameraControl instance;

    public CinemachineVirtualCamera [] cams;

    public List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera> ();

    public string setCamera;


    private void Awake() {
        //if(instance == null) {
        //    instance = this;
        //} else {
        //    Destroy(this);
        //}

        InitCamera();    
    }
    private void Start() {

    }
    void InitCamera()
    {
        cameras.Clear();

        cams = transform.GetComponentsInChildren<CinemachineVirtualCamera>();

        for(int i = 0; i < transform.childCount; i++)
        {
            CinemachineVirtualCamera cam = transform.GetChild(i).GetComponent<CinemachineVirtualCamera>();
            if(cam != null)
            {
                cameras.Add(cam);
            }
        }
    }
    public void SetCameraZero()
    {
        for(int i = 0; i < cameras.Count; i++)
        {
            cameras[i].Priority = 0;
        }
    }

    public void SetCurrentCamera(string camera, float blendSpeed)
    {
        SetCameraZero();

        CinemachineVirtualCamera currentCamera = GameObject.Find(camera).GetComponent<CinemachineVirtualCamera>();

        Debug.Log("opopo "+currentCamera.gameObject.name);

        SetBlendSpeed(blendSpeed);
        currentCamera.Priority = 40;
    }

    public void SetCurrentCamera(CinemachineVirtualCamera camera, int blendSpeed = 1)
    {
        SetCameraZero();
        SetBlendSpeed(blendSpeed);
        print(camera.name);
        camera.Priority = 40;
    }

    void SetBlendSpeed(float blendSpeed)
    {
        Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time = blendSpeed;
    }

    public void AddCamera(CinemachineVirtualCamera cam)
    {
        cameras.Add(cam);
    }
}
