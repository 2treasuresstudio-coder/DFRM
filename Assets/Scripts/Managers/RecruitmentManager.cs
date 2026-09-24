using UnityEngine;
using UnityEngine.InputSystem;

public class RecruitmentManager : MonoBehaviour
{
    public static RecruitmentManager Instance;

    [Header("References")]
    public RecruitmentPopupController popupController;

    private DriverData currentCandidate;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log("RecruitmentManager Start");

        if (!DriverManager.Instance.HasAnyDriver())
        {
            Debug.Log("No Driver Found");

            GenerateFirstDriverEvent();
        }
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.rKey.wasPressedThisFrame)
        {
            GenerateRecruitmentEvent();
        }
    }

    public void GenerateFirstDriverEvent()
    {
        Debug.Log("GenerateFirstDriverEvent");

        currentCandidate =  
            DriverManager.Instance.GenerateRandomDriver(true);

        popupController.ShowMandatoryCandidate(
            currentCandidate);
    }

    public void GenerateRecruitmentEvent()
    {
        currentCandidate =
            DriverManager.Instance.GenerateRandomDriver(true);

        popupController.ShowMandatoryCandidate(
            currentCandidate);
    }

    public DriverData GetCurrentCandidate()
    {
        return currentCandidate;
    }
}