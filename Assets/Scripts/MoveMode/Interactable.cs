using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/**
 * Scene object the sloth can interact with
 */
public class Interactable : MonoBehaviour {

	private float promptAlpha = 0f;

	private CanvasGroup promptCanvasGroup;
	private Image promptIcon;
	private Text promptText;

	[Header("Prompt")]
	[SerializeField] private Sprite icon;
	[SerializeField] private string prompt;
	[SerializeField] private float animDuration = 0.5f;

	public void Awake() {
		this.FindElements();
	}

	public void Start() {
		this.RefreshPrompt();
	}

	#if UNITY_EDITOR
	public void OnValidate() {
		this.FindElements();
		this.RefreshPrompt();
	}
	#endif

	public void Update () {
		if (Input.GetKey(KeyCode.F)) {
			this.Focus(true);
		}
		if (Input.GetKey(KeyCode.G)) {
			this.Focus(false);
		}

		this.RefreshPrompt();
	}

	/**
	 * Register all child elements
	 */
	private void FindElements() {
		this.promptCanvasGroup = transform.Find("Canvas/Prompt").GetComponent<CanvasGroup>();
		this.promptIcon = transform.Find("Canvas/Prompt/Icon").GetComponent<Image>();
		this.promptText = transform.Find("Canvas/Prompt/Text").GetComponent<Text>();
	}

	/**
	 * Sync all prompt elements
	 */
	private void RefreshPrompt() {
		this.promptCanvasGroup.alpha = this.promptAlpha;
		this.promptIcon.sprite = this.icon;
		this.promptText.text = this.prompt.ToUpper();
	}
		
	/**
	 * Display prompt on hover
	 * @param on Whether to turn on focus or not
	 */
	public void Focus(bool on) {
		float endValue = on ? 1f : 0f;
		DOTween.To(() => this.promptAlpha, x => this.promptAlpha = x, endValue, this.animDuration);
	}
}
