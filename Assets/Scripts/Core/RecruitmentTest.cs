using UnityEngine;
using UnityEngine.InputSystem;

public class RecruitmentTest : MonoBehaviour
{
    public RecruitmentPopupController popup;

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.rKey.wasPressedThisFrame)
        {
            popup.ShowCandidate(
                DriverManager.Instance.GenerateRandomDriver());
        }
    }
}