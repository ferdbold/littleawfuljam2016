using UnityEngine;
using System.Collections;

public class MiniGameController : MonoBehaviour {
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



	// Use this for initialization
	void Start () {
        _minArmZ = rightArm.transform.localPosition.z;
	}
	
	// Update is called once per frame
	void Update () {
        SticksInputs();
        ClawsInputs();
        PulseBlood();
        if (_miniGameOver)
        {
            LevelManager.instance.SwitchState(LevelManager.GameState.endLevelCutscene);
        }
	}

    /// <summary>
    /// On traite tous les inputs des sticks afin d'effectuer le déplacement des mains du paresseux
    /// </summary>
    private void SticksInputs()
    {
        //Taux de translation pour le bras droit à l'horizontal et à la vertical
        var xRightArm = (_canMoveHorizontalRightArm) ? Input.GetAxis("LeftAnalogHorizontal") * armSpeed : 0.0f;


        //Si le bras droit est planté ou pressque
        if (rightArm.transform.localPosition.z <= _minArmZ + 0.1)
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
            yRightArm = -Input.GetAxis("LeftAnalogVertical") * armSpeed;
        }

        
        //Taux de translation pour le bras gauche à l'horizontal et à la vertical
        var xLeftArm = (_canMoveHorizontalLeftArm) ? Input.GetAxis("RightAnalogHorizontal") * armSpeed : 0.0f; 

        var yLeftArm = 0.0f;


        //Si le bras gauche est planté ou pressque
        if (leftArm.transform.localPosition.z <= _minArmZ + 0.1)
        {
            _canMoveVerticalLeftArm = true;
        }
        //Sinon
        else
        {
            _canMoveVerticalLeftArm = false;
        }

        //Si le bras gauche est planté
        if (_canMoveVerticalLeftArm)
        {
            yLeftArm = -Input.GetAxis("RightAnalogVertical") * armSpeed;
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

                if (rightArm.transform.localPosition.z >= maxArmZ - 0.2) 
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

                if (leftArm.transform.localPosition.z >= maxArmZ - 0.2) //Si la main est presque rendu au bout de la distance, alors on peut bouger à l'horizontal
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
            if (rightArm.transform.localPosition.z <= _minArmZ && _isPiercingWithRightArm)
            {
                _slothHits++;//On incrémente de 1 le nombre d'hits du paresseux
                _isPiercingWithRightArm = false;
            }
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
            _currentTimeLeftArm = Time.time - _startTimeLeftArm;

            

            leftArm.transform.localPosition = Vector3.Lerp(_lastPostLeftArm,
                new Vector3(_lastPostLeftArm.x, _lastPostLeftArm.y, _minArmZ),
                _currentTimeLeftArm / timeToExitWound);
            _doPulseBloodLeftArm = false;
            if (leftArm.transform.localPosition.z <= _minArmZ && _isPiercingWithLeftArm)
            {
                _isPiercingWithLeftArm = false;
                Debug.Log(leftArm.GetComponent<ClawCut>().GetClawPosition() + "    " + _slothHits);
                _slothHits++;//On incrémente de 1 le nombre d'hits du paresseux
                if (leftArm.GetComponent<ClawCut>().GetClawPosition() == ClawCut.ClawStatus.LowerUpRight && _slothHits>=targetHealth)
                {
                    _miniGameOver = true;
                }
            }
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
