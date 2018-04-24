public abstract class PDEvent {

	protected bool cancelled;

	public virtual bool IsCancellable() {
		return false;
	}

	public virtual bool IsCancelled() {
		return cancelled;
	}

	public virtual void Cancel() {
		if (IsCancellable() && !IsCancelled()) {
			cancelled = true;
		}
	}

	public virtual string GetName() {
		return GetType().FullName;
	}

}