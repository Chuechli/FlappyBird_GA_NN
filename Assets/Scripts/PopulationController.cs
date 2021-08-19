using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationController
{
    private Color[] colors;

    public GameObject prefabBird;
    public GameObject[] population;

    public PopulationController(int populationSize, GameObject prefabBird) {
        this.prefabBird = prefabBird;
        population = new GameObject[populationSize];
        colors = new Color[populationSize];
        for (int i = 0; i < populationSize; i++) {
            colors[i] = Color.HSVToRGB(Random.value, 1f, 1f); //UnityEngine.Random.ColorHSV();

        }
    }

    public void CreatePopulation() {
        for (int i = 0; i < population.Length; i++) {
            GameObject bird = GameObject.Instantiate(prefabBird);
            bird.GetComponent<SpriteRenderer>().color = colors[i];
            bird.GetComponent<Bird>().AddNeuralNetwork(i, 2, 6, 1);
            population[i] = bird;

        }
        GameObject.Find("NeuralNetwork").GetComponent<NeuralNetworksController>().Initialize(population);
    }
    public void CreatePopulation(double[][][][] weights) {
        for (int i = 0; i < population.Length; i++) {
            GameObject bird = GameObject.Instantiate(prefabBird);
            bird.GetComponent<SpriteRenderer>().color = colors[i];
            bird.GetComponent<Bird>().AddNeuralNetwork(i, 2, 6, 1);
            bird.GetComponent<Bird>().NeuralNetwork.weights = weights[i];
            population[i] = bird;

        }
        GameObject.Find("NeuralNetwork").GetComponent<NeuralNetworksController>().Initialize(population);
    }

    public GameObject[] Selection(int winnerCount) {
        // sort the units of the current population	in descending order by their fitness
        System.Array.Sort(population, delegate (GameObject x, GameObject y) { return y.GetComponent<Bird>().Fitness.CompareTo(x.GetComponent<Bird>().Fitness); });

        GameObject[] winners = new GameObject[winnerCount];

        // mark the top units as the winners!
        for (var i = 0; i < winnerCount; i++) {
            population[i].GetComponent<Bird>().IsWinner = true;
            winners[i] = population[i];
        }

        // return an array of the top units from the current population
        return winners;
    }
}
