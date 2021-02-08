using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IND.Weapons
{
    [CreateAssetMenu(fileName = "Weapon_", menuName = "IND/Weapons/Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        public string weaponName;
        public GameObject modelPrefab;
        public int weaponAnimID;
        public float firingWeaponAnimTimer = 0.1f;
    }
}