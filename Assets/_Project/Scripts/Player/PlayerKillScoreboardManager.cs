using IND.Network;
using IND.UI;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IND.PlayerSys
{
    public class PlayerKillScoreboardManager : MonoBehaviourPun
    {
        public List<KillScoreboardPlayerItem> redTeamPlayers = new List<KillScoreboardPlayerItem>();
        public List<KillScoreboardPlayerItem> blueTeamPlayers = new List<KillScoreboardPlayerItem>();
        public List<KillScoreboardPlayerItem> specPlayers = new List<KillScoreboardPlayerItem>();

        private KillScoreboardUIManager killScoreboardUIManager;

        public static PlayerKillScoreboardManager singleton;
        private void Awake()
        {
            singleton = this;
        }

        private void Start()
        {
            killScoreboardUIManager = KillScoreboardUIManager.singleton;
            StartCoroutine(DelayedStart());
        }

        private IEnumerator DelayedStart()
        {
            yield return new WaitForSeconds(2f);
            GetExistingClientsState();
        }

        private void GetExistingClientsState()
        {
            ClientManager manager = ClientManager.singleton;
            for (int i = 0; i < manager.allClients.Count; i++)
            {
                if (manager.allClients[i].data.clientID != ClientManager.LocalPlayerClientControllerInstance.data.clientID)
                {
                    OnClientTeamChange(manager.allClients[i].data.clientID);
                }
            }
        }

        [PunRPC]
        private void OnClientTeamChange(int clientID)
        {
            ClientController clientController = ClientManager.singleton.GetClientByID(clientID);
            if (clientController == null)
                return;

            KillScoreboardPlayerItem item = new KillScoreboardPlayerItem();
            item.client = clientController;
            item.kills = item.client.data.kills;
            item.deaths = item.client.data.deaths;


            for (int i = 0; i < specPlayers.Count; i++)
            {
                if (specPlayers[i].client == item.client)
                {
                    specPlayers.RemoveAt(i);
                }
            }

            for (int i = 0; i < blueTeamPlayers.Count; i++)
            {
                if (blueTeamPlayers[i].client == item.client)
                {
                    blueTeamPlayers.RemoveAt(i);
                }
            }
            for (int i = 0; i < redTeamPlayers.Count; i++)
            {
                if (redTeamPlayers[i].client == item.client)
                {
                    redTeamPlayers.RemoveAt(i);
                }
            }


            switch (item.client.data.team)
            {
                case Teams.TeamType.SPEC:
                    specPlayers.Add(item);
                    break;
                case Teams.TeamType.BLUE:
                    blueTeamPlayers.Add(item);
                    break;
                case Teams.TeamType.RED:
                    redTeamPlayers.Add(item);
                    break;
            }

            killScoreboardUIManager.AddClientToScoreboard(item.client);
        }

        [PunRPC]
        public void AddKillToClient(int clientID)
        {
            ClientController client = ClientManager.singleton.GetClientByID(clientID);
            object[] killPacket = { 1, 0 };
            if (photonView.IsMine)
            {
                client.photonView.RPC("UpdateKillsAndDeaths", RpcTarget.AllBuffered, killPacket);
            }
        }
        [PunRPC]
        public void AddFriendlyFireKillToClient(int clientID)
        {
            ClientController client = ClientManager.singleton.GetClientByID(clientID);
            if (photonView.IsMine)
            {
                object[] killPacket = { -1, 0 };
                client.photonView.RPC("UpdateKillsAndDeaths", RpcTarget.AllBuffered, killPacket);
            }
        }

        [PunRPC]
        public void AddDeathToClient(int clientID)
        {
            ClientController client = ClientManager.singleton.GetClientByID(clientID);
            if (photonView.IsMine)
            {
                object[] deathPacket = { 0, 1 };
                client.photonView.RPC("UpdateKillsAndDeaths", RpcTarget.AllBuffered, deathPacket);
            }
        }
    }

    [System.Serializable]
    public class KillScoreboardPlayerItem
    {
        public ClientController client;
        public int kills = 0;
        public int deaths = 0;
        public int ping = 1;
    }
}