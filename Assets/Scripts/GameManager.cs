using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {

	public Text counterText, pauseText, resumeText, msgText;
	public PlayerS playerScript;
	//public Text debugText1, debugText2, debugText;

	private int counterValue, focusCounter, pauseCounter;
	public DateTime lastMinimize = DateTime.Now;  //added = part
	public double minimizedSeconds;
	public double totMinimizedSeconds;

	public bool resumed;

	void OnApplicationPause (bool isGamePause)
	{
		if (isGamePause) {
			//Debug.Log ("it was paused");
			pauseCounter++;
			//pauseText.text = "Paused : " + pauseCounter;
			playerScript.Save();
			GoToMinimize ();
		}
	}

	void OnApplicationFocus (bool isGameFocus)
	{
		if (isGameFocus) {
			//Debug.Log ("sent to onfocus");
			focusCounter++;
			resumeText.text = "Focused : " + focusCounter;
			GoToMaximize ();
		}
	}

		// Use this for initialization
	void Start ()
	{
		StartCoroutine ("StartCounter");
		Application.runInBackground = true;
	}

	IEnumerator StartCounter ()
	{
		yield return new WaitForSeconds (1f);
		counterText.text = "Counter : " + counterValue.ToString ();
		counterValue++;
		StartCoroutine ("StartCounter");
	}

	public void GoToMinimize ()
	{
		//Debug.Log ("sent to minimize");
		playerScript.Save();
		lastMinimize = DateTime.Now;
	}

	public void GoToMaximize ()
	{
		resumed = true;
		//Debug.Log ("sent to maximize");
		if (focusCounter >= 2) {
			minimizedSeconds = (DateTime.Now - lastMinimize).TotalSeconds;
			totMinimizedSeconds += minimizedSeconds;
			msgText.text = "min secs: " + minimizedSeconds.ToString ()+ " totmin: "+totMinimizedSeconds.ToString();
			counterValue += (Int32)minimizedSeconds;
		}
	}
		
	// Update is called once per frame
	void Update () {
	
	}
}
