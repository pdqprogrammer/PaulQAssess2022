using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class ConfigurationFileTest
{
    //example of testing to verify local files are there and functioning properly
    [OneTimeSetUp]
    public void LoadScene()
    {
        SceneManager.LoadScene("TheaterTicketingScene");
    }

    [Test, Order(1)]
    public void BookingJsonTest()
    {
        // Use the Assert class to test conditions
        string m_bookingDataPath = Application.streamingAssetsPath + "/bookings.json";

        Assert.IsTrue(File.Exists(m_bookingDataPath));

        if (File.Exists(m_bookingDataPath))
        {
            string fileContents = File.ReadAllText(m_bookingDataPath);
            var m_bookingData = JsonUtility.FromJson<BookingData>(fileContents);

            Assert.NotNull(m_bookingData);
        }

    }

    [Test, Order(2)]
    public void UserDataJsonTest()
    {
        // Use the Assert class to test conditions
        string m_userDataPath = Application.streamingAssetsPath + "/users.json";

        Assert.IsTrue(File.Exists(m_userDataPath));

        if (File.Exists(m_userDataPath))
        {
            string fileContents = File.ReadAllText(m_userDataPath);
            var userData = JsonUtility.FromJson<UserData>(fileContents);

            Assert.NotNull(userData);
        }

    }

    [UnityTest, Order(3)]
    public IEnumerator CheckForTheaterData()
    {
        yield return GameObject.Find("TicketSystemCanvas");

        var ticketSystemObject = GameObject.Find("TicketSystemCanvas").GetComponent<TicketSystemHandler>();
        var theaterData = ticketSystemObject.TheaterData;

        Assert.NotNull(theaterData);
    }
}
