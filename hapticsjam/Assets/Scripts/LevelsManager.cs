using System.Collections;
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

        public float mine; // values 0-1 on x axis where the mines are located
        public Vector2Int mineGridCoords; // indexed same as mines
    }

    public MInesDebug minesDebug;
    private float currentIndicatorX01;
    public LevelData currentLevel;
    public AkaiFireController input;

    public SoundManager sm;

    public LevelData GenerateLevel()
    {
        var level = new LevelData();
        level.mine = Random.value; // 0-1
        // for (int j = 0; j < 10000; j++) // for testing
        // {
            // var coords1 = new Vector2Int(Mathf.FloorToInt(Random.Range(0, 17)), Mathf.FloorToInt(Random.Range(0, 5)));
            // Assert.IsTrue(coords1.x >= 0 && coords1.x <= 16 && coords1.y >= 0 && coords1.y <= 4);
        // }
        var coords = new Vector2Int(Mathf.FloorToInt(Random.Range(0, 16)), Mathf.FloorToInt(Random.Range(0, 4)));
        level.mineGridCoords = (coords);

        return level;
    }

    private void Start()
    {
        sm = FindAnyObjectByType<SoundManager>();
        NextLevel();
    }

    private void OnEnable()
    {
        input.ButtonsJustPressed += OnButtonsJustPressed;
        input.ShootButtonPressed += TryShootTorpedo;
    }

    private void OnDisable()
    {
        input.ButtonsJustPressed -= OnButtonsJustPressed;
        input.ShootButtonPressed -= TryShootTorpedo;
    }

    private Vector2Int selectedTarget;

    private void OnButtonsJustPressed(List<Vector2Int> buttons)
    {
        if (isTuned == false)
        {
            sm.DeafSound();
            return;
        }

        Vector2Int lastButton = buttons[^1];
        selectedTarget = lastButton;
        
        sm.CheckSound(selectedTarget, currentLevel.mineGridCoords);
    }

    private void TryShootTorpedo()
    {
        if (isTuned == false)
        {
            return;
        }

        if (selectedTarget == currentLevel.mineGridCoords)
        {
            // shot correctly
            RumbleController.Instance.PlayRumbleForSeconds(2.0f);
            sm.LaunchRocketSound(selectedTarget.x, true);
            StartCoroutine(DelayedNextLevel());
        }
        else
        {
            // missed
            sm.LaunchRocketSound(selectedTarget.x, false);
        }
    }

    IEnumerator DelayedNextLevel()
    {
        yield return new WaitForSeconds(1.75f);
        sm.NextLevelSound();
        yield return new WaitForSeconds(2.0f);
        NextLevel();
    }

    private void NextLevel()
    {
        currentLevel = GenerateLevel();
        Debug.Log("level generated");
    }

    private void UpdateDebugUI(LevelData level)
    {
        minesDebug.minePos01 = level.mine;
        minesDebug.indicatorPos01 = currentIndicatorX01;
        minesDebug.mine.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500 * bandWidth);
    }

    private float lastKnobState;

    private void Update()
    {
        if (Application.isPlaying == false)
        {
            return;
        }

        MoveMineSeeker();
        HandleSonarSeeking();
        UpdateDebugUI(currentLevel);
    }

    public float bandWidth = 0.1f; // band is cenetered on the mine
    public bool isTuned;

    private float lastToMineSigned;
    private void HandleSonarSeeking()
    {
        float toMineAbs = Mathf.Abs(currentLevel.mine - currentIndicatorX01);

        if (toMineAbs > bandWidth / 2) // /2 bc mine is centered
        {
            // too far away from mine, we're not tuned
            isTuned = false;
        }
        else
        {
            // we are within tolerance, so we are tuned and can now search with buttons
            isTuned = true;
        }

        if (toMineAbs <= bandWidth) // do not play beep when we change sign due to wrapping around from 0 to 1
        {
            float toMineSigned = (currentLevel.mine - currentIndicatorX01);
            if (lastToMineSigned != 0 && Mathf.Sign(toMineSigned) * Mathf.Sign(lastToMineSigned) < 0) // different signs
            {
                sm.SonarSound(Mathf.FloorToInt(currentLevel.mine * 16f));
            }
            lastToMineSigned = toMineSigned;
        }
    }

    private void MoveMineSeeker()
    {
        if (AkaiFireController.Instance == null)
        {
            Debug.LogError("Akai is null");
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

        // Debug.Log(currentIndicatorX01);   
    }
}
