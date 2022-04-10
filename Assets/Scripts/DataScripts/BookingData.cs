using System;
using System.Collections.Generic;

[Serializable]
public class BookingData
{
    public List<Booking> bookings;
}

[Serializable]
public class Booking
{
    public string id;
    public List<Seating> reservedseating;
}