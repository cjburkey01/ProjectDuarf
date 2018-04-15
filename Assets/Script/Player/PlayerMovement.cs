using UnityEngine;

// This script controlls the PlayerController
[RequireComponent(typeof(PlayerController))]
public class PlayerMovement : MonoBehaviour {
	
	public float jumpHeight = 2.5f;
	public float jumpHalfTime = 0.35f;
	public float moveSpeed = 6.0f;
	public float moveAccelerationTime = 0.1f;
	
	private Vector2 input;
	private Vector2 velocity;
	private PlayerController controller;
	private float plyGravity;
	private float jumpVelocity;

	private bool left;
	private bool right;
	private bool pleft;
	private bool pright;
	private float time;

	void Start() {
		controller = GetComponent<PlayerController>();
		if (controller == null) {
			Debug.LogError("PlayerController not found on player object, disabling");
			enabled = false;
		}

		plyGravity = -(2.0f * jumpHeight) / Mathf.Pow(jumpHalfTime, 2);
		jumpVelocity = Mathf.Abs(plyGravity) * jumpHalfTime;
	}

	void Update() {
		if (GameHandler.IsPaused) {
			return;
		}
		if (controller.IsCollidingDown || controller.IsCollidingUp) {
			velocity.y = 0.0f;
		}
		DoMovement();
		velocity.x = input.x;
		velocity.y += input.y;
		velocity.y += plyGravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
	}

	private void DoMovement() {
		left = Mathf.Approximately(Input.GetAxisRaw("Horizontal"), -1.0f);
		right = Mathf.Approximately(Input.GetAxisRaw("Horizontal"), 1.0f);
		if (left != pleft || right != pright) {
			time = 0.0f;
		}
		pleft = left;
		pright = right;
		time += Time.deltaTime;
		input.x = Mathf.Lerp(input.x, moveSpeed * Input.GetAxisRaw("Horizontal"), time / moveAccelerationTime);
		input.y = 0.0f;
		if (controller.IsCollidingDown && Input.GetButtonDown("Jump")) {
			input.y = jumpVelocity;
		}
	}

}