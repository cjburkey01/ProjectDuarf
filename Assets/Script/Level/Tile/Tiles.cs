using UnityEngine;
using System.Collections.Generic;

public static class Tiles {

	static readonly List<TileInfo> tiles = new List<TileInfo>();

	public static TileInfo RegisterTile(TileInfo tile) {
		if (GetTile(tile.GetResourceName()) != null) {
			Debug.LogError("Tile already exists: " + tile.GetResourceName());
			return null;
		}
		tiles.Add(tile);
		return tile;
	}

	public static TileInfo[] GetTiles() {
		return tiles.ToArray();
	}

	public static TileInfo GetTile(string name) {
		foreach (TileInfo tile in tiles) {
			if (tile.GetResourceName().Equals(name)) {
				return tile;
			}
		}
		return null;
	}

}