using IND.Cursors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IND.UI
{
    public class UIManager : MonoBehaviour
    {
        private CursorManager cursorManager;
        public GameObject activeUI;

        public static UIManager singleton;
        private void Awake()
        {
            singleton = this;
            cursorManager = FindObjectOfType<CursorManager>();
        }

        private void Start()
        {
        }

        public void OnUIElementActivated(GameObject geo)
        {
            activeUI = geo;
            cursorManager.ToggleCursorVisibility(true);
            }

        public void OnUIElementDeActivated()
        {
            cursorManager.ToggleCursorVisibility(false);
        }
    }
}