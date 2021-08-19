using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBird {

    public class NeuralNetwork {
		public int[] parameters; //2,6,1
		public double[][][] weights;
		public int lenght;

		public NeuralNetwork(int[] parameters) {
			this.parameters = parameters;

			initializeVariables();

			for (int i = 0; i < parameters.Length - 1; i++) {
				weights[i] = new double[parameters[i]][];
				for (int j = 0; j < parameters[i]; j++) {
					weights[i][j] = new double[parameters[i + 1]];
					for (int k = 0; k < parameters[i + 1]; k++) {
						weights[i][j][k] = getRandomWeight();
						//Debug.Log (a);
					}
				}
			}
		}

		void initializeVariables() {
			this.weights = new double[parameters.Length - 1][][];
			this.lenght = parameters.Length;
		}

		public double[] process(double[] inputs) {
			//int a = 0;

			if (inputs.Length != parameters[0]) {
				Debug.Log("wrong input lenght!");
				return null;
			}

			double[] outputs;
			//Debug.Log (lenght);
			//for each layer
			for (int i = 0; i < (lenght - 1); i++) {
				//output values, they all start at 0 by default, checked that in C# Documentation ;)
				outputs = new double[parameters[i + 1]];

				//for each input neuron
				for (int j = 0; j < inputs.Length; j++) {

					//and for each output neuron
					for (int k = 0; k < outputs.Length; k++) {
						//Debug.Log (i + " " + j + " " + k);
						//a++;
						//increase the load of an output neuron by the value of each input neuron multiplied by the weight between them
						outputs[k] += inputs[j] * weights[i][j][k];
					}
				}

				//we have the proper output values, now we have to use them as inputs to the next layer and so on, until we hit the last layer
				inputs = new double[outputs.Length];

				//after all output neurons have their values summed up, apply the activation function and save the value into new inputs
				for (int l = 0; l < outputs.Length; l++) {
					inputs[l] = sigmoid(outputs[l] * 6);
					//Debug.Log ("i " + inputs [l]);
				}
			}

			//Debug.Log (a);
			return inputs;
		}

		double sigmoid(double x) {
			return 1 / (1 + Mathf.Exp(-(float) x));
		}

		double getRandomWeight() {
			return Random.Range(-1.0f, 1.0f);
		}
	}
}
