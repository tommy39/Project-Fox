using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IND.Player;
using IND.Teams;

namespace IND.UI
{
    public class PauseMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject childObject = default;
        [SerializeField] private Button respawnBtn;
        [SerializeField] private Button changeTeamBtn;
        [SerializeField] private Button optionsBtn;
        [SerializeField] private Button quitToMainMenuBtn;
        [SerializeField] private Button quitBtn;

        private bool isMenuActive = false;

        private PlayerManager playersManager;
        private TeamSelectionUI teamSelectionUI;

        private void Awake()
        {
            playersManager = FindObjectOfType<PlayerManager>();

            respawnBtn.onClick.AddListener(() => { RespawnButtonPressed(); });
            changeTeamBtn.onClick.AddListener(() => { ChangeTeamButtonPressed(); });
            optionsBtn.onClick.AddListener(() => { OptionsButtonPressed(); });
            quitToMainMenuBtn.onClick.AddListener(() => { QuitToMainMenuButtonPressed(); });
            quitBtn.onClick.AddListener(() => { QuitGameButtonPressed(); });

            childObject.SetActive(false);
        }

        private void Start()
        {
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

            if(val == false)
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

        private void OptionsButtonPressed()
        {

        }

        private void QuitToMainMenuButtonPressed()
        {

        }

        private void QuitGameButtonPressed()
        {
            Application.Quit();
        }
    }
}