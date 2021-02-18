using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
        }
        InputField _inputField = GetComponent<InputField>();
        if (_inputField != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                _inputField.text = defaultName;
            }
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
