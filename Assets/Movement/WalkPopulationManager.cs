using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WalkPopulationManager : MonoBehaviour
{
    public GameObject botPrefab;
    public int populationSize = 50;

    List<GameObject> population = new List<GameObject>();

    public static float elapsed = 0;
    public float trialTime = 5;
    int generation = 1;

    GUIStyle guiStyle = new GUIStyle();
    private void OnGUI()
    {

        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10, 10, 250, 150));
        GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
        GUI.Label(new Rect (10,25,200,30), "Gen: " + generation, guiStyle);
        GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time : {0:0.00}", elapsed), guiStyle);
        GUI.Label(new Rect(10, 75, 200, 30), "Population: " +population.Count, guiStyle);
        GUI.EndGroup();

    }

    private void Start()
    {
        for(int i = 0; i < populationSize; i++) //cria a primeira populacao
        {
            Vector3 startingPos = new Vector3(this.transform.position.x + Random.Range(-2,2),this.transform.position.y, this.transform.position.z + Random.Range(-2,2));

            GameObject b = Instantiate(botPrefab, startingPos, this.transform.rotation);
            b.GetComponent<WalkBrain>().Init();
            population.Add(b);

        }

        
    }

    GameObject Breed(GameObject parent1, GameObject parent2)
    {
        Vector3 startingPos = new Vector3(this.transform.position.x + Random.Range(-2, 2), this.transform.position.y, this.transform.position.z + Random.Range(-2, 2));

        GameObject offspring = Instantiate(botPrefab, startingPos, this.transform.rotation);
        WalkBrain b = offspring.GetComponent<WalkBrain>();

        if(Random.Range(0,100) == 1) //1% de mutacao
        {
            b.Init();
            b.dna.Mutate();
        }
        else
        {
            b.Init();
            b.dna.Combine(parent1.GetComponent<WalkBrain>().dna, parent2.GetComponent<WalkBrain>().dna);
        }

        return offspring;
    }

    void BreedNewPopulation()
    {
        List<GameObject> sortedList = population.OrderByDescending(o => /*(o.GetComponent<WalkBrain>().timeAlive +*/ o.GetComponent<WalkBrain>().distance/*)*/).ToList(); //Fitness Function no caso eh timeAlive + distance

        population.Clear();

        for(int i = (int)(sortedList.Count /2.0f) - 1; i < sortedList.Count -1; i++) 
        {
                population.Add(Breed(sortedList[i], sortedList[i + 1]));
                population.Add(Breed(sortedList[i + 1], sortedList[i]));
            
        }

        for(int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }

        generation++;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        if(elapsed >= trialTime)
        {
            BreedNewPopulation();
            elapsed = 0;
        }
    }
}
