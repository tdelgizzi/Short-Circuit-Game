using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasHealth : MonoBehaviour {
    [SerializeField] protected int startingHP;
    [SerializeField] protected string deathClip;
    [SerializeField] protected string hurtClip;
    protected int currentHP;

    protected virtual void Start() {
        ResetHealth();
    }

    protected virtual void CheckHealth() {
        if (currentHP <= 0) {
            AudioManager.PlayClipAtPoint(transform.position, deathClip);
        }
    }

    public virtual void UpdateHealth(int delta) {
        currentHP += delta;
        CheckHealth();
        AudioManager.PlayClipAtPoint(transform.position, hurtClip);
    }

    public virtual void ResetHealth() {
        currentHP = startingHP;
    }
}
