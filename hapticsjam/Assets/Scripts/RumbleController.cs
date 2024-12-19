using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class RumbleController : MonoBehaviour
{
    public static RumbleController Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private float rumbleTime;
    public float lowFreq = 1.0f; // below 0.25 there is no rumble, and it seems that the rumble is either 0 or 100%
    public float highFreq = 1.0f;

    public void PlayRumbleForSeconds(float seconds)
    {
        rumbleTime = seconds;
    }
    
    public void Update()
    {
        Gamepad.current.SetMotorSpeeds(0, 0);
        rumbleTime -= Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayRumbleForSeconds(1.0f);
        }
        
        if (rumbleTime > 0)
        {
            var gp = Gamepad.current;
            if (gp != null)
            {
                gp.SetMotorSpeeds(lowFreq, highFreq);
            }
            else
            {
                Debug.Log("gamepad is null");
            }
        }
    }
}
