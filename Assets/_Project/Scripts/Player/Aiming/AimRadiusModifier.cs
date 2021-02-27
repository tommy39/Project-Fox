namespace IND.PlayerSys
{
    [System.Serializable]
    public class AimRadiusModifier
    {
        public float amount;
        public bool hasBeenApplied = false;

        public void ApplyModifier(AimRadiusController controller)
        {
            if(amount > 0)
            {
                controller.targetRadiusSize += amount;
            }
            else
            {
                controller.targetRadiusSize -= amount;
            }

            hasBeenApplied = true;
        }

        public void RemoveModifier(AimRadiusController controller)
        {
            if (amount > 0)
            {
                controller.targetRadiusSize -= amount;
            }
            else
            {
                controller.targetRadiusSize += amount;
            }

            hasBeenApplied = false;
        }
    }
}