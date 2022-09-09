using UnityEngine;

public class Reactor : MonoBehaviour
{
    [SerializeField] GameObject batOne;
    [SerializeField] GameObject batTwo;
    [SerializeField] GameObject batThree;
    [SerializeField] GameObject batFour;
    [SerializeField] GameObject batFive;

    [SerializeField] string msg1;
    [SerializeField] float dur1;
    [SerializeField] string clip1;
    [SerializeField] string msg2;
    [SerializeField] float dur2;
    [SerializeField] string clip2;
    [SerializeField] string msg3;
    [SerializeField] float dur3;
    [SerializeField] string clip3;
    [SerializeField] string msg4;
    [SerializeField] float dur4;
    [SerializeField] string clip4;
    [SerializeField] string msg5;
    [SerializeField] float dur5;
    [SerializeField] string clip5;

    private GameObject[] batteryObjects;
    public int numBatteries = 0;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        batteryObjects = new GameObject[] {
            batOne, batTwo, batThree, batFour, batFive,
        };

        UpdateSprite();

        EventBus.Subscribe<BatteryCollectedEvent>(AddBattery);
    }

    private void UpdateSprite()
    {
        if (numBatteries == 1)
        {
            EventBus.Publish<ToastEvent>(new ToastEvent(msg1, dur1, clip1));
        }
        if (numBatteries == 2)
        {
            EventBus.Publish<ToastEvent>(new ToastEvent(msg2, dur2, clip2));
        }
        if (numBatteries == 3)
        {
            EventBus.Publish<ToastEvent>(new ToastEvent(msg3, dur3, clip3));
        }
        if (numBatteries == 4)
        {
            EventBus.Publish<ToastEvent>(new ToastEvent(msg4, dur4, clip4));
        }
        if (numBatteries == 5)
        {
            EventBus.Publish<ToastEvent>(new ToastEvent(msg5, dur5, clip5));
        }


        for (int i = 0; i < batteryObjects.Length; i++)
        {
            if (i < numBatteries)
                batteryObjects[i].SetActive(true);
            else
                batteryObjects[i].SetActive(false);
        }    
    }

    private void AddBattery(BatteryCollectedEvent e)
    {
        numBatteries += 1;
        UpdateSprite();
    }
}
