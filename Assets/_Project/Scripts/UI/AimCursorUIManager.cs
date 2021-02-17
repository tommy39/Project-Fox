using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IND.UI
{
    public class AimCursorUIManager : MonoBehaviour
    {
        private Camera cam;

        public static AimCursorUIManager singleton;
        private void Awake()
        {
            singleton = this;
            cam = FindObjectOfType<Camera>();
        }

        public void UpdatePosition(Vector3 worldPos)
        {
            transform.position = cam.WorldToScreenPoint(worldPos);
        }
    }
}