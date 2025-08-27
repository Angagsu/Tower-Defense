using UnityEngine;

public class TouchSFXHandler : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip uIClickAudioClip;
    [SerializeField] private AudioClip touchGroudAudioClip;
    [SerializeField] private AudioClip touchWaterAudioClip;
    [SerializeField] private AudioClip touchUniqueEntityAudioClip;
    [SerializeField] private AudioClip touchSecondEntityAudioClip;

    private PlayerInputHandler playerInputHandler;

    private const int groundLayer = 6;
    private const int waterLayer = 4;
    private const int uIlayer = 5;
    private const int uniqueEntityLayer = 19;
    private const int secondEntityLayer = 18;
    

    private void Start()
    {
        playerInputHandler = ServiceLocator.GetService<PlayerInputHandler>();
        playerInputHandler.TouchPressed += OnPlayerInputHandler_TouchPressed;
    }

    private void OnDestroy()
    {
        playerInputHandler.TouchPressed -= OnPlayerInputHandler_TouchPressed;
    }

    private void OnPlayerInputHandler_TouchPressed(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            int layerIndex = raycastHit.collider.gameObject.layer;
            
            switch (layerIndex)
            { 
                case groundLayer:
                    //audioSource.PlayOneShot(touchGroudAudioClip);
                    break;
                case waterLayer:
                    audioSource.PlayOneShot(touchWaterAudioClip);
                    break;
                case uIlayer:
                    audioSource.PlayOneShot(uIClickAudioClip);
                    break;
                case uniqueEntityLayer:
                    audioSource.PlayOneShot(touchUniqueEntityAudioClip);
                    break;
                case secondEntityLayer:
                    audioSource.PlayOneShot(touchSecondEntityAudioClip);
                    break;
            }


        }
        
    }

    
}
