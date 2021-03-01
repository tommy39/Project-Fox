using Cinemachine;
using IND.KOTH;
using IND.Network;
using IND.PlayerSys;
using IND.Teams;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IND.Spectator
{
    public class SpectatorManager : MonoBehaviourPun
    {
        private Camera gameCam;
        private CinemachineVirtualCamera virtualCam;
        private TeamManager teamManager;

        [HideInInspector] public PlayerController followingTarget;
        public int currentTargetInList = 0;
        private KOTHFlagController kothFlag;

        private List<ClientController> clientsOnRedOrBlue = new List<ClientController>();
        private List<ClientController> allClients = new List<ClientController>();

        public bool isInSpecMode = false;

        private SpectatorManagerUI uiManager;

        public static SpectatorManager singleton;
        private void Awake()
        {
            singleton = this;
            gameCam = FindObjectOfType<CamController>().GetComponent<Camera>();
            virtualCam = FindObjectOfType<CinemachineVirtualCamera>();
        }

        private void Start()
        {
            uiManager = SpectatorManagerUI.singleton;
            kothFlag = FindObjectOfType<KOTHFlagController>();
            teamManager = TeamManager.singleton;
            StartCoroutine(DelayedStart());
        }

        private IEnumerator DelayedStart()
        {
            yield return new WaitForSeconds(1f);
            GetExistingClients();
        }


        private void Update()
        {
            if (isInSpecMode == false)
                return;

            if (followingTarget == null && currentTargetInList != -1)
            {
                currentTargetInList++;
                SetFollowTarget(currentTargetInList);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                GoThroughList(true);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                GoThroughList(false);
            }
        }

        public void GoThroughList(bool forward)
        {
            if (forward)
            {
                SetFollowTarget(currentTargetInList += 1);
            }
            else
            {
                SetFollowTarget(currentTargetInList -=1);
            }
        }

        public void EnterSpectatorModeLocally()
        {
            isInSpecMode = true;
            SetFollowTarget(0);
            uiManager.OpenInterface();
        }

        public void LeaveSpectatorModeLocally()
        {
            isInSpecMode = false;
            uiManager.CloseInterface();
        }

        private void SetFollowTarget(int targetVal)
        {
            GetFollowTargetInList(ref targetVal);

            if (currentTargetInList == -1) //Assign Camera to Flag
            {
                AssignTargetToCamera(kothFlag.transform);
            }
            else
            {
                followingTarget = clientsOnRedOrBlue[currentTargetInList].data.characterController;
                AssignTargetToCamera(followingTarget.transform);
            }

            uiManager.UpdateInterface();
        }

        private void AssignTargetToCamera(Transform trans)
        {
            virtualCam.Follow = trans;
        }

        private void GetFollowTargetInList(ref int val)
        {

            if (val > clientsOnRedOrBlue.Count - 1)
            {
                val = -1;
            }

            if (val == -2)
            {
                val = clientsOnRedOrBlue.Count - 1;
            }

            currentTargetInList = val;
        }

        private void GetExistingClients()
        {
            ClientController[] clients = FindObjectsOfType<ClientController>();
            foreach (ClientController item in clients)
            {
                allClients.Add(item);
                if (item.data.team != TeamType.SPEC)
                {
                    clientsOnRedOrBlue.Add(item);
                }
            }
        }

        [PunRPC]
        public void OnClientJoined(int clientID)
        {
            ClientController client = ClientManager.singleton.GetClientByID(clientID);
            allClients.Add(client);
        }

        [PunRPC]
        public void OnPlayerJoinedSpectator(int clientID)
        {
            ClientController client = ClientManager.singleton.GetClientByID(clientID);
            if (clientsOnRedOrBlue.Contains(client))
            {
                clientsOnRedOrBlue.Remove(client);
            }
        }

        [PunRPC]
        public void OnPlayerJoinedRedOrBlueTeam(int clientID)
        {
            ClientController client = ClientManager.singleton.GetClientByID(clientID);
            clientsOnRedOrBlue.Add(client);
        }
    }
}