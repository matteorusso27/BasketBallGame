using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Selectors;
public class UIPresenter : MonoBehaviour
{
    public TMP_Text Time;
    public TMP_Text PlayerScore;
    public TMP_Text EnemyScore;
    public Image    EnergyBar;

    private float fillAmount = 0.25f;
    private void Start()
    {
        ScoreM.PlayerScoreUpdate += OnPlayerScore; 
        ScoreM.EnemyScoreUpdate += (int a) => EnemyScore.text = "Enemy: "+a.ToString();
        GameM.OnTimerChange += (int a) => Time.text = "Time: "+a.ToString();
        EnergyBar.fillAmount = 0f;
    }

    public void OnPlayerScore(int score, int energyBarScore)
    {
        PlayerScore.text = "Player: " + score.ToString();
        EnergyBar.fillAmount = energyBarScore * fillAmount;
    }
}
