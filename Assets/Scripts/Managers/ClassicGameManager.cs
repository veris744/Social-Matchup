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

    public GameObject angry1;
    public GameObject angry2;
    public GameObject crying1;
    public GameObject crying2;
    public GameObject embarassed1;
    public GameObject embarassed2;
    public GameObject involve1;
    public GameObject involve2;
    public GameObject laughing1;
    public GameObject laughing2;
    public GameObject scared1;
    public GameObject scared2;
    public GameObject smiling1;
    public GameObject smiling2;
    public GameObject surprised1;
    public GameObject surprised2;

    // Start is called before the first frame update
    void Start()
    {
        emoji1A = false;
        emoji2A = false;
        emoji3A = false;

        emoji1B = false;
        emoji2B = false;
        emoji3B = false;


        SpawnEmojis();
    }

    // Update is called once per frame
    void Update()
    {
        if (emoji1A & emoji1B)
        {
            Destroy(GameObject.Find("angry1").gameObject);
            Destroy(GameObject.Find("angry2").gameObject);
        }

        if (GameObject.FindGameObjectsWithTag("Emoji").Length == 0)
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

    public void SpawnEmojis()
    {
        crying1.SetActive(true);
        crying2.SetActive(true);
        angry1.SetActive(true);
        angry2.SetActive(true);
        laughing1.SetActive(true);
        laughing2.SetActive(true);
        /*
        Instantiate(Resources.Load("Models/angry"), new Vector3(0.15f, -0.239999f, -1.91f), Quaternion.identity);
        GameObject.Find("angry(Clone)").name = "angry1";

        Instantiate(Resources.Load("Models/angry"), new Vector3(-0.2f, 7.9f, 1.5f), Quaternion.identity);
        GameObject.Find("angry(Clone)").name = "angry2";
        /*
        if (PhotonManager.instance.NumberOfImages == 4)
        {

        } else if (PhotonManager.instance.NumberOfImages == 5)
        {

        }*/

    }

     void victory()
    {

    }
}
