using UnityEngine;
using DG.Tweening;

public class SunMovement : MonoBehaviour
{
    [SerializeField] private Light _sun;
    private TimeControl _time;

    private readonly Vector3 _startSunRotation = new(-190f, -61.7f, 0);

    private void Awake()
    {
        _time = GetComponent<TimeControl>();
    }

    private void OnEnable()
    {
        EventBus.OnNewDayStarted += RotateSunForNewDay;
        EventBus.OnCurrentDayEnded += RotateSunForEndDay;

        _time.SubscribeToHourPassed(UpdateSunPosition);
    }

    private void OnDisable()
    {
        EventBus.OnNewDayStarted -= RotateSunForNewDay;
        EventBus.OnCurrentDayEnded -= RotateSunForEndDay;

        _time.UnsubscribeToHourPassed(UpdateSunPosition);
    }

    private void RotateSunForNewDay()
    {
        _sun.transform.DORotate(new Vector3(_startSunRotation.x - 20f, _startSunRotation.y, _startSunRotation.z), 2f);
    }

    private void UpdateSunPosition()
    {
        int steps = _time.GetHoursForEndDay - _time.GetHoursForStartDay;
        float startDaySunPos = -210f;
        float endDaySunPos = -360f;

        float stepSize = (endDaySunPos - startDaySunPos) / steps;
        float newRotation = startDaySunPos + (_time.GetCurrentHours - _time.GetHoursForStartDay) * stepSize;

        _sun.transform.DORotate(new Vector3(newRotation, -61.7f, 0), 1f);
    }

    private void RotateSunForEndDay()
    {
        _sun.transform.DORotate(_startSunRotation, 2f);
    }
}
