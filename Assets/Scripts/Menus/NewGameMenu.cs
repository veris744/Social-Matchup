using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewGameMenu : MonoBehaviour
{
    private string playerName, task, location, numberOfImages;
    private byte numberOfPlayers;
    private bool audioChat;

    
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayMainMenuMusic();
    }

    public void StartGame()
    {

        playerName = GameObject.Find("NameInputField").GetComponent<TMP_InputField>().text;
        if (playerName == "")
            playerName = "PLAYER";



        //Default values for 1st version, notPVP, notHelper
        task = "Classic";
        location = "Castle";
        numberOfImages = "4";
        numberOfPlayers = 2;

        PhotonManager.instance.CreateRoom(playerName + " (" + task + " - " + location + ")");

    }
}
