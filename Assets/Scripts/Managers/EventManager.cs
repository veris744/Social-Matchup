using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class EventManager : MonoBehaviour
{
    public void OnBackToMainMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
        if (PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom();
    }

    public void OnOpenGamesButtonClicked()
    {
        SceneManager.LoadScene("JoinGameMenu");
    }

    public void OnNewgameButtonClicked()
    {
        SceneManager.LoadScene("NewGameMenu");
    }

    public void OnChangingRoomButtonClicked()
    {
        SceneManager.LoadScene("ChangingRoom");
    }


}
