using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject MainPanel;
    public GameObject[] HorseObjects;

    public List<Horse> HorseData;
    public List<Jockey> JockeyData;
    public List<Horse> EntryHorse;

    const int ENTRY_HORSE_NUMBER = 8;

    // Use this for initialization
    void Start () {
        EntryHorse = new List<Horse>();
        HorseData = transform.GetComponent<CsvController>().InputHorseCSV("horse_data");
        JockeyData = transform.GetComponent<CsvController>().InputJockeyCSV("jockey_data");
        SetAndShowHorse();
    }
	
    // 馬をランダムで取得して描画
    public void SetAndShowHorse()
    {
        SetRandomHorse();
        ShowEntryHorse();
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
            HorseObjects[i].GetComponent<HorseController>()
                .UpdateHorse(EntryHorse[i]);
        }
    }

    // 馬と騎手をランダムにセット
    public void SetRandomHorse()
    {
        EntryHorse.Clear();
        int[] HorseNumArr = (Enumerable.Range(0, HorseData.Count)).ToArray();
        int[] HorseArr = HorseNumArr.OrderBy(i => Guid.NewGuid()).Take(ENTRY_HORSE_NUMBER).ToArray();

        int[] JockeyNumArr = (Enumerable.Range(0, JockeyData.Count)).ToArray();
        int[] JockeyArr = JockeyNumArr.OrderBy(i => Guid.NewGuid()).Take(ENTRY_HORSE_NUMBER).ToArray();

        for (int i = 0; i < ENTRY_HORSE_NUMBER; i++)
        {
            Horse h = GetHorse(HorseArr[i]);
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
            EntryHorse[i].Odds = SumPopVal / EntryHorse[i].PopularValue * 0.8f;
        }
        EntryHorse = EntryHorse.OrderByDescending(h => h.PopularValue).ToList<Horse>();
        for(int i = 0; i < ENTRY_HORSE_NUMBER; i++)
        {
            EntryHorse[i].PopularNumber = i + 1;
        }
        EntryHorse = EntryHorse.OrderBy(i => Guid.NewGuid()).ToList<Horse>();
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
