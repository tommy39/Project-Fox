using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IND.Player;

namespace IND.Teams
{
    public class TeamSelectionUI : MonoBehaviour
    {
        [SerializeField] protected Button redTeamBtn;
        [SerializeField] protected Button blueTeamBtn;
        [SerializeField] protected Button specTeamBtn;

        public static TeamSelectionUI singleton;
        private void Awake()
        {
            singleton = this;
            redTeamBtn.onClick.AddListener(() => { SetTeam(TeamType.RED);});
            blueTeamBtn.onClick.AddListener(() => { SetTeam(TeamType.BLUE);});
            specTeamBtn.onClick.AddListener(() => { SetTeam(TeamType.SPEC);});
            gameObject.SetActive(false);
        }

        private void SetTeam(TeamType type)
        {
            PlayerManager.singleton.OnTeamChange(type);
            gameObject.SetActive(false);
        }
    }
}