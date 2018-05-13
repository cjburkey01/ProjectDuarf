using UnityEngine;

public class LevelEditorHandler : MonoBehaviour {

	public static LevelEditorHandler INSTANCE { private set; get; }

	public GridDisplay movingObject;
	public LevelData LoadedLevel { private set; get; }
	public float gridSize = 1.0f;
	public int gridWidth = 3;
	public Vector2 gridSizeBounds = new Vector2(0.25f, 4.0f);
	public Vector2 HoverPos { private set; get; }
	public bool LevelLoaded { private set; get; }

	private bool snapToGrid;
	private Vector2 lastMouse;

	public LevelEditorHandler() {
		INSTANCE = this;
	}

	public bool NewLevel(string name) {
		if (LevelIO.GetLevelExists(name)) {
			Debug.LogError("Level already exists: " + name);
			return false;
		}
		LoadedLevel = new LevelData(name);
		SaveLevel();
		LevelLoaded = true;
		return true;
	}

	public void LoadLevel(string name) {
		LoadedLevel = LevelIO.LoadLevel(name);
		LevelLoaded = LoadedLevel != null;
		LoadedLevel.InstantiateLevel(false, true, transform);
		if (!LevelLoaded) {
			Debug.Log("Could not load level from level editor handler: " + name);
		}
	}

	public void SaveLevel() {
		LevelIO.SaveLevel(LoadedLevel, LoadedLevel.Name);
	}

	void Start() {
		movingObject.UpdateGrid(gridWidth, gridSize);
	}

	void Update() {
		// Generic GUI keys
		if (Input.GetButtonDown("Cancel")) {
			if (GUIHandler.IsShown()) {
				GUIHandler.HideGui();
			} else {
				GUIHandler.ShowGui(GuiEditorMenu.INSTANCE);
			}
		}

		// Ignore any other open gui than the picker
		if (GUIHandler.IsShown() && !GUIHandler.IsShown(GuiEditorPicker.INSTANCE)) {
			return;
		}

		// If there is no level open and the dialog to choose a level is not shown, show it
		if (!LevelLoaded && !GUIHandler.IsShown(GuiEditorMenu.INSTANCE)) {
			GUIHandler.ShowGui(GuiEditorMenu.INSTANCE);
		}

		// Picker dialog keys
		if (Input.GetKeyDown(KeyCode.E)) {
			if (GUIHandler.IsShown(GuiEditorPicker.INSTANCE)) {
				GUIHandler.HideGui();
			} else {
				GUIHandler.ShowGui(GuiEditorPicker.INSTANCE);
			}
		}

		// Editor keys
		if (Input.GetKeyDown(KeyCode.Q)) {
			SetSelectedTile(null);
		}

		// If a gui is open, ignore the editor keys and buttons in the background
		if (GUIHandler.IsShown()) {
			return;
		}

		// Mouse buttons
		bool left = Input.GetButtonDown("Fire1");
		bool right = Input.GetButtonDown("Fire2");

		// Shift+click to edit data for tile
		if (Input.GetKey(KeyCode.LeftShift) && left) {
			TileData data = GetTileHovering();
			if (data != null) {
				GuiEditTileData.INSTANCE.tile = data;
				GUIHandler.ShowGui(GuiEditTileData.INSTANCE);
				return;
			}
		}

		// Building buttons and keys
		if (left) {
			movingObject.Place(LoadedLevel, this);
		} else if (right) {
			LoadedLevel.RemoveTile(GetTileHovering());
		}

		// Grid controls
		snapToGrid = !Input.GetKey(KeyCode.LeftAlt);
		//if (Input.GetKey(KeyCode.LeftShift)) {
		//	if (Input.GetKeyDown(KeyCode.Equals)) {
		//		gridSize *= 2.0f;
		//	} else if (Input.GetKeyDown(KeyCode.Minus)) {
		//		gridSize *= 0.5f;
		//	} else {
		//		return;
		//	}
		//	gridSize = Mathf.Clamp(gridSize, gridSizeBounds.x, gridSizeBounds.y);
		//	movingObject.UpdateGrid(gridWidth, gridSize);
		//}

		// If the preview is red, make it green again when the mouse moves (user feedback for error in placing object)
		if (!Mathf.Approximately(lastMouse.x, Input.mousePosition.x) || !Mathf.Approximately(lastMouse.y, Input.mousePosition.y)) {
			movingObject.ResetErrorColor();
		}

		// Mouse grid and object placement preview
		Vector3 at = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		lastMouse = Input.mousePosition;
		at.z = 0.0f;
		if (snapToGrid) {
			at = SnapToGrid(new Vector2(at.x, at.y));
		}
		movingObject.gameObject.transform.position = at;
		HoverPos = new Vector2(at.x, at.y);
	}

	Vector2 SnapToGrid(Vector2 snap) {
		return new Vector2((Mathf.Floor(snap.x / gridSize) + 0.5f) * gridSize, (Mathf.Floor(snap.y / gridSize) + 0.5f) * gridSize);
	}

	public void SetSelectedTile(TileInfo tile) {
		movingObject.SetTile(LoadedLevel, tile);
	}

	public TileData GetTileHovering() {
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		if (hit.collider != null) {
			Vector3 p = hit.collider.gameObject.transform.position;
			return LoadedLevel.GetTileAt(new Vector2(p.x, p.y));
		}
		return null;
	}

}