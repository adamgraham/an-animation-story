using UnityEngine;
using UnityEngine.UI;
using System.IO;

[RequireComponent(typeof(Text))]
public sealed class ScriptReader : MonoBehaviour
{
    public SceneDirector sceneDirector;
    public string textFilePath;
    public float characterDelay = 0.1f;
    public float lineDelay = 2.0f;
    public bool autoReadNextLine;

    private Text _textComponent;
    private string[] _lines;
    private string[] _sceneDirections;
    private int _currentLineIndex;
    private int _currentCharIndex;

    private void Awake()
    {
        _textComponent = GetComponent<Text>();
        _textComponent.text = "";

        TextAsset script = Resources.Load<TextAsset>(this.textFilePath);
        _lines = script.text.Split('\n');

        StripSceneDirections();
    }

    private void Start()
    {
        Play(this.lineDelay);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            SetNextLine();
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            SetPreviousLine();
        }
    }

    private void Play(float delay)
    {
        CancelInvoke();
        InvokeRepeating("SetNextCharacter", delay, this.characterDelay);
    }

    private void Stop()
    {
        CancelInvoke();
    }

    private void SetNextLine()
    {
        _currentLineIndex = Mathf.Clamp(++_currentLineIndex, 0, _lines.Length);
        _currentCharIndex = 0;

        Play(this.characterDelay);
    }

    private void SetPreviousLine()
    {
        _currentLineIndex = Mathf.Clamp(--_currentLineIndex, 0, _lines.Length);
        _currentCharIndex = 0;

        Play(this.characterDelay);
    }

    private void SetNextCharacter()
    {
        // Check that there is a valid line or character to read
        if (!this.enabled || _currentLineIndex >= _lines.Length || _currentCharIndex >= _lines[_currentLineIndex].Length) {
            return;
        }

        // Update scene if needed
        if (_currentCharIndex == 0 && this.sceneDirector != null) {
            this.sceneDirector.SetScene(_sceneDirections[_currentLineIndex]);
        }

        // Read the next character for the current line
        string line = _lines[_currentLineIndex];
        _textComponent.text = line.Substring(0, _currentCharIndex + 1);

        // Check if we should start the next line
        if (++_currentCharIndex >= line.Length && this.autoReadNextLine) {
            Invoke("SetNextLine", this.lineDelay);
        }
    }

    private void StripSceneDirections()
    {
        _sceneDirections = new string[_lines.Length];

        for (int i = 0; i < _lines.Length; i++)
        {
            string line = _lines[i];

            int startIndex = line.IndexOf("[");
            int endIndex = line.IndexOf("]");

            if (startIndex != -1 && endIndex != -1)
            {
                _sceneDirections[i] = line.Substring(startIndex, endIndex - startIndex + 1).Trim();
                _lines[i] = line.Substring(0, startIndex) + line.Substring(endIndex + 1);
            }
            else
            {
                _sceneDirections[i] = "";
            }
        }
    }

}
