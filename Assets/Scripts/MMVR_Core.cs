using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.UI;
public class MMVR_Core : MonoBehaviour {

	//mode managers
	public SandboxEditorManager sandboxManager;
	public LogicNodeManager logicManager;

	public PropertyPanelManager propertyPanelManager;

	public GameObject fpsControllerPrefab;
	private FirstPersonController fpsController;
	public EditorButton editorButton;
	public GameObject playerStart;
	public Text modeText;

	public GameObject logicModeCanvas;
	//logic mode

	private bool isPlaytesting=false;
	private bool isLogic=false;

	public enum Mode { 
		SandboxEditor,
		PlaytestMode,
		FlowEditor,
		BehaviorEditor,
		LogicEditor
	}

	public Mode currentMode;

	//IS A SINGLETON
	private static MMVR_Core _instance;
	public static MMVR_Core Instance{
		get{
			return _instance;
		}
	}

	void Awake(){
		if (_instance != null) {
			Debug.Log ("Instance already exists!");
			return;
		}
		_instance = this;
	}
	// Use this for initialization
	void Start () {

		sandboxManager.ToggleCamera (true);
		currentMode = Mode.SandboxEditor;
		ToggleBetweenModes (currentMode);
		Time.timeScale = 0f;
		modeText.text = currentMode.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			TogglePlaytest ();
		}
		
	}

	public void ToggleLogic(GameObject selectedObject)
	{
		isLogic = !isLogic;
		if (isLogic)
			ShowObjectLogic (selectedObject);
		else
			SwitchToEditor (selectedObject);
	}

	public void TogglePlaytest()
	{
		isPlaytesting = !isPlaytesting;
		if (isPlaytesting)
			PlaytestLevel ();
		else
			StopPlaytest ();
	}


	//switch from whatever mode to "Logic" mode
	void ShowObjectLogic(GameObject selectedObject)
	{
		currentMode = Mode.LogicEditor;
		logicModeCanvas.SetActive (true);

		ToggleBetweenModes (currentMode);

		sandboxManager.ToggleSandboxCanvas (false);
		sandboxManager.TogglePropertyPanel (false);
		sandboxManager.ToggleCamera (false);
	}

	void SwitchToEditor(GameObject selectedObject)
	{

		currentMode = Mode.SandboxEditor;
		logicModeCanvas.SetActive (false);
		ToggleBetweenModes (currentMode);

		sandboxManager.ToggleSandboxCanvas (true);
		sandboxManager.TogglePropertyPanel (true);
		sandboxManager.ToggleCamera (true);
	}

	//switch from whatever mode to "play" level
	void PlaytestLevel()
	{
		Time.timeScale = 1f;
		playerStart.SetActive (false);
		currentMode = Mode.PlaytestMode;
		sandboxManager.TogglePropertyPanel (false);
		modeText.text = currentMode.ToString ();
		editorButton.Play ();
		sandboxManager.ToggleCamera (false);
		//instantiate fpsController
		GameObject tempFPSController = Instantiate(fpsControllerPrefab,playerStart.transform.position,playerStart.transform.rotation) as GameObject;
		fpsController = tempFPSController.GetComponent<FirstPersonController> ();
		propertyPanelManager.ApplyPlayerProperties (fpsController);
		if(fpsController!=null)
			fpsController.gameObject.SetActive(true);
	}

	void StopPlaytest()
	{

		Time.timeScale = 0f;
		playerStart.SetActive (true);
		currentMode = Mode.SandboxEditor;
		sandboxManager.TogglePropertyPanel (true);
		modeText.text = currentMode.ToString ();
		editorButton.Stop ();
		sandboxManager.ToggleCamera (true);
		if (fpsController != null)
			Destroy (fpsController.gameObject);
	}

	public void SpawnLogicLayer(GameObject spawnedObj)
	{
		Debug.Log ("spawnedobj: " + spawnedObj.name);
		logicManager.SpawnBasicLayer (spawnedObj);
	}

	//button management

	void DisableAllModeButtons()
	{
		logicManager.sandboxButton.gameObject.SetActive (false);
		sandboxManager.logicButton.gameObject.SetActive (false);
	}

	public void ToggleBetweenModes(MMVR_Core.Mode mode)
	{
		//first disable all buttons
		DisableAllModeButtons ();

		//then selectively enable the necessary ones
		switch (mode) 
		{
		case MMVR_Core.Mode.SandboxEditor:
			sandboxManager.logicButton.gameObject.SetActive (true);
			break;
		case MMVR_Core.Mode.LogicEditor:
			logicManager.sandboxButton.gameObject.SetActive (true);
			break;
		}
	}

}
