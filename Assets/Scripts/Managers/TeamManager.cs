using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public static TeamManager Instance;

    [Header("Team")]
    public string companyName = "New Racing Team";

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

    public void SetCompanyName(
        string newName)
    {
        if (string.IsNullOrWhiteSpace(
            newName))
        {
            return;
        }

        companyName =
            newName.Trim();

        Debug.Log(
            $"Team Created: {companyName}");
    }

    public string GetCompanyName()
    {
        return companyName;
    }
}