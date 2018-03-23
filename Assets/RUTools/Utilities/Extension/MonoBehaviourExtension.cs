using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// MonoBehaviourExtension class.
/// </summary>
public static class MonoBehaviourExtension
{
    //Actions

    /// <summary>
    /// Delays an "action" by scaled "time" using coroutines.
    /// </summary>
    public static Coroutine DelayedAction(this MonoBehaviour mb, Action action, float time)
    {
        return mb.StartCoroutine (DelayedActionProcess(action, time, true));
    }
    /// <summary>
    /// Delays an "action" by unscaled "time" using coroutines.
    /// </summary>
    public static Coroutine DelayedActionUnscaled(this MonoBehaviour mb, Action action, float time)
    {
        return mb.StartCoroutine(DelayedActionProcess(action, time, false));
    }

    private static IEnumerator DelayedActionProcess(Action action, float time, bool scaled)
    {
        if(scaled)
            yield return new WaitForSeconds (time);
        else
            yield return new WaitForSecondsRealtime(time);

        if(action != null)
            action();
    }

    //Coroutine

    /// <summary>
    /// Checks if coroutine is null, if not stop it and sets "cr" to null.
    /// </summary>
    public static void StopCoroutineSafe(this MonoBehaviour mb, ref Coroutine cr)
    {
        if (cr == null)
            return;

        mb.StopCoroutine (cr);
        cr = null;
    }

    /// <summary>
    /// Calls "loopAction" for "duration" time and ends with "endAction".
    /// </summary>
    public static Coroutine CoroutineProgress(this MonoBehaviour mb, float duration,
        Action<float> loopAction, Action endAction,
        CoroutineProgressData config = new CoroutineProgressData())
    { return mb.CoroutineProgress(duration, 0, loopAction, endAction, config); }

    /// <summary>
    /// Starts after "delay", calls "loopAction" for "duration" time and ends with "endAction".
    /// </summary>
    public static Coroutine CoroutineProgress(this MonoBehaviour mb, float duration,
        float delay, Action<float> loopAction, Action endAction,
        CoroutineProgressData config = new CoroutineProgressData())
    {
        switch (config.type)
        {
            case CoroutineProgressType.WaitForSecondsRealtime:
                return mb.StartCoroutine(CoroutineProgressUnscaled(duration, delay, loopAction, endAction, config));
            default:
                return mb.StartCoroutine(CoroutineProgressScaled(duration, delay, loopAction, endAction, config));
        }
    }

    private static IEnumerator CoroutineProgressScaled(float duration, float delay,
        Action<float> loopAction, Action endAction,
        CoroutineProgressData config)
    {
        if (delay > 0)
            yield return new WaitForSeconds (delay);

        float startTime = Time.time;

        YieldInstruction yieldOnUpdate = null;
        switch (config.type)
        {
            case CoroutineProgressType.WaitForEndOfFrame:
                yieldOnUpdate = new WaitForEndOfFrame();
                break;
            case CoroutineProgressType.WaitForFixedUpdate:
                yieldOnUpdate = new WaitForFixedUpdate();
                break;
            case CoroutineProgressType.WaitForSeconds:
                yieldOnUpdate = new WaitForSeconds(config.timeToWait);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        float t = duration > 0 ? 0 : 1;

        while (t < 1)
        {
            t = Mathf.Clamp01((Time.time - startTime) / duration);

            if(loopAction != null)
                loopAction.Invoke(t);

            yield return yieldOnUpdate;
        }

        if (endAction != null)
            endAction.Invoke();
    }
    private static IEnumerator CoroutineProgressUnscaled(float duration, float delay,
        Action<float> loopAction, Action endAction,
        CoroutineProgressData config)
    {
        if (delay > 0)
            yield return new WaitForSecondsRealtime(delay);

        float startTime = Time.unscaledTime;
        float t = duration > 0 ? 0 : 1;

        WaitForSecondsRealtime yieldOnUpdate = new WaitForSecondsRealtime(config.timeToWait);

        while (t < 1)
        {
            t = Mathf.Clamp01((Time.unscaledTime - startTime) / duration);

            if (loopAction != null)
                loopAction.Invoke(t);

            yield return yieldOnUpdate;
        }

        if (endAction != null)
            endAction.Invoke();
    }

    public enum CoroutineProgressType
    {
        WaitForEndOfFrame = 0,
        WaitForFixedUpdate,
        WaitForSeconds,
        WaitForSecondsRealtime,
    }

    public struct CoroutineProgressData
    {
        public CoroutineProgressType type;
        public float timeToWait;

        public CoroutineProgressData(CoroutineProgressType type)
        {
            this.type = type;
            this.timeToWait = 0.01f;
        }

        public CoroutineProgressData(CoroutineProgressType type, float timeToWait)
        {
            this.type = type;
            this.timeToWait = timeToWait;
        }
    }
}