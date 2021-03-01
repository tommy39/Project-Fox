using Photon.Pun;
using UnityEngine;
using Cinemachine;

namespace IND.PlayerSys
{
    public class PlayerCameraAimController : MonoBehaviourPun
    {
        public Transform playerFocusPoint;
        private Transform aimTarget;

        private float curDistance;
        private Vector3 eulerRotation;

        public float focusPointMoveSpeedRegular = 5f;
        public float focusPointFastReturnSpeed = 20f;

        public float nonAimLookaheadTime = 0.1f;
        public float aimLookahedTime = 0.081f;

        public float deadZone1Distance = 8f;

        private void Start()
        {
            if (photonView.IsMine == false)
                return;

            aimTarget = GetComponent<PlayerAimController>().aimTarget;
        }

        public void SetCameraAiming(CinemachineVirtualCamera vcam, CinemachineFramingTransposer framingTransposer)
        {
            vcam.Follow = playerFocusPoint;
            framingTransposer.m_LookaheadTime = aimLookahedTime;
        }

        public void SetCameraRegular(CinemachineVirtualCamera vcam, CinemachineFramingTransposer framingTransposer)
        {
            vcam.Follow = transform;
            framingTransposer.m_LookaheadTime = nonAimLookaheadTime;
        }

        private void Update()
        {
            if (photonView.IsMine == false)
                return;

            if(aimTarget == null || playerFocusPoint == null)
            {
                Destroy(this);
                return;
            }

            RotateObjectTowardsMouse();
            MoveObjectForward();
        }

        void RotateObjectTowardsMouse()
        {
            eulerRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
            playerFocusPoint.rotation = Quaternion.Euler(eulerRotation);
        }

        void MoveObjectForward()
        {
            curDistance = Vector3.Distance(new Vector3(transform.position.x, playerFocusPoint.position.y, transform.position.z), playerFocusPoint.position);

            if (curDistance > deadZone1Distance)
            {
                if (curDistance > deadZone1Distance + 1f)
                {
                    playerFocusPoint.position = Vector3.MoveTowards(playerFocusPoint.position, new Vector3(transform.position.x, playerFocusPoint.position.y, transform.position.z), (focusPointFastReturnSpeed * Time.deltaTime));
                }
                else
                {
                    playerFocusPoint.position = Vector3.MoveTowards(playerFocusPoint.position, new Vector3(transform.position.x, playerFocusPoint.position.y, transform.position.z), (focusPointMoveSpeedRegular * Time.deltaTime));
                }
            }
            else
            {
                playerFocusPoint.position = Vector3.MoveTowards(playerFocusPoint.position, new Vector3(aimTarget.position.x, playerFocusPoint.position.y, aimTarget.position.z), (focusPointMoveSpeedRegular * Time.deltaTime));
            }

        }
    }
}