using System.Collections.Generic;
using UnityEngine;

public static class TileInitialization {

	public static TileInfo tilePlatformGrassLeft;
	public static TileInfo tilePlatformGrassMiddle;
	public static TileInfo tilePlatformGrassRight;

	public static TileInfo tileSolidGrassLeft;
	public static TileInfo tileSolidGrassMiddle;
	public static TileInfo tileSolidGrassRight;

	public static int i = 0;

	// Creates the types of tiles used in the game
	static TileInitialization() {
		tilePlatformGrassLeft = Tiles.RegisterTile(new TileBasic("Icon/Platform/PlatformGrassLeft", "Tile/Platform/PlatformGrassLeft"));
		tilePlatformGrassMiddle = Tiles.RegisterTile(new TileBasic("Icon/Platform/PlatformGrassMiddle", "Tile/Platform/PlatformGrassMiddle"));
		tilePlatformGrassRight = Tiles.RegisterTile(new TileBasic("Icon/Platform/PlatformGrassRight", "Tile/Platform/PlatformGrassRight"));

		tileSolidGrassLeft = Tiles.RegisterTile(new TileBasic("Icon/Solid/SolidGrassLeft", "Tile/Solid/SolidGrassLeft"));
		tileSolidGrassMiddle = Tiles.RegisterTile(new TileBasic("Icon/Solid/SolidGrassMiddle", "Tile/Solid/SolidGrassMiddle"));
		tileSolidGrassRight = Tiles.RegisterTile(new TileBasic("Icon/Solid/SolidGrassRight", "Tile/Solid/SolidGrassRight"));
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

	public static TileInfo[] GetTiles() {
		return new List<TileInfo>(tiles.Values).ToArray();
	}

}

public abstract class TileInfo {
	
	public abstract string GetResourceName();
	public abstract string GetIconResourceName();
	public abstract bool HasCustomInstantiation();
	public abstract GameObject DoCustomInstantiation(Vector2 pos, float z);

	public void OnDestroy(TileData self) {
	}

	public override bool Equals(object obj) {
		return base.Equals(obj);
	}

	public override int GetHashCode() {
		return base.GetHashCode() + GetResourceName().GetHashCode() + GetIconResourceName().GetHashCode();
	}

	public override string ToString() {
		return base.ToString();
	}
}

public class TileBasic : TileInfo {

	private readonly string resourceName;
	private readonly string iconName;

	public TileBasic(string iconName, string resourceName) {
		this.iconName = iconName;
		this.resourceName = resourceName;
	}

	public override string GetResourceName() {
		return resourceName;
	}

	public override string GetIconResourceName() {
		return iconName;
	}

	public override bool HasCustomInstantiation() {
		return false;
	}

	public override GameObject DoCustomInstantiation(Vector2 pos, float z) {
		return default(GameObject);
	}

}