using System;

[Serializable]
public class MovieShowingData
{
    public MovieResults[] results;
}

[Serializable]
public class MovieResults
{
    public int id;
}

[Serializable]
public class MovieDetails
{
    public int id;
    public string original_title;
    public string release_date;
    public int runtime;
}