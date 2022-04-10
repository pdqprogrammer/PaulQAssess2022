using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TheaterMovieData
{
    private Halls m_hallData;
    private MovieDetails m_movieDetailData;

    private DateTime m_endDate;
    private DateTime m_openingTime;
    private List<string> m_showTimes;

    private const string TIMEFORMAT = "h:mm tt";

    public Halls HallData => m_hallData;
    public MovieDetails MovieDetailData => m_movieDetailData;
    public List<string> ShowTimes => m_showTimes;

    public TheaterMovieData(Halls hall, MovieDetails movieDetail, string openingTime)
    {
        m_hallData = hall;
        m_movieDetailData = movieDetail;
        m_showTimes = new List<string>();

        //set time date for movie
        DateTime startTime;

        bool isValidDate = DateTime.TryParse(m_movieDetailData.release_date, out startTime);

        if (!isValidDate)
        {
            startTime = DateTime.Now;
        }

        m_endDate = startTime.AddDays(45);

        isValidDate = DateTime.TryParse(openingTime, out m_openingTime);

        if (!isValidDate)
        {
            m_openingTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, 11, 0, 0);
        }

        CalculateShowTimes();
    }

    private void CalculateShowTimes()
    {
        int runtimeAdjustment = m_movieDetailData.runtime + 30;//Runtime with padding for cleaning after shows
        int timeMultiplier = 0;

        DateTime adjustedTime;
        while (true)
        {
            adjustedTime = m_openingTime.AddMinutes(runtimeAdjustment * timeMultiplier);

            if ((adjustedTime.Hour < 24) && (adjustedTime.Hour >= m_openingTime.Hour))
            {
                m_showTimes.Add(adjustedTime.ToString(TIMEFORMAT));
                timeMultiplier++;
            }
            else { break; }
        }   
    }

    /// <summary>
    /// check if movie is still running
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public bool IsShowing(string date)
    {
        DateTime checkDate;
        bool isValidDate = DateTime.TryParse(date, out checkDate);

        if (isValidDate)
        {
            if(checkDate.Date < m_endDate.Date)
            {
                return true;
            }
        }

        return false;
    }
}
