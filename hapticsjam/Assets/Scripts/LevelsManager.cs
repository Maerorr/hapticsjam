using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public class LevelData
    {
        // basic gameplay assumes you "tune" the knob until you are near the mine
        // value and then look for position on the grid of buttons and when you think you're near you shoot.
        
        public float mine; // values 0-1 on x axis where the mines are located
        public Vector2Int mineGridCoords; // indexed same as mines
    }

    public MInesDebug minesDebug;
    private float currentIndicatorX01;
    private LevelData currentLevel;
    
    public LevelData GenerateLevel()
    {
        var level = new LevelData();
        level.mine = Random.value; // 0-1
        for (int j = 0; j < 10000; j++) // for testing
        {
            var coords1 = new Vector2Int(Mathf.FloorToInt(Random.Range(0, 17)), Mathf.FloorToInt(Random.Range(0, 5)));
            Assert.IsTrue(coords1.x >= 0 && coords1.x <= 16 && coords1.y >= 0 && coords1.y <= 4);
        }
        var coords = new Vector2Int(Mathf.FloorToInt(Random.Range(0, 17)), Mathf.FloorToInt(Random.Range(0, 5)));
        level.mineGridCoords = (coords);

        return level;
    }

    private void Start()
    {
        NextLevel();
    }

    public void NextLevel()
    {
        currentLevel = GenerateLevel();
    }
    
    private void UpdateDebugUI(LevelData level)
    {
        minesDebug.minePos01 = level.mine;
        minesDebug.indicatorPos01 = currentIndicatorX01;
    }

    private float lastKnobState;
    
    private void Update()
    {
        if (Application.isPlaying == false)
        {
            return;
        }

        float knobDelta = lastKnobState - AkaiFireController.Instance.knobsStates[0];
        lastKnobState = AkaiFireController.Instance.knobsStates[0];

        // stupid
        currentIndicatorX01 += knobDelta;
        while (currentIndicatorX01 > 1)
        {
            currentIndicatorX01 -= 1;
        }
        while (currentIndicatorX01 < 0)
        {
            currentIndicatorX01 += 1;
        }
        
        Debug.Log(currentIndicatorX01);   
        
        UpdateDebugUI(currentLevel);
    }
}