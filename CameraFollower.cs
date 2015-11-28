using UnityEngine;

public class CameraFollower : MonoBehaviour
{

	// The target we are following
	private Transform target;
	// The distance in the x-z plane to the target
	private float distance = 7f;
	// the height we want the camera to be above the target
	private float height = 2f;
	private float rotationDamping = 0.5f;
	private float heightDamping = 0.5f;

	// Use this for initialization
	void Start ()
	{ 
		this.target = GameObject.FindWithTag ("Player").gameObject.transform;
	}

	public void SetHeight (float val)
	{
		this.height = val;
	}

	// Update is called once per frame
	void LateUpdate ()
	{
		// Early out if we don't have a target
		if (!target)
			return;

		// Calculate the current rotation angles
		var wantedRotationAngle = target.eulerAngles.y;
		var wantedHeight = target.position.y + height;

		var currentRotationAngle = transform.eulerAngles.y;
		var currentHeight = transform.position.y;

		// Damp the rotation around the y-axis
		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

		// Damp the height
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);

		// Convert the angle into a rotation
		var currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);

		// Set the position of the camera on the x-z plane to:
		// distance meters behind the target
		transform.position = target.position;
		transform.position -= currentRotation * Vector3.forward * distance;

		// Set the height of the camera
		transform.position = new Vector3 (transform.position.x, currentHeight, transform.position.z);

		// Always look at the target
		transform.LookAt (target);
		//transform.LookAt (target);
	}
}