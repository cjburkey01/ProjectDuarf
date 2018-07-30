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
	public Button buttonDelete;
	public Button buttonNew;
	public Button buttonCancel;
	public LevelPack levelPack;

	LevelData selected;

	public GuiEditorLoadLevel() {
		INSTANCE = this;
	}

	public override string GetUniqueName() {
		return "GuiEditorLoadLevelFromPack";
	}

	public override void OnShow(GameGUI previousGui) {
		buttonLoad.onClick.AddListener(OnLoadClick);
		buttonDelete.onClick.AddListener(OnDeleteClick);
		buttonNew.onClick.AddListener(OnNewClick);
		buttonCancel.onClick.AddListener(OnCancelClick);

		Replace();
	}

	public override void OnHide(GameGUI nextGui) {
		Clear();
	}

	void Clear() {
		foreach (Transform t in container.transform) {
			Destroy(t.gameObject);
		}
	}

	public void Replace() {
		Clear();

		foreach (LevelData level in levelPack.GetLevels()) {
			GameObject obj = Instantiate(levelDisplayPrefab, container.transform, false);
			LevelDisplay ld = obj.GetComponent<LevelDisplay>();
			ld.levelLoader = this;
			ld.SetLevelName(level.Name);
		}

		Select(null);
	}

	public void OnLoadClick() {
		if (selected == null) {
			return;
		}
		levelEditorHandler.LoadLevel(selected);
		if (levelEditorHandler.LevelLoaded) {
			GUIHandler.HideGui();
		} else {
			Replace();
		}
	}

	public void OnDeleteClick() {
		
	}

	public void OnNewClick() {
		GuiEditorNewLevel.INSTANCE.levelPack = levelPack;
		GUIHandler.ShowGui(GuiEditorNewLevel.INSTANCE);
	}

	public void OnCancelClick() {
		buttonLoad.onClick.RemoveAllListeners();
		buttonCancel.onClick.RemoveAllListeners();

		GUIHandler.ShowGui(GuiEditorPackList.INSTANCE);
	}

	public void Select(LevelData level) {
		if (selected != null && selected.Equals(level)) {
			Select(null);   // Deselect
			return;
		}

		selected = level;

		buttonLoad.interactable = (selected != null);
		buttonDelete.interactable = (selected != null);
		
		foreach (Transform t in container.transform) {
			Image img = t.GetComponent<Image>();
			if (selected != null && t.GetComponent<LevelDisplay>().LevelName.Equals(selected.Name)) {	// TODO: AVOID USING NAMES TO COMPARE
				img.color = selectedColor;
			} else {
				img.color = defaultColor;
			}
		}
	}

	public void SetSelected(string name) {
		if (name == null) {
			Select(null);
		}
		foreach (LevelData level in levelPack.GetLevels()) {
			if (level.Name.Equals(name)) {
				Select(level);
				return;
			}
		}
		Select(null);
	}

}