using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;

// This script is similar to CharacterController in 3D, it does no movement on its own.
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour {

	public float skinWidth = 0.01f;
	public float maxSlopeAngle = 45.0f;
	public int raysForBottom = 5;
	public int raysForTop = 3;
	public int raysForRight = 5;
	public int raysForLeft = 5;
	public LayerMask collidingLayersTop;
	public LayerMask collidingLayersBottom;
	public LayerMask collidingLayersLeft;
	public LayerMask collidingLayersRight;

	public bool IsCollidingUp { private set; get; }
	public bool IsCollidingDown { private set; get; }
	public bool IsCollidingLeft { private set; get; }
	public bool IsCollidingRight { private set; get; }
	public bool IsClimbing { private set; get; }
	public bool Destroyed { private set; get; }
	public float SlopeAngle { private set; get; }
	public float SlopeAnglePrev { private set; get; }

	BoxCollider2D plyColl;
	readonly List<CollisionRay> rays = new List<CollisionRay>();
	Vector2 velocity;

	readonly List<string> triggerWithLast = new List<string>();
	readonly List<string> triggerWith = new List<string>();

	void Start() {
		plyColl = GetComponent<BoxCollider2D>();
		if (plyColl == null) {
			Debug.LogError("PlayerController object does not have a BoxCollider2D present, disabling");
			enabled = false;    // Disable this script, it's useless without the collider.
		}
		UpdateCollisionDetection();
	}

	void Update() {
		// Reset variables
		IsCollidingDown = false;
		IsCollidingUp = false;
		IsCollidingLeft = false;
		IsCollidingRight = false;
		IsClimbing = false;
		SlopeAnglePrev = SlopeAngle;
		SlopeAngle = 0.0f;

		/* Begin ugliest code I've ever written */

		// Reset triggers for next frame
		triggerWithLast.Clear();
		triggerWithLast.AddRange(triggerWith);
		triggerWith.Clear();

		// Determine how many colliders we intersect that are triggers
		foreach (BoxCollider2D obj in FindObjectsOfType<BoxCollider2D>()) {
			if (!obj.gameObject.activeInHierarchy || !obj.enabled || !obj.bounds.Intersects(plyColl.bounds) || obj.gameObject.layer != 11) {
				continue;   // Doesn't intersect or isn't a trigger, move on.
			}
			foreach (MonoBehaviour mb in GetComponents<MonoBehaviour>()) {
				try {
					if (!triggerWithLast.Contains(obj.gameObject.name) && !triggerWith.Contains(obj.gameObject.name)) {
						MethodInfo m1 = mb.GetType().GetMethod("OnTriggeredEnter", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(Transform) }, null);
						if (m1 != null) {
							m1.Invoke(mb, new object[] { obj.gameObject.gameObject.transform });
						}
					}
					MethodInfo m2 = mb.GetType().GetMethod("OnTriggered", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(Transform) }, null);
					if (m2 != null) {
						m2.Invoke(mb, new object[] { obj.gameObject.gameObject.transform });
					}
				} catch (Exception e) {
					Debug.LogWarning(e.Message);
				}
			}
			triggerWith.Add(obj.gameObject.gameObject.name);
		}

		// Determine the triggers that were called last frame but not this one, and call the according methods
		foreach (string obj in triggerWithLast) {
			if (triggerWith.Contains(obj)) {
				//Debug.Log(obj);
				continue;
			}
			MonoBehaviour[] mbs = GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour mb in mbs) {
				try {
					MethodInfo m2 = mb.GetType().GetMethod("OnTriggeredExit", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(Transform) }, null);
					if (m2 != null) {
						GameObject o = GameObject.Find(obj);
						m2.Invoke(mb, new object[] { ((o == null) ? default(Transform) : o.transform) });
					}
				} catch (Exception e) {
					Debug.LogError("Error: " + e.Message);
				}
			}
		}

		/* End ugliest code I've ever written */

		foreach (CollisionRay ray in rays) {
			// Basically determines how much of the this vector matches with velocity,
			// So the down ray will have no length if the character is moving up, and the right none
			// if the character is moving left (we don't need to detect collision if the player is moving
			// away from what they might hit)
			//
			// Note: This would be better to do with one statement, but I don't vector well
			float rayLength = -1.0f;
			if (ray.direction.Equals(Vector2.down)) {
				rayLength = -velocity.y;
			} else if (ray.direction.Equals(Vector2.up)) {
				rayLength = velocity.y;
			} else if (ray.direction.Equals(Vector2.left)) {
				rayLength = -velocity.x;
			} else if (ray.direction.Equals(Vector2.right)) {
				rayLength = velocity.x;
			}
			rayLength = Mathf.Max(rayLength, 0.0f) + skinWidth;
			if (rayLength <= skinWidth) {
				continue;
			}

			Vector2 s = ray.start + new Vector2(transform.position.x, transform.position.y);
			Debug.DrawLine(s + velocity, s + velocity + rayLength * ray.direction, Color.red);  // Velocity render
			RaycastHit2D[] hits = new RaycastHit2D[1];

			if (Physics2D.Raycast(s, ray.direction, ray.filter, hits, rayLength) > 0) {
				// If is a trigger
				if (hits[0].collider.gameObject.layer == 11) {
					continue;
				}

				// Slope handling
				float angle = Vector2.Angle(hits[0].normal, Vector2.up);
				if (ray.bottomHoriz && angle <= maxSlopeAngle) {
					float dist = 0.0f;
					if (!Mathf.Approximately(angle, SlopeAnglePrev)) {
						dist = (hits[0].distance - skinWidth) * Mathf.Sign(velocity.x);
						velocity.x -= dist;
					}
					ClimbSlope(angle);
					velocity.x += dist;
				}

				if (!IsClimbing || angle > maxSlopeAngle) {
					// Left and right
					if (ray.direction.Equals(Vector2.left)) {
						velocity.x = Mathf.Max(velocity.x, -hits[0].distance + skinWidth);
						IsCollidingLeft = true;
					}
					if (ray.direction.Equals(Vector2.right)) {
						velocity.x = Mathf.Min(velocity.x, hits[0].distance - skinWidth);
						IsCollidingRight = true;
					}

					// Slope helper
					if (IsClimbing) {
						velocity.y = Mathf.Tan(SlopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x); // If an object is on the slope a blocking the player's path
					}

					// Up and down
					if (ray.direction.Equals(Vector2.down)) {
						velocity.y = Mathf.Max(velocity.y, -hits[0].distance + skinWidth);
						IsCollidingDown = true;
					}
					if (ray.direction.Equals(Vector2.up)) {
						velocity.y = Mathf.Min(velocity.y, hits[0].distance - skinWidth);
						IsCollidingUp = true;
					}
				}

				// Another slope helper
				if (IsClimbing) {
					velocity.x = velocity.y / Mathf.Tan(SlopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x); // If an object is above the player on a slope
				}
			}
		}

		transform.position += new Vector3(velocity.x, velocity.y, 0.0f);
		velocity = Vector2.zero;    // Velocity is handled by the player movement motor, not the character controller
	}

	public void DoDestroy() {
		Destroyed = true;
	}

	void ClimbSlope(float angle) {
		float dist = Mathf.Abs(velocity.x);
		float climbVelY = Mathf.Sin(angle * Mathf.Deg2Rad) * dist;
		if (velocity.y <= climbVelY) {
			velocity.x = Mathf.Cos(angle * Mathf.Deg2Rad) * dist * Mathf.Sign(velocity.x);
			velocity.y = climbVelY;
			IsCollidingDown = true;
			IsClimbing = true;
			SlopeAngle = angle;
		}
	}

	// Reset the rays to use when calculating collisions (we only need to call this if the collider size is changed)
	public void UpdateCollisionDetection() {
		rays.Clear();

		float pDefX = skinWidth + GetTranslatedMin().y;
		float pDefY = skinWidth + GetTranslatedMin().x;
		float pos = pDefY;
		float verticalSpacing = (plyColl.bounds.size.x - skinWidth * 2.0f);
		float horizontalSpacing = (plyColl.bounds.size.y - skinWidth * 2.0f);

		float bottomSpacing = (verticalSpacing / (raysForBottom - 1.0f));
		float topSpacing = (verticalSpacing / (raysForTop - 1.0f));
		float leftSpacing = (horizontalSpacing / (raysForLeft - 1.0f));
		float rightSpacing = (horizontalSpacing / (raysForRight - 1.0f));

		// Rays going upwards
		for (int i = 0; i < raysForTop; i++) {
			rays.Add(new CollisionRay(collidingLayersTop, new Vector2(pos, GetTranslatedMax().y - skinWidth), Vector2.up));
			pos += topSpacing;
		}
		pos = pDefY;    // Reset the temporary position storage for the next ray set. This just cuts down extra variables.

		// Rays going downards
		for (int i = 0; i < raysForBottom; i++) {
			rays.Add(new CollisionRay(collidingLayersBottom, new Vector2(pos, GetTranslatedMin().y + skinWidth), Vector2.down));
			pos += bottomSpacing;
		}
		pos = pDefX;

		// Rays going right
		for (int i = 0; i < raysForRight; i++) {
			rays.Add(new CollisionRay(collidingLayersRight, new Vector2(GetTranslatedMax().x - skinWidth, pos), Vector2.right, i == 0));
			pos += rightSpacing;
		}
		pos = pDefX;

		// Rays going left
		for (int i = 0; i < raysForLeft; i++) {
			rays.Add(new CollisionRay(collidingLayersLeft, new Vector2(GetTranslatedMin().x + skinWidth, pos), Vector2.left, i == 0));
			pos += leftSpacing;
		}
	}

	// Tells the controller to move the player by the specified amount
	// This value must be pre-multiplied by Time.deltaTime, it is not
	// done by the controller
	// This method will allow the controller to move while abiding by
	// Colliders in the way, so we don't phase through objects
	public void Move(Vector2 velocity) {
		this.velocity += velocity;
	}

	Vector2 GetTranslatedMin() {
		return plyColl.bounds.min - transform.position;
	}

	Vector2 GetTranslatedMax() {
		return plyColl.bounds.max - transform.position;
	}

}

struct CollisionRay {

	public readonly Vector2 start;
	public readonly Vector2 direction;
	public readonly ContactFilter2D filter;
	public readonly bool bottomHoriz;

	public CollisionRay(LayerMask layers, Vector2 start, Vector2 direction) : this(layers, start, direction, false) {
	}

	public CollisionRay(LayerMask layers, Vector2 start, Vector2 direction, bool bottomHoriz) {
		this.start = start;
		this.direction = direction;
		filter = new ContactFilter2D() {
			useLayerMask = true,
			layerMask = layers
		};
		this.bottomHoriz = bottomHoriz;
	}

}