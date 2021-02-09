using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IND.Cursors
{
    public class CursorManager : MonoBehaviour
    {
        [SerializeField] private bool enableCursorOnStart = false;
        private void Start()
        {
            if(enableCursorOnStart == true)
            {
                ToggleCursorVisibility(true);
            }
            else
            {
                ToggleCursorVisibility(false);
            }
        }

        private void ToggleCursorVisibility(bool val)
        {
            Cursor.visible = val;
        }
    }
}
