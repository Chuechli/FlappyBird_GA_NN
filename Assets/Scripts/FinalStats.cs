using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalStats {
    public int PopulationSize;
    public int PopulationCount;
    public int WinnerCount;

    public float AverageFitnessCurrentPopulation;
    public float BestFitnessCurrentPopulation; //best fitness of this population
    public int BestScoreCurrentPopulation; //best score of this population

    public int BestPopulationNo; //average fitness was the best from what population
    public float BestPopulationAverageFitness; //average fitness was the best
    public float BestFitnessEver; //best fitness ever of a bird
    public int BestScoreEver; //best score ever of a bird

    public float[] Best4Fitnesses = new float[4];
    public int[] Best4Scores = new int[4];

    public FlappyBird.NeuralNetwork NeuralNetworkBestBird;


    public FinalStats(int populationSize, int populationCount, int winnerCount, float averageFitnessCurrentPopulation, float bestFitnessCurrentPopulation, int bestScoreCurrentPopulation, int bestPopulationNo, float bestPopulationAverageFitness, float bestFitnessEver, int bestScoreEver, float[] best4Fitnesses, int[] best4Scores, GameObject bird) {
        PopulationSize = populationSize;
        PopulationCount = populationCount;
        WinnerCount = winnerCount;
        AverageFitnessCurrentPopulation = averageFitnessCurrentPopulation;
        BestFitnessCurrentPopulation = bestFitnessCurrentPopulation;
        BestScoreCurrentPopulation = bestScoreCurrentPopulation;
        BestPopulationNo = bestPopulationNo;
        BestPopulationAverageFitness = bestPopulationAverageFitness;
        BestFitnessEver = bestFitnessEver;
        BestScoreEver = bestScoreEver;
        Best4Fitnesses = best4Fitnesses;
        Best4Scores = best4Scores;
        NeuralNetworkBestBird = bird.GetComponent<Bird>().NeuralNetwork;
    }
}
