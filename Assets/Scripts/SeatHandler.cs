using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeatHandler : MonoBehaviour
{
    [SerializeField]
    private Button m_seatButton;
    [SerializeField]
    private TMP_Text m_seatLabel;

    private Seating m_seat;
    private BuyTicketHandler m_ticketHandler;

    /// <summary>
    /// create seat button using data passed and attach ticket handler to update seating
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="seat"></param>
    /// <param name="ticketHandler"></param>
    /// <param name="active"></param>
    /// <returns></returns>
    public SeatHandler CreateSeat(RectTransform parent, Seating seat, BuyTicketHandler ticketHandler, bool active = true)
    {
        SeatHandler clone = Instantiate(this, parent);

        clone.m_seat = seat;
        clone.m_ticketHandler = ticketHandler;
        clone.m_seatButton.onClick.AddListener(clone.SetSeating);

        if (!active)
        {
            clone.m_seatLabel.SetText("X");
            clone.m_seatButton.interactable = false;
        }

        return clone;
    }

    private void SetSeating()
    {
        if (!m_seatButton.IsInteractable())
        {
            return;
        }

        if (m_ticketHandler.AddRemoveSeating(m_seat))
        {
            m_seatLabel.SetText("O");
        }
        else
        {
            m_seatLabel.SetText("");
        }
    }
}
