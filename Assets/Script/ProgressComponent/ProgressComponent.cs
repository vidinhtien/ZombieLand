using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Security.Principal;
using UnityEngine;

public class ProgressComponent : MonoBehaviour
{
    public static ProgressComponent Instance;
    const string LAST = "LASTLEVEL";
    const string CURRENT = "CURRENTLEVEL";
    const string GOAL = "GOAL";
    const string GOALLEVEL = "G_LEVEL";
    [SerializeField]
    int[] goalLevel;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        if (FirstOpenController.instance.IsOpenFirst)
        {
            FirstInit();
        }
    }

    public void FirstInit()
    {
        PlayerPrefs.SetInt(LAST, 0);
        PlayerPrefs.SetInt(CURRENT, 0);
        PlayerPrefs.SetInt(GOALLEVEL, 0);
        PlayerPrefs.SetInt(GOAL, goalLevel[0]);
    }

    public bool CheckGetGoal()
    {
        int last = GetLast();
        int current = GetCurrent();
        int goal = GetGoal();
        if (current - last >= goal)
        {
            return true;
        }
        return false;
    }
    public int GetLast()
    {
        return PlayerPrefs.GetInt(LAST, 1);
    }
    public int GetCurrent()
    {
        return PlayerPrefs.GetInt(CURRENT, 1);
    }
    public int GetGoal()
    {
        return PlayerPrefs.GetInt(GOAL, 1);
    }
    private int GetGoalLevel()
    {
        return PlayerPrefs.GetInt(GOALLEVEL, 0);
    }
    public float GetProgress()
    {
        int last = GetLast();
        int current = GetCurrent();
        int goal = GetGoal();
        float progress = current - last;
        if (progress < 0) progress = 0;
        if (goal == 0) return 0;
        return (float)progress / goal;
    }

    public void SetGoal(int goal)
    {
        PlayerPrefs.SetInt(GOAL, goal);
    }
    public void SetLast(int last)
    {
        if (GetLast() < last)
            PlayerPrefs.SetInt(LAST, last);
    }
    public void SetCurrent(int current)
    {
        if (GetCurrent() < current)
            PlayerPrefs.SetInt(CURRENT, current);
    }
    public void AchieveGoal()
    {
        if (CheckGetGoal())
        {
            SetLast(GetCurrent());
            int index = GetGoalLevel();
            if (index < goalLevel.Length - 1)
            {
                index++;
                SetGoal(goalLevel[index]);
            }
        }
    }
}
