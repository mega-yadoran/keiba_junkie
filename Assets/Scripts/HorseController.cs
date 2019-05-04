using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorseController : MonoBehaviour {
    public GameObject HorseNameObject;
    public GameObject JockeyNameObject;
    public GameObject PopularNumberObject;
    public GameObject OddsObject;

    public void UpdateHorse(Horse horse)
    {
        SetText4View(HorseNameObject, horse.Name);
        SetText4View(JockeyNameObject, horse.Jockey.Name);
        SetText4View(PopularNumberObject, horse.PopularNumber.ToString());
        SetText4View(OddsObject, horse.Odds.ToString("0.0"));
    }

    private void SetText4View(GameObject g, string str)
    {
        Text t = g.GetComponent<Text>();
        t.text = str;
    }
}
