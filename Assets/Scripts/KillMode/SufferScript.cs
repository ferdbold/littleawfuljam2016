using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SufferScript : MonoBehaviour {

    public GameObject objectiveText;
    public float timeToChangeColor;

    private float _currentTime;
    private float _startTime;
    private Color _colorFrom;
    private Color _colorTo;
    private bool _mustChangeTargetColor=true;
    private bool _textChanged;



	// Use this for initialization
	void Start () {
        _colorFrom = new Color(Mathf.Clamp(Random.value, 0.1f, 1.0f), Mathf.Clamp(Random.value, 0.1f, 1.0f), Mathf.Clamp(Random.value, 0.1f, 1.0f), 1.0f);
	}
	
	// Update is called once per frame
	void Update () {

        if (!_textChanged)
        {
            if (GetComponentInParent<MiniGameController>().IsKillable())
            {
                _textChanged = true;
                objectiveText.GetComponent<Text>().text = "GRAB HIS HEART";
            }
        }

        if (_mustChangeTargetColor)
        {
            _mustChangeTargetColor = false;
            _startTime = Time.time;
            if (!_textChanged)
            {
                _colorTo = new Color(Mathf.Clamp(Random.value, 0.4f, 1.0f), Mathf.Clamp(Random.value, 0.4f, 1.0f), Mathf.Clamp(Random.value, 0.4f, 1.0f));
            }
            else
            {
                _colorTo = new Color(Mathf.Clamp(Random.value, 0.7f, 1.0f), Mathf.Clamp(Random.value, 0.2f, 0.5f), Mathf.Clamp(Random.value, 0.2f, 0.5f));
            }
        }
        else{
            _currentTime = Time.time - _startTime;
            objectiveText.GetComponent<Text>().color = Color.Lerp(_colorFrom, _colorTo, _currentTime / timeToChangeColor);
            //Debug.Log(_colorFrom + "          " + _colorTo + "         " + objectiveText.GetComponent<Text>().color);

            if (_currentTime >= timeToChangeColor)
            {
                _mustChangeTargetColor = true;
                _colorFrom = _colorTo;
            }
        }

	}
}
