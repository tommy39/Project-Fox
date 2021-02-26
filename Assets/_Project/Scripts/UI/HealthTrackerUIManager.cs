using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IND.PlayerSys;

namespace IND.UI
{
    public class HealthTrackerUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject healthTrackerPrefab;

        public static HealthTrackerUIManager singleton;
        private void Awake()
        {
            singleton = this;
        }

        public PlayerHealthTrackerUIController CreateTracker(PlayerController player)
        {
            GameObject geo = Instantiate(healthTrackerPrefab, transform);
            PlayerHealthTrackerUIController controller = geo.GetComponent<PlayerHealthTrackerUIController>();
            controller.OnCreated(player);

            return controller;
        }
    }
}