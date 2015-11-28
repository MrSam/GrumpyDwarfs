using UnityEngine;
using System.Collections;

public class MouseSelector : MonoBehaviour {


	GameObject selectedTarget;

	public UnityEngine.UI.Text dwarfName;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray,out hit, 100)) {
			Debug.DrawLine (ray.origin, hit.point);
			if(hit.collider.tag == "Enemy") {


				if(hit.collider.gameObject == selectedTarget)
					return;

				if(hit.collider.gameObject != selectedTarget && selectedTarget != null)
					selectedTarget.GetComponentInChildren<Light>().enabled = false;

				Animator anim = hit.collider.gameObject.GetComponent<Animator>();
				anim.Play("Attack");
				hit.collider.gameObject.GetComponent<AudioSource>().Play();
				dwarfName.text = hit.collider.gameObject.name;
				hit.collider.gameObject.GetComponentInChildren<Light>().enabled = true;

				selectedTarget = hit.collider.gameObject;
				GameSettings.selectedDwarf = hit.collider.gameObject.name;

			} 
		}
	}

	public void LoadLevel(string levelName) {
		Application.LoadLevel(levelName);
		Destroy (this.gameObject);
	}

	public void hideObject(GameObject obj) {
		obj.SetActive (false);
	}

	public void showObject(GameObject obj) {
		obj.SetActive (true);
	}
}
