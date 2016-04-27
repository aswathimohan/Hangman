using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {


    public int score = 0, highScore = 0;
    private string scoreKey = "currentScore", highScoreKey = "highScore";

	// Use this for initialization
	void Start () {
	

        score = PlayerPrefs.GetInt(scoreKey, score);
        highScore  = PlayerPrefs.GetInt(highScoreKey, highScore);

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void saveScore(int inScore)
    {
    
        score = inScore;
        if(score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(highScoreKey, highScore);
        }


        PlayerPrefs.SetInt(scoreKey, score);
    }

}
