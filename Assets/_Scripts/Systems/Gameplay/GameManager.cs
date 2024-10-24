using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool hasStarted = false;
    public bool isPlaying = false;
    public int leftForNextGroup = 0;

    [Header("Score")]
    public int scoreCoin = 5;
    public int scoreEnemy = 8;

    [Space]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI endScore;
    private int score = 0;
    private const string preScore = "Score: ";
    private const string postcore = "Final score:\n ";

    [Header("Ui")]
    [SerializeField] private GameObject shadowBorders;

    [Header("Screen Properties")]
    [SerializeField] private float innerLimit = -1.6f;
    private Vector2 innerLimits = Vector2.zero;
    public static Vector2 InnerLimits => Instance.innerLimits;

    [SerializeField] private float playerLimit = -0.4f;
    private Vector2 playerLimits = Vector2.zero;
    public static Vector2 PlayerLimits => Instance.playerLimits;

    [SerializeField] private float bulletLimit = 0.1f;
    private Vector2 bulletLimits = Vector2.zero;
    public static Vector2 BulletLimits => Instance.bulletLimits;

    [SerializeField] private float boundsLimit = 0.5f;
    private Vector2 boundsLimits = Vector2.zero;
    public static Vector2 BoundsLimits => Instance.boundsLimits;

    [SerializeField] private float enemyLine = 1.55f;
    private float enemyLineLimit;
    public static float EnemyLine => Instance.enemyLineLimit;

    private float horizontalMultiplier = 1f;
    public static float HorizontalMultiplier => Instance.horizontalMultiplier;
    private float horizontalInvertedMultiplier = 1f;
    public static float HorizontalInvertedMultiplier => Instance.horizontalInvertedMultiplier;

    [Space]
    [SerializeField] private RectTransform uiRect;
    [SerializeField] private Camera mainCamera;

    [Space]
    [SerializeField, ColorUsage(true, true)] private Color[] colors;

    private const string _Cancel = "Cancel";

    [Header("Managers")]
    [SerializeField] private UiManager uiManager;
    [SerializeField] private RoundsController roundsController;
    [SerializeField] private ControlsManager controlsManager;
    [SerializeField] private PostProcessingController postProcessingController;
    [SerializeField] private AudioManager audioManager;

    private void OnDrawGizmosSelected()
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(uiRect, uiRect.position, mainCamera, out Vector3 canvasBorders);

        // Inner Limits
        Gizmos.color = Color.blue;
        Gizmos.DrawLineList(new Vector3[8]
            {
            new(-(canvasBorders.x - innerLimit), canvasBorders.y - innerLimit),
            new(canvasBorders.x - innerLimit, canvasBorders.y - innerLimit),

            new(-(canvasBorders.x - innerLimit), -(canvasBorders.y - innerLimit)),
            new(canvasBorders.x - innerLimit, -(canvasBorders.y - innerLimit)),

            new(-(canvasBorders.x - innerLimit), -(canvasBorders.y - innerLimit)),
            new(-(canvasBorders.x - innerLimit), canvasBorders.y - innerLimit),

            new(canvasBorders.x - innerLimit, -(canvasBorders.y - innerLimit)),
            new(canvasBorders.x - innerLimit, canvasBorders.y - innerLimit)
            });


        // Player Limits
        Gizmos.color = Color.cyan;
        Gizmos.DrawLineList(new Vector3[8]
            {
            new(-(canvasBorders.x - playerLimit), canvasBorders.y - playerLimit),
            new(canvasBorders.x - playerLimit, canvasBorders.y - playerLimit),

            new(-(canvasBorders.x - playerLimit), -(canvasBorders.y - playerLimit)),
            new(canvasBorders.x - playerLimit, -(canvasBorders.y - playerLimit)),

            new(-(canvasBorders.x - playerLimit), -(canvasBorders.y - playerLimit)),
            new(-(canvasBorders.x - playerLimit), canvasBorders.y - playerLimit),

            new(canvasBorders.x - playerLimit, -(canvasBorders.y - playerLimit)),
            new(canvasBorders.x - playerLimit, canvasBorders.y - playerLimit)
            });


        // Bullet Limits
        Gizmos.color = Color.red;
        Gizmos.DrawLineList(new Vector3[8]
            {
            new(-(canvasBorders.x - bulletLimit), canvasBorders.y - bulletLimit),
            new(canvasBorders.x - bulletLimit, canvasBorders.y - bulletLimit),

            new(-(canvasBorders.x - bulletLimit), -(canvasBorders.y - bulletLimit)),
            new(canvasBorders.x - bulletLimit, -(canvasBorders.y - bulletLimit)),

            new(-(canvasBorders.x - bulletLimit), -(canvasBorders.y - bulletLimit)),
            new(-(canvasBorders.x - bulletLimit), canvasBorders.y - bulletLimit),

            new(canvasBorders.x - bulletLimit, -(canvasBorders.y - bulletLimit)),
            new(canvasBorders.x - bulletLimit, canvasBorders.y - bulletLimit)
            });


        // Bounds Limits
        Gizmos.color = Color.blue + Color.red;
        Gizmos.DrawLineList(new Vector3[8]
            {
            new(-(canvasBorders.x - boundsLimit), canvasBorders.y - boundsLimit),
            new(canvasBorders.x - boundsLimit, canvasBorders.y - boundsLimit),

            new(-(canvasBorders.x - boundsLimit), -(canvasBorders.y - boundsLimit)),
            new(canvasBorders.x - boundsLimit, -(canvasBorders.y - boundsLimit)),

            new(-(canvasBorders.x - boundsLimit), -(canvasBorders.y - boundsLimit)),
            new(-(canvasBorders.x - boundsLimit), canvasBorders.y - boundsLimit),

            new(canvasBorders.x - boundsLimit, -(canvasBorders.y - boundsLimit)),
            new(canvasBorders.x - boundsLimit, canvasBorders.y - boundsLimit)
            });


        // Enemy Line
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector2(canvasBorders.x, canvasBorders.y - (enemyLine * canvasBorders.y)), new Vector2(-canvasBorders.x, canvasBorders.y - (enemyLine * canvasBorders.y)));
    }

    public void SelectColor(int value)
    {
        PlayerController.Instance._properties.color = colors[value];
        PlayerController.Instance.SetColor();
    }

    public void StartGame()
    {
        hasStarted = true;
        isPlaying = true;
        Time.timeScale = 1;

        UiManager.Instance.SetUi(UiType.Gameplay, true);
        UiManager.Instance.SetUi(UiType.Select, false, 1);

        RoundsController.Instance.StartRound();

        AudioManager.Instance.PlaySound(AudioManager.AudioType.Start, 2f);
    }

    public void GodMode(bool on)
    {
        PlayerController.Instance.SetHealth(on ? 1000000 : 1);
    }

    private void Awake()
    {
        PlayerLoopSystem loop = PlayerLoop.GetCurrentPlayerLoop();
        PlayerLoopHelper.Initialize(ref loop, InjectPlayerLoopTimings.Minimum);
        //PlayerLoopHelper.DumpCurrentPlayerLoop();

        Instance = this;

        uiManager.Init();
        roundsController.Init();
        //controlsManager.Init();
        audioManager.Init();
        postProcessingController.Init();

        scoreText.SetText(preScore + score);

        Time.timeScale = 1;
    }

    private void Update()
    {
        UpdateBorders();

        if (hasStarted)
        {
            if (Input.GetButtonDown(_Cancel))
            {
                Pause();
            }

            if (isPlaying)
            {
                if (leftForNextGroup == 0)
                {
                    RoundsController.Instance.StartGroup();
                }

                //DebugMode
                if (Input.GetKeyDown(KeyCode.P))
                {
                    leftForNextGroup = 0;
                }
            }
        }
    }

    private void UpdateBorders()
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(uiRect, uiRect.position, mainCamera, out Vector3 canvasBorders);

        innerLimits  = new Vector2(-canvasBorders.x + innerLimit,  -canvasBorders.y + innerLimit);
        playerLimits = new Vector2(-canvasBorders.x + playerLimit, -canvasBorders.y + playerLimit);
        bulletLimits = new Vector2(-canvasBorders.x + bulletLimit, -canvasBorders.y + bulletLimit);
        boundsLimits = new Vector2(-canvasBorders.x + boundsLimit, -canvasBorders.y + boundsLimit);

        enemyLineLimit = canvasBorders.y - (enemyLine * canvasBorders.y);
        horizontalMultiplier = mainCamera.pixelWidth / 1280f;
        horizontalInvertedMultiplier = 1280f / mainCamera.pixelWidth;

        // black gradient borders to fix wider aspect ratios
        shadowBorders.SetActive((mainCamera.pixelWidth / (float)mainCamera.pixelHeight) > 1.94f);
    }

    public void UpScore(int value)
    {
        if (isPlaying)
        {
            score += value;
            scoreText.SetText(preScore + score);

            if (value == scoreCoin)
            {
                AudioManager.Instance.PlaySound(AudioManager.AudioType.Coin);
            }
        }
    }

    public void EndLevel()
    {
        if (isPlaying)
        {
            isPlaying = false;
            hasStarted = false;
            UiManager.Instance.SetUi(UiType.End, true, 1, () => UiManager.Instance.SetUi(UiType.Gameplay, false));

            endScore.SetText(postcore + score);
            AudioManager.Instance.PlaySound(AudioManager.AudioType.End, 2.5f);

            Time.timeScale = 0.5f;

            leftForNextGroup = -1;
        }
    }

    public void Pause()
    {
        if (isPlaying)
        {
            UiManager.Instance.SetUi(UiType.Pause, true);
            Time.timeScale = 0;
            isPlaying = false;
        }
        else
        {
            UiManager.Instance.SetUi(UiType.Pause, false);
            Time.timeScale = 1;
            isPlaying = true;
        }
    }

    public void Retry()
    {
        UiManager.Instance.SetUi(UiType.Fade, true, 0.25f, () => SceneManager.LoadScene(0));
    }

    public void Exit()
    {
        Application.Quit();
    }
}