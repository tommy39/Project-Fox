using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IND.Player
{
    public static class PlayerStatics
    {
        public static void RespawnPlayer()
        {
            GameObject geo = Object.Instantiate(PlayerManager.singleton.playerPrefab);
        }
    }
}