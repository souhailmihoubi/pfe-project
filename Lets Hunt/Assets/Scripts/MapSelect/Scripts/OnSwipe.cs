using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GG.Infrastructure.Utils.Swipe;

public class OnSwipe : MonoBehaviour
{
    [SerializeField] private SwipeListener swipeListener;
    [SerializeField] private Image mapImage;
    [SerializeField] private TextMeshProUGUI mapName;
    [SerializeField] private TextMeshProUGUI mapDisc;

    private void OnEnable()
    {
        swipeListener.OnSwipe.AddListener(OnSwipee);
    }

    private void OnSwipee(string swipe)
    {
        if(swipe == "Right")
        {
            // right
            Debug.Log("fmkdf");
        }else 
            if(swipe == "Left")
        {
            // left
            Debug.Log("aaaaaaaaaa");
        }
    }

    private void OnDisable()
    {
        swipeListener?.OnSwipe.RemoveListener(OnSwipee);
    }
}
