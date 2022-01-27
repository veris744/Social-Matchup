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

    bool defaultCamera;
    bool rotated;


    // Start is called before the first frame update
    void Start()
    {
        this.pvp = PhotonManager.instance.pvp;
        defaultCamera = true;
        rotated = false;
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

        }

        if (GameObject.FindGameObjectsWithTag("Helper").Length == 1)
        {
            helper = GameObject.FindGameObjectsWithTag("Helper")[0];
            helper.transform.Find("Camera Offset").Find("Main Camera").GetComponent<Camera>().enabled = false;
            helper.transform.Find("Camera Offset").Find("RightHand Controller").gameObject.SetActive(false);
            helper.transform.Find("Camera Offset").Find("LeftHand Controller").gameObject.SetActive(false);
            if (helper.GetPhotonView().IsMine)
            {
                thisPlayer = PhotonManager.instance.Helper;
                Debug.Log("Player Mine: " + helper.GetInstanceID());
                thisPlayer = helper.gameObject;

                helper.transform.Find("Camera Offset").Find("Main Camera").GetComponent<Camera>().enabled = true;
                helper.transform.Find("Camera Offset").Find("RightHand Controller").gameObject.SetActive(true);
                helper.transform.Find("Camera Offset").Find("LeftHand Controller").gameObject.SetActive(true);
                CameraController cameraController = helper.transform.Find("Camera Offset").Find("Main Camera").gameObject.GetComponent<CameraController>();
                cameraController.enabled = true;
                cameraController.SetTarget(helper.transform);
                helper.transform.Find("Avatar").gameObject.SetActive(false);
            }
            if (!rotated)
            {
                rotated = true;
                helper.transform.Rotate(new Vector3(0, -90, 0));
            }
        }

    
        if (thisPlayer != null && defaultCamera)
        {
            GameObject.Find("DefaultCamera").SetActive(false);
            defaultCamera = false;
        }


    }


    public void SetPVP(bool pvp)
    {
        this.pvp = pvp;
    }


}