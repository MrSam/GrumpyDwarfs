using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {


	public GameObject healthBar;
	public float maxEnemyHealth = 100;
	private float enemyHealth = 100;
	public Color color_100;
	public Color color_50;
	public Color color_30;

	public GameObject deathPrefab;

	private RectTransform healthRectTransform;
	private bool isDying = false;
	private float prevHealth;
	private float initialHealthWidth;
	private bool healthBarVisible = false;
	private Animator m_Animator;

	void Start() {
		enemyHealth = maxEnemyHealth;

		healthRectTransform = healthBar.GetComponent<RectTransform> ();
		m_Animator = GetComponent<Animator> ();

		prevHealth = enemyHealth;
		initialHealthWidth = healthRectTransform.rect.width;
	}

	public void ShowHealthBar() {
		if (healthBarVisible)
			return;

		this.healthBar.SetActive (true);
		healthBarVisible = true;
	}

	public void HideHealthBar() {
		if (!healthBarVisible)
			return;

		this.healthBar.SetActive (false);
		healthBarVisible = false;
	}

	public bool IsDead() {
		return this.isDying;
	}

	// return true on good hit, false if he's dead/dying
	public bool DecreaseEnemyHealth(float val) {
		if (isDying)
			return false;

		this.enemyHealth -= val;

		if (this.enemyHealth <= 0) {
			StartCoroutine ("Death");
			return false;
		}

		return true;
	}

	void Death() {
		isDying = true;
		m_Animator.SetBool ("death", true);
		this.gameObject.tag = "Dead";
	
		GameManager.gm.Addpoints((int) maxEnemyHealth);
		Instantiate (deathPrefab, this.transform.position, this.transform.rotation);
		Destroy (this.gameObject);
	}

	void OnGUI() {
		// avoid unnecessary calculations
		if (prevHealth == enemyHealth)
			return;
		
		healthRectTransform.sizeDelta = getHealth();
		
		if (enemyHealth <= maxEnemyHealth && enemyHealth >= (maxEnemyHealth * 0.9f)) {
			healthBar.GetComponent<RawImage> ().color = color_100;
		}
		
		if (enemyHealth < (maxEnemyHealth * 0.9f) && enemyHealth > (maxEnemyHealth * 0.4f)) {
			healthBar.GetComponent<RawImage> ().color = color_50;
		}
		
		if (enemyHealth < (maxEnemyHealth * 0.4f) ) {
			healthBar.GetComponent<RawImage> ().color = color_30;
		}
		
		// save this 
		prevHealth = enemyHealth;
	}

	Vector2 getHealth() {
		float healthInPercentage = (enemyHealth / maxEnemyHealth) * 100;
		Vector2 healthVector = new Vector2 ((healthInPercentage / 100) * initialHealthWidth, healthRectTransform.rect.height);
		
		return healthVector;
	}
}
