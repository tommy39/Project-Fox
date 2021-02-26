using IND.Teams;
using IND.UI;
using Photon.Pun;
using UnityEngine;

namespace IND.PlayerSys
{
    public class PlayerManager : MonoBehaviourPun
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
            teamSelectionUIManager = TeamSelectionUI.singleton;
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

        public void SpawnPlayer()
        {
            Vector3 spawnPos = teamManager.GetSpawnPos(teamType);
            Quaternion spawnRot = Quaternion.identity;
            switch (teamType)
            {
                case TeamType.SPEC:
                    break;
                case TeamType.BLUE:
                    spawnRot = teamManager.blueTeamSpawnRot;
                    break;
                case TeamType.RED:
                    spawnRot = teamManager.redTeamSpawnRot;
                    break;
            }
            GameObject createdGEO = PhotonNetwork.Instantiate(playerPrefab.name, spawnPos, spawnRot);

            createdPlayerController = createdGEO.GetComponent<PlayerController>();
            createdPlayerController.OnSpawn(teamType);
        }

        public void RespawnPlayer()
        {
            if (teamType == TeamType.SPEC)
                return;

            HealthController healthCon = createdPlayerController.GetComponent<HealthController>();

            if (healthCon.isDead == false)
            {
                healthCon.photonView.RPC("ExternalDeath", RpcTarget.All);
            }

            healthCon.photonView.RPC("OnRespawn", RpcTarget.All);

            SpawnPlayer();
        }
    }
}