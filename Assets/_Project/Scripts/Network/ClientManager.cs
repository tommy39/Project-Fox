using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

namespace IND.Network
{
    public class ClientManager : MonoBehaviourPun
    {
        public ClientData localClient = new ClientData();
        [SerializeField] private List<ClientData> allClients = new List<ClientData>();

        private void Start()
        {
            CreateLocalClient();
        }

        private void CreateLocalClient()
        {
            localClient.clientID = GetFreeClientID();
            localClient.clientName = PhotonNetwork.NickName;
            object[] arrayData = { localClient.clientID, localClient.clientName };
            photonView.RPC("AssignClient", RpcTarget.AllBuffered, arrayData);
        }

        [PunRPC]
        private void AssignClient(int ID, string name)
        {
            ClientData data = new ClientData();
            data.clientID = ID;
            data.clientName = name;
            allClients.Insert(data.clientID, data);
        }

        private int GetFreeClientID()
        {
            int targetID = 0;
            bool idFound = false;

            if (allClients.Count > 0)
            {
                for (int i = 0; i < allClients.Count; i++)
                {
                    if(allClients[i].clientID == targetID)
                    {
                        targetID++;
                        idFound = false;
                    }
                    else
                    {
                        idFound = true;
                    }
                }

                if(idFound == false)
                {
                    targetID = allClients.Count;
                }
            }
            return targetID;
        }
    }
}

