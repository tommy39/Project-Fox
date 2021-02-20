using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

namespace IND.UI
{
    public class MainMenuManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] protected byte maxPlayersPerRoom = 32;
        [SerializeField] protected string gameVersion = "1";

        [SerializeField] private Button playOnlineButton = default;
        [SerializeField] private Button quitGameButton = default;

        [SerializeField] protected bool requireNameToPlay = true;

        private bool isConnecting = false;

        [SerializeField] private GameObject connectingMenu = default;
        [SerializeField] private GameObject notConnectingMenu = default;

        public bool startRoomAutomatically = false;

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            playOnlineButton.onClick.AddListener(() => { OnPlayOnlinePressed(); });
            quitGameButton.onClick.AddListener(() => { OnQuitGameButtonPressed(); });

        }

        private void Start()
        {
            if(startRoomAutomatically == true)
            {
                OnPlayOnlinePressed();
            }
        }

        private void OnPlayOnlinePressed()
        {
            BeginConnection();
        }

        private void OnQuitGameButtonPressed()
        {
            Application.Quit();
        }

        private void BeginConnection()
        {
            if (requireNameToPlay == true)
            {
                //Check if the players name is null
                if (string.IsNullOrEmpty(PhotonNetwork.NickName))
                {
                    Debug.Log("Player Name Cannot Be Null");
                    return;
                }
            }


            PlayerNameInputFieldController nameInputController = FindObjectOfType<PlayerNameInputFieldController>();
            PhotonNetwork.NickName = nameInputController.setName;
            connectingMenu.SetActive(true);
            notConnectingMenu.SetActive(false);

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }
        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinRandomRoom();
            isConnecting = false;
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("Disconnected");
            connectingMenu.SetActive(false);
            notConnectingMenu.SetActive(true);
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("Combat_Demo");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }
    }
}