using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

namespace IND.Network
{
    public class ClientManager : MonoBehaviourPun
    {
       public List<ClientController> allClients = new List<ClientController>();

        [SerializeField] private GameObject clientPrefab = default;
        public static ClientController LocalPlayerClientControllerInstance;


        public static ClientManager singleton;
        private void Awake()
        {
            singleton = this;
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {            
            StartCoroutine(DelayedStart());
        }

        private IEnumerator DelayedStart()
        {
            yield return new WaitForSeconds(0.1f);
            GameObject clientGEO = PhotonNetwork.Instantiate(clientPrefab.name, Vector3.zero, Quaternion.identity, 0);
        }

        public void AddNewClient(ClientController controller)
        {
            allClients.Add(controller);
        }

        public ClientController GetClientByID(int id)
        {
            for (int i = 0; i < allClients.Count; i++)
            {
                if (allClients[i].data.clientID == id)
                {
                    return allClients[i];
                }
            }

            return null;
        }
    }
}

