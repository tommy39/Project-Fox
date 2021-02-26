using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IND.Teams;

namespace IND.UI
{
    public class KillEventsHUDManager : MonoBehaviour
    {
        public GameObject killEventUIPrefab;
        public Transform killEventsList;
        public float removeItemTimer = 2f;
        public int maxItemsToDisplay = 4;

        private List<KillScoreItemUIController> createdItems = new List<KillScoreItemUIController>();

        public Color redColor;
        public Color blueColor;

        public void CreateKillEvent(string killerName, TeamType killerTeam, string recieverName, TeamType recieverTeam, Sprite icon)
        {
            GameObject geo = Instantiate(killEventUIPrefab, killEventsList);
            KillScoreItemUIController itemController = geo.GetComponent<KillScoreItemUIController>();
            createdItems.Add(itemController);
            Color killerColor = redColor;
            Color recieverColor = redColor;

            if(createdItems.Count > maxItemsToDisplay)
            {
                ForceDestroyItem(createdItems[0]);
            }

            StartCoroutine(TimerToRemoveItem(itemController));

            switch (killerTeam)
            {
                case TeamType.SPEC:
                    break;
                case TeamType.BLUE:
                    killerColor = blueColor;
                    break;
                case TeamType.RED:
                    killerColor = redColor;
                    break;
            }

            switch (recieverTeam)
            {
                case TeamType.SPEC:
                    break;
                case TeamType.BLUE:
                    recieverColor = blueColor;
                    break;
                case TeamType.RED:
                    recieverColor = redColor;
                    break;
            }

            itemController.OnSetup(killerName, killerColor, recieverName, recieverColor, icon);
        }

        private void ForceDestroyItem(KillScoreItemUIController item)
        {
            createdItems.Remove(item);
            Destroy(item.gameObject);
        }

        private IEnumerator TimerToRemoveItem(KillScoreItemUIController item)
        {
            yield return new WaitForSeconds(removeItemTimer);
            if(item != null)
            {
                createdItems.Remove(item);
                Destroy(item.gameObject);
            }
        }

        public static KillEventsHUDManager singleton;
        private void Awake()
        {
            singleton = this;
        }
    }
}