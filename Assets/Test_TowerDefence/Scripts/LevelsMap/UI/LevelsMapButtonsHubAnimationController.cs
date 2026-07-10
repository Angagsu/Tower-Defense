using UnityEngine;
using UnityEngine.UI;

public class LevelsMapButtonsHubAnimationController : MonoBehaviour
{
    [SerializeField] private Animator buttonsHubAnimator;
    [SerializeField] private Button knobButton;

    private bool toggleState;
    private int toggleHash;



    private void Awake()
    {
        toggleHash = Animator.StringToHash("Close");

        knobButton.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        toggleState = !toggleState;

        buttonsHubAnimator.SetBool(toggleHash, toggleState);
    }

    private void OnDestroy()
    {
        knobButton.onClick.RemoveAllListeners();
    }
}

