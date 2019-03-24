using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class CameraBob {
    // Variables declaration.
    [SerializeField] private float HorizontalBobRange;
    [SerializeField] private float VerticalBobRange;
    [SerializeField] private float VerticalToHorizontalRatio;
    [SerializeField] private float BobDuration;
    [SerializeField] private float BobAmount;
    public AnimationCurve Bobcurve = new AnimationCurve(new Keyframe(0f, 0f),
        new Keyframe(0.5f, 1f),
        new Keyframe(1f, 0f),
        new Keyframe(1.5f, -1f),
        new Keyframe(2f, 0f));
    private float cyclePositionX;
    private float cyclePositionY;
    private float bobBaseInterval;
    private Vector3 originalCameraPosition;
    private float t_Time;
    private float offSet = 0.0f;
    // End of variables declaration.

    // Camera bobbing initialization.
    public void Init(Camera camera, float bobBaseInt) {
        bobBaseInterval = bobBaseInt;
        originalCameraPosition = camera.transform.localPosition;
        t_Time = Bobcurve[Bobcurve.length - 1].time;
    }

    public float OffSet() => offSet;

    // Head Bobbing method according to speed (walk/run).
    public Vector3 DoHeadBob(float speed) {
        float xPos = originalCameraPosition.x + (Bobcurve.Evaluate(cyclePositionX) * HorizontalBobRange);
        float yPos = originalCameraPosition.y + (Bobcurve.Evaluate(cyclePositionY) * VerticalBobRange);

        cyclePositionX += (speed * Time.deltaTime) / bobBaseInterval;
        cyclePositionY += ((speed * Time.deltaTime) / bobBaseInterval) * VerticalToHorizontalRatio;

        if (cyclePositionX > t_Time) cyclePositionX = cyclePositionX - t_Time;
        if (cyclePositionY > t_Time) cyclePositionY = cyclePositionY - t_Time;

        return new Vector3(xPos, yPos, 0f);
    }

    public IEnumerator DoJumpBob() {
        float t = 0f;
        while(t<BobDuration) {
            offSet = Mathf.Lerp(BobAmount, 0f, t/BobDuration);
            t+=Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        offSet = 0f;
    }

}