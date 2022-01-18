using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicGameManager : MonoBehaviour
{
    public bool emoji1A;
    public bool emoji2A;
    public bool emoji3A;
    public bool emoji1B;
    public bool emoji2B;
    public bool emoji3B;

    // Start is called before the first frame update
    void Start()
    {
        emoji1A = false;
        emoji2A = false;
        emoji3A = false;

        emoji1B = false;
        emoji2B = false;
        emoji3B = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (emoji1A & emoji1B)
        {
            Destroy(GameObject.Find("angry1").gameObject);
            Destroy(GameObject.Find("angry2").gameObject);
        }

        if (GameObject.FindGameObjectsWithTag("Emoji"))
        {
            victory();
        }
    }


    public void clickOnAngry1()
    {
        if (!emoji1A)
        {
            emoji1A = true;
            GameObject.Find("angry1").transform.localScale = new Vector3(150, 150, 150f);
        }
        else
        {
            emoji1A = false;
            GameObject.Find("angry1").transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }


    public void clickOnAngry2()
    {
        if (!emoji1B)
        {
            emoji1B = true;
            GameObject.Find("angry2").transform.localScale = new Vector3(150, 150, 150f);
        }
        else
        {
            emoji1B = false;
            GameObject.Find("angry2").transform.localScale = new Vector3(100f, 100f, 100f);
        }
    }

    public void spawnEmojis()
    {
        Instantiate(Resources.Load("Models/angry"), new Vector3(0, 0, 0), Quaternion.identity);
        GameObject.Find("angry").name = "angry1";

        Instantiate(Resources.Load("Models/angry"), new Vector3(0, 0, 0), Quaternion.identity);
        GameObject.Find("angry").name = "angry2";

        if (PhotonManager.instance.NumberOfImages == 4)
        {

        } else if (PhotonManager.instance.NumberOfImages == 5)
        {

        }
    }

     void victory()
    {

    }
}
