using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserProfileHandler : MonoBehaviour
{
    [SerializeField]
    private TicketSystemHandler m_ticketSystemHandler;

    [SerializeField]
    private BuyTicketHandler m_buyTicketMenu;
    [SerializeField]
    private GameObject m_userLogin;

    [SerializeField]
    private RectTransform m_reservationContentTransform;
    [SerializeField]
    private ReservationDataHandler m_reservationRecordPrefab;

    [SerializeField]
    private Button m_buyTicketsButton;
    [SerializeField]
    private Button m_logoutButton;
    [SerializeField]
    private TMP_Text m_userNameText;

    private void Awake()
    {
        m_buyTicketsButton.onClick.AddListener(OpenBuyTickets);
        m_logoutButton.onClick.AddListener(Logout);
    }

    private void OpenBuyTickets()
    {
        gameObject.SetActive(false);
        m_buyTicketMenu.LoadTicketBuying();
    }

    private void Logout()
    {
        m_ticketSystemHandler.Logout();
        ClearReservations();
        gameObject.SetActive(false);
        m_userLogin.SetActive(true);
    }

    private void ClearReservations()
    {
        foreach( RectTransform child in m_reservationContentTransform)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// open user profile and create reservation objects showing reservations for current user
    /// </summary>
    public void OpenUserProfile()
    {
        gameObject.SetActive(true);
        m_userNameText.SetText(m_ticketSystemHandler.CurrentUser.name);
        ClearReservations();

        List<Reservation> reservations = m_ticketSystemHandler.CurrentUser.reservations;

        for (int i=0; i<reservations.Count; i++)
        {
            m_reservationRecordPrefab.CreateReservationData(m_reservationContentTransform, reservations[i]);
        }
    }
}
