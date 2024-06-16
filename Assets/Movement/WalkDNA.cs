using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkDNA
{

    List<int> genes = new List<int>(); //lista com os cromossomos
    int dnaLength = 0;
    int maxValues = 0;

    public WalkDNA(int l, int v)
    {
        dnaLength = l;
        maxValues = v;
        SetRandom();
    }

    public void SetRandom()
    {
        genes.Clear();
        for(int i = 0; i< dnaLength; i++)
        {
            genes.Add(Random.Range(0, maxValues));
        }
    }

    public void SetInt(int pos, int value)
    {
        genes[pos] = value;
    }
    /// <summary>
    /// Combina o dna dos parentes para ser o dna do filho(este objeto)
    /// </summary>
    /// <param name="d1">Parente 1</param>
    /// <param name="d2">Parente 2</param>
    public void Combine(WalkDNA d1, WalkDNA d2)
    {
        for(int i = 0; i < dnaLength; i++)
        {
            if(i < dnaLength / 2.0)                     //Aqui esta combinando o DNA dos parentes de forma NAO aleatoria,
            {                                           //A primeira metade dos genes vem de 1 deles
                int c = d1.genes[i];                    //A segunda metade vem de outro
                genes[i] = c;                           //
            }                                           //
            else                                        //
            {                                           //
                int c = d2.genes[i];                    //
                genes[i] = c;                           //
            }                                           //
        }
    }
    /// <summary>
    /// Mutacao aleatoria de um gene
    /// </summary>
    public void Mutate()
    {
        genes[Random.Range(0, dnaLength)] = Random.Range(0, maxValues);
    }
    /// <summary>
    /// Pega os genes
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public int GetGene(int pos)
    {
        return genes[pos];
    }
}
