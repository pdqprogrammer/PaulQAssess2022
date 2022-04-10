using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using TMPro;
using UnityEngine.UI;
using System;

public class UserLoginTest
{
    //example test of verifying functionality is working properly for login
    [OneTimeSetUp]
    public void LoadScene()
    {
        SceneManager.LoadScene("TheaterTicketingScene");
    }

    //try to login
    //check current user data is not null

    [UnityTest, Order(1)]
    public IEnumerator LoginTest()
    {
        yield return GameObject.Find("UserNameInput");

        var nameInputobject = GameObject.Find("UserNameInput").GetComponent<TMP_InputField>();
        Assert.NotNull(nameInputobject);
        nameInputobject.SetTextWithoutNotify("John Riggs");

        yield return GameObject.Find("LoginButton");

        var loginButtonObject = GameObject.Find("LoginButton").GetComponent<Button>();
        Assert.NotNull(loginButtonObject);
        loginButtonObject.onClick.Invoke();

        yield return GameObject.Find("MainMenu");

        var mainMenuObject = GameObject.Find("MainMenu");
        Assert.NotNull(mainMenuObject);
        Assert.IsTrue(mainMenuObject.activeSelf);
    }

    [UnityTest, Order(2)]
    public IEnumerator CheckForUserData()
    {
        yield return GameObject.Find("TicketSystemCanvas");

        var ticketSystemObject = GameObject.Find("TicketSystemCanvas").GetComponent<TicketSystemHandler>();
        Assert.NotNull(ticketSystemObject);

        var userData = ticketSystemObject.CurrentUser;

        Assert.NotNull(userData.id);
        Assert.NotNull(userData.name);
        Assert.NotNull(userData.reservations);

        yield return null;
    }
}
