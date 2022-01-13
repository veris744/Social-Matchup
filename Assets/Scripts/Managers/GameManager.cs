using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    protected GameObject[] players;
    protected GameObject thisPlayer;
    protected bool pvp;
    private bool isAssigned;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameManger name: " + gameObject.name);
        //this.pvp = PhotonManager.instance.pvp;
        this.pvp = false; //pvp always false for now only 2 players
        isAssigned = false;
    }

    // Update is called once per frame
    private void Update()
    {   /*
        if (!isAssigned & players.Length == 2)
        {
            foreach (GameObject player in players)
            {
                Debug.Log("PLAYER ID VIEW: " + player.GetPhotonView().GetInstanceID());
                if (player.GetPhotonView().IsMine)
                {
                    Debug.Log("PLAYER ID VIEW MINE: " + player.GetPhotonView().GetInstanceID());
                    CameraController cameraController = player.transform.Find("Camera Offset").Find("Main Camera")
                        .gameObject.GetComponent<CameraController>();
                    cameraController.enabled = true;
                    cameraController.SetTarget(player.transform);
                    player.transform.Find("Camera Offset").Find("Main Camera").gameObject.SetActive(true);
                }
            }
            isAssigned = true;
        }
        */


        if ((pvp && (players == null || players.Length < 4)) || (!pvp && (players == null || players.Length < 2)))
        {
            players = GameObject.FindGameObjectsWithTag("MainCamera");

            if (!pvp && players.Length == 2)
            {
                Debug.Log("Player found");

                foreach (GameObject player in players)
                {
                    Debug.Log("Player: " + player.name);
                    if (player.GetPhotonView().IsMine)
                        thisPlayer = player.gameObject;
                }

            }
            else if (pvp && players.Length == 4)
            {
                Debug.Log("Player found - PVP");

                foreach (GameObject player in players)
                {
                    if (player.GetPhotonView().IsMine)
                    {
                        thisPlayer = player.gameObject;
                    }
                }

            }
        }

        //if (thisPlayer == null && players != null) thisPlayer = PhotonManager.instance.Helper;
        Debug.Log("IsPlaying = " + AudioManager.instance.gameObject.GetComponent<AudioSource>().isPlaying);
    }

    public void SetPVP(bool pvp)
    {
        this.pvp = pvp;
    }


    [PunRPC]
    public void StartVictoryAnimations()
    {
        StartCoroutine(OnVictory());
    }
    /*
    [PunRPC]
    public void PvpHurraySound(int teamIndex)
    {
        if (GetTeam(thisPlayer.gameObject.GetPhotonView().Owner.ActorNumber) == teamIndex)
            AudioManager.instance.PlayHurraySound();
    }*/

    protected IEnumerator OnVictory()
    {
        yield return new WaitForSeconds(1);
        AudioManager.instance.PlayHurraySound();
        yield return new WaitForSeconds(3);
        AudioManager.instance.StopMusic();
        yield return new WaitForSeconds(2);
        AudioManager.instance.PlayVictorySound();

        Debug.Log("thisPlayer = " + thisPlayer.name);
        Debug.Log("BlackPanel = " + thisPlayer.transform.Find("BlackPanel").name);
        Debug.Log("Sprite renderer = " + thisPlayer.transform.Find("BlackPanel").GetComponent<SpriteRenderer>().ToString());

        SpriteRenderer endGamePanel = thisPlayer.transform.Find("BlackPanel").GetComponent<SpriteRenderer>();
        endGamePanel.color = Color.black;

        yield return new WaitForSeconds(6);
        SceneManager.LoadScene("MainMenu");

        PhotonNetwork.LeaveRoom();
    }

    /*
    public int GetTeam(int playerId)
    {
        if (playerId >= 1 && playerId <= 2) return 1;
        if (playerId >= 3 && playerId <= 4) return 2;
        return 0;
    }*/
}
