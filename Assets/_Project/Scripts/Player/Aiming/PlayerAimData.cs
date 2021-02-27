using UnityEngine;

namespace IND.PlayerSys
{
    [CreateAssetMenu(fileName = "Player Aim Data", menuName = "IND/Gameplay/Player Aim Data")]
    public class PlayerAimData : ScriptableObject
    {
        [Header ("Core")]
        public float minRadiusSize = 0.5f;
        public float maxRadiusSize = 2f;
        public float radiusGrowSpeed = 2f;

        [Header("Modifiers")]
        public AimRadiusModifier standingMovementModifier = new AimRadiusModifier();
        public AimRadiusModifier runningMovementModifier = new AimRadiusModifier();
        public AimRadiusModifier crouchedMovementModifier = new AimRadiusModifier();
        public AimRadiusModifier proneMovementModifier = new AimRadiusModifier();

        public void ResetData()
        {
            AimRadiusModifier[] modifers = {standingMovementModifier, runningMovementModifier, crouchedMovementModifier, proneMovementModifier };
            foreach (AimRadiusModifier item in modifers)
            {
                item.hasBeenApplied = false;
            }
        }
    }
}