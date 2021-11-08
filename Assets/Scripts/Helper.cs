﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Helper : MonoBehaviour
{
    void Start()
    {
        //disable the camera for all players except the one who owns the Helper (otherwise other players may 
        //see the game from the wrong camera
        if (GetComponent<PhotonView>().IsMine == false)
        {
            this.GetComponent<Camera>().enabled = false;
            this.transform.Find("GvrReticlePointer").gameObject.SetActive(false);
        }
    }
}
