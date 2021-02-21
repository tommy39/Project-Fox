using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;

namespace IND.Network
{
    public class AutomaticNetworkRoomCreator : MonoBehaviourPunCallbacks
    {
        [SerializeField] private bool loadFromMainMenu = true;
        bool isConnecting = false;

        private void Awake()
        {
            if (PhotonNetwork.InRoom == true) //Already In a Room
            {
                return;
            }

            if(loadFromMainMenu == true)
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