using UnityEngine;
using System.Collections;


public class ControlsScript : MonoBehaviour {

	public RectTransform rightTrigger;
	public RectTransform leftTrigger;
	//public RectTransform rightButton;
	//public RectTransform leftButton;

	void Awake()
	{
		leftTrigger.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
		rightTrigger.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
		//rightButton.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
		//leftButton.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
	}

	public void EnableRT()
	{
		rightTrigger.GetComponent<CanvasRenderer>().SetAlpha(1.0f);
		//rightButton.GetComponent<CanvasRenderer>().SetAlpha(1.0f);
	}

	public void EnableLT()
	{
		leftTrigger.GetComponent<CanvasRenderer>().SetAlpha(1.0f);
		//leftButton.GetComponent<CanvasRenderer>().SetAlpha(1.0f);
	}

	public void DisableRT()
	{
		rightTrigger.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
		//rightButton.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
	}

	public void DisableLT()
	{
		leftTrigger.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
		//leftButton.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
	}

	public void GreyRT()
	{
		rightTrigger.GetComponent<CanvasRenderer>().SetColor(new Color(0.2f, 0.2f, 0.2f));
		//rightButton.GetComponent<CanvasRenderer>().SetColor(new Color(0.2f, 0.2f, 0.2f));
	}

	public void GreyLT()
	{
		leftTrigger.GetComponent<CanvasRenderer>().SetColor(new Color(0.2f, 0.2f, 0.2f));
		//leftButton.GetComponent<CanvasRenderer>().SetColor(new Color(0.2f, 0.2f, 0.2f));
	}

	public void WhiteRT()
	{
		rightTrigger.GetComponent<CanvasRenderer>().SetColor(new Color(1.0f, 1.0f, 1.0f));
		//rightButton.GetComponent<CanvasRenderer>().SetColor(new Color(1.0f, 1.0f, 1.0f));
	}

	public void WhiteLT()
	{
		leftTrigger.GetComponent<CanvasRenderer>().SetColor(new Color(1.0f, 1.0f, 1.0f));
		//leftButton.GetComponent<CanvasRenderer>().SetColor(new Color(1.0f, 1.0f, 1.0f));
	}
}
