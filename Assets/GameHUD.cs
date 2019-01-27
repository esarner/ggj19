using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public Text Name;
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

    public void SetMissionBriefing(string missionName, string description)
    {
        MissionBriefing.Name.text = missionName;
        MissionBriefing.Description.text = description;
    }

    public void SetScoreScreen(string missionName, IEnumerable<(PickupType Type, int Points)> score)
    {
        ScoreScreen.Name.text = missionName;

        var scoreText = string.Join("\n", score.Select(s => $"{s.Type.ToString()}: {s.Points}"));
        scoreText += $"\n\nTotal score: {score.Sum(s => s.Points)}";
        ScoreScreen.Score.text = scoreText;
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
