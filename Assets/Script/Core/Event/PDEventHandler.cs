using System.Collections.Generic;

public class PDEventHandler<T> where T : PDEvent {

	readonly List<OnEvent<T>> listeners = new List<OnEvent<T>>();

	public void AddListener(OnEvent<T> listener) {
		if (!listeners.Contains(listener)) {
			listeners.Add(listener);
		}
	}

	public void RemoveListener(OnEvent<T> listener) {
		if (listeners.Contains(listener)) {
			listeners.Remove(listener);
		}
	}

	public void RemoveListeners() {
		listeners.Clear();
	}

	public void TriggerEvent(T e) {
		if (ReferenceEquals(e, null)) {
			return;
		}
		foreach (OnEvent<T> listener in listeners) {
			if (listener == null) {
				continue;
			}
			listener.Invoke(e);
		}
	}

}