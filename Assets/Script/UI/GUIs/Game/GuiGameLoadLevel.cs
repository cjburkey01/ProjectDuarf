using UnityEngine;
using UnityEngine.UI;

public class GuiGameLoadLevel : GameGUI, ILevelSelector {

	public static GuiGameLoadLevel INSTANCE { private set; get; }

	public GameObject container;
	public GameObject levelDisplayPrefab;
	public Color defaultColor = new Color(1.0f, 1.0f, 1.0f, 0.572f);
	public Color selectedColor = new Color(0.11f, 0.835f, 0.125f, 0.847f);
	public GameHandler gameHandler;
	public Button buttonBuiltin;
	public Button buttonExternal;
	public Button buttonLoad;
	public Button buttonCancel;

	bool init;
	string selected;

	public GuiGameLoadLevel() {
		INSTANCE = this;
	}

	public override string GetUniqueName() {
		return "GuiGameLoadLevel";
	}

	public override void OnShow(GameGUI previousGui) {
		if (!init) {
			init = true;
			buttonBuiltin.onClick.AddListener(OnBuiltinClick);
			buttonExternal.onClick.AddListener(OnExternalClick);
			buttonLoad.onClick.AddListener(OnLoadClick);
			buttonCancel.onClick.AddListener(OnCancelClick);
		}
		Replace(true);
		Select(buttonBuiltin, buttonExternal);
	}

	public override void OnHide(GameGUI nextGui) {
		Clear();
	}

	private void Clear() {
		foreach (Transform t in container.transform) {
			Destroy(t.gameObject);
		}
	}

	public void Replace(bool builtin) {
		/*foreach (string level in LevelIO.GetLevels(builtin)) {
			GameObject obj = Instantiate(levelDisplayPrefab, container.transform, false);
			if (obj == null) {
				continue;
			}
			LevelDisplay ld = obj.GetComponent<LevelDisplay>();
			ld.levelLoader = this;
			ld.SetLevelName(level);
		}
		SetSelected(null);
		buttonLoad.interactable = false;
		if (builtin) {
			Select(buttonBuiltin, buttonExternal);
		} else {
			Select(buttonExternal, buttonBuiltin);
		}*/
	}

	private void Select(Button sl, Button other) {
		ColorBlock a = sl.colors;
		a.highlightedColor = selectedColor;
		a.normalColor = selectedColor;
		a.pressedColor = selectedColor;
		sl.colors = a;

		ColorBlock b = other.colors;
		b.highlightedColor = defaultColor;
		b.normalColor = defaultColor;
		b.pressedColor = defaultColor;
		other.colors = b;
	}

	public void OnBuiltinClick() {
		Clear();
		Replace(true);
	}

	public void OnExternalClick() {
		Clear();
		Replace(false);
	}

	public void OnLoadClick() {
		/*if (selected == null) {
			return;
		}
		gameHandler.LoadLevel(false, selected);
		if (!gameHandler.LevelLoaded) {
			Clear();
			Replace(true);
		}*/
	}

	public void OnCancelClick() {
		init = false;
		buttonLoad.onClick.RemoveAllListeners();
		buttonCancel.onClick.RemoveAllListeners();
		GUIHandler.ShowGui(GuiMainMenu.INSTANCE);
	}

	public void SetSelected(string name) {
		if (selected != null && selected.Equals(name)) {
			name = null;    // Deselect
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