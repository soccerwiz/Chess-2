using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public GameObject gobPick;
	public GameObject gobBoard;
	public List<GameObject> lstValidSquares = new List<GameObject>();

	private GameObject _gobPieceInHand;
	private Property_Piece _propPiece;
	private Property_Square _propSquare;
	private CreateBoard brdBoardManager;
	private bool _bPieceInHand = false;

	// Use this for initialization
	void Start () {
		gobBoard = GameObject.FindGameObjectWithTag ("BOARD");
		brdBoardManager = gobBoard.GetComponent<CreateBoard> () as CreateBoard;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit)){
				if(hit.collider.gameObject.tag == "SQUARE" || hit.collider.gameObject.tag == "PIECE"){
					Debug.Log ("HIT: " + hit.collider.gameObject.name);
					if(hit.collider.gameObject.tag == "PIECE"){
						if(!_bPieceInHand){
							Vector3 vecPickPos = new Vector3(hit.transform.localPosition.x, gobPick.transform.localPosition.y, hit.transform.localPosition.z);
							gobPick.transform.localPosition = vecPickPos;
							PickPiece(hit.collider.gameObject);
						}else{
							PlacePiece(hit.collider.gameObject);
						}
					}else{
						if(_bPieceInHand){
							PlacePiece(hit.collider.gameObject);
						}
					}
				}
			}
		}

		if (Input.GetMouseButtonDown (1) && _bPieceInHand) {
			PlacePiece(_propPiece.gobSquare);
		}
	}

	public void PlacePiece(GameObject square){
		if (_propPiece.gobSquare == square || _gobPieceInHand == square) {
			Debug.Log ("Returning: " + _gobPieceInHand.name);
			_gobPieceInHand.transform.parent = gobBoard.transform;
			_gobPieceInHand.transform.localPosition = new Vector3(_propPiece.gobSquare.transform.localPosition.x, 0.0f, _propPiece.gobSquare.transform.localPosition.z);
			_bPieceInHand = false;
			_gobPieceInHand = null;
			_propPiece = null;
			_propSquare = null;
		}else if(lstValidSquares.Contains(square)){
			Debug.Log("Moving: " + _gobPieceInHand.name);
			_gobPieceInHand.transform.parent = gobBoard.transform;
			_gobPieceInHand.transform.localPosition = new Vector3(square.transform.localPosition.x, 0.0f, square.transform.localPosition.z);
			Property_Square propSquarePlacing = square.GetComponent<Property_Square>() as Property_Square;
			if(propSquarePlacing.bOccupied){
				Destroy(propSquarePlacing.gobPiece);
			}
			_propPiece.SetPosition(propSquarePlacing.iRow, propSquarePlacing.iColumn);
			_propPiece.bHasMoved = true;
			_propSquare.SetPiece(false);
			_propPiece.gobSquare = square;
			propSquarePlacing.SetPiece(true, _propPiece.bIsWhite, _propPiece.sType, _gobPieceInHand);
			_bPieceInHand = false;
			_gobPieceInHand = null;
			_propPiece = null;
			_propSquare = null;
			lstValidSquares.Clear();
		}else if(square.tag == "PIECE"){
			Property_Piece propPieceTaking = square.GetComponent<Property_Piece>() as Property_Piece;
			if(propPieceTaking.bIsWhite != _propPiece.bIsWhite && lstValidSquares.Contains(propPieceTaking.gobSquare)){
				Debug.Log ("Taking Piece: " + square.name);
				_gobPieceInHand.transform.parent = gobBoard.transform;
				_gobPieceInHand.transform.localPosition = new Vector3(square.transform.localPosition.x, 0.0f, square.transform.localPosition.z);
                _propPiece.gobSquare.GetComponent<Property_Square>().SetPiece(false);
				_propPiece.gobSquare = propPieceTaking.gobSquare;
				_propPiece.iCurrentRow = propPieceTaking.iCurrentRow;
				_propPiece.iCurrentColumn = propPieceTaking.iCurrentColumn;
				_propPiece.gobSquare.GetComponent<Property_Square>().SetPiece(true, _propPiece.bIsWhite, _propPiece.sType, _gobPieceInHand);
				_bPieceInHand = false;
				_gobPieceInHand = null;
				_propPiece = null;
				_propSquare = null;
				Destroy (square);
			}
		}
	}

	public bool ValidMove(GameObject square, bool bMovePieceWhite, string sMovePieceType){
		Property_Square propSquareProposed = square.GetComponent<Property_Square> () as Property_Square;
		if (!propSquareProposed.bOccupied) {
			return true;
		}else if((propSquareProposed.bOccupiedWhite != bMovePieceWhite) && propSquareProposed.sOccupiedType != "KING"){
			return true;
		}
		return false;
	}

    public bool ContainsPiece(GameObject square)
    {
        Property_Square propSquareCheck = square.GetComponent<Property_Square>() as Property_Square;
        if (propSquareCheck.bOccupied)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool KingCanBeTaken(bool white, int iRow, int iColumn)
    {
        Debug.Log("King Move");
        // N E S W
        for(int i = 0; i < 4; i++)
        {
            int iRowCheck = iRow;
            int iColumnCheck = iColumn;

            int iIncrementRow = (i == 0 || i == 2) ? ((i == 0) ? 1 : -1) : 0;
            int iIncrementColumn = (i == 1 || i == 3) ? ((i == 1) ? 1: -1): 0;

            bool notValid = false;

            //Debug.Log(iIncrementRow + " | " + iIncrementColumn);

            while(iRowCheck + iIncrementRow >= 0 && iRowCheck + iIncrementRow < 8 && iColumnCheck + iIncrementColumn >= 0 && iColumnCheck + iIncrementColumn < 8 && !notValid)
            {
                iRowCheck += iIncrementRow;
                iColumnCheck += iIncrementColumn;

                GameObject gobSquareCheck = brdBoardManager.gobSquares[iRowCheck, iColumnCheck];
                Property_Square propSquareCheck = gobSquareCheck.GetComponent<Property_Square>() as Property_Square;
                if (propSquareCheck.bOccupied)
                {
                    Debug.Log(iRowCheck + " | " + iColumnCheck);
                    if(propSquareCheck.bOccupiedWhite != white)
                    {
                        switch (propSquareCheck.sOccupiedType)
                        {
                            case "KING":
                                Debug.Log("KING FOUND");

                                int kingPosRow = propSquareCheck.iRow;
                                int kingPosCol = propSquareCheck.iColumn;

                                if ((kingPosCol == iColumn + 1 || kingPosCol == iColumn - 1) && kingPosRow == iRow)
                                {
                                    return true;
                                }

                                if ((kingPosRow == iRow + 1 || kingPosRow == iRow - 1) && kingPosCol == iColumn)
                                {
                                    return true;
                                }

                                break;
                            default:
                                return false;
                                break;
                        }
                    }
                }
                else
                {
                    notValid = true;
                }
            }
        }

        // TODO Intercardinal directions
        // NE SE SW NW
        for(int i = 0; i < 4; i++)
        {

        }
        return false;
    }

	public void PickPiece(GameObject piece){
		_gobPieceInHand = piece;
		_bPieceInHand = true;
		piece.transform.parent = gobPick.transform;
		piece.transform.localPosition = new Vector3 (0.0f, 0.0f, 0.0f);
		_propPiece = piece.GetComponent<Property_Piece> () as Property_Piece;
		_propSquare = _propPiece.gobSquare.GetComponent<Property_Square> () as Property_Square;
		int iRow = _propPiece.iCurrentRow;
		int iColumn = _propPiece.iCurrentColumn;
		bool white = _propPiece.bIsWhite;
		string sType = _propPiece.sType;
		switch (sType) {
		case "PAWN":
			int iPawnMoveSquares = 1;

			if(!white){
				iPawnMoveSquares *= -1;
			}

			if(iRow + iPawnMoveSquares < 8 && iRow + iPawnMoveSquares >= 0){
				if(ValidMove(brdBoardManager.gobSquares[iRow + iPawnMoveSquares, iColumn], white, sType)){
					lstValidSquares.Add (brdBoardManager.gobSquares[iRow + iPawnMoveSquares, iColumn]);
				}
				
				
				if(!_propPiece.bHasMoved){
					iPawnMoveSquares *= 2;
					if(ValidMove(brdBoardManager.gobSquares[iRow + iPawnMoveSquares, iColumn], white, sType)){
						lstValidSquares.Add (brdBoardManager.gobSquares[iRow + iPawnMoveSquares, iColumn]);
					}
					//lstValidSquares.Add (brdBoardManager.gobSquares[iRow + iPawnMoveSquares, iColumn]);
				}
			}
			break;
		case "KNIGHT":
			// All Possible Move Positions
			for(int i = 0; i < 2; i++){
				for(int j = 0; j < 2; j++){
					for(int k = 0; k < 2; k++){
						for(int l = 0; l < 2; l++){
							int iRowCheckValue = (k == 0) ? 1 : 2;
							int iColCheckValue = (l == 0) ? 1 : 2;

							int iRowToCheck = iRow + ((i == 0) ? -iRowCheckValue : iRowCheckValue);
							int iColToCheck = iColumn + ((j == 0) ? -iColCheckValue : iColCheckValue);

							if(iRowToCheck >= 0 && iRowToCheck < 8 && iColToCheck >= 0 && iColToCheck < 8 && iRowCheckValue != iColCheckValue){
                                //Exists
								if(ValidMove(brdBoardManager.gobSquares[iRowToCheck, iColToCheck], white, sType)){
									lstValidSquares.Add (brdBoardManager.gobSquares[iRowToCheck, iColToCheck]);
								}
							}
						}
					}
				}
			}
			break;
		case "BISHOP":
			for(int i = 0; i < 2; i++){
				for (int j = 0; j < 2; j++){
					int iCheckRowValue = iRow;
					int iCheckColValue = iColumn;

                    //int iIncrementValueRow = (j == 0) ? 1 : 2;
                    //int iIncrementValueCol = (j == 0) ? 1 : 2;

                    int iIncrementValueRow = 1;
                    int iIncrementValueCol = 1;

                    int iIncrementingRow = (i == 0) ? iIncrementValueRow : -iIncrementValueRow;
					int iIncrementingCol = (j == 0) ? iIncrementValueCol : -iIncrementValueCol;

					bool notValid = false;
                    bool lastContainedPiece = false;

					while(iCheckColValue + iIncrementingCol >= 0 && iCheckColValue + iIncrementingCol < 8 && iCheckRowValue + iIncrementingRow >= 0 && iCheckRowValue +iIncrementingRow < 8 && !notValid && !lastContainedPiece){
						iCheckRowValue += iIncrementingRow;
						iCheckColValue += iIncrementingCol;
                        
						if(ValidMove(brdBoardManager.gobSquares[iCheckRowValue, iCheckColValue], white, sType)){
							lstValidSquares.Add (brdBoardManager.gobSquares[iCheckRowValue, iCheckColValue]);
                            lastContainedPiece = ContainsPiece(brdBoardManager.gobSquares[iCheckRowValue, iCheckColValue]);
						}else{
							notValid = true;
						}
					}
				}
			}
			break;
        case "ROOK":
            // Check rows
            for(int i = 0; i < 2; i++)
            {
                int iCheckRowValue = iRow;

                int iIncrementValue = (i == 0) ? 1 : -1;

                bool notValid = false;

                while(iCheckRowValue + iIncrementValue >= 0 && iCheckRowValue + iIncrementValue < 8 && !notValid)
                {
                    iCheckRowValue += iIncrementValue;

                    if(ValidMove(brdBoardManager.gobSquares[iCheckRowValue, iColumn], white, sType))
                    {
                        lstValidSquares.Add(brdBoardManager.gobSquares[iCheckRowValue, iColumn]);
                    }
                    else
                    {
                        notValid = true;
                    }
                }
            }
            // Check columns
            for(int i = 0; i < 2; i++)
            {
                int iCheckColValue = iColumn;

                int iIncrementValue = (i == 0) ? 1 : -1;

                bool notValid = false;

                while(iCheckColValue + iIncrementValue >= 0 && iCheckColValue + iIncrementValue < 8 && !notValid)
                {
                    iCheckColValue += iIncrementValue;

                    if(ValidMove(brdBoardManager.gobSquares[iRow, iCheckColValue], white, sType))
                    {
                        lstValidSquares.Add(brdBoardManager.gobSquares[iRow, iCheckColValue]);
                    }
                    else
                    {
                        notValid = true;
                    }
                }
            }
            break;
        case "QUEEN":
            // Check rows
            for (int i = 0; i < 2; i++)
            {
                int iCheckRowValue = iRow;

                int iIncrementValue = (i == 0) ? 1 : -1;

                bool notValid = false;

                while (iCheckRowValue + iIncrementValue >= 0 && iCheckRowValue + iIncrementValue < 8 && !notValid)
                {
                    iCheckRowValue += iIncrementValue;

                    if (ValidMove(brdBoardManager.gobSquares[iCheckRowValue, iColumn], white, sType))
                    {
                        lstValidSquares.Add(brdBoardManager.gobSquares[iCheckRowValue, iColumn]);
                    }
                    else
                    {
                        notValid = true;
                    }
                }
            }
            // Check columns
            for (int i = 0; i < 2; i++)
            {
                int iCheckColValue = iColumn;

                int iIncrementValue = (i == 0) ? 1 : -1;

                bool notValid = false;

                while (iCheckColValue + iIncrementValue >= 0 && iCheckColValue + iIncrementValue < 8 && !notValid)
                {
                    iCheckColValue += iIncrementValue;

                    if (ValidMove(brdBoardManager.gobSquares[iRow, iCheckColValue], white, sType))
                    {
                        lstValidSquares.Add(brdBoardManager.gobSquares[iRow, iCheckColValue]);
                    }
                    else
                    {
                        notValid = true;
                    }
                }
            }
            // Check diagonal
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    int iCheckRowValue = iRow;
                    int iCheckColValue = iColumn;

                    //int iIncrementValueRow = (j == 0) ? 1 : 2;
                    //int iIncrementValueCol = (j == 0) ? 1 : 2;

                    int iIncrementValueRow = 1;
                    int iIncrementValueCol = 1;

                    int iIncrementingRow = (i == 0) ? iIncrementValueRow : -iIncrementValueRow;
                    int iIncrementingCol = (j == 0) ? iIncrementValueCol : -iIncrementValueCol;

                    bool notValid = false;
                    bool lastContainedPiece = false;

                    while (iCheckColValue + iIncrementingCol >= 0 && iCheckColValue + iIncrementingCol < 8 && iCheckRowValue + iIncrementingRow >= 0 && iCheckRowValue + iIncrementingRow < 8 && !notValid && !lastContainedPiece)
                    {
                        iCheckRowValue += iIncrementingRow;
                        iCheckColValue += iIncrementingCol;

                        if (ValidMove(brdBoardManager.gobSquares[iCheckRowValue, iCheckColValue], white, sType))
                        {
                            lstValidSquares.Add(brdBoardManager.gobSquares[iCheckRowValue, iCheckColValue]);
                            lastContainedPiece = ContainsPiece(brdBoardManager.gobSquares[iCheckRowValue, iCheckColValue]);
                        }
                        else
                        {
                            notValid = true;
                        }
                    }
                }
            }
            break;
        case "KING":
            // TODO Check if moving into check

            for(int i = 0; i < 2; i++)
            {
                for(int j = 0; j < 2; j++)
                {
                    int iCheckRowValue = iRow;
                    int iCheckColumnValue = iColumn;

                    int iIncrementRowValue = (j == 0) ? ((i == 0) ? 1 : -1) : 0;
                    int iIncrememntColumnValue = (j == 1) ? ((i == 1) ? 1 : -1) : 0;

                    iCheckRowValue += iIncrementRowValue;
                    iCheckColumnValue += iIncrememntColumnValue;

                    if(iCheckRowValue >= 0 && iCheckRowValue < 8 && iCheckColumnValue >= 0 && iCheckColumnValue < 8)
                    {
                        if(ValidMove(brdBoardManager.gobSquares[iCheckRowValue, iCheckColumnValue], white, sType) && !KingCanBeTaken(white, iCheckRowValue, iCheckColumnValue))
                        {
                            lstValidSquares.Add(brdBoardManager.gobSquares[iCheckRowValue, iCheckColumnValue]);
                        }
                    }
                }
            }
            break;
		}
	}
}
