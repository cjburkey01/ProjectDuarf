using UnityEngine;
using UnityEngine.UI;

public class GuiPickLevel : GameGUI {

	public static GuiPickLevel INSTANCE { private set; get; }

	public OnLevelSelected OnCall;
	public GameObject container;
	public GameObject levelLevelHorizDisplayPrefab;
	public GameObject levelLevelDisplayPrefab;
	public Button buttonSelect;
	public Button buttonCancel;

	bool init;
	public LevelData[] levels;
	LevelData selectedLevel;
	int shownId;

	public GuiPickLevel() {
		INSTANCE = this;
	}

	public override string GetUniqueName() {
		return "GuiPickLevel";
	}

	public override void OnShow(GameGUI previousGui) {
		if (!init) {
			init = true;
			buttonSelect.onClick.AddListener(OnSelectClick);
			buttonCancel.onClick.AddListener(OnCancelClick);
		}
		buttonSelect.interactable = false;
		UpdateLevelDisplay();
	}

	void UpdateLevelDisplay() {
		Deselect();
		Clear();
		int rows = GetRowsAndCols();
		for (int row = 0; row < rows; row++) {
			GameObject obj = Instantiate(levelLevelHorizDisplayPrefab, container.transform, false);
			for (int level = 0; level < rows; level++) {
				int i = row * rows + level;
				if (i >= levels.Length) {
					Instantiate(levelLevelDisplayPrefab, obj.transform, false).GetComponent<LevelDisplayTile>().SetLevel(null);
					continue;
				}
				Instantiate(levelLevelDisplayPrefab, obj.transform, false).GetComponent<LevelDisplayTile>().SetLevel(levels[i]);
			}
		}
	}

	int GetRowsAndCols() {
		int i = 1;
		while (i * i < levels.Length) {
			i++;
		}
		return i;
	}

	public void SetSelected(LevelDisplayTile display, LevelData level) {
		bool same = selectedLevel == level;
		Deselect();
		if (same) {
			return;
		}
		selectedLevel = level;
		display.SetSelected(true);
		buttonSelect.interactable = true;
	}

	public override void OnHide(GameGUI nextGui) {
		Clear();
	}

	void Deselect() {
		selectedLevel = null;
		buttonSelect.interactable = false;
		foreach (Transform t in container.transform) {
			foreach (Transform tr in t) {
				LevelDisplayTile display = tr.GetComponentInChildren<LevelDisplayTile>();
				display.SetSelected(false);
			}
		}
	}

	void Clear() {
		foreach (Transform t in container.transform) {
			Destroy(t.gameObject);
		}
	}

	void OnSelectClick() {
		if (buttonSelect.interactable) {
			Debug.Log("Load level: " + selectedLevel.Name);
			if (OnCall == null) {
				Debug.LogError("No level select action defined");
				return;
			}
			OnCall.Invoke(selectedLevel);
		}
	}

	void OnCancelClick() {
		GUIHandler.ShowGui(GuiPickPack.INSTANCE);
	}

}