﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//utility script to move objects by keyboard
public class Movable : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            transform.Rotate(Vector3.right, -2f);
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            transform.Rotate(Vector3.up, -2);
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            transform.Rotate(Vector3.right, 2f);
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            transform.Rotate(Vector3.up, 2f);
    }
}
