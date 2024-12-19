using UnityEngine;
using UnityEngine.UI;

public class PadsDisplay : MonoBehaviour
{
    public GameObject row1Parent;
    public GameObject row2Parent;
    public GameObject row3Parent;
    public GameObject row4Parent;
    private Image[,] pads = new Image[4, 16];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 16; i++)
        {
            pads[0, i] = row1Parent.transform.GetChild(i).gameObject.GetComponent<Image>();
            pads[1, i] = row2Parent.transform.GetChild(i).gameObject.GetComponent<Image>();
            pads[2, i] = row3Parent.transform.GetChild(i).gameObject.GetComponent<Image>();
            pads[3, i] = row4Parent.transform.GetChild(i).gameObject.GetComponent<Image>();
        }
    }

    public void OnPadStatesUpdate(int[,] padStates)
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                if (padStates[i, j] == 1)
                {
                    pads[i, j].color = Color.red;
                }
                else
                {
                    pads[i, j].color = Color.white;
                }
            }
        }
    }
}
