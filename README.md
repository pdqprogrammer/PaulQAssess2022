# PaulQuinonesUnityAssess2022
 
TARGET DEVICES - Windows and Android

Run Instructions:
Windows - This application is set to run using the Streaming Assets files and update those directly. Standard build settings with a resolution of 1920x1080 have been set for a Windowed application.
Android - This application is built to run in a landscape view.



/////Update resolution and scaling settings /////test android changes /////renamae github make public





Sample existing user - "John Riggs"
This test user features reservations as an example of previous movie choices.

Considerations:
JSON vs SQL - for this I am using JSON as it is a format that is better for transferring smaller amounts of data. But if we wanted to deal with larger amounts of data it would make more sense to work with a webservice / API call that queries data from a DB. This way you can store larger amounts of data that would be required for a theater but have access to them in lighter format without directly accessing your DB.

Movie data and theater data isnt intended to be updated by the user so arrays are used there. But, things like bookings and user data has the possibility to be updated by a user so lists are used for there flexibility.

Having one source of data was important because the goal was being able to access the data from the different parts of the application.
This allows for data in the application to be up to date and each section of the application wasn't reliant on data from another one.

Another thing that was important was understanding the basic flow of how the application should work. From a simple login screen to having an error warning to inform users on when something is wrong and what that is.

Application Design:
After looking at the requirements one of the first steps was to look at the data needed and plan on which would be data pulled from files or from APIs.
The other thing taken into consideration was which data is only being accessed and which ones will have to be written back into as well.
Bookings and User Data fall into the group of data that would be edited so for this case those files were placed inside of the StreamAssets for access.
In most cases it would be better to build out a service with an API that can be used to access and update the data.

The next step take was to plan out the different features and states a user would be interacting with.
This included planning the components needed and arranging these inside of the Unity editor after everything is decided on.
From here intial UI components were arranged and placed within the scene.

After each of the states and components were put together then the flow of the application was run through.
This step allowed for a visual look at how the different actions of a user should behave and think out functionality for it.
Once the flow was decided and any changes made to components in the Scene then functionality and code was worked on.

Implementation:
The first step was to build out for the data needed from files and API calls to be used.
This also meant creating sample json files that could be used for testing.

After building out the code for handling the json data, the TicketSystemHandler was built out with the intention of working with various data from sources.
Its goal is to be a source for each of the components and states to access for data used to implement features.
From there features were built out for each state in the following order:
-User Login
-Error Messaging
-Main Menu
-User Profile
-Ticket Buying

Ticket Buying required the most development time because of all the various features required.
Ticket Buying has to deal with loading in show dates and determining if a show is still playing on a certain date.
Also, movie information such as title and showtime needed to be pulled in from the TicketSystemHandler.

Another part of the ticket building that required work was the generation of seating/booking information.
This required having an id in this case to compare with already existing data. Based on that data it would be determined which seats are no longer available
Then when a user "buys" a ticket that information has to be written back to the data sources.

Testing:
Three simple sets of tests were created for testing the different aspects of the applications.
-ConfigurationFileTest is focused on testing the different files being used and making sure they are loading as intended.
-MovieDBTest features tests checking that access to MovieDB is working and that the data is pulled in correctly.
-UserLoginTest is test the functionality of the Login UI and verify that data being accessed is loaded properly for use across different parts of the application.

Future testing built out for this would include more indepth tests of the application flow and various integrations.
Also more testing on failed states to verify error messaging is working for the user.

Future Planning or Features:
While the application gets the minimal done there are some future considerations for improving things from functionality to usability.
-The main menu works more as a splash screen at the moment but a future iteration it would benefit the application to have a menu that pops up with a menu button.
--This would reduce some of the functionality repeating as it does with implementing buttons with similar functions across the application
-Building out locks on seating would be another function to build out to avoid someone from buying the same seat as another person.
-Build out for multiple theaters eventually that would require managing the data differently that in this case.
-Along the lines of recommending seats based on party size, recommending theaters based on user location or home data that could be added to the user profile.
-Adding in support for more resolutions to accomodate for a large variety of devices.