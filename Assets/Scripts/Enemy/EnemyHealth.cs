using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : HasHealth {

    [SerializeField] GameObject deathEffect;
    [SerializeField] GameObject hitEffect;

    protected override void Start() {
        base.Start();
    }

    protected override void CheckHealth() {
        base.CheckHealth();

        if (base.currentHP <= 0) {
            this.gameObject.SetActive(false);
            EventBus.PublishAnalyticsEvent(new EnemyKilledAnalyticsEvent(gameObject));
            GameObject particles  = GameObject.Instantiate(deathEffect);
            particles.transform.position = this.transform.position;
        }
    }

    public override void UpdateHealth(int delta) {
        base.UpdateHealth(delta);
        GameObject particles  = GameObject.Instantiate(hitEffect);
        particles.transform.position = this.transform.position;
    }

    public override void ResetHealth() {
        base.ResetHealth();
    }
}
