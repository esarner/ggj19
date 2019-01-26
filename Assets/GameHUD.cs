using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public struct MissionBriefing
{
    public Canvas Canvas;
    public Text Name;
    public Text Description;
}

[Serializable]
public struct ScoreScreen
{
    public Canvas Canvas;
    public Text Score;
}


public class GameHUD : MonoBehaviour
{
    public MissionBriefing MissionBriefing;
    public ScoreScreen ScoreScreen;
    public TimePiece TimePiece;

    public void Start()
    {
    }

    public void SetMissionBriefing(string name, string description)
    {
        MissionBriefing.Name.text = name;
        MissionBriefing.Description.text = description;
    }

    public void SetScoreScreen(int score)
    {
        ScoreScreen.Score.text = $"Score: {score}";
    }

    public void DisplayScore(bool display)
    {
        ScoreScreen.Canvas.enabled = display;
    }

    public void DisplayBriefing(bool display)
    {
        MissionBriefing.Canvas.enabled = display;
    }

    public void DisplayTimePiece(bool display)
    {
        TimePiece.Canvas.enabled = display;
    }
}
