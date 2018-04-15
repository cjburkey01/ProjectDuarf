using UnityEngine;
using UnityEngine.UI;

public class GuiEditorLoadLevel : GameGUI, ILevelSelector {

	public static GuiEditorLoadLevel INSTANCE { private set; get; }

	public GameObject container;
	public GameObject levelDisplayPrefab;
	public Color defaultColor = new Color(1.0f, 1.0f, 1.0f, 0.572f);
	public Color selectedColor = new Color(0.11f, 0.835f, 0.125f, 0.847f);
	public LevelEditorHandler levelEditorHandler;
	public Button buttonLoad;
	public Button buttonCancel;
	
	private bool init = false;
	private string selected = null;

	public GuiEditorLoadLevel() {
		INSTANCE = this;
	}

	public override string GetUniqueName() {
		return "GuiEditorLoadLevel";
	}

	public override void OnShow(GameGUI previousGui) {
		if (!init) {
			init = true;
			buttonLoad.onClick.AddListener(OnLoadClick);
			buttonCancel.onClick.AddListener(OnCancelClick);
		}
		Replace();
	}

	public override void OnHide(GameGUI nextGui) {
		Clear();
	}

	private void Clear() {
		foreach (Transform t in container.transform) {
			Destroy(t.gameObject);
		}
	}

	private void Replace() {
		foreach (string level in LevelIO.GetLevels(false)) {
			GameObject obj = Instantiate(levelDisplayPrefab, container.transform, false);
			LevelDisplay ld = obj.GetComponent<LevelDisplay>();
			ld.levelLoader = this;
			ld.SetLevelName(level.Substring(0, level.Length - 4));
		}
		SetSelected(null);
		buttonLoad.interactable = false;
	}

	public void OnLoadClick() {
		if (selected == null) {
			return;
		}
		levelEditorHandler.LoadLevel(false, selected);
		if (levelEditorHandler.LevelLoaded) {
			GUIHandler.HideGui();
		} else {
			Clear();
			Replace();
		}
	}

	public void OnCancelClick() {
		init = false;
		buttonLoad.onClick.RemoveAllListeners();
		buttonCancel.onClick.RemoveAllListeners();
		GUIHandler.ShowGui(GuiEditorMenu.INSTANCE);
	}

	public void SetSelected(string name) {
		if (selected != null && selected.Equals(name)) {
			name = null;	// Deselect
		}
		selected = name;
		buttonLoad.interactable = name != null;
		foreach (Transform t in container.transform) {
			Image img = t.GetComponent<Image>();
			if (t.GetComponent<LevelDisplay>().LevelName.Equals(name)) {
				img.color = selectedColor;
			} else {
				img.color = defaultColor;
			}
		}
	}

}