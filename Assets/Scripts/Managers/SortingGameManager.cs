using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class SortingGameManager : GameManager
    {
        private int numberOfObjects;
        private int[] combinationPlayer1;
        private int[] combinationPlayer2;
        private int[] combinationPlayer3;
        private int[] combinationPlayer4;
        private GameObject[] scoresTeam1;
        private GameObject[] scoresTeam2;
        private bool victoryReached = false;

        public bool IsPlayerDragging { get; set; } //true if the player is dragging an object, false otherwise

        public override void Start()
        {
            base.Start();
            
            if(pvp) GameObject.Find("Living room").transform.Find("Structure").transform.Find("WallPVPDivider").gameObject.SetActive(false);
            
            AudioManager.instance.PlayBackgroundMusic();

            //synchronize the value of numberOfObjects between all clients (otherwise only the player who creates the room will have it)
            /*
        if (gameObject.GetPhotonView().isMine)
            gameObject.GetPhotonView().RPC("SetNumberOfObjects", PhotonTargets.All, PhotonManager.instance.NumberOfImages);
        */

            numberOfObjects = PhotonManager.instance.NumberOfImages;
            
            if (pvp)
            {
                //Disabling not pvp scoreboards
                GameObject.Find("Scoreboard").SetActive(false);
                GameObject.Find("Scoreboard").SetActive(false);
            }

            if (!pvp)
            {
                GameObject.Find("TeamMarker").SetActive(false);
                GameObject.Find("TeamMarker").SetActive(false);
                GameObject.Find("TeamMarker").SetActive(false);
                GameObject.Find("TeamMarker").SetActive(false);
            }
            
            scoresTeam1 = GameObject.FindGameObjectsWithTag("Score");
            scoresTeam2 = GameObject.FindGameObjectsWithTag("Score2");
            
            IsPlayerDragging = false;
        }

        void SpawnObjects()
        {
            Debug.Log("PlayerId = " + thisPlayer.GetPhotonView().OwnerActorNr);
            if (GetTeam(thisPlayer.GetPhotonView().OwnerActorNr) == 1)
            {
                for (int i = 0; i < numberOfObjects; i++)
                {
                    Vector3 objectPosition = new Vector3(Random.Range(-numberOfObjects / 2f, numberOfObjects / 2f), 1, thisPlayer.transform.position.z / 1.7f);
                    GameObject draggableObject = PhotonNetwork.Instantiate("Balloon", objectPosition, Quaternion.identity, 0);
                    draggableObject.GetPhotonView().RPC("SetIndex", RpcTarget.All, i);
                }
            }

            if (GetTeam(thisPlayer.GetPhotonView().OwnerActorNr) == 2)
            {
                for (int i = 0; i < numberOfObjects; i++)
                {
                    Vector3 objectPosition = new Vector3(17f + Random.Range(-numberOfObjects / 2f, numberOfObjects / 2f), 1, thisPlayer.transform.position.z / 1.7f);
                    GameObject draggableObject = PhotonNetwork.Instantiate("Balloon", objectPosition, Quaternion.identity, 0);
                    draggableObject.GetPhotonView().RPC("SetIndex", RpcTarget.All, i);
                }
            }
            
            Instantiate(Resources.Load<GameObject>("WaitingAnimation"), Vector3.zero, Quaternion.identity);
        }

        void SpawnAnchorPoints()
        {
            for (int i=0; i<numberOfObjects; i++)
            {
                GameObject anchorPointPlayer1 = PhotonNetwork.Instantiate("AnchorPoint", new Vector3(numberOfObjects * -0.75f + 0.75f + 1.5f * i, 1.7f, 7), Quaternion.identity, 0);
                anchorPointPlayer1.GetPhotonView().RPC("SetIndex", RpcTarget.All, i);
                GameObject anchorPointPlayer2 = PhotonNetwork.Instantiate("AnchorPoint", new Vector3(numberOfObjects * -0.75f + 0.75f + 1.5f * i, 1.7f, -7), Quaternion.identity, 0);
                anchorPointPlayer2.GetPhotonView().RPC("SetIndex", RpcTarget.All, numberOfObjects - i -1);
                if (pvp)
                {
                    GameObject anchorPointPlayer3 = PhotonNetwork.Instantiate("AnchorPoint", new Vector3(17f + numberOfObjects * -0.75f + 0.75f + 1.5f * i, 1.7f, 7), Quaternion.identity, 0);
                    anchorPointPlayer3.GetPhotonView().RPC("SetIndex", RpcTarget.All, i);
                    GameObject anchorPointPlayer4 = PhotonNetwork.Instantiate("AnchorPoint", new Vector3(17f + numberOfObjects * -0.75f + 0.75f + 1.5f * i, 1.7f, -7), Quaternion.identity, 0);
                    anchorPointPlayer4.GetPhotonView().RPC("SetIndex", RpcTarget.All, numberOfObjects - i -1);
                }
            }
        }

        [PunRPC]
        void UpdateScore(string scoreString, int team)
        {
            if (team == 1)
            {
                foreach (GameObject score in scoresTeam1)
                {
                    score.GetComponent<TextMeshPro>().text = scoreString;
                }
            }
            
            if (team == 2)
            {
                foreach (GameObject score in scoresTeam2)
                {
                    score.GetComponent<TextMeshPro>().text = scoreString;
                }
            }
        }

        [PunRPC]
        public void OnObjectPositioned(int playerId, int objectIndex, int anchorPointIndex)
        {
            if (this.gameObject.GetPhotonView().IsMine)
            {
                if (playerId == this.gameObject.GetPhotonView().OwnerActorNr)
                    combinationPlayer1[anchorPointIndex] = objectIndex + 1;
                else
                {
                    if (playerId == 2) combinationPlayer2[anchorPointIndex] = objectIndex + 1;
                    if (playerId == 3) combinationPlayer3[anchorPointIndex] = objectIndex + 1;
                    if (playerId == 4) combinationPlayer4[anchorPointIndex] = objectIndex + 1;
                }
                
                int numberOfCorrectObjectsTeam1 = 0;
                int numberOfCorrectObjectsTeam2 = 0;
                
                for (int i = 0; i < numberOfObjects; i++)
                {
                    if (combinationPlayer1[i] == combinationPlayer2[i] && combinationPlayer1[i] != 0)
                        numberOfCorrectObjectsTeam1++;
                }

                Debug.Log("Number of correct obj Team 1: " + numberOfCorrectObjectsTeam1);
                
                for (int i = 0; i < numberOfObjects; i++)
                {
                    if (combinationPlayer3[i] == combinationPlayer4[i] && combinationPlayer3[i] != 0)
                        numberOfCorrectObjectsTeam2++;
                }
                
                Debug.Log("Number of correct obj Team 2: " + numberOfCorrectObjectsTeam2);
                
                int numberOfCorrectObjects = GetTeam(playerId) == 1 ? numberOfCorrectObjectsTeam1 : numberOfCorrectObjectsTeam2;
                this.gameObject.GetPhotonView().RPC("UpdateScore", RpcTarget.All,
                    numberOfCorrectObjects + "/" + numberOfObjects, GetTeam(playerId));

                if (numberOfCorrectObjectsTeam1 == numberOfObjects && numberOfCorrectObjectsTeam2 == numberOfObjects && pvp)
                {
                    this.gameObject.GetPhotonView().RPC("StartVictoryAnimations", RpcTarget.All);
                    return;
                }
                if (numberOfCorrectObjectsTeam1 == numberOfObjects && pvp && !victoryReached)
                {
                    this.gameObject.GetPhotonView().RPC("PvpHurraySound", RpcTarget.All, 1);
                    victoryReached = true;
                }
                if (numberOfCorrectObjectsTeam2 == numberOfObjects && !victoryReached)
                {
                    this.gameObject.GetPhotonView().RPC("PvpHurraySound", RpcTarget.All, 2);
                    victoryReached = true;
                }
                if (numberOfCorrectObjectsTeam1 == numberOfObjects && !pvp)
                    this.gameObject.GetPhotonView().RPC("StartVictoryAnimations", RpcTarget.All);
            }
        }

        [PunRPC]
        public void OnObjectRemoved(int playerId, int anchorPointIndex)
        {
            if (this.gameObject.GetPhotonView().IsMine)
            {
                if (playerId == this.gameObject.GetPhotonView().OwnerActorNr)
                    combinationPlayer1[anchorPointIndex] = 0;
                else
                {
                    if (playerId == 2) combinationPlayer2[anchorPointIndex] = 0;
                    if (playerId == 3) combinationPlayer3[anchorPointIndex] = 0;
                    if (playerId == 4) combinationPlayer4[anchorPointIndex] = 0;
                }

                int numberOfCorrectObjectsTeam1 = 0;
                int numberOfCorrectObjectsTeam2 = 0;

            
                for (int i = 0; i < numberOfObjects; i++)
                {
                    if (combinationPlayer1[i] == combinationPlayer2[i] && combinationPlayer1[i] != 0)
                        numberOfCorrectObjectsTeam1++;
                }

                for (int i = 0; i < numberOfObjects; i++)
                {
                    if (combinationPlayer3[i] == combinationPlayer4[i] && combinationPlayer3[i] != 0)
                        numberOfCorrectObjectsTeam2++;
                }

                int numberOfCorrectObjects = GetTeam(playerId) == 1 ? numberOfCorrectObjectsTeam1 : numberOfCorrectObjectsTeam2;
                this.gameObject.GetPhotonView().RPC("UpdateScore", RpcTarget.All,
                    numberOfCorrectObjects + "/" + numberOfObjects, GetTeam(playerId));
                
                if (numberOfCorrectObjectsTeam1 == numberOfObjects && numberOfCorrectObjectsTeam2 == numberOfObjects && pvp)
                {
                    this.gameObject.GetPhotonView().RPC("StartVictoryAnimations", RpcTarget.All);
                    return;
                }

                if (numberOfCorrectObjectsTeam1 == numberOfObjects && pvp && !victoryReached)
                {
                    this.gameObject.GetPhotonView().RPC("PvpHurraySound", RpcTarget.All, 1);
                    victoryReached = true;
                }

                if (numberOfCorrectObjectsTeam2 == numberOfObjects && !victoryReached)
                {
                    this.gameObject.GetPhotonView().RPC("PvpHurraySound", RpcTarget.All, 2);
                    victoryReached = true;
                }
                if (numberOfCorrectObjectsTeam1 == numberOfObjects && !pvp)
                    this.gameObject.GetPhotonView().RPC("StartVictoryAnimations", RpcTarget.All);
            }
        }

        /*
    [PunRPC]
    void SetNumberOfObjects(int number)
    {
        numberOfObjects = number;
    }
    */
        public int GetTeam(int playerId)
        {
            if (playerId >= 1 && playerId <= 2) return 1;
            if (playerId >= 3 && playerId <= 4) return 2;
            return 0;
        }

        protected override void SetUpGame()
        {
            SpawnObjects();

            if (this.gameObject.GetPhotonView().IsMine)
            {
                combinationPlayer1 = new int[numberOfObjects];
                combinationPlayer2 = new int[numberOfObjects];
                combinationPlayer3 = new int[numberOfObjects];
                combinationPlayer4 = new int[numberOfObjects];

                this.gameObject.GetPhotonView().RPC("UpdateScore", RpcTarget.All, "0/" + numberOfObjects, 1); 
                this.gameObject.GetPhotonView().RPC("UpdateScore", RpcTarget.All, "0/" + numberOfObjects, 2);

                SpawnAnchorPoints();
            }
        }
    }
}
