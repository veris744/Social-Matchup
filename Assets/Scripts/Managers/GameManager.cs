using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameManager : MonoBehaviour
{
    protected GameObject[] players;
    protected GameObject thisPlayer;
    protected bool pvp;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameManger name: " + gameObject.name);
        //this.pvp = PhotonManager.instance.pvp;
        this.pvp = false; //pvp always false for now only 2 players
    }

    // Update is called once per frame
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

                SetUpGame();
            }
        }

        //if (thisPlayer == null && players != null) thisPlayer = PhotonManager.instance.Helper;
        Debug.Log("IsPlaying = " + AudioManager.instance.gameObject.GetComponent<AudioSource>().isPlaying);
    }
    protected abstract void SetUpGame();
}
