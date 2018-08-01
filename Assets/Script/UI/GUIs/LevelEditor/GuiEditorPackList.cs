using UnityEngine;
using UnityEngine.UI;

public class GuiEditorPackList : GameGUI {

	public static GuiEditorPackList INSTANCE { private set; get; }

	public GameObject container;
	public GameObject levelPackDisplayPrefab;
	public Color defaultColor = new Color(1.0f, 1.0f, 1.0f, 0.572f);
	public Color selectedColor = new Color(0.11f, 0.835f, 0.125f, 0.847f);
	public LevelEditorHandler levelEditorHandler;
	public Button buttonEdit;
	public Button buttonDelete;
	public Button buttonNew;
	public Button buttonCancel;
	
	LevelPack selected;

	public GuiEditorPackList() {
		INSTANCE = this;
	}

	public override string GetUniqueName() {
		return "GuiEditorPickPack";
	}

	public override void OnShow(GameGUI previousGui) {
		buttonEdit.onClick.AddListener(OnLoadClick);
		buttonCancel.onClick.AddListener(OnCancelClick);
		buttonNew.onClick.AddListener(OnNewClick);
		buttonDelete.onClick.AddListener(OnDeleteClick);

		Select(null);

		Replace();
	}

	public override void OnHide(GameGUI nextGui) {
		Clear();
	}

	// Clears the GUI
	void Clear() {
		foreach (Transform t in container.transform) {
			Destroy(t.gameObject);
		}
	}

	// Reloads the GUI
	public void Replace() {
		Clear();
		foreach (LevelPack levelPack in LevelIO.GetLevelPacks()) {
			GameObject obj = Instantiate(levelPackDisplayPrefab, container.transform, false);
			LevelPackPickDisplay levelPackPickDisplay = obj.GetComponent<LevelPackPickDisplay>();
			levelPackPickDisplay.parent = this;
			levelPackPickDisplay.levelPack = levelPack;
		}
		Select(null);
		buttonEdit.interactable = false;
		buttonDelete.interactable = false;
	}

	// Display the level picker screen
	public void OnLoadClick() {
		if (selected == null) {
			return;
		}

		GuiEditorLoadLevel.INSTANCE.levelPack = selected;
		GUIHandler.ShowGui(GuiEditorLoadLevel.INSTANCE);
	}

	public void OnCancelClick() {
		buttonEdit.onClick.RemoveAllListeners();
		buttonCancel.onClick.RemoveAllListeners();
		buttonNew.onClick.RemoveAllListeners();
		buttonDelete.onClick.RemoveAllListeners();

		GUIHandler.ShowGui(GuiEditorMenu.INSTANCE);
	}

	public void OnNewClick() {
		GUIHandler.ShowGui(GuiEditorNewPack.INSTANCE);
	}

	public void OnDeleteClick() {
		
	}

	public void Select(LevelPack pack) {
		if (selected != null && selected.Equals(pack)) {
			Select(null);   // Deselect
			return;
		}

		selected = pack;
		buttonEdit.interactable = (selected != null);
		buttonDelete.interactable = (selected != null);

		foreach (Transform t in container.transform) {
			Image img = t.GetComponent<Image>();
			if (t.GetComponent<LevelPackPickDisplay>().levelPack.Equals(selected)) {
				img.color = selectedColor;
			} else {
				img.color = defaultColor;
			}
		}
	}

}