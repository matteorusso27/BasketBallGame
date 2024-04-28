using UnityEngine;
using UnityEngine.UI;
public class RangePresenter : MonoBehaviour
{
    [SerializeField] private Image FillBar;
    [SerializeField] private Image PerfectRange;
    [SerializeField] private Image BoardRange;

    public float GetPerfectRangeHeight() => PerfectRange.rectTransform.rect.height;
    public float GetBoardRangeHeight() => BoardRange.rectTransform.rect.height;
    public float GetFillBarHeight() => FillBar.rectTransform.rect.height;

    public void DrawRanges(float startRangePerfect, float startRangeBoard)
    {
        var startingPerfectRangePositionY = GetFillBarHeight() * startRangePerfect;
        PerfectRange.rectTransform.anchoredPosition = new Vector3(0, startingPerfectRangePositionY, 0);

        var startingBoardRangePositionY = GetFillBarHeight() * startRangeBoard;
        BoardRange.rectTransform.anchoredPosition = new Vector3(0, startingBoardRangePositionY, 0);
    }
}
