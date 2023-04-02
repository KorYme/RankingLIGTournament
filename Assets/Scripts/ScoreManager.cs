using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TeamOne;
using UnityEngine.UI;
using System;
using TMPro;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    const int differencyToScore = 1;
    const int differencyToRanking = 10;
    private int pageNumber = 1;
    private int maxPageNumber = 1;
    public struct TeamValues
    {
        public string teamRanking;
        public string teamName;
        public string teamScore;
        public TeamValues(string teamRanking, string teamName, string teamScore)
        {
            this.teamRanking = teamRanking;
            this.teamName = teamName;
            this.teamScore = teamScore;
        }
    }
    List<TeamValues> teamValuesList;
    public enum ColumnRanking
    {
        RocketLIG,
        NotAlone,
        LIGByrinthe,
        LIGNinja,
        Photographie,
        Kahoot,
        CoopHead,
        LIGParty,
        Total,
    }
    ColumnRanking columnRanking = ColumnRanking.Total;

    [Header("References")]
    [SerializeField] ReadGoogleSheet googleSheet;
    [SerializeField] TMP_Dropdown allGamesDropdown;
    [SerializeField] TMP_Text pageNumberText;
    [SerializeField] List<InitializeValues> uiPrefabs;

    private void Start()
    {
        allGamesDropdown.onValueChanged.AddListener(Dropdown_IndexChanged);
        teamValuesList = new();
        pageNumber = 1;
        InitDropdown();
        StartCoroutine(RefreshInfos());
    }

    private void InitDropdown()
    {
        string[] enumNames = Enum.GetNames(typeof(ColumnRanking));
        List<string> names = new List<string>(enumNames);
        allGamesDropdown.AddOptions(names);
    }

    public void Dropdown_IndexChanged(int index)
    {
        columnRanking = (ColumnRanking)index;
        StartCoroutine(RefreshInfos());
    }

    public void ChangePageNumber(bool isIncreasing)
    {
        if (isIncreasing)
        {
            pageNumber++;
            if (pageNumber>maxPageNumber)
            {
                pageNumber = 1;
            }
        }
        else
        {
            pageNumber--;
            if (pageNumber<=0)
            {
                pageNumber = maxPageNumber;
            }
        }
        pageNumberText.text = pageNumber.ToString() + "/" + maxPageNumber.ToString();
        RefreshValuesDisplayed();
    }

    private void RefreshValuesDisplayed()
    {
        for (int i = 0; i < 10; i++)
        {
            if (i + (pageNumber - 1) * 10 < teamValuesList.Count)
            {
                uiPrefabs[i].InitializeValue(teamValuesList[i + (pageNumber - 1) * 10]);
            }
            else
            {
                uiPrefabs[i].InitializeEmpty();
            }
        }
    }

    IEnumerator RefreshInfos()
    {
        if (googleSheet != null)
        {
            teamValuesList.Clear();
            googleSheet.RefreshData();
            while (googleSheet.isLoading)
                yield return null;
            for (int i = 0; i < googleSheet.currentNode["values"].Count-2; i++)
            {
                var data = googleSheet.currentNode["values"];
                TeamValues value;
                if (columnRanking != ColumnRanking.Total)
                {
                    value = new(data[i + 2][(int)columnRanking + differencyToRanking],
                        data[i + 2][0],
                        data[i + 2][(int)columnRanking + differencyToScore]);
                }
                else
                {
                    value = new(data[i + 2][19],
                        data[i + 2][0],
                        data[i + 2][18]);
                }
                teamValuesList.Add(value);
            }
            teamValuesList = teamValuesList.OrderBy(x => Int16.Parse(x.teamRanking)).ToList();
            RefreshValuesDisplayed();
            maxPageNumber = (((teamValuesList.Count - 1) / 10) + 1);
            pageNumberText.text = pageNumber.ToString() + "/" + maxPageNumber.ToString();
        }
        else
        {
            Debug.Log("There is no reference to ReadGoogleSheet");
            yield return null;
        }
    }
}