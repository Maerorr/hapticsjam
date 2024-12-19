using UnityEngine;

public class MInesDebug : MonoBehaviour
{
    public float minePos;
    public float indicatorPos;

    public Transform indicator;
    public Transform mine;
    
    void Update()
    {
        indicator.transform.position = new Vector3(x, indicator.transform.position, indicator.transform.position);
    }
}
