using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Selectors;
public class ScorePresenter : MonoBehaviour
{
    public TMP_Text PlayerScore;
    public TMP_Text EnemyScore;
    private void Start()
    {
        ScoreM.PlayerScoreUpdate += (int a) => PlayerScore.text = "Player: "+a.ToString(); 
        ScoreM.EnemyScoreUpdate += (int a) => EnemyScore.text = "Enemy: "+a.ToString();
    }
}
