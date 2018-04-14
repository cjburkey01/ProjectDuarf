using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMove : MonoBehaviour {
	
	public float smoothing = 0.1f;
	public float moveSpeed = 10.0f;
	public float zoomSpeed = 150.0f;
	public float zoomRatio = 1.5f;
	public Vector2 zoomBounds = new Vector2(2.0f, 15.0f);

	private Camera cam;
	private float z;
	private Vector2 goalPos;
	private Vector3 velPos;
	private float zoomGoal;
	private float zoomVel;

	void Start() {
		cam = GetComponent<Camera>();
		if (cam == null) {
			Debug.LogError("Failed to locate camera component on level builder camera object");
			enabled = false;
		}
		z = transform.position.z;
		zoomGoal = cam.orthographicSize;
	}

	void Update() {
		goalPos.x += Input.GetAxisRaw("Horizontal") * Time.deltaTime * moveSpeed;
		goalPos.y += Input.GetAxisRaw("Vertical") * Time.deltaTime * moveSpeed;

		zoomGoal -= Input.GetAxisRaw("Mouse ScrollWheel") * Time.deltaTime * Mathf.Pow(zoomSpeed, zoomRatio);
		zoomGoal = Mathf.Clamp(zoomGoal, zoomBounds.x, zoomBounds.y);

		cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, zoomGoal, ref zoomVel, smoothing);
		transform.position = Vector3.SmoothDamp(transform.position, new Vector3(goalPos.x, goalPos.y, z), ref velPos, smoothing);
	}

}