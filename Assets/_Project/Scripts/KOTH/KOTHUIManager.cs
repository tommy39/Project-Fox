using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace IND.KOTH
{
    public class KOTHUIManager : MonoBehaviour
    {
        [SerializeField] private Slider redTeamSlider;
        [SerializeField] private TextMeshProUGUI redTeamTextValue;

        [SerializeField] private Slider blueTeamSlider;
        [SerializeField] private TextMeshProUGUI blueTeamTextValue;

        private KOTHManager manager;

        public static KOTHUIManager singleton;
        private void Awake()
        {
            singleton = this;
        }

        private void Start()
        {
            manager = KOTHManager.singleton;
            redTeamSlider.maxValue = manager.maxScore;
            blueTeamSlider.maxValue = manager.maxScore;
        }

        public void UpdateScore(int redTeamVal, int blueTeamVal)
        {
            redTeamTextValue.text = redTeamVal.ToString();
            blueTeamTextValue.text = blueTeamVal.ToString();

            redTeamSlider.value = redTeamVal;
            blueTeamSlider.value = blueTeamVal;
        }
    }
}