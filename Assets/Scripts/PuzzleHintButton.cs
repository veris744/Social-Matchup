using System.Collections;
using System.Collections.Generic;
using Managers;
using Photon.Pun;
using UnityEngine;

public class PuzzleHintButton : MonoBehaviour
{
	private GameObject gameManager;
	private bool canBeUsed;
	
	
	
	public void OnGazeEnter()
	{
		if (canBeUsed && gameManager!=null)
		{
			gameManager.GetPhotonView().RPC("ShowHint",RpcTarget.All);
			canBeUsed = false;
			StartCoroutine("Recharge");
		}
	}

	// Use this for initialization
	void Start ()
	{
		canBeUsed = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameManager == null)
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
	}

	private IEnumerator Recharge()
	{
		if (!canBeUsed)
		{
			yield return new WaitForSeconds(15);
			canBeUsed = true;
		}
	}
	
}
