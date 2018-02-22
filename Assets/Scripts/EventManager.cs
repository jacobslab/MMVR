using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EventManager : MonoBehaviour {

	// A delegate type for hooking up change notifications.
	public delegate void ChangedEventHandler(object sender, EventArgs e);

		// An event that clients can use to be notified whenever the
		// elements of the list change.
		public event ChangedEventHandler Changed;

	public delegate void SpawnableEventHandler ();
	public static event SpawnableEventHandler OnSpawned;
	public static event SpawnableEventHandler OnDestroyed;
	public static event SpawnableEventHandler OnInitialSetupComplete;
		// Invoke the Changed event; called whenever list changes
		protected virtual void OnChanged(EventArgs e)
		{
		
		if (Changed != null) {
			Debug.Log ("changed");
			Changed (this, e);
		} else {
			Debug.Log ("invoked");
		}

		}

	public static void CompleteSetup()
	{

		OnInitialSetupComplete ();
	}

	void Start()
	{
		OnChanged (EventArgs.Empty);
	}

	void Update()
	{
		
	}


}
