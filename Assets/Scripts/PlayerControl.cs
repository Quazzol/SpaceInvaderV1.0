using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	private float minX, maxX, padding;
	private bool isPaused;
	private GameObject levelUp;
	private ScoreKeeper scoreKeeper;

	public static bool isLevelUp;

	public int level, currentLevel;
	public float speed = 15;
	public GameObject laserPrefab, dieTextPrefab, playAgainPrefab;
	public float laserSpeed = 5;
	public float firingRate = 0.2f;
	public float health = 250;
	public Text healthText;
	public AudioClip fireSound, lostSound;

	private void Start () {
		float distance = this.gameObject.transform.position.z - Camera.main.transform.position.z;
		Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightMost = Camera.main.ViewportToWorldPoint (new Vector3(1,0,distance));
		padding = 0.5f;
		minX = leftMost.x + padding;
		maxX = rightMost.x - padding;
		healthText.text = health.ToString ();

		/*Levelling*/
		scoreKeeper = GameObject.Find ("Score").GetComponent<ScoreKeeper> ();
		level = 1;
		levelUp = GameObject.Find ("LevelUp").gameObject;
		levelUp.SetActive (false);

	}

	private void Update () {
		if (Input.GetKeyDown (KeyCode.P)) {
			gamePause ();
		}

		if (Input.GetKey (KeyCode.LeftArrow))
			this.gameObject.transform.position += Vector3.left * speed * Time.deltaTime;
		else if (Input.GetKey(KeyCode.RightArrow))
			this.gameObject.transform.position += Vector3.right * speed * Time.deltaTime;

		if (Input.GetKeyDown (KeyCode.Space)) {
			InvokeRepeating ("fire", 0.000001f, firingRate);
		}

		if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke ("fire");
		}

		float newX = Mathf.Clamp (this.gameObject.transform.position.x, minX, maxX);
		this.gameObject.transform.position = new Vector3 (newX, this.gameObject.transform.position.y,
															this.gameObject.transform.position.z);

		/*Levelling*/
		currentLevel = scoreKeeper.getLevel (scoreKeeper.getScore ()) + 1;
		if (level != currentLevel) {
			setLevel (currentLevel);
			isLevelUp = true;
		}
		
	}

	public void OnCollisionEnter2D(Collision2D col){
		Laser missile = col.gameObject.GetComponent<Laser> ();
		if (missile) {
			missile.Hit ();
			health -= missile.GetDamage ();
			healthText.text = health.ToString ();
			if (health <= 0) {
				healthText.text = "0";
				die ();
			}
		}
	}

	private void fire(){
		GameObject beam = Instantiate (laserPrefab, transform.position, Quaternion.identity) as GameObject;
		beam.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0, laserSpeed,0);
		AudioSource.PlayClipAtPoint (fireSound, transform.position);
	}

	private void die(){
		Transform transformOfCanvas = GameObject.Find ("Canvas").GetComponent<Transform> ();
		GameObject dieText = Instantiate (dieTextPrefab, transformOfCanvas.position,Quaternion.identity) as GameObject;
		dieText.transform.parent = transformOfCanvas;
		Vector3 playAgainPos = transformOfCanvas.position + new Vector3 (0, -100, 0);
		GameObject playAgain = Instantiate (playAgainPrefab, playAgainPos ,Quaternion.identity) as GameObject;
		playAgain.transform.parent = transformOfCanvas;
		AudioSource.PlayClipAtPoint (lostSound, transform.position);
		Destroy (gameObject);
	}

	private void gamePause(){
		isPaused = !isPaused;
		Time.timeScale = isPaused ? 0: 1;
	}

	public void setLevel(int _level){
		levelUp.SetActive (true);
//		Enemy.setEnemyInfo (_level);
		levelUp.SetActive (false);
	}

	private void levelUpMessage(){
		levelUp.SetActive(true);
	}

}
