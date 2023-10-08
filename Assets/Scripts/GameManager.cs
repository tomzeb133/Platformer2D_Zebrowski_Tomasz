using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public enum GameState{
		GS_PAUSEMENU,
		GS_GAME,
		GS_LEVELCOMPLETED,
		GS_GAME_OVER
	}
	
	public GameState currentGameState = GameState.GS_PAUSEMENU;
	public static GameManager instance;
	public Canvas pauseMenuCanvas;
	public Canvas inGameCanvas;
	public Canvas gameOverCanvas;
	public Canvas levelCompletedCanvas;
	
	// MONEY
	public Text coinsText;
	private int coins = 0;
	
	// LIFE
	public Text heartsText;
	private int hearts = 3;
	
	// ENEMIES
	public Text goblinsText;
	private int goblins = 0;
	
	// SCROLLS
	public Image[] scrollsTab;
	public Image[] heartsTab;
	private int scrolls = 0;

	
	// TIME
	private float timer = 0;
	public Text timerText;
	private int minutes = 0;
	private int seconds = 0;
	private String minutesString;
	private String secondsString;
	
	// SCORE
	private int finalPoints=0;
	public Text finalPointsText;
	private String finalPointsString;
	
	
	// HIGHSCORE 
	public Text highScoreText; 
	private int maxSecsToHighscore = 5 * 60;
	
	void Awake()
	{
		instance = this;
		InGame();
		coinsText.text = coins.ToString();
		
		if(!PlayerPrefs.HasKey("HighScoreLevel1"))
		{
			PlayerPrefs.SetInt("HighScoreLevel1", 0);
		}
	}
	
	
	void SetGameState (GameState newGameState)
	{
		currentGameState = newGameState;
		
		if(newGameState == GameState.GS_LEVELCOMPLETED){
			Scene currentScene = SceneManager.GetActiveScene();
			if(currentScene.name == "Level1"){
				//int score = hearts * 20 + coins * 10 + goblins * 50 + (maxSecsToHighscore - (int)timer) * 20;
				finalPoints = hearts*10 + coins*7 + goblins*5 - minutes*3 -  seconds*2;
				if (PlayerPrefs.GetInt ("HighScoreLevel1") < finalPoints)
					PlayerPrefs.SetInt("HighScoreLevel1", finalPoints);
				highScoreText.text = "HighScore: " + PlayerPrefs.GetInt("HighScoreLevel1").ToString();
				finalPointsText.text = "Points:" + finalPoints.ToString();
			
			}
		}
		
		inGameCanvas.enabled = (currentGameState == GameState.GS_GAME);
		pauseMenuCanvas.enabled = (currentGameState == GameState.GS_PAUSEMENU);
		gameOverCanvas.enabled = (currentGameState == GameState.GS_GAME_OVER);
		levelCompletedCanvas.enabled = (currentGameState == GameState.GS_LEVELCOMPLETED);
		
		
	}
	
	public void InGame()
	{
		SetGameState(GameState.GS_GAME);
	}
	
	public void GameOver()
	{
		
		SetGameState(GameState.GS_GAME_OVER);
	}
	
	public void PauseMenu()
	{
		SetGameState(GameState.GS_PAUSEMENU);
	}
	
	public void LevelCompleted()
	{
		SetGameState(GameState.GS_LEVELCOMPLETED);
		//finalPoints = hearts*10 + coins*7 + goblins*5 - minutes*3 -  seconds*2;
		//finalPointsString = "Points: " + finalPoints.ToString();
		//finalPointsText.text = finalPointsString;
	}
	


    // Update is called once per frame
    void Update()
    {
		
		if(Input.GetKeyDown(KeyCode.Escape) && currentGameState == GameState.GS_PAUSEMENU)
		{
			InGame();
		}
		if(Input.GetKeyDown(KeyCode.Escape) && currentGameState == GameState.GS_GAME)
		{
			PauseMenu();
		}
	/*
		timer += Time.deltaTime;
		timerText.text = (string.Format("{0:00}:{1:00}", Math.Floor(timer/60), timer%60));
		
		if(timer > LevelGenerator.instance.maxGameTime && !LevelGenerator.instance.shouldFinish)
			LevelGenerator.instance.Finish();
	*/
    }
	
	// Button Elements
	public void OnResumeButtonClicked()
	{
		InGame();
	}
	
	public void OnRestartButtonClicked()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}
	
	public void OnExitButtonClicked()
	{
		SceneManager.LoadScene ("MainMenu");
	}
	
	public void OnNextLevelButtonClicked()
	{
		SceneManager.LoadScene("Level2");
	}
	
	
	// HUD Elements
	
	public void addCoins(int coinNumber)
	{
		coins += coinNumber;
		coinsText.text = coins.ToString();
	}
	
	public void addHearts(int heartNumber)
	{
		hearts += heartNumber;
		heartsText.text = hearts.ToString();
	}
	
	public void addGoblins()
	{
		goblins += 1;
		goblinsText.text = goblins.ToString();
	}
	
	public void addScrolls(int scrollsNumber)
	{
		scrollsTab[scrollsNumber].color = Color.black;
		//scrollsTab[scrollsNumber].SetActive(false);
		scrolls += 1;
		//if (scrolls == 3)
		//	scrollsCompleted = true;
	}
	
	public void lostHeart(int HeartNumber)
	{
		hearts -= 1;
		heartsText.text = hearts.ToString();
		if (hearts <= 0)
			GameOver();
	}
	

	public void updateClock()
	{
		timer += Time.deltaTime;
		seconds =  (int)timer;
		
		if (seconds == 60)
		{
			minutes += 1;
			seconds = 0;
		}
		
		if (minutes < 10)
			minutesString = "0" + minutes.ToString();
		else
			minutesString = minutes.ToString();
		
		if (seconds < 10)
			secondsString = "0" + seconds.ToString();
		else
			secondsString = seconds.ToString();
		
		timerText.text = minutesString + ":" + secondsString;
	}
	
}
