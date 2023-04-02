using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static ScoreManager;

public class InitializeValues : MonoBehaviour
{
    [SerializeField] TMP_Text ranking;
    [SerializeField] TMP_Text teamName;
    [SerializeField] TMP_Text score;

    public void InitializeValue(ScoreManager.TeamValues teamValue)
    {
        ranking.text = "#"+teamValue.teamRanking;
        teamName.text = teamValue.teamName;
        score.text = teamValue.teamScore + " pts";
    }

    public void InitializeEmpty()
    {
        ranking.text = "";
        teamName.text = "";
        score.text = "";
    }
}
