using UnityEngine;

public class TilePlaceholder : TileInfo {

	readonly string prefabPath;
	readonly string name;
	readonly string iconPath;
	readonly bool limitOne;
	readonly System.Guid uuid;

	public TilePlaceholder(string prefabPath, string name, string iconPath, bool limitOne) : this(prefabPath, name, iconPath, limitOne, new TileDataCustomizationWrapper[0]) {
	}

	public TilePlaceholder(string prefabPath, string name, string iconPath, bool limitOne, TileDataCustomizationWrapper[] data) : base(data) {
		this.prefabPath = prefabPath;
		this.name = name;
		this.iconPath = iconPath;
		this.limitOne = limitOne;
		if (limitOne) {
			uuid = System.Guid.NewGuid();
		}
	}

	public override string GetResourceName() {
		return name;
	}

	public override string GetIconResourceName() {
		return iconPath;
	}

	public override bool DoCustomInstantiation(bool init, Vector2 pos, float z, TileData tile, out GameObject obj) {
		obj = null;
		if (!init) {
			// In the editor
			obj = new GameObject("Placeholder " + name);
			SpriteRenderer spren = obj.AddComponent<SpriteRenderer>();
			BoxCollider2D coll = obj.AddComponent<BoxCollider2D>();
			coll.offset = new Vector2(0.0f, 0.0f);
			coll.size = new Vector2(1.0f, 1.0f);
			spren.sprite = Resources.Load<Sprite>(iconPath);
			if (spren.sprite == null) {
				Debug.LogError("Sprite not found: " + iconPath);
			}
			obj.transform.position = new Vector3(pos.x, pos.y, z);
			obj.layer = LayerMask.NameToLayer("Solid");
			return true;
		}
		// Playing the level
		GameObject at = Resources.Load<GameObject>(prefabPath);
		if (at == null) {
			Debug.LogError("Failed to load prefab: " + at);
			return true;
		}
		if (limitOne && GameObject.Find(uuid.ToString()) != null) {
			Debug.LogWarning("Attempted to spawn multiple instances of single-only placeholder: " + name);
			return true;
		}
		obj = Object.Instantiate(at, new Vector3(pos.x, pos.y, z), Quaternion.identity);
		if (limitOne) {
			obj.name = uuid.ToString();
		}
		return true;
	}

}