using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IND.UI
{
    public class AimCursorUIManager : MonoBehaviour
    {
        private Camera cam;
        private void Awake()
        {
            cam = FindObjectOfType<Camera>();
        }

        public void UpdatePosition(Vector3 worldPos)
        {
            transform.position = cam.WorldToScreenPoint(worldPos);
        }
    }
}