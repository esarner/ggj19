using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimePiece : MonoBehaviour
{

    public Image HourHand;
    public Image MinuteHand;
    public Image LimitIndicator;
    public Canvas Canvas;

    private void SetHours(int hours, float minutes)
    {

        var angleStep = 360f / 12f;

        var grej = angleStep / 60f;

        HourHand.rectTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, -angleStep * hours - grej * minutes));
    }


    private void SetMinutes(float minutes)
    {
        var angleStep = 360f / 60f;

        MinuteHand.rectTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, -angleStep * minutes));
    }

    public void SetTime(float minutes)
    {
        var hours = (int)minutes / 60;
        SetMinutes(minutes);
        SetHours(hours, minutes % 60f);
    }

    public void SetLimit(int hours)
    {
        var anglestep = 360f / 12f;

        LimitIndicator.rectTransform.localRotation = Quaternion.Euler(0, 0, -anglestep * hours);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
