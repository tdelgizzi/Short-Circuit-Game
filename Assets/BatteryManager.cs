using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryManager : MonoBehaviour
{
    [SerializeField] GameObject batOneMark;
    [SerializeField] GameObject batTwoMark;
    [SerializeField] GameObject batThreeMark;
    [SerializeField] GameObject batFourMark;
    [SerializeField] GameObject batFiveMark;

    public int numBatteries = 0;

    // Start is called before the first frame update
    void Start()
    {
        EventBus.Subscribe<BatteryCollectedEvent>(AddBattery);
    }

    private void AddBattery(BatteryCollectedEvent e)
    {
        numBatteries += 1;
        if (numBatteries == 0)
        {
            batOneMark.SetActive(true);
        }
        if (numBatteries == 1)
        {
            batTwoMark.SetActive(true);
        }
        if (numBatteries == 2)
        {
            batThreeMark.SetActive(true);
        }
        if (numBatteries == 3)
        {
            batFourMark.SetActive(true);
        }
        if (numBatteries == 4)
        {
            batFiveMark.SetActive(true);
        }
    }
}
