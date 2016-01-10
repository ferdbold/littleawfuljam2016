using UnityEngine;
using System.Collections;

public class ArowScript : MonoBehaviour {

    public GameObject target;
    public RectTransform rightArrow;
    public RectTransform leftArrow;

    private Vector3 _leftArrowBaseRect;
    private Vector3 _rightArrowBaseRect;

    private bool _isGoingOutLeft=true;
    private bool _isGoingOutRight = true;
    private float _startTimeLeft;
    private float _startTimeRight;
    private float _currentTimeLeft;
    private float _currentTimeRight;
    public float timeBeforeGoingOut;
    public float timeBeforeGoingIn;
    public float distanceForArrowX;


	// Use this for initialization
	void Start () {
        _leftArrowBaseRect = leftArrow.localPosition;
        _rightArrowBaseRect = rightArrow.localPosition;
	}
	
	// Update is called once per frame
	void Update () {

        //Effets de mouvements et d'opacité flèche gauche
        if (target.GetComponent<MiniGameController>().IsAttackingLeft() || Input.GetKey(KeyCode.KeypadEnter))
        {
            //On va vers l'extérieur
            if (_isGoingOutLeft)
            {
                leftArrow.GetComponent<CanvasRenderer>().SetAlpha(1.0f);
                _currentTimeLeft = Time.time - _startTimeLeft;
                leftArrow.localPosition = Vector3.Lerp(_leftArrowBaseRect,
                    new Vector3(_leftArrowBaseRect.x - distanceForArrowX,
                        _leftArrowBaseRect.y, _leftArrowBaseRect.z),
                        _currentTimeLeft / timeBeforeGoingOut);
                if (_currentTimeLeft >= timeBeforeGoingOut)
                {
                    _isGoingOutLeft = false;
                    _startTimeLeft = Time.time;
                }
            }
            
            //On va vers l'intérieur
            else
            {
                _currentTimeLeft = Time.time - _startTimeLeft;
                leftArrow.localPosition = Vector3.Lerp(leftArrow.localPosition,
                    new Vector3(_leftArrowBaseRect.x,
                        _leftArrowBaseRect.y, _leftArrowBaseRect.z),
                        _currentTimeLeft / timeBeforeGoingIn);
                if (_currentTimeLeft >= timeBeforeGoingIn)
                {
                    _isGoingOutLeft = true;
                    _startTimeLeft = Time.time;
                }
            }

        }
        else
        {
            leftArrow.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
            leftArrow.localPosition = _leftArrowBaseRect;
            _startTimeLeft = Time.time;
        }


        //Effets de mouvements et d'opacité flèche droite
        if (target.GetComponent<MiniGameController>().IsAttackingRight())
        {
            //On va vers l'extérieeur
            if (_isGoingOutRight)
            {
                rightArrow.GetComponent<CanvasRenderer>().SetAlpha(1.0f);
                _currentTimeRight = Time.time - _startTimeRight;
                rightArrow.localPosition = Vector3.Lerp(_rightArrowBaseRect,
                    new Vector3(_rightArrowBaseRect.x + distanceForArrowX,
                        _rightArrowBaseRect.y, _rightArrowBaseRect.z),
                        _currentTimeRight / timeBeforeGoingOut);
                    if (_currentTimeRight >= timeBeforeGoingOut)
                    {
                        _isGoingOutRight = false;
                        _startTimeRight = Time.time;
                    }
            }
            //On va vers l'intérieur
            else
            {
                _currentTimeRight = Time.time - _startTimeRight;
                rightArrow.localPosition = Vector3.Lerp(rightArrow.localPosition,
                    new Vector3(_rightArrowBaseRect.x,
                        _rightArrowBaseRect.y, _rightArrowBaseRect.z),
                        _currentTimeRight / timeBeforeGoingIn);
                if (_currentTimeRight >= timeBeforeGoingIn)
                {
                    _isGoingOutRight = true;
                    _startTimeRight = Time.time;
                }
            }
        }
        else
        {
            rightArrow.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
            rightArrow.localPosition = _rightArrowBaseRect;
            _startTimeRight = Time.time;
        }
	}
}
