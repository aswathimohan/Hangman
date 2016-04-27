using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour {


    public Text currentScoreText;
    public Text highScoreText;
    
	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {

        Score scoreController = gameObject.GetComponent<Score>();
        currentScoreText.text = "Your Score: " + scoreController.score;
        highScoreText.text = "High Score: " + scoreController.highScore;

    }

    public void startNewGame()
    {
        Application.LoadLevel("Hangman");

    }

}
