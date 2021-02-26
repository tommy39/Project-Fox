using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IND.PlayerSys;

namespace IND.UI
{
    public class RespawnMenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject childObject = default;

        [SerializeField] private Button respawnBtn = default;

        public static RespawnMenuUI singleton;
        private void Awake()
        {
            singleton = this;
            respawnBtn.onClick.AddListener(OnRespawnBtnPressed);
            childObject.SetActive(false);
        }

        public void OpenInterface()
        {
            childObject.SetActive(true);
        }

        private void OnRespawnBtnPressed()
        {
            childObject.SetActive(false);
            PlayerManager.singleton.RespawnPlayer();
        }
    }
}