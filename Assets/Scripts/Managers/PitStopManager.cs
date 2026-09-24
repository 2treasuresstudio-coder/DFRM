using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PitStopManager : MonoBehaviour
{
    public static PitStopManager Instance;

    [Header("Popup")]
    public GameObject pitStopPopup;

    [Header("Timer")]
    public float pitStopDuration = 30f;

    [Header("UI")]
    public TextMeshProUGUI countdownText;

    public Image timerFillBar;

    private float remainingTime;

    private bool pitStopActive;

    private WaypointMover activeCar;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (pitStopPopup != null)
        {
            pitStopPopup.SetActive(false);
        }
    }

    private void Update()
    {
        if (!pitStopActive)
            return;

        remainingTime -= Time.deltaTime;

        if (countdownText != null)
        {
            countdownText.text =
                Mathf.CeilToInt(
                    remainingTime).ToString();
        }

        if (timerFillBar != null)
        {
            timerFillBar.fillAmount =
                remainingTime /
                pitStopDuration;
        }

        if (remainingTime <= 0f)
        {
            CompletePitStop();
        }
    }

    public bool IsPitStopActive()
    {
        return pitStopActive;
    }

    public void StartPitStop(
        WaypointMover car)
    {
        if (pitStopActive)
            return;

        pitStopActive = true;

        activeCar = car;

        remainingTime = pitStopDuration;

        if (countdownText != null)
        {
            countdownText.text =
                Mathf.CeilToInt(
                    remainingTime).ToString();
        }

        if (timerFillBar != null)
        {
            timerFillBar.fillAmount = 1f;
        }

        if (pitStopPopup != null)
        {
            pitStopPopup.SetActive(true);
        }

        if (activeCar != null)
        {
            activeCar.enabled = false;
        }

        Debug.Log(
            "Pit Stop Started");
    }

    private void CompletePitStop()
    {
        pitStopActive = false;

        if (RaceManager.Instance != null)
        {
            RaceManager.Instance.RestoreVehicle();
        }

        if (activeCar != null)
        {
            activeCar.enabled = true;
        }

        if (pitStopPopup != null)
        {
            pitStopPopup.SetActive(false);
        }

        Debug.Log(
            "Pit Stop Complete");
    }

    private void ClosePopup()
    {
        pitStopActive = false;

        if (pitStopPopup != null)
        {
            pitStopPopup.SetActive(false);
        }
    }
}