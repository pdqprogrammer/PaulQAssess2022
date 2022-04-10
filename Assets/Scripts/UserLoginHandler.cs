using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserLoginHandler : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField m_userNameInput;
    [SerializeField]
    private Button m_loginButton;
    [SerializeField]
    private ErrorWarningHandler m_errorWarningHandler;
    [SerializeField]
    private TicketSystemHandler m_ticketSystemHandler;
    [SerializeField]
    private MainMenuHandler m_mainMenu;

    private void Awake()
    {
        m_loginButton.onClick.AddListener(Login);
    }

    private void Login()
    {
        string userInputText = m_userNameInput.text;

        if (!string.IsNullOrEmpty(userInputText))
        {
            m_ticketSystemHandler.Login(m_userNameInput.text);
            m_userNameInput.SetTextWithoutNotify("");
            gameObject.SetActive(false);
            m_mainMenu.OpenMenu("Welcome!");
        }
        else
        {
            m_errorWarningHandler.OpenPopup("Login Error", "Please input a user name to continue.");
        }
    }
}
