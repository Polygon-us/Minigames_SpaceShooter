using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : ShipBaseController
{
    public static PlayerController Instance;

    [Space]
    [SerializeField] private GameplayScriptable gameplayScriptable;

    [Space]
    [SerializeField] private Slider healthBar;

    [Header("Controllers")]
    public ControlsManager controls;
    public PlayerMovement movement;
    public PlayerShoot shoot;

    [ReadOnly] public bool copy = false;

    private int maxHealth = 0;

    public void Init(bool _copy = false)
    {
        copy = _copy;

        if (controls == null)
        {
            controls = Instance.controls;
        }

        if (!copy)
        {
            Instance = this;

            movement.Init(this, controls);
            shoot.Init(this, controls);
        }

        SetHealth(_properties.health);
        SetColor();
    }

    public void SetHealthUi(int _health)
    {
        maxHealth = _health;

        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
        healthNormalized = (float)health / maxHealth;
    }

    public void SetColor()
    {
        if (!copy)
        {
            renderer.material.SetColor(MaterialProperties.ColorCapsule, _properties.color);
            renderer.material.SetColor(MaterialProperties.Color, ConvertColor(_properties.color));
        }
        else
        {
            renderer.material = _properties.material;
            renderer.sprite = _properties.sprite;
        }

        module = engine1.main;
        module.startColor = _properties.color;
        module = engine2.main;
        module.startColor = _properties.color;
    }

    private Color ConvertColor(Color col)
    {
        Color.RGBToHSV(col, out float h, out float s, out float v);
        return Color.HSVToRGB(h, s * 0.25f, v);
    }

    public void SetHealth(int value)
    {
        health = value;
    }

    private void Update()
    {
        if (GameManager.Instance.isPlaying && gameObject.activeSelf)
        {
            movement.OnUpdate();
            shoot.OnUpdate();

            if (controls.power && GameManager.Instance.gameplayScriptable.selectedPowerUp != null)
            {
                PowerUpsManager.Instance.AddPowerUp(GameManager.Instance.gameplayScriptable.selectedPowerUp);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Bullet) && collision.GetComponent<Bullet>().bulletType == Bullet.TypeBullet.enemy)
        {
            collision.gameObject.SetActive(false);

            PostProcessingController.Instance.VolumePunch();
            VfxPool.Instance.InitVfx(collision.transform.position);

            if (!copy)
            {
                CollisionForce(gameplayScriptable.forceTime, new Vector2(transform.position.x - collision.transform.position.x, -gameplayScriptable.force * 0.02f)).Forget();
            }

            DoDamage();
        }
        else if (collision.CompareTag(Tags.Collectable))
        {
            collision.gameObject.SetActive(false);

            GameManager.Instance.UpCoins(GameManager.Instance.gameplayScriptable.coinValue);
        }
        else if (collision.CompareTag(Tags.Enemy) && collision.TryGetComponent(out EnemyController enemyController) && enemyController._properties.enemyCollision)
        {
            PostProcessingController.Instance.VolumePunch();
            VfxPool.Instance.InitVfx(collision.transform.position);

            enemyController.DoDamage();

            DoDamage();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Enemy) && collision.TryGetComponent(out EnemyController enemyController) && enemyController._properties.enemyCollision)
        {
            transform.position += new Vector3(
                (transform.position.x - collision.transform.position.x) * 0.5f,
                (transform.position.y - collision.transform.position.y) * 0.5f);
        }
    }

    public void DoDamage(int damage = 1)
    {
        PowerUpsManager.Player_Damage?.Invoke();

        if (health < 0)
        {
            return;
        }

        health -= damage;
        UpdateHealthUi();
        PostProcessingController.Instance.SetVolumeHealth(healthNormalized.Remap(0, 1, PostProcessingController.Instance.maxVignette, 0));

        if (health <= 0)
        {
            if (!copy)
            {
                GameManager.Instance.EndLevel();
                PostProcessingController.Instance.ScreenShake(0.5f).Forget();
                gameObject.SetActive(false);
            }
            else
            {
                PowerUpsManager.Instance.RemovePowerUp(PowerUpsManager.Instance.currentPowerUp);
                Destroy(gameObject);
            }
        }
    }

    public void UpdateHealthUi()
    {
        if (copy)
        {
            return;
        }

        healthNormalized = (float)health / maxHealth;
        healthBar.value = health;
    }

    private async UniTaskVoid CollisionForce(float _time, Vector3 direction)
    {
        float time = _time;

        while (time > 0 && this != null)
        {
            transform.localPosition += gameplayScriptable.force * Time.deltaTime * direction;
            movement.ClampPosition();
            await UniTask.Yield();

            time -= Time.deltaTime;
        }
    }
}