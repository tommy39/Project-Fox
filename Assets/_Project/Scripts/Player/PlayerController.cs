using UnityEngine;
using Cinemachine;
using IND.Teams;

namespace IND.Player
{
    public class PlayerController : MonoBehaviour
    {
        public TeamType teamType;

        [SerializeField] private SkinnedMeshRenderer surfaceMesh;
        [SerializeField] private SkinnedMeshRenderer jointsMesh;

        [SerializeField] private Material redSurfaceMat;
        [SerializeField] private Material redJointsMat;
        [SerializeField] private Material blueSurfaceMat;
        [SerializeField] private Material blueJointsMat;


        private Camera cam;
        private CinemachineVirtualCamera vcam;
        private PlayerManager playerManager;

        public void OnSpawn(TeamType team, PlayerManager manager)
        {
            teamType = team;
            playerManager = manager;
            cam = FindObjectOfType<CinemachineBrain>().GetComponent<Camera>();
            vcam = FindObjectOfType<CinemachineVirtualCamera>();
            vcam.Follow = transform;
            AssignMaterials();
        }          

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