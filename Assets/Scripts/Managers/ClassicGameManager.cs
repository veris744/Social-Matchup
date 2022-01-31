using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ClassicGameManager : GameManager
{

    Vector3 baseEmojiPosition1 = new Vector3(0, 3, 1.1f);
    Vector3 baseEmojiPosition2 = new Vector3(0, 3, -1.1f);
    Vector3[] positionsArray1;
    Vector3[] positionsArray2;
    PhotonView photonView;
    GameObject XRinteractionManager;

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

    bool done;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        positionsArray1 = new[] { new Vector3(-52.09f, 4, 1.25f), new Vector3(-53f, 6, 1.25f), new Vector3(-54.42f, 4, 1.25f),
            new Vector3(-50.43f, 6, 1.25f), new Vector3(-56.44f, 4, 1.25f), new Vector3(-49.11f, 4, 1.25f), new Vector3(-55.92f, 6, 1.25f) };
        positionsArray2 = new[] { new Vector3(-52.09f, 4, -1.25f), new Vector3(-53f, 6, -1.25f), new Vector3(-54.42f, 4, -1.25f),
            new Vector3(-50.43f, 6,-1.25f), new Vector3(-56.44f, 4, -1.25f), new Vector3(-49.11f, 4, -1.25f), new Vector3(-55.92f, 6, -1.25f) };

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

        done = false;

    }


        // Update is called once per frame
        void Update()
    {

        if (init == 0)
        {
            init++;
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("SpawnEmojis", RpcTarget.All, null);
            }
        }


        if (angry1 && angry2)
        {
            photonView.RPC("DestroyGameObject", RpcTarget.All, "Angry");
        }
        if (embarassed1 && embarassed2)
        {
            photonView.RPC("DestroyGameObject", RpcTarget.All, "Embarassed");
        }
        if (crying1 && crying2)
        {
            photonView.RPC("DestroyGameObject", RpcTarget.All, "Crying");
        }
        if (surprised1 && surprised2)
        {
            photonView.RPC("DestroyGameObject", RpcTarget.All, "Surprised");
        }
        if (involve1 && involve2)
        {
            photonView.RPC("DestroyGameObject", RpcTarget.All, "Involve");
        }
        if (smiling1 && smiling2)
        {
            photonView.RPC("DestroyGameObject", RpcTarget.All, "Smiling");
        }
        if (laughing1 && laughing2)
        {
            photonView.RPC("DestroyGameObject", RpcTarget.All, "Laughing");
        }


        if (GameObject.FindGameObjectsWithTag("Emoji").Length == 0 && objectCreated && (!finished1 || !finished2) & !done)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                finished1 = true;
            } else
            {
                finished2 = true;
            }
            photonView.RPC("StartVictoryAnimations", RpcTarget.All, null);
        }
    }


    [PunRPC]
    public void StartVictoryAnimations()
    {
        StartCoroutine(OnVictory());
    }


    protected IEnumerator OnVictory()
    {
        done = true;
        yield return new WaitForSeconds(1);
        AudioManager.instance.PlayHurraySound();
        yield return new WaitForSeconds(3);
        AudioManager.instance.StopMusic();

        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("MainMenu");

        PhotonNetwork.LeaveRoom();
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

    [PunRPC]
    public void SpawnEmojis()
    {
        Vector3[] v1 = Shuffle(positionsArray1);
        Vector3[] v2 = Shuffle(positionsArray2);

        GameObject.Find("Embarassed1").transform.position = v1[0];
        GameObject.Find("Laughing1").transform.position = v1[1];
        GameObject.Find("Involve1").transform.position = v1[2];


        GameObject.Find("Embarassed2").transform.position = v2[0];
        GameObject.Find("Laughing2").transform.position = v2[1];
        GameObject.Find("Involve2").transform.position = v2[2];


        if (PhotonManager.instance.NumberOfImages >= 4)
        {
            GameObject.Find("Crying1").transform.position = v1[3];
            GameObject.Find("Crying2").transform.position = v2[3];
        } else
        {
            Destroy(GameObject.Find("Crying1"));
            Destroy(GameObject.Find("Crying2"));
        }
        if (PhotonManager.instance.NumberOfImages >= 5)
        {
            GameObject.Find("Smiling1").transform.position = v1[4];
            GameObject.Find("Smiling2").transform.position = v2[4];
        }
        else
        {
            Destroy(GameObject.Find("Smiling1"));
            Destroy(GameObject.Find("Smiling2"));
        }
        if (PhotonManager.instance.NumberOfImages >= 6)
        {
            GameObject.Find("Angry1").transform.position = v1[5];
            GameObject.Find("Angry2").transform.position = v2[5];
        }
        else
        {
            Destroy(GameObject.Find("Angry1"));
            Destroy(GameObject.Find("Angry2"));
        }
        if (PhotonManager.instance.NumberOfImages == 7)
        {
            GameObject.Find("Surprised1").transform.position = v1[6];
            GameObject.Find("Surprised2").transform.position = v2[6];
        }
        else
        {
            Destroy(GameObject.Find("Surprised1"));
            Destroy(GameObject.Find("Surprised2"));
        }
        objectCreated = true;
    }


    [PunRPC]
    void DestroyGameObject(String name)
    {
        Destroy(GameObject.Find(name + "1"));
        Destroy(GameObject.Find(name + "2"));
    }



    public void clickAngry1()
    {
        if (!angry1)
        {
            photonView.RPC("selectAngry1", RpcTarget.All, true, new Vector3 (1.5f,1.5f,1.5f));
        }
        else
        {
            photonView.RPC("selectAngry1", RpcTarget.All, false, new Vector3(1f, 1f, 1f));
        }

    }
    [PunRPC]
    void selectAngry1(bool b, Vector3 v)
    {
        GameObject.Find("Angry1").transform.localScale = v;
        angry1 = b;
    }

    public void clickAngry2()
    {
        if (!angry2)
        {
            photonView.RPC("selectAngry2", RpcTarget.All, true, new Vector3(1.5f, 1.5f, 1.5f));
        }
        else
        {
            photonView.RPC("selectAngry2", RpcTarget.All, false, new Vector3(1f, 1f, 1f));
        }

    }
    [PunRPC]
    void selectAngry2(bool b, Vector3 v)
    {
        GameObject.Find("Angry2").transform.localScale = v;
        angry2 = b;
    }




    public void clickEmbarassed1()
    {
        if (!embarassed1)
        {
            photonView.RPC("selectEmbarassed1", RpcTarget.All, true, new Vector3(1.5f, 1.5f, 1.5f));
        }
        else
        {
            photonView.RPC("selectEmbarassed1", RpcTarget.All, false, new Vector3(1f, 1f, 1f));
        }

    }
    [PunRPC]
    void selectEmbarassed1(bool b, Vector3 v)
    {
        GameObject.Find("Embarassed1").transform.localScale = v;
        embarassed1 = b;
    }

    public void clickEmbarassed2()
    {
        if (!embarassed2)
        {
            photonView.RPC("selectEmbarassed2", RpcTarget.All, true, new Vector3(1.5f, 1.5f, 1.5f));
        }
        else
        {
            photonView.RPC("selectEmbarassed2", RpcTarget.All, false, new Vector3(1f, 1f, 1f));
        }
    }
    [PunRPC]
    void selectEmbarassed2(bool b, Vector3 v)
    {
        GameObject.Find("Embarassed2").transform.localScale = v;
        embarassed2 = b;
    }






    public void clickCrying1()
    {
        
        if (!crying1)
        {
            photonView.RPC("selectCrying1", RpcTarget.All, true, new Vector3(1.5f, 1.5f, 1.5f));
        }
        else
        {
            photonView.RPC("selectCrying1", RpcTarget.All, false, new Vector3(1f, 1f, 1f));
        }

    }
    [PunRPC]
    void selectCrying1(bool b, Vector3 v)
    {
        GameObject.Find("Crying1").transform.localScale = v;
        crying1 = b;
    }

    public void clickCrying2()
    {
        if (!crying2)
        {
            photonView.RPC("selectCrying2", RpcTarget.All, true, new Vector3(1.5f, 1.5f, 1.5f));
        }
        else
        {
            photonView.RPC("selectCrying2", RpcTarget.All, false, new Vector3(1f, 1f, 1f));
        }
    }
    [PunRPC]
    void selectCrying2(bool b, Vector3 v)
    {
        GameObject.Find("Crying2").transform.localScale = v;
        crying2 = b;
    }





    public void clickSurprised1()
    {
        if (!surprised1)
        {
            photonView.RPC("selectSurprised1", RpcTarget.All, true, new Vector3(1.5f, 1.5f, 1.5f));
        }
        else
        {
            photonView.RPC("selectSurprised1", RpcTarget.All, false, new Vector3(1f, 1f, 1f));
        }

    }
    [PunRPC]
    void selectSurprised1(bool b, Vector3 v)
    {
        GameObject.Find("Surprised1").transform.localScale = v;
        surprised1 = b;
    }

    public void clickSurprised2()
    {
        if (!surprised2)
        {
            photonView.RPC("selectSurprised2", RpcTarget.All, true, new Vector3(1.5f, 1.5f, 1.5f));
        }
        else
        {
            photonView.RPC("selectSurprised2", RpcTarget.All, false, new Vector3(1f, 1f, 1f));
        }
    }
    [PunRPC]
    void selectSurprised2(bool b, Vector3 v)
    {
        GameObject.Find("Surprised2").transform.localScale = v;
        surprised2 = b;
    }




    public void clickInvolve1()
    {
        if (!involve1)
        {
            photonView.RPC("selectInvolve1", RpcTarget.All, true, new Vector3(1.5f, 1.5f, 1.5f));
        }
        else
        {
            photonView.RPC("selectInvolve1", RpcTarget.All, false, new Vector3(1f, 1f, 1f));
        }

    }
    [PunRPC]
    void selectInvolve1(bool b, Vector3 v)
    {
        GameObject.Find("Involve1").transform.localScale = v;
        involve1 = b;
    }

    public void clickInvolve2()
    {
        if (!involve2)
        {
            photonView.RPC("selectInvolve2", RpcTarget.All, true, new Vector3(1.5f, 1.5f, 1.5f));
        }
        else
        {
            photonView.RPC("selectInvolve2", RpcTarget.All, false, new Vector3(1f, 1f, 1f));
        }
    }
    [PunRPC]
    void selectInvolve2(bool b, Vector3 v)
    {
        GameObject.Find("Involve2").transform.localScale = v;
        involve2 = b;
    }



    public void clickSmiling1()
    {
        if (!smiling1)
        {
            photonView.RPC("selectSmiling1", RpcTarget.All, true, new Vector3(1.5f, 1.5f, 1.5f));
        }
        else
        {
            photonView.RPC("selectSmiling1", RpcTarget.All, false, new Vector3(1f, 1f, 1f));
        }

    }
    [PunRPC]
    void selectSmiling1(bool b, Vector3 v)
    {
        GameObject.Find("Smiling1").transform.localScale = v;
        smiling1 = b;
    }

    public void clickSmiling2()
    {
        if (!smiling2)
        {
            photonView.RPC("selectSmiling2", RpcTarget.All, true, new Vector3(1.5f, 1.5f, 1.5f));
        }
        else
        {
            photonView.RPC("selectSmiling2", RpcTarget.All, false, new Vector3(1f, 1f, 1f));
        }
    }
    [PunRPC]
    void selectSmiling2(bool b, Vector3 v)
    {
        GameObject.Find("Smiling2").transform.localScale = v;
        smiling2 = b;
    }




    public void clickLaughing1()
    {
        if (!laughing1)
        {
            photonView.RPC("selectLaughing1", RpcTarget.All, true, new Vector3(1.5f, 1.5f, 1.5f));
        }
        else
        {
            photonView.RPC("selectLaughing1", RpcTarget.All, false, new Vector3(1f, 1f, 1f));
        }

    }
    [PunRPC]
    void selectLaughing1(bool b, Vector3 v)
    {
        GameObject.Find("Laughing1").transform.localScale = v;
        laughing1 = b;
    }

    public void clickLaughing2()
    {
        if (!laughing2)
        {
            photonView.RPC("selectLaughing2", RpcTarget.All, true, new Vector3(1.5f, 1.5f, 1.5f));
        }
        else
        {
            photonView.RPC("selectLaughing2", RpcTarget.All, false, new Vector3(1f, 1f, 1f));
        }
    }
    [PunRPC]
    void selectLaughing2(bool b, Vector3 v)
    {
        GameObject.Find("Laughing2").transform.localScale = v;
        laughing2 = b;
    }


}
