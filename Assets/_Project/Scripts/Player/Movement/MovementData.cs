using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IND.Player
{
    [CreateAssetMenu(fileName = "Movement Data", menuName = "IND/Player/Movement Data")]
    public class MovementData : ScriptableObject
    {
        public float standingJogSpeed;
        public float sprintSpeed = 15f;
        public float crouchedMovementSpeed;
        public float proneMovementSpeed;
        public float aimRotationSpeed = 20f;
        public float groundCheckDistance = 2f;
    }
}