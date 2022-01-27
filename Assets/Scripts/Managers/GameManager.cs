using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    protected GameObject[] players;
    protected GameObject thisPlayer;
    protected GameObject helper;
    protected bool pvp;


    // Start is called before the first frame update
    void Start()
    {
        this.pvp = PhotonManager.instance.pvp;
    }

    // Update is called once per frame
    private void Update()
    {

        if ((pvp && (players == null || players.Length < 4)) || (!pvp && (players == null || players.Length < 2)))
        {
            players = GameObject.FindGameObjectsWithTag("Player");

            if (!pvp && players.Length == 2)
            {
                Debug.Log("Player found");

                foreach (GameObject player in players)
                {

                    if (player.GetPhotonView().IsMine)
                    {
                        thisPlayer = player.gameObject;

                        CameraController cameraController = player.transform.Find("Camera Offset").Find("Main Camera").gameObject.GetComponent<CameraController>();
                        cameraController.enabled = true;
                        cameraController.SetTarget(player.transform);
                        player.transform.Find("BaseAvatar").gameObject.SetActive(false);

                    }
                    else
                    {
                        player.transform.Find("Camera Offset").Find("Main Camera").GetComponent<Camera>().enabled = false;
                        player.transform.Find("Camera Offset").Find("RightHand Controller").gameObject.SetActive(false);
                        player.transform.Find("Camera Offset").Find("LeftHand Controller").gameObject.SetActive(false);
                    }

                }

            }


            if (GameObject.FindGameObjectsWithTag("Helper").Length == 1)
            {
                helper = GameObject.FindGameObjectsWithTag("Helper")[0];
                if (helper.GetPhotonView().IsMine)
                {
                    thisPlayer = PhotonManager.instance.Helper;
                    thisPlayer = helper.gameObject;

                    CameraController cameraController = helper.transform.Find("Camera Offset").Find("Main Camera").gameObject.GetComponent<CameraController>();
                    cameraController.enabled = true;
                    cameraController.SetTarget(helper.transform);
                    helper.transform.Find("Avatar").gameObject.SetActive(false);
                }
                else
                {
                    helper.transform.Find("Camera Offset").Find("Main Camera").GetComponent<Camera>().enabled = false;
                    helper.transform.Find("Camera Offset").Find("RightHand Controller").gameObject.SetActive(false);
                    helper.transform.Find("Camera Offset").Find("LeftHand Controller").gameObject.SetActive(false);
                }

            }


            if (thisPlayer != null)
            {
                GameObject.Find("DefaultCamera").SetActive(false);

            }

        }


    }


    public void SetPVP(bool pvp)
    {
        this.pvp = pvp;
    }


}