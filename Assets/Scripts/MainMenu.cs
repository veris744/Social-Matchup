using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using VR;

//General script for MainMenu functionalities
public class MainMenu : MonoBehaviour
{
    private Button newGameButton;
    private Button joinGameButton;
    private VrModeController vrModeController;

    private void Start()
    {
        //creates a PhotonManager
        if (PhotonManager.instance == null)
            Instantiate(Resources.Load<GameObject>("Managers/PhotonManager"));           

        AudioManager.instance.PlayMainMenuMusic();
        vrModeController = GetComponent<VrModeController>();
        
        //Switch VR off
        Screen.orientation = ScreenOrientation.Portrait;
        vrModeController.ExitVR();

        newGameButton = GameObject.Find("NewGameButton").GetComponent<Button>();
        joinGameButton = GameObject.Find("JoinGameButton").GetComponent<Button>();
    }

    //Switch the game back to 2D mode
    /*private IEnumerator SwitchVROff()
    {
        XRSettings.LoadDeviceByName("None");
        yield return null;
        XRSettings.enabled = false;
    }*/
    

    //disable the NewGame / JoinGame buttons wether the connection is not available
    private void Update()
    {
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
