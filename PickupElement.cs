using UnityEngine;
using System.Collections;

public class PickupElement : MonoBehaviour {

	public AudioClip collisionClip;
	public GameObject pickupExplode = null;
	public Vector3 pickupExplodeRot;
	public int numberOfPoints = 250;

	public bool doRotate = true;
	public bool doUpDown = true;

	public bool endOfLevel = false;

	public bool destructAfterPickup = true;

	public float floatStrength = 0.4f;
	float originalY;

	void Start() {
		this.originalY = this.transform.position.y;
	}

	void Update() {
		if (doRotate) {
			transform.Rotate (0, 45 * Time.deltaTime, 0);
		}
		if (doUpDown) {
			transform.position = new Vector3 (transform.position.x,
		                                 originalY + ((float)System.Math.Sin (Time.time) * floatStrength),
		                                 transform.position.z);
		}

	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			if(endOfLevel == true) {
				GameManager.gm.EndOfLevel();
			}

			if(numberOfPoints > 0)
				GameManager.gm.Addpoints(numberOfPoints);

			if(collisionClip != null)
				AudioSource.PlayClipAtPoint (collisionClip, this.transform.position);

			if(pickupExplode != null)
				this.transform.Rotate(pickupExplodeRot);
				Instantiate(pickupExplode,this.transform.position, this.transform.rotation);

			if(destructAfterPickup)
				Destroy (gameObject);
		}
	}

}
