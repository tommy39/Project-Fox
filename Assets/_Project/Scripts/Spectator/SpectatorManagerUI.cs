using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IND.Spectator
{
    public class SpectatorManagerUI : MonoBehaviour
    {
        [SerializeField] private GameObject childPanel;
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private Button rightArrowBtn;
        [SerializeField] private Button leftArrowBtn;

        private SpectatorManager spectatorManager;

        public static SpectatorManagerUI singleton;
        private void Awake()
        {
            singleton = this;
            rightArrowBtn.onClick.AddListener(OnRightButtonPressed);
            leftArrowBtn.onClick.AddListener(OnLeftButtonPressed);
            CloseInterface();
        }

        private void Start()
        {
            spectatorManager = SpectatorManager.singleton;
        }

        public void CloseInterface()
        {
            childPanel.SetActive(false);
        }

        public void OpenInterface()
        {
            childPanel.SetActive(true);
            UpdateInterface();
        }

        public void UpdateInterface()
        {
            string targetName = "";
            if (spectatorManager.currentTargetInList == -1 || spectatorManager.followingTarget == null)
            {
                targetName = "None";
            }
            else
            {
                targetName = spectatorManager.followingTarget.clientController.data.clientName;
            }

            playerNameText.text = targetName;
        }

        private void OnRightButtonPressed()
        {
            spectatorManager.GoThroughList(true);
        }

        private void OnLeftButtonPressed()
        {
            spectatorManager.GoThroughList(false);
        }
    }
}