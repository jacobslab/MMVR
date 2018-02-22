using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;
public class MMVR_Core : MonoBehaviour {

	public SandboxEditorManager sandboxManager;
	public GameObject fpsControllerPrefab;
	private FirstPersonController fpsController;
	public EditorButton editorButton;
	public GameObject playerStart;
	public Text modeText;

	private bool isPlaytesting=false;

	public enum Mode { 
		SandboxEditor,
		PlaytestMode,
		FlowEditor,
		BehaviorEditor,
		LogicEditor
	}

	public Mode currentMode;
	// Use this for initialization
	void Start () {

		sandboxManager.ToggleCamera (true);
		currentMode = Mode.SandboxEditor;
		modeText.text = currentMode.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			TogglePlaytest ();
		}
		
	}

	public void TogglePlaytest()
	{
		isPlaytesting = !isPlaytesting;
		if (isPlaytesting)
			PlaytestLevel ();
		else
			StopPlaytest ();
	}

	//switch from whatever mode to "play" level
	void PlaytestLevel()
	{
		playerStart.SetActive (false);
		currentMode = Mode.PlaytestMode;
		sandboxManager.TogglePropertyPanel (false);
		modeText.text = currentMode.ToString ();
		editorButton.Play ();
		sandboxManager.ToggleCamera (false);
		//instantiate fpsController
		GameObject tempFPSController = Instantiate(fpsControllerPrefab,playerStart.transform.position,playerStart.transform.rotation) as GameObject;
		fpsController = tempFPSController.GetComponent<FirstPersonController> ();
		if(fpsController!=null)
			fpsController.gameObject.SetActive(true);
	}

	void StopPlaytest()
	{
		playerStart.SetActive (true);
		currentMode = Mode.SandboxEditor;
		sandboxManager.TogglePropertyPanel (true);
		modeText.text = currentMode.ToString ();
		editorButton.Stop ();
		sandboxManager.ToggleCamera (true);
		if (fpsController != null)
			Destroy (fpsController.gameObject);
	}
}
