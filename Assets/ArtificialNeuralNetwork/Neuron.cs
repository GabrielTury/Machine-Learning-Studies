using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron //basicamente um perceptron
{
    public int numInputs;//Recebe 1 input de cara neuron na layer anterior
    public double bias;
    public double output;
    public double errorGradient;

    public List<double> weights = new List<double>();
    public List<double> inputs = new List<double>();

    public Neuron (int nInputs)
    {
        bias = UnityEngine.Random.Range(-1.0f, 1.0f);
        numInputs = nInputs;

        for(int i = 0; i < numInputs; i++) //adiciona um peso aleatorio pra cada input recebido
        {
            weights.Add(UnityEngine.Random.Range(-1.0f, 1.0f));
        }
    }
}
