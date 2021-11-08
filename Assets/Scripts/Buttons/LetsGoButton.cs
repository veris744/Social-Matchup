using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetsGoButton : MonoBehaviour
{
    private string playerName, task, location, imagesType, numberOfImages, theme;
    private byte numberOfPlayers;
    private bool audioChat;

    public void StartGame()
    {
        playerName = GameObject.Find("NameInputField").GetComponent<TMP_InputField>().text;
        if (playerName == "")
            playerName = "PLAYER";

        task = GameObject.Find("TaskButton").transform.Find("Label").GetComponent<TextMeshProUGUI>().text;
        location = GameObject.Find("LocationButton").transform.Find("Label").GetComponent<TextMeshProUGUI>().text;

        if (task == "Classic" || task == "Sorting")
            numberOfImages = GameObject.Find("NumberOfImagesButton").transform.Find("Label").GetComponent<TextMeshProUGUI>().text;

        if (task == "Classic")
            imagesType = GameObject.Find("ImagesButton").transform.Find("Label").GetComponent<TextMeshProUGUI>().text;

        if (task == "Puzzling")
        {
            theme = GameObject.Find("ThemeButton").transform.Find("Label").GetComponent<TextMeshProUGUI>().text;
            switch (GameObject.Find("NumberOfPiecesButton").transform.Find("Label").GetComponent<TextMeshProUGUI>().text)
            {
                case "2x2": numberOfImages = "4"; break;
                case "3x3": numberOfImages = "9"; break;
                case "4x4": numberOfImages = "16"; break;
                case "5x5": numberOfImages = "25"; break;
                default: numberOfImages = "4"; break;
            }
        }
            

        bool isPVP = GameObject.Find("PVPToggle").GetComponent<Toggle>().isOn;
        bool hasHelper = GameObject.Find("HelperToggle").GetComponent<Toggle>().isOn;
        
        if (hasHelper && !isPVP)
            numberOfPlayers = 3;
        else if(hasHelper && isPVP) numberOfPlayers = 5;
        
        else if (!hasHelper && !isPVP) numberOfPlayers = 2;
        
        else if (!hasHelper && isPVP) numberOfPlayers = 4;

        audioChat = GameObject.Find("AudioChatToggle").GetComponent<Toggle>().isOn;

        PhotonManager.instance.NumberOfPlayers = numberOfPlayers;

        if (task == "Classic" || task == "Sorting" || task == "Puzzling")
            PhotonManager.instance.NumberOfImages = System.Convert.ToInt32(numberOfImages);
        if (task == "Classic")
            PhotonManager.instance.ImageType = imagesType;
     
        PhotonManager.instance.Location = location;
        PhotonManager.instance.Task = task;
        PhotonManager.instance.AudioChat = audioChat;
        PhotonManager.instance.pvp = isPVP;
        PhotonManager.instance.theme = theme;
        
        //Changing orientation before loading scene
        Screen.orientation = ScreenOrientation.Landscape;
        
        PhotonManager.instance.CreateRoom(playerName + " (" + task + " - " + location + " - " + imagesType + ")");
        
    }
}
