using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TestTools;

public class MovieDBTest
{
    // Example of tests showing some testing for external api usage
    [UnityTest, Order(0)]
    public IEnumerator MovieDBCurrentlyPlayingTest()
    {
        string m_movieDBUrl = "https://api.themoviedb.org/3/movie/now_playing?api_key=ad40aee7720357734e45d2f700180538&language=en-US&page=1";

        UnityWebRequest webConnection = UnityWebRequest.Get(m_movieDBUrl);

        yield return webConnection.SendWebRequest();

        Assert.IsTrue(webConnection.isDone);

        if (webConnection.isDone)
        {
            string jsonText = webConnection.downloadHandler.text;
            var movieShowingData = JsonUtility.FromJson<MovieShowingData>(jsonText);

            Assert.NotNull(movieShowingData);
        }
    }

    [UnityTest, Order(1)]
    public IEnumerator MovieDBMovieDetailsTest()
    {
        string m_movieDBUrl = "https://api.themoviedb.org/3/movie/675353?api_key=ad40aee7720357734e45d2f700180538&language=en-US";

        UnityWebRequest webConnection = UnityWebRequest.Get(m_movieDBUrl);

        yield return webConnection.SendWebRequest();

        Assert.IsTrue(webConnection.isDone);

        if (webConnection.isDone)
        {
            string jsonText = webConnection.downloadHandler.text;
            MovieDetails movieDetails = JsonUtility.FromJson<MovieDetails>(jsonText);

            Assert.NotNull(movieDetails);
        }
    }
}
