using UnityEngine;
using System.Text.RegularExpressions;

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