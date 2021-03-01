using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace IND.UI
{
    public class AimCursorUIManager : MonoBehaviour
    {
        private Camera cam;

        public static AimCursorUIManager singleton;
        private void Awake()
        {
            singleton = this;
            cam = FindObjectOfType<CamController>().GetComponent<Camera>();
        }

        private void Start()
        {
            CloseElement();

        }

        public void UpdatePosition(Vector3 worldPos)
        {
            transform.position = cam.WorldToScreenPoint(worldPos);
        }

        public void CloseElement()
        {
            gameObject.SetActive(false);
        }
    }
}