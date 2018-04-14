using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TileDisplay : MonoBehaviour {
	
	private TileInfo tile;
	private Image image;

	void Start() {
		image = GetComponent<Image>();
		if (image == null) {
			Debug.LogError("Tile preview has no image component");
			enabled = false;
		}
		if (tile != null) {
			SetIcon();
		}
	}

	public void SetTile(TileInfo tile) {
		this.tile = tile;
		if (tile == null || image == null) {
			return;
		}
		SetIcon();
	}
	
	public void OnClick() {
		if (tile != null) {
			PickerUI.INSTANCE.Toggle();
			LevelEditorHandler.INSTANCE.SetSelectedTile(tile);
		}
	}

	private void SetIcon() {
		Sprite spr = Resources.Load<Sprite>(tile.GetIconResourceName());
		if (spr == null) {
			Debug.LogError("Tile icon not found for: " + tile.GetIconResourceName());
			return;
		}
		image.sprite = spr;
	}

}