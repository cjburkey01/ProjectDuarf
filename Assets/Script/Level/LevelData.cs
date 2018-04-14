using System.Collections.Generic;
using UnityEngine;

public class LevelData {
	
	private Dictionary<Vector2, TileData> tiles = new Dictionary<Vector2, TileData>();

	public void AddTile(Transform parent, Vector2 position, float z, TileInfo tile) {
		TileData tileD = new TileData(position, z, tile);
		if (InstantiateTile(parent, tileD)) {
			tiles.Add(position, tileD);
		}
	}

	public void RemoveTile(Vector2 position) {
		if (tiles.ContainsKey(position)) {
			tiles[position].Destroy();
			tiles.Remove(position);
		}
	}

	private bool InstantiateTile(Transform parent, TileData data) {
		if (data.Tile.HasCustomInstantiation()) {
			GameObject instantiated = data.Tile.DoCustomInstantiation(data.Position, data.Z);
			if (ReferenceEquals(instantiated, null)) {
				Debug.LogError("Unable to instantiate tile: " + data.Tile.GetResourceName() + ", the custom instantiation returned null");
				return false;
			}
			data.SetInstantiated(instantiated);
		} else {
			GameObject prefab = Resources.Load<GameObject>(data.Tile.GetResourceName());
			if (ReferenceEquals(prefab, null)) {
				Debug.LogWarning("Unable to instantiate tile: " + data.Tile.GetResourceName() + ", the resource could not be found");
				return false;
			}
			GameObject instantiated = Object.Instantiate(prefab, new Vector3(data.Position.x, data.Position.y, data.Z), Quaternion.identity, parent);
			instantiated.name = data.Tile.GetResourceName() + " (" + data.Position.x + ", " + data.Position.y + ")";
			data.SetInstantiated(instantiated);
		}
		return true;
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