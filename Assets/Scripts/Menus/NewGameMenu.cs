using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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


        numberOfImages = GameObject.Find("NumberOfImagesButton").transform.Find("Label").GetComponent<TextMeshProUGUI>().text;

        audioChat = GameObject.Find("AudioChatToggle").GetComponent<Toggle>().isOn;


        //Default values for 1st version, notPVP, notHelper
        task = "Classic";
        location = "Castle";
        numberOfPlayers = 2;

        PhotonManager.instance.Location = location;
        PhotonManager.instance.Task = task;
        PhotonManager.instance.NumberOfImages = System.Convert.ToInt32(numberOfImages);
        PhotonManager.instance.AudioChat = audioChat;
        PhotonManager.instance.NumberOfPlayers = numberOfPlayers;
        PhotonManager.instance.pvp = false;

        PhotonManager.instance.CreateRoom(playerName + " (" + task + " - " + numberOfImages + ")");

    }

    public void startWaitingAnimation()
    {
        Instantiate(Resources.Load<GameObject>("WaitingAnimation"), GameObject.Find("Canvas").transform);
    }

}
