using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerShield : MonoBehaviour {

    [SerializeField] float rechargeTime;

    bool shieldActive = true;
    Material material;
    float initShieldSize;
    Light2D playerLight;
    float initIntensity;

    void Start() {
        material = this.GetComponent<SpriteRenderer>().material;
        initShieldSize = material.GetFloat("_ShieldSize");
        playerLight = this.GetComponentInChildren<Light2D>();
        initIntensity = playerLight.intensity;
        SetShieldActive(true);
    }

    private void OnEnable()
    {
        if (!shieldActive) SetShieldActive(true);
    }

    public void SetShieldActive(bool on) {
        shieldActive = on;

        if (on) {
            material.SetFloat("_ShieldSize", initShieldSize);
            AudioManager.PlayClipNow("Shield Activate");
            playerLight.intensity = initIntensity;
            DisplayManager.Instance.UpdatShieldUI(0);
        } else {
            material.SetFloat("_ShieldSize", 0);
            AudioManager.PlayClipNow("Shield Deactivate");
            StartCoroutine(ActivateShieldCoroutine());
            playerLight.intensity = 0.5f;
            DisplayManager.Instance.UpdatShieldUI(1);
        }
    }

    public bool IsShieldActive() {
        return shieldActive;
    }

    IEnumerator ActivateShieldCoroutine() {
        yield return new WaitForSeconds(rechargeTime);
        SetShieldActive(true);
    }
}
