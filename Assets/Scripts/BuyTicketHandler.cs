using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyTicketHandler : MonoBehaviour
{
    [SerializeField]
    private TicketSystemHandler m_ticketSystemHandler;
    [SerializeField]
    private MainMenuHandler m_mainMenu;
    [SerializeField]
    private ErrorWarningHandler m_errorWarningHandler;

    [SerializeField]
    private Button m_buyTicketsButton;

    [SerializeField]
    private TMP_Dropdown m_dateDropdown;
    [SerializeField]
    private TMP_Dropdown m_movieDropdown;
    [SerializeField]
    private TMP_Dropdown m_timeDropdown;

    [SerializeField]
    private SeatHandler m_seatButtonObject;
    [SerializeField]
    private RectTransform m_seatArrangementPanel;
    [SerializeField]
    private GridLayoutGroup m_seatGridLayoutGroup;

    [SerializeField]
    private Button m_seeReservationsButton;
    [SerializeField]
    private Button m_logoutButton;
    [SerializeField]
    private UserProfileHandler m_userProfile;
    [SerializeField]
    private GameObject m_userLogin;

    private const string DATEFORMAT = "M/dd/yyyy";

    private List<string> m_nextAvailableDates;
    private Halls m_currentHall;
    private MovieDetails m_currentMovie;
    private List<Seating> m_selectedSeats;
    private List<Seating> m_reservedSeats;

    private void Awake()
    {
        m_dateDropdown.onValueChanged.AddListener(DateChanged);
        m_movieDropdown.onValueChanged.AddListener(MovieChanged);
        m_timeDropdown.onValueChanged.AddListener(TimeChanged);

        m_seeReservationsButton.onClick.AddListener(OpenUserProfile);
        m_logoutButton.onClick.AddListener(Logout);
        m_buyTicketsButton.onClick.AddListener(SetBuyingTickets);

        m_reservedSeats = new List<Seating>();

        CalculateNextDates();
    }

    //calculate next 7 dates
    private void CalculateNextDates()
    {
        m_nextAvailableDates = new List<string>();
        DateTime currTime = DateTime.Now;
        for(int i=0; i<7; i++)
        {
            currTime = currTime.AddDays(i);
            m_nextAvailableDates.Add(currTime.ToString(DATEFORMAT));
        }
    }

    //method to run when clicking by tickets that will set new reservation and booking data
    private void SetBuyingTickets()
    {
        if(m_selectedSeats.Count <= 0)
        {
            m_errorWarningHandler.OpenPopup("Seat Selection Error", "Please select seats in order to continue with purchase.");
        }
        else
        {
            Reservation newReservation = new Reservation
            {
                movie = m_currentMovie.original_title,
                date = m_dateDropdown.options[m_dateDropdown.value].text,
                time = m_timeDropdown.options[m_timeDropdown.value].text,
                seating = m_selectedSeats
            };

            for(int i=0; i<m_selectedSeats.Count; i++)
            {
                m_reservedSeats.Add(m_selectedSeats[i]);
            }

            Booking newBooking = new Booking
            {
                id = m_currentMovie.id + "-" + newReservation.date + "-" + newReservation.time,
                reservedseating = m_reservedSeats
            };

            //Send data over to be updated
            m_ticketSystemHandler.SetTicketData(newReservation, newBooking);

            gameObject.SetActive(false);
            m_mainMenu.OpenMenu("Purchase Complete!");
        }
    }

    private void SetDropdownOptions()
    {
        m_dateDropdown.ClearOptions();
        m_dateDropdown.AddOptions(m_nextAvailableDates);
        DateChanged(0);
    }

    //////////////DROPDOWN CHANGE METHODS/////////////////////////
    private void DateChanged(int selection)
    {
        m_dateDropdown.value = selection;

        //check movie to see if playing
        var theaterData = m_ticketSystemHandler.TheaterMovieData;
        List<string> showingMovies = new List<string>();
        for (int i = 0; i < theaterData.Count; i++)
        {
            if (theaterData[i].IsShowing(m_dateDropdown.options[m_dateDropdown.value].text))
            {
                showingMovies.Add(theaterData[i].MovieDetailData.original_title);
            }
        }

        m_movieDropdown.ClearOptions();
        m_movieDropdown.AddOptions(showingMovies);
        MovieChanged(0);
    }

    private void MovieChanged(int selection)
    {
        m_movieDropdown.value = selection;

        m_timeDropdown.ClearOptions();
        var theaterData = m_ticketSystemHandler.TheaterMovieData;
        for (int i = 0; i < theaterData.Count; i++)
        {
            string movieTitle = m_movieDropdown.options[m_movieDropdown.value].text;
            if (theaterData[i].MovieDetailData.original_title.Equals(movieTitle))
            {
                m_currentHall = theaterData[i].HallData;
                m_currentMovie = theaterData[i].MovieDetailData;
                m_timeDropdown.AddOptions(theaterData[i].ShowTimes);
                TimeChanged(0);
                break;
            }
        }
    }

    private void TimeChanged(int selection)
    {
        m_timeDropdown.value = selection;
        GetReservedSeating();
        SetSeatingArrangement();//refresh panel after dropdown change
    }

    //Get reserved seats based on current dropdown and movie settings
    private void GetReservedSeating()
    {
        
        string date = m_dateDropdown.options[m_dateDropdown.value].text;
        string time = m_timeDropdown.options[m_timeDropdown.value].text;

        string id = m_currentMovie.id + "-" + date + "-" + time;

        m_reservedSeats = m_ticketSystemHandler.GetBookedSeats(id);
    }

    private void SetSeatingArrangement()
    {
        float width = m_seatArrangementPanel.rect.width/m_currentHall.seats;
        float height = m_seatArrangementPanel.rect.height / m_currentHall.rows;

        foreach (RectTransform child in m_seatArrangementPanel)
        {
            Destroy(child.gameObject);
        }

        m_seatGridLayoutGroup.cellSize = new Vector2(width, height);

        for (int i=0; i < m_currentHall.rows; i++)
        {
            for(int j=0; j<m_currentHall.seats; j++)
            {
                //set the seating and button functionality
                Seating seating = new Seating
                {
                    row = i + 1,
                    seat = j + 1
                };

                bool notReserved = true;
                if (m_reservedSeats.Contains(seating))
                {
                    notReserved = false;
                }

                m_seatButtonObject.CreateSeat(m_seatArrangementPanel, seating, this, notReserved);
            }
        }
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
    /// Load ticket buying ui
    /// </summary>
    public void LoadTicketBuying()
    {
        gameObject.SetActive(true);
        m_selectedSeats = new List<Seating>();//This could further on be set to reset only when session expires or tickets bought
        SetDropdownOptions();
    }

    /// <summary>
    /// method to handle adding or removing seating from selected seats list
    /// </summary>
    /// <param name="seating"></param>
    /// <returns></returns>
    public bool AddRemoveSeating(Seating seating)
    {
        if (m_selectedSeats.Contains(seating))
        {
            m_selectedSeats.Remove(seating);
            return false;
        }
        else
        {
            m_selectedSeats.Add(seating);
            return true;
        }
    }
}
