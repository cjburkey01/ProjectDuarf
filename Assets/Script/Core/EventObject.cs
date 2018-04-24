using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;

public class EventObject : MonoBehaviour {

	public static PDEventSystem EventSystem { private set; get; }

	readonly static List<Type> toRegisterEvents = new List<Type>();

	void Main() {
		if (EventSystem != null) {
			Debug.LogWarning("The event system has already been initialized");
			return;
		}
		Debug.Log("Initializing event system");
		EventSystem = new PDEventSystem();
		foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies()) {
			foreach (Type type in a.GetTypes()) {
				if (type.GetCustomAttributes(typeof(RegisterEventHandlers), true).Length > 0) {
					toRegisterEvents.Add(type);
				}
			}
		}
		Debug.Log("Found " + toRegisterEvents.Count + " types for which to automatically register events");
		foreach (Type type in toRegisterEvents) {
			try {
				MethodInfo method = type.GetMethod("RegisterEvents", BindingFlags.Static | BindingFlags.NonPublic);
				if (method == null) {
					throw new Exception("\"private static void RegisterEvents()\" method not found");
				}
				method.Invoke(null, new object[0]);
			} catch (Exception err) {
				Debug.LogWarning("Couldn't register type for automatic event registering: " + type.Name + ". " + err.Message);
			}
		}
		toRegisterEvents.Clear();
		Debug.Log("Initialized event system");
	}

	void Start() {
		EventSystem.TriggerEvent(new EventGameInit());
	}

}