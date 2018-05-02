using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelData {

	public string Name { private set; get; }

	readonly List<TileData> tiles = new List<TileData>();

	public LevelData() : this("") {
	}

	public LevelData(string name) {
		Name = name;
	}

	public bool AddTile(Vector2 position, float z, TileInfo tile) {
		TileData n;
		return AddTile(position, z, tile, out n);
	}

	public bool AddTile(Vector2 position, float z, TileInfo tile, out TileData tDone) {
		tDone = null;
		TileData at = GetTileAt(position);
		if (at != null) {
			if (at.Tile.Equals(tile)) {
				return false;
			}
			z = at.Z - 0.1f;
		}
		tDone = new TileData(position, z, tile);
		AddTile(tDone);
		return true;
	}

	public void AddTile(TileData tile) {
		if (!tiles.Contains(tile)) {
			tiles.Add(tile);
		}
	}

	public void InstantiateLevel(bool init, bool fullColliders, Transform parent) {
		foreach (TileData tile in tiles) {
			InstantiateTile(init, parent, tile);
			if (fullColliders) {
				BoxCollider2D c = tile.Instantiated.GetComponent<BoxCollider2D>();
				if (c == null) {
					c = tile.Instantiated.AddComponent<BoxCollider2D>();
				}
				c.offset = new Vector2(0.0f, 0.0f);
				c.size = new Vector2(1.0f, 1.0f);
			}
		}
		Debug.Log("Instantiated level");
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
		string data = Name + ";";
		foreach (TileData tile in tiles) {
			data += tile.Serialize() + ";";
		}
		return data;
	}

	public void Deserialize(bool init, bool fullColliders, Transform parent, string serialized) {
		OnDestroy(parent);
		string[] spl = serialized.Split(new char[] { '\n', ';' }, System.StringSplitOptions.RemoveEmptyEntries);
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
				AddTile(t);
			} else {
				Debug.LogError("Failed to deserialize tile: " + tile);
			}
		}
		Debug.Log("Loaded " + tiles.Count + " tiles");
		InstantiateLevel(init, fullColliders, parent);
	}

	public GameObject InstantiateTile(bool init, Transform parent, TileData data) {
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
		if (instance != null) {
			data.Tile.OnAdd(data);
		}
		return instance;
	}

}