using UnityEngine;

// This script controlls the PlayerController
[RequireComponent(typeof(PlayerController))]
public class PlayerMovement : MonoBehaviour {

	public float gravity = 9.2f;

	private Vector2 velocity;
	private PlayerController controller;

	void Start() {
		controller = GetComponent<PlayerController>();
		if (controller == null) {
			Debug.LogError("PlayerController not found on player object, disabling");
			enabled = false;
		}
	}

	void Update() {
		velocity.y -= gravity * Time.deltaTime;

		controller.Move(velocity * Time.deltaTime);
	}

}