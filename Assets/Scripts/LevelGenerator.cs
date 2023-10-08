using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	public static LevelGenerator instance;
	
	public Transform levelStartPoint;
	public List<LevelPieceBasic> levelPrefabs = new List<LevelPieceBasic>();
	public List<LevelPieceBasic> pieces = new List<LevelPieceBasic>();
	private int piecesNumber = 0;
	private int randomIndex = 0;
	
	public LevelPieceBasic startPlatformPrefab;
	public LevelPieceBasic endPlatformPrefab;
	
	public int maxGameTime = 60;
	public bool shouldFinish = false;
	
    // Start is called before the first frame update
    void Start()
    {
        instance=this;
		
		ShowPiece((LevelPieceBasic)Instantiate (startPlatformPrefab));
		
		AddPiece();
    }

	public void ShowPiece(LevelPieceBasic piece)
	{
		piece.transform.SetParent(this.transform, false);
		
		if(pieces.Count < 1)
			piece.transform.position = levelStartPoint.position;
		else
			piece.transform.position = pieces [pieces.Count - 1].exitPoint.position + new Vector3(20, 0, 0);
		
		pieces.Add(piece);
	}

	public void AddPiece()
	{
		randomIndex = Random.Range(0, levelPrefabs.Count);
		LevelPieceBasic piece = (LevelPieceBasic)Instantiate (levelPrefabs[randomIndex]);
		ShowPiece(piece);
	}
	
	public void RemoveOldestPiece()
	{
		if(pieces.Count > 3){
			
			LevelPieceBasic oldestPiece = pieces[0];
			pieces.RemoveAt(0);
			Destroy (oldestPiece.gameObject);
		}
	}
	
	public void Finish()
	{
		shouldFinish = true;
		ShowPiece((LevelPieceBasic)Instantiate(endPlatformPrefab));
	}
}
