using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IND.Player
{
    public static class PlayerAnimatorStatics
    {
        public static int verticalMovementInt;
        public static int horizontalMovementInt;
        public static int postureStateInt;

        public static int isAimingAnimBool;
        public static int isFiringAnimBool;

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
            isAimingAnimBool = Animator.StringToHash("IsAiming");

            equipWeaponAnimClass = Animator.StringToHash("Equip Weapon");
        }
    }
}