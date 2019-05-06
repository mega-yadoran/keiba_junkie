using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Horse
{
    public string Name { get; set; }
    public string Sex { get; set; }
    public int Age { get; set; }
    public int Rating { get; set; }
    public Jockey Jockey { get; set; }
    public int PopularNumber { get; set; }
    public float Odds { get; set; }
    public float PopularValue { get; set; }
    public int Bet { get; set; }

    public Horse()
    {

    }

    public Horse(Horse h)
    {
        this.Name = h.Name;
        this.Sex = h.Sex;
        this.Age = h.Age;
        this.Rating = h.Rating;
    }

    public void CalcPopularValue()
    {
        PopularValue = (Mathf.Pow((float)Rating / 100, 30) + Rating * Jockey.Winrate / 1000) * Random.Range(0.9f, 1.1f);
    }
}

public class Jockey
{
    public string Name { get; set; }
    public float Winrate { get; set; }
}

public class HorseUIController : MonoBehaviour {
    public GameObject HorseNameObject;
    public GameObject JockeyNameObject;
    public GameObject PopularNumberObject;
    public GameObject OddsObject;
    public GameObject BetObject;
        
    public void UpdateHorse(Horse horse)
    {
        SetText4View(HorseNameObject, horse.Name);
        SetText4View(JockeyNameObject, horse.Jockey.Name);
        SetText4View(PopularNumberObject, horse.PopularNumber.ToString());
        SetText4View(OddsObject, horse.Odds.ToString("0.0"));
        SetText4View(BetObject, horse.Bet.ToString());
    }

    private void SetText4View(GameObject g, string str)
    {
        Text t = g.GetComponent<Text>();
        t.text = str;
    }
}
