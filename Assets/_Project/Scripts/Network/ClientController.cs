using IND.PlayerSys;
using IND.Spectator;
using IND.Teams;
using IND.UI;
using Photon.Pun;
using UnityEngine;

namespace IND.Network
{
    public class ClientController : MonoBehaviourPun
    {
        public ClientData data;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (photonView.IsMine)
            {
                ClientManager.LocalPlayerClientControllerInstance = this;
            }
            //Assign Client Data
            data.clientID = photonView.ControllerActorNr;
            data.clientGEO = gameObject;

            var playerData = PhotonNetwork.CurrentRoom.GetPlayer(photonView.ControllerActorNr);
            data.clientName = playerData.NickName;

            gameObject.name = "Client-" + data.clientID;

            if (photonView.IsMine)
            {
                photonView.RPC("AssignToClientManager", RpcTarget.AllBuffered);
                SpectatorManager.singleton.photonView.RPC("OnClientJoined", RpcTarget.All, data.clientID);
                TeamManager.singleton.OnTeamChange(TeamType.SPEC, false, true);
            }
        }

        private void Update()
        {
            if (photonView.IsMine == true)
            {
                photonView.RPC("UpdatePing", RpcTarget.All);
            }
        }

        [PunRPC]
        private void UpdateKillsAndDeaths(int kills, int deaths)
        {
            if (kills < 0)
            {
                data.kills--;
            }
            else if (kills > 0)
            {
                data.kills++;
            }
            data.deaths += deaths;

            KillScoreboardUIManager.singleton.UpdateScoreboard(this);
        }

        [PunRPC]
        private void UpdatePing()
        {
            data.clientPing = PhotonNetwork.GetPing();
        }

        [PunRPC]
        private void AssignToClientManager()
        {
            ClientManager.singleton.AddNewClient(this);
        }

        public void OnTeamChanged()
        {
            if (photonView.IsMine == false)
                return;

            SpectatorManager spectatorManager = SpectatorManager.singleton;
            switch (data.team)
            {
                case TeamType.SPEC:
                    spectatorManager.EnterSpectatorModeLocally();
                    spectatorManager.photonView.RPC("OnPlayerJoinedSpectator", RpcTarget.All, data.clientID);
                    break;
                case TeamType.BLUE:
                    spectatorManager.LeaveSpectatorModeLocally();
                    spectatorManager.photonView.RPC("OnPlayerJoinedRedOrBlueTeam", RpcTarget.All, data.clientID);
                    break;
                case TeamType.RED:
                    spectatorManager.LeaveSpectatorModeLocally();
                    spectatorManager.photonView.RPC("OnPlayerJoinedRedOrBlueTeam", RpcTarget.All, data.clientID);
                    break;
            }
        }

        public void OnRoundEnd()
        {
            PlayerController playerController = data.characterController;

            PhotonRigidbodyView rigidbodyView = playerController.GetComponent<PhotonRigidbodyView>();
            if(rigidbodyView != null)
            {
                Destroy(rigidbodyView);
            }

            Rigidbody rigid = playerController.GetComponent<Rigidbody>();
            if(rigid != null)
            {
                Destroy(rigid);
            }

            //Destroy Components
            PlayerAnimController animController = playerController.GetComponent<PlayerAnimController>();
            if (animController != null)
            {
                animController.OnDeath();
            }

            PlayerMovementController movementController = playerController.GetComponent<PlayerMovementController>();
            if (movementController != null)
            { movementController.OnDeath(); }

            PlayerAimController aimController = playerController.GetComponent<PlayerAimController>();
            if(aimController != null)
            {
                aimController.OnDeath();
            }

            PlayerInventoryController inventoryController = playerController.GetComponent<PlayerInventoryController>();
            if(inventoryController != null)
            {
                inventoryController.OnDeath();
            }

            RagdollController ragdollController = playerController.GetComponent<RagdollController>();
            if(ragdollController != null)
            {
                Destroy(ragdollController);
            }

            PhotonTransformView transformView = playerController.GetComponent<PhotonTransformView>();
            if(transformView != null)
            {
                Destroy(transformView);
            }

        }
    }
}