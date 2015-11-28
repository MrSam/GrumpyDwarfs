using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager gm;

	public Text pointsLabel;
	public Text timerLabel;

	public GameObject restartButton;
	public GameObject levelComplete;

	public Text endPointsLabel;
	public Text endTimerLabel;
	
	public float levelTimer = 300f;

	public AudioClip levelSong;

	private float start_time;
	private AudioSource cameraAudioSource;

	public bool gameRunning = true;

	void Start() {
		if (gm == null) 
			gm = this.gameObject.GetComponent<GameManager>();

		pointsLabel.text = "0";
		cameraAudioSource = Camera.main.GetComponent<AudioSource> ();
		cameraAudioSource.clip = levelSong;
		cameraAudioSource.Play ();
	}

	void Update() {
		if (!gameRunning) 
			return;

		float remainingTime = levelTimer - Time.timeSinceLevelLoad;

		// play music faster and faster
		if (remainingTime < levelTimer / 3) {
			if(cameraAudioSource.pitch < 1.20f)
				cameraAudioSource.pitch = 1.20f;
		} else if(remainingTime < levelTimer / 2) {
				cameraAudioSource.pitch = 1.10f;
		}

		System.TimeSpan t = System.TimeSpan.FromSeconds( remainingTime );
		timerLabel.text = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);

		if (levelTimer - Time.timeSinceLevelLoad <= 0) {
			gameRunning = false;
			GameObject.FindWithTag("Player").GetComponent<PlayerHealth>().DecreasePlayerHealth(10000f);

		}
	}

	public void Addpoints(int points) {
		int currentPoints = System.Convert.ToInt32 (pointsLabel.text);
		currentPoints += points;
		pointsLabel.text = currentPoints.ToString();

	}

	public void playerDeath() {
		restartButton.SetActive (true);

		// reset pitch
		cameraAudioSource.Stop ();
		cameraAudioSource.pitch = 1f;
		gameRunning = false;
	}

	public void EndOfLevel ()
	{
		endPointsLabel.text = "Points: " + pointsLabel.text;
		endTimerLabel.text = "Time: " + timerLabel.text;
		pointsLabel.text = "";
		timerLabel.text = "";

		levelComplete.SetActive (true);
		gameRunning = false;
		GameObject.FindWithTag ("Player").SetActive (false);

		Camera.main.GetComponent<CameraFollower> ().SetHeight (40);

		// show restart button ?
		restartButton.SetActive (true);

		// stop music
		cameraAudioSource.Stop ();

	}

	public void reloadLevel() {
		Application.LoadLevel(Application.loadedLevel);
		gameRunning = true;
	}
}
