using IND.Teams;
using TMPro;
using UnityEngine;
using Photon.Pun;
using IND.Network;

namespace IND.UI
{
    public class KillScoreboardUIItem : MonoBehaviour
    {
        public int clientID = -1;
        public ClientController clientController;
        public string clientName;
        public int clientKills;
        public int clientDeaths;
        public int clientPing;
        public TeamType teamType;

        private int refreshPingFrameRate = 60;
        private int currentFrame;

        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI killsText;
        [SerializeField] private TextMeshProUGUI deathsText;
        [SerializeField] private TextMeshProUGUI pingText;
               
        public void SetTextColor(Color targetColor)
        {
            nameText.color = targetColor;
            killsText.color = targetColor;
            deathsText.color = targetColor;
            pingText.color = targetColor;
        }

        public void UpdateItem(int clientKills, int clientDeaths)
        {
            nameText.text = clientName;
            killsText.text = clientKills.ToString();
            deathsText.text = clientDeaths.ToString();
        }

        private void Update()
        {
            if (clientID == -1)
                return;

            if(currentFrame < refreshPingFrameRate)
            {
                currentFrame++;
                return;
            }
            else
            {
                currentFrame = 0;
            }

            UpdatePingText();
        }


        private void UpdatePingText()
        {
            pingText.text = clientController.data.clientPing.ToString();
        }
    }
}