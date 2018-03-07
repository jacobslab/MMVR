using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.UI;
public class MMVR_Core : MonoBehaviour {

	public PropertyPanelManager propertyPanelManager;
	public SandboxEditorManager sandboxManager;
	public GameObject fpsControllerPrefab;
	private FirstPersonController fpsController;
	public EditorButton editorButton;
	public GameObject playerStart;
	public Text modeText;

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
		
	}

	void SwitchToEditor(GameObject selectedObject)
	{
		
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
}
