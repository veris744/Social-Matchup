using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    

    public void LoadNewGameScene()
    {
        SceneManager.LoadScene("NewGameMenu");
    }

    public void LoadJoinGameScene()
    {
        SceneManager.LoadScene("JoinGameMenu");
    }

    public void BackToMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
        if (PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom();
    }
}
