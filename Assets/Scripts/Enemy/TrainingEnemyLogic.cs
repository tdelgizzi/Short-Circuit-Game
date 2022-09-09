using UnityEngine;

public class TrainingEnemyLogic : EnemyLogic
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnVisionEnter(Transform t)
    {
        base.OnVisionEnter(t);
    }

    protected override void OnVisionStay(Transform t)
    {
        SmoothLookAt(t);
    }

    protected override void OnVisionExit(Transform t)
    {
        base.OnVisionExit(t);
    }
}

