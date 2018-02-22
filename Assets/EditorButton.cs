using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EditorButton : MonoBehaviour {

	public Image buttonImage;
	public Sprite playIcon;
	public Sprite pausedIcon;

	// Use this for initialization
	void Start () {
		buttonImage.sprite = playIcon;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Play()
	{
		buttonImage.sprite = pausedIcon;
	}
	public void Stop()
	{
		buttonImage.sprite = playIcon;
	}
}
