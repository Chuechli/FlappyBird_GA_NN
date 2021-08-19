using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlappyBird;
using System;

public class NeuralNetworksController : MonoBehaviour
{
    public Bird[] birds;
    private GameObject closestColumn = null;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameControl.instance.gameOver) {
            GameObject oldCurrentColumn = closestColumn;
            for (int i = 0; i < birds.Length; i++) {
                if (!birds[i].IsDead) {
                    float leftPosXBird = birds[i].GetComponent<PolygonCollider2D>().bounds.center.x - birds[i].GetComponent<PolygonCollider2D>().bounds.size.x / 2;
                    findClosestColumn(leftPosXBird);
                    if (oldCurrentColumn != null && oldCurrentColumn != closestColumn) {
                        birds[i].Score += 1;
                        GameControl.instance.BirdScored();
                    }
                    if (closestColumn != null) {
                        double[] sensors = new double[2]; //input

                        BoxCollider2D collider = closestColumn.GetComponentInChildren<BoxCollider2D>();

                        sensors[0] = getSensorHorizontal(leftPosXBird, closestColumn.GetComponentInChildren<BoxCollider2D>().bounds.center, closestColumn.GetComponentInChildren<BoxCollider2D>().bounds.size.x);
                        sensors[1] = getSensorVertical(closestColumn.transform.position, birds[i].gameObject.transform.position);

                        Debug.DrawRay(new Vector2(leftPosXBird, birds[i].GetComponent<PolygonCollider2D>().bounds.center.y), Vector2.right * Convert.ToSingle(sensors[0]), Color.green, 0, true);
                        Debug.DrawRay(birds[i].gameObject.transform.position, Vector2.up * Convert.ToSingle(sensors[1]), Color.blue, 0, true);


                        double[] results = birds[i].NeuralNetwork.process(sensors);
                        if (results[0] > 0.5f) {
                            birds[i].DoFlap();
                        }
                    }
                }
            }
        }
    }

    private void findClosestColumn(float leftPosXBird) {
        float rightPosCurrentColumnX = 0f;
        float columnScaleX = 0f;
        if (closestColumn != null) {
            columnScaleX = closestColumn.GetComponentInChildren<BoxCollider2D>().bounds.size.x;
            rightPosCurrentColumnX = closestColumn.GetComponentInChildren<BoxCollider2D>().bounds.center.x + columnScaleX / 2;
        }

        GameObject[] columns = GameObject.FindGameObjectsWithTag("Wall");
        foreach (var item in columns) {
            float columnX = item.GetComponentInChildren<BoxCollider2D>().bounds.center.x;
            float rightPosColumnX = columnX + columnScaleX / 2;
            if (rightPosColumnX > leftPosXBird) {
                if (closestColumn == null) {
                    closestColumn = item;
                } else if (rightPosCurrentColumnX < leftPosXBird) {
                    closestColumn = item;
                } else if (rightPosColumnX < rightPosCurrentColumnX) {
                    //scale must not be considered as distance are too big anyway between 2 columns
                    closestColumn = item;
                }
            }
        }
    }

    public void Initialize(GameObject[] birds) {
        this.birds = new Bird[birds.Length];
        for (int i = 0; i < birds.Length; i++) {
            this.birds[i] = birds[i].GetComponent<Bird>();
        }
    }

    double getSensorVertical(Vector3 columnPos, Vector3 birdPos) {
        float distance = columnPos.y - birdPos.y;
        return distance;
    }

    double getSensorHorizontal(float leftPosXBird, Vector3 columnPos, float worldScaleX) {
        float distance = (columnPos.x + worldScaleX/2) - leftPosXBird;
        return distance;
    }
}
