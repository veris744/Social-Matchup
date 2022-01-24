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
        Debug.Log("GameManger name: " + gameObject.name);
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
                    Debug.Log("Player: " + player.GetPhotonView().IsMine);
                    player.transform.Find("Camera Offset").Find("Main Camera").GetComponent<Camera>().enabled = false;
                    player.transform.Find("RightHand Controller").gameObject.SetActive(false);
                    player.transform.Find("LeftHand Controller").gameObject.SetActive(false);

                    if (player.GetPhotonView().IsMine)
                    {
                        Debug.Log("Player Mine: " + player.GetPhotonView().ViewID);
                        thisPlayer = player.gameObject;

                        player.transform.Find("Camera Offset").Find("Main Camera").GetComponent<Camera>().enabled = true;
                        player.transform.Find("RightHand Controller").gameObject.SetActive(true);
                        player.transform.Find("LeftHand Controller").gameObject.SetActive(true);
                        CameraController cameraController = player.transform.Find("Camera Offset").Find("Main Camera").gameObject.GetComponent<CameraController>();
                        cameraController.enabled = true;
                        cameraController.SetTarget(player.transform);
                        player.transform.Find("BaseAvatar").gameObject.SetActive(false);
                    }
                }

            }
        }

        if (GameObject.FindGameObjectsWithTag("Helper").Length == 1)
        {
            helper = GameObject.FindGameObjectsWithTag("Helper")[0];
            helper.transform.Find("Camera Offset").Find("Main Camera").GetComponent<Camera>().enabled = false;
            helper.transform.Find("RightHand Controller").gameObject.SetActive(false);
            helper.transform.Find("LeftHand Controller").gameObject.SetActive(false);
            if (helper.GetPhotonView().IsMine)
            {
                thisPlayer = PhotonManager.instance.Helper;
                Debug.Log("Player Mine: " + helper.GetInstanceID());
                thisPlayer = helper.gameObject;

                helper.transform.Find("Camera Offset").Find("Main Camera").GetComponent<Camera>().enabled = true;
                helper.transform.Find("RightHand Controller").gameObject.SetActive(true);
                helper.transform.Find("LeftHand Controller").gameObject.SetActive(true);
                CameraController cameraController = helper.transform.Find("Camera Offset").Find("Main Camera").gameObject.GetComponent<CameraController>();
                cameraController.enabled = true;
                cameraController.SetTarget(helper.transform);
                helper.transform.Find("Avatar").gameObject.SetActive(false);
            }
        }



        Debug.Log("IsPlaying = " + AudioManager.instance.gameObject.GetComponent<AudioSource>().isPlaying);
    }

    public void SetPVP(bool pvp)
    {
        this.pvp = pvp;
    }


}