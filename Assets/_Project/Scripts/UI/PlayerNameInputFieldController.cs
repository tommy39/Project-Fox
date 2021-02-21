using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace IND.UI
{
    public class PlayerNameInputFieldController : MonoBehaviour
    {
        const string playerNamePrefKey = "PlayerName";
        public string setName;
        private void Start()
        {
            string defaultName = string.Empty;
            if (!string.IsNullOrEmpty(setName))
            {
                defaultName = setName;

                if (setName == "Player")
                {
                    defaultName = defaultName + Random.Range(0, 32).ToString();
                    setName = defaultName;
                }
            }
            InputField _inputField = GetComponent<InputField>();
            if (_inputField != null)
            {

                _inputField.text = defaultName;

            }
            PhotonNetwork.NickName = defaultName;
        }

        public void SetPlayerName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player Name is null or empty");
                return;
            }

            PhotonNetwork.NickName = value;
            setName = value;
            PlayerPrefs.SetString(playerNamePrefKey, value);
        }

    }
}