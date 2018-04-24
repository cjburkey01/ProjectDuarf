using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDoubleJump : PowerUp {

	public float time = 15.0f;

	public override string GetUniqueName() {
		return "PowerUpDoubleJump";
	}

	public override void OnPickup(Player ply) {
		
	}

	public override float GetLength() {
		return time;
	}

	public override void OnExpire(Player ply) {

	}

}