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
}
