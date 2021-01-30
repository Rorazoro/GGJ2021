using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class CameraManager : SingletonBehaviour<CameraManager> {

    public List<GameObject> Cameras;
    [SerializeField]
    private GameObject ActiveCamera;

    private void Awake () {
        int activeId = Cameras.IndexOf (ActiveCamera);
        ToggleCamera (activeId);
    }

    public void ToggleCamera (int cameraId) {
        //Set active camera
        ActiveCamera = Cameras[cameraId];
        //Deactivate other cameras
        foreach (GameObject camera in Cameras) {
            if (camera != ActiveCamera) {
                camera.SetActive (false);
            }
        }
        //Activate active camera
        ActiveCamera.SetActive (true);
    }

    public void SetCameraLookAt (Transform t) {
        ActiveCamera.GetComponent<CinemachineVirtualCamera> ().m_LookAt = t;
    }

    public void SetCameraFollow (Transform t) {
        ActiveCamera.GetComponent<CinemachineVirtualCamera> ().m_Follow = t;
    }

    public string ListCameraNames () {
        string names = "";
        foreach (GameObject obj in Cameras) {
            string activeText = obj == ActiveCamera ? "*" : "";
            names += $"[{Cameras.IndexOf(obj)}{activeText}]{obj.name}";
            if (obj != Cameras.Last ()) {
                names += "\n";
            }
        }
        return names;
    }
}