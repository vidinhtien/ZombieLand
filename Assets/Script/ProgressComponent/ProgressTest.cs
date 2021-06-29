using Pathfinding.Ionic.Zip;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressTest : MonoBehaviour
{
    public ProgressComponent po;
    public Text progress, status, last, curr, goal;
    int current = 0;
    public void _Reset()
    {
        po.FirstInit();
    }
    public void Pass()
    {
        current++;
        po.SetCurrent(current);
        last.text = "last: " + po.GetLast();
        if (po.CheckGetGoal())
        {
            po.AchieveGoal();
            status.text = "Get reward!";

        }
        else
        {
            status.text = "In progress!";
        }
        Debug.Log(po.GetProgress());
        progress.text = "progress: " + po.GetProgress();
        curr.text = "current: " + po.GetCurrent();
        goal.text = "goal: " + po.GetGoal();
    }
    public void Show()
    {
        last.text = "last: " + po.GetLast();
        if (po.CheckGetGoal())
        {
            po.AchieveGoal();
            status.text = "Get reward!";

        }
        else
        {
            status.text = "In progress!";
        }
        Debug.Log(po.GetProgress());
        progress.text = "progress: " + po.GetProgress();
        curr.text = "current: " + po.GetCurrent();
        goal.text = "goal: " + po.GetGoal();
    }

}
