using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithmController : MonoBehaviour
{
    public float timeScale;
    private bool isInitialized = false;
    public int populationSize = 10;
    public int populationCount;
    private int winnerCount;

    public float averageFitnessCurrentPopulation;
    public float bestFitnessCurrentPopulation; //best fitness of this population
    public int bestScoreCurrentPopulation; //best score of this population

    public int bestPopulationNo; //average fitness was the best from what population
    public float bestPopulationAverageFitness; //average fitness was the best
    public float bestFitnessEver; //best fitness ever of a bird
    public int bestScoreEver; //best score ever of a bird

    //public int mutateRate;
    private double[][][][] newPopulationWeights;

    [JsonIgnore]
    public GameObject prefabBird;

    [JsonIgnore]
    public PopulationController populationController;
    public float[] best4Fitnesses = new float[4];
    public int[] best4Scores = new int[4];

    [JsonIgnore]
    public bool IsInitialized { get => isInitialized; }

    private void Update() {
        if (!GameControl.instance.gameOver) {
            if (populationController != null) {
                Time.timeScale = timeScale;
                EvaluateBestFitnessAndScoreCurrentPopulation();
                EvaluateAverageFitnessCurrentPopulation();
                GameObject[] fittestBirds = populationController.Selection(winnerCount);
                for (int i = 0; i < fittestBirds.Length; i++) {
                    best4Fitnesses[i] = fittestBirds[i].GetComponent<Bird>().Fitness;
                }
                for (int i = 0; i < fittestBirds.Length; i++) {
                    best4Scores[i] = fittestBirds[i].GetComponent<Bird>().Score;
                }

                bool allDead = true;
                foreach (GameObject birdGO in populationController.population) {
                    if (!birdGO.GetComponent<Bird>().IsDead) {
                        allDead = false;
                    }
                }
                if (allDead) {
                    checkBestPopulationBestFitnessBestScore();
                    GameObject[] winners = populationController.Selection(winnerCount);
                    newPopulationWeights = new double[populationSize][][][];

                    //4 offspring of 2 best winners
                    for (int i = 0; i < populationSize / 5 * 2; i++) {
                        newPopulationWeights[i] = CrossOverAndMutate(winners[0].GetComponent<Bird>(), winners[1].GetComponent<Bird>());
                    }

                    //6 offspring of random winners
                    System.Random random = new System.Random();
                    for (int i = populationSize / 5 * 2; i < populationSize; i++) {
                        int randomIndex = random.Next(0, winners.Length);
                        int randomIndex2 = random.Next(0, winners.Length);
                        while (randomIndex == randomIndex2) {
                            randomIndex2 = random.Next(0, winners.Length);
                        }
                        newPopulationWeights[i] = CrossOverAndMutate(winners[randomIndex].GetComponent<Bird>(), winners[randomIndex2].GetComponent<Bird>());
                    }
                    GameControl.instance.RestartScene();
                }
            }
        }
    }

    private void checkBestPopulationBestFitnessBestScore() {
        if (averageFitnessCurrentPopulation > bestPopulationAverageFitness) {
            bestPopulationAverageFitness = averageFitnessCurrentPopulation;
            bestPopulationNo = populationCount;
        }
        if (best4Fitnesses[0] > bestFitnessEver) {
            bestFitnessEver = best4Fitnesses[0];
        }
        if (best4Scores[0] > bestScoreEver) {
            bestScoreEver = best4Scores[0];
        }
    }

    private void EvaluateAverageFitnessCurrentPopulation() {
        float totalFitness = 0f;
        foreach (GameObject birdGo in populationController.population) {
            totalFitness += birdGo.GetComponent<Bird>().Fitness;
        }
        averageFitnessCurrentPopulation = totalFitness / populationSize;
    }

    private void EvaluateBestFitnessAndScoreCurrentPopulation() {
        foreach (GameObject birdGo in populationController.population) {
            if (birdGo.GetComponent<Bird>().Fitness > bestFitnessCurrentPopulation) {
                bestFitnessCurrentPopulation = birdGo.GetComponent<Bird>().Fitness;
                bestScoreCurrentPopulation = birdGo.GetComponent<Bird>().Score;
            }
        }
    }

    public void Initialize(int populationCount, int winnerCount) {
        this.populationSize = populationCount;
        this.winnerCount = winnerCount;
        bestPopulationAverageFitness = 0;
        bestPopulationNo = 1;
        Reset();
        populationController = new PopulationController(populationCount, prefabBird);
        populationController.CreatePopulation();
        this.populationCount += 1;
        isInitialized = true;
        timeScale = 1.0f;
    }


    public void ReInitialize() {
        Reset();
        populationController.CreatePopulation(newPopulationWeights);
        populationCount += 1;
    }

    public void Reset() {
        bestFitnessCurrentPopulation = 0;
        bestScoreCurrentPopulation = 0;
        averageFitnessCurrentPopulation = 0f;
        //mutateRate = 1;
    }

    public double[][][] CrossOverAndMutate(Bird mom, Bird dad) {
        int[] parameters = mom.NeuralNetwork.parameters;
        double[][][] weights = new double[parameters.Length - 1][][];
        int lenght = parameters.Length;

        for (int i = 0; i < parameters.Length - 1; i++) {

            weights[i] = new double[parameters[i]][];

            for (int j = 0; j < parameters[i]; j++) {

                weights[i][j] = new double[parameters[i + 1]];

                for (int k = 0; k < parameters[i + 1]; k++) {

                    if (Random.Range(0, 2) == 0) {
                        weights[i][j][k] = mom.NeuralNetwork.weights[i][j][k];
                    }
                    else {
                        weights[i][j][k] = dad.NeuralNetwork.weights[i][j][k];
                    }
                }
            }
        }

        //nur 1 weight wird mutiert -> besser: 1/10 aller weights
        int mutationLayer = Random.Range(0, weights.Length);
        int mutationLeft = Random.Range(0, weights[mutationLayer].Length);
        int mutationRight = Random.Range(0, weights[mutationLayer][mutationLeft].Length);

        weights[mutationLayer][mutationLeft][mutationRight] = getRandomWeight();

        //Debug.Log (mutationLayer + " " + mutationLeft + " " + mutationRight);
        return weights;
    }
    double getRandomWeight() {
        return Random.Range(-1.0f, 1.0f);
    }

    public FinalStats getFinalStats() {
        return new FinalStats(populationSize, populationCount, winnerCount, averageFitnessCurrentPopulation, bestFitnessCurrentPopulation, bestScoreCurrentPopulation, bestPopulationNo, bestPopulationAverageFitness, bestFitnessEver, bestScoreEver, best4Fitnesses, best4Scores, populationController.Selection(winnerCount)[0]);
    }
}
