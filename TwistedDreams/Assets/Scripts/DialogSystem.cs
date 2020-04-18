using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogSystem : MonoBehaviour
{
    public GameObject textBoxPanel;
    public GameObject whoBoxPanel;
    public GameObject contPanel;

    public Text contText;
    public Text theText;
    public Text whoText;
    public Text autoText;
    public Text writefText;

    private string cont = "Press Q to continue...";
    private string fin = "Press Q to finish...";
    private string enabWrit = "Press Z to enable writing effect...";
    private string disabWrit = "Press Z to disable writing effect...";
    private string enabAuto = "Press X to automatically continue...";
    private string disabAuto = "Press X to stop automatic continue...";

    private bool autoDialog;
    private bool writtingEffect;

    private float autoTime;

    public TextAsset textFile;
    private string[] dialogLines;
    private string currentText;

    private int currentLine;
    private int endAtLine;
    private bool finished = true;
    private bool finished_current_line = false;

    public bool active = false;
    public bool running = false;

    private bool call;

    private Coroutine st;
    private Coroutine autoDialogWait;

    public float typeDelay = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        autoDialog = false;
        autoText.text = enabAuto;
        contPanel.SetActive(true);
        writtingEffect = true;
        writefText.text = disabWrit;

        call = autoDialog;

        autoTime = 2.0f;

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

            writeLine();

            // Next Line on dialog
            if (!autoDialog)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    if (finished_current_line)
                    {
                        currentLine += 2;
                        finished_current_line = false;
                    }
                }
            }
            else
            {
                if (finished_current_line)
                {
                    if (call)
                    {
                        autoDialogWait = StartCoroutine(DialogCont());
                        call = false;
                    }
                }
            }

            if (currentLine > endAtLine)
            {
                textBoxPanel.SetActive(false);
                whoBoxPanel.SetActive(false);
                running = false;
                active = false;
            }
        }
    }

    private void writeLine()
    {
        if (currentLine <= endAtLine)
        {
            // Initiate Writing of current line of dialog
            if (writtingEffect)
            {
                if (!finished_current_line)
                {
                    contText.text = fin;
                    st = StartCoroutine(showText(currentLine));
                }
            }
            else
                finishText(false);
        }
    }

    public bool getFinishedCurrLine()
    {
        return finished_current_line;
    }

    public bool getFinished()
    {
        return finished;
    }

    public bool getautoDialog()
    {
        return autoDialog;
    }


    public void changeWrittingEffect()
    {
        if (writtingEffect)
        {
            writtingEffect = false;
            writefText.text = enabWrit;
        }
        else
        {
            writtingEffect = true;
            writefText.text = disabWrit;
        }
    }

    public void changeAutoDialog()
    {
        if (autoDialog)
        {
            autoDialog = false;
            autoText.text = enabAuto;
            contPanel.SetActive(true);
        }
        else
        {
            autoDialog = true;
            autoText.text = disabAuto;
            contPanel.SetActive(false);
        }
        call = autoDialog;
    }

    public void finishText(bool stopST)
    {
        if(stopST)
            StopCoroutine(st);
        theText.text = dialogLines[currentLine];
        finished = true;
        finished_current_line = true;
        contText.text = cont;
    }

    IEnumerator showText(int lineNum)
    {
        finished = false;
        for (int i = 0; i < dialogLines[lineNum].Length; i++)
        {
            currentText = dialogLines[lineNum].Substring(0, i);
            theText.text = currentText;
            yield return new WaitForSeconds(typeDelay);
        }
        finished = true;
        finished_current_line = true;
        contText.text = cont;
    }

    IEnumerator DialogCont()
    {
        yield return new WaitForSeconds(autoTime);
        currentLine += 2;
        finished_current_line = false;
        call = true;
    }
}