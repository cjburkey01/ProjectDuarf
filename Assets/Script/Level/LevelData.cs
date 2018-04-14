using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelData {
	
	private List<TileData> tiles = new List<TileData>();

	public bool AddTile(Transform parent, Vector2 position, float z, TileInfo tile) {
		TileData at = GetTileAt(position);
		if (at != null) {
			if (at.Tile.Equals(tile)) {
				return false;
			}
			z = at.Z - 0.1f;
		}
		TileData tileD = new TileData(position, z, tile);
		if (InstantiateTile(parent, tileD)) {
			tiles.Add(tileD);
		}
		return true;
	}

	public void RemoveTile(Vector2 position) {
		TileData at = GetTileAt(position);
		if (at == null) {
			return;
		}
		at.Destroy();
		tiles.Remove(at);
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

	public static GameObject InstantiateTile(Transform parent, TileData data) {
		GameObject instance = null;
		if (data.Tile.HasCustomInstantiation()) {
			instance = data.Tile.DoCustomInstantiation(data.Position, data.Z);
			if (ReferenceEquals(instance, null)) {
				Debug.LogError("Unable to instantiate tile: " + data.Tile.GetResourceName() + ", the custom instantiation returned null");
				return null;
			}
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

public class TileData {

	public Vector2 Position { private set; get; }
	public float Z { private set; get; }
	public TileInfo Tile { private set; get; }
	public GameObject Instantiated { private set; get; }

	public TileData(Vector2 position, float z, TileInfo tile) {
		Position = position;
		Z = z;
		Tile = tile;
	}

	public void SetInstantiated(GameObject self) {
		Instantiated = self;
	}

	public void Destroy() {
		Tile.OnDestroy(this);
		if (!ReferenceEquals(Instantiated, null)) {
			Object.Destroy(Instantiated);
		}
	}

}