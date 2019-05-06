using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject CoinTextObject;
    public GameObject MainPanel;
    public GameObject HorseBetPanel;
    public GameObject ResultPanel;
    public GameObject FinishBetButton;
    public GameObject NextRaceButton;
    public GameObject[] HorseUIObjects;
    public GameObject[] HorseObjects;
    public GameObject[] ResultHorseUIObjects;
    public GameObject[] ResultHorseUIHolders;

    public List<Horse> HorseData;
    public List<Jockey> JockeyData;
    public List<Horse> EntryHorse;
    public List<Horse> GoalHorse;

    private int Coin;
    private int BetRate = 1;
    private bool IsRacing;

    const int ENTRY_HORSE_NUMBER = 8;

    void Start () {
        HorseData = transform.GetComponent<CsvController>().InputHorseCSV("horse_data");
        JockeyData = transform.GetComponent<CsvController>().InputJockeyCSV("jockey_data");

        Coin = 100;
        IsRacing = false;

        SetAndShowHorse();
    }
	
    // 馬をランダムで取得して描画
    public void SetAndShowHorse()
    {
        NextRaceButton.SetActive(false);
        FinishBetButton.SetActive(true);
        ResultPanel.SetActive(false);
        SetRandomHorse();
        ShowEntryHorse();
        HorseBetPanel.SetActive(true);
    }

    public void ShowOrHiddenMainPanel()
    {
        MainPanel.SetActive(!MainPanel.activeSelf);
    }

    // 馬情報を画面表示
    public void ShowEntryHorse()
    {
        for(int i = 0; i < ENTRY_HORSE_NUMBER; i++)
        {
            HorseUIObjects[i].GetComponent<HorseUIController>()
                .UpdateHorse(EntryHorse[i]);
        }
    }

    // 馬と騎手をランダムにセット
    public void SetRandomHorse()
    {
        EntryHorse = new List<Horse>();
        int[] HorseNumArr = (Enumerable.Range(0, HorseData.Count)).ToArray();
        int[] HorseArr = HorseNumArr.OrderBy(i => Guid.NewGuid()).Take(ENTRY_HORSE_NUMBER).ToArray();

        int[] JockeyNumArr = (Enumerable.Range(0, JockeyData.Count)).ToArray();
        int[] JockeyArr = JockeyNumArr.OrderBy(i => Guid.NewGuid()).Take(ENTRY_HORSE_NUMBER).ToArray();

        for (int i = 0; i < ENTRY_HORSE_NUMBER; i++)
        {
            Horse h = new Horse(GetHorse(HorseArr[i]));
            h.Jockey = GetJockey(JockeyArr[i]);
            h.CalcPopularValue();
            EntryHorse.Add(h);
        }
        CalcOdds();
    }

    // 馬の人気指数からオッズを計算
    private void CalcOdds()
    {
        float SumPopVal = 0;
        for(int i = 0; i < ENTRY_HORSE_NUMBER; i++)
        {
            SumPopVal += EntryHorse[i].PopularValue;
        }
        for(int i = 0; i < ENTRY_HORSE_NUMBER; i++)
        {
            EntryHorse[i].Odds = Mathf.Max(SumPopVal / EntryHorse[i].PopularValue * 0.8f, 1.0f);
        }
        EntryHorse = EntryHorse.OrderByDescending(h => h.PopularValue).ToList<Horse>();
        for(int i = 0; i < ENTRY_HORSE_NUMBER; i++)
        {
            EntryHorse[i].PopularNumber = i + 1;
        }
        EntryHorse = EntryHorse.OrderBy(i => Guid.NewGuid()).ToList<Horse>();
    }

    public void BetHorse(int i)
    {
        if(!IsRacing)
        {
            if (Coin < BetRate)
                return;
            Coin -= BetRate;
            UpdateCoin();

            EntryHorse[i].Bet += BetRate;
            HorseUIObjects[i].GetComponent<HorseUIController>()
                .UpdateHorse(EntryHorse[i]);
        }
    }

    public void StartRace()
    {
        if (!IsRacing)
        {
            GoalHorse = new List<Horse>();
            IsRacing = true;
            MainPanel.SetActive(false);
            FinishBetButton.SetActive(false);
            for (int i = 0; i < ENTRY_HORSE_NUMBER; i++)
            {
                HorseObjects[i].GetComponent<HorseController>().StartRun(EntryHorse[i]);
            }
        }
    }

    public void Goal(int i)
    {
        GoalHorse.Add(EntryHorse[i]);
        int rank = GoalHorse.Count - 1; // 着順

        // 子オブジェクトを削除
        foreach (Transform n in ResultHorseUIHolders[rank].transform)
        {
            GameObject.Destroy(n.gameObject);
        }

        GameObject goalHorse = Instantiate(
            ResultHorseUIObjects[i],
            ResultHorseUIHolders[rank].transform // 着順の箇所に表示
            );

        // 1着のときは王冠表示
        if(rank == 0)
        {
            goalHorse.transform.Find("Crown").gameObject.SetActive(true);
        }

        // 表示更新
        goalHorse.GetComponent<HorseUIController>().UpdateHorse(EntryHorse[i]);


        // 最下位のときは清算へ
        if (rank == ENTRY_HORSE_NUMBER - 1)
        {
            IsRacing = false;
            Result();
        }
    }

    // 精算
    public void Result()
    {
        if(GoalHorse[0].Bet > 0)
        {
            Coin += (int) Mathf.Floor(GoalHorse[0].Odds * GoalHorse[0].Bet);
            UpdateCoin();
        }
        HorseBetPanel.SetActive(false);
        ResultPanel.SetActive(true);
        NextRaceButton.SetActive(true);
        MainPanel.SetActive(true);
    }

    private void UpdateCoin()
    {
        Text t = CoinTextObject.GetComponent<Text>();
        t.text = Coin.ToString();
    }

    public Horse GetHorse(int i)
    {
        return HorseData[i];
    }
    public Jockey GetJockey(int i)
    {
        return JockeyData[i];
    }
}
