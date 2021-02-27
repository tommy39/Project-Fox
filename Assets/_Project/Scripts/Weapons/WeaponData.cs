using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IND.Weapons
{
    [CreateAssetMenu(fileName = "Weapon_", menuName = "IND/Weapons/Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        public string weaponName;
        public string weaponDescription;
        public GameObject modelPrefab;
        public int weaponAnimID;

        [Header("Fire Data")]
        public WeaponFireRate fireRateMode;
        public float firingWeaponAnimTimer = 0.1f;
        public float fireRate = 0.1f;
        public float reloadDuration = 1f;
        public int maxMagazineAmmo = 5;
        public float minDistanceFromWallToShoot = 1f;

        [Header ("Aim Radius")]
        public float increaseAimRadiusWhenShotAmount = 0.2f;
        public float aimContractionCooldown = 0.15f;
        public float effectiveRange = 20f;
        public float radiusIncreaseForOutsideEffectiveRangePerMetre = 0.1f;

        [Header("Damage")]
        public float weaponDamage = 100f;

        [Header("Bullet Mode")]
        public WeaponBulletType bulletType;
        public int bulletAmounts = 1;
        public float spreadAngle = 0f;

        [Header ("UI")]
        public Sprite weaponDiagonalIcon;
        public Sprite weaponHorizontalIcon;

        [Header("3D Weapon Render")]
        public Vector3 renderPos;
        public Quaternion renderRot;

    }
}