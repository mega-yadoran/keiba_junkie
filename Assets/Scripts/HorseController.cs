using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseController : MonoBehaviour {

    private const float Corner = 20;
    private const float Speed = 7;

    public bool IsRunning;
    public int HorseNumber;


    public readonly float[][] SpeedRateArray = {
        new float[] { 1.1f , 1.1f, 1.0f , 0.9f, 0.9f },
        new float[] { 1.0f , 1.0f, 1.0f , 1.0f, 1.0f },
        new float[] { 0.9f , 0.9f, 1.1f , 1.1f, 1.5f },
        new float[] { 0.9f , 1.0f, 1.0f , 1.0f, 1.1f },
    };

    private float[] SpeedRate; // 大逃げ・逃げ・差し・追い込み

    private float Condition;
    private float BaseSpeed;

    private enum Line
    {
        First,
        Second,
        Third,
        Fourth,
        Fifth,
        Goal
    };

    private Line CurrentLine;

    // Use this for initialization
    void Start () {
        CurrentLine = Line.First;
        IsRunning = false;
    }

    private void Update()
    {
        if (IsRunning)
        {
            Run();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Goal")
        {

        }
    }

    public void StartRun(Horse h)
    {
        SpeedRate = SpeedRateArray[Random.Range(0, SpeedRateArray.Length)];
        Condition = Random.Range(0.95f, 1.05f); // 調子
        BaseSpeed = Time.deltaTime
            * Speed
            * Condition
            * Mathf.Pow(h.Rating, 1.2f) / Mathf.Pow(100, 1.2f)
            * (1 + h.Jockey.Winrate);

        IsRunning = true;
    }

    private void Run()
    {
        // 移動処理
        float _x = 0; float _z = 0;
        switch (CurrentLine)
        {
            case Line.First:
                _x = transform.position.x - BaseSpeed * SpeedRate[0];
                _z = transform.position.z;
                transform.position = new Vector3(_x, 0, _z);
                if (_x <= Corner * -1)
                {
                    CurrentLine = Line.Second;
                }
                break;
            case Line.Second:
                transform.RotateAround(new Vector3(Corner * -1, 0, 0), Vector3.up, BaseSpeed * Mathf.PI * SpeedRate[1]);
                if (transform.position.x > Corner * -1)
                {
                    CurrentLine = Line.Third;
                }
                break;
            case Line.Third:
                _x = transform.position.x + BaseSpeed * SpeedRate[2];
                _z = transform.position.z;
                transform.position = new Vector3(_x, 0, _z);
                if (transform.position.x >= Corner)
                {
                    CurrentLine = Line.Fourth;
                }
                break;
            case Line.Fourth:
                transform.RotateAround(new Vector3(Corner, 0, 0), Vector3.up, BaseSpeed * Mathf.PI * SpeedRate[3]);
                if (transform.position.x < Corner)
                {
                    CurrentLine = Line.Fifth;
                }
                break;
            case Line.Fifth:
                _x = transform.position.x - BaseSpeed * SpeedRate[4];
                _z = transform.position.z;
                transform.position = new Vector3(_x, 0, _z);
                if (_x <= 0)
                {
                    CurrentLine = Line.Goal;
                }
                break;
            case Line.Goal:
                IsRunning = false;
                CurrentLine = Line.First;
                break;
        }
    }
}
