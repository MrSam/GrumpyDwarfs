using UnityEngine;
using System.Collections;

public class MoveBridge : MonoBehaviour {

	public Transform bridgeTransform;
	private float initialHeight;
	public AudioClip loweringSound;

	bool isUsed = false;

	void Start() {
		initialHeight = bridgeTransform.position.y;

		// move the bridge up so we can lower it
		while (bridgeTransform.transform.position.y <= initialHeight + 5f) {
			bridgeTransform.Translate(Vector3.up * 0.1f);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (isUsed)
			return;

		if (other.gameObject.tag == "Player") {
			isUsed = true;

			//Hide it
			gameObject.GetComponent<MeshRenderer>().enabled = false;

			//move bridge down
			StartCoroutine(LowerTheBridge());

			//play clip
			if(loweringSound != null)
				AudioSource.PlayClipAtPoint(loweringSound, bridgeTransform.transform.position);

			// self destruct
			Destroy(gameObject,5f);
		}
	}

	IEnumerator LowerTheBridge() {
		while(bridgeTransform.position.y > initialHeight) {
			bridgeTransform.Translate(Vector3.down * 0.1f);
			yield return new WaitForSeconds(0.05f);
		}
	}
}
