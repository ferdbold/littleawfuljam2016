using UnityEngine;
using System.Collections;

public class MiniGameController : MonoBehaviour {

    //Les bras du paresseux
    public GameObject rightArm;
    public GameObject leftArm;

    //Vitesse de deplacement du bras
    public float armSpeed;

    //Vitesse de l'entrée dans le bras
    public float armPierceSpeed;

    //Valeur max en z
    public float maxArmZ;


    //Valeurs max et min y et x
    public float maxArmX;
    public float maxArmY;
    public float minArmX;
    public float minArmY;

    //Valeur min en z
    private float _minArmZ;

    //Variables pour le smoothing afin d'extirper les griffes
    private float _startTimeRightArm;
    private float _currentTimeRightArm;
    private float _startTimeLeftArm;
    private float _currentTimeLeftArm;
    private Vector3 _lastPosRightArm;
    private Vector3 _lastPostLeftArm;
    public float timeToExitWound;


	// Use this for initialization
	void Start () {
        _minArmZ = rightArm.transform.localPosition.z;
	}
	
	// Update is called once per frame
	void Update () {
        SticksInputs();
        ClawsInputs();
	}

    /// <summary>
    /// On traite tous les inputs des sticks afin d'effectuer le déplacement des mains du paresseux
    /// </summary>
    private void SticksInputs()
    {
        //Taux de rotation pour le bras droit à l'horizontal et à la vertical
        var xRightArm = Input.GetAxis("LeftAnalogHorizontal") * armSpeed;
        var yRightArm = -Input.GetAxis("LeftAnalogVertical") * armSpeed;


        //Taux de rotation pour le bras gauche à l'horizontal et à la vertical
        var xLeftArm = Input.GetAxis("RightAnalogHorizontal") * armSpeed;
        var yLeftArm = -Input.GetAxis("RightAnalogVertical") * armSpeed;


        //On effectue la translation pour le right arm + on clamp sur un range
        rightArm.transform.localPosition = new Vector3(Mathf.Clamp(rightArm.transform.localPosition.x + xRightArm, minArmX, 0),
            Mathf.Clamp(rightArm.transform.localPosition.y + yRightArm, minArmY, maxArmY),
            rightArm.transform.localPosition.z);

        //On effectue la translation pour le right arm + on clamp sur un range 
        leftArm.transform.localPosition = new Vector3(Mathf.Clamp(leftArm.transform.localPosition.x + xLeftArm, 0, maxArmX),
            Mathf.Clamp(leftArm.transform.localPosition.y + yLeftArm, minArmY, maxArmY),
            leftArm.transform.localPosition.z);
    }

    /// <summary>
    /// On call cette fonction dans update pour les inputs des gachettes avec les griffes du paresseux
    /// </summary>
    private void ClawsInputs(){
        //On prend l'input des gachettes de tirs
        var pierceRightArm = Input.GetAxis("LeftTrigger") * armPierceSpeed;
        var pierceLeftArm = Input.GetAxis("RightTrigger") * armPierceSpeed;


        //On retire la griffe droite
        if (pierceRightArm < 0.01 && rightArm.transform.localPosition.z != _minArmZ)
        {
            _currentTimeRightArm = Time.time - _startTimeRightArm;
            rightArm.transform.localPosition = Vector3.Lerp(_lastPosRightArm,
                new Vector3(_lastPosRightArm.x, _lastPosRightArm.y, _minArmZ),
                _currentTimeRightArm/timeToExitWound);
        }
        //On perce avec la griffe droite 
        else
        {
            rightArm.transform.localPosition = new Vector3(rightArm.transform.localPosition.x,
            rightArm.transform.localPosition.y,
            Mathf.Clamp(rightArm.transform.localPosition.z + pierceRightArm, _minArmZ, maxArmZ));
            _startTimeRightArm = Time.time;
            _lastPosRightArm = rightArm.transform.localPosition;
        }


        //On retire la griffe gauche
        if (pierceLeftArm < 0.01 && leftArm.transform.localPosition.z != _minArmZ)
        {
            _currentTimeLeftArm = Time.time - _startTimeLeftArm;
            leftArm.transform.localPosition = Vector3.Lerp(_lastPostLeftArm,
                new Vector3(_lastPostLeftArm.x, _lastPostLeftArm.y, _minArmZ),
                _currentTimeLeftArm / timeToExitWound);
        }
        //On perce avec la griffe gauche
        else
        {
            leftArm.transform.localPosition = new Vector3(leftArm.transform.localPosition.x,
            leftArm.transform.localPosition.y,
            Mathf.Clamp(leftArm.transform.localPosition.z + pierceLeftArm, _minArmZ, maxArmZ));
            _startTimeLeftArm = Time.time;
            _lastPostLeftArm = leftArm.transform.localPosition;
        }
    }
}
