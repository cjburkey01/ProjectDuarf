using UnityEngine;

public class PickerUI : MonoBehaviour {
	
	public static PickerUI INSTANCE { private set; get; }

	public GameObject pickerUi;
	public GameObject container;
	public GameObject prefabDisplay;
	public bool Enabled { private set; get; }

	public PickerUI() {
		INSTANCE = this;

		// Reset
		Enabled = false;
	}

	public void Toggle() {
		Enabled = !Enabled;
		if (Enabled) {
			Enable();
		} else {
			Clear();
		}
		pickerUi.SetActive(Enabled);
	}

	public void Enable() {
		TileInitialization.i ++;
		foreach (TileInfo tile in Tiles.GetTiles()) {
			GameObject obj = Instantiate(prefabDisplay, container.transform, false);
			obj.name = "Tile " + tile.GetResourceName();
			TileDisplay td = obj.GetComponent<TileDisplay>();
			td.SetTile(tile);
		}
	}

	public void Clear() {
		foreach (Transform tile in container.transform) {
			Destroy(tile.gameObject);
		}
	}

}