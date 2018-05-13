using UnityEngine;
using UnityEngine.UI;

public class LevelPackDisplay : MonoBehaviour {

	Color defaultColor;
	public Color selectedColor = new Color(0.4784f, 1.0f, 0.4353f, 1.0f);
	public LevelPack LevelPack { private set; get; }

	Text text;

	void Start() {
		text = GetComponentInChildren<Text>();
		if (LevelPack != null) {
			SetText();
		}
		defaultColor = text.transform.parent.GetComponent<Image>().color;
	}

	public void SetLevel(LevelPack levelPack) {
		LevelPack = levelPack;
		SetText();
	}

	void SetText() {
		if (text != null) {
			text.text = LevelPack.name;
		}
	}

	public void OnClick() {
		GuiPickPack.INSTANCE.SetSelected(this, LevelPack);
	}

	public void SetSelected(bool selected) {
		text.transform.parent.GetComponent<Image>().color = (selected) ? selectedColor : defaultColor;
	}

}