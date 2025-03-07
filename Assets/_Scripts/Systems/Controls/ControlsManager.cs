using UnityEngine;
using UnityEngine.EventSystems;

public class ControlsManager : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("Editor controls")]
    [SerializeField] private bool editorTouch = false;
#endif

    [Header("Controls")]
    [ReadOnly] public bool fireDown;
    [ReadOnly] public bool fireUp;
    [ReadOnly] public Vector2 move;
    [ReadOnly] public bool power;

    [Header("Touch Controls")]
    public static bool hasTouch;
    [SerializeField] private GameObject touchUi;
    [SerializeField] private EventTrigger fireBtn;
    [SerializeField] private EventTrigger powerBtn;
    [SerializeField] private Joystick joystick;

    [Header("Tutorial")]
    [SerializeField] private GameObject tutorialMobile;
    [SerializeField] private GameObject tutorialPC;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        hasTouch = false;
    }

    private void OnEnable()
    {
        GameManager.GamePreStart += SetLevelTypeBtns;
    }

    private void OnDestroy()
    {
        GameManager.GamePreStart -= SetLevelTypeBtns;
    }

    [System.Obsolete]
    private void Start()
    {
#if Platform_Web && !UNITY_EDITOR
        Application.ExternalCall("CheckDevice");
#elif UNITY_EDITOR
        OnDeviceCheck(editorTouch ? 1 : 0);

#elif Platform_Mobile
        OnDeviceCheck(1);
#endif
    }

    private void SetLevelTypeBtns()
    {
        switch (RoundsController.Instance.levelType)
        {
            case RoundsController.LevelType.Normal:
                fireBtn.gameObject.SetActive(true);
                powerBtn.gameObject.SetActive(GameManager.Instance.gameplayScriptable.selectedPowerUp != null);

                if (GameManager.Instance.gameplayScriptable.selectedPowerUp != null)
                {
                    powerBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(-60, 260);
                }
                break;

            case RoundsController.LevelType.Infinite:
                fireBtn.gameObject.SetActive(false);
                powerBtn.gameObject.SetActive(GameManager.Instance.gameplayScriptable.selectedPowerUp != null);

                if (GameManager.Instance.gameplayScriptable.selectedPowerUp != null)
                {
                    powerBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(-60, 60);
                }
                break;
        }
    }

    // This method will be called from JavaScript for WebGl builds
    public void OnDeviceCheck(int isMobile)
    {
        hasTouch = isMobile == 1;
        touchUi.SetActive(hasTouch);

        if (hasTouch)
        {
            // fire btn
            EventTrigger.Entry fireEntryDown = new()
            {
                eventID = EventTriggerType.PointerDown
            };
            fireEntryDown.callback.AddListener((eventData) => { fireDown = true; fireUp = false; });
            fireBtn.triggers.Add(fireEntryDown);

            EventTrigger.Entry fireEntryUp = new()
            {
                eventID = EventTriggerType.PointerUp
            };
            fireEntryUp.callback.AddListener((eventData) => { fireDown = false; fireUp = true; });
            fireBtn.triggers.Add(fireEntryUp);


            // power btn
            EventTrigger.Entry powerEntryDown = new()
            {
                eventID = EventTriggerType.PointerDown
            };
            powerEntryDown.callback.AddListener((eventData) => power = true);
            powerBtn.triggers.Add(powerEntryDown);

            EventTrigger.Entry powerEntryUp = new()
            {
                eventID = EventTriggerType.PointerUp
            };
            powerEntryUp.callback.AddListener((eventData) => power = false);
            powerBtn.triggers.Add(powerEntryUp);
        }
    }

    public void OpenTutorial(bool on)
    {
        if (on)
        {
            if (hasTouch)
            {
                tutorialMobile.SetActive(true);
                tutorialPC.SetActive(false);
            }
            else
            {
                tutorialMobile.SetActive(false);
                tutorialPC.SetActive(true);
            }
        }

        UiManager.Instance.SetUi(UiType.Tutorial, on, 0.5f);
        UiManager.Instance.SetUi(UiType.Select, !on, 0.5f);
    }

    private void LateUpdate()
    {
        if (hasTouch)
        {
            move = joystick.InputDirection;

            if (fireDown == true)
                fireDown = false;

            if (fireUp == true)
                fireUp = false;

            if (power == true)
                power = false;
        }
        else
        {
            move.x = Input.GetAxisRaw(Inputs.Horizontal);
            move.y = Input.GetAxisRaw(Inputs.Vertical);

            fireDown = Input.GetButtonDown(Inputs.Fire);
            fireUp = Input.GetButtonUp(Inputs.Fire);

            power = Input.GetButtonDown(Inputs.Jump);
        }
    }
}