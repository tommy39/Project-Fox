using IND.Teams;
using IND.UI;
using UnityEngine;

namespace IND.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public TeamType teamType;
        public GameObject playerPrefab;
        public static PlayerController createdPlayerController;

        private TeamManager teamManager;
        private TeamSelectionUI teamSelectionUIManager;
        private WeaponLoadoutUIManager loadoutUIManager;

        public static PlayerManager singleton;
        private void Awake()
        {
            singleton = this;
            teamManager = FindObjectOfType<TeamManager>();
            teamSelectionUIManager =TeamSelectionUI.singleton;
        }

        private void Start()
        {
            loadoutUIManager = WeaponLoadoutUIManager.singleton;

            if (teamType == TeamType.SPEC)
            {
                //Enable The Pick Team UI on start
                teamSelectionUIManager.gameObject.SetActive(true);
            }
            else
            {
                //Spawn The Player On Join
                SpawnPlayer();
            }
        }

        public void OnTeamChange(TeamType type, bool openLoadoutInterface)
        {
            if (type == teamType)
                return;

            if (type != TeamType.SPEC)
            {
                //Remove Character Controller From The World
                if (createdPlayerController != null)
                {
                    createdPlayerController.GetComponent<HealthController>().Death(true);
                }
            }

            teamType = type;

            if(openLoadoutInterface == true)
            {
                loadoutUIManager.OpenInterface();
                return;
            }

            if(type != TeamType.SPEC)
            {
                SpawnPlayer();
            }
        }



        public void SpawnPlayer()
        {
            GameObject createdGEO = Instantiate(playerPrefab);
            createdPlayerController = createdGEO.GetComponent<PlayerController>();
            Vector3 spawnPos = teamManager.GetSpawnPos(teamType);
            createdPlayerController.transform.position = spawnPos;
            createdPlayerController.OnSpawn(teamType, this);
        }

        public void RespawnPlayer()
        {
            if (teamType == TeamType.SPEC)
                return;

            if(createdPlayerController != null)
            {
                createdPlayerController.GetComponent<HealthController>().Death(true);
            }

            SpawnPlayer();
        }
    }
}