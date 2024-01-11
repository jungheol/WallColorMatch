using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject mainPanel;
    public GameObject gamePanel;
    public Text gameScore;
    
    public void GameStart() {
        mainPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void UpdateScore(int score) {
        if (score < 10) {
            gameScore.text = score.ToString("D2");
        } else {
            gameScore.text = score.ToString();
        }
    }
}
