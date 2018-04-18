using UnityEngine;
using UnityEngine.UI;

public class ButtonFinderInjector : MonoBehaviour {

	public static ButtonFinderInjector INSTANCE { private set; get; }

	public Color defaultColor;
	public Color hoverColor;
	public Color activeColor;
	public Color disabledColor;
	public float multiplier;
	public float transitionTime;

	int i;
	int j;
	int k;

	void Start() {
		InjectColors();
	}

	public void InjectColors() {
		i = 0;
		j = 0;
		k = 0;
		ColorBlock cbl = new ColorBlock {
			normalColor = defaultColor,
			highlightedColor = hoverColor,
			pressedColor = activeColor,
			disabledColor = disabledColor,
			colorMultiplier = multiplier,
			fadeDuration = transitionTime
		};
		foreach (Button btn in Resources.FindObjectsOfTypeAll<Button>()) {
			if (btn.gameObject.scene.name == null) {
				k ++;
				continue;
			}
			if (btn.gameObject.GetComponent<IgnoreColorInject>() != null) {
				j ++;
				continue;
			}
			bool wasActive = btn.gameObject.activeSelf;
			if (!wasActive) {
				btn.gameObject.SetActive(true);
			}
			btn.colors = cbl;
			i ++;
			btn.gameObject.SetActive(wasActive);
		}
		Debug.Log("Colored " + i + " buttons with default colors, ignored " + j + " buttons and " + k + " prefabs");
	}

}