using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	
	public Text highscoreLevel1Text;
	
	void Awake()
	{
		if(!PlayerPrefs.HasKey("HighscoreLevel1"))
		{
			PlayerPrefs.SetInt("HighscoreLevel1", 0);
		}
		highscoreLevel1Text.text = "Highscore: " + PlayerPrefs.GetInt("HighscoreLevel1");
	
	}
	
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		//if(Input.GetKeyDown (KeyCode.S))
		//	onLevel1ButtonPressed();
        
    }
	
	private IEnumerator StartGame(string levelName)
	{
		yield return new WaitForSeconds(0.1f);
		SceneManager.LoadScene(levelName);
	}
	
	public void onLevel1ButtonPressed()
	{
		StartCoroutine(StartGame("Level1"));
	}
	
	public void onLevel2ButtonPressed()
	{
		StartCoroutine(StartGame("Level2"));
	}
	
}
