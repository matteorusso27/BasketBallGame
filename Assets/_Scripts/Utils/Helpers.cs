using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helpers : MonoBehaviour
{
    public enum GameTag
    {
        Terrain,
        Hoop,
        ScoreUpdater
    }

    public static string GameTagToString(GameTag gt) => gt.ToString();

    public static System.Random RANDOM = new System.Random();

    public static int GetRandomNumber(int min, int max) => RANDOM.Next(min, max + 1);
}
