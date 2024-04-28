using System.Collections;
using UnityEngine;
using System;
using static GameManager;
using static Selectors;
public class RangeManager : Singleton<RangeManager>
{
    private float startPerfectRange;
    private float endPerfectRange;
    private float startBoardRange;
    private float endBoardRange;

    [SerializeField]
    private RangePresenter RangePresenter;

    private const float RANGESDISTANCE = 0.35f;
    private const float TOLERANCE = 0.02f;
    private void Start()
    {
        CalculateGameRanges();
    }

    public void CalculateGameRanges(int difficulty = 2)
    {
        startPerfectRange = (1f / difficulty); //todo
        endPerfectRange = startPerfectRange + RangePresenter.GetPerfectRangeHeight() / RangePresenter.GetFillBarHeight();

        startBoardRange = startPerfectRange + RANGESDISTANCE;
        endBoardRange = startBoardRange + RangePresenter.GetBoardRangeHeight() / RangePresenter.GetFillBarHeight();

        RangePresenter.DrawRanges(startPerfectRange, startBoardRange);
    }
   

    public bool IsInsidePerfectRange(float value)
    {
        float adjustValueAtEnd = 0.01f;
        return value >= startPerfectRange && value <= endPerfectRange - adjustValueAtEnd;
    }

    public bool IsRegularRange(float value)
    {
        return value >= startPerfectRange - TOLERANCE && value <= endPerfectRange + TOLERANCE;
    }

    public bool IsBoardShoot(float value)
    {
        return value >= startBoardRange - TOLERANCE && value <= endBoardRange + TOLERANCE;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CalculateGameRanges(Helpers.GetRandomNumber(2, 5));
        }
    }
}
