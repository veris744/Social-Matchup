using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[Serializable]
public struct PuzzleCombination {
	public GameObject puzzlePiece;
	public int row;
	public int column;
}

public class Puzzle : MonoBehaviour
{

	private List<PuzzleCombination> puzzleCombinations = new List<PuzzleCombination>();
	private int rows;
	private int cols;
	public Vector3 center = new Vector3(0,0,0);

	// Use this for initialization
	void Start () {
		foreach (Transform child in transform)
		{
			//Setting anchor points for puzzling
			AnchorPoint anchorPoint = child.gameObject.GetComponent<AnchorPoint>();
			if(anchorPoint!=null) anchorPoint.puzzling = true;
			
			PuzzleCombination newCombination = new PuzzleCombination();
			newCombination.puzzlePiece = child.gameObject;
			string name = child.gameObject.name;
			string coordinates = name.Substring(name.IndexOf('(')+1, 2);
			newCombination.row = int.Parse(coordinates.Substring(0,1));
			newCombination.column = int.Parse(coordinates.Substring(1,1));
			//Debug.Log("Row" + newCombination.row + " Col" +  newCombination.column);
			puzzleCombinations.Add(newCombination);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// It initializes the puzzle matrix dimensions (max dimension 5x5)
	/// </summary>
	/// <param name="rows"></param>
	/// <param name="cols"></param>
	public void SetPuzzleMatrix(int rows, int cols)
	{
		this.rows = rows;
		this.cols = cols;
		
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				if (i >= rows || j >= cols)
				{
					int i_nameTag = i+1, j_nameTag = j+1;
					//Debug.Log("AnchorPoint (" + i_nameTag + j_nameTag + ")");
					gameObject.transform.Find("AnchorPoint (" + i_nameTag + j_nameTag + ")").gameObject.SetActive(false);
				}
			}
		}
	}

	/// <summary>
	/// Use this function to set the proper distance among the anchor points, based on the puzzle piece dimensions
	/// </summary>
	/// <param name="width"></param>
	/// <param name="height"></param>
	public void SetPuzzlePieceDimension(float width, float height)
	{
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				int i_nameTag = i+1, j_nameTag = j+1;
				GameObject anchorPoint = gameObject.transform.Find("AnchorPoint (" + i_nameTag + j_nameTag + ")").gameObject;
				anchorPoint.transform.localPosition = new Vector3((i)*height, anchorPoint.transform.localPosition.y, (j)*width);
				float markDimension = (width < height) ? width : height; 
				anchorPoint.transform.Find("Mark").gameObject.transform.localScale = new Vector3((markDimension-1f < 0.5f)?(0.5f):(markDimension-1f), (markDimension-1f < 0.5f)?(0.5f):(markDimension-1f),1);
			}
		}
		center = new Vector3(0, height*rows/2,width*cols/2);
	}

	/// <summary>
	/// Deprecated
	/// </summary>
	/// <returns></returns>
	public bool IsCompleted()
	{
		Debug.Log("-----------------Puzzle State----------------------");
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				//GameObject puzzlePiece = puzzleCombination.puzzlePiece.GetPhotonView().gameObject.GetComponent<AnchorPoint>().anchoredObject;
				GameObject puzzlePiece = gameObject.transform
					.Find("AnchorPoint (" + i + j + ")").gameObject
					.GetPhotonView().gameObject.GetComponent<AnchorPoint>().anchoredObject;
				if (puzzlePiece != null) puzzlePiece = puzzlePiece.GetPhotonView().gameObject;

				Debug.Log(gameObject.transform
					.Find("AnchorPoint (" + i + j + ")").gameObject.name);
				if (puzzlePiece != null)
					Debug.Log("Anchor point " + i + j + ": puzzle piece "
					          + ((int) (puzzlePiece.GetComponent<PuzzlePiece>().correctRow + 1)) +
					          ((int) (puzzlePiece.GetComponent<PuzzlePiece>().correctColumn + 1)));

				if (puzzlePiece == null
				    || puzzlePiece.GetComponent<PuzzlePiece>().correctRow + 1 != i
				    || puzzlePiece.GetComponent<PuzzlePiece>().correctColumn + 1 != j)
				{
					Debug.Log("----------------------------------------------------");
					return false;
				}
			}
		}

		Debug.Log("----------------------------------------------------");
		return true;
	}
}
