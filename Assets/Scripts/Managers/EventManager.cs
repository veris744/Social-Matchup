using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    

    public void LoadNewGameButtonClicked()
    {
        SceneManager.LoadScene("NewGameMenu");
    }

    public void LoadJoinGameButtonClicked()
    {
        SceneManager.LoadScene("JoinGameMenu");
    }
}
