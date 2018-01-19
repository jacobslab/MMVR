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

		

	void Start()
	{
		OnChanged (EventArgs.Empty);
	}

	void Update()
	{
		
	}


}
