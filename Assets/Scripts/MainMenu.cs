using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private Button newGameButton;
    private Button joinGameButton;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonManager.instance == null)
            Instantiate(Resources.Load<GameObject>("Managers/PhotonManager"));

        AudioManager.instance.PlayMainMenuMusic();

        newGameButton = GameObject.Find("NewGameButton").GetComponent<Button>();
        joinGameButton = GameObject.Find("JoinGameButton").GetComponent<Button>();
    }

    
    void Update()
    {
        //disable the NewGame / JoinGame buttons wether the connection is not available
        if (PhotonNetwork.IsConnected)
        {
            newGameButton.interactable = true;
            joinGameButton.interactable = true;
        }

        else
        {
            newGameButton.interactable = false;
            joinGameButton.interactable = false;
        }
    }
}
