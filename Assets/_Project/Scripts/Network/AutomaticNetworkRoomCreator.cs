using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IND.Network
{
    public class AutomaticNetworkRoomCreator : MonoBehaviourPunCallbacks
    {
        [SerializeField] private bool loadFromMainMenu = true;
        bool isConnecting = false;
        public bool loadArtScene = true;

        private void Awake()
        {

            if (loadArtScene)
            {
                SceneManager.LoadScene("Combat_Demo_Art", LoadSceneMode.Additive);
            }
            if (PhotonNetwork.InRoom == true) //Already In a Room
            {
                return;
            }

            if (loadFromMainMenu == true)
            {
                SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
            }

            PhotonNetwork.NickName = "Player";

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = "0.1";
            }
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinRandomRoom();
            isConnecting = false;
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            isConnecting = false;
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 32 });
        }
    }
}