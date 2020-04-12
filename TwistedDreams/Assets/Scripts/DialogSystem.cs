using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    public GameObject textBoxPanel;
    public GameObject whoBoxPanel;

    public Text theText;
    public Text whoText;

    public TextAsset textFile;
    private string[] dialogLines;

    private int currentLine;
    private int endAtLine;

    public bool active = false;
    public bool running = false;

    // Start is called before the first frame update
    void Start()
    {
        if (textFile != null)
        {
            dialogLines = (textFile.text.Split('\n'));
        }

        if (endAtLine == 0)
        {
            endAtLine = dialogLines.Length - 1;
        }

        currentLine = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            running = true;
            textBoxPanel.SetActive(true);
            whoBoxPanel.SetActive(true);

            whoText.text = dialogLines[currentLine - 1];
            theText.text = dialogLines[currentLine];

            if (Input.GetKeyDown(KeyCode.Space))
                currentLine += 2;

            if (currentLine > endAtLine)
            {
                active = false;
                textBoxPanel.SetActive(false);
                whoBoxPanel.SetActive(false);
                running = false;

                if (textFile != null)
                {
                    dialogLines = (textFile.text.Split('\n'));
                }

                if (endAtLine == 0)
                {
                    endAtLine = dialogLines.Length - 1;
                }

                currentLine = 1;
            }
        }
    }
}
