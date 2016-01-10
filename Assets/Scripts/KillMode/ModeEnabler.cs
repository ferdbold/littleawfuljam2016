using UnityEngine;
using System.Collections;

public class ModeEnabler : MonoBehaviour {

    public MiniGameController script;

	// Use this for initialization
	void Start () {
        script.enabled = false;
	}

    public void enableScript()
    {
        script.enabled = true;
    }

    public void disableScript()
    {
        script.enabled = false;
    }
}
