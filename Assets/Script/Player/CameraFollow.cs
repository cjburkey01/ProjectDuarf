using UnityEngine;

public class CameraFollow : MonoBehaviour {
	
	public Transform follow;
	public float smoothing = 0.1f;

	private Vector3 vel;
	private Vector3 current;
	private float z = -10;

	void Start() {
		z = transform.position.z;
	}

	void Update() {
		current = transform.position;
		current = Vector3.SmoothDamp(transform.position, follow.position, ref vel, smoothing);
		current.z = z;
		transform.position = current;
	}

}