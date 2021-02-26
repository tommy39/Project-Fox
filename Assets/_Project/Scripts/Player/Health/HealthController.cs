using IND.Network;
using IND.UI;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace IND.PlayerSys
{
    public class HealthController : MonoBehaviourPun
    {
        public float currentHealth = 100f;
        private List<HealthHitboxController> childHitboxes = new List<HealthHitboxController>();
        private RagdollController ragdollController;

        [HideInInspector] public bool isDead = false;
        [SerializeField] private bool isDummy = false;

        private PlayerKillScoreboardManager killScoreboardManager;

        private void Awake()
        {
            ragdollController = GetComponent<RagdollController>();
            isDead = false;
        }

        private void Start()
        {
            killScoreboardManager = PlayerKillScoreboardManager.singleton;
        }

        [PunRPC]
        public void TakeDamage(float damage, int clientID)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                CreateKillEvent(clientID);
                Death(false);
            }
        }

        private void CreateKillEvent(int killerID)
        {
            ClientController recieveClient = ClientManager.singleton.GetClientByID(photonView.ControllerActorNr);
            ClientController killerClient = ClientManager.singleton.GetClientByID(killerID);

            if (killScoreboardManager.photonView.IsMine == true)
            {
                if (recieveClient.data.team != killerClient.data.team)
                {
                    killScoreboardManager.photonView.RPC("AddKillToClient", RpcTarget.All, killerID);
                }
                else
                {
                    killScoreboardManager.photonView.RPC("AddFriendlyFireKillToClient", RpcTarget.All, killerID);
                }
                killScoreboardManager.photonView.RPC("AddDeathToClient", RpcTarget.All, recieveClient.data.clientID);
            }

            KillEventsHUDManager.singleton.CreateKillEvent(killerClient.data.clientName, killerClient.data.team, recieveClient.data.clientName, recieveClient.data.team, killerClient.data.characterController.GetComponent<PlayerInventoryController>().weaponData.weaponHorizontalIcon);
        }

        private void Death(bool isForced)
        {
            isDead = true;
            Destroy(GetComponent<PhotonRigidbodyView>());
            Destroy(GetComponent<Rigidbody>());

            ragdollController.EnableRagdoll();

            if (isDummy == true)
                return;

            //Destroy Components
            GetComponent<PlayerAnimController>().OnDeath();
            GetComponent<PlayerMovementController>().OnDeath();
            GetComponent<PlayerAimController>().OnDeath();
            GetComponent<PlayerInventoryController>().OnDeath();
            Destroy(ragdollController);
            Destroy(GetComponent<PhotonTransformView>());

            Ragdolls.RagdollManager.singleton.AddRagdoll(gameObject);

            if (photonView.IsMine)
            {
                if (isForced == false)
                {
                    //Open Respawn Interface
                    RespawnMenuUI.singleton.OpenInterface();
                }
            }
        }

        [PunRPC]
        private void OnRespawn()
        {
            Destroy(GetComponent<PlayerController>());
            Destroy(GetComponent<PhotonView>());
            Destroy(this);
        }

        [PunRPC]
        public void ExternalDeath()
        {
            Death(true);
        }

        public void AddChildHitbox(HealthHitboxController hitbox)
        {
            childHitboxes.Add(hitbox);
        }
    }
}