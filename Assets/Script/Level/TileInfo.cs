using System.Collections.Generic;
using UnityEngine;

public static class TileInitialization {

	public static TileInfo tilePlatformBasic;
	
	// Creates the types of tiles used in the game
	static TileInitialization() {
		tilePlatformBasic = Tiles.RegisterTile(new TileBasic("Tile/Platform/PlatformBasic"));
	}

}

public static class Tiles {

	private static Dictionary<string, TileInfo> tiles = new Dictionary<string, TileInfo>();

	public static TileInfo RegisterTile(TileInfo tile) {
		if (!tiles.ContainsKey(tile.GetResourceName()) && !tiles.ContainsValue(tile)) {
			tiles.Add(tile.GetResourceName(), tile);
		}
		return tile;
	}

}

public abstract class TileInfo {
	
	public abstract string GetResourceName();
	public abstract bool HasCustomInstantiation();
	public abstract GameObject DoCustomInstantiation(Vector2 pos, float z);

	public void OnDestroy(TileData self) {
	}

}

public class TileBasic : TileInfo {

	private readonly string resourceName;

	public TileBasic(string resourceName) {
		this.resourceName = resourceName;
	}

	public override string GetResourceName() {
		return resourceName;
	}

	public override bool HasCustomInstantiation() {
		return false;
	}

	public override GameObject DoCustomInstantiation(Vector2 pos, float z) {
		return default(GameObject);
	}

}