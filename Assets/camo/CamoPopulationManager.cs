using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CamoPopulationManager : MonoBehaviour
{

    public GameObject personPrefab;

    public int populationSize = 10;

    List<GameObject> population = new List<GameObject>();

    public static float elapsed = 0;

    int trialTime = 10;
    int generation = 1;

    GUIStyle guiStyle = new GUIStyle();
    private void OnGUI()
    {
        guiStyle.fontSize = 50;
        guiStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(10, 10, 100, 20), "Generation" + generation, guiStyle);
        GUI.Label(new Rect(10, 65, 100, 20), "Trial Time" + (int)elapsed, guiStyle);
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-9, 9), Random.Range(-4.5f, 4.5f), 0);
            GameObject go = Instantiate(personPrefab, pos, Quaternion.identity);

            CamoDNA dna = go.GetComponent<CamoDNA>();
            dna.r = Random.Range(0.0f, 1.0f);
            dna.g = Random.Range(0.0f, 1.0f);
            dna.b = Random.Range(0.0f, 1.0f);
            dna.size = Random.Range(0.13f, 0.26f);

            population.Add(go);
        }
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed > trialTime)
        {
            BreedNewPopulation();
            elapsed = 0;
        }
    }
    private GameObject Breed(GameObject parent1, GameObject parent2)
    {
        Vector3 pos = new Vector3(Random.Range(-9, 9), Random.Range(-4.5f, 4.5f), 0);

        GameObject offspring = Instantiate(personPrefab, pos, Quaternion.identity);

        CamoDNA dna1 = parent1.GetComponent<CamoDNA>();
        CamoDNA dna2 = parent2.GetComponent<CamoDNA>();
        CamoDNA dnaOffspring = offspring.GetComponent<CamoDNA>();

        //Parte central do algoritmo genético MAIS IMPORTANTE
        //Versão com mutacao aleatoria (sem mutacao no fim do script)
        if (Random.Range(0, 100) > 1)  //50% de mutacao eh muito alto (no script esta com 1%)
        {
            dnaOffspring.r = Random.Range(0, 10) < 5 ? dna1.r : dna2.r;   
            dnaOffspring.g = Random.Range(0, 10) < 5 ? dna1.g : dna2.g; 
            dnaOffspring.b = Random.Range(0, 10) < 5 ? dna1.b : dna2.b;
            dnaOffspring.size = Random.Range(0, 10) < 5 ? dna1.size : dna2.size;
        }
        else
        {
            dnaOffspring.r = Random.Range(0.0f, 1.0f);
            dnaOffspring.g = Random.Range(0.0f, 1.0f);
            dnaOffspring.b = Random.Range(0.0f, 1.0f);
            dnaOffspring.size = Random.Range(0.13f, 0.26f);
        }
                                                                    
                                                                    
        return offspring;                                           
                                                                    
                                                                    
                                                                    
    }

    private void BreedNewPopulation()
    {
        List<GameObject> newPopulation = new List<GameObject>();
        //OrderBy vai na ordem crescente
        //OrderByDescending vai na decrescente
        List<GameObject> sortedList = population.OrderByDescending(o => o.GetComponent<CamoDNA>().timeToDie).ToList(); //Usa biblioteca LINQ pra ordenar a lista de acordo com o time to die 
                                                                                                                   //aqui esta nossa fitness function

        population.Clear();

        //gera filhos com os 50% mais aptos na lista
        for (int i = (int)(sortedList.Count / 2.0f) - 1; i < sortedList.Count - 1; i++) //pega metade de baixo da lista ordenada
        {
            //Loopa pela lista apartir dos 50% mais aptos
            population.Add(Breed(sortedList[i], sortedList[i + 1]));
            population.Add(Breed(sortedList[i + 1], sortedList[i]));
            //Tem Breed 2 vezes para manter o mesmo numero de populacao, ja que soh tem os 50% mais aptos, pega eles e faz 2x
        }

        for(int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }
        generation++;
    }



}
//Código genetico sem mutacao aleatoria
/*Parte central do algoritmo genetico MAIS IMPORTANTE
dnaOffspring.r = Random.Range(0, 10) < 5 ? dna1.r : dna2.r;  Mesma coisa que
dnaOffspring.g = Random.Range(0, 10) < 5 ? dna1.g : dna2.g;  if(Random.Range(0,10) < 5)
dnaOffspring.b = Random.Range(0, 10) < 5 ? dna1.b : dna2.b;  {
                                                               dna1.r
                                                             }
return offspring;                                            else
                                                             {
                                                               dna2.r
                                                             }*/