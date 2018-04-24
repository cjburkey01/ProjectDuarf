using UnityEngine;
using System.Collections;

public class TileBasic : TileInfo {

	readonly string iconPath;
	readonly Vector2 colliderOffset;
	readonly Vector2 colliderSize;
	readonly bool solid;

	public TileBasic(Vector2 colliderOffset, Vector2 colliderSize, string iconPath) : this(colliderOffset, colliderSize, iconPath, false) {
	}

	public TileBasic(Vector2 colliderOffset, Vector2 colliderSize, string iconPath, bool solid) {
		this.iconPath = iconPath;
		this.colliderOffset = colliderOffset;
		this.colliderSize = colliderSize;
		this.solid = solid;
	}

	public override string GetResourceName() {
		return iconPath;
	}

	public override string GetIconResourceName() {
		return iconPath;
	}

	public override bool DoCustomInstantiation(bool init, Vector2 pos, float z, out GameObject obj) {
		obj = new GameObject("Tile " + iconPath);
		SpriteRenderer spren = obj.AddComponent<SpriteRenderer>();
		BoxCollider2D coll = obj.AddComponent<BoxCollider2D>();
		coll.offset = colliderOffset;
		coll.size = colliderSize;
		spren.sprite = Resources.Load<Sprite>(iconPath);
		if (spren.sprite == null) {
			Debug.LogError("Sprite not found: " + iconPath);
		}
		obj.transform.position = new Vector3(pos.x, pos.y, z);
		obj.layer = LayerMask.NameToLayer((solid) ? "Solid" : "Platform");
		return true;
	}

}