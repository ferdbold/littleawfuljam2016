using UnityEngine;
using System.Collections;

public class ModeEnabler : MonoBehaviour {

    public MiniGameController script;
    public Canvas killCamCanvas;

	// Use this for initialization
	void Start () {
        script.enabled = false;
        killCamCanvas.enabled = false;
	}

    public void enableScript()
    {
        script.enabled = true;
        killCamCanvas.enabled = true;
    }

    public void disableScript()
    {
        script.enabled = false;
        killCamCanvas.enabled = false;
    }
}
