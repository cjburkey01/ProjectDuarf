using UnityEngine;

public class GuiEditorPicker : GameGUI {

	public static GuiEditorPicker INSTANCE { private set; get; }

	public GameObject container;
	public GameObject prefabDisplay;

	public GuiEditorPicker() {
		INSTANCE = this;
	}

	public override string GetUniqueName() {
		return "GuiEditorPicker";
	}

	public override void OnShow(GameGUI previousGui) {
		foreach (TileInfo tile in Tiles.GetTiles()) {
			GameObject obj = Instantiate(prefabDisplay, container.transform, false);
			obj.name = "Tile " + tile.GetResourceName();
			TileDisplay td = obj.GetComponent<TileDisplay>();
			td.SetTile(tile);
		}
	}

	public override void OnHide(GameGUI nextGui) {
		foreach (Transform tile in container.transform) {
			Destroy(tile.gameObject);
		}
	}

}