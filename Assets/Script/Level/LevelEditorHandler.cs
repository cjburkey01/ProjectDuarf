using UnityEngine;

public class LevelEditorHandler : MonoBehaviour {

	public static LevelEditorHandler INSTANCE { private set; get; }

	public GridDisplay movingObject;
	public LevelData level = new LevelData();
	public float gridSize = 1.0f;
	public int gridWidth = 3;
	public Vector2 gridSizeBounds = new Vector2(0.25f, 4.0f);
	public Vector2 HoverPos { private set; get; }

	private bool snapToGrid;
	private Vector2 lastMouse;

	public LevelEditorHandler() {
		INSTANCE = this;
	}

	void Start() {
		movingObject.UpdateGrid(gridWidth, gridSize);
	}
	
	void Update() {
		if (Input.GetKeyDown(KeyCode.E) || (PickerUI.INSTANCE.Enabled && Input.GetButtonDown("Cancel"))) {
			PickerUI.INSTANCE.Toggle();
		}
		if (PickerUI.INSTANCE.Enabled) {
			return;
		}

		if (Input.GetKeyDown(KeyCode.Q)) {
			SetSelectedTile(null);
		}

		bool left = Input.GetButtonDown("Fire1");
		bool right = Input.GetButtonDown("Fire2");
		if (left || right) {
			if (left) {
				movingObject.Place(level, this);
			} else {
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				if (hit.collider != null) {
					Vector3 p = hit.collider.gameObject.transform.position;
					level.RemoveTile(new Vector2(p.x, p.y));
				}
			}
		}

		snapToGrid = !Input.GetKey(KeyCode.LeftAlt);
		if (Input.GetKey(KeyCode.LeftShift)) {
			if (Input.GetKeyDown(KeyCode.Equals)) {
				gridSize *= 2.0f;
			} else if (Input.GetKeyDown(KeyCode.Minus)) {
				gridSize *= 0.5f;
			} else {
				return;
			}
			gridSize = Mathf.Clamp(gridSize, gridSizeBounds.x, gridSizeBounds.y);
			movingObject.UpdateGrid(gridWidth, gridSize);
		}

		if (lastMouse.x != Input.mousePosition.x || lastMouse.y != Input.mousePosition.y) {
			movingObject.ResetErrorColor();
		}

		Vector3 at = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		lastMouse = Input.mousePosition;
		at.z = 0.0f;
		if (snapToGrid) {
			at = SnapToGrid(new Vector2(at.x, at.y));
		}
		movingObject.gameObject.transform.position = at;
		HoverPos = new Vector2(at.x, at.y);
	}

	private Vector2 SnapToGrid(Vector2 snap) {
		return new Vector2((Mathf.Floor(snap.x / gridSize) + 0.5f) * gridSize, (Mathf.Floor(snap.y / gridSize) + 0.5f) * gridSize);
	}

	public void SetSelectedTile(TileInfo tile) {
		movingObject.SetTile(tile);
	}

}