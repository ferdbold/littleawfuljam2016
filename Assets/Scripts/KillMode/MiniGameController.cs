using UnityEngine;
using System.Collections;

public class MiniGameController : MonoBehaviour {

	//Audio stuff
    private AudioSource _audio1;
    private AudioSource _audio2;
	private float _initialPitch1;
	private float _initialPitch2;
	public float maxPitchDifference = 0.15f;

	//MiniGame terminé
	private bool _miniGameOver = false;

    //Nombre de hits effectués par le paresseux
    private int _slothHits=0;
    public int targetHealth;

    //Les bras du paresseux
    public GameObject rightArm;
    public GameObject leftArm;

    //Peut-il planter ses griffes
    public bool canPlantRightArm;
    public bool canPlantLeftArm;

    //Vitesse de deplacement du bras
    public float armSpeed;

    //Vitesse de l'entrée dans le bras
    public float armPierceSpeed;

    //Valeur max en z
    public float maxArmZ;


    //Valeurs max et min y et x
    public float maxRightArmX;
    public float minRightArmX;
    public float maxLeftArmX;
    public float minLeftArmX;
    public float maxArmY;
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

    //Variables smoothing entrée des griffes
    private float _startTimeRightArmEnter;
    private float _currentTimeRightArmEnter;
    private float _startTimeLeftArmEnter;
    private float _currentTimeLeftArmEnter;
    private Vector3 _lastPosRightArmEnter;
    private Vector3 _lastPostLeftArmEnter;
    public float timeToEnter;



    //Est-ce qu'il est entrain de percé le scientifique
    private bool _isPiercingWithLeftArm = false;
    private bool _isPiercingWithRightArm = false;

    //Variables pour les pulsions de sang lors de la sortie des griffes
    private bool _doPulseBloodRightArm;
    private bool _doPulseBloodLeftArm;

    //Peut-on bouger verticalement avec les griffes plantés
    private bool _canMoveVerticalRightArm = true;
    private bool _canMoveVerticalLeftArm = true;

    //Peut-on bouger horizontalement les griffes
    private bool _canMoveHorizontalRightArm = true;
    private bool _canMoveHorizontalLeftArm = true;

    private bool _leftArmDamageDone = false;
    private bool _rightArmDamageDone = false;



	// Use this for initialization
	void Start () {
        _minArmZ = rightArm.transform.localPosition.z;
        AudioSource[] array = GetComponents<AudioSource>();
        _audio1 = array[0];
        _audio2 = array[1];
		_initialPitch1 = array[0].pitch;
		_initialPitch2 = array[1].pitch;
	}
	


	// Update is called once per frame
	void Update () {
        SticksInputs();
        ClawsInputs();
        PulseBlood();
		AudioFeedback();
		UpdateUI();

		//Si la game est terminé
		if (_miniGameOver)
		{
			LevelManager.instance.SwitchState(LevelManager.GameState.endLevelCutscene);
		}



	}

	private void UpdateUI()
	{
		if (canPlantLeftArm)
		{
			GetComponentInChildren<Canvas>().GetComponent<ControlsScript>().EnableRT();
		}
		else
		{
			GetComponentInChildren<Canvas>().GetComponent<ControlsScript>().DisableRT();
		}
		if (canPlantRightArm)
		{
			GetComponentInChildren<Canvas>().GetComponent<ControlsScript>().EnableLT();
		}
		else
		{
			GetComponentInChildren<Canvas>().GetComponent<ControlsScript>().DisableLT();
		}

		if (!_canMoveVerticalLeftArm)
		{
			GetComponentInChildren<Canvas>().GetComponent<ControlsScript>().GreyRT();
		}
		else
		{
			GetComponentInChildren<Canvas>().GetComponent<ControlsScript>().WhiteRT();

		}

		if (!_canMoveVerticalRightArm)
		{
			GetComponentInChildren<Canvas>().GetComponent<ControlsScript>().GreyLT();
		}
		else
		{
			GetComponentInChildren<Canvas>().GetComponent<ControlsScript>().WhiteLT();
		}
	}

	/// <summary>
	/// On traite tous les inputs des sticks afin d'effectuer le déplacement des mains du paresseux
	/// </summary>
	private void SticksInputs()
    {

		var xRightArm = 0.0f;

		if (Mathf.Abs(Input.GetAxis("LeftAnalogHorizontal")) > Mathf.Abs(Input.GetAxis("LeftAnalogHorizontalController")))
		{
			xRightArm = (_canMoveHorizontalLeftArm) ? Input.GetAxis("LeftAnalogHorizontal") * armSpeed : 0.0f;
		}
		else
		{
			xRightArm = (_canMoveHorizontalLeftArm) ? Input.GetAxis("LeftAnalogHorizontalController") * armSpeed : 0.0f;
		}


		//Taux de translation pour le bras droit à l'horizontal et à la vertical
		//var xRightArm = (_canMoveHorizontalRightArm) ? Input.GetAxis("LeftAnalogHorizontal") * armSpeed : 0.0f;


        //Si le bras droit est planté ou pressque
        if (rightArm.transform.localPosition.z <= _minArmZ)
        {
            _canMoveVerticalRightArm = true;
        }
        //Sinon
        else
        {
            _canMoveVerticalRightArm = false;
        }

        var yRightArm = 0.0f;

		//Si le bras droit est planté
		if (_canMoveVerticalRightArm)
		{
			if (Mathf.Abs(Input.GetAxis("LeftAnalogVertical")) > Mathf.Abs(Input.GetAxis("LeftAnalogVerticalController"))) {
				yRightArm = -Input.GetAxis("LeftAnalogVertical") * armSpeed;
			}
			else
			{
				yRightArm = -Input.GetAxis("LeftAnalogVerticalController") * armSpeed;
			}
        }


		//Taux de translation pour le bras gauche à l'horizontal et à la vertical

		var xLeftArm=0.0f;

		if (Mathf.Abs(Input.GetAxis("RightAnalogHorizontalController")) > Mathf.Abs(Input.GetAxis("RightAnalogHorizontal")))
		{
			 xLeftArm = (_canMoveHorizontalLeftArm) ? Input.GetAxis("RightAnalogHorizontalController") * armSpeed : 0.0f;
		}
		else
		{
			 xLeftArm = (_canMoveHorizontalLeftArm) ?  Input.GetAxis("RightAnalogHorizontal") * armSpeed : 0.0f;
		}
		

        


        //Si le bras gauche est planté ou pressque
        if (leftArm.transform.localPosition.z <= _minArmZ)
        {
            _canMoveVerticalLeftArm = true;
        }
        //Sinon
        else
        {
            _canMoveVerticalLeftArm = false;
        }

		var yLeftArm = 0.0f;

		//Si le bras gauche est planté
		if (_canMoveVerticalLeftArm)
        {
			if (Mathf.Abs(Input.GetAxis("RightAnalogVertical")) > Mathf.Abs(Input.GetAxis("RightAnalogVerticalController")))
			{
				yLeftArm = -Input.GetAxis("RightAnalogVertical") * armSpeed;
			}
			else
			{
				yLeftArm = -Input.GetAxis("RightAnalogVerticalController") * armSpeed;
			}
		}

        //On effectue la translation pour le right arm + on clamp sur un range
        rightArm.transform.localPosition = new Vector3(Mathf.Clamp(rightArm.transform.localPosition.x + xRightArm, minRightArmX, maxRightArmX),
            Mathf.Clamp(rightArm.transform.localPosition.y + yRightArm, minArmY, maxArmY),
            rightArm.transform.localPosition.z);

        //On effectue la translation pour le right arm + on clamp sur un range 
        leftArm.transform.localPosition = new Vector3(Mathf.Clamp(leftArm.transform.localPosition.x + xLeftArm, minLeftArmX, maxLeftArmX),
            Mathf.Clamp(leftArm.transform.localPosition.y + yLeftArm, minArmY, maxArmY),
            leftArm.transform.localPosition.z);
		
	}

    /// <summary>
    /// On call cette fonction dans update pour les inputs des gachettes avec les griffes du paresseux
    /// </summary>
    private void ClawsInputs(){
        //On prend l'input des gachettes de tirs
        var pierceRightArm=0.0f;
        var pierceLeftArm=0.0f;

        if (canPlantRightArm) //Si on peut percer avec le bras droit
        {
            pierceRightArm = Input.GetAxis("LeftTrigger") * armPierceSpeed;
            if (pierceRightArm > 0.0f)
            {
                _currentTimeRightArmEnter = Time.time - _startTimeRightArmEnter;
                _canMoveVerticalRightArm = false; 

				if (rightArm.transform.localPosition.z >= maxArmZ) 
                {
                    _canMoveHorizontalRightArm = true;
                }
                else
                {
                    _canMoveHorizontalRightArm = false;
                }
                
            }
            else
            {
                _startTimeRightArmEnter = Time.time;
                _lastPosRightArmEnter = rightArm.transform.localPosition;
                _canMoveHorizontalRightArm = true;
            }
        }


        if (canPlantLeftArm) //Si on peut percer avec le bras gauche
        {
            pierceLeftArm = Input.GetAxis("RightTrigger") * armPierceSpeed;
            if (pierceLeftArm > 0.0f)
            {
                _currentTimeLeftArmEnter = Time.time - _startTimeLeftArmEnter;
                _canMoveVerticalLeftArm = false;

                if (leftArm.transform.localPosition.z >= maxArmZ) //Si la main est presque rendu au bout de la distance, alors on peut bouger à l'horizontal
                {
                    _canMoveHorizontalLeftArm = true;
                }
                else
                {
                    _canMoveHorizontalLeftArm = false;
                }
                
            }
            else
            {
                _startTimeLeftArmEnter = Time.time;
                _lastPostLeftArmEnter = leftArm.transform.localPosition;
                _canMoveHorizontalLeftArm = true;
            }
        }



        //On retire la griffe droite
        if (pierceRightArm < 0.01 && rightArm.transform.localPosition.z != _minArmZ)
        {
            _doPulseBloodRightArm = false;
            _currentTimeRightArm = Time.time - _startTimeRightArm;
            rightArm.transform.localPosition = Vector3.Lerp(_lastPosRightArm,
                new Vector3(_lastPosRightArm.x, _lastPosRightArm.y, _minArmZ),
                _currentTimeRightArm/timeToExitWound);
            _isPiercingWithRightArm = false;
            _rightArmDamageDone = false;
        }
        //On perce avec la griffe droite 
        else if(pierceRightArm>0)
        {
            _isPiercingWithRightArm = true;
            //Si on n'est pas rendu presque au bout, on lerp
            if (!(rightArm.transform.localPosition.z == maxArmZ))
            {
                _currentTimeRightArm = Time.time - _startTimeRightArm;
                rightArm.transform.localPosition = Vector3.Lerp(_lastPosRightArmEnter,
                new Vector3(maxRightArmX, _lastPosRightArmEnter.y, maxArmZ),
                _currentTimeRightArmEnter / timeToEnter);

                
            }
            else
            {

                if (!_rightArmDamageDone)
                {
                    _rightArmDamageDone = true;
                    _slothHits++;
                }

                //On griffe et ca fait saigner le bras gauche!
                _doPulseBloodRightArm = true;
                rightArm.transform.localPosition = new Vector3(rightArm.transform.localPosition.x,
                rightArm.transform.localPosition.y,
                Mathf.Clamp(rightArm.transform.localPosition.z + pierceRightArm, _minArmZ, maxArmZ));
            }
            //Partie pour preparer a retirer la griffe
            _startTimeRightArm = Time.time;
            _lastPosRightArm = rightArm.transform.localPosition;

        }

        

        //On retire la griffe gauche
        if (pierceLeftArm < 0.01 && leftArm.transform.localPosition.z != _minArmZ)
        {
            
            //CONDITION DE VICTOIRE 
            if (leftArm.transform.localPosition.z == maxArmZ)
            {
                if ((leftArm.GetComponent<ClawCut>().GetClawPosition() == (ClawCut.ClawStatus.LowerUpRight)
                    || leftArm.GetComponent<ClawCut>().GetClawPosition() == ClawCut.ClawStatus.UpperUpRight)
                    && _slothHits > targetHealth)
                {
                    _miniGameOver = true;
                }
            }

            _currentTimeLeftArm = Time.time - _startTimeLeftArm;

            

            leftArm.transform.localPosition = Vector3.Lerp(_lastPostLeftArm,
                new Vector3(_lastPostLeftArm.x, _lastPostLeftArm.y, _minArmZ),
                _currentTimeLeftArm / timeToExitWound);
            _doPulseBloodLeftArm = false;
            _isPiercingWithLeftArm = false;
            _leftArmDamageDone = false;



		}

        //On perce avec la griffe gauche
        else if (pierceLeftArm > 0)
        {
            _isPiercingWithLeftArm = true; //il perce le scientifique avec son bras gauche

            if (!(leftArm.transform.localPosition.z == maxArmZ))
            {
                _currentTimeLeftArmEnter = Time.time - _startTimeLeftArmEnter;
                leftArm.transform.localPosition = Vector3.Lerp(_lastPostLeftArmEnter,
                new Vector3(minLeftArmX, _lastPostLeftArmEnter.y, maxArmZ),
                _currentTimeLeftArmEnter / timeToEnter);

            }

            else
            {
                //On griffe et ca fait saigner le bras gauche!
                if (!_leftArmDamageDone)
                {
                    _leftArmDamageDone = true;
                    _slothHits++;
                }
                _doPulseBloodLeftArm = true;
                leftArm.transform.localPosition = new Vector3(leftArm.transform.localPosition.x,
                leftArm.transform.localPosition.y,
                Mathf.Clamp(leftArm.transform.localPosition.z + pierceRightArm, _minArmZ, maxArmZ));
            }
            //Partie pour preparer a retirer la griffe
            _startTimeLeftArm = Time.time;
            _lastPostLeftArm = leftArm.transform.localPosition;

        }
    }

	/// <summary>
	/// Méthode qui effectue le feedback audio yo
	/// </summary>
	private void AudioFeedback()
	{
		if (_isPiercingWithLeftArm || _isPiercingWithRightArm)
		{
			if (!_audio1.isPlaying && !_audio2.isPlaying)
			{
				if (Random.Range(0f, 1f) >= 0.5f)
				{
					_audio1.Play();
					_audio1.pitch = Mathf.Clamp(_audio1.pitch + Random.Range(-0.1f, 0.1f), _initialPitch1 - maxPitchDifference, _initialPitch1 + maxPitchDifference);
				}
				else
				{
					_audio2.Play();
					_audio2.pitch = Mathf.Clamp(_audio2.pitch + Random.Range(-0.1f, 0.1f), _initialPitch2 - maxPitchDifference, _initialPitch2 + maxPitchDifference);
				}


			}
		}
		else
		{
			if (_audio1.isPlaying)
				_audio1.Stop();
			if (_audio2.isPlaying)
				_audio2.Stop();
		}
	}

    public bool IsKillable()
    {
        if (_slothHits >= targetHealth)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    private void PulseBlood()
    {
        if (_doPulseBloodLeftArm) {
            leftArm.GetComponent<EllipsoidParticleEmitter>().emit = true;
        }
        else
        {
            leftArm.GetComponent<EllipsoidParticleEmitter>().emit = false;
        }

        if (_doPulseBloodRightArm){
            rightArm.GetComponent<EllipsoidParticleEmitter>().emit = true;
        }
        else
        {
            rightArm.GetComponent<EllipsoidParticleEmitter>().emit = false;
        }
    }

    public bool IsAttackingLeft()
    {
        return !_canMoveVerticalRightArm;
    }

    public bool IsAttackingRight()
    {
        return !_canMoveVerticalLeftArm;
    }

}
