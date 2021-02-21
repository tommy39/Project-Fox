using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IND.PlayerSys
{
    public static class PlayerAnimatorStatics
    {
        public static int verticalMovementInt;
        public static int horizontalMovementInt;
        public static int postureStateInt;

        public static int isAimingAnimBool;
        public static int isFiringAnimBool;
        public static int isReloadingAnimBool;
        public static int isSprintingAnimBool;

        public static int equipWeaponAnimClass;

        public static string weaponAnimClassPrefix = "Weapon_";

        public static bool hasBeenInitialized = false;
        public static void Initialize()
        {
            if (hasBeenInitialized == true)
                return;

            horizontalMovementInt = Animator.StringToHash("Horizontal");
            verticalMovementInt = Animator.StringToHash("Vertical");
            postureStateInt = Animator.StringToHash("PostureState");

            isFiringAnimBool = Animator.StringToHash("IsFiring");
            isReloadingAnimBool = Animator.StringToHash("IsReloading");
            isSprintingAnimBool = Animator.StringToHash("IsSprinting");
            isAimingAnimBool = Animator.StringToHash("IsAiming");

            equipWeaponAnimClass = Animator.StringToHash("Equip Weapon");
        }
    }
}