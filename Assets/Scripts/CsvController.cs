using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

    public void CalcPopularValue()
    {
        PopularValue = (Mathf.Pow((float)Rating / 100 ,30) + Rating * Jockey.Winrate / 1000) * Random.Range(0.9f, 1.1f);
    }
}

public class Jockey
{
    public string Name { get; set; }
    public float Winrate { get; set; }
}

public class CsvController : MonoBehaviour {

    const int CSV_CULUMN_NUM_HORSE_RATING = 0;
    const int CSV_CULUMN_NUM_HORSE_NAME = 1;
    const int CSV_CULUMN_NUM_HORSE_SEX = 2;
    const int CSV_CULUMN_NUM_HORSE_AGE = 3;

    const int CSV_CULUMN_NUM_JOCKEY_NAME = 0;
    const int CSV_CULUMN_NUM_JOCKEY_WIN = 1;
    const int CSV_CULUMN_NUM_JOCKEY_WINRATE = 2;

    public List<Horse> InputHorseCSV(string fileName)
    {
        List<Horse> HorseData = new List<Horse>();
        TextAsset csvFile = Resources.Load("CSV/" + fileName) as TextAsset;
        StringReader reader = new StringReader(csvFile.text);
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            string[] line_arr = line.Split(','); // リストに入れる
            Horse h = new Horse();
            h.Name = line_arr[CSV_CULUMN_NUM_HORSE_NAME];
            h.Rating = int.Parse(line_arr[CSV_CULUMN_NUM_HORSE_RATING]);
            h.Sex = line_arr[CSV_CULUMN_NUM_HORSE_SEX];
            h.Age = int.Parse(line_arr[CSV_CULUMN_NUM_HORSE_AGE]);
            HorseData.Add(h);
        }
        return HorseData;
    }


    public List<Jockey> InputJockeyCSV(string fileName)
    {
        List<Jockey> JockeyData = new List<Jockey>();
        TextAsset csvFile = Resources.Load("CSV/" + fileName) as TextAsset;
        StringReader reader = new StringReader(csvFile.text);
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            string[] line_arr = line.Split(','); // リストに入れる
            Jockey j = new Jockey();
            j.Name = line_arr[CSV_CULUMN_NUM_JOCKEY_NAME];
            j.Winrate = float.Parse(line_arr[CSV_CULUMN_NUM_JOCKEY_WINRATE]);
            JockeyData.Add(j);
        }
        return JockeyData;
    }
}
