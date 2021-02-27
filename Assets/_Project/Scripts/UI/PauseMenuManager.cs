using IND.PlayerSys;
using IND.Teams;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace IND.UI
{
    public class PauseMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject childObject = default;
        [SerializeField] private Button respawnBtn;
        [SerializeField] private Button changeLoadoutBtn;
        [SerializeField] private Button changeTeamBtn;
        [SerializeField] private Button optionsBtn;
        [SerializeField] private Button quitToMainMenuBtn;
        [SerializeField] private Button quitBtn;

        private bool isMenuActive = false;

        private PlayerManager playersManager;
        private TeamSelectionUI teamSelectionUI;
        private WeaponLoadoutUIManager loadoutManagerUI;

        public static PauseMenuManager singleton;
        private void Awake()
        {
            singleton = this;
            playersManager = FindObjectOfType<PlayerManager>();

            respawnBtn.onClick.AddListener(() => { RespawnButtonPressed(); });
            changeLoadoutBtn.onClick.AddListener(() => { OnChangeLoadoutButtonPressed(); });
            changeTeamBtn.onClick.AddListener(() => { ChangeTeamButtonPressed(); });
            optionsBtn.onClick.AddListener(() => { OptionsButtonPressed(); });
            quitToMainMenuBtn.onClick.AddListener(() => { QuitToMainMenuButtonPressed(); });
            quitBtn.onClick.AddListener(() => { QuitGameButtonPressed(); });

            childObject.SetActive(false);
        }

        private void Start()
        {
            loadoutManagerUI = WeaponLoadoutUIManager.singleton;
            teamSelectionUI = TeamSelectionUI.singleton;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isMenuActive == false)
                {
                    ToggleMenu(true);
                }
                else
                {
                    ToggleMenu(false);
                }
            }
        }

        private void ToggleMenu(bool val)
        {
            childObject.SetActive(val);
            isMenuActive = val;

            if (val == false)
            {
                UIManager.singleton.OnUIElementDeActivated();
            }
            else
            {
                UIManager.singleton.OnUIElementActivated(childObject);
            }
        }

        private void RespawnButtonPressed()
        {
            ToggleMenu(false);
            playersManager.RespawnPlayer();
        }

        private void ChangeTeamButtonPressed()
        {
            ToggleMenu(false);
            teamSelectionUI.OpenInterface();
        }

        private void OnChangeLoadoutButtonPressed()
        {
            ToggleMenu(false);
            loadoutManagerUI.OpenInterface();
        }

        private void OptionsButtonPressed()
        {

        }

        private void QuitToMainMenuButtonPressed()
        {
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("Main Menu");
        }

        private void QuitGameButtonPressed()
        {
            Application.Quit();
        }
    }
}