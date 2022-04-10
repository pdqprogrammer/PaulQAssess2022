using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField]
    private TicketSystemHandler m_ticketSystemHandler;

    [SerializeField]
    private BuyTicketHandler m_buyTicketMenu;
    [SerializeField]
    private UserProfileHandler m_userProfile;
    [SerializeField]
    private GameObject m_userLogin;

    [SerializeField]
    private Button m_buyTicketsButton;
    [SerializeField]
    private Button m_seeReservationsButton;
    [SerializeField]
    private Button m_logoutButton;
    [SerializeField]
    private TMP_Text m_messageText;

    private void Awake()
    {
        m_buyTicketsButton.onClick.AddListener(OpenBuyTickets);
        m_seeReservationsButton.onClick.AddListener(OpenUserProfile);
        m_logoutButton.onClick.AddListener(Logout);
    }

    private void OpenBuyTickets()
    {
        gameObject.SetActive(false);
        m_buyTicketMenu.LoadTicketBuying();
    }

    private void OpenUserProfile()
    {
        
        gameObject.SetActive(false);
        m_userProfile.OpenUserProfile();
    }

    private void Logout()
    {
        m_ticketSystemHandler.Logout();
        gameObject.SetActive(false);
        m_userLogin.SetActive(true);
    }

    /// <summary>
    /// Open Main Menu and set message
    /// </summary>
    /// <param name="message"></param>
    public void OpenMenu(string message)
    {
        m_messageText.SetText(message);
        gameObject.SetActive(true);
    }
}
