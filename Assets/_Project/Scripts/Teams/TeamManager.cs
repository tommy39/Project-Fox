using UnityEngine;

namespace IND.Teams
{
    public class TeamManager : MonoBehaviour
    {
        [SerializeField] private BoxCollider redTeamSpawn = default;
        [SerializeField] private BoxCollider blueTeamSpawn = default;
        [SerializeField] private BoxCollider specTeamSpawn = default;

        public Vector3 GetSpawnPos(TeamType teamType)
        {
            switch (teamType)
            {
                case TeamType.SPEC:
                    return Vector3.zero;
                case TeamType.BLUE:
                    return GetSpawnPosInCollider(blueTeamSpawn);
                case TeamType.RED:
                    return GetSpawnPosInCollider(redTeamSpawn);
            }

            return Vector3.zero;
        }

        private Vector3 GetSpawnPosInCollider(BoxCollider collider)
        {
            return new Vector3(Random.Range(collider.bounds.min.x, collider.bounds.max.x),
        Random.Range(collider.bounds.min.y, collider.bounds.max.y),
        Random.Range(collider.bounds.min.z, collider.bounds.max.z));
        }
    }
}