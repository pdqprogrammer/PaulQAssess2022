using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class TicketSystemHandler : MonoBehaviour
{
    [SerializeField]
    private TextAsset m_theaterDataText;
    [SerializeField]
    private TextAsset m_userDataText;
    [SerializeField]
    private TextAsset m_bookingDataText;
    [SerializeField]
    private string m_movieDBUrl = "https://api.themoviedb.org/3/movie/now_playing?api_key=ad40aee7720357734e45d2f700180538&language=en-US&page=1";

    private TheaterData m_theaterData;
    private List<TheaterMovieData> m_theaterMovieData;
    private BookingData m_bookingData;
    private UserData m_userData;
    private User m_currentUser;

    string m_bookingDataPath = Path.Combine(Application.streamingAssetsPath, "bookings.json");
    string m_userDataPath = Path.Combine(Application.streamingAssetsPath, "users.json");

    public TheaterData TheaterData => m_theaterData;
    public List<TheaterMovieData> TheaterMovieData => m_theaterMovieData;
    public User CurrentUser => m_currentUser;

    private void Awake()
    {
        m_theaterMovieData = new List<TheaterMovieData>();

        m_theaterData = JsonUtility.FromJson<TheaterData>(m_theaterDataText.text);

        if (File.Exists(m_bookingDataPath))
        {
            string fileContents;

            if (Application.platform == RuntimePlatform.Android)
            {
                UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(m_bookingDataPath);
                www.SendWebRequest();

                while (!www.isDone) { }//TODO replace this

                fileContents = www.downloadHandler.text;
            }
            else
            {
                fileContents = File.ReadAllText(m_bookingDataPath);
            }

            m_bookingData = JsonUtility.FromJson<BookingData>(fileContents);
        }

        StartCoroutine(SetMovieData());
    }

    private IEnumerator SetMovieData()
    {
        UnityWebRequest webConnection = UnityWebRequest.Get(m_movieDBUrl);

        yield return webConnection.SendWebRequest();

        if (webConnection.isDone)
        {
            string jsonText = webConnection.downloadHandler.text;
            var movieShowingData = JsonUtility.FromJson<MovieShowingData>(jsonText);
            StartCoroutine(GetMovieDetails(movieShowingData.results));
        }
    }

    private IEnumerator GetMovieDetails(MovieResults[] movieResults)
    {
        string urlFront = "https://api.themoviedb.org/3/movie/";
        string urlBack = "?api_key=ad40aee7720357734e45d2f700180538&language=en-US";

        int moviesToRun = (m_theaterData.halls.Length < movieResults.Length) ? m_theaterData.halls.Length : movieResults.Length;

        for (int i=0; i < moviesToRun; i++)
        {
            string movieUrl = urlFront + movieResults[i].id + urlBack;
            UnityWebRequest webConnection = UnityWebRequest.Get(movieUrl);

            yield return webConnection.SendWebRequest();

            if (webConnection.isDone)
            {
                string jsonText = webConnection.downloadHandler.text;
                MovieDetails movieDetails = JsonUtility.FromJson<MovieDetails>(jsonText);

                m_theaterMovieData.Add(new TheaterMovieData(m_theaterData.halls[i], movieDetails, m_theaterData.openTime));
            }

            Debug.Log("Total Movies Playing: " + m_theaterMovieData.Count);
        }
    }

    /// <summary>
    /// method for handling the setting of current user based on name input and users that have logged in
    /// </summary>
    /// <param name="userName"></param>
    public void Login(string userName)
    {
        //NOTE: this data should not be stored in a full application because of security issues
        if (m_userData == null)
        {
            if (File.Exists(m_userDataPath))
            {
                string fileContents;

                Debug.Log("Starting data check");

                if (Application.platform == RuntimePlatform.Android)
                {
                    UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(m_userDataPath);
                    www.SendWebRequest();

                    while (!www.isDone) { }//TODO replace this

                    fileContents = www.downloadHandler.text;
                }
                else
                {
                    fileContents = fileContents = File.ReadAllText(m_userDataPath);
                }

                m_userData = JsonUtility.FromJson<UserData>(fileContents);
            }
        }

        for(int i=0; i< m_userData.users.Count; i++)
        {
            if (userName.Equals(m_userData.users[i].name))
            {
                m_currentUser = m_userData.users[i];
            }
        }

        //check if user was not set
        if(m_currentUser == null)
        {
            m_currentUser = new User();
            m_currentUser.name = userName;
            m_currentUser.id = m_userData.users.Count + 1;
            m_currentUser.reservations = new List<Reservation>();
        }
    }

    /// <summary>
    /// Logout User
    /// </summary>
    public void Logout()
    {
        m_userData = null;
        m_currentUser = null;
    }

    /// <summary>
    /// Set ticket data based on user selections and save to json files
    /// </summary>
    /// <param name="reservation"></param>
    /// <param name="booking"></param>
    public void SetTicketData(Reservation reservation, Booking booking)
    {
        m_currentUser.reservations.Add(reservation);
        bool userFound = false;

        for(int i=0; i<m_userData.users.Count; i++)
        {
            if(m_userData.users[i].id == m_currentUser.id)
            {
                m_userData.users[i] = m_currentUser;
                userFound = true;
                break;
            }
        }

        if (!userFound)
        {
            m_userData.users.Add(m_currentUser);
        }

        bool bookingFound = false;

        for (int i = 0; i < m_bookingData.bookings.Count; i++)
        {
            if (m_bookingData.bookings[i].id.Equals(booking.id))
            {
                m_bookingData.bookings[i] = booking;
                bookingFound = true;
                break;
            }
        }

        if (!bookingFound)
        {
            m_bookingData.bookings.Add(booking);
        }

        //write data back to jsons
        string userJson = JsonUtility.ToJson(m_userData, true);
        string bookingsJson = JsonUtility.ToJson(m_bookingData, true);

        File.WriteAllText(m_userDataPath, userJson);
        File.WriteAllText(m_bookingDataPath, bookingsJson);
    }

    /// <summary>
    /// get the list of booked seats if there is a matching id otherwise get an empty list
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<Seating> GetBookedSeats(string id)
    {
        List<Seating> bookedSeats = new List<Seating>();

        for (int i = 0; i < m_bookingData.bookings.Count; i++)
        {
            if (m_bookingData.bookings[i].id.Equals(id))
            {
                bookedSeats = m_bookingData.bookings[i].reservedseating;
                break;
            }
        }

        return bookedSeats;
    }
}
