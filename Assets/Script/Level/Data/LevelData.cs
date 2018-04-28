using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelData {

	public string Name { private set; get; }

	readonly List<TileData> tiles = new List<TileData>();

	public LevelData() {
		Name = "";
	}

	public LevelData(string name) {
		Name = name;
	}

	public bool AddTile(bool init, Transform parent, Vector2 position, float z, TileInfo tile) {
		TileData n;
		return AddTile(init, parent, position, z, tile, out n);
	}

	public bool AddTile(bool init, Transform parent, Vector2 position, float z, TileInfo tile, out TileData tDone) {
		tDone = null;
		TileData at = GetTileAt(position);
		if (at != null) {
			if (at.Tile.Equals(tile)) {
				return false;
			}
			z = at.Z - 0.1f;
		}
		TileData tileD = new TileData(position, z, tile);
		if (InstantiateTile(init, parent, tileD) != null) {
			tiles.Add(tileD);
			tDone = tileD;
			return true;
		}
		return false;
	}

	public bool AddTile(bool init, Transform parent, TileData tile) {
		if (InstantiateTile(init, parent, tile) != null) {
			tiles.Add(tile);
			tile.Tile.OnAdd(tile);
			return true;
		}
		return false;
	}

	public void OnInit() {
		foreach (TileData tile in tiles) {
			tile.Tile.OnCreate(tile);
		}
	}

	public void OnUpdate() {
		foreach (TileData tile in tiles) {
			tile.Tile.OnUpdate(tile);
		}
	}

	public void OnDestroy(Transform parent) {
		foreach (TileData tile in tiles) {
			tile.Tile.OnDestroy(tile);
		}
		tiles.Clear();
		foreach (Transform t in parent) {
			Object.Destroy(t.gameObject);
			t.name = "**__==DESTROYED==__**";
			if (t.GetComponent<PlayerController>() != null) {
				t.GetComponent<PlayerController>().DoDestroy();
			}
		}
		Debug.Log("Level unloaded");
	}

	public void RemoveTile(Vector2 position) {
		TileData at = GetTileAt(position);
		if (at == null) {
			return;
		}
		RemoveTile(at);
	}

	public void RemoveTile(TileData tile) {
		if (tile == null) {
			return;
		}
		tile.Destroy();
		tiles.Remove(tile);
	}

	public TileData GetTileAt(Vector2 position) {
		List<TileData> at = new List<TileData>();
		foreach (TileData data in tiles) {
			if (data.Position.Equals(position)) {
				at.Add(data);
			}
		}
		if (at.Count <= 0) {
			return null;
		}
		return at.OrderBy(d => d.Z).ToList()[0];
	}

	public string Serialize() {
		string data = Name + "\n";
		foreach (TileData tile in tiles) {
			data += tile.Serialize() + "\n";
		}
		return data;
	}

	// One of the ugliest methods I have ever written
	public void Deserialize(bool init, bool fullColliders, Transform parent, string serialized) {
		OnDestroy(parent);
		string[] spl = serialized.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
		bool loadingName = true;
		Debug.Log("Loading tiles: " + (spl.Length - 1));
		foreach (string tile in spl) {
			if (loadingName) {
				Name = tile;
				loadingName = false;
				continue;
			}
			TileData t = TileData.Deserialize(tile);
			if (t != null) {
				if (!AddTile(init, parent, t)) {
					Debug.LogError("Failed to add tile: " + tile);
				}
				if (fullColliders) {
					BoxCollider2D c = t.Instantiated.GetComponent<BoxCollider2D>();
					if (c == null) {
						c = t.Instantiated.AddComponent<BoxCollider2D>();
					}
					c.offset = new Vector2(0.0f, 0.0f);
					c.size = new Vector2(1.0f, 1.0f);
				}
			} else {
				Debug.LogError("Failed to deserialize tile: " + tile);
			}
		}
		Debug.Log("Loaded " + tiles.Count + " tiles");
	}

	public static GameObject InstantiateTile(bool init, Transform parent, TileData data) {
		GameObject instance = null;
		if (data.Tile.DoCustomInstantiation(init, data.Position, data.Z, data, out instance)) {
			if (ReferenceEquals(instance, null)) {
				Debug.LogError("Unable to instantiate tile: " + data.Tile.GetResourceName() + ", the custom instantiation returned null");
				return null;
			}
			instance.transform.parent = parent;
			data.SetInstantiated(instance);
		} else {
			GameObject prefab = Resources.Load<GameObject>(data.Tile.GetResourceName());
			if (ReferenceEquals(prefab, null)) {
				Debug.LogWarning("Unable to instantiate tile: " + data.Tile.GetResourceName() + ", the resource could not be found");
				return null;
			}
			instance = Object.Instantiate(prefab, new Vector3(data.Position.x, data.Position.y, data.Z), Quaternion.identity, parent);
			instance.name = data.Tile.GetResourceName() + " (" + data.Position.x + ", " + data.Position.y + ")";
			data.SetInstantiated(instance);
		}
		return instance;
	}

}