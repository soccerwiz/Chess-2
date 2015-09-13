using UnityEngine;
using System.Collections;

public class Property_Square : MonoBehaviour {

	public bool bWhite;
	public bool bOccupied;
	public bool bOccupiedWhite;
	public string sOccupiedType;
	public GameObject gobPiece;
	public int iRow;
	public int iColumn;
	public int iId;

	public void SetProperties(bool white, int i, int j, int id){
		bWhite = white;
		iRow = i;
		iColumn = j;
		iId = id;
	}

	public void SetPiece(bool placing, bool white = false, string type = "", GameObject piece = null){
		if (placing) {
			bOccupiedWhite = white;
			bOccupied = true;
			sOccupiedType = type;
			gobPiece = piece;
		} else {
			bOccupiedWhite = false;
			bOccupied = false;
			sOccupiedType = "";
			gobPiece = null;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
