using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrainingSet
{
    public double[] input;
    public double output;
}
public class Perceptron : MonoBehaviour
{
    public TrainingSet[] ts;

    double[] weights = { 0, 0 };
    double bias = 0;
    double totalError = 0;

    public SimpleGrapher sg;

    double DotProductBias(double[] inWeights, double[] inInputs) //dot product que serve para calcular tudo e depois ver se ativou a funcao ou n
    {
        if (inWeights == null || inInputs == null)
            return -1;

        if(inWeights.Length != inInputs.Length) return -1;

        double d = 0;
        for(int x = 0; x < inWeights.Length; x++)
        {
            d += inWeights[x] * inInputs[x];
        }

        d += bias;

        return d;
    }

    double CalcOutput(int i) //activation function
    {
        double dp = DotProductBias(weights, ts[i].input);

        if (dp > 0) return (1);
        return 0;
    }

    void UpdateWeights(int j) //atualiza os pesos de acordo com a funcao
    {
        double error = ts[j].output - CalcOutput(j);
        totalError += Mathf.Abs((float)error);

        for(int i = 0; i < weights.Length; i++)
        {
            weights[i] = weights[i] + error* ts[j].input[i];
        }
        bias += error;
    }
    void InitializeWeights()
    {
        for(int i = 0; i < weights.Length; i++)
        {
            weights[i] = Random.Range(-1.0f, 1.0f);
        }
        bias = Random.Range(-1.0f, 1.0f);
    }
    double CalcOutput(double i1, double i2) //activation function que recebe inputs para testar o treino
    {
        double[] inp = new double[] { i1, i2 };

        double dp = DotProductBias(weights, inp);
        if (dp > 0) return (1);
        return 0;
    }
    void Train(int epochs)
    {
        InitializeWeights();

        for(int e =0; e < epochs; e++)
        {
            totalError = 0;
            for(int t = 0; t < ts.Length; t++)
            {
                UpdateWeights(t);
                Debug.Log("W1: " + (weights[0]) + "W2: " + (weights[1]) + "B: " + bias);
            }
            Debug.Log("TOTAL ERROR: " + totalError);
        }
    }

    void DrawAllPoints()
    {
        for (int i = 0; i < ts.Length; i++)
        {
            if (ts[i].output == 0)
            {
                sg.DrawPoint((float)ts[i].input[0], (float)ts[i].input[1], Color.magenta);
            }
            else
            {
                sg.DrawPoint((float)ts[i].input[0], (float)ts[i].input[1], Color.green);
            }
        }
    }
    void Start()
    {
        DrawAllPoints();

        Train(200);
        sg.DrawRay((float)(-(bias / weights[1]) / (bias / weights[0])), (float)(-bias / weights[1]), Color.red);
        
        if(CalcOutput(.3,.9) == 0)
            sg.DrawPoint(0.3f,0.9f, Color.red);
        else
            sg.DrawPoint(0.3f, 0.9f, Color.yellow);

        if (CalcOutput(.8, .1) == 0)
            sg.DrawPoint(0.8f, 0.1f, Color.red);
        else
            sg.DrawPoint(0.8f, 0.1f, Color.yellow);
    }

}
