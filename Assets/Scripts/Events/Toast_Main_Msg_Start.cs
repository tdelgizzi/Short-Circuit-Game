using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toast_Main_Msg_Start : MonoBehaviour
{
    private string msg1 = "This is not a simulation, We have been boarded by an unknown enemy, with biological DNA not in our databases.";
    private string msg2 = "The enemy has appeared to have removed the 5 Cores from the Reactor.";
    private string msg3 = "You must return these or everyone on board will die.";
    private string msg4 = "Unlike the simulations, the robot you command only has a short amount of power that you can use.You also can die of damage if your shields fail.";


    [SerializeField]
    private float dur1;
    [SerializeField]
    private float dur2;
    [SerializeField]
    private float dur3;
    [SerializeField]
    private float dur4;

    // Start is called before the first frame update
    void Start()
    {
        //EventBus.Publish<ToastEvent>(new ToastEvent(msg1, dur1, "AI_M_1"));
        EventBus.Publish<ToastEvent>(new ToastEvent(msg2, dur2, "AI_M_2"));
        EventBus.Publish<ToastEvent>(new ToastEvent(msg3, dur3, "AI_M_3"));
        EventBus.Publish<ToastEvent>(new ToastEvent(msg4, dur4, "AI_M_4"));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
