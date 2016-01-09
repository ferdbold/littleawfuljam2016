using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// Scene object the sloth can interact with
/// </summary>
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
			SendMessage("Focus");
		}
		if (Input.GetKey(KeyCode.G)) {
			SendMessage("Blur");
		}

		RefreshPrompt();
	}

	/// <summary>
	/// Register all child elements
	/// </summary>
	private void FindElements() {
		_promptCanvasGroup = transform.Find("Canvas/Prompt").GetComponent<CanvasGroup>();
		_promptIcon = transform.Find("Canvas/Prompt/Icon").GetComponent<Image>();
		_promptText = transform.Find("Canvas/Prompt/Text").GetComponent<Text>();
	}

	/// <summary>
	/// Sync all prompt elements
	/// </summary>
	private void RefreshPrompt() {
		_promptCanvasGroup.alpha = _promptAlpha;
		_promptIcon.sprite = _icon;
		_promptText.text = _prompt.ToUpper();
	}

	/// <summary>
	/// Change prompt visibility
	/// </summary>
	/// <param name="on">The new prompt visibility</param>
	public void TogglePrompt(bool on) {
		float endValue = on ? 1f : 0f;
		DOTween.To(() => _promptAlpha, x => _promptAlpha = x, endValue, _animDuration).SetEase(Ease.OutCubic);
	}

	/// <summary>
	/// Show the prompt
	/// </summary>
	public void Focus() { TogglePrompt(true); }

	/// <summary>
	/// Hide the prompt
	/// </summary>
	public void Blur() { TogglePrompt(false); }
}
