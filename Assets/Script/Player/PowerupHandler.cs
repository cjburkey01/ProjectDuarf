using System.Collections.Generic;
using UnityEngine;

public class PowerupHandler : MonoBehaviour {

	readonly Dictionary<PowerUp, float> active = new Dictionary<PowerUp, float>();
	readonly List<PowerUp> toRemove = new List<PowerUp>();

	protected void HandlePowerupTick() {
		foreach (KeyValuePair<PowerUp, float> powerup in active) {
			if (powerup.Value >= powerup.Key.GetLength()) {
				powerup.Key.OnExpire();
				toRemove.Add(powerup.Key);
				continue;
			}
			powerup.Key.OnTick();
			active[powerup.Key] = powerup.Value + Time.deltaTime;
		}
		while (toRemove.Count > 0) {
			active.Remove(toRemove[0]);
			toRemove.RemoveAt(0);
		}
	}

	protected void AddPowerup(PowerUp powerUp) {
		active.Add(powerUp, 0.0f);
		powerUp.OnPickup();
	}

}