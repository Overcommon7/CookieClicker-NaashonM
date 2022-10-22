using MyBox;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using System.IO;

[Serializable]
public struct RunTimeVariables
{
    [ReadOnly] public double cookiesPerSecondBoost;
    public TextMeshProUGUI totalCookies;
    public TextMeshProUGUI cookiesPerSecond;
    [ReadOnly] public double clicksPerSecond;
    [ReadOnly] public short cookiePerClickMultiplier;
    [ReadOnly] public ulong totalClicks;
    [ReadOnly] public int timeSustained;
}

[Serializable]
public struct PlayerValues
{
    public double amountOfCookies;
    public double cookiesPerSecond;
    public double cookiesPerClick;
}

[Serializable]
public struct ClicksPerSecondVariables
{
    public List<short> numOfClicks; 
    public double timeTracker;
    public readonly double timeFrame;
    public short index;
    public short numOfClicksPerTimeFrame;

    public ClicksPerSecondVariables(int vectorSize, double TimeFrame)
    {
        numOfClicks = new List<short>();
        for (short i = 0; i < vectorSize; i++)
            numOfClicks.Add(0); 
        numOfClicksPerTimeFrame = 0;
        timeTracker = 0;
        timeFrame = TimeFrame;
        index = 0;
    }
}

public class PlayerManager : MonoBehaviour
{
    public PlayerValues playerValues;
    public RunTimeVariables runTimeValues;
    public ClicksPerSecondVariables clickVariables;
    public static PlayerManager instance = null;
    readonly static string dataFilePath = "PlayerStats.txt";
    readonly static string timeFilePath = "time.txt";
    private void Awake()
    {
        if (!instance) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        clickVariables = new ClicksPerSecondVariables(12, 0.12);
        var path = Utils.GetPath(dataFilePath);
        var data = Utils.GetFile(dataFilePath);
        if (data == null || data.Length < 4)
        {
            if (File.Exists(path)) File.Delete(path);
            playerValues.cookiesPerClick = 1;
            playerValues.amountOfCookies = 0;
            playerValues.cookiesPerSecond = 0;
            runTimeValues.totalClicks = 0;
            List<string> temp = new List<string>();
            temp.Add(playerValues.amountOfCookies.ToString());
            temp.Add(playerValues.cookiesPerSecond.ToString());
            temp.Add(playerValues.cookiesPerClick.ToString());
            temp.Add(runTimeValues.totalClicks.ToString());
            Utils.WriteToFile(ref temp, dataFilePath);
        }
        else
        {
            playerValues.amountOfCookies = double.Parse(data[0]);
            playerValues.cookiesPerSecond = double.Parse(data[1]);
            playerValues.cookiesPerClick = double.Parse(data[2]);
            runTimeValues.totalClicks = ulong.Parse(data[3]);
        }
        if (playerValues.cookiesPerClick <= 0) playerValues.cookiesPerClick = 1;
        runTimeValues.cookiePerClickMultiplier = 1;
        var cookiesFromTimeGone = LoadTimePassed() * playerValues.cookiesPerSecond;
        playerValues.amountOfCookies += cookiesFromTimeGone;



    }
    void FixedUpdate()
    {
        playerValues.amountOfCookies += playerValues.cookiesPerSecond * Time.fixedDeltaTime;
        runTimeValues.totalCookies.text = string.Format("{0:#,##0}", playerValues.amountOfCookies);
        runTimeValues.cookiesPerSecond.text = string.Format("{0:#,##0.##}", playerValues.cookiesPerSecond + runTimeValues.cookiesPerSecondBoost);
        int pos = runTimeValues.cookiesPerSecond.text.LastIndexOf('.');
        if (pos != -1) runTimeValues.cookiesPerSecond.text = runTimeValues.cookiesPerSecond.text.Substring(0, pos + 2);
        runTimeValues.cookiesPerSecond.text += " per second";
    }

    public void Update()
    {
        clickVariables.timeTracker += Time.deltaTime;
        if (clickVariables.timeTracker >= clickVariables.timeFrame)
        {
            ++clickVariables.index;
            if (clickVariables.index >= clickVariables.numOfClicks.Count) clickVariables.index = 0;           
            clickVariables.numOfClicks[clickVariables.index] = clickVariables.numOfClicksPerTimeFrame;
            clickVariables.timeTracker = 0;
            clickVariables.numOfClicksPerTimeFrame = 0;
            int total = 0;
            foreach (var i in clickVariables.numOfClicks)
                total += i;
            runTimeValues.clicksPerSecond = total / (clickVariables.numOfClicks.Count * clickVariables.timeFrame);
        }
    }
    private void OnDestroy()
    {
        List<string> data = new List<string>();
        data.Add(playerValues.amountOfCookies.ToString());
        data.Add(playerValues.cookiesPerSecond.ToString());
        data.Add(playerValues.cookiesPerClick.ToString());
        data.Add(runTimeValues.totalClicks.ToString());
        Utils.WriteToFile(ref data, dataFilePath);
        data.Clear();
        data.Add(DateTime.Now.ToString());
        Utils.WriteToFile(ref data, timeFilePath);
    }

    private double LoadTimePassed()
    {
        var path = Utils.GetPath(timeFilePath);
        var dateTime = Utils.GetFile(timeFilePath);
        if (!File.Exists(path) || dateTime.IsNullOrEmpty())
        {
            var fs = File.Create(path);
            fs.Close();
            List<string> time = new List<string>();
            time.Add(DateTime.Now.ToString());
            Utils.WriteToFile(ref time, timeFilePath);
            return 0;
        }
        DateTime prevTime;
        if (!DateTime.TryParse(dateTime[0], out prevTime))
        {
            Debug.LogWarning("Failed To Load Cookies From Time Span!");
            List<string> time = new List<string>();
            time.Add(DateTime.Now.ToString());
            Utils.WriteToFile(ref time, timeFilePath);
            return 0;
        }
        var current = DateTime.Now;
        double seconds = (current - prevTime).TotalDays * 86400;
        return seconds;
 
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) OnDestroy();
        else
        {
            instance = null;
            Awake();
        }
    }
}
