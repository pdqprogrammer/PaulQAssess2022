using TMPro;
using UnityEngine;

public class ReservationDataHandler : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_movieTitle;
    [SerializeField]
    private TMP_Text m_date;
    [SerializeField]
    private TMP_Text m_time;
    [SerializeField]
    private TMP_Text m_seating;

    /// <summary>
    /// create clone with record data for reservation
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="reservation"></param>
    /// <returns></returns>
    public ReservationDataHandler CreateReservationData(RectTransform parent, Reservation reservation)
    {
        ReservationDataHandler clone = Instantiate(this, parent);

        clone.m_movieTitle.SetText(reservation.movie);
        clone.m_date.SetText(reservation.date);
        clone.m_time.SetText(reservation.time);

        string seating = "";

        for(int i=0; i<reservation.seating.Count; i++)
        {
            if(i > 0)
            {
                seating += ",";
            }

            seating += reservation.seating[i].row + "-" + reservation.seating[i].seat;
        }

        clone.m_seating.SetText(seating);

        return clone;
    }
}
