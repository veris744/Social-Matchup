using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Managers
{
    public class TeamPuzzle
    {
        List<int> pieces;
        public TeamPuzzle(int size)
        {
            pieces = new List<int>(size);
        }

        public List<int> GetPieces()
        {
            return pieces;
        }
    }
    
    public class PuzzlingGameManager : GameManager
    {
        private int numberOfObjects;
        private List<TeamPuzzle> teamPuzzles;
        private Texture2D tex;
        private float realWidth;
        private float realHeight;
        private List<int> randomValues = null;
        private int highRangeLimit;
        private int correctPiecesPlayer1;
        private int correctPiecesPlayer2;
        private int correctPiecesPlayer3;
        private int correctPiecesPlayer4;

        private List<GameObject> hintPuzzle = new List<GameObject>();

        public bool IsPlayerDragging { get; set; } //true if the player is dragging an object, false otherwise

        private void Start()
        {
            base.Start();
            if (pvp)
            {
                GameObject.Find("WallPVPDivider").SetActive(false);
                GameObject.Find("HintButtonSingle").SetActive(false);
            }
            else
            {
                GameObject.Find("HintButtonPVP").SetActive(false);
                GameObject.Find("HintButtonPVP2").SetActive(false);
            }
            teamPuzzles = new List<TeamPuzzle>(2);
            teamPuzzles.Add(new TeamPuzzle((int)Math.Sqrt(numberOfObjects)));
            AudioManager.instance.PlayBackgroundMusic();

            //synchronize the value of numberOfObjects between all clients (otherwise only the player who creates the room will have it)
            /*
        if (gameObject.GetPhotonView().isMine)
            gameObject.GetPhotonView().RPC("SetNumberOfObjects", PhotonTargets.All, PhotonManager.instance.NumberOfImages);
        */

            numberOfObjects = PhotonManager.instance.NumberOfImages;
            IsPlayerDragging = false;
        }

        void SpawnObjects()
        {
            Debug.Log("Number of Objects: " + numberOfObjects);
            int rows = Mathf.RoundToInt(Mathf.Sqrt(numberOfObjects));
            int cols = rows;        //TODO initialized for not square puzzles
            int width = Mathf.FloorToInt((float)tex.width / cols);    //dimensions in #pixels
            int height = Mathf.FloorToInt((float)tex.height / rows);
            Vector3 player1PiecesPosition = GameObject.Find("Player1PuzzlePieces").transform.position,
                    player2PiecesPosition = GameObject.Find("Player2PuzzlePieces").transform.position;
            
            realWidth = (float) 4*tex.width / (tex.height * cols);    //real dimensions
            realHeight = (float) 4*tex.height / (tex.height * rows);

            
            if (pvp)
            {
                Vector3 fullPuzzlePosition1 = GameObject.Find("FullPuzzlePvp1").transform.position;
                Vector3 fullPuzzlePosition2 = GameObject.Find("FullPuzzlePvp2").transform.position;
                PhotonNetwork.Instantiate("FullHintPuzzle", fullPuzzlePosition1, Quaternion.Euler(0,0,0), 0);
                PhotonNetwork.Instantiate("FullHintPuzzle", fullPuzzlePosition2, Quaternion.Euler(0,180,0), 0);
            }
            else
            {
                Vector3 fullPuzzlePosition = GameObject.Find("FullPuzzle").transform.position;
                PhotonNetwork.Instantiate("FullHintPuzzle", fullPuzzlePosition, Quaternion.Euler(0,90,0), 0);
            }
            
            this.gameObject.GetPhotonView().RPC("InstantiateHintPuzzle", RpcTarget.All,realHeight * rows, realWidth * cols);

                for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                {
                    int pieceOwnerId = FindPieceOwner();
                    //Vector3 position = new Vector3(-10.2f,1f + i*realHeight,-0.7f + (pieceOwnerId==((int)0)?(2f+j*realWidth):(-4f-j*realWidth)));
                    Vector3 position = pieceOwnerId == 0 ? (player1PiecesPosition + new Vector3(0,i*realHeight, -j*realWidth)): 
                                                           (player2PiecesPosition + new Vector3(0,i*realHeight, -j*realWidth)); 
                    Quaternion rotation = Quaternion.Euler(0,0,0);
                    
                    if (pvp)
                    {
                        Vector3 position2;
                        Vector3 player3PiecesPosition = GameObject.Find("Player3PuzzlePieces").transform.position,
                                player4PiecesPosition = GameObject.Find("Player4PuzzlePieces").transform.position;
                        
                        if (pieceOwnerId + 2 == 2)
                            position2 = player3PiecesPosition + new Vector3(0, i * realHeight, -j * realWidth);
                        else
                            position2 = player4PiecesPosition + new Vector3(0, i * realHeight, -j * realWidth);
                        
                        Debug.Log("pieceOwnerId= " + pieceOwnerId);
                        foreach (var player in players)
                        {
                            if (player.GetPhotonView().Owner.ActorNumber == pieceOwnerId + 1)
                            {
                                this.gameObject.GetPhotonView().RPC("InstantiatePuzzlePiece", player.GetPhotonView().Owner,position, rotation, i, j, pieceOwnerId, rows, cols);
                            }
                            if (player.GetPhotonView().Owner.ActorNumber == pieceOwnerId + 1 + 2)
                            {
                                this.gameObject.GetPhotonView().RPC("InstantiatePuzzlePiece", player.GetPhotonView().Owner,position2, Quaternion.Euler(0,180,0), i, j, pieceOwnerId+2, rows, cols);
                            }
                        }
                    }
                    else
                    {
                        foreach (var player in players)
                        {
                            if(player.GetPhotonView().Owner.ActorNumber == pieceOwnerId + 1) this.gameObject.GetPhotonView().RPC("InstantiatePuzzlePiece", player.GetPhotonView().Owner,position, rotation, i, j, pieceOwnerId, rows, cols);
                        }
                    }
                }

            StartCoroutine(WaitingForTextureLoading(width,height));

            //Creating Puzzle and setting its dimensions for all players
            Vector3 puzzlePosition1 = GameObject.Find("PuzzleTeam1Position").transform.position;
            GameObject puzzle1 = PhotonNetwork.Instantiate("Puzzle", puzzlePosition1/*new Vector3(-10.2f,4.5f,-2.7f)*/, Quaternion.Euler(0,0,-90), 0);
            puzzle1.name = "PuzzleTeam1";
            
            if (pvp)
            {
                Vector3 puzzlePosition2 = GameObject.Find("PuzzleTeam2Position").transform.position;
                GameObject puzzle2 = PhotonNetwork.Instantiate("Puzzle", puzzlePosition2/*new Vector3(-10.2f,4.5f,-2.7f)*/, Quaternion.Euler(0,180,-90), 0);
                puzzle2.name = "PuzzleTeam2";
                this.gameObject.GetPhotonView().RPC("SetPuzzleNames", RpcTarget.Others,puzzle1.GetPhotonView().ViewID, puzzle2.GetPhotonView().ViewID);
            } 
            else this.gameObject.GetPhotonView().RPC("SetPuzzleNames", RpcTarget.Others,puzzle1.GetPhotonView().ViewID, -5);

            SetPuzzle(rows,cols, realWidth, realHeight);
            this.gameObject.GetPhotonView().RPC("SetPuzzle", RpcTarget.Others,rows, cols, realWidth, realHeight);

            Instantiate(Resources.Load<GameObject>("WaitingAnimation"), Vector3.zero, Quaternion.identity);
        }

        [PunRPC]
        private void SetPuzzleNames(int id1, int id2)
        {
            PhotonView.Find(id1).gameObject.name = "PuzzleTeam1";
            if(id2 != -5) PhotonView.Find(id2).gameObject.name = "PuzzleTeam2";
        }

        [PunRPC]
        private void InstantiateHintPuzzle(float height, float width)
        {
            foreach (GameObject hint in GameObject.FindGameObjectsWithTag("FullPuzzle"))
            {
                hintPuzzle.Add(hint);
                hint.GetComponent<FullPuzzle>().Initialize(tex, width,height);
                hint.SetActive(false);
            }
        }

        private IEnumerator WaitingForTextureLoading(int width, int height)
        {
            int numberOfPieces = 0;
            while (numberOfPieces < numberOfObjects)
            {
                numberOfPieces = GameObject.FindGameObjectsWithTag("PuzzlePiece").Length;
                yield return new WaitForSeconds(0.2f);
            }
            
            LoadPieceTexture(width,height);
            this.gameObject.GetPhotonView().RPC("LoadPieceTexture", RpcTarget.Others,width, height);
        }

        [PunRPC]
        private void InstantiatePuzzlePiece(Vector3 position, Quaternion rotation, int i, int j, int pieceOwnerId, int rows, int cols)
        {
            //Debug.Log("Player that is instantiating: " + thisPlayer.GetPhotonView().ownerId + "\n" + "Player that should instantiate: " + players[pieceOwnerId].GetPhotonView().ownerId + " pieceOwnerId: " + pieceOwnerId);
            object[] data = new object[3];
            data[0] = i;
            data[1] = j;
            GameObject puzzlePiece = PhotonNetwork.Instantiate("PuzzlePiece", position, rotation, 0, data);
            puzzlePiece.GetPhotonView().RPC("DisableDraggable", RpcTarget.Others);
            this.gameObject.GetPhotonView().RPC("PieceOutline", RpcTarget.All, pieceOwnerId, puzzlePiece.GetPhotonView().ViewID);
        }
        
        [PunRPC]
        private void InstantiatePuzzlePiecePvp(Vector3 positionTeam1, Vector3 positionTeam2, Quaternion rotation, int i, int j, int pieceOwnerId, int rows, int cols)
        {
            object[] data = new object[3];
            data[0] = i;
            data[1] = j;
            GameObject puzzlePieceTeam1 = PhotonNetwork.Instantiate("PuzzlePiece", positionTeam1, rotation, 0, data);
            GameObject puzzlePieceTeam2 = PhotonNetwork.Instantiate("PuzzlePiece", positionTeam2, Quaternion.Euler(0,180,0), 0, data);
            puzzlePieceTeam1.GetPhotonView().RPC("DisableDraggable", RpcTarget.Others);
            this.gameObject.GetPhotonView().RPC("PieceOutline", RpcTarget.All, pieceOwnerId, puzzlePieceTeam1.GetPhotonView().ViewID);
            puzzlePieceTeam2.GetPhotonView().RPC("DisableDraggable", RpcTarget.Others);
            this.gameObject.GetPhotonView().RPC("PieceOutline", RpcTarget.All, pieceOwnerId, puzzlePieceTeam2.GetPhotonView().ViewID);
        }

        private void PuzzleReady()
        {
            
        }

        private int FindPieceOwner()
        {
            if (randomValues == null || highRangeLimit == 0)
            {
                randomValues = new List<int>();
                for (int i = 0; i < 2; i++)
                {
                    randomValues.Add(i);
                }

                highRangeLimit = 2;
            }

            int selectedValueIndex = Random.Range(0, highRangeLimit);
            int selectedValue = randomValues[selectedValueIndex];
            highRangeLimit--;
            randomValues.RemoveAt(selectedValueIndex);

            Debug.Log("PieceOwnerNumber: " + selectedValue);
            return selectedValue;
        }

        [PunRPC]
        public void PieceOutline(int pieceOwnerId, int viewId)
        {
            if (pieceOwnerId == 0 || pieceOwnerId == 2) PhotonView.Find(viewId).transform.Find("Back").GetComponent<MeshRenderer>().material.color = Color.red;
        }

        [PunRPC]
        public void LoadPieceTexture(int width, int height)
        {
            int rows = Mathf.RoundToInt(Mathf.Sqrt(numberOfObjects));
            int cols = rows;        //TODO initialized for not square puzzles
            realWidth = (float) 4*tex.width / (tex.height * cols);    //real dimensions
            realHeight = (float) 4*tex.height / (tex.height * rows);
            
            
            GameObject[] puzzlePieces = GameObject.FindGameObjectsWithTag("PuzzlePiece");
            foreach (GameObject piece in puzzlePieces)
            {
                piece.transform.localScale = new Vector3(1,realHeight,realWidth);
                PuzzlePiece puzzlePiece = piece.GetComponent<PuzzlePiece>();
                int i = puzzlePiece.correctRow;
                int j = puzzlePiece.correctColumn;
                Texture2D pieceTex = new Texture2D(width,height);
                int x = j * width;
                int y = (rows-1) * height - i * height;
                
                Debug.Log("X: " + x + " Y: " + y);
                Debug.Log("Width: " + width + " Height: " + height);
                
                pieceTex.SetPixels(0,0,width,height,tex.GetPixels(x,y,width,height));
                pieceTex.Apply();
                
                puzzlePiece.gameObject.transform.Find("FrontImage").gameObject.GetComponent<MeshRenderer>().materials[0]
                    .mainTexture = pieceTex;
            }
        }

        [PunRPC]
        private void SetPuzzle(int rows, int cols, float realW, float realH)
        {
            Debug.Log(this.gameObject.GetPhotonView().Owner);
            Puzzle puzzleTeam1 = GameObject.Find("PuzzleTeam1").GetComponent<Puzzle>();
            puzzleTeam1.SetPuzzleMatrix(rows, cols);
            puzzleTeam1.SetPuzzlePieceDimension(realW,realH);
            puzzleTeam1.transform.position += new Vector3(0, puzzleTeam1.center.y - realH/2, -puzzleTeam1.center.z + realW/2);
            if (pvp)
            {
                Puzzle puzzleTeam2 = GameObject.Find("PuzzleTeam2").GetComponent<Puzzle>();
                puzzleTeam2.SetPuzzleMatrix(rows, cols);
                puzzleTeam2.SetPuzzlePieceDimension(realW,realH);
                puzzleTeam2.transform.position += new Vector3(0, puzzleTeam2.center.y - realH/2, puzzleTeam2.center.z - realW/2);
            }
        }

        /*[PunRPC]*/
        /*public void OnObjectPositioned(int playerId)
        {
            Debug.Log(GameObject.Find("PuzzleTeam1").GetComponent<Puzzle>().IsCompleted());
            if (this.gameObject.GetPhotonView().isMine)
            {
                if (playerId == this.gameObject.GetPhotonView().ownerId || GetTeam(playerId) == gameObject.GetPhotonView().ownerId)
                {
                    if(GameObject.Find("PuzzleTeam1").GetComponent<Puzzle>().IsCompleted())
                        this.gameObject.GetPhotonView().RPC("StartVictoryAnimations", PhotonTargets.All);
                }
                else if(GameObject.Find("PuzzleTeam2").GetComponent<Puzzle>().IsCompleted())
                    this.gameObject.GetPhotonView().RPC("StartVictoryAnimations", PhotonTargets.All);
            }
        }*/
        
        /*

        [PunRPC]
        public void OnObjectRemoved(int playerId, int anchorPointIndex)
        {

            if (playerId == this.gameObject.GetPhotonView().ownerId)
                combinationPlayer1[anchorPointIndex] = 0;
            else combinationPlayer2[anchorPointIndex] = 0;

            int numberOfCorrectObjects = 0;

            for (int i = 0; i < numberOfObjects; i++)
                if (combinationPlayer1[i] == combinationPlayer2[i] && combinationPlayer1[i] != 0)
                    numberOfCorrectObjects++;

            this.gameObject.GetPhotonView().RPC("UpdateScore", PhotonTargets.All, numberOfCorrectObjects + "/" + numberOfObjects);

            if (numberOfCorrectObjects == numberOfObjects)
                this.gameObject.GetPhotonView().RPC("StartVictoryAnimations", PhotonTargets.All);
        }

        private int GetTeam(int playerId)
        {
            if (playerId >= 1 && playerId <= 2) return 1;
            if (playerId >= 3 && playerId <= 4) return 2;
            return 0;
        }*/

        /*
    [PunRPC]
    void SetNumberOfObjects(int number)
    {
        numberOfObjects = number;
    }
    */

        protected override void SetUpGame()
        {
            Debug.Log("Setup ongoing...");
            
            Debug.Log("-------------Players[]--------------");
            foreach (var player in players)
            {
                Debug.Log(player.GetPhotonView().Owner.ActorNumber);
            }
            Debug.Log("--------------------------------------");

            //Positioning players
            /*foreach (var player in players)
            {
                if(player==thisPlayer) player.gameObject.transform.position += new Vector3(-2.5f,-1.5f,0);
            }*/

            if (this.gameObject.GetPhotonView().IsMine)
            {
                Texture2D[] images = Resources.LoadAll<Texture2D>("PuzzlingImages/" + PhotonManager.instance.theme);
                int numberOfPossibleImages = images.Length;
                int randomIndex = Random.Range(0, numberOfPossibleImages);
                this.gameObject.GetPhotonView().RPC("SetTexture", RpcTarget.Others, images[randomIndex].name, PhotonManager.instance.theme);
                tex = images[randomIndex];
                
                SpawnObjects();
                /*combinationPlayer1 = new int[numberOfObjects];
                combinationPlayer2 = new int[numberOfObjects];*/

                //this.gameObject.GetPhotonView().RPC("UpdateScore", PhotonTargets.All, "0/" + numberOfObjects);
                //SpawnAnchorPoints();
            }
        }

        [PunRPC]
        public void OnObjectPositioned(int playerId)
        {
            if (gameObject.GetPhotonView().IsMine)
            {
                //Debug.Log("Puzzle.isCompleted() = " + GameObject.Find("PuzzleTeam1").GetPhotonView().gameObject.GetComponent<Puzzle>().IsCompleted());
                if (playerId == 1)
                {
                    correctPiecesPlayer1++;
                    Debug.Log("CORRECT PIECES team 1" + (correctPiecesPlayer1 + correctPiecesPlayer2));
                    if (correctPiecesPlayer1 + correctPiecesPlayer2 == numberOfObjects && correctPiecesPlayer3 + correctPiecesPlayer4 != numberOfObjects)
                    {
                        Debug.Log("Team 1 wins!");
                        foreach (var player in players)
                        {
                            Debug.Log("Team: " + GetTeam(player.GetPhotonView().Owner.ActorNumber) + " player: " + thisPlayer.GetPhotonView().Owner.ActorNumber);
                            if (GetTeam(player.GetPhotonView().Owner.ActorNumber) == 1)
                            {
                                gameObject.GetPhotonView().RPC("PlayVictorySound", player.GetPhotonView().Owner);
                                foreach (var puzzlePiece in GameObject.FindGameObjectsWithTag("PuzzlePiece"))
                                {
                                    puzzlePiece.GetPhotonView().RPC("DisableDraggable", player.GetPhotonView().Owner);
                                }
                            }
                            if(GetTeam(player.GetPhotonView().Owner.ActorNumber) ==2)
                                gameObject.GetPhotonView().RPC("PlayLossSound", player.GetPhotonView().Owner);
                        }
                    }
                }
                else if (playerId == 2)
                {
                    correctPiecesPlayer2++;
                    Debug.Log("CORRECT PIECES team 2" + (correctPiecesPlayer2 + correctPiecesPlayer1));
                    if (correctPiecesPlayer2 + correctPiecesPlayer1 == numberOfObjects && correctPiecesPlayer3 + correctPiecesPlayer4 != numberOfObjects)
                    {
                        Debug.Log("Team 1 wins!");
                        foreach (var player in players)
                        {
                            Debug.Log("Team: " + GetTeam(player.GetPhotonView().Owner.ActorNumber) + " player: " + thisPlayer.GetPhotonView().Owner.ActorNumber);
                            if (GetTeam(player.GetPhotonView().Owner.ActorNumber) == 1)
                            {
                                gameObject.GetPhotonView().RPC("PlayVictorySound", player.GetPhotonView().Owner);
                                foreach (var puzzlePiece in GameObject.FindGameObjectsWithTag("PuzzlePiece"))
                                {
                                    puzzlePiece.GetPhotonView().RPC("DisableDraggable", player.GetPhotonView().Owner);
                                }
                            } 
                            if(GetTeam(player.GetPhotonView().Owner.ActorNumber)==2)
                                gameObject.GetPhotonView().RPC("PlayLossSound", player.GetPhotonView().Owner);
                        }
                    }
                }
                
                else if (playerId == 3)
                {
                    correctPiecesPlayer3++;
                    Debug.Log("CORRECT PIECES team 1" + (correctPiecesPlayer3 + correctPiecesPlayer4));
                    if (correctPiecesPlayer3 + correctPiecesPlayer4 == numberOfObjects &&
                        correctPiecesPlayer2 + correctPiecesPlayer1 != numberOfObjects)
                    {
                        Debug.Log("Team 2 wins!");
                        foreach (var player in players)
                        {
                            Debug.Log("Team: " + GetTeam(player.GetPhotonView().Owner.ActorNumber) + " player: " +
                                      thisPlayer.GetPhotonView().Owner.ActorNumber);
                            if (GetTeam(player.GetPhotonView().Owner.ActorNumber) == 2)
                            {
                                gameObject.GetPhotonView().RPC("PlayVictorySound", player.GetPhotonView().Owner);
                                foreach (var puzzlePiece in GameObject.FindGameObjectsWithTag("PuzzlePiece"))
                                {
                                    puzzlePiece.GetPhotonView().RPC("DisableDraggable", player.GetPhotonView().Owner);
                                }
                            }

                            if (GetTeam(player.GetPhotonView().Owner.ActorNumber) == 1)
                                gameObject.GetPhotonView().RPC("PlayLossSound", player.GetPhotonView().Owner);
                        }
                    }
                }
                else if (playerId == 4)
                {
                    correctPiecesPlayer4++;
                    Debug.Log("CORRECT PIECES team 1" + correctPiecesPlayer4 + correctPiecesPlayer3);
                    if (correctPiecesPlayer3 + correctPiecesPlayer4 == numberOfObjects &&
                        correctPiecesPlayer2 + correctPiecesPlayer1 != numberOfObjects)
                    {
                        Debug.Log("Team 2 wins!");
                        foreach (var player in players)
                        {
                            Debug.Log("Team: " + GetTeam(player.GetPhotonView().Owner.ActorNumber) + " player: " +
                                      thisPlayer.GetPhotonView().Owner.ActorNumber);
                            if (GetTeam(player.GetPhotonView().Owner.ActorNumber) == 2)
                            {
                                gameObject.GetPhotonView().RPC("PlayVictorySound", player.GetPhotonView().Owner);
                                foreach (var puzzlePiece in GameObject.FindGameObjectsWithTag("PuzzlePiece"))
                                {
                                    puzzlePiece.GetPhotonView().RPC("DisableDraggable", player.GetPhotonView().Owner);
                                }
                            }

                            if (GetTeam(player.GetPhotonView().Owner.ActorNumber) == 1)
                                gameObject.GetPhotonView().RPC("PlayLossSound", player.GetPhotonView().Owner);
                        }
                    }
                }

                if (pvp)
                {
                    if (correctPiecesPlayer1 + correctPiecesPlayer2 == numberOfObjects && correctPiecesPlayer3 + correctPiecesPlayer4 == numberOfObjects)
                    {
                        gameObject.GetPhotonView().RPC("StartVictoryAnimations", RpcTarget.All);
                    }
                }
                else
                {
                    if (correctPiecesPlayer1 + correctPiecesPlayer2 == numberOfObjects)
                    {
                        gameObject.GetPhotonView().RPC("StartVictoryAnimations", RpcTarget.All);
                    }
                }
            }
        }

        [PunRPC]
        public void OnObjectRemoved(int playerId)
        {
            if (gameObject.GetPhotonView().IsMine)
            {
                Debug.Log("Remove player: " + playerId);
                if (playerId == 1) correctPiecesPlayer1--;
                else if (playerId == 2) correctPiecesPlayer2--;
                else if (playerId == 3) correctPiecesPlayer3--;
                else if (playerId == 4) correctPiecesPlayer4--;
            }
        }

        [PunRPC]
        public void PlayVictorySound()
        {
            AudioManager.instance.PlayHurraySound();
        }
        
        [PunRPC]
        public void PlayLossSound()
        {
            AudioManager.instance.PlayLossSound();
        }
        

        [PunRPC]
        public void SetTexture(string imageName, string theme)
        {
            Texture2D image = Resources.Load<Texture2D>("PuzzlingImages/" + theme + "/" + imageName);
            this.tex = image;
        }

        [PunRPC]
        public void ShowHint()
        {
            StartCoroutine("ShowPuzzle");
        }

        private IEnumerator ShowPuzzle()
        {
            foreach (GameObject fullPuzzle in hintPuzzle)
            {
                if (!fullPuzzle.activeSelf)
                {
                    fullPuzzle.SetActive(true);
                }
            }
            AudioManager.instance.PlayPopSound();
            yield return new WaitForSeconds(12);
            foreach (GameObject fullPuzzle in hintPuzzle)
            {
                if (fullPuzzle.activeSelf)
                {
                    fullPuzzle.SetActive(false);
                }
            }
        }
        
    }
    
    
}
