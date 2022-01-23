using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewGameMenu : MonoBehaviour
{
    private string task, numberOfImages;
    private byte numberOfPlayers;
    private bool audioChat, hasHelper;


    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayMainMenuMusic();
    }

    public void StartGame()
    {
        numberOfImages = GameObject.Find("NumberOfImagesButton").transform.Find("Label").GetComponent<TextMeshProUGUI>().text;

        audioChat = GameObject.Find("AudioChatToggle").GetComponent<Toggle>().isOn;

        hasHelper = GameObject.Find("HelperToggle").GetComponent<Toggle>().isOn;


        task = "Classic";

        if (hasHelper)
            numberOfPlayers = 3;
        else
            numberOfPlayers = 2;

        PhotonManager.instance.Location = "Room";
        PhotonManager.instance.Task = task;
        PhotonManager.instance.NumberOfImages = System.Convert.ToInt32(numberOfImages);
        PhotonManager.instance.AudioChat = audioChat;
        PhotonManager.instance.NumberOfPlayers = numberOfPlayers;
        PhotonManager.instance.pvp = false;

        System.Random rnd = new System.Random();
        int idRoom = rnd.Next(0, 10000);
        PhotonManager.instance.CreateRoom(task + " - " + idRoom);

    }

}