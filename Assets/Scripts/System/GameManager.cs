using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Encapsulates win, score, timer, cycles
public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [SerializeField] float initialTimeRemaining;
    [SerializeField] int scoreToWin;
    [SerializeField] GameObject player;
    [SerializeField] BoxCollider WeaponsRoom;
    [SerializeField] Robots robots;

    float timeRemaining;
    float startTime = 0;
    int currentCycle = 1;
    int score = 0;
    int robotsRemaining = 4;
    PlayerRespawn playerRespawn;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        playerRespawn = player.GetComponent<PlayerRespawn>();
    }

    void Start()
    {
        startTime = Time.time;
        ResetTime();
        ResetCycle();
        ResetScore();
    }

    void Update()
    {
        // Insert code to update

        if (!PauseManager.Instance.IsPaused()) UpdateTime(-Time.deltaTime);
    }

    // ======================== Cycles ========================
    public int GetCurrentCycle()
    {
        return currentCycle;
    }

    public void IncrementCycle()
    {
        currentCycle += 1;
        DisplayManager.Instance.UpdateCycleUI(currentCycle);
    }

    public void ResetCycle()
    {
        currentCycle = 1;
        DisplayManager.Instance.UpdateCycleUI(currentCycle);
    }

    // ======================== Score ========================
    public int GetScore()
    {
        return score;
    }

    public void IncrementScore()
    {
        score += 1;
        DisplayManager.Instance.UpdateScoreUI(score);
        EventBus.Publish(new BatteryCollectedEvent());
        if (score >= scoreToWin)
        {
            EndGame(true);
        }
    }

    public void ResetScore()
    {
        score = 0;
        DisplayManager.Instance.UpdateScoreUI(score);
    }

    // ======================== Robot Lives ========================
    public int GetRobots()
    {
        return robotsRemaining;
    }

    public void RemoveRobot()
    {
        robotsRemaining -= 1;

        // TODO: update UI
        // DisplayManager.Instance.UpdateScoreUI(score);
    }

    public void ResetRobots()
    {
        score = 0;
        // TODO: update UI
        DisplayManager.Instance.UpdateScoreUI(score);
    }

    // ======================== Time ========================
    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

    private bool playerHasLeftRoom = false;
    private bool lowTimeWarning = false;
    public void UpdateTime(float delta)
    {
        var playerInRoom = WeaponsRoom.bounds.Contains(player.transform.position);
        if (!playerInRoom) playerHasLeftRoom = true;
        if (playerInRoom && !playerHasLeftRoom) return;

        timeRemaining += delta;
        if (timeRemaining <= 0)
        {
            if (playerInRoom)
            {
                StartNextRun(false);
            }
            else
            {
                PauseManager.Instance.SetPauseState(true);
                player.GetComponent<PlayerDeath>().Die(PlayerDiedAnalyticsEvent.Reason.Time);
            }
        }
        else
        {
            DisplayManager.Instance.UpdateTimerUI(timeRemaining);
        }

        if (timeRemaining <= 30 && !lowTimeWarning)
        {
            string msg = "You only have 30 Seconds left of power, get back to the starting room unless you want to lose this robot.";
            float dur = 5.5f;
            string clip = "AI_M_5";

            EventBus.Publish<ToastEvent>(new ToastEvent(msg, dur, clip));
            lowTimeWarning = true;
        }
    }

    public void ResetTime()
    {
        timeRemaining = initialTimeRemaining;
        DisplayManager.Instance.UpdateTimerUI(timeRemaining);
        playerHasLeftRoom = false;
        lowTimeWarning = false;
    }

    public float GetMaxTime()
    {
        return initialTimeRemaining;
    }

    public float GetStartTime() {
        return startTime;
    }

    // ======================== Win/Loss ========================
    public void EndGame(bool victory)
    {

        if (victory)
        {
            EventBus.PublishAnalyticsEvent(new GameWonAnalyticsEvent(currentCycle, robotsRemaining));
            EndScene endScene = this.gameObject.GetComponent<EndScene>();
            endScene.Initiate();
        }
        else
        {
            PauseManager.Instance.SetPauseState(true);
            EventBus.PublishAnalyticsEvent(new GameLostAnalyticsEvent(currentCycle, robotsRemaining, score));
            DisplayManager.Instance.UpdateLoseScreen(true);
        }
    }

    public void StartNextRun(bool died)
    {
        IncrementCycle();
        ResetTime();
        if (died)
        {
            PauseManager.Instance.SetPauseState(false);
            robots.UseNextRobot();
            playerRespawn.RespawnPlayer(robots.SpawnPosition());
            DisplayManager.Instance.UpdateDieScreen(false, "");
        }
    }

    public void ResetGame()
    {
        startTime = Time.time;
        DisplayManager.Instance.UpdateWinScreen(false);
        DisplayManager.Instance.UpdateLoseScreen(false);
        ResetScore();
        ResetTime();
        ResetCycle();
        ResetRobots();
    }

}
