using UnityEngine;
using System.Collections;

public class DeathOnImpact : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player")
			other.gameObject.GetComponent<PlayerHealth>().DecreasePlayerHealth(10000f);
	}
}
