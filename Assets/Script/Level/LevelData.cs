using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class LevelData {
	
	public string Name { private set; get; }

	private readonly List<TileData> tiles = new List<TileData>();

	public LevelData(string name) {
		Name = name;
	}

	public bool AddTile(bool initialize, Transform parent, Vector2 position, float z, TileInfo tile) {
		TileData at = GetTileAt(position);
		if (at != null) {
			if (at.Tile.Equals(tile)) {
				return false;
			}
			z = at.Z - 0.1f;
		}
		TileData tileD = new TileData(position, z, tile);
		if (InstantiateTile(parent, tileD, initialize) != null) {
			tiles.Add(tileD);
			return true;
		}
		return false;
	}

	public bool AddTile(bool initialize, Transform parent, TileData tile) {
		if (InstantiateTile(parent, tile, initialize) != null) {
			tiles.Add(tile);
			return true;
		}
		return false;
	}

	public void OnUpdate() {
		foreach (TileData tile in tiles) {
			tile.Tile.OnUpdate(tile);
		}
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

	public string Serialize() {
		string data = Name + "\n";
		foreach (TileData tile in tiles) {
			data += tile.Serialize() + "\n";
		}
		return data;
	}

	public void Deserialize(Transform parent, string serialized) {
		tiles.Clear();
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
				if (!AddTile(true, parent, t)) {
					Debug.LogError("Failed to add tile: " + tile);
				}
			} else {
				Debug.LogError("Failed to deserialize tile: " + tile);
			}
		}
		Debug.Log("Loaded " + tiles.Count + " tiles");
	}

	public static GameObject InstantiateTile(Transform parent, TileData data) {
		return InstantiateTile(parent, data, true);
	}

	public static GameObject InstantiateTile(Transform parent, TileData data, bool initialize) {
		GameObject instance = null;
		if (data.Tile.HasCustomInstantiation()) {
			instance = data.Tile.DoCustomInstantiation(data.Position, data.Z);
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
		if (initialize) {
			data.Tile.OnCreate(data);
		}
		return instance;
	}

}

public class TileData {
	
	public Vector2 Position { private set; get; }
	public float Z { private set; get; }
	public TileInfo Tile { private set; get; }
	public GameObject Instantiated { private set; get; }
	public readonly DataHandler Data;

	public TileData(Vector2 position, float z, TileInfo tile) : this(position, z, tile, new DataHandler()) {
	}

	public TileData(Vector2 position, float z, TileInfo tile, DataHandler data) {
		Position = position;
		Z = z;
		Tile = tile;
		Data = data;
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

	public string Serialize() {
		return "\"" + Tile.GetResourceName() + "\"(" + Position.x + "," + Position.y + "," + Z + ")[" + Data.Serialize() + "]" + Tile.Serialize(this);
	}

	public override string ToString() {
		return Serialize();
	}

	public static TileData Deserialize(string serialized) {
		// Load the tile type
		string name = Regex.Match(serialized, "\\\".*\\\"").Value;
		if (name == null || name.Length <= 2) {
			Debug.LogError("Unable to determine name of tile from serialized string: " + serialized);
			return null;
		}
		name = name.Substring(1, name.Length - 2);
		TileInfo tile = Tiles.GetTile(name);
		if (tile == null) {
			Debug.LogError("Failed to load tile: " + name);
			return null;
		}

		// Load the tile position
		string pos = Regex.Match(serialized, "\\(.*\\)").Value;
		if (pos == null || pos.Length <= 2) {
			Debug.LogError("Unable to determine position of tile from serialized string: " + serialized);
			return null;
		}
		pos = pos.Substring(1, pos.Length - 2);
		string[] split = pos.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
		if (split.Length != 3) {
			Debug.LogError("Unable to determine position of tile from serialized position string: " + pos);
			return null;
		}
		float x;
		float y;
		float z;
		if (!float.TryParse(split[0], out x) || !float.TryParse(split[1], out y) || !float.TryParse(split[2], out z)) {
			Debug.LogError("Unable to determine position of tile from serialized position string: " + pos);
			return null;
		}
		
		// Load the data handler
		string data = Regex.Match(serialized, "\\[.*\\]").Value;
		if (data == null || data.Length <= 1) {
			Debug.LogError("Unable to determine data of tile from serialized string: " + serialized);
			return null;
		}
		data = data.Substring(1, data.Length - 2);
		DataHandler dh = new DataHandler();
		dh.Deserialize(data);

		// Create the tile and deserialize tile-specific data
		TileData tileData = new TileData(new Vector2(x, y), z, tile, dh);
		tile.Deserialize(serialized, tileData);
		return tileData;
	}

}

public class DataHandler {

	private readonly Dictionary<string, string> data = new Dictionary<string, string>();

	public void Set(string key, string value) {
		if (data.ContainsKey(key)) {
			data[key] = value;
		} else {
			data.Add(key, value);
		}
	}

	public int GetInt(string key) {
		string at = Get(key);
		int res;
		if (at == null || !int.TryParse(at, out res)) {
			return int.MinValue;
		}
		return res;
	}

	public long GetLong(string key) {
		string at = Get(key);
		long res;
		if (at == null || !long.TryParse(at, out res)) {
			return long.MinValue;
		}
		return res;
	}

	public float GetFloat(string key) {
		string at = Get(key);
		float res;
		if (at == null || !float.TryParse(at, out res)) {
			return float.MinValue;
		}
		return res;
	}

	public double GetDouble(string key) {
		string at = Get(key);
		double res;
		if (at == null || !double.TryParse(at, out res)) {
			return double.MinValue;
		}
		return res;
	}

	public string Get(string key) {
		string value;
		if (data.TryGetValue(key, out value)) {
			return value;
		}
		return null;
	}

	public string Serialize() {
		string ret = "";
		foreach (KeyValuePair<string, string> pair in data) {
			ret += pair.Key + "=" + pair.Value + ",";
		}
		return ret.Substring(0, (ret.Length > 0) ? ret.Length - 1 : 0);
	}

	public void Deserialize(string serialized) {
		data.Clear();
		string[] ser = serialized.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
		foreach (string data in ser) {
			string[] spl = data.Split(new char[] { '=' }, System.StringSplitOptions.RemoveEmptyEntries);
			if (spl.Length != 2) {
				continue;
			}
			Set(spl[0], spl[1]);
		}
	}

	public override string ToString() {
		return Serialize();
	}

	public override bool Equals(object obj) {
		return base.Equals(obj);
	}
	
	public override int GetHashCode() {
		return base.GetHashCode();
	}

}