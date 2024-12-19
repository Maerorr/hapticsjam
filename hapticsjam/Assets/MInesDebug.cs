using UnityEngine;

public class MInesDebug : MonoBehaviour
{
    public float minePos01;
    public float indicatorPos01;

    public RectTransform indicator;
    public RectTransform mine;
    
    void Update()
    {
        {
            float x = indicatorPos01 * 500; // width of visualization
            indicator.anchoredPosition = new Vector2(x, indicator.anchoredPosition.y);
        }

        {
            float x = minePos01 * 500;
            mine.anchoredPosition = new Vector2(x, mine.anchoredPosition.y);
        }
    }
}
