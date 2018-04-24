using System.Collections.Generic;
using UnityEngine;

public class PowerupHandler : MonoBehaviour {

	readonly Dictionary<PowerUp, float> active = new Dictionary<PowerUp, float>();
	readonly List<PowerUp> toRemove = new List<PowerUp>();

	protected void HandlePowerupTick(Player ply) {
		foreach (KeyValuePair<PowerUp, float> powerup in active) {
			if (powerup.Value >= powerup.Key.GetLength()) {
				powerup.Key.OnExpire(ply);
				toRemove.Add(powerup.Key);
				continue;
			}
			powerup.Key.OnTick(ply);
			active[powerup.Key] = powerup.Value + Time.deltaTime;
		}
		while (toRemove.Count > 0) {
			active.Remove(toRemove[0]);
			toRemove.RemoveAt(0);
		}
	}

	protected void AddPowerup(Player ply, PowerUp powerUp) {
		active.Add(powerUp, 0.0f);
		powerUp.OnPickup(ply);
	}

}