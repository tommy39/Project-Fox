using IND.Network;
using IND.PlayerSys;
using IND.Spectator;
using IND.UI;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IND.Teams
{
    public class TeamManager : MonoBehaviourPun
    {
        [SerializeField] private BoxCollider redTeamSpawn = default;
        [SerializeField] private BoxCollider blueTeamSpawn = default;
        [SerializeField] private BoxCollider specTeamSpawn = default;

        public List<int> blueTeamClients = new List<int>();
        public List<int> redTeamClients = new List<int>();
        public List<int> specClients = new List<int>();

        public Quaternion blueTeamSpawnRot;
        public Quaternion redTeamSpawnRot;

        public static TeamManager singleton;
        private void Awake()
        {
            singleton = this;
        }

        private void Start()
        {
            StartCoroutine(DelayedStart());
        }

        private IEnumerator DelayedStart()
        {
            yield return new WaitForSeconds(0.5f);
            GetExistingTeams();
        }

        private void GetExistingTeams()
        {
            ClientController[] clients = ClientManager.singleton.allClients.ToArray();
            foreach (ClientController item in clients)
            {
                if (item.data.characterController != null)
                {
                    AssignTeamToClient(item.data.clientID, item.data.characterController.teamType);
                    item.data.team = item.data.characterController.teamType;
                }
                else
                {
                    AssignTeamToClient(item.data.clientID, item.data.team);
                }
            }
        }

        public void OnTeamChange(TeamType type, bool openLoadoutInterface, bool intialization)
        {
            ClientController localClient = ClientManager.LocalPlayerClientControllerInstance;

            if (type == localClient.data.team && intialization == false)
                return;

            if (type != TeamType.SPEC)
            {
                //Remove Character Controller From The World
                if (localClient.data.characterController != null)
                {
                    localClient.data.characterController.GetComponent<HealthController>().ExternalDeath();
                }
            }
            object[] UpdateClientDataArray = { localClient.data.clientID, type };
            if (intialization == false)
            {
                photonView.RPC("UpdateClientData", RpcTarget.All, UpdateClientDataArray);
            }

            photonView.RPC("AssignTeamToClient", RpcTarget.All, UpdateClientDataArray);
            PlayerManager.singleton.teamType = type;
            PlayerKillScoreboardManager.singleton.photonView.RPC("OnClientTeamChange", RpcTarget.AllBuffered, localClient.data.clientID);
        
            localClient.OnTeamChanged();

            if (openLoadoutInterface == true)
            {
                WeaponLoadoutUIManager.singleton.OpenInterface();
                return;
            }

            if (type != TeamType.SPEC)
            {
                PlayerManager.singleton.SpawnPlayer();
            }
        }

        [PunRPC]
        private void UpdateClientData(int clientID, TeamType type)
        {
            ClientController targetClient = ClientManager.singleton.GetClientByID(clientID);
            targetClient.data.team = type;
        }

        [PunRPC]
        public void AssignTeamToClient(int id, TeamType type)
        {
            List<int> curTeam = FindClientInTeams(id);
            if (curTeam != null)
            {
                for (int i = 0; i < curTeam.Count; i++)
                {
                    if (curTeam[i] == id)
                    {
                        curTeam.RemoveAt(i);
                        break;
                    }
                }
            }

            switch (type)
            {
                case TeamType.SPEC:
                    specClients.Add(id);
                    break;
                case TeamType.BLUE:
                    blueTeamClients.Add(id);
                    break;
                case TeamType.RED:
                    redTeamClients.Add(id);
                    break;
            }
        }

        public List<int> FindClientInTeams(int id)
        {
            for (int i = 0; i < redTeamClients.Count; i++)
            {
                if (redTeamClients[i] == id)
                {
                    return redTeamClients;
                }
            }
            for (int i = 0; i < blueTeamClients.Count; i++)
            {
                if (blueTeamClients[i] == id)
                {
                    return blueTeamClients;
                }
            }
            for (int i = 0; i < specClients.Count; i++)
            {
                if (specClients[i] == id)
                {
                    return specClients;
                }
            }

            return null;
        }


        public Vector3 GetSpawnPos(TeamType teamType)
        {
            switch (teamType)
            {
                case TeamType.SPEC:
                    return GetSpawnPosInCollider(specTeamSpawn);
                case TeamType.BLUE:
                    return GetSpawnPosInCollider(blueTeamSpawn);
                case TeamType.RED:
                    return GetSpawnPosInCollider(redTeamSpawn);
            }

            return Vector3.zero;
        }

        private Vector3 GetSpawnPosInCollider(BoxCollider collider)
        {
            return new Vector3(Random.Range(collider.bounds.min.x, collider.bounds.max.x),
        Random.Range(collider.bounds.min.y, collider.bounds.max.y),
        Random.Range(collider.bounds.min.z, collider.bounds.max.z));
        }
    }
}