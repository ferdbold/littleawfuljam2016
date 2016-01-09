using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/**
 * Scene object the sloth can interact with
 */
public class Interactable : MonoBehaviour {

	private float _promptAlpha = 0f;

	private CanvasGroup _promptCanvasGroup;
	private Image _promptIcon;
	private Text _promptText;

	[Header("Prompt")]
	[SerializeField] private Sprite _icon;
	[SerializeField] private string _prompt;
	[SerializeField] private float _animDuration = 0.5f;

	public void Awake() {
		FindElements();
	}

	public void Start() {
		RefreshPrompt();
	}

	#if UNITY_EDITOR
	public void OnValidate() {
		FindElements();
		RefreshPrompt();
	}
	#endif

	public void Update() {
		if (Input.GetKey(KeyCode.F)) {
			Focus(true);
		}
		if (Input.GetKey(KeyCode.G)) {
			Focus(false);
		}

		RefreshPrompt();
	}

	/**
	 * Register all child elements
	 */
	private void FindElements() {
		_promptCanvasGroup = transform.Find("Canvas/Prompt").GetComponent<CanvasGroup>();
		_promptIcon = transform.Find("Canvas/Prompt/Icon").GetComponent<Image>();
		_promptText = transform.Find("Canvas/Prompt/Text").GetComponent<Text>();
	}

	/**
	 * Sync all prompt elements
	 */
	private void RefreshPrompt() {
		_promptCanvasGroup.alpha = _promptAlpha;
		_promptIcon.sprite = _icon;
		_promptText.text = _prompt.ToUpper();
	}
		
	/**
	 * Display prompt on hover
	 * @param on Whether to turn on focus or not
	 */
	public void Focus(bool on) {
		float endValue = on ? 1f : 0f;
		DOTween.To(() => _promptAlpha, x => _promptAlpha = x, endValue, _animDuration);
	}
}
