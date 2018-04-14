using UnityEngine;

// This script controlls the PlayerController
[RequireComponent(typeof(PlayerController))]
public class PlayerMovement : MonoBehaviour {
	
	public float jumpHeight = 2.5f;
	public float jumpHalfTime = 0.25f;
	public float moveSpeed = 3.0f;
	
	private Vector2 input;
	private Vector2 velocity;
	private PlayerController controller;
	private float plyGravity;
	private float jumpVelocity;

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
		if (controller.IsCollidingDown || controller.IsCollidingUp) {
			velocity.y = 0.0f;
		}
		input = new Vector2();
		DoMovement();
		velocity.x = input.x;
		velocity.y += input.y;
		velocity.y += plyGravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
	}

	private void DoMovement() {
		input.x = moveSpeed * Input.GetAxisRaw("Horizontal");
		if (controller.IsCollidingDown && Input.GetAxisRaw("Jump") > 0) {
			input.y = jumpVelocity;
		}
	}

}