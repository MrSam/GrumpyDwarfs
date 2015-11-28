using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerHealth : MonoBehaviour {

	public GameObject healthBar;
	public float playerHealth = 100; // from 100 till 0;
	public float maxPlayerHealth = 100;
	public Color color_100;
	public Color color_50;
	public Color color_30;
	public AudioClip deathSound;

	private bool isDying = false;

	private RectTransform healthRectTransform;
	private float prevHealth;
	private Animator m_Animator;

	private float lastHealthDecrease;

	void Start() {
		healthRectTransform = healthBar.GetComponent<RectTransform> ();
		prevHealth = playerHealth;
		m_Animator = GetComponent<Animator> ();
	}

	public float GetPlayerHealth() {
		return playerHealth;
	}

	public void Update() {
		if(playerHealth < maxPlayerHealth && lastHealthDecrease < Time.time - 5f)
			IncreasePlayerHealth(Time.deltaTime * 5f);

		// go faster if you are really low
		if(playerHealth < maxPlayerHealth / 2 && lastHealthDecrease < Time.time - 5f)
			IncreasePlayerHealth(Time.deltaTime * 5f);
	}

	public void SetPlayerHealth(float val) {
		this.playerHealth = val;
		lastHealthDecrease = Time.time;
	}

	public bool DecreasePlayerHealth(float val) {
		lastHealthDecrease = Time.time;

		if (isDying)
			return false;

		this.playerHealth -= val;

		if (this.playerHealth <= 0) {
			StartCoroutine ("PlayerDeath");
			return false;
		} else {
			StartCoroutine("PlayerDamage");
		}

		return true;
	}

	public void IncreasePlayerHealth(float val) {
		if (isDying)
			return;

		if(this.playerHealth + val <= maxPlayerHealth)
			this.playerHealth += val;

		if (this.playerHealth + val > maxPlayerHealth)
			this.playerHealth = maxPlayerHealth;
	}

	void OnGUI() {
		// avoid unnecessary calculations
		if (prevHealth == playerHealth)
			return;

		healthRectTransform.sizeDelta = getHealth();

		if (playerHealth <= maxPlayerHealth && playerHealth >= maxPlayerHealth / 2) {
			healthBar.GetComponent<RawImage> ().color = color_100;
		}

		if (playerHealth < maxPlayerHealth /2 && playerHealth > maxPlayerHealth / 3) {
			healthBar.GetComponent<RawImage> ().color = color_50;
		}

		if (playerHealth < 30) {
			healthBar.GetComponent<RawImage> ().color = color_30;
		}

		// save this 
		prevHealth = playerHealth;
	}

	IEnumerator PlayerDamage() {
		m_Animator.SetBool ("impact", true);
		yield return new WaitForSeconds(0.5f);
		m_Animator.SetBool ("impact", false);
	}

	IEnumerator PlayerDeath() {
		if (!isDying) {
			isDying = true;
			m_Animator.SetBool ("death", true);
			this.gameObject.GetComponent<ThirdPersonUserControl>().enabled = false;
			yield return new WaitForSeconds (0.5f);
			AudioSource.PlayClipAtPoint (deathSound, transform.position);
			yield return new WaitForSeconds (3f);

			GameManager.gm.playerDeath ();
		}
	}


	Vector2 getHealth() {
		Vector2 healthVector = new Vector2 (playerHealth, healthRectTransform.rect.height);

		return healthVector;
	}
}
