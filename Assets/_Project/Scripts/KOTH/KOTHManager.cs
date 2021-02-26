using IND.Network;
using IND.Teams;
using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using IND.UI;

namespace IND.KOTH
{
    public class KOTHManager : MonoBehaviourPun
    {
        public int blueTeamScore;
        public int redTeamScore;

        public int maxScore = 100;

        [SerializeField] private int newRoundCountdownTimer = 5;

        private KOTHUIManager uiManager;

        public static KOTHManager singleton;
        private void Awake()
        {
            singleton = this;
        }

        private void Start()
        {
            uiManager = KOTHUIManager.singleton;
            blueTeamScore = 0;
            redTeamScore = 0;

            uiManager.UpdateScore(redTeamScore, blueTeamScore);
        }

        [PunRPC]
        public void AddScore(int red, int blue)
        {
            blueTeamScore += blue;
            redTeamScore += red;
            uiManager.UpdateScore(redTeamScore, blueTeamScore);

            if (photonView.IsMine == true)
            {
                if (redTeamScore >= maxScore || blueTeamScore >= maxScore)
                {
                    photonView.RPC("RoundEnd", RpcTarget.AllBuffered);
                }
            }
        }

        [PunRPC]
        private void RoundEnd()
        {
            TeamType winningTeam = TeamType.SPEC;
            if (blueTeamScore == maxScore)
            {
                winningTeam = TeamType.BLUE;
            }
            else
            {
                winningTeam = TeamType.RED;
            }

            //Activate the Winning Team Interface
            KOTHEndOfRoundUIManager.singleton.OpenInterface(winningTeam);

            //Destory All Inputs For Clients to Tick (Except Checking Scoreboard)
            ClientController[] clients = FindObjectsOfType<ClientController>();

            foreach (ClientController item in clients)
            {
                item.OnRoundEnd();
            }

            //Destroy UI Managers;
            AimCursorUIManager.singleton.CloseElement();
            Destroy(TeamSelectionUI.singleton);
            Destroy(PauseMenuManager.singleton);


            //Prevent the Current game mode from resuming the score ticking by destroying necessary elements. 
            KOTHFlagController flagController = GetComponentInChildren<KOTHFlagController>();
            Destroy(flagController);

            if(photonView.IsMine == true)
            {
                StartCoroutine(NewRoundTickDown());
            }
        }

        private IEnumerator NewRoundTickDown()
        {
            while (newRoundCountdownTimer > 0)
            {
                yield return new WaitForSeconds(1f);
                newRoundCountdownTimer--;
                KOTHEndOfRoundUIManager.singleton.serverClosingText.text = "Server Closing in " + newRoundCountdownTimer;
                if (newRoundCountdownTimer <= 1)
                {
                    photonView.RPC("EndRound", RpcTarget.AllBuffered);
                }
            }
        }

        [PunRPC]
        private void EndRound()
        {
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("Main Menu");            
        }
    }
}