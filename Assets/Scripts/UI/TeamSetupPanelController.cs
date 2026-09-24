using TMPro;
using UnityEngine;

public class TeamSetupPanelController : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panelRoot;

    [Header("Input")]
    public TMP_InputField companyNameInput;

    private void Start()
    {
        if (TeamManager.Instance != null &&
            !string.IsNullOrWhiteSpace(
                TeamManager.Instance.companyName) &&
            TeamManager.Instance.companyName !=
            "New Racing Team")
        {
            panelRoot.SetActive(false);
        }
        else
        {
            panelRoot.SetActive(true);
        }
    }

    public void CreateTeam()
    {
        if (TeamManager.Instance == null)
            return;

        TeamManager.Instance.SetCompanyName(
            companyNameInput.text);

        panelRoot.SetActive(false);
    }
}