using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;

public class PadsDisplay : MonoBehaviour
{
    public GameController gameController;
    public GameObject row1Parent;
    public GameObject row2Parent;
    public GameObject row3Parent;
    public GameObject row4Parent;
    private Image[,] pads = new Image[4, 16];

    public RectTransform knob1;
    public RectTransform knob2;
    public RectTransform knob3;
    public RectTransform knob4;

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

        // knob1.localScale = new Vector3(5.0f, 1.0f, 1.0f);
        // knob2.localScale = new Vector3(5.0f, 1.0f, 1.0f);
        // knob3.localScale = new Vector3(5.0f, 1.0f, 1.0f);
        // knob4.localScale = new Vector3(5.0f, 1.0f, 1.0f);
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
                    Vector2Int coords = new Vector2Int(j, i);
                    if (gameController.currentLevel.mineGridCoords == coords)
                    {
                        pads[i, j].color = Color.yellow;
                    }
                    else
                    {
                        pads[i, j].color = Color.white;
                    }
                }
            }
        }
    }

    public void OnKnobStatesUpdate(float[] knobsStates)
    {
        // set scale as 1.0f + knobState
        // knob1.localScale = new Vector3(5.0f + knobsStates[0], 1.0f, 1.0f);
        // knob2.localScale = new Vector3(5.0f + knobsStates[1], 1.0f, 1.0f);
        // knob3.localScale = new Vector3(5.0f + knobsStates[2], 1.0f, 1.0f);
        // knob4.localScale = new Vector3(5.0f + knobsStates[3], 1.0f, 1.0f);
    }
}
