using UnityEngine;
using UnityEngine.UI;

public class LevelDisplayTile : MonoBehaviour {

	public Color defaultColor;
	public Color selectedColor = new Color(0.4784f, 1.0f, 0.4353f, 1.0f);
	public LevelData LevelData { private set; get; }

	Text text;

	void Start() {
		text = GetComponentInChildren<Text>();
		defaultColor = text.transform.parent.GetComponent<Image>().color;
		if (LevelData != null) {
			SetText();
		} else {
			Destroy(text.transform.parent.gameObject);
			Debug.Log("Destroyed text");
		}
	}

	public void SetLevel(LevelData levelData) {
		LevelData = levelData;
		SetText();
	}

	void SetText() {
		if (text != null && LevelData != null) {
			text.text = LevelData.Name;
		}
	}

	public void OnClick() {
		if (LevelData != null) {
			GuiPickLevel.INSTANCE.SetSelected(this, LevelData);
		}
	}

	public void SetSelected(bool selected) {
		if (LevelData != null) {
			//Debug.Log("Set " + LevelData.Name + " to" + ((selected) ? "" : " not") + " selected");
			text.transform.parent.GetComponent<Image>().color = (selected) ? selectedColor : defaultColor;
		}
	}

}