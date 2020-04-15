using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogSystem : MonoBehaviour
{
    public GameObject textBoxPanel;
    public GameObject whoBoxPanel;

    public Text contText;
    public Text theText;
    public Text whoText;

    private string cont = "Press space to continue...";
    private string fin = "Press space to finish...";

    public TextAsset textFile;
    private string[] dialogLines;
    private string currentText;

    private int currentLine;
    private int endAtLine;
    private bool finished = true;
    private bool finished_current_line = false;

    public bool active = false;
    public bool running = false;

    public float typeDelay = 0.01f;

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
        if (active && finished)
        {
            running = true;
            textBoxPanel.SetActive(true);
            whoBoxPanel.SetActive(true);

            whoText.text = dialogLines[currentLine - 1];

            if (!finished_current_line)
            {
                contText.text = fin;
                Coroutine st = StartCoroutine(showText(currentLine));
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (finished_current_line)
                {
                    currentLine += 2;
                    finished_current_line = false;
                }
                //else
                //{
                //    StopCoroutine(st);
                //    theText.text = dialogLines[currentLine];
                //    finished = true;
                //    finished_current_line = true;
                //}
            }

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

    IEnumerator showText(int lineNum)
    {
        finished = false;
        for (int i = 0; i < dialogLines[lineNum].Length; i++)
        {
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    theText.text = dialogLines[lineNum];
            //    finished = true;
            //    finished_current_line = true;
            //    contText.text = cont;
            //    yield break;
            //}
            currentText = dialogLines[lineNum].Substring(0, i);
            theText.text = currentText;
            yield return new WaitForSeconds(typeDelay);
        }
        finished = true;
        finished_current_line = true;
        contText.text = cont;
    }
}