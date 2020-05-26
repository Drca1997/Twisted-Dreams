using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogSystem : MonoBehaviour
{
    [Tooltip("Referência ao objeto que contem a caixa de texto, para poder meter e tirar do ecra")]
    public GameObject textBoxPanel;
    [Tooltip("Referência ao objeto contem a caixa de texto de quem esta a falar, para poder meter e tirar do ecra")]
    public GameObject whoBoxPanel;
    [Tooltip("Referência ao objeto contem a caixa de texto com a informação sobre qual o botao para continuar o dialogo, para poder meter e tirar do ecra")]
    public GameObject contPanel;
    [Tooltip("Referência ao objeto que contem a caixa de historico do dialogo, para poder meter e tirar do ecra")]
    public GameObject logPanel;

    [Tooltip("Referência ao objeto de texto para continuar, para poder mudar o seu conteudo")]
    public Text contText;
    [Tooltip("Referência ao objeto de texto para mudar o que esta a ser dito, para poder mudar o seu conteudo")]
    public Text theText;
    [Tooltip("Referência ao objeto de texto para mudar quem esta a falar, para poder mudar o seu conteudo")]
    public Text whoText;
    [Tooltip("Referência ao objeto de texto para ativar/desativar o continuar automatico, para poder mudar o seu conteudo")]
    public Text autoText;
    [Tooltip("Referência ao objeto de texto para ativar/desativar o efeito de escrita, para poder mudar o seu conteudo")]
    public Text writefText;
    [Tooltip("Referência ao objeto de texto para ver o historico de dialogo, para poder mudar o seu conteudo")]
    public Text logText;

    // aqui estao variaveis que mudam o conteudo de alguns dos textos acima
    private string cont = "Press Q to continue...";
    private string fin = "Press Q to finish...";
    private string enabWrit = "Press Z to enable writing effect...";
    private string disabWrit = "Press Z to disable writing effect...";
    private string enabAuto = "Press X to automatically continue...";
    private string disabAuto = "Press X to stop automatic continue...";

    [Tooltip("boolean que mantem o estado do continuar automatico, isto é, se está ou nao ativo.")]
    private bool autoDialog;
    [Tooltip("boolean que mantem o estado do efeito de escrita, isto é, se está ou nao ativo.")]
    private bool writtingEffect;

    [Tooltip("Referencia ao guiao da cena")]
    public TextAsset textFile;
    [Tooltip("Array de strings do guiao. As linhas alternam entre quem diz e o que diz, ou seja, a primeira linha tem quem diz, a segunda o que diz, a terceira quem diz, quarta o que diz, etc")]
    private string[] dialogLines;

    [Tooltip("Variavel que contem o indice da proxima linha de dialogo a ser dita. Assim, e sempre aumentada de 2 em 2. Para aceder a quem diz, usa-se currentLine - 1")]
    private int currentLine;
    [Tooltip("Variavel que contem o indice da ultima linha de dialogo.")]
    private int endAtLine;

    [Tooltip("Boolean que diz se a linha atual de dialogo ja acabou de ser imprimido no ecra com o efeito de escrita. Difere do abaixo descrito pois permite ao script playerinput averiguar se pode dar skip ao efeito de escrita.")]
    private bool WEfinished = true;
    [Tooltip("Boolean que diz se a linha atual de dialogo ja foi impresso na totalidade no ecra.")]
    private bool finished_current_line = false;

    [Tooltip("Variavel que serve para ativar o sistema de dialogo de fora.")]
    public bool active = false;

    [Tooltip("Variavel necessaria para a espera do continuar automatico. Diz se o wait deve ser chamado novamente. Se nao houvesse, apenas era chamada uma vez.")]
    private bool call;

    [Tooltip("Variavel que mantém a co-rotina para o efeito de escrita, para que esta possa ser parada caso o efeito de escrita seja desligado.")]
    private Coroutine st;

    [Tooltip("Variavel que mantem a velocidade com que as letras sao impressas no ecra com o efeito de escrita.")]
    public float typeDelay = 0.01f;
    [Tooltip("Rapidez do continuar automatico.")]
    private float autoTime = 0.2f;

    [Tooltip("Variavel que diz se se pode mexer ou nao durante o dialogo.")]
    [SerializeField]
    private bool movable;

    private bool nonindep;

    private bool waitindep;

    private Coroutine autoDialogWait;

    private void Awake()
    {
        nonindep = false;
        writefText.text = "";

        // Carregar linhas de dialogo
        if (textFile != null)
        {
            dialogLines = (textFile.text.Split('\n'));
        }

        // Definir index do ultimo dialogo
        if (endAtLine == 0)
        {
            endAtLine = dialogLines.Length - 1;
        }

        // Linha inicial. Começa do 1 pois o indice 0 contem quem disse, enquanto que o indice 1 tem o que disse. Aumenta sempre de 2 em 2.
        currentLine = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Comecar com definiçoes default: continuar automatico desligado; efeito de escrita ligado.
        autoDialog = PlayerPrefs.GetInt("AutoDialog") == 1 ? true : false;
        if (autoDialog)
        {
            autoText.text = disabAuto;
        }
        else
        {
            autoText.text = enabAuto;
        }
        contPanel.SetActive(true);
        writtingEffect = true;

        call = autoDialog;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0.0f)
        {
            // So faz update se dialogo tiver sido ativado, o efeito de escrita tiver acabado ou ainda n tiver começado e o jogador n esteja a verificar o historico
            if (active && WEfinished && !getLogStatus() && !nonindep)
            {
                // mostrar caixas de dialogo e de quem disse
                textBoxPanel.SetActive(true);
                whoBoxPanel.SetActive(true);

                // Meter quem disse na caixa apropriada
                whoText.text = dialogLines[currentLine - 1];

                // Meter o que disse na caixa de dialogo
                writeLine();

                // Next Line on dialog 
                // Input para avançar no dialogo - So se o dialogo automatico estiver desligado e o dialogo atual tiver terminado de ser escrito.
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    if (finished_current_line)
                    {
                        if (call)
                        {
                            StopCoroutine(autoDialogWait);
                            call = false;
                        }
                        currentLine += 2;
                        finished_current_line = false;
                    }
                }
                if (autoDialog)
                {
                    // Esperar para escrever a proxima linha de dialogo - so escreve quando o tempo passar a partir do momento que terminou de escrever a linha (importante para o efeito de escrita)
                    if (finished_current_line)
                    {
                        if (call)
                        {
                            autoDialogWait = StartCoroutine(DialogCont(dialogLines[currentLine]));
                            call = false;
                        }
                    }
                }

                // Fim do dialogo
                if (Is_Dialog_Finished())
                {
                    textBoxPanel.SetActive(false);
                    whoBoxPanel.SetActive(false);
                    active = false;
                    setMovable(true);
                }
            }
            else
            {
                if (waitindep)
                {
                    finishIndependent();
                    waitindep = false;
                }
            }
        }
    }

    // Para escrever a proxima linha
    private void writeLine()
    {
        if (currentLine <= endAtLine)
        {
            if (writtingEffect)
            {
                // Com efeito de escrita
                if (!finished_current_line)
                {
                    contText.text = fin;
                    st = StartCoroutine(showText(currentLine,""));
                }
            }
            else
                // Sem efeito de escrita
                finishText(false);
        }
    }

    // Funcao que devolve a variavel que diz se a linha atual de dialogo ja foi impresso na totalidade no ecra.
    public bool getFinishedCurrLine()
    {
        return finished_current_line;
    }

    // Funcao que devolve a variavel que diz se a linha atual de dialogo ja acabou de ser imprimido no ecra com o efeito de escrita.
    public bool getFinished()
    {
        return WEfinished;
    }

    // Funcao que devolve a variavel que mantem o estado do continuar automatico, isto é, se está ou nao ativo
    public bool getautoDialog()
    {
        return autoDialog;
    }

    // Funcao que ativa/desativa o efeito de escrita com base no seu estado atual - inverte o seu estado
    public void changeWrittingEffect()
    {
        writefText.text = "";
        if (writtingEffect)
        {
            writtingEffect = false;
            //writefText.text = enabWrit;
        }
        else
        {
            writtingEffect = true;
            //writefText.text = disabWrit;
        }
    }

    // Funcao que ativa/desativa o continuar automatico do dialogo com base no seu estado atual - inverte o seu estado
    public void changeAutoDialog()
    {
        if (autoDialog)
        {
            autoDialog = false;
            PlayerPrefs.SetInt("AutoDialog", autoDialog ? 1 : 0);
            autoText.text = enabAuto;
        }
        else
        {
            autoDialog = true;
            PlayerPrefs.SetInt("AutoDialog", autoDialog ? 1 : 0);
            autoText.text = disabAuto;
        }
        call = autoDialog;
    }

    // Funcao que termina de escrever linha de dialogo atual se chamada qd o efeito de escrita estiver ativo.
    // O parametro stopST indica se o efeito de escrita acabou de ser parado. Quando e falso, ou seja,
    // quando o efeito de escrita foi parado noutra linha de dialogo, deve ser falso. Mas se foi parado na linha atual de dialogo, deve ser true.
    public void finishText(bool stopST)
    {
        if(stopST)
            StopCoroutine(st);
        theText.text = dialogLines[currentLine];
        WEfinished = true;
        finished_current_line = true;
        contText.text = cont;
    }

    // Co-rotina que produz o efeito de escrita, usando um wait para adicionar as letras uma a uma.
    IEnumerator showText(int lineNum,string dialog)
    {
        string currentText;
        WEfinished = false;
        if (lineNum != -1)
        {
            for (int i = 0; i < dialogLines[lineNum].Length; i++)
            {
                currentText = dialogLines[lineNum].Substring(0, i);
                theText.text = currentText;
                if(currentText.EndsWith(".") || currentText.EndsWith("?") || currentText.EndsWith("!"))
                    yield return new WaitForSeconds(typeDelay+0.3f);
                else
                    yield return new WaitForSeconds(typeDelay);
            }
        }
        else
        {
            for (int i = 0; i < dialog.Length + 1; i++)
            {
                currentText = dialog.Substring(0, i);
                theText.text = currentText;
                if (currentText.EndsWith(".") || currentText.EndsWith("?") || currentText.EndsWith("!"))
                    yield return new WaitForSeconds(typeDelay + 0.3f);
                else
                    yield return new WaitForSeconds(typeDelay);
            }
            yield return new WaitForSeconds(waitTime(dialog));
            waitindep = true;
        }
        WEfinished = true;
        finished_current_line = true;
        contText.text = cont;
    }

    // Co-rotina que permite o continuar automatico, usando um wait para esperar antes de avançar para a proxima linha de dialogo.
    IEnumerator DialogCont(string line)
    {
        yield return new WaitForSeconds(waitTime(line));
        currentLine += 2;
        finished_current_line = false;
        call = true;
    }

    // Funcao que ativa/desativa o ecra de historico de dialogo com base no seu estado atual - inverte o seu estado
    public void switchLog()
    {
        if (logPanel.activeSelf)
        {
            logPanel.SetActive(false);
            textBoxPanel.SetActive(true);
            whoBoxPanel.SetActive(true);
        }
        else
        {
            textBoxPanel.SetActive(false);
            whoBoxPanel.SetActive(false);
            logPanel.SetActive(true);
            logTextWrite();
        }
    }

    // Funcao que devolve a variavel que diz se o historico de dialogo esta a ser visto.
    public bool getLogStatus()
    {
        return logPanel.activeSelf;
    }

    // Funcao que preenche o historico de dialogo com o que ja foi dito ate ao momento.
    private void logTextWrite()
    {
        string log = "";
        int i = 0;

        while(i <= currentLine)
        {
            log += dialogLines[i];
            if (i % 2 != 0)
                log += "\n";
            else
                log += ": ";
            i++;
        }
        
        logText.text = log;
    }

    public bool is_in_independent()
    {
        return nonindep;
    }

    // Funcao que permite dialogos independentes
    public void independentDialog(string who, string dialog)
    {
        if (!active)
        {
            nonindep = true;
            active = true;
            waitindep = false;
            textBoxPanel.SetActive(true);
            whoBoxPanel.SetActive(true);
            contPanel.SetActive(false);
            whoText.text = who;
            StartCoroutine(showText(-1, dialog));
        }
    }

    // Funcao que desliga dialogo independente.
    public void finishIndependent()
    {
        contPanel.SetActive(true);
        textBoxPanel.SetActive(false);
        whoBoxPanel.SetActive(false);
        whoText.text = null;
        theText.text = null;
        active = false;
        nonindep = false;
    }

    // Funcao que calcula tempo de espera com base no tamanho da string
    private float waitTime(string dialog)
    {
        if(writtingEffect)
            return (dialog.Length * autoTime)/2;
        else
            return dialog.Length * autoTime;
    }

    // Reinicio do sistema para novo guiao. Enquanto que o Start usa o guiao dado no unity, este usa um guiao dado por codigo
    // Uso:
    // if(!Canvas.GetComponent<DialogSystem>().is_active() && condicao de ativacao){
    //      ReStart(novoguiao,bool para se queremos que haja continuar automatico);
    //      Canvas.GetComponent<DialogSystem>().ActivateDialog();
    //  }
    public void ReStart(TextAsset newGuiao,bool auto)
    {
        if (!active)
        {
            finished_current_line = false;
            autoDialog = auto;
            PlayerPrefs.SetInt("AutoDialog", auto ? 1 : 0);
            autoText.text = enabAuto;
            contPanel.SetActive(true);
            writtingEffect = true;
            writefText.text = "";
           

            call = autoDialog;

            // Carregar linhas de dialogo
            if (newGuiao != null)
                dialogLines = newGuiao.text.Split('\n');

            endAtLine = dialogLines.Length - 1;

            // Linha inicial. Começa do 1 pois o indice 0 contem quem disse, enquanto que o indice 1 tem o que disse. Aumenta sempre de 2 em 2.
            currentLine = 1;
        }
    }

    //Funcao que ativa dialogo
    public void ActivateDialog(bool mov)
    {
        movable = mov;
        active = true;
    }

    // Funcao que determina se o dialogo ja terminou.
    public bool Is_Dialog_Finished()
    {
        return (currentLine > endAtLine ? true : false); 
    }

    public void setMovable(bool mov)
    {
        movable = mov;
    }

    public bool getMovable()
    {
        return movable;
    }

    public bool is_active()
    {
        return active;
    }

    public string GetCurrentLine()
    {
        
        return dialogLines[currentLine];
    }
}