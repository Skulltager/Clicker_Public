using System.Collections;
using UnityEngine;

public class AnimatedScale : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private bool playOnAwake;

    [SerializeField] private float xScaleBase;
    [SerializeField] private float yScaleBase;
    [SerializeField] private float xScaleCurveFactor;
    [SerializeField] private float yScaleCurveFactor;
    [SerializeField] private AnimationCurve xScaleCurve;
    [SerializeField] private AnimationCurve yScaleCurve;
    [SerializeField] private float animationDuration;
    [SerializeField] private bool loop;

    private Coroutine routine;

    private void Awake()
    {
        if (!playOnAwake)
            return;

        Play();
    }

    public void Play()
    {
        if (!isActiveAndEnabled)
            return;

        if (routine != null)
            return;

        routine = StartCoroutine(Routine_AnimatedScale());
    }

    public void Restart()
    {
        if (!isActiveAndEnabled)
            return;

        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(Routine_AnimatedScale());
    }

    public void Stop()
    {
        if (!isActiveAndEnabled)
            return;

        if (routine == null)
            return;

        targetTransform.localScale = Vector3.one;
        StopCoroutine(routine);
        routine = null;
    }

    private IEnumerator Routine_AnimatedScale()
    {
        float timeInState = Time.deltaTime;

        do
        {
            while (timeInState < animationDuration)
            {
                float animationFactor = timeInState / animationDuration;
                float xScale = xScaleBase + xScaleCurve.Evaluate(animationFactor) * xScaleCurveFactor;
                float yScale = yScaleBase + yScaleCurve.Evaluate(animationFactor) * yScaleCurveFactor;
                targetTransform.localScale = new Vector3(xScale, yScale, xScale);
                yield return null;
                timeInState += Time.deltaTime;
            }

            timeInState -= animationDuration;
        }
        while (loop);

        targetTransform.localScale = Vector3.one;
        routine = null;
    }

    private void OnDisable()
    {
        routine = null;
    }
}