using System.Collections;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public Player player;
    Light shield;
    CircleCollider2D shieldCollider;

    private bool shieldActive = false;
    private bool bulletBuffActive = false;
    public bool BulletBuffActive
    {
        get
        {
            return bulletBuffActive;
        }
    }

    private void Awake()
    {
        shield = player.GetComponentInChildren<Light>();
        shieldCollider = shield.GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        PowerUps.HealthBuff += HealthBuff;
        PowerUps.ShieldBuff += ShieldBuff;
        PowerUps.BulletBuff += BulletBuff;
    }

    private void OnDestroy()
    {
        PowerUps.HealthBuff -= HealthBuff;
        PowerUps.ShieldBuff -= ShieldBuff;
        PowerUps.BulletBuff -= BulletBuff;
    }

    private void HealthBuff()
    {
        player.AddLife();
    }

    private void ShieldBuff()
    {
        StartCoroutine(ShieldDuration());
    }
    IEnumerator ShieldDuration()
    {
        float fade = 0.01f;
        float blinkFade = 0.0025f;
        float blinkDuration = 0.25f;
        float blink = (blinkFade * 200) + (blinkDuration * 2);
        float duration = 10 - (fade * 200) - blink;

        if (shieldActive == false)
        {
            shieldActive = true;
            player.Shield = true;
            shieldCollider.enabled = true;
            for (int i = 0; i < 100; i++) // 1 second
            {
                if (shieldCollider.radius < 1.75f)
                {
                    shieldCollider.radius += .005f;
                }
                if (shield.range < 3)
                {
                    shield.range += .01f;
                }
                shield.intensity += .005f;
                if (shield.color != new Color32(0, 191, 255, 1))
                {
                    shield.color = new Color32(0, 191, 255, 1);
                }
                yield return new WaitForSeconds(fade);
            }

            for (int i = 0; i < duration; i++)
            {
                yield return new WaitForSeconds(1);
            }

            for (int i = 0; i < 2; i++) // 0.5 seconds
            {
                for (int j = 0; j < 100; j++) // 0.25 seconds
                {
                    if (shield.intensity > .5f)
                    {
                        shield.intensity -= .01f;
                    }
                    yield return new WaitForSeconds(blinkFade);
                }
                for (int j = 0; j < 100; j++) // 0.25 seconds
                {
                    if (shield.intensity < 1)
                    {
                        shield.intensity += .01f;
                    }
                    yield return new WaitForSeconds(blinkFade);
                }
                yield return new WaitForSeconds(blinkDuration);
            }

            for (int i = 0; i < 100; i++) // 1 seconds
            {
                if (shield.range > 2)
                {
                    shield.range -= .01f;
                }
                if (shield.intensity > .5f)
                {
                    shield.intensity -= .005f;
                }
                if (shieldCollider.radius > 1.25f)
                {
                    shieldCollider.radius -= .005f;
                }
                yield return new WaitForSeconds(fade);
            }
            shield.color = Color.white;
            shieldCollider.enabled = false;
            player.Shield = false;
            shieldActive = false;
        }
    }

    private void BulletBuff()
    {
        StartCoroutine(BulletDuration());
    }
    IEnumerator BulletDuration()
    {
        int duration = 10;

        bulletBuffActive = true;
        for (int i = 0; i < duration; i++)
        {
            yield return new WaitForSeconds(1);
        }
        bulletBuffActive = false;
    }
}
