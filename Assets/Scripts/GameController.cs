using UnityEngine;
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

	// Use this for initialization
	void Start () {
	
		hangman = GameObject.FindGameObjectWithTag("Player").GetComponent<HangmanController>();
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
        player.userScore = 0;
        player.timeTaken = timeIndicator.text;

			if(finalStatus){
				statusIndicator.text = "You won! \n Please press any key";
				statusIndicator.color = Color.green; 
			}
			else{
				statusIndicator.text = "You Lost! \n Please press any key";
				statusIndicator.color = Color.red;
			}

        logWriter.log(player);
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
		int score = 0;
		
		for(int i = 0;i < revealed.Length;i++){
			if(c == word[i]){
					ret = true;
					if(revealed[i] == 0){
						revealed[i] = c;
						score++;
					}
			}
			
			if(revealed[i] != 0){
				complete++;
			}
		}

			//Score Mainpulation
			if(score != 0){
				this.score += score;
				if(complete == revealed.Length){
					this.completed = true;
					this.score += revealed.Length;
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
        setword(Dictionary.instance.next(0));
		statusIndicator.text = "";
		initialTime = Time.time;
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
        string word = climateWord.word;
        word = word.ToUpper();
		this.word = word;
        this.hint = climateWord.meaning;
		revealed = new char[word.Length];
		lettesIndicator.text = "Letters: " + word.Length;
	
		updatewordIndicator();
	}
	
	
	public void reset(){
		score = 0;
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
