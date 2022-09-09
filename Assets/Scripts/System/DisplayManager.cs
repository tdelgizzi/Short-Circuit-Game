using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Inventory;
using UnityEngine.UI;

// Interface for setting all UI
public class DisplayManager : MonoBehaviour {
   
    public static DisplayManager Instance;

    [SerializeField] TextMeshProUGUI cycleText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Slider timerSlider;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Color timerColor;
    [SerializeField] Image timerFill;
    [SerializeField] RectTransform timerRect;


    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject dieScreen;
    [SerializeField] TextMeshProUGUI dieScreenText;
    [SerializeField] GameObject loseScreen;

    [SerializeField] InventoryUI inventory;
    [SerializeField] GameObject loadoutHelpMessage;
    private TextMeshProUGUI loadoutHelpText;
    [SerializeField] LoadoutUI loadout;

    [SerializeField] List<GameObject> healthBoxes;
    [SerializeField] GameObject shieldBox;

    [SerializeField] GameObject player;
    [SerializeField] BoxCollider weaponsRoom;

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        loadoutHelpText = loadoutHelpMessage.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        var playerInRoom = weaponsRoom.bounds.Contains(player.transform.position);
        var someLoadoutChoice = loadout.SomeChoiceExists();
        loadoutHelpMessage.SetActive(playerInRoom && someLoadoutChoice);
    }

    public void UpdateCycleUI(int cycles) {
        cycleText.text = cycles.ToString("00");
    }

    public void UpdateScoreUI(int score) {
        scoreText.text = score.ToString("00");
    }

    public void UpdateTimerUI(float time) {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        timerText.text = $"{(int)timeSpan.TotalMinutes}:{timeSpan.Seconds:00}";
        timerSlider.value = time / GameManager.Instance.GetMaxTime();

        if (timerSlider.value <= .2)
        {
            timerFill.color = Color.red;
            timerRect.localScale = new Vector3((1 + (.2f-timerSlider.value)), (1 + (.2f - timerSlider.value)), (.2f - timerSlider.value));
        }
        else
        {
            timerRect.localScale = new Vector3(1, 1, 1);
            timerFill.color = timerColor;
        }
    }

    public void HideUIText(bool hidden) {
        scoreText.enabled = !hidden;
        timerText.enabled = !hidden;
        cycleText.enabled = !hidden;
        loadoutHelpText.enabled = !hidden;
        timerText.transform.parent.gameObject.SetActive(!hidden);
    } 

    public void UpdateWinScreen(bool show) {
        winScreen.SetActive(show);
        HideUIText(show);
    }

    public void UpdateDieScreen(bool show, string reason)
    {
        dieScreen.SetActive(show);
        dieScreenText.text = reason;
        HideUIText(show);
    }

    public void UpdateLoseScreen(bool show) {
        loseScreen.SetActive(show);
        HideUIText(show);
    }

    public void UpdateInventoryUI(List<InventoryItem> items)
    {
        inventory.UpdateItems(items);
    }

    public void UpdateHealthUI(int health) {
        for (int i = 0; i < healthBoxes.Count; ++i) {
            GameObject healthIndicator = healthBoxes[i].transform.GetChild(0).gameObject; 
            if (i < health) {
                healthIndicator.SetActive(true);
            } else {
                healthIndicator.SetActive(false);
            }
        }
    }
    public void UpdatShieldUI(int shield)
    {
            GameObject shieldIndicator = shieldBox.transform.GetChild(0).gameObject;
            if (shield == 1)
            {
                shieldIndicator.SetActive(false);
            }
            else
                 {
            shieldIndicator.SetActive(true);
            }
    }

}