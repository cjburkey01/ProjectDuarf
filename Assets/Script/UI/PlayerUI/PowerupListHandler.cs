using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupListHandler : MonoBehaviour {

	public static PowerupListHandler INSTANCE { private set; get; }

	public GameObject activePowerupPrefab;

	readonly Dictionary<PowerUp, GameObject> instantiated = new Dictionary<PowerUp, GameObject>();
	Image img;
	int mins;
	int secs;

	public PowerupListHandler() {
		INSTANCE = this;
	}

	void Start() {
		img = GetComponent<Image>();
		if (img == null) {
			Debug.LogWarning("Could not locate image component on powerup list handler");
			enabled = false;
			return;
		}
		CheckImage();
	}

	void Update() {
		foreach (PowerUp powerup in instantiated.Keys) {
			Text txt = instantiated[powerup].GetComponentInChildren<Text>();
			if (txt != null) {
				// Display nicely formatted time ('m:SS' instead of 's' formatting)
				float time = powerup.length - PowerupHandler.GetTimeForPowerup(powerup);
				mins = Mathf.FloorToInt(time / 60.0f);
				secs = Mathf.FloorToInt(time - mins * 60.0f);
				txt.text = string.Format("{0:0}:{1:00}", mins, secs);
			}
		}
	}

	public void AddPowerup(PowerUp pup) {
		if (instantiated.ContainsKey(pup)) {
			return;
		}

		// Create the powerup display
		GameObject inst = Instantiate(activePowerupPrefab, transform, false);
		if (inst == null) {
			Debug.LogWarning("Unable to instantiate activePowerupPrefab");
			return;
		}

		// Add the icon, if the powerup reports one; if there is no sprite, make the box invisible (so there is no pure-white box in place of an icon)
		Image[] sprs = inst.GetComponentsInChildren<Image>();
		Image spr = (sprs.Length > 1) ? sprs[1] : null;
		if (sprs.Length <= 1) {
			Color c = spr.color;
			c.a = 0.0f;
			spr.color = c;
		}

		// Register it in our list so we can update it every frame with a new time value
		if (spr != null) {
			spr.sprite = pup.GetSprite();
		}
		instantiated.Add(pup, inst);
		CheckImage();
	}

	public void RemovePowerup(PowerUp pup) {
		if (!instantiated.ContainsKey(pup)) {
			return;
		}
		Destroy(instantiated[pup]);
		instantiated.Remove(pup);
		CheckImage();
	}

	void CheckImage() {
		// Will show or hide the powerup panel depending on if there are any powerups or not
		img.enabled = instantiated.Count > 0;
	}

}