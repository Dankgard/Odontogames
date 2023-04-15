using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerasManager : MonoBehaviour
{
    public Camera[] cameras;
    private int currentCameraIndex;

    public static CamerasManager camerasManagerInstance;

    private void Start()
    {
        camerasManagerInstance = this;
        //if (camerasManagerInstance == null)
        //{
        //    camerasManagerInstance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else if (camerasManagerInstance != null)
        //{
        //    Destroy(gameObject);
        //}

        currentCameraIndex = 0;

        // Deactivate all cameras then enable the current index camera
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }


        if (cameras.Length > 0)
        {
            cameras[currentCameraIndex].gameObject.SetActive(true);
        }
    }

    public void SwapCamera(int cameraIndex)
    {
        if (cameraIndex < 0 || cameraIndex >= cameras.Length)
        {
            Debug.LogError("Camera index outside array index");
            return;
        }

        // Deactivate current camera
        cameras[currentCameraIndex].gameObject.SetActive(false);

        // Update current camera index
        currentCameraIndex = cameraIndex;

        // Activate current camera
        cameras[currentCameraIndex].gameObject.SetActive(true);
    }

    public Camera GetCurrentCamera()
    {
        return cameras[currentCameraIndex];
    }
}
