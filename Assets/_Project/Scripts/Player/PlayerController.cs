using Cinemachine;
using IND.Network;
using IND.Teams;
using Photon.Pun;
using UnityEngine;
using IND.UI;

namespace IND.PlayerSys
{
    public class PlayerController : MonoBehaviourPun
    {
        public TeamType teamType;
        public ClientController clientController;

        public Transform cameraHealthTrackerRayCastTarget;

        [SerializeField] private SkinnedMeshRenderer surfaceMesh;
        [SerializeField] private SkinnedMeshRenderer jointsMesh;

        [SerializeField] private Material redSurfaceMat;
        [SerializeField] private Material redJointsMat;
        [SerializeField] private Material blueSurfaceMat = default;
        [SerializeField] private Material blueJointsMat = default;

        private PlayerHealthTrackerUIController healthTrackerUI;

        private Camera cam;
        private CinemachineVirtualCamera vcam;
        private PlayerManager playerManager;

        private void Start()
        {
            playerManager = PlayerManager.singleton;
            if (photonView.IsMine)
            {
                cam = FindObjectOfType<CamController>().GetComponent<Camera>();
                vcam = FindObjectOfType<CinemachineVirtualCamera>();
                vcam.Follow = transform;
            }
            AssignClientController(photonView.ControllerActorNr);
            AssignPlayerCharacterToClient();
            CreateHealthTracker();
        }

        public void OnSpawn(TeamType team)
        {
            photonView.RPC("AssignTeam", RpcTarget.AllBuffered, team);
            photonView.RPC("AssignMaterials", RpcTarget.AllBuffered);
        }

        private void CreateHealthTracker()
        {
           healthTrackerUI =  HealthTrackerUIManager.singleton.CreateTracker(this);
        }
        
        private void AssignClientController(int id)
        {
            clientController = ClientManager.singleton.GetClientByID(id);
        }

        private void AssignPlayerCharacterToClient()
        {
            clientController.data.characterController = this;
        }

        [PunRPC]
        private void AssignTeam(TeamType type)
        {
            teamType = type;
        }

        [PunRPC]
        void AssignMaterials()
        {
            switch (teamType)
            {
                case TeamType.SPEC:
                    break;
                case TeamType.BLUE:
                    surfaceMesh.sharedMaterial = blueSurfaceMat;
                    jointsMesh.sharedMaterial = blueJointsMat;
                    break;
                case TeamType.RED:
                    surfaceMesh.sharedMaterial = redSurfaceMat;
                    jointsMesh.sharedMaterial = redJointsMat;
                    break;
                default:
                    break;
            }
        }

    }
}