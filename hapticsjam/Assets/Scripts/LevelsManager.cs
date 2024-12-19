using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public class LevelData
    {
        // basic gameplay assumes you "tune" the knob until you are near the mine
        // value and then look for position on the grid of buttons and when you think you're near you shoot.
        
        public List<float> mines; // values 0-1 on x axis where the mines are located
        public List<Vector2Int> mineGridCoords; // indexed same as mines
    }

    public Vector2Int numberMines;
    
    public LevelData GenerateLevel()
    {
        var level = new LevelData();
        int numMines = Random.Range(numberMines.x, numberMines.y);
        for (int i = 0; i < numMines; i++)
        {
            level.mines.Add(Random.value); // 0-1
            for (int j = 0; j < 10000; j++) // for testing
            {
                var coords1 = new Vector2Int(Mathf.FloorToInt(Random.Range(0, 17)), Mathf.FloorToInt(Random.Range(0, 5)));
                Assert.IsTrue(coords1.x >= 0 && coords1.x <= 16 && coords1.y >= 0 && coords1.y <= 4);
            }
            var coords = new Vector2Int(Mathf.FloorToInt(Random.Range(0, 17)), Mathf.FloorToInt(Random.Range(0, 5)));
            level.mineGridCoords.Add(coords);
        }

        return level;
    }

    private void Start()
    {
        // start game
        var level = GenerateLevel();
        UpdateDebugUI(level);
    }

    private void UpdateDebugUI(LevelData level)
    {
    }

    private void Update()
    {
        
    }
}
