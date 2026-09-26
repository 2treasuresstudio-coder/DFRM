using TMPro;
using UnityEngine;

public class TeamSetupPanelController : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panelRoot;

    [Header("Input")]
    public TMP_InputField companyNameInput;

    private void Awake()
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(true);
        }
    }

    private void Start()
    {
        if (TeamManager.Instance == null)
        {
            Debug.LogError(
                "TeamManager not found in scene.");

            return;
        }

        Debug.Log(
            $"Company Name: [{TeamManager.Instance.companyName}]");

        bool hasCompanyName =
            !string.IsNullOrWhiteSpace(
                TeamManager.Instance.companyName);

        panelRoot.SetActive(
            !hasCompanyName);
    }

    public void CreateTeam()
    {
        if (TeamManager.Instance == null)
            return;

        if (string.IsNullOrWhiteSpace(
            companyNameInput.text))
        {
            return;
        }

        TeamManager.Instance.SetCompanyName(
            companyNameInput.text);

        panelRoot.SetActive(false);
    }
}