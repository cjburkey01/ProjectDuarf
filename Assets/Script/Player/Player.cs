using UnityEngine;

public class Player : MonoBehaviour {

	public static Player INSTANCE { private set; get; }

	public PlayerMovement PlayerMotor { private set; get; }

	Vector3 spawnPos;

	public Player() {
		INSTANCE = this;
	}

	void Start() {
		PlayerMotor = GetComponent<PlayerMovement>();
		if (PlayerMotor == null) {
			Debug.LogError("Player lacking PlayerMovement component");
			enabled = false;
		}
		spawnPos = transform.position;
	}

	void Update() {
		PowerupHandler.HandlePowerupTick();
	}

	// Custom trigger enter method, called by custom 2D character controller
	void OnTriggeredEnter(Transform other) {
		if (other.gameObject == null) {
			return;
		}
		PowerUp pu = other.gameObject.GetComponent<PowerUp>();
		if (pu != null) {
			PowerupHandler.AddPowerup(pu);
			Destroy(pu.gameObject);
		}
	}

	public void Kill() {
		PowerupHandler.ClearPowerups();
		EventObject.EventSystem.TriggerEvent(new PlayerDeathEvent(this));
		PlayerMotor.SetVelocity(Vector2.zero);
		transform.position = spawnPos;
	}

}