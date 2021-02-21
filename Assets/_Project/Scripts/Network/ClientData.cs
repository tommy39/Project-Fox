using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IND.Teams;
using IND.PlayerSys;

namespace IND.Network
{
    [System.Serializable]
    public class ClientData
    {
        public int clientID;
        public string clientName;
        public GameObject clientGEO;
        public TeamType team = TeamType.SPEC;
        public PlayerController characterController;
    }
}