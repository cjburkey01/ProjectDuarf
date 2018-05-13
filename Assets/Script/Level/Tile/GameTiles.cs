using UnityEngine;

[RegisterEventHandlers]
public static class GameTiles {

	public static TileInfo tilePlaceholderPlayer;

	public static TileInfo tilePowerupDoubleJump;
	public static TileInfo tilePowerupSpeed;

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

	public static bool HasInit { private set; get; }

	static void RegisterEvents() {
		EventObject.EventSystem.AddListener<EventGameInit>(Init);
	}

	// Creates the types of tiles used in the game
	static void Init<T>(T e) where T : EventGameInit {
		tilePlaceholderPlayer = Tiles.RegisterTile(new TilePlaceholder("Prefab/Game/Player", "PlaceholderPlayer", "Tile/Placeholder/Player", true));

		tilePowerupDoubleJump = Tiles.RegisterTile(new TilePowerup("Prefab/Powerup/DoubleJump", "PUDoubleJump", "Tile/Placeholder/PowerUp/DoubleJump"));
		tilePowerupSpeed = Tiles.RegisterTile(new TilePowerup("Prefab/Powerup/Speed", "PUSpeed", "Tile/Placeholder/PowerUp/Speed"));

		tileSolidGrassLeft = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.0f), new Vector2(1.0f, 1.0f), "Tile/Solid/SolidGrassLeft", true));
		tileSolidGrassMiddle = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.0f), new Vector2(1.0f, 1.0f), "Tile/Solid/SolidGrassMiddle", true));
		tileSolidGrassRight = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.0f), new Vector2(1.0f, 1.0f), "Tile/Solid/SolidGrassRight", true));
		tileSolidGrassCliffLeft = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.0f), new Vector2(1.0f, 1.0f), "Tile/Solid/SolidGrassCliffLeft", true));
		tileSolidGrassCliffLeftAlt = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.0f), new Vector2(1.0f, 1.0f), "Tile/Solid/SolidGrassCliffLeftAlt", true));
		tileSolidGrassCliffRight = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.0f), new Vector2(1.0f, 1.0f), "Tile/Solid/SolidGrassCliffRight", true));
		tileSolidGrassCliffRightAlt = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.0f), new Vector2(1.0f, 1.0f), "Tile/Solid/SolidGrassCliffRightAlt", true));
		tileSolidDirt = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.0f), new Vector2(1.0f, 1.0f), "Tile/Solid/SolidDirt", true));
		tileSolidDirtCenter = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.0f), new Vector2(1.0f, 1.0f), "Tile/Solid/SolidDirtCenter", true));

		tilePlatformGrass = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.49f), new Vector2(1.0f, 0.01f), "Tile/Platform/PlatformGrass"));
		tilePlatformGrassLeft = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.49f), new Vector2(1.0f, 0.01f), "Tile/Platform/PlatformGrassLeft"));
		tilePlatformGrassMiddle = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.49f), new Vector2(1.0f, 0.01f), "Tile/Platform/PlatformGrassMiddle"));
		tilePlatformGrassRight = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.49f), new Vector2(1.0f, 0.01f), "Tile/Platform/PlatformGrassRight"));

		tilePlatformSand = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.49f), new Vector2(1.0f, 0.01f), "Tile/Platform/PlatformSand"));
		tilePlatformSandLeft = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.49f), new Vector2(1.0f, 0.01f), "Tile/Platform/PlatformSandLeft"));
		tilePlatformSandMiddle = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.49f), new Vector2(1.0f, 0.01f), "Tile/Platform/PlatformSandMiddle"));
		tilePlatformSandRight = Tiles.RegisterTile(new TileBasic(new Vector2(0.0f, 0.49f), new Vector2(1.0f, 0.01f), "Tile/Platform/PlatformSandRight"));

		Debug.Log("Loaded game tiles: " + Tiles.GetTiles().Length);
	}

}