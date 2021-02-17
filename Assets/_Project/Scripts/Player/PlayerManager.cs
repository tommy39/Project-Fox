using IND.Teams;
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

        public static PlayerManager singleton;
        private void Awake()
        {
            singleton = this;
            teamManager = FindObjectOfType<TeamManager>();
            teamSelectionUIManager =TeamSelectionUI.singleton;
        }

        private void Start()
        {
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

        public void OnTeamChange(TeamType type)
        {
            if (type == teamType)
                return;

            if (type != TeamType.SPEC)
            {
                //Remove Character Controller From The World

            }

            teamType = type;

            switch (type)
            {
                case TeamType.SPEC:
                    break;
                case TeamType.BLUE:
                    SpawnPlayer();
                    break;
                case TeamType.RED:
                    SpawnPlayer();
                    break;
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