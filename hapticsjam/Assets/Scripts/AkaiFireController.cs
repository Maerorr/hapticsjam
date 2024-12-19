using System;
using System.Collections.Generic;
using UnityEngine;
using Minis;
using UnityEngine.InputSystem;

public class AkaiFireController : MonoBehaviour
{
    public static AkaiFireController Instance;

    public event Action<List<Vector2Int>> ButtonsJustPressed;
    private void Awake()
    {
        Instance = this;
    }

    public int[,] padStates = new int[4, 16];
    private int[,] previousPadStates = new int[4, 16];
    private int lowest_pad_num = 54;
    private int highest_pad_num = 117;
    private int pads_per_row = 16;
    private int rows = 4;

    public float[] knobsStates = new float[4];
    private int lowest_knob_num = 16;
    private int highest_knob_num = 19;
    private float knob_left_right_threshold = 0.5f;
    public float knob_sensitivity = 0.1f;

    public PadsDisplay padsDisplay;

    void Start()
    {
        InputSystem.onDeviceChange += (device, change) =>
        {
            if (!Application.isPlaying) return;
            if (change != InputDeviceChange.Added) return;

            var midiDevice = device as Minis.MidiDevice;
            if (midiDevice == null) return;

            midiDevice.onWillControlChange += (control, value) =>
            {
                // Debug.Log(string.Format(
                //     "Control Change #{0} ({1}) val:{2:0.00} ch:{3} dev:'{4}'",
                //     control.controlNumber,
                //     control.shortDisplayName,
                //     value,
                //     midiDevice.channel,
                //     midiDevice.description.product
                // ));

                // Calculate the knob number
                int knob_num = control.controlNumber;
                if (knob_num < lowest_knob_num || knob_num > highest_knob_num) return;
                knob_num -= lowest_knob_num;

                // Update the knob state
                if (value > knob_left_right_threshold)
                {
                    knobsStates[knob_num] += knob_sensitivity;
                }
                else
                {
                    knobsStates[knob_num] -= knob_sensitivity;
                }

                // Debug.Log(knobsStates[0] + " " + knobsStates[1] + " " + knobsStates[2] + " " + knobsStates[3]);

                padsDisplay.OnKnobStatesUpdate(knobsStates);
            };

            midiDevice.onWillNoteOn += (note, velocity) =>
            {
                // Note that you can't use note.velocity because the state
                // hasn't been updated yet (as this is "will" event). The note
                // object is only useful to specify the target note (note
                // number, channel number, device name, etc.) Use the velocity
                // argument as an input note velocity.
                // Debug.Log(string.Format(
                //     "Note On #{0} ({1}) vel:{2:0.00} ch:{3} dev:'{4}'",
                //     note.noteNumber,
                //     note.shortDisplayName,
                //     velocity,
                //     (note.device as Minis.MidiDevice)?.channel,
                //     note.device.description.product
                // ));

                // Calculate the pad number
                int pad_num = note.noteNumber;
                if (pad_num < lowest_pad_num || pad_num > highest_pad_num) return;
                pad_num -= lowest_pad_num;

                // Calculate the row and column
                int row = pad_num / pads_per_row;
                int col = pad_num % pads_per_row;

                // Update the pad state
                previousPadStates[row, col] = padStates[row, col];
                padStates[row, col] = 1;
                var justPressed = GetJustPressedButtons();
                ButtonsJustPressed?.Invoke(justPressed);

                // Update the pads display
                padsDisplay.OnPadStatesUpdate(padStates);
            };

            midiDevice.onWillNoteOff += (note) =>
            {
                // Debug.Log(string.Format(
                //     "Note Off #{0} ({1}) ch:{2} dev:'{3}'",
                //     note.noteNumber,
                //     note.shortDisplayName,
                //     (note.device as Minis.MidiDevice)?.channel,
                //     note.device.description.product
                // ));

                // Calculate the pad number
                int pad_num = note.noteNumber;
                if (pad_num < lowest_pad_num || pad_num > highest_pad_num) return;
                pad_num -= lowest_pad_num;

                // Calculate the row and column
                int row = pad_num / pads_per_row;
                int col = pad_num % pads_per_row;

                // Update the pad state
                previousPadStates[row, col] = padStates[row, col];
                padStates[row, col] = 0;
                
                // Update the pads display
                padsDisplay.OnPadStatesUpdate(padStates);
            };
        };
    }

    public List<Vector2Int> GetJustPressedButtons()
    {
        var ret = new List<Vector2Int>();
        
        for (int row = 0; row < padStates.GetLength(0); row++)
        {
            for (int col = 0; col < padStates.GetLength(1); col++)
            {
                if (padStates[row, col] != previousPadStates[row, col]) // state changed
                {
                    if (padStates[row, col] != 0) // 0 means its off = not pressed
                    {
                        ret.Add(new Vector2Int(col, row));
                    }
                }                
            }
        }

        return ret;
    }
}
