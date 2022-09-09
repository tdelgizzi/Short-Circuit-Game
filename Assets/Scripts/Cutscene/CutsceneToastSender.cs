using System.Collections.Generic;
using UnityEngine;

public class CutsceneToastSender : MonoBehaviour
{
    private class ToastData
    {
        private string msg;
        private float duration;
        private string audioKey;

        public ToastData(string msg, float duration, string audioKey)
        {
            this.msg = msg;
            this.duration = duration;
            this.audioKey = audioKey;
        }

        public void Send()
        {
            EventBus.Publish(new ToastEvent(msg, duration, audioKey));
        }
    }

    private List<ToastData> firstToasts = new List<ToastData>
    {
        new ToastData("The date is 12.10.2494.", 3, "CS1_1"),
        new ToastData("I am HERMES, the AI tasked with the handling of ship functions while all human crew and passengers are in cryo", 6, "CS1_2"),
        new ToastData("Starship Astoria's systems are Nominal, and the Journey has been uneventful... Until now", 5, "CS1_3")
    };

    private List<ToastData> secondToasts = new List<ToastData>
    {
         new ToastData("20 minutes ago I detected an anomaly that tore the fabric of reality outside the vessel", 5, "CS2_1"),
         new ToastData("Several hostile entities have boarded the Astoria and have disabled our primary power systems", 5, "CS2_2"),
         new ToastData("By my calculations, if the ship were to enter hyperspace, these entities would instantly perish", 5, "CS2_3"),
         new ToastData("However, without power, I am unable to engage the jump to hyperspeed", 4, "CS2_4"),
         new ToastData("I need your help to restore the ship's systems and save the crew", 3, "CS2_5")
    };

    // Start is called before the first frame update
    public void SendFirstBatch()
    {
        foreach (ToastData toast in firstToasts)
        {
            toast.Send();
        }
    }

    public void SendSecondBatch()
    {
        foreach (ToastData toast in secondToasts)
        {
            toast.Send();
        }
    }
}
