using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreKeeper : MonoBehaviour {
	public int score = 0;
	public int levelUp = 10000;
	private Text scoreText;


	public void Start(){
		scoreText = gameObject.GetComponent<Text> ();
		Reset ();
	}

	public void Score (int points){
		score += points;
		scoreText.text = score.ToString ();
	}

	public void Reset(){
		score = 0;
		scoreText.text = score.ToString ();
	}

	public int getScore(){
		return score;
	}

	public int getLevel(int score){
		return (int) score / levelUp;
	}
}
