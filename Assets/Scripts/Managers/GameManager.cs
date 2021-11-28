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
        this.pvp = false;       //no pvp yet
    }

    private void Update()
    {
        if (!pvp && (players == null || players.Length < 2))
        {
            players = GameObject.FindGameObjectsWithTag("Player");

            if (!pvp && players.Length == 2)
            {
                Debug.Log("Player found");

                foreach (GameObject player in players)
                {
                    Debug.Log("Player: " + player.name);
                    if (player.GetPhotonView().IsMine)
                        thisPlayer = player.gameObject;
                }
                
                SetUpGame();
            }
        }
        
        //No helper
        //if (thisPlayer == null && players != null) thisPlayer = PhotonManager.instance.Helper;
        Debug.Log("IsPlaying = " + AudioManager.instance.gameObject.GetComponent<AudioSource>().isPlaying);
    }


    protected abstract void SetUpGame(); 

    
    public int GetTeam(int playerId)
    {
        if (playerId >= 1 && playerId <= 2) return 1;
        if (playerId >= 3 && playerId <= 4) return 2;
        return 0;
    }

}
