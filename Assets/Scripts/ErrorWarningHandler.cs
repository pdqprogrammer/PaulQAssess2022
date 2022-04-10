using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ErrorWarningHandler : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_warningText;
    [SerializeField]
    private TMP_Text m_errorMessageText;
    [SerializeField]
    private Button m_closeButton;

    private void Awake()
    {
        m_closeButton.onClick.AddListener(ClosePopup);
    }

    private void ClosePopup()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Open warning popup
    /// </summary>
    /// <param name="warning"></param>
    /// <param name="message"></param>
    public void OpenPopup(string warning, string message)
    {
        m_warningText.SetText(warning);
        m_errorMessageText.SetText(message);

        gameObject.SetActive(true);
    }
}
