using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private bool _boolToContinue = false;
    private float _timeToContinue;
    private float customFloat = 0;
    private bool customBool = false;
    private Vector2 startPosition;

    public void Init(Enums.Behaviour properties, bool boolToContinue, float timeToContinue, int customInt)
    {
        _boolToContinue = boolToContinue;
        _timeToContinue = timeToContinue;

        switch (properties)
        {
            case Enums.Behaviour.Linear:
                break;

            case Enums.Behaviour.Direct:
                break;

            case Enums.Behaviour.Waves:
                LimitX();
                startPosition = transform.localPosition;
                break;

            case Enums.Behaviour.WavesDirect:
                LimitX();
                startPosition = transform.localPosition;
                break;

            case Enums.Behaviour.Diagonal:
                if (transform.localPosition.x > 0)
                {
                    transform.localEulerAngles = new Vector3(0, 0, 225);
                }
                else
                {
                    transform.localEulerAngles = new Vector3(0, 0, 135);
                }
                break;

            case Enums.Behaviour.Wave8:
                transform.localPosition = new Vector2(0, GameManager.BoundsLimits.y);
                startPosition = transform.localPosition;
                break;

            case Enums.Behaviour.Borders:
                if (customInt % 2 == 0)
                {
                    transform.localPosition = new Vector2(GameManager.PlayerLimits.x + 0.25f, transform.localPosition.y);
                    transform.localEulerAngles = new Vector3(0, 0, 90);
                }
                else
                {
                    transform.localPosition = new Vector2(-GameManager.PlayerLimits.x - 0.25f, transform.localPosition.y);
                    transform.localEulerAngles = new Vector3(0, 0, 270);
                }
                break;
        }

        customFloat = 0;
        customBool = false;
    }

    private void LimitX()
    {
        if (transform.localPosition.x > GameManager.InnerLimits.x)
        {
            transform.localPosition = new Vector2(GameManager.InnerLimits.x, transform.localPosition.y);
        }
        else if (transform.localPosition.x < -GameManager.InnerLimits.x)
        {
            transform.localPosition = new Vector2(-GameManager.InnerLimits.x, transform.localPosition.y);
        }
    }

    public void Move(ShipScriptable properties)
    {
        if (_boolToContinue && _timeToContinue > 0)
        {
            _timeToContinue -= Time.deltaTime;

            if (_timeToContinue < 0)
            {
                _timeToContinue = 0;
            }
        }

        switch (properties.behaviour)
        {
            case Enums.Behaviour.Linear:
                Linear(properties);
                break;

            case Enums.Behaviour.Direct:
                Direct(properties);
                break;

            case Enums.Behaviour.Waves:
                Waves(properties);
                break;

            case Enums.Behaviour.WavesDirect:
                WavesDirect(properties);
                break;

            case Enums.Behaviour.Diagonal:
                Diagonal(properties);
                break;

            case Enums.Behaviour.Wave8:
                Wave8(properties);
                break;

            case Enums.Behaviour.Borders:
                Borders(properties);
                break;

            case Enums.Behaviour.Chase:
                Chase(properties);
                break;
        }
    }

    private void Linear(ShipScriptable properties)
    {
        transform.position += properties.speed * Time.deltaTime * transform.up;
    }

    private void Direct(ShipScriptable properties)
    {
        if (transform.position.y > GameManager.EnemyLine || _timeToContinue == 0)
        {
            transform.position += properties.speed * Time.deltaTime * transform.up;
        }
    }

    private void Waves(ShipScriptable properties)
    {
        transform.position += properties.speed * Time.deltaTime * transform.up;

        transform.localPosition = new Vector2(
            startPosition.x + Mathf.Sin(Time.time * GameManager.HorizontalInvertedMultiplier) * properties.behaviourMathfSin * GameManager.HorizontalMultiplier,
            transform.localPosition.y);
    }

    private void WavesDirect(ShipScriptable properties)
    {
        if (transform.position.y > GameManager.EnemyLine || _timeToContinue == 0)
        {
            transform.position += properties.speed * Time.deltaTime * transform.up;
        }

        transform.localPosition = new Vector2(
            startPosition.x + Mathf.Sin(Time.time * GameManager.HorizontalInvertedMultiplier) * properties.behaviourMathfSin * GameManager.HorizontalMultiplier,
            transform.localPosition.y);
    }

    private void Diagonal(ShipScriptable properties)
    {
        if (transform.localEulerAngles.z == 135)
        {
            transform.position += properties.speed * Time.deltaTime * -transform.right;
        }
        else
        {
            transform.position -= properties.speed * Time.deltaTime * -transform.right;
        }
    }

    private void Wave8(ShipScriptable properties)
    {
        if (!customBool && transform.position.y > 1)
        {
            transform.position += properties.speed * Time.deltaTime * transform.up;
            transform.localPosition = new Vector2(startPosition.x + Mathf.Sin(Time.time) * properties.behaviourMathfSin, transform.localPosition.y);
        }
        else
        {
            customBool = true;
            transform.localPosition = new Vector2(
                startPosition.x + Mathf.Sin(Time.time) * properties.behaviourMathfSin,
                Mathf.Lerp(transform.localPosition.y, 0.5f + Mathf.Sin(Time.time * 1.5f) * properties.behaviourMathfSin / 2, customFloat));
        }

        if (customBool && customFloat < 1 && customFloat >= 0)
        {
            customFloat += Time.deltaTime / 4;
        }
    }

    private void Borders(ShipScriptable properties)
    {
        if (transform.localEulerAngles.z == 90)
        {
            transform.position += properties.speed * Time.deltaTime * -transform.right;
        }
        else
        {
            transform.position += properties.speed * Time.deltaTime * transform.right;
        }
    }

    private void Chase(ShipScriptable properties)
    {
        transform.position += properties.speed * Time.deltaTime * transform.up;

        //if (transform.position.y < PlayerController.Instance.transform.position.y)
        //{
        //    return;
        //}

        if (transform.position.x > PlayerController.Instance.transform.position.x)
        {
            transform.position = new Vector2(transform.position.x - (properties.speed * Time.deltaTime * properties.behaviourMathfSin), transform.position.y);
        }
        else if (transform.position.x < PlayerController.Instance.transform.position.x)
        {
            transform.position = new Vector2(transform.position.x + (properties.speed * Time.deltaTime * properties.behaviourMathfSin), transform.position.y);
        }
    }
}