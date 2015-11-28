using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour {

	public float afterSecs = 5f;

	// Use this for initialization
	void Awake () {
		Destroy (gameObject, afterSecs);
	}

}
