using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {

	public Animator animator;
	public AudioClip castSound;
	public AudioClip castFailedSound;
	public GameObject spellPrefab;
	public GameObject spellDamagePrefab;
	//public GameObject damagePointsPrefab;
	public float magicRange = 25f;
	public float magicCost = 20f;
	public float magicDamage = 20f;

	public AudioClip hammerSound;

	private bool isCasting = false;
	private GameObject selectedTarget;
	private Renderer selectedRender;
	private EnemyHealth selectedHealth;
	
	private PlayerHealth playerHealth;
	private PlayerMagic playerMagic;

	public void Start() {
		playerHealth = this.GetComponent<PlayerHealth> ();
		playerMagic = this.GetComponent<PlayerMagic> ();
	}

	public void Update() {

		if (playerHealth.playerHealth <= 0)
			return;

		// use the mouse to target people
		// right to select target
		if (Input.GetButtonDown("Fire1") ) {

			// cannot change while casting
			if(isCasting)
				return;

			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );

			UnSelectTarget();

			RaycastHit hitInfo;
			if (Physics.Raycast (ray, out hitInfo, magicRange)) {
				// is it the floor ?
				if(hitInfo.collider.gameObject.tag != "Enemy") 
					return;

				// does the object has health ?
				EnemyHealth targetHealth = hitInfo.collider.gameObject.GetComponent<EnemyHealth>();

				if(targetHealth == null)
					return;

				// do we have the rendered object ?
				Renderer targetRenderer =  hitInfo.collider.gameObject.GetComponentInChildren<Renderer>();

				if(targetRenderer == null)
					return;

				// Owkay, you can select this motherfsker and tag it!
				selectedTarget = hitInfo.collider.gameObject;
				selectedRender = targetRenderer;
				selectedHealth = targetHealth;
				targetRenderer.material.color = Color.red;

				Vector3 point = hitInfo.transform.position;
				point.y = transform.position.y;

				transform.LookAt(point);
			}
		}

		// left to shoot target
		if (Input.GetButtonDown("Fire2")) {
			if (!isCasting && selectedTarget != null) {
				if(playerMagic.GetPlayerMagic() >= magicCost) {
					StartCoroutine ("Shoot");
				} else {
					AudioSource.PlayClipAtPoint (castFailedSound, transform.position);
				}
			}
		}
	}

	void UnSelectTarget() {
		if (!selectedTarget)
			return;

		selectedRender.material.color = Color.white;
		selectedTarget = null;
		selectedRender = null;
		selectedHealth = null;
	}

	public IEnumerator Shoot () {

		isCasting = true;
		// look at it
		Vector3 targetPoint = selectedTarget.transform.position;
		targetPoint.y = transform.position.y;
		transform.LookAt(targetPoint);

		// place fancy animation below player
		Vector3 spellPosition = transform.position;
		spellPosition.y += 0.01f;
		animator.SetBool("attack", true);
		GameObject spell = Instantiate (spellPrefab, spellPosition, transform.rotation) as GameObject;
		AudioSource.PlayClipAtPoint (castSound, transform.position);
		playerMagic.DecreasePlayerMagic(magicCost);



		yield return new WaitForSeconds(0.8f);
		AudioSource.PlayClipAtPoint (hammerSound, transform.position);

		GameObject spellDamage = Instantiate (spellDamagePrefab, selectedTarget.transform.position, selectedTarget.transform.rotation) as GameObject;

		//Destroy (selectedTarget);
		bool goodAttack = selectedHealth.DecreaseEnemyHealth (magicDamage);
		animator.SetBool("attack", false);

		yield return new WaitForSeconds(0.7f);



		if (!goodAttack) {
			// character is dead, unselect
			yield return new WaitForSeconds(0.3f);
			UnSelectTarget();
		}

		Destroy (spell, 0.5f);
		Destroy (spellDamage, 0.5f);
		isCasting = false;

	}
}