using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Senses
{
    public class SensesDNA
    {
        List<int> genes = new List<int>();

        int dnaLength = 0;
        int maxValues = 0;

        public SensesDNA(int l, int v)
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

        public void Combine(SensesDNA dna1, SensesDNA dna2)
        {
            for (int i = 0; i < dnaLength; i++)
            {
                if (i < dnaLength / 2.0)                    //Aqui esta combinando o DNA dos parentes de forma NAO aleatoria,
                {                                           //A primeira metade dos genes vem de 1 deles
                    int c = dna1.genes[i];                  //A segunda metade vem de outro
                    genes[i] = c;                           //
                }                                           //
                else                                        //
                {                                           //
                    int c = dna2.genes[i];                  //
                    genes[i] = c;                           //
                }                                           //
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
