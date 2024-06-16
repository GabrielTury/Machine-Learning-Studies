using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ANN;

//Base da grande maioria das Redes Neurais (tirando a activation Function)
public class ANN
{
    public enum ActivationFunctionType
        {
            TanH,
            Sigmoid,
            ReLu,
            LeakyReLu,
            SoftPlus
        }

    public int numInputs;
    public int numOutputs;
    public int numHidden;
    public int numNPerHidden;

    public double alpha;//tipo um weight mas eh um valor que determina o quao rapido a ANN aprende, 
    //afeta o quanto o resultado do training set afeta o resultado seguinte, podendo definir a porcentagem que cada uma influencia
    List<Layer> layers = new List<Layer>();

    public ANN(int nI, int nO, int nH, int nPH, double o)
    {
        numInputs = nI;
        numOutputs = nO;
        numHidden = nH;
        numNPerHidden = nPH;
        alpha = o;

        if(numHidden > 0)
        {
            layers.Add(new Layer(numNPerHidden, numInputs)); //cria a 1 layer

            for (int i = 0; i < numHidden-1; i++)
            {
                layers.Add(new Layer(numNPerHidden, numNPerHidden)); //crias as hidden layers 
            }

            layers.Add(new Layer(numOutputs, numNPerHidden));//cria a ultima layer
        }
        else
        {
            layers.Add(new Layer(numOutputs, numInputs));
        }
    }

    public List<double> Go(List<double> inputValues, List<double> desiredOutput)
    {
        List<double> inputs = new List<double>();
        List<double> outputs = new List<double>();

        if(inputValues.Count != numInputs)
        {
            Debug.Log("ERROR: Numer of Inputs must be: "+ numInputs);
            return outputs;
        }

        inputs = new List<double>(inputValues);

        for (int i = 0;i < numHidden + 1;i++) //loopa pela input layer, pelas hidden layers e pela outpur layer(+1)
        {
            if(i > 0) //no primeiro loop os inputs serão os colocados, depois os inputs serao o output da layer anterior
            {
                inputs = new List<double>(outputs);
            }
            outputs.Clear(); //limpa os outputs para popular de novo

            for (int j = 0; j < layers[i].numNeurons; j++) //loopa pelos neurons dentro a layer
            {
                double N = 0; //N é o valor do Peso * Input
                layers[i].neurons[j].inputs.Clear(); //limpa os inputs desse neuron quue estavam antes

                for(int k = 0; k < layers[i].neurons[j].numInputs; k++) //loopa por cada input de um unico neuron
                {
                    layers[i].neurons[j].inputs.Add(inputs[k]); //coloca os novos inputs 
                    N += layers[i].neurons[j].weights[k] * inputs[k]; //Peso * Input (operacao basica do perceptron) DOT PRODUCT
                }

                N -= layers[i].neurons[j].bias; //subtrai o bias para ter aquela linha do grafico

                if(i == numHidden)
                {
                    layers[i].neurons[j].output = ActivationFunctionOutput(N);//faz os outputs que chegam para a output layer passarem por outra activation function
                }
                else
                {
                    layers[i].neurons[j].output = ActivationFunction(N); //faz cada output ( da hidden layer) passar pela activation function
                }

                outputs.Add(layers[i].neurons[j].output); //popula a lista de output para passar para a proxima layer

            }
        }
            UpdateWeights(outputs, desiredOutput);
        
           

        return outputs;
    }

    void UpdateWeights( List<double> outputs, List<double> desiredOutput )
    {
        double error;

        for(int i = numHidden; i >= 0; i--)// loopa pelas layers (esta com i-- para fazer o caminho inverso e propagar de tras para frente o erro e atualizar os pesos
        {
            for(int j = 0; j < layers[i].numNeurons; j++) //loopa pelos neurons
            {
                if(i == numHidden) //se esta na output layer determina qual é o erro
                {
                    error = desiredOutput[j] - outputs[j];
                    layers[i].neurons[j].errorGradient = ErrorGradientFunctionOutput(layers[i].neurons[j].output, error, ActivationFunctionType.Sigmoid); //errorGradient da o erro daquele neuron em particular o quao responsavel o neuron eh pelo erro
                    //todos os errorGradient somados dao o erro total calculado pelo desiredOutput - outputs;
                    // Delta Rule
                    
                }
                else
                {
                    layers[i].neurons[j].errorGradient = ErrorGradientByFunction(i, j, ActivationFunctionType.Sigmoid);
                    double errorGradSum = 0; //soma dos errorGradients

                    for(int p = 0; p < layers[i+1].numNeurons; p++) //para cade neuron na layer posterior, adiciona o error gradient deles para o total
                    {
                        errorGradSum += layers[i + 1].neurons[p].errorGradient * layers[i + 1].neurons[p].weights[j];
                    }
                    layers[i].neurons[j].errorGradient *= errorGradSum;// aplica o "valor de culpa" ao neuron
                }
                for (int k = 0; k < layers[i].neurons[j].numInputs; k++) // loopa pelos inputs de um neuron
                {
                    if(i == numHidden) // se for a output layer
                    {
                        error = desiredOutput[j] - outputs[j]; //seta o erro
                        layers[i].neurons[j].weights[k] += alpha * layers[i].neurons[j].inputs[k] * error; //atualiza o peso com os inputs alpha e erro
                    }
                    else
                    {
                        layers[i].neurons[j].weights[k] += alpha * layers[i].neurons[j].inputs[k] * layers[i].neurons[j].errorGradient;//se n for a output layer atualiza como o errorGradient
                                                                                                                                       // para ter a parcel ad a culpa
                    }
                }
                layers[i].neurons[j].bias += alpha * -1 * layers[i].neurons[j].errorGradient;//atualiza o bias do neuron
            }

        }

    }

    double ErrorGradientByFunction(int i, int j, ActivationFunctionType activationFunctionType)
    {
        double result = 0;
        switch (activationFunctionType)
        {
            case ActivationFunctionType.TanH:
                result = 1 - System.Math.Pow(System.Math.Tanh(layers[i].neurons[j].output), 2);
                break;
            case ActivationFunctionType.Sigmoid:
                result = layers[i].neurons[j].output * (1 - layers[i].neurons[j].output);
                break;
            case ActivationFunctionType.ReLu:
                if (layers[i].neurons[j].output > 0)
                {
                    result = 1;
                }
                else 
                { 
                    result = 0; 
                }
                break;
            case ActivationFunctionType.LeakyReLu:
                if (layers[i].neurons[j].output > 0)
                {
                    result = 1;
                }
                else
                {
                    result = 0.01;
                }
                break;
            case ActivationFunctionType.SoftPlus:
                result = 1/ (1+ System.Math.Exp(-1 * layers[i].neurons[j].output));
                break;
        }
        
        return result;
    }

    double ErrorGradientFunctionOutput(double outputs, double error, ActivationFunctionType activationFunctionType)
    {

        double result  = 0;
        switch (activationFunctionType)
        {
            case ActivationFunctionType.TanH:
                result = 1 - System.Math.Pow(System.Math.Tanh(outputs), 2);
                break;
            case ActivationFunctionType.Sigmoid:
                result = outputs * (1 - outputs);
                break;
            case ActivationFunctionType.ReLu:
                if (outputs > 0)
                {
                    result = error;
                }
                else { result = 0; }
                break;
            case ActivationFunctionType.LeakyReLu:
                if (outputs > 0)
                {
                    result = error;
                }
                else
                {
                    result = 0.01;
                }
                break;
            case ActivationFunctionType.SoftPlus:
                result = 1 / (1 + System.Math.Exp(-1 * outputs));
                break;
        }

        return result;
    }
    //Lista de activation functions na wikipedia
    double ActivationFunction(double value)
    {
        return Sigmoid(value);
    }

    double ActivationFunctionOutput(double value)
    {
        return Sigmoid(value);
    }

    double Step(double value) // binary step (usada no perceptron simples pra aprender OR e AND
    {
        if (value < 0) return 0;
        else return 1;
    }

    double Sigmoid(double value) //Logistic softstep 
    {
        double k = (double) System.Math.Exp(value);
        return k / (1.0f + k);
    }

    double TanH(double value)
    {
        return (2 * (Sigmoid(2 * value)) - 1);
    }

    double ReLu(double value)
    {
        if(value > 0) return value;
        else return 0;
    }
    double LeakyReLu(double value)
    {
        if (value < 0) return 0.01f * value;
        else return value;
    }
    double Sinusoid(double value)
    {
        return Mathf.Sin((float)value);
    }

    double ArcTan(double value)
    {
        return Mathf.Atan((float)value);
    }

    double SoftSign(double value)
    {
        return value / (1 + Mathf.Abs((float)value));
    }


}
