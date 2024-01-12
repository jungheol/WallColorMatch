using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public UIManager uiManager;
    public Transform wallPrefabs;
    public Transform leftWalls;
    public Transform rightWalls;

    public int currentLevel = 4;
    private int maxLevel = 7;
    private int currentScore = 0;
    
    public List<Color32> colors;
    public Player player;
    private float wallMaxScaleY = 20;

    private int[] wallCount = new int[7] { 1, 2, 3, 4, 5, 6, 7 };
    private int[] levelUpScore = new int[7] { 1, 2, 4, 8, 16, 32, 64 };

    private void Awake() {
        instance = this;
        SpawnWalls();
        SetColors();
    }

    IEnumerator Start() {
        while (true) {
            if (Input.GetMouseButtonDown(0)) {
                uiManager.GameStart();
                player.GameStart();
                
                yield break;
            }

            yield return null;
        }
    }

    private void SpawnWalls() {
        int numOfWalls = wallCount[currentLevel - 1];
        int currentWallCount = leftWalls.childCount;

        if (currentWallCount < numOfWalls) {
            for (int i = 0; i < numOfWalls - currentWallCount; ++i) {
                Instantiate(wallPrefabs, leftWalls);
                Instantiate(wallPrefabs, rightWalls);
            }
        }

        for (int i = 0; i < numOfWalls; i++) {
            Vector3 scale = new Vector3(0.5f, 1, 1);
            scale.y = wallMaxScaleY / numOfWalls;

            Vector3 position = Vector3.zero;
            position.y = scale.y * (numOfWalls / 2 - i) - (numOfWalls % 2 == 0 ? scale.y / 2 : 0);

            CommonUtil.SetTransform(leftWalls.GetChild(i), position, scale);
            CommonUtil.SetTransform(rightWalls.GetChild(i), position, scale);
        }
    }

    private void SetColors() {
        var tempColors = new List<Color32>();

        int[] indexs = CommonUtil.RandomNumbers(colors.Count, wallCount[currentLevel - 1]);
        for (int i = 0; i < indexs.Length; i++) {
            tempColors.Add(colors[indexs[i]]);
        }

        int colorCount = tempColors.Count;

        int[] leftWallIndexs = CommonUtil.RandomNumbers(colorCount, colorCount);
        for (int i = 0; i < leftWalls.childCount; i++) {
            leftWalls.GetChild(i).GetComponent<SpriteRenderer>().color = tempColors[leftWallIndexs[i]];
        }
        
        int[] rightWallIndexs = CommonUtil.RandomNumbers(colorCount, colorCount);
        for (int i = 0; i < rightWalls.childCount; i++) {
            rightWalls.GetChild(i).GetComponent<SpriteRenderer>().color = tempColors[rightWallIndexs[i]];
        }

        int index = Random.Range(0, tempColors.Count);
        player.GetComponent<SpriteRenderer>().color = tempColors[index];
    }

    public void CollisionWall() {
        currentScore++;
        uiManager.UpdateScore(currentScore);

        if (currentLevel < maxLevel && levelUpScore[currentLevel] < currentScore) {
            currentLevel++;
            
            SpawnWalls();
        }
        
        SetColors();
    }

    public void GameOver() {
        StartCoroutine(GameOverProcess());
    }
    IEnumerator GameOverProcess() {
        if (currentScore > PlayerPrefs.GetInt("BestScore")) {
            PlayerPrefs.SetInt("BestScore", currentScore);
        }
        
        uiManager.GameOver();

        while (true) {
            if (Input.GetMouseButtonDown(0)) {
                SceneManager.LoadScene(0);
            }

            yield return null;
        }
    }
}
