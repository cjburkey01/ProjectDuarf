using System.Collections.Generic;
using UnityEngine;

// This script is similar to CharacterController in 3D, it does no movement on its own.
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour {

    public float skinWidth = 0.01f;
    public int raysForBottom = 5;
    public int raysForTop = 3;
    public int raysForRight = 5;
    public int raysForLeft = 5;

    private BoxCollider2D plyColl;
	private readonly List<Ray> rays = new List<Ray>();

    void Start() {
		plyColl = GetComponent<BoxCollider2D>();
		if (plyColl == null) {
			Debug.LogError("PlayerController object does not have a BoxCollider2D present, disabling");
			enabled = false;	// Disable this script, it's useless without the collider.
		}
		UpdateCollisionDetection();
    }

    void Update() {
		foreach (Ray ray in rays) {
			Debug.DrawRay(ray.origin + transform.position, ray.direction, Color.red);
		}
    }

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
			rays.Add(new Ray(new Vector3(pos, plyColl.bounds.max.y - skinWidth, 0.0f), new Vector3(0.0f, 1.0f, 0.0f)));
			pos += topSpacing;
		}
		pos = pDef;	// Reset the temporary position storage for the next ray set. This just cuts down extra variables.

		// Rays going downards
        for (int i = 0; i < raysForBottom; i ++) {
			rays.Add(new Ray(new Vector3(pos, plyColl.bounds.min.y + skinWidth, 0.0f), new Vector3(0.0f, -1.0f, 0.0f)));
			pos += bottomSpacing;
        }
		pos = pDef;

		// Rays going right
		for (int i = 0; i < raysForRight; i++) {
			rays.Add(new Ray(new Vector3(plyColl.bounds.max.x - skinWidth, pos, 0.0f), new Vector3(1.0f, 0.0f, 0.0f)));
			pos += rightSpacing;
		}
		pos = pDef;

		// Rays going left
		for (int i = 0; i < raysForLeft; i++) {
			rays.Add(new Ray(new Vector3(plyColl.bounds.min.x + skinWidth, pos, 0.0f), new Vector3(-1.0f, 0.0f, 0.0f)));
			pos += leftSpacing;
		}
    }

	public void Move(Vector3 velocity) {
		transform.position += velocity;
	}

}