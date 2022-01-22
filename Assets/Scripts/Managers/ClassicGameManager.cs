using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClassicGameManager : GameManager
{
    public enum EmojiEnum
    {
        none1,
        none2,
        angry,
        crying,
        embarassed,
        laughing,
        scared,
        smiling
    }

    public EmojiEnum selected1 = EmojiEnum.none1;
    public EmojiEnum selected2 = EmojiEnum.none2;

    Vector3 baseEmojiPosition1 = new Vector3(0, 3, 1.1f);
    Vector3 baseEmojiPosition2 = new Vector3(0, 3, -1.1f);

    public GameObject angry1;
    public GameObject angry2;
    public GameObject crying1;
    public GameObject crying2;

    int init = 0;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (init == 0)
        {
            init++;
            if (PhotonNetwork.IsMasterClient)
            {
                SpawnEmojis();
            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.Log("Object clicked: " + hit.transform.gameObject.name);
                if (thisPlayer.GetInstanceID() == 1001)
                {
                    Debug.Log("player 1001");
                }
                else
                {
                    if(thisPlayer.GetInstanceID() == 2001)
                    {
                        Debug.Log("player 2001");
                    }
                }
            }
        }
        if (selected1 == selected2)
        {
            Destroy(GameObject.Find(selected1.ToString() + "1").gameObject);
            Destroy(GameObject.Find(selected1 + "2").gameObject);
        }

        if (GameObject.FindGameObjectsWithTag("Emoji").Length == 0)
        {
            victory();
        }
    }



    public void SpawnEmojis()
    {
        Vector3 pos1 = new Vector3(1f, 0f, 0f);
        angry1 = PhotonNetwork.Instantiate("Models/Prefab/Angry1", baseEmojiPosition1, Quaternion.identity, 0);
        angry2 = PhotonNetwork.Instantiate("Models/Prefab/Angry2", baseEmojiPosition2, Quaternion.identity, 0);
        crying1 = PhotonNetwork.Instantiate("Models/Prefab/Crying1", baseEmojiPosition1 + pos1, Quaternion.identity, 0);
        crying2 = PhotonNetwork.Instantiate("Models/Prefab/Crying2", baseEmojiPosition2 + pos1, Quaternion.identity, 0);
    }


    public void ClickOnAngry1()
    {
        Debug.Log("ClickOnAngry1");
        OnClick1("Angry1(Clone)", EmojiEnum.angry);
    }

    public void ClickOnAngry2()
    {
        Debug.Log("ClickOnAngry2");
        OnClick2("Angry2(Clone)", EmojiEnum.angry);
    }

    public void ClickOnCrying1()
    {
        Debug.Log("ClickOnCrying1");
        OnClick1("Crying1(Clone)", EmojiEnum.angry);
    }

    public void ClickOnCrying2()
    {
        Debug.Log("ClickOnCrying2");
        OnClick2("Crying2(Clone)", EmojiEnum.angry);
    }

    void OnClick1(string gameObjectName, EmojiEnum emojiName)
    {
        if (selected1.CompareTo(emojiName) == 0)
        {
            selected1 = EmojiEnum.none1;
            GameObject.Find(gameObjectName).transform.localScale = new Vector3(50f, 50f, 50f);

        }
        else
        {
            if (selected1.CompareTo(EmojiEnum.none1) != 0)
            {
                GameObject.Find(selected1.ToString() + "1").transform.localScale = new Vector3(50f, 50f, 50f);
            }
            selected1 = emojiName;
            GameObject.Find(gameObjectName).transform.localScale = new Vector3(75f, 75f, 75f);
        }
    }

    void OnClick2(string gameObjectName, EmojiEnum emojiName)
    {
        if (selected2.CompareTo(emojiName) == 0)
        {
            GameObject.Find(gameObjectName).transform.localScale = new Vector3(50f, 50f, 50f);
            selected2 = EmojiEnum.none2;
        }
        else
        {
            if (selected2.CompareTo(EmojiEnum.none2) != 0)
            {
                GameObject.Find(selected2.ToString() + "2").transform.localScale = new Vector3(50f, 50f, 50f);
            }

            selected2 = emojiName;
            GameObject.Find(gameObjectName).transform.localScale = new Vector3(75f, 75f, 75f);

        }
    }

    void victory()
    {

    }
}
