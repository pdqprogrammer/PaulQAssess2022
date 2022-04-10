using System;
using System.Collections.Generic;

[Serializable]
public class UserData
{
    public List<User> users;
}

[Serializable]
public class User
{
    public string name;
    public int id;
    public List<Reservation> reservations;
}

[Serializable]
public class Reservation
{
    public string movie;
    public string date;
    public string time;
    public List<Seating> seating;
}

[Serializable]
public class Seating : IEquatable<Seating>
{
    public int row;
    public int seat;

    public bool Equals(Seating other)
    {
        if(this.row == other.row && this.seat == other.seat)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}