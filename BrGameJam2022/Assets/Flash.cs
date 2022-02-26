using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    public GameObject fleeingAlien;
    public GameObject foodTruck;
    public Sprite foodTruckWithoutAlien;

    private void Start()
    {
        foodTruck.GetComponent<SpriteRenderer>().sprite = foodTruckWithoutAlien;
        fleeingAlien.SetActive(true);
        PlayerController.pc.canClimbDownManhole = true;
    }

    public void DisableMe()
    {
        gameObject.SetActive(false);
    }


}
