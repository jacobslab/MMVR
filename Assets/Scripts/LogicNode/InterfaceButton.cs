using UnityEngine;

using System;
using System.Collections;

public class InterfaceButton : MonoBehaviour
{
	public event Func<InterfaceButton, IEnumerator> ButtonPressedCoroutine = null;

	public IEnumerator ExecuteAction(IEnumerator coroutine)
	{
			yield return StartCoroutine(coroutine);
	}
}