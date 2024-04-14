using UnityEngine;
using UnityEngine.UI;
public class SwipePresenter : MonoBehaviour
{
    [SerializeField] private SwipeListener Listener;
    [SerializeField] private Image         Fill;

    private void Start()
    {
        Listener.OnSwipeDetection += UpdateUI;
    }
    private void UpdateUI()
    {
        Fill.fillAmount = Listener.GetNormalizedDistance();
    }
}
