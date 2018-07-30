using UnityEngine;
using UnityEngine.UI;

public class LevelPackPickDisplay : MonoBehaviour {

	public GuiEditorPackList parent;
	public LevelPack levelPack;

	Text text;

	void Start() {
		text = GetComponentInChildren<Text>();
		if (levelPack != null) {
			text.text = levelPack.name;
		}
	}

	public void OnClick() {
		parent.Select(levelPack);
	}

}