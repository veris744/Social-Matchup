using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicGameManager : MonoBehaviour
{
    enum Emoji
    {
        none,
        angry,
        crying,
        embarassed,
        laughing,
        scared,
        smiling
    }

    Emoji selected1 = Emoji.none;
    Emoji selected2 = Emoji.none;

    public bool emoji1A;
    public bool emoji2A;
    public bool emoji1B;
    public bool emoji2B;
    public bool emoji1C;
    public bool emoji2C;
    public bool emoji1D;
    public bool emoji2D;
    public bool emoji1E;
    public bool emoji2E;
    public bool emoji1F;
    public bool emoji2F;

    public GameObject angry1;
    public GameObject angry2;
    public GameObject crying1;
    public GameObject crying2;
    public GameObject embarassed1;
    public GameObject embarassed2;
    public GameObject laughing1;
    public GameObject laughing2;
    public GameObject scared1;
    public GameObject scared2;
    public GameObject smiling1;
    public GameObject smiling2;

    // Start is called before the first frame update
    void Start()
    {
        emoji1A = false;
        emoji2A = false;
        emoji1B = false;
        emoji2B = false;
        emoji1C = false;
        emoji2C = false;
        emoji1D = false;
        emoji2D = false;
        emoji1E = false;
        emoji2E = false;
        emoji1F = false;
        emoji2F = false;


        SpawnEmojis();
    }

    // Update is called once per frame
    void Update()
    {
        if (emoji1A & emoji2A)
        {
            Destroy(angry1);
            Destroy(GameObject.Find("angry2").gameObject);
        }
        if (emoji1B & emoji2B)
        {
            Destroy(GameObject.Find("crying1").gameObject);
            Destroy(GameObject.Find("crying2").gameObject);
        }
        if (emoji1C & emoji2C)
        {
            Destroy(GameObject.Find("embarassed1").gameObject);
            Destroy(GameObject.Find("embarassed2").gameObject);
        }
        if (emoji1D & emoji2D)
        {
            Destroy(GameObject.Find("laughing1").gameObject);
            Destroy(GameObject.Find("laughing2").gameObject);
        }
        if (emoji1E & emoji2E)
        {
            Destroy(GameObject.Find("scared1").gameObject);
            Destroy(GameObject.Find("scared2").gameObject);
        }
        if (emoji1F & emoji2F)
        {
            Destroy(GameObject.Find("smiling1").gameObject);
            Destroy(GameObject.Find("smiling2").gameObject);
        }

        if (GameObject.FindGameObjectsWithTag("Emoji").Length == 0)
        {
            victory();
        }
    }


    public void ClickOnAngry1()
    {
        OnClick("angry1", emoji1A, out emoji1A);
        selected1 = Emoji.angry;
    }

    public void ClickOnAngry2()
    {
        OnClick("angry2", emoji2A, out emoji2A);
        selected2 = Emoji.angry;
    }

    public void ClickOnCrying1()
    {
        OnClick("crying1", emoji1B, out emoji1B);
        selected1 = Emoji.crying;
    }

    public void ClickOnCrying2()
    {
        OnClick("crying2", emoji2B, out emoji2B);
        selected2 = Emoji.crying;
    }
    public void ClickOnEmbarassed1()
    {
        OnClick("embarassed1", emoji1C, out emoji1C);
    }

    public void ClickOnEmbarassed2()
    {
        OnClick("embarassed2", emoji2C, out emoji2C);
    }
    public void ClickOnLaughing1()
    {
        OnClick("laughing1", emoji1D, out emoji1D);
    }

    public void ClickOnLaughing2()
    {
        OnClick("laughing2", emoji2D, out emoji2D);
    }

    public void ClickOnScared1()
    {
        OnClick("scared1", emoji1E, out emoji1E);
    }

    public void ClickOnScared2()
    {
        OnClick("scared2", emoji2E, out emoji2E);
    }
    public void ClickOnSmiling1()
    {
        OnClick("smiling2", emoji1F, out emoji1F);
    }

    public void ClickOnSmiling2()
    {
        OnClick("smiling2", emoji2F, out emoji2F);
    }

    void OnClick(string gameObjectName,in bool isEmojiActive,  out bool emojiActive)
    {
        if (!isEmojiActive)
        {
            emojiActive = true;
            GameObject.Find(gameObjectName).transform.localScale = new Vector3(75f, 75f, 75f);
        }
        else
        {
            emojiActive = false;
            GameObject.Find(gameObjectName).transform.localScale = new Vector3(50f, 50f, 50f);
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
