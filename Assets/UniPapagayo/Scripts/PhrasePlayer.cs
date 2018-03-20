using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PhrasePlayer : MonoBehaviour {

    [SerializeField] PhraseContainer phraseContainer;

    [SerializeField] Animator anim;
    [SerializeField] AudioSource audioSource;

	public MorphAnimator morphAnim;

    Dictionary<int, string> frames = new Dictionary<int, string>();

    bool isPlaying = false;
    int currentFrame = 0, lastFrame = 1;
    float timer = 0f, timeDelta = 1f;

    void Start () {
        
    }
    
    void Update () {
        if (isPlaying)
        {
            timer += Time.deltaTime;
            if (timer >= timeDelta)
            {
                timer = 0f;
                currentFrame++;
                if (frames.ContainsKey(currentFrame))
                {
					string stringVal = "";
					frames.TryGetValue (currentFrame, out stringVal);
					Debug.Log ("string is: " + stringVal);
//                    anim.SetTrigger(stringVal);
					morphAnim.AnimateMorph(stringVal);
                }
            }
            if (currentFrame == lastFrame) { isPlaying = false; }
        }

		if (Input.GetKeyDown (KeyCode.U))
			PlayPhrase (0);
    }

    public void PlayPhrase(int i)
    {
        ParsePapagayoFile(phraseContainer.phrases[i].text);
        audioSource.Stop();
        audioSource.PlayOneShot(phraseContainer.phrases[i].audioClip);
        timer = 0;
        currentFrame = 0;
        isPlaying = true;
    }

    void ParsePapagayoFile(TextAsset file)
    {
        frames.Clear();
        string[] strings = file.text.Split('\n');
        foreach (string s in strings)
        {
            if (s.Contains("MohoSwitch") || s == "") { continue; }
            string[] line = s.Split(' ');
            lastFrame = int.Parse(line[0]);
			Debug.Log (line[1]);
			frames [lastFrame] = line [1];
			Debug.Log ("set frame val as: " + frames [lastFrame]);
        }
        timeDelta = phraseContainer.phrases[0].audioClip.length / lastFrame;
    }
}
