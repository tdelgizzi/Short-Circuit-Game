using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Helper class necessary to manage Biter behavior while use animation events
// on pincer movement, which is a child of the overall Biter enemy
public class BiterAnimationEvent : MonoBehaviour {
    
    public enum BiteAnimationState {
        Start,
        Snap,
        Shut,
        Done
    }

    public BiteAnimationState state;

    public void SetStart() {
        state = BiteAnimationState.Start;
    }

    public void SetSnap() {
        state = BiteAnimationState.Snap;
    }

    public void SetShut() {
        state = BiteAnimationState.Shut;
    }

    public void SetDone() {
        state = BiteAnimationState.Done;
    }
}
