using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GuiEditTileData : GameGUI {

	public static GuiEditTileData INSTANCE { private set; get; }

	public GameObject optionTextPrefab;
	public GameObject optionChoosePrefab;
	public GameObject optionBoolPrefab;

	public bool ignoreReset;
	public TileData tile;

	bool init = true;
	readonly Dictionary<TileDataCustomizationWrapper, object> unsavedChanges = new Dictionary<TileDataCustomizationWrapper, object>();

	public Button accept;
	public Button cancel;

	public GuiEditTileData() {
		INSTANCE = this;
	}

	public override string GetUniqueName() {
		return "GuiEditTileData";
	}

	public override void OnShow(GameGUI previousGui) {
		if (init) {
			init = false;
			accept.onClick.AddListener(OnAccept);
			cancel.onClick.AddListener(OnCancel);
			ignoreReset = false;
		}
		if (!ignoreReset) {
			Reset();
			ignoreReset = true;
		}
	}

	public void OnAccept() {
		foreach (TileDataCustomizationWrapper tileData in unsavedChanges.Keys) {
			tile.OnDataUpdate(tileData, unsavedChanges[tileData]);
		}
		ignoreReset = false;
		GUIHandler.HideGui();
	}

	public void OnCancel() {
		ignoreReset = false;
		GUIHandler.HideGui();
	}

	void Reset() {
		foreach (Transform trans in transform) {
			if (!trans.gameObject.tag.Equals("GuiPersistRefresh")) {
				Destroy(trans.gameObject);
			}
		}
		unsavedChanges.Clear();
		foreach (TileDataCustomizationWrapper tileData in tile.Tile.GetCustomData()) {
			string displayName = tileData.GetDisplayName();
			ISerializableData val = tile.GetData(tileData);
			unsavedChanges.Add(tileData, val.Get());
			GameObject inst;
			if (tileData is TileDataBool) {
				inst = AddBoolOption(tileData, displayName, val as SerializableBool);
			} else if (tileData is TileDataChoose) {
				inst = AddChooseOption(displayName, ((TileDataString) val).GetTypedValidValues(), val as SerializableString);
			} else {
				inst = AddTextOption(tileData, displayName, tileData.GetPrompt(), val as SerializableData);
			}
			if (inst != null) {
				inst.transform.SetSiblingIndex(inst.transform.GetSiblingIndex() - 1);
			}
		}
	}

	GameObject AddBoolOption(TileDataCustomizationWrapper tileData, string displayName, SerializableBool value) {
		GameObject inst = Instantiate(optionBoolPrefab, transform, false);
		if (inst == null) {
			Debug.LogWarning("Failed to instantiate boolean option prefab");
			return null;
		}
		Text text = inst.GetComponentInChildren<Text>();
		if (text != null) {
			text.text = displayName;
		}
		Toggle toggle = inst.GetComponentInChildren<Toggle>();
		if (toggle != null) {
			toggle.isOn = (bool) value.Get();
			toggle.onValueChanged.AddListener((b) => OnChangedValue(tileData, b.ToString()));
		}
		return inst;
	}

	GameObject AddChooseOption(string displayName, SerializableString[] options, SerializableString value) {
		Debug.LogWarning("NOT IMPLEMENTED: CHOOSE OPTION");
		//GameObject inst = Instantiate(optionChoosePrefab, transform, false);
		//if (inst == null) {
		//	Debug.LogWarning("Failed to instantiate boolean option prefab");
		//	return;
		//}
		return null;
	}

	GameObject AddTextOption(TileDataCustomizationWrapper tileData, string displayName, string prompt, SerializableData value) {
		GameObject inst = Instantiate(optionTextPrefab, transform, false);
		if (inst == null) {
			Debug.LogWarning("Failed to instantiate boolean option prefab");
			return null;
		}
		Text[] text = inst.GetComponentsInChildren<Text>();
		if (text.Length > 0 && text[0] != null) {
			text[0].text = displayName;
		}
		if (text.Length > 1 && text[1] != null) {
			text[1].text = prompt;
		}
		InputField inputField = inst.GetComponentInChildren<InputField>();
		if (inputField != null) {
			if (value.Get() != null) {
				inputField.text = value.Serialize();
			}
			inputField.onValueChanged.AddListener((txt) => OnChangedValue(tileData, txt));
		}
		return inst;
	}

	public void OnChangedValue(TileDataCustomizationWrapper tileData, string serialized) {
		if (unsavedChanges.ContainsKey(tileData)) {
			unsavedChanges[tileData] = tileData.GetDeserialized(serialized);
		}
	}

}