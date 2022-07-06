﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using NaughtyAttributes;

public class MainCameraController : MonoBehaviour
{

    public static MainCameraController instance;
    public List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera> ();
    private CinemachineVirtualCamera currentCamera;

    public CinemachineVirtualCamera CurrentCamera
    {
        get
        {
            return currentCamera;
        }
    }

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }

        InitCamera();    

    }
    private void Start() {
    }
    void InitCamera()
    {
        cameras.Clear();
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

    public void SetCurrentCamera(string camera, float blendSpeed = 1)
    {
        SetCameraZero();
        currentCamera = transform.Find(camera).GetComponent<CinemachineVirtualCamera>();
        SetBlendSpeed(blendSpeed);
        currentCamera.Priority = 15;

    }

    void SetBlendSpeed(float blendSpeed)
    {
        Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time = blendSpeed;
    }

}
