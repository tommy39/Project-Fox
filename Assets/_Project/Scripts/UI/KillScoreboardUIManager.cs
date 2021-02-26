using IND.Network;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace IND.UI
{
    public class KillScoreboardUIManager : MonoBehaviourPun
    {
        [SerializeField] private GameObject childPanel;
        [SerializeField] private GameObject scoreboardItemPrefab;

        [SerializeField] private Transform redTeamPlayersList;
        [SerializeField] private Transform blueTeamPlayersList;
        [SerializeField] private Transform specTeamPlayersList;

        [SerializeField] private Color redTeamColor;
        [SerializeField] private Color blueTeamColor;
        [SerializeField] private Color specTeamColor;

        [SerializeField] private TextMeshProUGUI[] blueTeamElements;
        [SerializeField] private TextMeshProUGUI[] redTeamElements;

        private List<KillScoreboardUIItem> createdBluePlayers = new List<KillScoreboardUIItem>();
        private List<KillScoreboardUIItem> createdRedPlayers = new List<KillScoreboardUIItem>();
        private List<KillScoreboardUIItem> createdSpecPlayers = new List<KillScoreboardUIItem>();

        private bool isOpen = false;

        public static KillScoreboardUIManager singleton;
        private void Awake()
        {
            singleton = this;
        }

        private void Start()
        {
            SetupTeams();
            CloseInterface();
        }

        private void SetupTeams()
        {
            foreach (TextMeshProUGUI item in redTeamElements)
            {
                item.color = redTeamColor;
            }

            foreach (TextMeshProUGUI item in blueTeamElements)
            {
                item.color = blueTeamColor;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (isOpen == false)
                {
                    OpenInterface();
                }
        
            }

            if (Input.GetKeyUp(KeyCode.Tab))
            {
                if (isOpen == true)
                {
                    CloseInterface();
                }
            }
        }

        private void OpenInterface()
        {
            isOpen = true;
            childPanel.SetActive(true);
        }

        private void CloseInterface()
        {
            isOpen = false;
            childPanel.SetActive(false);
        }



        public void UpdateScoreboard(ClientController client)
        {
            switch (client.data.team)
            {
                case Teams.TeamType.SPEC:
                    for (int i = 0; i < createdSpecPlayers.Count; i++)
                    {
                        if(createdSpecPlayers[i].clientController == client)
                        {
                            createdSpecPlayers[i].UpdateItem(client.data.kills, client.data.deaths);
                        }
                    }
                    break;
                case Teams.TeamType.BLUE:
                    for (int i = 0; i < createdBluePlayers.Count; i++)
                    {
                        if (createdBluePlayers[i].clientController == client)
                        {
                            createdBluePlayers[i].UpdateItem(client.data.kills, client.data.deaths);
                        }
                    }
                    break;
                case Teams.TeamType.RED:
                    for (int i = 0; i < createdRedPlayers.Count; i++)
                    {
                        if (createdRedPlayers[i].clientController == client)
                        {
                            createdRedPlayers[i].UpdateItem(client.data.kills, client.data.deaths);
                        }
                    }
                    break;
            }
        }

        public void AddClientToScoreboard(ClientController client)
        {
            for (int i = 0; i < createdBluePlayers.Count; i++)
            {
                if (createdBluePlayers[i].clientID == client.data.clientID)
                {
                    Destroy(createdBluePlayers[i].gameObject);
                    createdBluePlayers.RemoveAt(i);
                }
            }

            for (int i = 0; i < createdRedPlayers.Count; i++)
            {
                if (createdRedPlayers[i].clientID == client.data.clientID)
                {
                    Destroy(createdRedPlayers[i].gameObject);
                    createdRedPlayers.RemoveAt(i);
                }
            }

            for (int i = 0; i < createdSpecPlayers.Count; i++)
            {
                if (createdSpecPlayers[i].clientID == client.data.clientID)
                {
                    Destroy(createdSpecPlayers[i].gameObject);
                    createdSpecPlayers.RemoveAt(i);
                }
            }

            GameObject geo = null;
            switch (client.data.team)
            {
                case Teams.TeamType.SPEC:
                    geo = Instantiate(scoreboardItemPrefab, specTeamPlayersList);
                    break;
                case Teams.TeamType.BLUE:
                    geo = Instantiate(scoreboardItemPrefab, blueTeamPlayersList);
                    break;
                case Teams.TeamType.RED:
                    geo = Instantiate(scoreboardItemPrefab, redTeamPlayersList);
                    break;
            }

            KillScoreboardUIItem controller = geo.GetComponent<KillScoreboardUIItem>();
            controller.clientID = client.data.clientID;
            controller.clientName = client.data.clientName;
            controller.teamType = client.data.team;
            controller.clientController = client;
            controller.UpdateItem(client.data.kills, client.data.deaths);

            Color targetColor = new Color();
            switch (controller.teamType)
            {
                case Teams.TeamType.SPEC:
                    targetColor = specTeamColor;
                    createdSpecPlayers.Add(controller);
                    break;
                case Teams.TeamType.BLUE:
                    createdBluePlayers.Add(controller);
                    targetColor = blueTeamColor;
                    break;
                case Teams.TeamType.RED:
                    createdRedPlayers.Add(controller);
                    targetColor = redTeamColor;
                    break;
            }
            controller.SetTextColor(targetColor);
        }

    }
}