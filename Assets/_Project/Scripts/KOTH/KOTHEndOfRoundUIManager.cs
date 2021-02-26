using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using IND.Teams;

namespace IND.KOTH
{
    public class KOTHEndOfRoundUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject childPanel;
        [SerializeField] private TextMeshProUGUI teamNameTxt;

        [SerializeField] private Color redTeamColor;
        [SerializeField] private Color blueTeamColor;

        public TextMeshProUGUI serverClosingText;

        public static KOTHEndOfRoundUIManager singleton;
        private void Awake()
        {
            singleton = this;
            CloseInterface();
        }

        public void OpenInterface(TeamType winningTeam)
        {
            childPanel.SetActive(true);
            switch (winningTeam)
            {
                case TeamType.SPEC:
                    break;
                case TeamType.BLUE:
                    teamNameTxt.text = "Blue";
                    teamNameTxt.color = blueTeamColor;
                    break;
                case TeamType.RED:
                    teamNameTxt.text = "Red";
                    teamNameTxt.color = redTeamColor;
                    break;
            }
        }

        private void CloseInterface()
        {
            childPanel.SetActive(false);
        }
    }
}