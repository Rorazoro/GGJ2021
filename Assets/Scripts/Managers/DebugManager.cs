using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugManager : SingletonBehaviour<DebugManager> {
    private string input;
    private Vector2 scroll;
    private List<string> consoleOutput;
    private List<string> commandLog;
    private int lastLogLength = 0;
    private int lastCommandIndex = 0;

    public bool showConsole;
    public GameObject player;
    public List<object> commandList;

    public static DebugCommand HELP;
    public static DebugCommand LIST_CAMERAS;
    public static DebugCommand<int> SET_ACTIVECAMERA;

    private void OnGUI () {
        if (!showConsole) { return; }

        float y = 0f;

        //if (consoleOutput.Count > 0) {
        GUI.Box (new Rect (0, y, Screen.width, 100), "");

        if (lastLogLength != consoleOutput.Count) {
            scroll = new Vector2 (0, 20 * consoleOutput.Count);
            lastLogLength = consoleOutput.Count;
        }

        Rect viewport = new Rect (0, 0, Screen.width - 30, 20 * consoleOutput.Count);
        scroll = GUI.BeginScrollView (new Rect (0, y + 5f, Screen.width, 90), scroll, viewport, false, true);

        for (int i = 0; i < consoleOutput.Count; i++) {
            string outputText = consoleOutput[i];
            Rect labelRect = new Rect (5, 20 * i, viewport.width - 100, 20);
            GUI.Label (labelRect, outputText);
        }

        GUI.EndScrollView ();

        y += 100;
        //}

        GUI.Box (new Rect (0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color (0, 0, 0, 0);

        GUI.SetNextControlName ("ConsoleTextField");
        input = GUI.TextField (new Rect (10f, y + 5f, Screen.width - 20f, 20f), input);
        GUI.FocusControl ("ConsoleTextField");
    }

    private void Awake () {
        commandLog = new List<string> ();
        consoleOutput = new List<string> ();

        HELP = new DebugCommand ("help", "Shows a list of commands", "help", () => {
            for (int i = 0; i < commandList.Count; i++) {
                DebugCommandBase command = commandList[i] as DebugCommandBase;
                string outputText = $"{command.commandFormat} - {command.commandDescription}";
                HandleConsoleOutput (outputText);
            }
        });

        LIST_CAMERAS = new DebugCommand ("list_cameras", "List all available cameras", "list_cameras", () => {
            HandleConsoleOutput (CameraManager.Instance.ListCameraNames ());
        });

        SET_ACTIVECAMERA = new DebugCommand<int> ("set_activecamera", "Sets the active camera via camera id", "set_activecamera <id>", (id) => {
            CameraManager.Instance.ToggleCamera (id);
        });

        commandList = new List<object> {
            HELP,
            LIST_CAMERAS,
            SET_ACTIVECAMERA
        };
    }

    // private void Update () {
    //     if (InputManager.Instance.ToggleConsoleInput) {
    //         ToggleConsole ();
    //     } else if (InputManager.Instance.ReturnInput) {
    //         ReturnInput ();
    //     } else if (InputManager.Instance.PreviousCommandInput) {
    //         GetPreviousCommand ();
    //     } else if (InputManager.Instance.NextCommandInput) {
    //         GetNextCommand ();
    //     }
    // }

    // private void ToggleConsole () {
    //     showConsole = !showConsole;
    //     if (showConsole) {
    //         InputManager.Instance.SwitchInputMap ("Debug");
    //         InputManager.Instance.SwitchCursorLockState (CursorLockMode.None);

    //     } else {
    //         string actionMap = InputManager.Instance.PreviousInputActionMap;
    //         CursorLockMode mode = InputManager.Instance.PreviousCursorLockState;

    //         InputManager.Instance.SwitchInputMap (actionMap);
    //         InputManager.Instance.SwitchCursorLockState (mode);
    //     }

    //     ClearConsoleInput ();
    // }
    private void ReturnInput () {
        if (showConsole) {
            HandleConsoleInput ();
            ClearConsoleInput ();
        }
    }

    private void GetPreviousCommand () {
        if (lastCommandIndex > 0) {
            lastCommandIndex--;
            input = commandLog[lastCommandIndex];
        }
    }

    private void GetNextCommand () {
        if (lastCommandIndex < commandLog.Count - 1) {
            lastCommandIndex++;
            input = commandLog[lastCommandIndex];
        }
    }

    private void HandleConsoleInput () {
        if (!string.IsNullOrWhiteSpace (input)) {
            commandLog.Add (input);
            consoleOutput.Add ($"${input}");
            lastCommandIndex = commandLog.Count;
        }

        string[] properties = input.Split (' ');
        //bool commandFound = false;

        for (int i = 0; i < commandList.Count; i++) {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;
            if (properties[0].Contains (commandBase.commandId)) {
                //commandFound = true;

                if (commandList[i] as DebugCommand != null) {
                    (commandList[i] as DebugCommand).Invoke ();
                } else if (commandList[i] as DebugCommand<float> != null) {
                    (commandList[i] as DebugCommand<float>).Invoke (float.Parse (properties[1]));
                } else if (commandList[i] as DebugCommand<int> != null) {
                    (commandList[i] as DebugCommand<int>).Invoke (int.Parse (properties[1]));
                } else if (commandList[i] as DebugCommand<string> != null) {
                    (commandList[i] as DebugCommand<string>).Invoke (properties[1]);
                } else if (commandList[i] as DebugCommand<bool> != null) {
                    (commandList[i] as DebugCommand<bool>).Invoke (bool.Parse (properties[1]));
                }

                break;
            }
        }
        // if (!commandFound) {
        //     consoleOutput.Add ($"$ERROR! - Command not found");
        // }
    }

    private void HandleConsoleOutput (string outputText) {
        string[] output = Regex.Split (outputText, "\r\n|\r|\n");
        foreach (string line in output) {
            consoleOutput.Add ($"   {line}");
        }
    }

    private void ClearConsoleInput () {
        input = "";
    }
}