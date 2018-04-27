using System.Collections.Generic;
using UnityEngine;

public static class PowerupHandler {

	static readonly Dictionary<PowerUp, float> active = new Dictionary<PowerUp, float>();

	public static void HandlePowerupTick() {
		foreach (PowerUp powerup in new List<PowerUp>(active.Keys)) {
			if (active[powerup] >= powerup.length) {
				RemovePowerup(powerup);
				continue;
			}
			powerup.OnTick(Player.INSTANCE);
			active[powerup] += Time.deltaTime;
		}
	}

	public static void ClearPowerups() {
		foreach (PowerUp powerup in new List<PowerUp>(active.Keys)) {
			RemovePowerup(powerup);
		}
	}

	public static void AddPowerup(PowerUp powerUp) {
		if (powerUp.length > 0.0f && !powerUp.GetIsReusable()) {
			active.Add(powerUp, 0.0f);
			PowerupListHandler.INSTANCE.AddPowerup(powerUp);
		}
		powerUp.OnPickup(Player.INSTANCE);
		Debug.Log("Powerup enabled: " + powerUp.GetUniqueName());
	}

	public static void RemovePowerup(PowerUp powerUp) {
		if (powerUp.GetIsReusable()) {
			return;
		}
		powerUp.OnExpire(Player.INSTANCE);
		active.Remove(powerUp);
		PowerupListHandler.INSTANCE.RemovePowerup(powerUp);
		Debug.Log("Powerup disabled: " + powerUp.GetUniqueName());
	}

	public static float GetTimeForPowerup(PowerUp powerUp) {
		if (active.ContainsKey(powerUp)) {
			return active[powerUp];
		}
		return -1.0f;
	}

}