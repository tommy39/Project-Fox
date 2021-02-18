using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IND.Player;
using IND.UI;

namespace IND.Teams
{
    public class TeamSelectionUI : MonoBehaviour
    {
        [SerializeField] protected Button redTeamBtn;
        [SerializeField] protected Button blueTeamBtn;
        [SerializeField] protected Button specTeamBtn;

        private WeaponLoadoutUIManager loadoutUIManager;

        public static TeamSelectionUI singleton;
        private void Awake()
        {
            singleton = this;
            redTeamBtn.onClick.AddListener(() => { SetTeam(TeamType.RED); });
            blueTeamBtn.onClick.AddListener(() => { SetTeam(TeamType.BLUE); });
            specTeamBtn.onClick.AddListener(() => { SetTeam(TeamType.SPEC); });
            gameObject.SetActive(false);
        }

        private void Start()
        {
            loadoutUIManager = WeaponLoadoutUIManager.singleton;
        }

        public void OpenInterface()
        {
            UIManager.singleton.OnUIElementActivated(gameObject);
            gameObject.SetActive(true);
        }

        public void CloseInterface()
        {
            UIManager.singleton.OnUIElementDeActivated();
            gameObject.SetActive(false);
        }

        private void SetTeam(TeamType type)
        {
            CloseInterface();
            PlayerManager.singleton.OnTeamChange(type, true);
        }
    }
}