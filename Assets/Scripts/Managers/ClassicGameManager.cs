using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicGameManager : GameManager
{
    public enum Emoji
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

    public Emoji selected1 = Emoji.none1;
    public Emoji selected2 = Emoji.none2;


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

    int init = 0;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
        if(init==0)
        {
            init++;
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("master");
                SpawnEmojis();
            }
        }
        

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.Log(hit.transform.gameObject.name);
            }
        }
        if (selected1 == selected2)
        {
            Destroy(GameObject.Find(selected1.ToString() + "1").gameObject);
            Destroy(GameObject.Find(selected1+"2").gameObject);
        }

        if (GameObject.FindGameObjectsWithTag("Emoji").Length == 0)
        {
            victory();
        }
    }


    public void ClickOnAngry1()
    {
        OnClick1("angry1", Emoji.angry);
    }

    public void ClickOnAngry2()
    {
        OnClick2("angry2", Emoji.angry);
    }

    public void ClickOnCrying1()
    {
        OnClick1("crying1", Emoji.crying);
    }

    public void ClickOnCrying2()
    {
        
        OnClick2("crying2", Emoji.crying);
    }
    public void ClickOnEmbarassed1()
    {
        OnClick1("embarassed1", Emoji.embarassed);
    }

    public void ClickOnEmbarassed2()
    {
        OnClick2("embarassed2", Emoji.embarassed);
    }
    public void ClickOnLaughing1()
    {
        OnClick1("laughing1", Emoji.laughing);
    }

    public void ClickOnLaughing2()
    {
        OnClick2("laughing2", Emoji.laughing);
    }

    public void ClickOnScared1()
    {
        OnClick1("scared1", Emoji.scared);
    }

    public void ClickOnScared2()
    {
        OnClick2("scared2", Emoji.scared);
    }
    public void ClickOnSmiling1()
    {
        OnClick1("smiling2", Emoji.smiling);
    }

    public void ClickOnSmiling2()
    {
        OnClick2("smiling2", Emoji.smiling);
    }

    void OnClick1(string gameObjectName, Emoji emojiName)
    {
        if (selected1.CompareTo(emojiName) == 0)
        {
            selected1 = Emoji.none1;
            GameObject.Find(gameObjectName).transform.localScale = new Vector3(50f, 50f, 50f);

        }
        else
        {
            if (selected1.CompareTo(Emoji.none1) != 0)
            {
                GameObject.Find(selected1.ToString() + "1").transform.localScale = new Vector3(50f, 50f, 50f);
            }
            selected1 = emojiName;
            GameObject.Find(gameObjectName).transform.localScale = new Vector3(75f, 75f, 75f);
        }
    }

    void OnClick2(string gameObjectName, Emoji emojiName)
    {
        if (selected2.CompareTo(emojiName)==0)
        {
            GameObject.Find(gameObjectName).transform.localScale = new Vector3(50f, 50f, 50f);
            selected2 = Emoji.none2;
        }
        else
        {
            if(selected2.CompareTo(Emoji.none2) != 0)
            {
                GameObject.Find(selected2.ToString() + "2").transform.localScale = new Vector3(50f, 50f, 50f);
            }
            
            selected2 = emojiName;
            GameObject.Find(gameObjectName).transform.localScale = new Vector3(75f, 75f, 75f);

        }
    }

    public void SpawnEmojis()
    {
        angry1 = PhotonNetwork.Instantiate("Models/Prefab/Angry", new Vector3(2.7712f, 3.9141f, 1.5f), Quaternion.Euler(new Vector3(270, 0, 0)), 0);
        angry2 = PhotonNetwork.Instantiate("Models/Prefab/Angry", new Vector3(-34.2f, 2.794099f, -1.5f), Quaternion.Euler(new Vector3(270, 0, 180)), 0);
        crying1 = PhotonNetwork.Instantiate("Models/Prefab/Crying", new Vector3(2.7712f, 3.9141f, 1.5f), Quaternion.Euler(new Vector3(270, 0, 0)), 0);
        crying2 = PhotonNetwork.Instantiate("Models/Prefab/Crying", new Vector3(-34.2f, 2.794099f, -1.5f), Quaternion.Euler(new Vector3(270, 0, 180)), 0);
        embarassed1 = PhotonNetwork.Instantiate("Models/Prefab/Embarassed", new Vector3(2.7712f, 3.9141f, 1.5f), Quaternion.Euler(new Vector3(270, 0, 0)), 0);
        embarassed2 = PhotonNetwork.Instantiate("Models/Prefab/Embarassed", new Vector3(-34.2f, 2.794099f, -1.5f), Quaternion.Euler(new Vector3(270, 0, 180)), 0);
        laughing1 = PhotonNetwork.Instantiate("Models/Prefab/Laughing", new Vector3(2.7712f, 3.9141f, 1.5f), Quaternion.Euler(new Vector3(270, 0, 0)), 0);
        laughing2 = PhotonNetwork.Instantiate("Models/Prefab/Laughing", new Vector3(-34.2f, 2.794099f, -1.5f), Quaternion.Euler(new Vector3(270, 0, 180)), 0);
        scared1 = PhotonNetwork.Instantiate("Models/Prefab/Scared", new Vector3(2.7712f, 3.9141f, 1.5f), Quaternion.Euler(new Vector3(270, 0, 0)), 0);
        scared2 = PhotonNetwork.Instantiate("Models/Prefab/Scared", new Vector3(-34.2f, 2.794099f, -1.5f), Quaternion.Euler(new Vector3(270, 0, 180)), 0);
        smiling1 = PhotonNetwork.Instantiate("Models/Prefab/Smiling", new Vector3(2.7712f, 3.9141f, 1.5f), Quaternion.Euler(new Vector3(270, 0, 0)), 0);
        smiling2 = PhotonNetwork.Instantiate("Models/Prefab/Smiling", new Vector3(-34.2f, 2.794099f, -1.5f), Quaternion.Euler(new Vector3(270, 0, 180)), 0);

    }

    void victory()
    {

    }
}
