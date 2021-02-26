using IND.PlayerSys;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IND.KOTH
{
    public class KOTHFlagController : MonoBehaviourPun
    {
        [SerializeField] private GameObject flagObject;
        [SerializeField] private Transform flagMaxPos;
        [SerializeField] private Transform flagMinPos;
        [SerializeField] private float flagMoveSpeed = 10;
        [SerializeField] private Color redFlagColor;
        [SerializeField] private Color blueFlagColor;
        [SerializeField] private Color neutralFlagColor;
        private SkinnedMeshRenderer flagMeshRenderer;

        public int flagCurrentCapValue = 100;
        public KOTHTeamType currentFlagState = KOTHTeamType.Neutral;
        public KOTHFlagOwnerType flagOwnerType;
        public KOTHCappingState cappingState;
        public bool flagHasBeenCappedFully = false;

        public List<PlayerController> redTeamPlayersInZone = new List<PlayerController>();
        public List<PlayerController> blueTeamPlayersInZone = new List<PlayerController>();

        private bool isFlagMovingUp = false;
        private bool isFlagMovingDown = false;

        [SerializeField] private float flagTimeTicker = 1;

        private float distanceBetweenMaxAndMin;
        private float amountToMovePerValueOfOne;

        private bool isTickingScore = false;
        [SerializeField] private float tickScoreTimer = 1;
        [SerializeField] private int increaseScoreAmount = 1;

        private KOTHUIManager uiManager;
        private KOTHManager gameModeManager;

        private void Awake()
        {

        }

        private void Start()
        {
            uiManager = KOTHUIManager.singleton;
            gameModeManager = KOTHManager.singleton;
            flagMeshRenderer = flagObject.GetComponentInChildren<SkinnedMeshRenderer>();
            distanceBetweenMaxAndMin = Vector3.Distance(flagMaxPos.position, flagMinPos.position);
            amountToMovePerValueOfOne = distanceBetweenMaxAndMin / 100;
            ChangeFlagColor(neutralFlagColor);
        }

        private void Update()
        {
            UpdateCappingState();

            switch (currentFlagState)
            {
                case KOTHTeamType.Neutral:
                    HandleNeutralState();
                    break;
                case KOTHTeamType.TeamOwned:
                    HandleTeamOwnedState();
                    break;
            }

            HandleScore();
        }

        private void HandleScore()
        {
            if (currentFlagState != KOTHTeamType.TeamOwned || flagHasBeenCappedFully != true)
            {
                if (isTickingScore == true)
                {
                    isTickingScore = false;
                    StopCoroutine(TickScore());
                }
                return;
            }

            if (isTickingScore == true)
                return;

            isTickingScore = true;
            StartCoroutine(TickScore());
        }

        private IEnumerator TickScore()
        {
            while (isTickingScore == true)
            {
                yield return new WaitForSeconds(tickScoreTimer);
                if (photonView.IsMine == true)
                {

                    switch (flagOwnerType)
                    {
                        case KOTHFlagOwnerType.none:
                            break;
                        case KOTHFlagOwnerType.BlueTeam:
                            object[] rpcData = { 0, increaseScoreAmount };
                            gameModeManager.photonView.RPC("AddScore", RpcTarget.AllBuffered, rpcData);
                            // gameModeManager.AddScore(0, increaseScoreAmount);
                            break;
                        case KOTHFlagOwnerType.RedTeam:
                            object[] rpcRedData = { increaseScoreAmount, 0 };
                            gameModeManager.photonView.RPC("AddScore", RpcTarget.AllBuffered, rpcRedData);
                          //  gameModeManager.AddScore(increaseScoreAmount, 0);
                            break;
                    }
                }
            }
        }

        private void UpdateCappingState()
        {
            if (redTeamPlayersInZone.Count == 0 && blueTeamPlayersInZone.Count == 0)
            {
                cappingState = KOTHCappingState.NoTeamCapping;
                return;
            }


            if (redTeamPlayersInZone.Count == blueTeamPlayersInZone.Count)
            {
                cappingState = KOTHCappingState.Contested;
                return;
            }
            else if (redTeamPlayersInZone.Count > blueTeamPlayersInZone.Count)
            {
                if (flagOwnerType == KOTHFlagOwnerType.RedTeam && flagHasBeenCappedFully)
                {
                    cappingState = KOTHCappingState.NoTeamCapping;
                    return;
                }

                cappingState = KOTHCappingState.RedTeamCapping;
                return;
            }
            else if (blueTeamPlayersInZone.Count > redTeamPlayersInZone.Count)
            {
                if (flagOwnerType == KOTHFlagOwnerType.BlueTeam && flagHasBeenCappedFully)
                {
                    cappingState = KOTHCappingState.NoTeamCapping;
                    return;
                }

                cappingState = KOTHCappingState.BlueTeamCapping;
                return;
            }
        }

        private void HandleNeutralState()
        {
            if (cappingState == KOTHCappingState.NoTeamCapping)
            {
                if (flagCurrentCapValue != 100)
                {
                    MoveFlagUp();
                }
            }
            else if (cappingState == KOTHCappingState.BlueTeamCapping || cappingState == KOTHCappingState.RedTeamCapping)
            {
                //Move Flag Down
                MoveFlagDown();
            }
        }

        private void HandleTeamOwnedState()
        {
            if (cappingState == KOTHCappingState.NoTeamCapping)
            {
                if (flagOwnerType == KOTHFlagOwnerType.RedTeam || flagOwnerType == KOTHFlagOwnerType.BlueTeam && flagHasBeenCappedFully)
                {
                    if (flagCurrentCapValue != 100)
                    {
                        MoveFlagUp();
                    }

                }
                else
                {
                    if (flagCurrentCapValue != 0)
                    {
                        MoveFlagDown();
                    }
                }
            }
            else if (cappingState == KOTHCappingState.BlueTeamCapping)
            {
                switch (flagOwnerType)
                {
                    case KOTHFlagOwnerType.none:
                        break;
                    case KOTHFlagOwnerType.BlueTeam:
                        if (flagCurrentCapValue != 100)
                        {
                            MoveFlagUp();
                        }
                        break;
                    case KOTHFlagOwnerType.RedTeam:
                        if (flagCurrentCapValue != 0)
                        {
                            MoveFlagDown();
                        }
                        break;
                }
            }
            else if (cappingState == KOTHCappingState.RedTeamCapping)
            {
                switch (flagOwnerType)
                {
                    case KOTHFlagOwnerType.none:
                        break;
                    case KOTHFlagOwnerType.BlueTeam:
                        if (flagCurrentCapValue != 0)
                        {
                            MoveFlagDown();
                        }
                        break;
                    case KOTHFlagOwnerType.RedTeam:
                        if (flagCurrentCapValue != 100)
                        {
                            MoveFlagUp();
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void MoveFlagUp()
        {
            if (isFlagMovingUp == true)
                return;

            if (isFlagMovingDown == true)
            {
                StopCoroutine(MoveFlagDownTicker());
                isFlagMovingDown = false;
            }

            isFlagMovingUp = true;
            StartCoroutine(MoveFlagUpTicker());
        }

        private void MoveFlagDown()
        {
            if (isFlagMovingDown == true)
                return;


            if (isFlagMovingUp == true)
            {
                StopCoroutine(MoveFlagDownTicker());
                isFlagMovingUp = false;
            }

            isFlagMovingDown = true;
            StartCoroutine(MoveFlagDownTicker());
        }

        private IEnumerator MoveFlagDownTicker()
        {
            while (isFlagMovingDown)
            {
                yield return new WaitForSecondsRealtime(flagTimeTicker);

                if (photonView.IsMine)
                {
                    photonView.RPC("OnFlagMovedDown", RpcTarget.AllBuffered);
                }
            }
        }

        [PunRPC]
        private void OnFlagMovedDown()
        {
            flagObject.transform.position -= new Vector3(0, amountToMovePerValueOfOne, 0);
            flagCurrentCapValue--;
            if (flagCurrentCapValue == 0)
            {
                OnFlagReachedZero();
            }
        }

        private IEnumerator MoveFlagUpTicker()
        {
            while (isFlagMovingUp == true)
            {
                yield return new WaitForSecondsRealtime(flagTimeTicker);

                photonView.RPC("OnFlagMovedUp", RpcTarget.AllBuffered);
            }
        }

        [PunRPC]
        private void OnFlagMovedUp()
        {
            flagObject.transform.position += new Vector3(0, amountToMovePerValueOfOne, 0);
            flagCurrentCapValue++;
            if (flagCurrentCapValue >= 100)
            {
                OnFlagReachedMax();
            }
        }

        private void OnFlagReachedZero()
        {
            isFlagMovingDown = false;
            StopCoroutine(MoveFlagDownTicker());
            flagHasBeenCappedFully = false;

            if (photonView.IsMine == true)
            {
                switch (currentFlagState) //Current Flag State Before Capture
                {
                    case KOTHTeamType.Neutral:
                        if (cappingState == KOTHCappingState.BlueTeamCapping)
                        {
                            photonView.RPC("ChangeFlagOwner", RpcTarget.AllBuffered, KOTHFlagOwnerType.BlueTeam);
                        }
                        else if (cappingState == KOTHCappingState.RedTeamCapping)
                        {
                            photonView.RPC("ChangeFlagOwner", RpcTarget.AllBuffered, KOTHFlagOwnerType.RedTeam);
                        }
                        break;
                    case KOTHTeamType.TeamOwned:
                        if (cappingState == KOTHCappingState.NoTeamCapping)
                        {
                            //Set Flag To Become Neutral
                            photonView.RPC("ChangeFlagOwner", RpcTarget.AllBuffered, KOTHFlagOwnerType.none);
                        }
                        else
                        {
                            if (cappingState == KOTHCappingState.BlueTeamCapping)
                            {
                                photonView.RPC("ChangeFlagOwner", RpcTarget.AllBuffered, KOTHFlagOwnerType.BlueTeam);
                            }
                            else if (cappingState == KOTHCappingState.RedTeamCapping)
                            {
                                photonView.RPC("ChangeFlagOwner", RpcTarget.AllBuffered, KOTHFlagOwnerType.RedTeam);
                            }
                        }
                        break;
                }
            }
        }

        private void OnFlagReachedMax()
        {
            isFlagMovingUp = false;
            StopCoroutine(MoveFlagUpTicker());
            flagCurrentCapValue = 100;

            switch (flagOwnerType)
            {
                case KOTHFlagOwnerType.none:
                    flagHasBeenCappedFully = false;
                    break;
                case KOTHFlagOwnerType.BlueTeam:
                    flagHasBeenCappedFully = true;
                    break;
                case KOTHFlagOwnerType.RedTeam:
                    flagHasBeenCappedFully = true;
                    break;
            }
        }

        [PunRPC]
        private void ChangeFlagOwner(KOTHFlagOwnerType owner)
        {
            flagOwnerType = owner;
            switch (owner)
            {
                case KOTHFlagOwnerType.none: //Set To Neutral
                    currentFlagState = KOTHTeamType.Neutral;
                    ChangeFlagColor(neutralFlagColor);
                    break;
                case KOTHFlagOwnerType.RedTeam:
                    currentFlagState = KOTHTeamType.TeamOwned;
                    ChangeFlagColor(redFlagColor);
                    break;
                case KOTHFlagOwnerType.BlueTeam:
                    currentFlagState = KOTHTeamType.TeamOwned;
                    ChangeFlagColor(blueFlagColor);
                    break;
            }
        }

        private void ChangeFlagColor(Color targetColor)
        {
            flagMeshRenderer.sharedMaterials[0].color = targetColor;
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerController controller = other.GetComponentInParent<PlayerController>();
            switch (controller.teamType)
            {
                case Teams.TeamType.SPEC:
                    break;
                case Teams.TeamType.BLUE:
                    if (!blueTeamPlayersInZone.Contains(controller))
                    {
                        blueTeamPlayersInZone.Add(controller);
                    }
                    break;
                case Teams.TeamType.RED:
                    if (!redTeamPlayersInZone.Contains(controller))
                    {
                        redTeamPlayersInZone.Add(controller);
                    }
                    break;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            PlayerController controller = other.GetComponentInParent<PlayerController>();
            switch (controller.teamType)
            {
                case Teams.TeamType.SPEC:
                    break;
                case Teams.TeamType.BLUE:
                    if (blueTeamPlayersInZone.Contains(controller))
                    {
                        blueTeamPlayersInZone.Remove(controller);
                    }
                    break;
                case Teams.TeamType.RED:
                    if (redTeamPlayersInZone.Contains(controller))
                    {
                        redTeamPlayersInZone.Remove(controller);
                    }
                    break;
            }
        }
    }
}