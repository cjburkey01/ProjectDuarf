public abstract class PlayerEvent : PDEvent {

	public readonly Player player;

	protected PlayerEvent(Player player) {
		this.player = player;
	}

}

public class PlayerDeathEvent : PlayerEvent {

	public PlayerDeathEvent(Player player) : base(player) {
	}

}