using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void OnLevelSelected(LevelData data);

public class GuiPickPack : GameGUI {

	public static readonly int showPerPage = 3;

	public static GuiPickPack INSTANCE { private set; get; }

	public OnLevelSelected OnCall;
	public GameObject container;
	public GameObject levelDisplayPrefab;
	public Button buttonLeft;
	public Button buttonRight;
	public Button buttonSelect;
	public Button buttonCancel;

	bool init;
	LevelPack selectedPack;
	List<LevelPack> packs = new List<LevelPack>();
	int shownId;

	public GuiPickPack() {
		INSTANCE = this;
	}

	public override string GetUniqueName() {
		return "GuiPickPack";
	}

	public override void OnShow(GameGUI previousGui) {
		if (!init) {
			init = true;
			buttonLeft.onClick.AddListener(OnLeftClick);
			buttonRight.onClick.AddListener(OnRightClick);
			buttonSelect.onClick.AddListener(OnSelectClick);
			buttonCancel.onClick.AddListener(OnCancelClick);
		}
		buttonSelect.interactable = false;
		ReloadPacks();
		UpdatePackDisplay();
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			OnLeftClick();
		} else if (Input.GetKeyDown(KeyCode.RightArrow)) {
			OnRightClick();
		} else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
			OnSelectClick();
		}
	}

	public void ReloadPacks() {
		shownId = 0;
		packs.Clear();
		packs.AddRange(LevelIO.GetLevelPacks());
		if (packs.Count < 1) {
			Debug.LogWarning("No level packs found!");
		} else {
			Debug.Log("Loaded " + packs.Count + " level packs");
		}
		selectedPack = null;
	}

	void UpdatePackDisplay() {
		buttonLeft.interactable = shownId > 0;
		buttonRight.interactable = shownId < packs.Count - showPerPage;
		Deselect();
		Clear();
		for (int i = shownId; (i < shownId + showPerPage) && (i < packs.Count); i++) {
			GameObject inst = Instantiate(levelDisplayPrefab, container.transform, false);
			LevelPackDisplay display = inst.GetComponentInChildren<LevelPackDisplay>();
			display.SetLevel(packs[i]);
		}
	}

	public void SetSelected(LevelPackDisplay display, LevelPack pack) {
		bool same = selectedPack == pack;
		Deselect();
		if (same) {
			return;
		}
		selectedPack = pack;
		display.SetSelected(true);
		buttonSelect.interactable = true;
	}

	public override void OnHide(GameGUI nextGui) {
		Clear();
	}

	void Deselect() {
		selectedPack = null;
		buttonSelect.interactable = false;
		foreach (Transform t in container.transform) {
			LevelPackDisplay display = t.GetComponentInChildren<LevelPackDisplay>();
			display.SetSelected(false);
		}
	}

	void Clear() {
		foreach (Transform t in container.transform) {
			Destroy(t.gameObject);
		}
	}

	void OnLeftClick() {
		if (buttonLeft.interactable) {
			shownId -= showPerPage;
			shownId = Mathf.Max(shownId, 0);
			UpdatePackDisplay();
		}
	}

	void OnRightClick() {
		if (buttonRight.interactable) {
			shownId += showPerPage;
			shownId = Mathf.Min(shownId, packs.Count - showPerPage);
			UpdatePackDisplay();
		}
	}

	void OnSelectClick() {
		if (buttonSelect.interactable) {
			Debug.Log("load level pack: " + selectedPack.name);
			GuiPickLevel.INSTANCE.levels = selectedPack.GetLevels();
			GuiPickLevel.INSTANCE.OnCall = OnCall;
			GUIHandler.ShowGui(GuiPickLevel.INSTANCE);
		}
	}

	void OnCancelClick() {
		GUIHandler.ShowGui(GuiMainMenu.INSTANCE);
	}

}