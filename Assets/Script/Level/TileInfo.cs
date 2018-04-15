using System.Collections.Generic;
using UnityEngine;

public static class TileInitialization {
	
	public static TileInfo tileSolidGrassLeft;
	public static TileInfo tileSolidGrassMiddle;
	public static TileInfo tileSolidGrassRight;
	public static TileInfo tileSolidGrassCliffLeft;
	public static TileInfo tileSolidGrassCliffLeftAlt;
	public static TileInfo tileSolidGrassCliffRight;
	public static TileInfo tileSolidGrassCliffRightAlt;
	public static TileInfo tileSolidDirt;
	public static TileInfo tileSolidDirtCenter;

	public static TileInfo tilePlatformGrass;
	public static TileInfo tilePlatformGrassLeft;
	public static TileInfo tilePlatformGrassMiddle;
	public static TileInfo tilePlatformGrassRight;

	public static TileInfo tilePlatformSand;
	public static TileInfo tilePlatformSandLeft;
	public static TileInfo tilePlatformSandMiddle;
	public static TileInfo tilePlatformSandRight;

	// Creates the types of tiles used in the game
	public static void Init() {
		tileSolidGrassLeft = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.0f), new Vector2(1.0f, 1.0f), "Tile/Solid/SolidGrassLeft", true));
		tileSolidGrassMiddle = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.0f), new Vector2(1.0f, 1.0f), "Tile/Solid/SolidGrassMiddle", true));
		tileSolidGrassRight = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.0f), new Vector2(1.0f, 1.0f), "Tile/Solid/SolidGrassRight", true));
		tileSolidGrassCliffLeft = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.0f), new Vector2(1.0f, 1.0f), "Tile/Solid/SolidGrassCliffLeft", true));
		tileSolidGrassCliffLeftAlt = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.0f), new Vector2(1.0f, 1.0f), "Tile/Solid/SolidGrassCliffLeftAlt", true));
		tileSolidGrassCliffRight = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.0f), new Vector2(1.0f, 1.0f), "Tile/Solid/SolidGrassCliffRight", true));
		tileSolidGrassCliffRightAlt = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.0f), new Vector2(1.0f, 1.0f), "Tile/Solid/SolidGrassCliffRightAlt", true));
		tileSolidDirt = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.0f), new Vector2(1.0f, 1.0f), "Tile/Solid/SolidDirt", true));
		tileSolidDirtCenter = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.0f), new Vector2(1.0f, 1.0f), "Tile/Solid/SolidDirtCenter", true));

		tilePlatformGrass = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.21f), new Vector2(1.0f, 0.57f), "Tile/Platform/PlatformGrass"));
		tilePlatformGrassLeft = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.21f), new Vector2(1.0f, 0.57f), "Tile/Platform/PlatformGrassLeft"));
		tilePlatformGrassMiddle = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.21f), new Vector2(1.0f, 0.57f), "Tile/Platform/PlatformGrassMiddle"));
		tilePlatformGrassRight = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.21f), new Vector2(1.0f, 0.57f), "Tile/Platform/PlatformGrassRight"));

		tilePlatformSand = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.21f), new Vector2(1.0f, 0.57f), "Tile/Platform/PlatformSand"));
		tilePlatformSandLeft = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.21f), new Vector2(1.0f, 0.57f), "Tile/Platform/PlatformSandLeft"));
		tilePlatformSandMiddle = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.21f), new Vector2(1.0f, 0.57f), "Tile/Platform/PlatformSandMiddle"));
		tilePlatformSandRight = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.21f), new Vector2(1.0f, 0.57f), "Tile/Platform/PlatformSandRight"));

		Debug.Log("Loaded game tiles: " + Tiles.GetTiles().Length);
	}

}

public static class Tiles {

	private static List<TileInfo> tiles = new List<TileInfo>();

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

public abstract class TileInfo {
	
	public abstract string GetResourceName();
	public abstract string GetIconResourceName();

	public virtual bool HasCustomInstantiation() {
		return false;
	}

	public virtual GameObject DoCustomInstantiation(Vector2 pos, float z) {
		Debug.LogError("Failed to instantiate custom object: the instantiation method was not overriden");
		return new GameObject("FailedToInstantiate");
	}

	public virtual void OnDestroy(TileData self) {
	}

	public virtual void OnUpdate(TileData self) {
	}

	public virtual void OnCreate(TileData self) {
	}
	
	/// <summary>
	///		This is extra data appended to the serialized tile data
	/// </summary>
	public virtual string Serialize(TileData self) {
		return "";
	}

	public virtual void Deserialize(string serialized, TileData data) {
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
	
	private readonly string iconPath;
	private readonly Vector2 colliderOffset;
	private readonly Vector2 colliderSize;
	private readonly bool solid;

	public TileBasic(Vector2 colliderOffset, Vector2 colliderSize, string iconPath) : this(colliderOffset, colliderSize, iconPath, false) {
	}

	public TileBasic(Vector2 colliderOffset, Vector2 colliderSize, string iconPath, bool solid) {
		this.iconPath = iconPath;
		this.colliderOffset = colliderOffset;
		this.colliderSize = colliderSize;
		this.solid = solid;
	}

	public override string GetResourceName() {
		return iconPath;
	}

	public override string GetIconResourceName() {
		return iconPath;
	}

	public override bool HasCustomInstantiation() {
		return true;
	}

	public override GameObject DoCustomInstantiation(Vector2 pos, float z) {
		GameObject obj = new GameObject("Tile " + iconPath);
		SpriteRenderer spren = obj.AddComponent<SpriteRenderer>();
		BoxCollider2D coll = obj.AddComponent<BoxCollider2D>();
		coll.offset = colliderOffset;
		coll.size = colliderSize;
		spren.sprite = Resources.Load<Sprite>(iconPath);
		if (spren.sprite == null) {
			Debug.LogError("Sprite not found: " + iconPath);
		}
		obj.transform.position = new Vector3(pos.x, pos.y, z);
		obj.layer = LayerMask.NameToLayer((solid) ? "Solid" : "Platform");
		return obj;
	}

}