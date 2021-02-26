using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IND.Network;
using TMPro;
using IND.PlayerSys;

namespace IND.UI
{
    public class KillScoreItemUIController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI killerNameTxt = default;
        [SerializeField] private TextMeshProUGUI recieverNameTxt = default;
        [SerializeField] private Image weaponIcon = default;

        public void OnSetup(string killerName, Color killerColor, string recieverName, Color recieverColor, Sprite icon)
        {
            killerNameTxt.text = killerName;
            recieverNameTxt.text = recieverName;
            weaponIcon.sprite = icon;
            killerNameTxt.color = killerColor;
            recieverNameTxt.color = recieverColor;
        }
    }
}