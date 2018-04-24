using System.Collections.Generic;
using UnityEngine;

public class PowerupHandler : MonoBehaviour {

	readonly Dictionary<PowerUp, float> active = new Dictionary<PowerUp, float>();

	protected void HandlePowerupTick(Player ply) {
		foreach (PowerUp powerup in new List<PowerUp>(active.Keys)) {
			if (active[powerup] >= powerup.GetLength()) {
				powerup.OnExpire(ply);
				active.Remove(powerup);
				continue;
			}
			powerup.OnTick(ply);
			active[powerup] += Time.deltaTime;
		}
	}

	protected void ClearPowerups(Player ply) {
		foreach (PowerUp powerup in new List<PowerUp>(active.Keys)) {
			powerup.OnExpire(ply);
			active.Remove(powerup);
		}
		HandlePowerupTick(ply);
	}

	protected void AddPowerup(Player ply, PowerUp powerUp) {
		if (powerUp.GetLength() > 0.0f) {
			active.Add(powerUp, 0.0f);
		}
		powerUp.OnPickup(ply);
		Debug.Log("Powerup enabled: " + powerUp.GetUniqueName());
	}

}