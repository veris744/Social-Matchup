using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class ClassicGameManager : GameManager
{

    Vector3 baseEmojiPosition1 = new Vector3(0, 3, 1.1f);
    Vector3 baseEmojiPosition2 = new Vector3(0, 3, -1.1f);
    Vector3[] positionsArray;
    PhotonView photonView;
    bool objectCreated;
    bool finished1;
    bool finished2;

    int init = 0;

    bool angry1;
    bool embarassed1;
    bool crying1;
    bool surprised1;
    bool involve1;
    bool smiling1;
    bool laughing1;


    bool angry2;
    bool embarassed2;
    bool crying2;
    bool surprised2;
    bool involve2;
    bool smiling2;
    bool laughing2;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        positionsArray = new[] { new Vector3(0f, 0f, 0f), new Vector3(1.2f, 0f, 0f), new Vector3(-1.2f, 0f, 0f),
            new Vector3(0.6f, -1f, 0f), new Vector3(-0.6f, -1f, 0f), new Vector3(1.8f, -1f, 0f), new Vector3(-1.8f, -1f, 0f) };

        objectCreated = false;
        finished1 = false;
        finished2 = false;

        angry1 = false;
        embarassed1 = false;
        crying1 = false;
        surprised1 = false;
        involve1 = false;
        smiling1 = false;
        laughing1 = false;


        angry2 = false;
        embarassed2 = false;
        crying2 = false;
        surprised2 = false;
        involve2 = false;
        smiling2 = false;
        laughing2 = false;
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

        if (angry1 && angry2)
        {
            Destroy(GameObject.Find("Angry1(Clone)"));
            Destroy(GameObject.Find("Angry2(Clone)"));
            Destroy(GameObject.Find("angryButton1"));
            Destroy(GameObject.Find("angryButton2"));
        }
        if (embarassed1 && embarassed2)
        {
            Destroy(GameObject.Find("Embarassed1(Clone)"));
            Destroy(GameObject.Find("Embarassed2(Clone)"));
            Destroy(GameObject.Find("embarassedButton1"));
            Destroy(GameObject.Find("embarassedButton2"));
        }
        if (crying1 && crying2)
        {
            Destroy(GameObject.Find("Crying1(Clone)"));
            Destroy(GameObject.Find("Crying2(Clone)"));
            Destroy(GameObject.Find("cryingButton1"));
            Destroy(GameObject.Find("cryingButton2"));
        }
        if (surprised1 && surprised2)
        {
            Destroy(GameObject.Find("Surprised1(Clone)"));
            Destroy(GameObject.Find("Surprised2(Clone)"));
            Destroy(GameObject.Find("surprisedButton1"));
            Destroy(GameObject.Find("surprisedButton2"));
        }
        if (involve1 && involve2)
        {
            Destroy(GameObject.Find("Involve1(Clone)"));
            Destroy(GameObject.Find("Involve2(Clone)"));
            Destroy(GameObject.Find("involveButton1"));
            Destroy(GameObject.Find("involveButton2"));
        }
        if (smiling1 && smiling2)
        {
            Destroy(GameObject.Find("Smiling1(Clone)"));
            Destroy(GameObject.Find("Smiling2(Clone)"));
            Destroy(GameObject.Find("smilingButton1"));
            Destroy(GameObject.Find("smilingButton2"));
        }
        if (laughing1 && laughing2)
        {
            Destroy(GameObject.Find("Laughing1(Clone)"));
            Destroy(GameObject.Find("Laughing2(Clone)"));
            Destroy(GameObject.Find("laughingButton1"));
            Destroy(GameObject.Find("laughingButton2"));
        }


        if (GameObject.FindGameObjectsWithTag("Emoji").Length == 0 && objectCreated && (!finished1 || !finished2))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                finished1 = true;
            } else
            {
                finished2 = true;
            }
            StartCoroutine(OnVictory());
        }
    }

    public static Vector3[] Shuffle(Vector3[] array)
    {
        System.Random rnd = new System.Random();
        for (int i = 0; i < array.Length; i++)
        {
            int k = rnd.Next(0, i);
            Vector3 value = array[k];
            array[k] = array[i];
            array[i] = value;
        }
        return array;
    }


    public void SpawnEmojis()
    {
        PhotonNetwork.Instantiate("Models/Prefab/Angry1", new Vector3(3.89f, 4, 1.25f), Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Crying1", new Vector3(2.57f, 6, 1.25f), Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Embarassed1", new Vector3(0.91f, 4, 1.25f), Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Laughing1", new Vector3(0f, 6, 1.25f), Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Involve1", new Vector3(-1.42f, 4, 1.25f), Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Smiling1", new Vector3(-3.44f, 4, 1.25f), Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Surprised1", new Vector3(-2.92f, 6, 1.25f), Quaternion.identity, 0);

        PhotonNetwork.Instantiate("Models/Prefab/Angry2", new Vector3(-3.44f, 4, -1.25f), Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Crying2", new Vector3(-2.92f, 6, -1.25f), Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Embarassed2", new Vector3(-1.42f, 4, -1.25f), Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Laughing2", new Vector3(0f, 6, -1.25f), Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Involve2", new Vector3(0.91f, 4, -1.25f), Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Smiling2", new Vector3(3.89f, 4, -1.25f), Quaternion.identity, 0);
        PhotonNetwork.Instantiate("Models/Prefab/Surprised2", new Vector3(2.57f, 6, -1.25f), Quaternion.identity, 0);

        objectCreated = true;
    }


    public void clickAngry1()
    {
        Debug.Log("click");
        if (!angry1)
        {
            GameObject.Find("Angry1(Clone)").transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            bool b = true;
            photonView.RPC("selectAngry1", RpcTarget.All, b);
        }
        else
        {
            GameObject.Find("Angry1(Clone)").transform.localScale = new Vector3(1f, 1f, 1f);
            bool b = false;
            photonView.RPC("selectAngry1", RpcTarget.All, b);
        }

    }
    [PunRPC]
    void selectAngry1(bool b)
    {
        angry1 = b;
    }

    public void clickAngry2()
    {
        if (!angry2)
        {
            GameObject.Find("Angry2(Clone)").transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            bool b = true;
            photonView.RPC("selectAngry2", RpcTarget.All, b);
        }
        else
        {
            GameObject.Find("Angry2(Clone)").transform.localScale = new Vector3(1f, 1f, 1f);
            bool b = false;
            photonView.RPC("selectAngry2", RpcTarget.All, b);
        }

    }
    [PunRPC]
    void selectAngry2(bool b)
    {
        angry2 = b;
    }




    public void clickEmbarassed1()
    {
        Debug.Log("click");
        if (!embarassed1)
        {
            GameObject.Find("Embarassed1(Clone)").transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            bool b = true;
            photonView.RPC("selectEmbarassed1", RpcTarget.All, b);
        }
        else
        {
            GameObject.Find("Embarassed1(Clone)").transform.localScale = new Vector3(1f, 1f, 1f);
            bool b = false;
            photonView.RPC("selectEmbarassed1", RpcTarget.All, b);
        }

    }
    [PunRPC]
    void selectEmbarassed1(bool b)
    {
        embarassed1 = b;
    }

    public void clickEmbarassed2()
    {
        if (!embarassed2)
        {
            GameObject.Find("Embarassed2(Clone)").transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            bool b = true;
            photonView.RPC("selectEmbarassed2", RpcTarget.All, b);
        }
        else
        {
            GameObject.Find("Embarassed2(Clone)").transform.localScale = new Vector3(1f, 1f, 1f);
            bool b = false;
            photonView.RPC("selectEmbarassed2", RpcTarget.All, b);
        }
    }
    [PunRPC]
    void selectEmbarassed2(bool b)
    {
        embarassed2 = b;
    }






    public void clickCrying1()
    {
        Debug.Log("click");
        if (!crying1)
        {
            GameObject.Find("Crying1(Clone)").transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            bool b = true;
            photonView.RPC("selectCrying1", RpcTarget.All, b);
        }
        else
        {
            GameObject.Find("Crying1(Clone)").transform.localScale = new Vector3(1f, 1f, 1f);
            bool b = false;
            photonView.RPC("selectCrying1", RpcTarget.All, b);
        }

    }
    [PunRPC]
    void selectCrying1(bool b)
    {
        crying1 = b;
    }

    public void clickCrying2()
    {
        if (!crying2)
        {
            GameObject.Find("Crying2(Clone)").transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            bool b = true;
            photonView.RPC("selectCrying2", RpcTarget.All, b);
        }
        else
        {
            GameObject.Find("Crying2(Clone)").transform.localScale = new Vector3(1f, 1f, 1f);
            bool b = false;
            photonView.RPC("selectCrying2", RpcTarget.All, b);
        }
    }
    [PunRPC]
    void selectCrying2(bool b)
    {
        crying2 = b;
    }





    public void clickSurprised1()
    {
        if (!surprised1)
        {
            GameObject.Find("Surprised1(Clone)").transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            bool b = true;
            photonView.RPC("selectSurprised1", RpcTarget.All, b);
        }
        else
        {
            GameObject.Find("Surprised1(Clone)").transform.localScale = new Vector3(1f, 1f, 1f);
            bool b = false;
            photonView.RPC("selectSurprised1", RpcTarget.All, b);
        }

    }
    [PunRPC]
    void selectSurprised1(bool b)
    {
        surprised1 = b;
    }

    public void clickSurprised2()
    {
        if (!surprised2)
        {
            GameObject.Find("Surprised2(Clone)").transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            bool b = true;
            photonView.RPC("selectSurprised2", RpcTarget.All, b);
        }
        else
        {
            GameObject.Find("Surprised2(Clone)").transform.localScale = new Vector3(1f, 1f, 1f);
            bool b = false;
            photonView.RPC("selectSurprised2", RpcTarget.All, b);
        }
    }
    [PunRPC]
    void selectSurprised2(bool b)
    {
        surprised2 = b;
    }




    public void clickInvolve1()
    {
        if (!involve1)
        {
            GameObject.Find("Involve1(Clone)").transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            bool b = true;
            photonView.RPC("selectInvolve1", RpcTarget.All, b);
        }
        else
        {
            GameObject.Find("Involve1(Clone)").transform.localScale = new Vector3(1f, 1f, 1f);
            bool b = false;
            photonView.RPC("selectInvolve1", RpcTarget.All, b);
        }

    }
    [PunRPC]
    void selectInvolve1(bool b)
    {
        involve1 = b;
    }

    public void clickInvolve2()
    {
        if (!involve2)
        {
            GameObject.Find("Involve2(Clone)").transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            bool b = true;
            photonView.RPC("selectInvolve2", RpcTarget.All, b);
        }
        else
        {
            GameObject.Find("Involve2(Clone)").transform.localScale = new Vector3(1f, 1f, 1f);
            bool b = false;
            photonView.RPC("selectInvolve2", RpcTarget.All, b);
        }
    }
    [PunRPC]
    void selectInvolve2(bool b)
    {
        involve2 = b;
    }



    public void clickSmiling1()
    {
        if (!smiling1)
        {
            GameObject.Find("Smiling1(Clone)").transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            bool b = true;
            photonView.RPC("selectSmiling1", RpcTarget.All, b);
        }
        else
        {
            GameObject.Find("Smiling1(Clone)").transform.localScale = new Vector3(1f, 1f, 1f);
            bool b = false;
            photonView.RPC("selectSmiling1", RpcTarget.All, b);
        }

    }
    [PunRPC]
    void selectSmiling1(bool b)
    {
        smiling1 = b;
    }

    public void clickSmiling2()
    {
        if (!smiling2)
        {
            GameObject.Find("Smiling2(Clone)").transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            bool b = true;
            photonView.RPC("selectSmiling2", RpcTarget.All, b);
        }
        else
        {
            GameObject.Find("Smiling2(Clone)").transform.localScale = new Vector3(1f, 1f, 1f);
            bool b = false;
            photonView.RPC("selectSmiling2", RpcTarget.All, b);
        }
    }
    [PunRPC]
    void selectSmiling2(bool b)
    {
        smiling2 = b;
    }




    public void clickLaughing1()
    {
        if (!laughing1)
        {
            GameObject.Find("Laughing1(Clone)").transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            bool b = true;
            photonView.RPC("selectLaughing1", RpcTarget.All, b);
        }
        else
        {
            GameObject.Find("Laughing1(Clone)").transform.localScale = new Vector3(1f, 1f, 1f);
            bool b = false;
            photonView.RPC("selectLaughing1", RpcTarget.All, b);
        }

    }
    [PunRPC]
    void selectLaughing1(bool b)
    {
        laughing1 = b;
    }

    public void clickLaughing2()
    {
        if (!laughing2)
        {
            GameObject.Find("Laughing2(Clone)").transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            bool b = true;
            photonView.RPC("selectLaughing2", RpcTarget.All, b);
        }
        else
        {
            GameObject.Find("Laughing2(Clone)").transform.localScale = new Vector3(1f, 1f, 1f);
            bool b = false;
            photonView.RPC("selectLaughing2", RpcTarget.All, b);
        }
    }
    [PunRPC]
    void selectLaughing2(bool b)
    {
        laughing2 = b;
    }


}
