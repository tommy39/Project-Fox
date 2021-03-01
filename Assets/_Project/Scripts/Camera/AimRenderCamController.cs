using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimRenderCamController : MonoBehaviour
{
    private void Awake()
    {
        Camera cam = GetComponent<Camera>();
        cam.clearFlags = CameraClearFlags.Depth;
    }
}
