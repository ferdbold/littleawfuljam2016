using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum ClawStatus : int { UpperUpLeft, UpperUpRight, LowerUpLeft, LowerUpRight, UpperBottomLeft, UpperBottomRight, LowerBottomLeft, LowerBottomRight, None };

public class ClawCut : MonoBehaviour {


    private ClawStatus _currentPosition;

    private List<GameObject> _ObjectsInCollision = new List<GameObject>();

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "attack-zone")
        {
            _ObjectsInCollision.Add(col.gameObject);
            Debug.Log("EnterAttackZone");
            Debug.Log(_ObjectsInCollision);
            AttackZoneCheckIn(col);
        }

        if (col.gameObject.tag == "enemy-target")
        {
           // Debug.Log("EnterEnemyContact");
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "attack-zone")
        {
            Debug.Log("ExitAttackZone");
            _ObjectsInCollision.Remove(col.gameObject);
            if (_ObjectsInCollision.Count < 1)
            {
                Debug.Log(_ObjectsInCollision);
                AttackZoneCheckOut(col);
            }
        }

        if (col.gameObject.tag == "enemy-target")
        {
           // Debug.Log("ExitEnemyContact");
        }
    }

    /// <summary>
    /// On verifie si la zone d'attaque est possible, si oui on modifie le booléen canPlantArm correspondant dans le MiniGameController
    /// </summary>
    /// <param name="col"></param>
    private void AttackZoneCheckIn(Collider col)
    {
        switch (col.gameObject.name)
        {
            case "UpperUpLeftAttackZone":
                GetComponentInParent<MiniGameController>().canPlantRightArm = true;
                _currentPosition = ClawStatus.UpperUpLeft;
                break;

            case "LowerUpLeftAttackZone":
                GetComponentInParent<MiniGameController>().canPlantRightArm = true;
                _currentPosition = ClawStatus.LowerUpLeft;
                break;

            case "UpperBottomLeftAttackZone":
                GetComponentInParent<MiniGameController>().canPlantRightArm = true;
                _currentPosition = ClawStatus.UpperBottomLeft;
                break;

            case "LowerBottomLeftAttackZone":
                GetComponentInParent<MiniGameController>().canPlantRightArm = true;
                _currentPosition = ClawStatus.LowerBottomLeft;
                break;

            case "UpperUpRightAttackZone":
                GetComponentInParent<MiniGameController>().canPlantLeftArm = true;
                _currentPosition = ClawStatus.UpperUpRight;
                break;

            case "LowerUpRightAttackZone":
                GetComponentInParent<MiniGameController>().canPlantLeftArm = true;
                _currentPosition = ClawStatus.LowerUpRight;
                break;

            case "UpperBottomRightAttackZone":
                GetComponentInParent<MiniGameController>().canPlantLeftArm = true;
                _currentPosition = ClawStatus.UpperBottomRight;
                break;

            case "LowerBottomRightAttackZone":
                GetComponentInParent<MiniGameController>().canPlantLeftArm = true;
                _currentPosition = ClawStatus.LowerBottomRight;
                break;
        }
    }

    /// <summary>
    /// On enleve la possibilité de planter un bras lorsque le bras sort d'une des zones d'attaques
    /// </summary>
    /// <param name="col"></param>
    private void AttackZoneCheckOut(Collider col)
    {
        switch (col.gameObject.name)
        {
            case "UpperUpLeftAttackZone" :
                GetComponentInParent<MiniGameController>().canPlantRightArm = false;
                _currentPosition = ClawStatus.None;
                break;

            case "LowerUpLeftAttackZone":
                GetComponentInParent<MiniGameController>().canPlantRightArm = false;
                _currentPosition = ClawStatus.None;
                break;

            case "UpperBottomLeftAttackZone":
                GetComponentInParent<MiniGameController>().canPlantRightArm = false;
                _currentPosition = ClawStatus.None;
                break;
                
            case "LowerBottomLeftAttackZone":
                GetComponentInParent<MiniGameController>().canPlantRightArm = false;
                _currentPosition = ClawStatus.None;
                break;
                
            case "UpperUpRightAttackZone":
                GetComponentInParent<MiniGameController>().canPlantLeftArm = false;
                _currentPosition = ClawStatus.None;
                break;

            case "LowerUpRightAttackZone":
                GetComponentInParent<MiniGameController>().canPlantLeftArm = false;
                _currentPosition = ClawStatus.None;
                break;

            case "UpperBottomRightAttackZone":
                GetComponentInParent<MiniGameController>().canPlantLeftArm = false;
                _currentPosition = ClawStatus.None;
                break;

            case "LowerBottomRightAttackZone":
                GetComponentInParent<MiniGameController>().canPlantLeftArm = false;
                _currentPosition = ClawStatus.None;
                break;
        }

    }

    public int GetClawPosition() { return (int) _currentPosition; }
}
