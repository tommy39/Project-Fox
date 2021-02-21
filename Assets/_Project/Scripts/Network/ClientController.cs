using Photon.Pun;
using UnityEngine;
using IND.Teams;


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
                TeamManager.singleton.OnTeamChange(TeamType.SPEC, false, true);
            }
        }

        [PunRPC]
        private void AssignToClientManager()
        {
            ClientManager.singleton.AddNewClient(this);
        }
    }
}