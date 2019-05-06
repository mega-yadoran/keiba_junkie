using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour {

    public GameObject GameControllerObj;

    private GameController gameController;

    private void Start()
    {
        gameController = GameControllerObj.GetComponent<GameController>();
    }
    

    private void OnTriggerEnter(Collider other)
    {
        HorseController horseController = other.transform.GetComponent<HorseController>();
        if (horseController.IsRunning)
        {
            int horseNumber = other.transform.GetComponent<HorseController>().HorseNumber;
            gameController.Goal(horseNumber);
        }
    }
}
