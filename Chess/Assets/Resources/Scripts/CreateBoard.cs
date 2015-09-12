using UnityEngine;
using System.Collections;

public class CreateBoard : MonoBehaviour {

	public int iBoardSize = 8;
	public Color colSquareBlack;
	public Color colSquareWhite;
	public Color colPieceWhite;
	public Color colPieceBlack;
	public GameObject gobPrefabSquare;
	public GameObject gobPrefabPawn;
	public GameObject gobPrefabRook;
	public GameObject gobPrefabKnight;
	public GameObject gobPrefabBishop;
	public GameObject gobPrefabKing;
	public GameObject gobPrefabQueen;
	public GameObject gobBoard;

	private float fSquareSizeY;
	private float fSquareSizeX;
	private float fSquareSizeZ;
	private GameObject[,] gobSquares;

	// Use this for initialization
	void Start () {
		gobSquares = new GameObject[iBoardSize, iBoardSize];

		fSquareSizeX = gobPrefabSquare.transform.localScale.x;
		fSquareSizeY = gobPrefabSquare.transform.localScale.y;
		fSquareSizeZ = gobPrefabSquare.transform.localScale.z;

		gobBoard = GameObject.FindGameObjectWithTag ("BOARD");

		GenerateBoard (iBoardSize);
		PlacePieces ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlacePieces(){
		for (int i = 0; i < 2; i++) {
			for(int j = 0; j < iBoardSize; j++){
				GameObject gob = Instantiate (gobPrefabPawn);
				gob.transform.parent = gobBoard.transform;
				gob.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
				GameObject square = gobSquares [(i == 0) ? 1 : iBoardSize - 2, j];
				gob.transform.localPosition = new Vector3 (square.transform.localPosition.x, 0.0f, square.transform.localPosition.z);

				Property_Piece prop = gob.AddComponent<Property_Piece>() as Property_Piece;

				bool white = true;

				if(i == 0){
					gob.GetComponent<Renderer>().material.color = colPieceWhite;
				}else{
					gob.GetComponent<Renderer>().material.color = colPieceBlack;
					white = false;
				}

				prop.SetProperties(white, square, "PAWN");
			}

			for(int j = 0; j < 2; j++){
				GameObject gob = Instantiate(gobPrefabRook);
				gob.transform.parent = gobBoard.transform;
				gob.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
				GameObject square = gobSquares[(i == 0) ? 0 : iBoardSize - 1, (j == 1) ?  0 : iBoardSize - 1];
				gob.transform.localPosition = new Vector3(square.transform.localPosition.x, 0.0f, square.transform.localPosition.z);


				Property_Piece prop = gob.AddComponent<Property_Piece>() as Property_Piece;

				bool white = true;

				if(i == 0){
					gob.GetComponent<Renderer>().material.color = colPieceWhite;
				}else{
					gob.GetComponent<Renderer>().material.color = colPieceBlack;
					white = false;
				}

				prop.SetProperties(white, square, "ROOK");
			}

			for(int j = 0; j < 2; j++){
				GameObject gob = Instantiate(gobPrefabKnight);
				gob.transform.parent = gobBoard.transform;
				gob.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
				GameObject square = gobSquares[(i == 0) ? 0 : iBoardSize - 1, (j == 1) ?  1 : iBoardSize - 2];
				gob.transform.localPosition = new Vector3(square.transform.localPosition.x, 0.0f, square.transform.localPosition.z);
				
				
				Property_Piece prop = gob.AddComponent<Property_Piece>() as Property_Piece;
				
				bool white = true;
				
				if(i == 0){
					gob.GetComponent<Renderer>().material.color = colPieceWhite;
				}else{
					gob.GetComponent<Renderer>().material.color = colPieceBlack;
					white = false;
				}
				
				prop.SetProperties(white, square, "KNIGHT");
			}

			for(int j = 0; j < 2; j++){
				GameObject gob = Instantiate(gobPrefabBishop);
				gob.transform.parent = gobBoard.transform;
				gob.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
				GameObject square = gobSquares[(i == 0) ? 0 : iBoardSize - 1, (j == 1) ?  2 : iBoardSize - 3];
				gob.transform.localPosition = new Vector3(square.transform.localPosition.x, 0.0f, square.transform.localPosition.z);
				
				
				Property_Piece prop = gob.AddComponent<Property_Piece>() as Property_Piece;
				
				bool white = true;
				
				if(i == 0){
					gob.GetComponent<Renderer>().material.color = colPieceWhite;
				}else{
					gob.GetComponent<Renderer>().material.color = colPieceBlack;
					white = false;
				}
				
				prop.SetProperties(white, square, "BISHOP");
			}

			GameObject gobKing = Instantiate(gobPrefabKing);
			gobKing.transform.parent = gobBoard.transform;
			gobKing.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			GameObject squareKing = gobSquares[(i == 0) ? 0 : iBoardSize - 1, iBoardSize -4];
			gobKing.transform.localPosition = new Vector3(squareKing.transform.localPosition.x, 0.0f, squareKing.transform.localPosition.z);
			
			
			Property_Piece propKing = gobKing.AddComponent<Property_Piece>() as Property_Piece;
			
			bool whiteKing = true;
			
			if(i == 0){
				gobKing.GetComponent<Renderer>().material.color = colPieceWhite;
			}else{
				gobKing.GetComponent<Renderer>().material.color = colPieceBlack;
				whiteKing = false;
			}
			
			propKing.SetProperties(whiteKing, squareKing, "KING");

			GameObject gobQueen = Instantiate(gobPrefabQueen);
			gobQueen.transform.parent = gobBoard.transform;
			gobQueen.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			GameObject squareQueen = gobSquares[(i == 0) ? 0 : iBoardSize - 1, iBoardSize -5];
			gobQueen.transform.localPosition = new Vector3(squareQueen.transform.localPosition.x, 0.0f, squareQueen.transform.localPosition.z);
			
			
			Property_Piece propQueen = gobQueen.AddComponent<Property_Piece>() as Property_Piece;
			
			bool whiteQueen = true;
			
			if(i == 0){
				gobQueen.GetComponent<Renderer>().material.color = colPieceWhite;
			}else{
				gobQueen.GetComponent<Renderer>().material.color = colPieceBlack;
				whiteQueen = false;
			}
			
			propQueen.SetProperties(whiteQueen, squareQueen, "QUEEN");
		}
	}

	public void GenerateBoard(int size){
		for (int i = 0; i < size; i++) {
			for(int j = 0; j < size; j++){
				float fPosX = -(size/2.0f)+(j*fSquareSizeX)+(fSquareSizeX/2.0f);
				float fPosY = -fSquareSizeY/2.0f;
				float fPosZ = -(size/2.0f)+(i*fSquareSizeZ)+(fSquareSizeZ/2.0f);
				GameObject gob = Instantiate(gobPrefabSquare);
				gob.transform.parent = gobBoard.transform;
				gob.transform.localPosition = new Vector3(fPosX, fPosY, fPosZ);

				Property_Square prop = gob.AddComponent<Property_Square>() as Property_Square;

				if(j % 2 == 0 && i % 2 != 0){
					// White
					gob.GetComponent<Renderer>().material.color = colSquareWhite;
					prop.SetProperties(true, i, j);
				}else if(j % 2 != 0 && i % 2 == 0){
					// White
					gob.GetComponent<Renderer>().material.color = colSquareWhite;
					prop.SetProperties(true, i, j);
				}else{
					// Black
					gob.GetComponent<Renderer>().material.color = colSquareBlack;
					prop.SetProperties(false, i, j);
				}

				gobSquares[i, j] = gob;
			}
		}
	}
}
