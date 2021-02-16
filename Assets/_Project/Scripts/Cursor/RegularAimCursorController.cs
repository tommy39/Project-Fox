using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IND.UI
{
    public class RegularAimCursorController : MonoBehaviour
    {
        private void FixedUpdate()
        {
            transform.position = Input.mousePosition;
        }
    }
}