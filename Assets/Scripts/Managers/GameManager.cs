using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

//Parent class of all different GameManagers. Provides the basic methods to setup the game correctly when both players are
//ready and correctly instantiated over the network, and to close the game correctly when the match is over.
public abstract class GameManager : MonoBehaviour
{
    protected GameObject[] players;
    protected GameObject thisPlayer;
    protected bool pvp;

    public virtual void Start()
    {
        Debug.Log("GameManger name: " + gameObject.name);
        this.pvp = PhotonManager.instance.pvp;
    }

    private void Update()
    {
        if ((pvp && (players == null || players.Length < 4)) || (!pvp && (players == null || players.Length < 2)))
        {
            players = GameObject.FindGameObjectsWithTag("MainCamera");

            if (!pvp && players.Length == 2)
            {
                Debug.Log("Giocatori trovati");

                foreach (GameObject player in players)
                {
                    Debug.Log("Player: " + player.name);
                    if (player.GetPhotonView().IsMine)
                        thisPlayer = player.gameObject;
                }
                
                //Screen.orientation = ScreenOrientation.Landscape;
                SetUpGame();
            }
            else if (pvp && players.Length == 4)
            {
                Debug.Log("Giocatori trovati - PVP");

                foreach (GameObject player in players)
                {
                    if (player.GetPhotonView().IsMine)
                    {
                        thisPlayer = player.gameObject;
                    }
                }
                
                //Screen.orientation = ScreenOrientation.Landscape;
                SetUpGame();
            }
        }
        
        if (thisPlayer == null && players != null) thisPlayer = PhotonManager.instance.Helper;
        Debug.Log("IsPlaying = " + AudioManager.instance.gameObject.GetComponent<AudioSource>().isPlaying);
    }

    public void SetPVP(bool pvp)
    {
        this.pvp = pvp;
    }

    protected abstract void SetUpGame(); 

    [PunRPC]
    public void StartVictoryAnimations()
    {
        StartCoroutine(OnVictory());
    }
    
    [PunRPC]
    public void PvpHurraySound(int teamIndex)
    {
        if(GetTeam(thisPlayer.gameObject.GetPhotonView().Owner.ActorNumber) == teamIndex) 
            AudioManager.instance.PlayHurraySound();
    }

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
    
    public int GetTeam(int playerId)
    {
        if (playerId >= 1 && playerId <= 2) return 1;
        if (playerId >= 3 && playerId <= 4) return 2;
        return 0;
    }

}
