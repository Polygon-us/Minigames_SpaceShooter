using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class PowerUpBase : ScriptableObject
{
    [HideInInspector] public ShipBaseController shipBase;

    [Header("Descriptions")]
    public PowerUpsManager.PowerUpType type;
    public string powerName;
    public Sprite sprite;
    [TextArea] public string description;

    [Header("Prices")]
    public uint currentAmount = 0;
    //public uint level = 1;
    public uint cost = 10;

    [Header("Properties")]
    public bool useDuration = false;
    public Vector2 durationRange = new(-1f, -1f);
    [ReadOnly] public float duration = -1f;

    [Header("Audios")]
    public AudioPart clip_activate;
    public AudioPart clip_constant;
    public AudioPart clip_deactivate;
    public bool waitForActivate = false;
    public AudioPart clip_shoot;
    public AudioPart clip_playerDamage;
    public AudioPart clip_enemyDamage;
    public AudioPart clip_enemyDeath;

    public float InitDuration()
    {
        return duration = Random.Range(durationRange.x, durationRange.y);
    }

    public abstract void OnActivate();

    public async UniTaskVoid CountDuration()
    {
        while (duration > 0 && shipBase != null)
        {
            duration -= Time.deltaTime;

            if (duration <= 0)
            {
                PowerUpsManager.Instance.RemovePowerUp(this);
                break;
            }

            await UniTask.Yield();
        }
    }

    public abstract void OnDeactivate();

    public abstract void OnPlayerShoot();

    public abstract void OnPlayerDamage();

    public abstract void OnEnemyDamage(ShipBaseController enemy);

    public abstract void OnEnemyDeath(ShipBaseController enemy);

    public abstract void OnGameUpdate();

    public virtual void Dispose()
    {
        shipBase = null;
    }
}