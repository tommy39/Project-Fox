using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IND.PlayerSys;
using TMPro;
using UnityEngine.UI;

namespace IND.UI
{
    public class PlayerHealthTrackerUIController : MonoBehaviour
    {
        private PlayerController playerController;
        private HealthController healthController;
        private PlayerMovementController playerMovementController;

        [Tooltip("Pixel offset from the player target")]
        [SerializeField]
        private Vector3 screenOffset = new Vector3(0f, 30f, 0f);

        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField]
        private Slider playerHealthSlider;

        private float curPostureHeight;
        private Vector3 targetPosition;


        public void OnCreated(PlayerController player)
        {
            playerController = player;
            healthController = player.GetComponent<HealthController>();
            playerMovementController = player.GetComponent<PlayerMovementController>();
            playerHealthSlider.maxValue = healthController.maxHealth;

            playerNameText.text = player.clientController.data.clientName;
        }

        private void Update()
        {
            if(playerController == null || healthController == null)
            {
                DestoryTracker();
                return;
            }

            if(healthController.currentHealth > 0)
            {
                playerHealthSlider.value = healthController.currentHealth;
            }
            else
            {
                DestoryTracker();
                return;
            }
        }

        private void LateUpdate()
        {
            if (playerController != null && playerMovementController != null)
            {
                targetPosition = playerController.transform.position;

                switch (playerMovementController.postureState)
                {
                    case PostureState.STANDING:
                        targetPosition.y += playerMovementController.standingCollider.height;
                        break;
                    case PostureState.CROUCHED:
                        targetPosition.y += playerMovementController.crouchedCollider.height;
                        break;
                    case PostureState.PRONE:
                        targetPosition.y += playerMovementController.proneCollider.height;
                        break;
                }

                transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
            }
        }

        public void DestoryTracker()
        {
            Destroy(gameObject);
        }
    }
}