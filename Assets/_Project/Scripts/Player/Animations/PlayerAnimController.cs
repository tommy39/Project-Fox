using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IND.Player
{
    public class PlayerAnimController : MonoBehaviour
    {
        private Animator anim;

        private void Awake()
        {
            anim = GetComponentInChildren<Animator>();
            PlayerAnimatorStatics.Initialize();
        }

        public void OnDeath()
        {
            Destroy(anim);
            Destroy(this);
        }

        public bool GetAnimBoolState(int id)
        {
            return anim.GetBool(id);
        }
        public void SetAnimBool(int id, bool value)
        {
            anim.SetBool(id, value);
        }
        public void SetAnimFloat(int id, float value)
        {
            anim.SetFloat(id, value);
        }
        public void SetAnimInt(int id, int value)
        {
            anim.SetInteger(id, value);
        }
        public void SetAnimFloatAdvanced(int id, float value, float dampTime, float deltaTime)
        {
            anim.SetFloat(id, value, dampTime, deltaTime);
        }
        public void SetAnimatorSpeed(float value)
        {
            anim.speed = value;
        }
        public void PlayAnimationHash(int id)
        {
            anim.Play(id);
        }
        public void PlayAnimationString(string id)
        {
            anim.Play(id);
        }
        public void PlayAnimationStringAtSpecificTime(string id, int Layer, float normalizedTime)
        {
            anim.Play(id, Layer, normalizedTime);
        }
        public void EnableRootMotion()
        {
            anim.applyRootMotion = true;
        }
        public void DisableRootMotion()
        {
            anim.applyRootMotion = false;
        }
        public int GetAnimHash(string name)
        {
            return Animator.StringToHash(name);
        }
        public void DisableAnimator()
        {
            anim.enabled = false;
        }
    }
}