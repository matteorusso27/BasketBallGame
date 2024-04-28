using UnityEngine;
using UnityEngine.UI;
public class SwipePresenter : MonoBehaviour
{
    [SerializeField] private SwipeListener Listener;
    [SerializeField] private Image         Fill;
    [SerializeField] private Image         PerfectRange;
    [SerializeField] private Image         BoardingRange;

    private void Start()
    {
        Listener.OnSwipeDetection += UpdateUI;
    }
    private void UpdateUI()
    {
        Fill.fillAmount = Listener.GetNormalizedDistance();
    }
}
