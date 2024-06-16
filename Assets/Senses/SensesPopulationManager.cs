using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Senses
{
    public class SensesPopulationManager : MonoBehaviour
    {
        public GameObject botPrefab;
        public int populationSize = 50;

        List<GameObject> population = new List<GameObject>();

        public static float elapsed = 0;
        public float trialTime = 5;

        int generation;
        [SerializeField, Range(0, 10)]
        private float newTimeScale = 1;

        GUIStyle guiStyle = new GUIStyle();
        private void OnGUI()
        {

            guiStyle.fontSize = 25;
            guiStyle.normal.textColor = Color.white;
            GUI.BeginGroup(new Rect(10, 10, 250, 150));
            GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
            GUI.Label(new Rect(10, 25, 200, 30), "Gen: " + generation, guiStyle);
            GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time : {0:0.00}", elapsed), guiStyle);
            GUI.Label(new Rect(10, 75, 200, 30), "Population: " + population.Count, guiStyle);
            GUI.EndGroup();

        }
        void Start() //praticamente nao muda entre os AG
        {
            for (int i = 0; i < populationSize; i++) //cria a primeira populacao
            {
                Vector3 startingPos = new Vector3(this.transform.position.x + Random.Range(-2, 2), this.transform.position.y, this.transform.position.z + Random.Range(-2, 2));

                GameObject b = Instantiate(botPrefab, startingPos, this.transform.rotation);
                b.GetComponent<SensesBrain>().Init();
                population.Add(b);

            }

            //Time.timeScale = newTimeScale;
        }

        GameObject Breed(GameObject parent1, GameObject parent2) //praticamente nao muda entre os AG
        {
            Vector3 startingPos = new Vector3(this.transform.position.x + Random.Range(-2, 2), this.transform.position.y, this.transform.position.z + Random.Range(-2, 2));

            GameObject offspring = Instantiate(botPrefab, startingPos, this.transform.rotation);
            SensesBrain b = offspring.GetComponent<SensesBrain>();

            if (Random.Range(0, 100) == 1) //1% de mutacao
            {
                b.Init();
                b.dna.Mutate();
            }
            else
            {
                b.Init();
                b.dna.Combine(parent1.GetComponent<SensesBrain>().dna, parent2.GetComponent<SensesBrain>().dna);
            }

            return offspring;
        }

        void BreedNewPopulation() //praticamente nao muda entre os AG
        {
            List<GameObject> sortedList = population.OrderBy(o => 
            (o.GetComponent<SensesBrain>().timeAlive + o.GetComponent<SensesBrain>().timeWalking*5/*Esse 5 seria o peso adicionado, para o time walking ter mais peso*/)).ToList(); //Fitness Function no caso eh timeAlive + distance

            population.Clear();

            for (int i = (int)(sortedList.Count / 2.0f) - 1; i < sortedList.Count - 1; i++)
            {
                population.Add(Breed(sortedList[i], sortedList[i + 1]));
                population.Add(Breed(sortedList[i + 1], sortedList[i]));

            }

            for (int i = 0; i < sortedList.Count; i++)
            {
                Destroy(sortedList[i]);
            }

            generation++;
        }


        void Update()
        {
            elapsed += Time.deltaTime;

            if(elapsed >= trialTime)
            {
                BreedNewPopulation();
                elapsed = 0;
            }
        }

        [ContextMenu("SetTimeScale")]
        private void SetTimeScale()
        {
            Time.timeScale = newTimeScale;
        }
    }
}
