using UnityEngine;

public class Player : PowerupHandler {

	public PlayerMovement PlayerMotor { private set; get; }

	Vector3 spawnPos;

	void Start() {
		PlayerMotor = GetComponent<PlayerMovement>();
		if (PlayerMotor == null) {
			Debug.LogError("Player lacking PlayerMovement component");
			enabled = false;
		}
		spawnPos = transform.position;
	}

	void Update() {
		HandlePowerupTick(this);
	}

	// Custom trigger enter method, called by custom 2D character controller
	void OnTriggeredEnter(Transform other) {
		if (other.gameObject == null) {
			return;
		}
		PowerUp pu = other.gameObject.GetComponent<PowerUp>();
		if (pu != null) {
			AddPowerup(this, pu);
			Destroy(pu.gameObject);
		}
	}

	public void Kill() {
		ClearPowerups(this);
		EventObject.EventSystem.TriggerEvent(new PlayerDeathEvent(this));
		PlayerMotor.SetVelocity(Vector2.zero);
		transform.position = spawnPos;
	}

}