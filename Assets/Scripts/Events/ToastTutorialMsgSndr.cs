using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastTutorialMsgSndr : MonoBehaviour
{
    // Start is called before the first frame update
    private string msg1 = "Hello passenger 28W. This is a training simulation, you have been chosen at random to defend the ship incase of emergency.";
    private string msg2 = "My name is Hermes and I am the ships AI. As per the Elon Musk Accords Of 2045, AI are no longer permitted to use weapons.";
    private string msg3 = "If there is an emergency, you will be able to control a MkZ10 battle bot via neural connection from your cryopod.";
    private string msg4 = "We are in a 0 G environment, so traditional movement is not viable.";
    private string msg5 = "Please press space to use your grapple hook to move out of the charging chamber";
    private string msg6 = "Good job! Due to Newton’s third law, if you use a mass driver to shoot in one direction, the recoil will propel you in the opposite.";
    private string msg7 = "Try this alternate movement option by right clicking to use your pistol.";
    private string msg8 = "Try navigating to the other end of this simulation chamber to get a feel for the movement.";
    [SerializeField]
    private float dur1;
    [SerializeField]
    private float dur2;
    [SerializeField]
    private float dur3;
    [SerializeField]
    private float dur4;
    [SerializeField]
    private float dur6;
    [SerializeField]
    private float dur7;
    [SerializeField]
    private float dur8;
    void Start()
    {
        EventBus.Publish<ToastEvent>(new ToastEvent(msg1, dur1, "AI_T_1"));
        EventBus.Publish<ToastEvent>(new ToastEvent(msg2, dur2, "AI_T_2"));
        EventBus.Publish<ToastEvent>(new ToastEvent(msg3, dur3, "AI_T_3"));
        EventBus.Publish<ToastEvent>(new ToastEvent(msg4, dur4, "AI_T_4"));
        EventBus.Publish<ToastEvent>(new ToastEvent(msg5, KeyCode.Space, "AI_T_5"));
        EventBus.Publish<ToastEvent>(new ToastEvent(msg6, dur6, "AI_T_6"));
        EventBus.Publish<ToastEvent>(new ToastEvent(msg7, dur7, "AI_T_7"));
        EventBus.Publish<ToastEvent>(new ToastEvent(msg8, dur8, "AI_T_8"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
