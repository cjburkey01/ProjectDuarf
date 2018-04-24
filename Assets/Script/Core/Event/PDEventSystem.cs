using System.Collections.Generic;

public delegate void OnEvent<T>(T e) where T : PDEvent;

public class PDEventSystem {

	readonly List<object> handlers = new List<object>();

	public void AddListener<T>(OnEvent<T> onCall) where T : PDEvent {
		GetHandler<T>().AddListener(onCall);
	}

	public void RemoveListener<T>(OnEvent<T> onCall) where T : PDEvent {
		GetHandler<T>().RemoveListener(onCall);
	}

	public void RemoveListeners<T>() where T : PDEvent {
		GetHandler<T>().RemoveListeners();
	}

	public void TriggerEvent<T>(T e) where T : PDEvent {
		GetHandler<T>().TriggerEvent(e);
	}

	PDEventHandler<T> GetHandler<T>() where T : PDEvent {
		foreach (object handler in handlers) {
			if (handler != null && handler is PDEventHandler<T>) {
				return handler as PDEventHandler<T>;
			}
		}
		PDEventHandler<T> nhandler = new PDEventHandler<T>();
		handlers.Add(nhandler);
		return nhandler;
	}

}