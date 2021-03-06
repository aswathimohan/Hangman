﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
	
	public Text wordIndicator;
	public Text scoreIndicator;
	public Text lettesIndicator;
	public Text statusIndicator;
	public Text timeIndicator;
    public Text hintText;
	
	
	public HangmanController hangman;
	private string word;
    private string hint;
	private char[] revealed;
	private int score;
	private bool completed;
	private bool finalStatus;
	private float initialTime;
    private XMLLogWriter logWriter;
    private bool hintUsed;
    public int correctWordMark = 5;
    private ClimateWord[] playedWords;
    private int indexPlayedWords = 0;

	// Use this for initialization
	void Start () {
        hangman = GameObject.FindGameObjectWithTag("Player").GetComponent<HangmanController>();
        playedWords = new ClimateWord[Dictionary.instance.totalWords()];
		reset();
        logWriter = new XMLLogWriter();
        logWriter.setFileName("playHangman.xml");
	}

	// Update is called once per frame
	void Update () {

		timerStatus ();
		if(completed){
			string temp = Input.inputString;
			if(Input.anyKeyDown){
				next();
				return;
			}
		}
	
		
//		if(!Input.anyKeyDown)
//			return;
		
		string s = Input.inputString;
	
		if(s.Length == 1 && TextUtils.isAlpha(s[0])){
			if(!check(s.ToUpper()[0])){
				hangman.punish();
				Debug.Log(s);
				
				if(hangman.isDead){
					wordIndicator.text = word;
					completed = true;
					finalStatus = false;
					updateGameStatus(finalStatus);
				
				}
				
			}
		
		}
	
	}
	
	
public void updateGameStatus(bool finalStatus){

        LogPlayer player = new LogPlayer();
        player.correctWord = this.word;
        player.hintsUsed = this.hintUsed;
        string userAnswer = new string(revealed);
        player.userAnswer = userAnswer;
        player.timeTaken = timeIndicator.text;
       

        if (finalStatus){
				statusIndicator.text = "You won! \n Please press any key";
				statusIndicator.color = Color.green;
                player.userScore = correctWordMark;
        }
			else{
				statusIndicator.text = "You Lost! \n Please press any key";
				statusIndicator.color = Color.red;
                 player.userScore = 0;
        }

        logWriter.log(player);
       
        indexPlayedWords++;
        

        if(indexPlayedWords == Dictionary.instance.totalWords())

        {
            Score scoreController = gameObject.GetComponent<Score>();
            scoreController.saveScore(this.score);
            Application.LoadLevel("GameOver");
        }

	}
	
	/*		
	public bool gameStatus(){
	
			int complete = 0;
			for(int i = 0;i < revealed.Length;i++){
				if(revealed[i] != 0){
						complete++;
				}
			}
			
		if (complete == word.Length) {
			return true;
		} else {
			return false;
		}

}*/
	
	
	public bool check(char c){
		bool ret = false;
		int complete = 0;
		//int score = 0;
		
		for(int i = 0;i < revealed.Length;i++){
			if(c == word[i]){
					ret = true;
					if(revealed[i] == 0){
						revealed[i] = c;
					//	score++;
					}
			}
			
			if(revealed[i] != 0){
				complete++;
			}
		}

        //Score Mainpulation,if(score != 0)
        {

            if (complete == revealed.Length && this.completed == false){
                    this.score += correctWordMark;
                    this.completed = true;
					//this.score += revealed.Length;
					finalStatus = true;
				updateGameStatus(finalStatus);
				}
				updateScoreIndicator();
				updatewordIndicator();
				
			}
		
	
		return ret;
	}
	
	
	public void next(){
		hangman.reset();
		completed = false;
        hintUsed = false;
        hintText.gameObject.SetActive(false);
        // setword(Dictionary.instance.next(0));
        setword(findAUniqueWord());
		statusIndicator.text = "";
		initialTime = Time.time;
	}
	
    private ClimateWord findAUniqueWord()
    {
        ClimateWord uniqueWord = Dictionary.instance.next(0);
        while(this.isPlayed(uniqueWord) == true)
        {
            uniqueWord = Dictionary.instance.next(0);
        }

        return uniqueWord;
    }

    private bool isPlayed(ClimateWord word)
    {
        for(int i = 0;i < indexPlayedWords; i++)
        {
            ClimateWord currentWord = playedWords[i];
            if(currentWord.word == word.word)
            {
                return true;
            }
        }

        return false;
    }


    public void enableHint()
    {


        hintText.gameObject.SetActive(!hintText.gameObject.active);
        hintText.text = this.hint;

        if (!hintUsed)
        {
            hintUsed = hintText.gameObject.active;
        }
    }


	private void updatewordIndicator(){
	
		string displayed = "";
		
		for(int i = 0; i < revealed.Length;i++){
			char c = revealed[i];
			if(c == 0){
				c = '-';
			}
			
			displayed += ' ';
			displayed += c; 
		}
		Debug.Log(displayed);
		wordIndicator.text = displayed;
	
	}
	
	public void updateScoreIndicator(){
	
		scoreIndicator.text = "Score: " + score;
	
	}
	
	
	private void setword(ClimateWord climateWord)
    {
        playedWords[indexPlayedWords] = climateWord;
        string word = climateWord.word;
        word = word.ToUpper();
		this.word = word;
        this.hint = climateWord.meaning;
		revealed = new char[word.Length];
		lettesIndicator.text = "Letters: " + word.Length;
        Debug.Log(word);
		updatewordIndicator();
	}
	
	
	public void reset(){
		score = 0;
        indexPlayedWords = 0;

        updateScoreIndicator();
		next();
	}

	private void timerStatus(){
		
		float totalTime = Time.time - initialTime;

		int minutes = (int)totalTime / 60;
		int seconds = (int)totalTime % 60;

		timeIndicator.text = string.Format ("{0:00}:{1:00}", minutes, seconds);

	}
	
}
