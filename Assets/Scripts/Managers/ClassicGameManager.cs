using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClassicGameManager : GameManager
{

    private Dictionary<int, GameObject[]> images;     //Dictionary that pairs each playerId with the array of images of that player
    private int numberOfDestroyedImages;              //  e.g:        <ID Player1> | [img0, img1, img2, img3]
                                                      //              <ID Player2> | [img0, img1, img2, img3]
                                                      // used to check if the players are looking at the same image
    private int numberOfDestroyedImagesTeam1;
    private int numberOfDestroyedImagesTeam2;
    private Coroutine destroyTeam1;
    private Coroutine destroyTeam2;
    private bool victoryReached;


    public override void Start()
    {
        base.Start();
        numberOfDestroyedImages = 0;
        numberOfDestroyedImagesTeam1 = 0;
        numberOfDestroyedImagesTeam2 = 0;
        victoryReached = false;
        AudioManager.instance.PlayBackgroundMusic();
    }

    //generate a number of images equal to variable NumberOfImages for both player in randomic position around them.
    //The images are chosen randomly from the set of multiple sprites decided before launching the task.
    private void SpawnRandomImages()
    {
        images = new Dictionary<int, GameObject[]>();

        int numberOfPossibleImages = Resources.LoadAll<GameObject>(PhotonManager.instance.ImageType).Length;
        //Instantiate(Resources.Load<GameObject>("Assets / Resources / Models / emojis"), GameObject.Find("Canvas").transform);
        Debug.Log("numberOfPossibleImages"+ numberOfPossibleImages);
        //generate a list with random indexes of the sprites
        List<int> chosenImages = new List<int>();

        while (chosenImages.Count < PhotonManager.instance.NumberOfImages)
        {
            //generate random index
            int randomIndex = Random.Range(0, numberOfPossibleImages);
            //check it's not already taken, otherwise add it
            if (!chosenImages.Contains(randomIndex))
                chosenImages.Add(randomIndex);
        }

        foreach (GameObject player in players)
        {
            images.Add(player.gameObject.GetPhotonView().OwnerActorNr, new GameObject[PhotonManager.instance.NumberOfImages]);

            for (int i = 0; i < PhotonManager.instance.NumberOfImages; i++)
            {
                Vector3 imagePosition = player.transform.position + Random.onUnitSphere * 2;
                Quaternion imageRotation = Quaternion.LookRotation(player.transform.position - imagePosition);
                GameObject image = PhotonNetwork.Instantiate("Image", imagePosition, imageRotation, 0);
                image.GetPhotonView().RPC("SetSprite", RpcTarget.All, PhotonManager.instance.ImageType, chosenImages[i]);
                image.GetPhotonView().RPC("SetIndex", RpcTarget.All, i);

                if (!pvp)
                {
                    if (player.GetPhotonView().IsMine)
                        image.GetPhotonView().RPC("ChangeCircleColor", RpcTarget.All, 0f, 0f, 255f);
                    else
                    {
                        image.GetPhotonView().RPC("ChangeCircleColor", RpcTarget.All, 255f, 0f, 0f);
                        Debug.Log("Image: " + image.name + " owned by " + player.GetPhotonView().Owner.ActorNumber);
                        image.GetPhotonView().TransferOwnership(player.GetPhotonView().Owner.ActorNumber);
                    }
                }
                else
                {
                    if (player.GetPhotonView().IsMine)
                        image.GetPhotonView().RPC("ChangeCircleColor", RpcTarget.All, 0f, 0f, 255f);
                    else if (player.gameObject.GetPhotonView().OwnerActorNr == 2)
                    {
                        image.GetPhotonView().RPC("ChangeCircleColor", RpcTarget.All, 0f, 0f, 255f);
                        image.GetPhotonView().TransferOwnership(player.GetPhotonView().Owner.ActorNumber);
                    }
                    else
                    {
                        image.GetPhotonView().RPC("ChangeCircleColor", RpcTarget.All, 255f, 0f, 0f);
                        image.GetPhotonView().TransferOwnership(player.GetPhotonView().Owner.ActorNumber);
                    }

                }


                images[player.GetPhotonView().OwnerActorNr][i] = image;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void SetUpGame()
    {
        if (this.gameObject.GetPhotonView().IsMine) //only the main GameManager must Spawn the Images
        {
            SpawnRandomImages();
        }
    }
}
