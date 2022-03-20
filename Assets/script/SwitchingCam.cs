using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchingCam : MonoBehaviour
{
    public Camera firstPersonCamera;
    public Camera overheadCamera;
    public int mode =0 ;
    // Call this function to disable FPS camera,
    // and enable overhead camera.
    public void ShowOverheadView()
    {
        mode = 0;
        firstPersonCamera.enabled = false;
        overheadCamera.enabled = true;
    }

    // Call this function to enable FPS camera,
    // and disable overhead camera.
    public void ShowFirstPersonView()
    {
        mode = 1;
        firstPersonCamera.enabled = true;
        overheadCamera.enabled = false;
    }
}
