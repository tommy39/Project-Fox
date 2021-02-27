using IND.Weapons;
using UnityEngine;

namespace IND.PlayerSys
{
    public class AimRadiusController : MonoBehaviour
    {
        [SerializeField] private PlayerAimData data;

        public float currentRadiusSize;
        public float targetRadiusSize;
        private float finalTargetRadiusSize;

        private MeshRenderer renderer;

        private PlayerAimController aimController;
        private PlayerMovementController movementController;
        [HideInInspector] public WeaponController weaponController;

        private float currentContractionCooldown = 0f;

        private Vector3 targetRadiusVec;

        private float curRange;

        private float applyableEffectiveRangeModifier;
        private float appliedEffectiveRangedModifier;

        public void AssignPlayer(PlayerAimController controller)
        {
            aimController = controller;
            movementController = controller.GetComponent<PlayerMovementController>();
            renderer = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            ToggleRenderer(false);
            data.ResetData();
            SetGameObjectSize(data.minRadiusSize);
            SetTargetRadius(data.minRadiusSize);
        }

        public void ToggleRenderer(bool val)
        {
            renderer.enabled = val;
        }

        private void Update()
        {
            if (aimController == null)
            {
                Destroy(gameObject);
                return;
            }
            UnApplyAnyModifiersNotNeeded();
            CheckMovement();
            CheckEffectiveRange();
        }

        private void LateUpdate()
        {
            if (aimController == null)
            {
                Destroy(gameObject);
                return;
            }

            transform.position = aimController.aimTarget.position;
            RotateToPlayer();
            UpdateRadiusSize();
        }

        private void UpdateRadiusSize()
        {
            if (currentContractionCooldown > 0)
            {
                currentContractionCooldown -= Time.deltaTime;
                return;
            }

            finalTargetRadiusSize = targetRadiusSize;

            if (finalTargetRadiusSize > data.maxRadiusSize)
            {
                finalTargetRadiusSize = data.maxRadiusSize;
            }

            if (finalTargetRadiusSize < data.minRadiusSize)
            {
                finalTargetRadiusSize = data.minRadiusSize;
            }

            targetRadiusVec = new Vector3(finalTargetRadiusSize, finalTargetRadiusSize, finalTargetRadiusSize);

            if (currentRadiusSize != targetRadiusSize)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, targetRadiusVec, data.radiusGrowSpeed * Time.deltaTime);
            }

            currentRadiusSize = transform.localScale.x;
        }

        private void CheckMovement()
        {
            if (movementController.isSprinting == true)
            {
                //Apply Sprint Modifier
                if (data.runningMovementModifier.hasBeenApplied == false)
                {
                    data.runningMovementModifier.ApplyModifier(this);
                }
            }
            else if (movementController.isPressingMovementKeys == true)
            {
                //Apply Regular Movement Modifier
                switch (movementController.postureState)
                {
                    case PostureState.STANDING:
                        if (data.standingMovementModifier.hasBeenApplied == false)
                        {
                            data.standingMovementModifier.ApplyModifier(this);
                        }
                        break;
                    case PostureState.CROUCHED:
                        if (data.crouchedMovementModifier.hasBeenApplied == false)
                        {
                            data.crouchedMovementModifier.ApplyModifier(this);
                        }
                        break;
                    case PostureState.PRONE:
                        if (data.proneMovementModifier.hasBeenApplied == false)
                        {
                            data.proneMovementModifier.ApplyModifier(this);
                        }
                        break;
                }
            }

        }

        private void UnApplyAnyModifiersNotNeeded()
        {
            if (data.standingMovementModifier.hasBeenApplied)
            {
                if (movementController.isPressingMovementKeys == false)
                {
                    data.standingMovementModifier.RemoveModifier(this);
                }

                if (movementController.postureState != PostureState.STANDING && movementController.isPressingMovementKeys == true)
                {
                    data.standingMovementModifier.RemoveModifier(this);
                }
            }

            if (data.crouchedMovementModifier.hasBeenApplied)
            {
                if (movementController.isPressingMovementKeys == false)
                {
                    data.crouchedMovementModifier.RemoveModifier(this);
                }

                if (movementController.postureState != PostureState.CROUCHED && movementController.isPressingMovementKeys == true)
                {
                    data.crouchedMovementModifier.RemoveModifier(this);
                }
            }


            if (data.proneMovementModifier.hasBeenApplied)
            {
                if (movementController.isPressingMovementKeys == false)
                {
                    data.proneMovementModifier.RemoveModifier(this);
                }

                if (movementController.postureState != PostureState.PRONE && movementController.isPressingMovementKeys == true)
                {
                    data.proneMovementModifier.RemoveModifier(this);
                }
            }

            if (data.runningMovementModifier.hasBeenApplied)
            {
                if (movementController.isSprinting == false)
                {
                    data.runningMovementModifier.RemoveModifier(this);
                }
            }
        }


        private void RotateToPlayer()
        {
            transform.LookAt(aimController.transform);
        }

        private void SetGameObjectSize(float size)
        {
            currentRadiusSize = size;
            gameObject.transform.localScale = new Vector3(size, size, size);
        }

        private void SetTargetRadius(float size)
        {
            targetRadiusSize = size;
            targetRadiusVec = new Vector3(size, size, size);
        }

        public void ForceSetCurrentRadius(float amount)
        {
            float finalAmount = 0f;

            if (amount > 0)
            {
                finalAmount = amount + targetRadiusSize;
                currentRadiusSize += amount;

                if (finalAmount > data.maxRadiusSize)
                {
                    finalAmount = data.maxRadiusSize;
                }

                if (currentRadiusSize > data.maxRadiusSize)
                {
                    currentRadiusSize = data.maxRadiusSize;
                }
            }
            else
            {
                finalAmount = targetRadiusSize - amount;
                currentRadiusSize -= amount;

                if (finalAmount < data.minRadiusSize)
                {
                    finalAmount = data.minRadiusSize;
                }

                if (currentRadiusSize < data.minRadiusSize)
                {
                    currentRadiusSize = data.minRadiusSize;
                }
            }
            Debug.Log(currentRadiusSize);

            targetRadiusVec = new Vector3(currentRadiusSize, currentRadiusSize, currentRadiusSize);
            gameObject.transform.localScale = new Vector3(currentRadiusSize, currentRadiusSize, currentRadiusSize);
        }

        public void ApplyContractionCooldown(float amount)
        {
            currentContractionCooldown = amount;
        }

        private void CheckEffectiveRange()
        {
            if (weaponController == null)
                return;

            if (aimController.isAiming == false)
                return;

            curRange = Vector3.Distance(weaponController.shootpoint.position, transform.position);
            if (curRange >= weaponController.weaponData.effectiveRange)
            {
                float outsideEffectiveRangeAmount = curRange - weaponController.weaponData.effectiveRange;
                int roundedDistance = (int)outsideEffectiveRangeAmount;
                if (roundedDistance > 0)
                {
                    float targetAmount = weaponController.weaponData.radiusIncreaseForOutsideEffectiveRangePerMetre * roundedDistance;
                    ApplyEffectiveRangeModifier(targetAmount);
                }
            }
            else if (curRange <= weaponController.weaponData.effectiveRange)
            {
                ApplyEffectiveRangeModifier(-1);
            }
        }

        private void ApplyEffectiveRangeModifier(float amount)
        {
            if (amount > 0)
            {
                float applyableAmount = 0f;

                if (amount == appliedEffectiveRangedModifier)
                    return;

                //Get how much we have applied and how much we can apply in this frame if any. 
                if (targetRadiusSize + applyableEffectiveRangeModifier > data.maxRadiusSize && amount >= applyableEffectiveRangeModifier) //Already over Limit
                {
                    return;
                }
                else //Under Limit
                {
                    applyableEffectiveRangeModifier = amount;
                }

                applyableAmount = amount - appliedEffectiveRangedModifier;


                float totalRadius = targetRadiusSize + applyableAmount;
                if (totalRadius > data.maxRadiusSize)
                {
                    float difference = totalRadius - data.maxRadiusSize;
                    applyableAmount = applyableAmount - difference;
                }

                if (applyableAmount > 0)
                {
                    appliedEffectiveRangedModifier += applyableAmount;
                    targetRadiusSize += applyableAmount;
                }
                else
                {
                    appliedEffectiveRangedModifier -= applyableAmount;
                    targetRadiusSize -= applyableAmount;
                }

                //   Debug.Log(targetRadiusSize + " Target Radius");
                Debug.Log(applyableAmount + " Applyable amount");
            }
            else //Negative, Reduction, Remove Existing amount
            {
                targetRadiusSize = targetRadiusSize - applyableEffectiveRangeModifier;
                applyableEffectiveRangeModifier = 0;
            }
        }
    }
}