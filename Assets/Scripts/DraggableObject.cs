using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

//class for the object used in Sorting game mode. Provides methods to drag and drop the object and to notify the 
// SortingGameManager wether the object is placed on a AnchorPoint
public class DraggableObject : MonoBehaviour
{
    public Transform camTransform;
    private Rigidbody rb;
    private bool dragging;
    private bool droppable;
    private Vector3 lastFramePosition; //used to check if the player is moving the object
    private Vector3 lastAnchorPointPosition;
    private int index; //ID of the object, to distinguish it between the others
    private GameObject gameManager; //reference to the manager, needed to send info on the network e check wether the player is dragging an object

    public bool puzzling = false;
    public int playerId;

    private bool isOnAnchor;

    private GameObject anchorPoint;
    private GameObject overAnchorPoint;
    public GameObject light;
    private Color[] colors = new Color[] { Color.red, Color.green, Color.blue, Color.yellow, Color.cyan, Color.magenta, Color.white };
    private float distance;

    private bool isPositioning;
    private bool isDropping = false;
    private Coroutine positioningCoroutine;

    private float rightLimit;
    private float leftLimit;
    private float upLimit;
    private float downLimit;

    void Start ()
    {
        camTransform = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        droppable = true;
        lastAnchorPointPosition = Vector3.zero;

        //disable the script and the event trigger if the object is not mine (in this way the other player cannot drag it)
        if (!puzzling)
        {
            if (GetComponent<PhotonView>().IsMine == false)
            {
                this.GetComponent<EventTrigger>().enabled = false;
                this.enabled = false;
            }
        }
        else
        {
            upLimit = GameObject.Find("WallLimitUp").transform.position.y;
            downLimit = GameObject.Find("WallLimitDown").transform.position.y;
            rightLimit = GameObject.Find("WallLimitRight").transform.position.z;
            leftLimit = GameObject.Find("WallLimitLeft").transform.position.z;
        }
        
    }	

	void Update ()
    {
        if (dragging)
        {
            if (!puzzling)
            {

                //follow the camera movements
                if (transform.position.x <= 8)
                    transform.position = camTransform.position +
                                         camTransform.forward * (3 + Mathf.Abs(transform.position.x / 2f));
                else
                {
                    transform.position = camTransform.position +
                                         camTransform.forward * (3 + Mathf.Abs((transform.position.x - 17) / 2f));
                }


                //avoid going under y = 0.7 (otherwise it will go under the pavement)
                if (transform.position.y < 0.7f)
                    transform.position = new Vector3(transform.position.x, 0.7f, transform.position.z);

                //if the player stops moving, drops the object
                if (Mathf.Abs((transform.position - lastFramePosition).magnitude) < 0.06f && !isDropping)
                    StartCoroutine("Drop");
                else if (Mathf.Abs((transform.position - lastFramePosition).magnitude) < 0.06f && isDropping)
                {
                }
                else
                {
                    StopAllCoroutines();
                    isDropping = false;
                }

                lastFramePosition = transform.position;
            }

            else
            {
                float distance = Vector3.Distance(camTransform.position, transform.position);

                float yCoord = camTransform.position.y + camTransform.forward.y * distance;
                float zCoord = camTransform.position.z + camTransform.forward.z * distance;
                int id = gameObject.GetPhotonView().OwnerActorNr;
                float newZ;
                float newY;
                if (zCoord > rightLimit) newZ = rightLimit;
                else if (zCoord < leftLimit) newZ = leftLimit;
                else newZ = zCoord;
                if (yCoord > upLimit) newY = upLimit;
                else if (yCoord < downLimit) newY = downLimit;
                else newY = yCoord;
                transform.position = new Vector3(transform.position.x,newY,newZ);
                
                //if the player stops moving, drops the object
                if(!isOnAnchor && Mathf.Abs((transform.position - lastFramePosition).magnitude) < 0.06f && !isDropping)
                    StartCoroutine("Drop");
                else if (isOnAnchor && !isPositioning && Mathf.Abs((transform.position - lastFramePosition).magnitude) < 0.06f)
                {
                    positioningCoroutine = StartCoroutine("PositionPuzzlePiece");
                }
                else if (isOnAnchor && isPositioning)
                {
                    //DO NOTHING
                }
                else if (!isOnAnchor && Mathf.Abs((transform.position - lastFramePosition).magnitude) < 0.06f && isDropping)
                {}
                else
                {
                    StopAllCoroutines();
                    isPositioning = false;
                    isDropping = false;
                }
                
                

                lastFramePosition = transform.position;
            }
        }


        if (gameManager == null)
            if (!puzzling)
            {
                try
                {
                    gameManager = GameObject.Find("SortingGameManager(Clone)");
                }
                catch (System.NullReferenceException e) { Debug.Log("Manager not found!"); }
            }
            else
            {
                try
                {
                    gameManager = GameObject.Find("PuzzlingGameManager(Clone)");
                }
                catch (System.NullReferenceException e)
                {
                    Debug.Log("Manager not found!");
                }
            }


        if (Vector3.Distance(this.transform.position, lastAnchorPointPosition) > 1)
            droppable = true;
            
    }

    //called when the player starts looking at the object
    public void OnGazeEnter()
    {
        if (enabled)
        {
            if (!puzzling)
            {
                if (gameManager.GetComponent<SortingGameManager>().IsPlayerDragging == false)
                {
                    distance = Vector3.Distance(camTransform.position, transform.position);
                    StartCoroutine("Drag");
                } //if the player is not already dragging an object
            }
            else
            {
                if (!gameManager.GetComponent<PuzzlingGameManager>().IsPlayerDragging)
                {
                    distance = Vector3.Distance(camTransform.position, transform.position);
                    StartCoroutine("Drag");
                }
            }
        }
    }

    //called when the player stops looking at the object
    public void OnGazeExit()
    {
        if(enabled)
            StopAllCoroutines();
    }

    //wait a few seconds, then drag the object and notifies the SortingGameManager
    private IEnumerator Drag()
    {
        yield return new WaitForSeconds(2.5f);
        AudioManager.instance.PlayPopSound();
        dragging = true;
        if (!puzzling)
        {
            gameManager.GetComponent<SortingGameManager>().IsPlayerDragging = true;
        }
        else
        {
            gameManager.GetComponent<PuzzlingGameManager>().IsPlayerDragging = true;
        }
        light.SetActive(true); 

        if (anchorPoint != null && gameObject.GetPhotonView().IsMine)
        {
            anchorPoint.GetComponent<AnchorPoint>().anchoredObject = null;
            
            if (puzzling)
            {
                anchorPoint.GetPhotonView().RPC("OnObjectRemoved",RpcTarget.Others);
                int i = this.gameObject.GetComponent<PuzzlePiece>().correctRow + 1;
                int j = this.gameObject.GetComponent<PuzzlePiece>().correctColumn + 1;
                string pos = i.ToString() + j.ToString();
                string anchor = anchorPoint.name;
                Debug.Log("anchor = " + anchor);
                if (anchor.Contains(pos))
                {
                    int teamID = anchorPoint.transform.parent.gameObject.name == "PuzzleTeam1" ? 1 : 2;
                    Debug.Log("Removing Object, team = " + teamID);
                    if(puzzling) gameManager.GetPhotonView().RPC("OnObjectRemoved", RpcTarget.All, gameObject.GetPhotonView().Owner.ActorNumber);
                    else gameManager.GetPhotonView().RPC("OnObjectRemoved", RpcTarget.All);
                }
            }
            
            anchorPoint = null;
        }
    }

    private IEnumerator Drop()
    {
        isDropping = true;
        yield return new WaitForSeconds(3.5f);
        dragging = false;
        
        AudioManager.instance.PlayPopSound();

        light.SetActive(false);
        if (!puzzling)
            gameManager.GetComponent<SortingGameManager>().IsPlayerDragging = false; //now the player is free to pick up a new object
        else gameManager.GetComponent<PuzzlingGameManager>().IsPlayerDragging = false;
    }

    private IEnumerator PositionPuzzlePiece()
    {
        isPositioning = true;
        yield return new WaitForSeconds(1.2f);
        dragging = false;
        light.SetActive(false);
        AudioManager.instance.PlayDingSound();
        anchorPoint = overAnchorPoint;
        anchorPoint.GetComponent<AnchorPoint>().anchoredObject = this.gameObject;
        anchorPoint.GetPhotonView().RPC("OnObjectPositioned",RpcTarget.Others, gameObject.GetPhotonView().ViewID);
        this.lastAnchorPointPosition = anchorPoint.transform.position;
        droppable = false;
        gameManager.GetComponent<PuzzlingGameManager>().IsPlayerDragging = false;
        int i = this.gameObject.GetComponent<PuzzlePiece>().correctRow + 1;
        int j = this.gameObject.GetComponent<PuzzlePiece>().correctColumn + 1;
        string pos = i.ToString() + j.ToString();
        string anchor = anchorPoint.name;
        Debug.Log("ANCHOR IS " + anchor);
        Debug.Log("POS IS " + pos);
        if (anchor.Contains(pos))
        {
            Debug.Log("CONTROLLO ESEGUITO, OK");
            int teamID = anchorPoint.transform.parent.gameObject.name == "PuzzleTeam1" ? 1 : 2;
            gameManager.GetPhotonView().RPC("OnObjectPositioned", RpcTarget.All, gameObject.GetPhotonView().Owner.ActorNumber);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if collides with the AnchorPoint, stick it to it and notifies the SortingGameManager
        if (other.tag == "AnchorPoint" && gameObject.GetPhotonView().IsMine && other.GetComponent<AnchorPoint>().anchoredObject == null && droppable)
        {
            if (puzzling)
            {
                isOnAnchor = true;
                overAnchorPoint = other.gameObject;
            }
            else
            {

                dragging = false;
                light.SetActive(false);
                AudioManager.instance.PlayDingSound();
                other.GetComponent<AnchorPoint>().anchoredObject = this.gameObject;
                this.anchorPoint = other.gameObject;
                this.lastAnchorPointPosition = other.transform.position;
                droppable = false;
                gameManager.GetComponent<SortingGameManager>().IsPlayerDragging = false; //now the player is free to pick up a new object
                //notifies the SortingGameManager
                gameManager.GetPhotonView().RPC("OnObjectPositioned", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, this.index, other.gameObject.GetComponent<AnchorPoint>().Index);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "AnchorPoint" && !isOnAnchor && puzzling && gameObject.GetPhotonView().IsMine && other.GetComponent<AnchorPoint>().anchoredObject == null && droppable)
        {
            isOnAnchor = true;
            overAnchorPoint = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "AnchorPoint" && puzzling)
        {
            isOnAnchor = false;
            overAnchorPoint = null;
            if (isPositioning)
            {
                isPositioning = false;
                StopCoroutine(positioningCoroutine);
            }
        }
    }

    [PunRPC]
    public void DisableDraggable()
    {
        this.GetComponent<EventTrigger>().enabled = false;
        this.enabled = false;
    }
    
    
    [PunRPC]
    public void SetIndex(int index)
    {
        this.index = index;
        GetComponent<MeshRenderer>().material.color = colors[index];
        transform.Find("Tip").gameObject.GetComponent<MeshRenderer>().material.color = colors[index];
        light.SetActive(true);
        light.GetComponent<Light>().color = colors[index];
        light.SetActive(false);
    }
}
