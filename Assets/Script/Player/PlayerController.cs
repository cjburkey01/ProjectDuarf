using System.Collections.Generic;
using UnityEngine;

// This script is similar to CharacterController in 3D, it does no movement on its own.
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour {

    public float skinWidth = 0.01f;
	public LayerMask collidingLayers;
	public int raysForBottom = 5;
    public int raysForTop = 3;
    public int raysForRight = 5;
    public int raysForLeft = 5;

    private BoxCollider2D plyColl;
	private readonly List<Ray> rays = new List<Ray>();
	private Vector3 velocity;

    void Start() {
		plyColl = GetComponent<BoxCollider2D>();
		if (plyColl == null) {
			Debug.LogError("PlayerController object does not have a BoxCollider2D present, disabling");
			enabled = false;	// Disable this script, it's useless without the collider.
		}
		UpdateCollisionDetection();
    }

    void Update() {
		float minDown = float.MaxValue;
		float minUp = float.MaxValue;
		float minLeft = float.MaxValue;
		float minRight = float.MaxValue;
		foreach (Ray ray in rays) {
			// Basically determines how much of the this vector matches with velocity,
			// So the down ray will have no length if the character is moving up, and the right none
			// if the character is moving left (we don't need to detect collision if the player is moving
			// away from what they might hit)
			float rayLength = Vector3.Dot(velocity, ray.direction) + skinWidth;
			if (rayLength <= 0.0f) {
				continue;	// Skip this ray, it has a negative or no length
			}
			Vector3 s = ray.origin + transform.position + velocity;
			Debug.DrawLine(s, s + rayLength * ray.direction, Color.red);	// Velocity render
			RaycastHit hit;
			if (Physics.Raycast(s, ray.direction, out hit, rayLength, collidingLayers.value)) {
				// Determine in which direction the ray collided with something,
				// Then makes sure the velocity is set to stop our motion next frame when we collide
				// and where we collide
				if (ray.direction.Equals(Vector3.down)) {
					minDown = Mathf.Min(minDown, hit.distance - skinWidth);
					continue;
				} else if (ray.direction.Equals(Vector3.up)) {
					minUp = Mathf.Min(minUp, hit.distance - skinWidth);
					continue;
				} else if (ray.direction.Equals(Vector3.right)) {
					minRight = Mathf.Min(minRight, hit.distance - skinWidth);
					continue;
				} else if (ray.direction.Equals(Vector3.left)) {
					minLeft = Mathf.Min(minLeft, hit.distance - skinWidth);
					continue;
				}
				Debug.LogError("Failed to find vector: " + ray.direction);
			}
			minDown = Mathf.Min(minDown, rayLength);
			minUp = Mathf.Min(minUp, rayLength);
			minLeft = Mathf.Min(minLeft, rayLength);
			minRight = Mathf.Min(minRight, rayLength);
		}
		velocity.x = Mathf.Min(velocity.x, minRight, minLeft);
		velocity.y = Mathf.Min(velocity.y, minUp, minDown);

		transform.position += velocity;
		velocity = Vector3.zero;	// Velocity is handled by the player movement motor, not the character controller
	}

	// Reset the rays to use when calculating collisions (we only need to call this if the collider size is changed)
    public void UpdateCollisionDetection() {
        rays.Clear();

		float pDef = skinWidth + plyColl.bounds.min.x;
		float pos = pDef;
		float verticalSpacing = (plyColl.size.x - skinWidth * 2.0f);
		float horizontalSpacing = (plyColl.size.y - skinWidth * 2.0f);

		float bottomSpacing = (verticalSpacing / (raysForBottom - 1.0f));
		float topSpacing = (verticalSpacing / (raysForTop - 1.0f));
		float leftSpacing = (horizontalSpacing / (raysForLeft - 1.0f));
		float rightSpacing = (horizontalSpacing / (raysForRight - 1.0f));

		// Rays going upwards
		for (int i = 0; i < raysForTop; i++) {
			rays.Add(new Ray(new Vector3(pos, plyColl.bounds.max.y - skinWidth, 0.0f), Vector3.up));
			pos += topSpacing;
		}
		pos = pDef;	// Reset the temporary position storage for the next ray set. This just cuts down extra variables.

		// Rays going downards
        for (int i = 0; i < raysForBottom; i ++) {
			rays.Add(new Ray(new Vector3(pos, plyColl.bounds.min.y + skinWidth, 0.0f), Vector3.down));
			pos += bottomSpacing;
        }
		pos = pDef;

		// Rays going right
		for (int i = 0; i < raysForRight; i++) {
			rays.Add(new Ray(new Vector3(plyColl.bounds.max.x - skinWidth, pos, 0.0f), Vector3.right));
			pos += rightSpacing;
		}
		pos = pDef;

		// Rays going left
		for (int i = 0; i < raysForLeft; i++) {
			rays.Add(new Ray(new Vector3(plyColl.bounds.min.x + skinWidth, pos, 0.0f), Vector3.left));
			pos += leftSpacing;
		}
    }

	// Tells the controller to move the player by the specified amount
	// This value must be pre-multiplied by Time.deltaTime, it is not
	// done by the controller
	// This method will allow the controller to move while abiding by
	// Colliders in the way, so we don't phase through objects
	public void Move(Vector3 velocity) {
		this.velocity += velocity;
	}

}