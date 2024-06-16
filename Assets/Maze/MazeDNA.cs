using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    public class MazeDNA
    {
        List<int> genes = new List<int>();

        int dnaLength = 0;
        int maxValues = 0;

        public MazeDNA(int l, int v)
        {
            dnaLength = l;
            maxValues = v;

            SetRandom();
        }

        public void SetRandom()
        {
            genes.Clear();

            for (int i = 0; i < dnaLength; i++)
            {
                genes.Add(Random.Range(0, maxValues));
            }
        }

        public void SetInt(int pos, int value)
        {
            genes[pos] = value;
        }

        public void Combine(MazeDNA dna1, MazeDNA dna2)
        {
            for (int i = 0; i < dnaLength; i++)
            {
                if (i < dnaLength / 2.0)
                {
                    int c = dna1.genes[i];
                    genes[i] = c;
                }
                else
                {
                    int c = dna2.genes[i];
                    genes[i] = c;
                }
                //genes[i] = Random.Range(0, 10) > 4 ? dna1.genes[i] : dna2.genes[i];
            }
        }

        public void Mutate()
        {
            genes[Random.Range(0, dnaLength)] = Random.Range(0, maxValues);
        }

        public int GetGene(int pos)
        {
            return genes[pos];
        }

    }
}
