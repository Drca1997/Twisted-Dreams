using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use
        public GameObject Canvas;
        private bool showing_log = false;
        public GameObject LogCanvas;
        private float timeBeforePause = 1.0f;
        public GameObject pauseMenu;
        public bool is_paused = false;
        private bool DSrunning = false;

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }

        private void Update()
        {
            // Skip writing effect on dialog
            if (Input.GetKeyDown(KeyCode.Q) && Time.timeScale > 0.0f && !Canvas.GetComponent<DialogSystem>().is_in_independent() && !Canvas.GetComponent<DialogSystem>().getLogStatus() && !Canvas.GetComponent<DialogSystem>().getFinished())
            {
                Canvas.GetComponent<DialogSystem>().finishText(true);
                Canvas.GetComponent<DialogSystem>().skipped_line();
            }
            // Enable auto continue on dialog
            if (Input.GetKeyDown(KeyCode.X) && Time.timeScale > 0.0f && !Canvas.GetComponent<DialogSystem>().getLogStatus())
                Canvas.GetComponent<DialogSystem>().changeAutoDialog();

            // Enable/Disable Log
            if (Input.GetKeyDown(KeyCode.L))
            {
                if (!showing_log)
                {
                    if (Canvas.GetComponent<DialogSystem>().is_active())
                    {
                        DSrunning = true;
                        Canvas.transform.Find("DialogTextPanel").gameObject.SetActive(false);
                        Canvas.transform.Find("WhoTalkinText").gameObject.SetActive(false);
                    }
                    timeBeforePause = Time.timeScale;
                    Time.timeScale = 0.0f;
                    is_paused = true;
                    LogCanvas.transform.Find("Image").gameObject.SetActive(true);
                    LogCanvas.transform.Find("ScrollArea").gameObject.SetActive(true);
                    LogCanvas.transform.Find("LogTurnText").GetComponent<UnityEngine.UI.Text>().text = "  Esc/L - Go back";
                    LogCanvas.GetComponent<logSystem>().update_log();
                    showing_log = true;
                }
                else
                {
                    if (DSrunning)
                    {
                        DSrunning = false;
                        Canvas.transform.Find("DialogTextPanel").gameObject.SetActive(true);
                        Canvas.transform.Find("WhoTalkinText").gameObject.SetActive(true);
                    }
                    is_paused = false;
                    Time.timeScale = timeBeforePause;
                    LogCanvas.transform.Find("Image").gameObject.SetActive(false);
                    LogCanvas.transform.Find("ScrollArea").gameObject.SetActive(false);
                    LogCanvas.transform.Find("LogTurnText").GetComponent<UnityEngine.UI.Text>().text = "  L - Check Log";
                    showing_log = false;
                }
                //Canvas.GetComponent<DialogSystem>().switchLog();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!showing_log)
                {
                    if (Time.timeScale > 0.0f) // pause
                    {
                        timeBeforePause = Time.timeScale;
                        Time.timeScale = 0.0f;
                        is_paused = true;
                        pauseMenu.SetActive(true);
                    }
                    else // unpause
                    {
                        is_paused = false;
                        Time.timeScale = timeBeforePause;
                        pauseMenu.SetActive(false);
                    }
                }
                else
                {
                    if (DSrunning)
                    {
                        DSrunning = false;
                        Canvas.transform.Find("DialogTextPanel").gameObject.SetActive(true);
                        Canvas.transform.Find("WhoTalkinText").gameObject.SetActive(true);
                    }
                    is_paused = false;
                    Time.timeScale = timeBeforePause;
                    LogCanvas.transform.Find("Image").gameObject.SetActive(false);
                    LogCanvas.transform.Find("ScrollArea").gameObject.SetActive(false);
                    showing_log = false;
                }
            }
        }

        private void FixedUpdate()
        {
            // pass the input to the car!
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
#if !MOBILE_INPUT
            float handbrake = CrossPlatformInputManager.GetAxis("Jump");
            m_Car.Move(h, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }
    }
}
