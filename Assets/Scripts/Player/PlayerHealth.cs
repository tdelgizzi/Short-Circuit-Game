using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : HasHealth {

    private PlayerShield shield;
    private bool invincible = false;
    [SerializeField] private float invicibilityTime;
    [SerializeField] private int invicibilityFlashes;

    [SerializeField] GameObject shieldEffect;
    [SerializeField] GameObject hitEffect;

    protected override void Start() {
        base.Start();
        shield = this.GetComponent<PlayerShield>();
    }

    protected override void CheckHealth() {
        base.CheckHealth();

        if (base.currentHP <= 0) {
            GetComponent<PlayerDeath>().Die(PlayerDiedAnalyticsEvent.Reason.Shot);
        }
    }

    public override void UpdateHealth(int delta) {
        if (invincible)
        {
            return;
        }
        if (shield && shield.IsShieldActive()) {
            // Shield takes first hit
            shield.SetShieldActive(false);
            StartCoroutine(MakeInvincible(invicibilityTime));
            StartCoroutine(InvincibilityFlash(invicibilityTime, invicibilityFlashes));
            GameObject shieldParticles  = GameObject.Instantiate(shieldEffect);
            shieldParticles.transform.position = this.transform.position;
            return;
        }

        GameObject hitParticles  = GameObject.Instantiate(hitEffect);
        hitParticles.transform.position = this.transform.position;
        base.UpdateHealth(delta);
        if (base.currentHP > 0)
        {
            StartCoroutine(MakeInvincible(invicibilityTime));
            StartCoroutine(InvincibilityFlash(invicibilityTime, invicibilityFlashes));
        }
        DisplayManager.Instance.UpdateHealthUI(currentHP);
    }

    public override void ResetHealth() {
        base.ResetHealth();
        DisplayManager.Instance.UpdateHealthUI(currentHP);
    }

    IEnumerator MakeInvincible(float invTime)
    {
        invincible = true;

        yield return new WaitForSeconds(invTime);

        invincible = false;
    }

    IEnumerator InvincibilityFlash(float invTime, int flashCount)
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < flashCount; i++)
        {
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                Color oldColor = spriteRenderer.color;
                Color newColor = new Color(oldColor.r, oldColor.g, oldColor.r, 0.2f);
                spriteRenderer.color = newColor;
            }
            yield return new WaitForSeconds(invTime / (flashCount * 2));
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                Color oldColor = spriteRenderer.color;
                Color newColor = new Color(oldColor.r, oldColor.g, oldColor.r, 1.0f);
                spriteRenderer.color = newColor;
            }
            yield return new WaitForSeconds(invTime / (flashCount * 2));
        }
    }
}
