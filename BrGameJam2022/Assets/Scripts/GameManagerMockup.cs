using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class GameManagerMockup : MonoBehaviour
{
    [NonSerialized]
    public static GameManagerMockup instance;

    public GameObject Player;
    public GameObject NPC_Prefab;
    public GameObject Alien_Prefab;

    public List<GameObject> Homes = new List<GameObject>();
    public List<GameObject> WorkPlaces = new List<GameObject>();
    public List<GameObject> Recreation = new List<GameObject>();
    public GameObject PizzaPlace;

    public List<GameObject> NPC_s = new List<GameObject>();

    System.Random r = new System.Random();

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    void Start()
    {
        SpawnAlien();
        SpawnNPCs();
        SpawnNPCs();
        SpawnNPCs();
        SpawnNPCs();
        SpawnNPCs();
        SpawnNPCs();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {

        }
    }

    #region Spawning NPCs
    public void SpawnAlien()
    {
        var aux = Instantiate(Alien_Prefab, new Vector3(15,3,0), Quaternion.identity);
        var pos = aux.transform.position;
        pos.z = 0;
        aux.transform.position = pos;

        aux.GetComponent<Alien>().CurrentTarget = PizzaPlace.transform.GetChild(0).gameObject;
        aux.GetComponent<Alien>().Player = Player;
    }

    public void SpawnNPCs()
    {
        var NPC_SpawnPoint = RandVect3(-50f, 50f, 3f, 3f);
        var aux = Instantiate(NPC_Prefab, NPC_SpawnPoint, Quaternion.identity);
        var pos = aux.transform.position;
        pos.z = 0;
        aux.transform.position = pos;
        aux.name = "NPC_" + NPC_s.Count.ToString();

        aux.GetComponent<NPC>().PlacesToAttend = PathCreator();

        NPC_s.Add(aux);

        aux.GetComponentInChildren<OnTrigger>().AddEvent("Enter", "Alien", On_NPC_see_Alien);
    }

    public GameObject[] PathCreator()
    {
        GameObject[] path = new GameObject[3];
        path[0] = Homes[IntRandRange(0, Homes.Count)].transform.GetChild(0).gameObject;
        path[1] = WorkPlaces[IntRandRange(0, WorkPlaces.Count)].transform.GetChild(0).gameObject;
        path[2] = Recreation[IntRandRange(0, Recreation.Count)].transform.GetChild(0).gameObject;
        return path;
    }
    #endregion

    public GameObject FarthestNPC(Vector3 position)
    {
        float minDistance = 0f;
        GameObject reference = null;
        foreach(GameObject npc in NPC_s)
        {
            if(Vector2.Distance(position, npc.transform.position) > minDistance)
            {
                minDistance = Vector2.Distance(position, npc.transform.position);
                reference = npc;
            }
        }
        return reference;
    }
    public GameObject ClosestNPC(Vector3 position)
    {
        float minDistance = 5000f;
        GameObject reference = null;
        foreach (GameObject npc in NPC_s)
        {
            if (Vector2.Distance(position, npc.transform.position) < minDistance)
            {
                minDistance = Vector2.Distance(position, npc.transform.position);
                reference = npc;
            }
        }
        return reference;
    }

    #region Events
    public void On_NPC_see_Alien(GameObject sender, Collider2D other)
    {
        //Debug.Log(sender.name + " SAW THE ALIEN");
    }
    #endregion

    #region Auxilliary Functions
    public Vector3 RandVect3(float minX, float maxX, float minY, float maxY)
    {
        var x = r.NextDouble() * (maxX - minX) + minX;
        var y = r.NextDouble() * (maxY - minY) + minY;
        return new Vector3((float)x, (float)y, 0);
    }

    public int IntRandRange(int min, int max)
    {
        return r.Next(min, max);
    }
    public float FloatRandRange(float min, float max)
    {
        var x = r.NextDouble() * (max - min) + min;
        return (float)x;
    }
    public float[] ShuffleArray(float[] array)
    {
        float[] arr = new float[array.Length];
        array.CopyTo(arr, 0);
        // Start from the last element and 
        // swap one by one. We don't need to 
        // run for the first element  
        // that's why i > 0
        int n = arr.Length;
        for (int i = n - 1; i > 0; i--)
        {

            // Pick a random index 
            // from 0 to i 
            int j = r.Next(0, i + 1);

            // Swap arr[i] with the 
            // element at random index 
            float temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }
        // Prints the random array 
        return arr;
    }
    public int[] PrimeNumbersInRange(int min, int max)
    {
        List<int> arr = new List<int>();
        // Declare the variables
        int a, b, i, j, flag;

        // Take input
        a = min;

        // Take input
        b = max;

        // Print display message
        Console.WriteLine("\nPrime numbers between " +
                          "{0} and {1} are: ", a, b);

        // Traverse each number in the interval
        // with the help of for loop
        for (i = a; i <= b; i++)
        {

            // Skip 0 and 1 as they are
            // niether prime nor composite
            if (i == 1 || i == 0)
                continue;

            // flag variable to tell
            // if i is prime or not
            flag = 1;

            for (j = 2; j <= i / 2; ++j)
            {
                if (i % j == 0)
                {
                    flag = 0;
                    break;
                }
            }

            // flag = 1 means i is prime
            // and flag = 0 means i is not prime
            if (flag == 1) arr.Add(i);
        }
        return arr.ToArray();
    }
    public int PrimeRandRange(int min, int max)
    {
        int[] primeArray = PrimeNumbersInRange(min, max);
        int index = IntRandRange(0, primeArray.Length);
        return primeArray[index];
    }
    #endregion
}
