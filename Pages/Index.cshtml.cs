using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Google.Cloud.Firestore;
using AcidDec.Models;


namespace aciddecemberarchives.Pages;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

public class IndexModel : PageModel
{
    private readonly FirestoreDb _db;
    private readonly ILogger<IndexModel> _logger;

    public List<Song>? Songs { get; set; }

    public bool IsRootPage { get; set; }

    // Song of the day
    public Song? SongOfTheDay { get; set; }

    public IndexModel(ILogger<IndexModel> logger, FirestoreDb db)
    {
        InitialTracksJson = string.Empty;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public string InitialTracksJson { get; set; }

    // Get the year from the URL


    public async Task OnGetAsync(string year)
    {
        System.DateTime filterdatestart;
        System.DateTime filterdateend;
        System.DateTime publishdatetime = System.DateTime.Now;
        IsRootPage = string.IsNullOrEmpty(year);
        String urlyear = year ?? "2024";
        if (urlyear == "2024")
        {
            // this year
            year = "2024";

            // set to todays date
            filterdatestart = System.DateTime.Now;
            // today plus 6 months
            filterdateend = filterdatestart.AddMonths(6);
        }
        else
        {
            // every other year
            // a range of dates from the 30th of November to the 30th of January the following year

            filterdatestart = System.DateTime.Parse(urlyear + "-11-30"); // 30th of November 2023
            filterdateend = System.DateTime.Parse("2024-01-30"); // 30th of January 2024

        }
        ViewData["Year"] = urlyear;
        CollectionReference acidDecemberRef = _db.Collection("aciddecember");
        // get all posts from the database id ascending
        Query q = acidDecemberRef.OrderBy("id");

        QuerySnapshot snapshot = await q.GetSnapshotAsync();
        /*      Populate the variable Songs with the data from Firestore. */
        Songs = new List<Song>();

        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            Dictionary<string, object> documentDictionary = document.ToDictionary();
            if (documentDictionary.TryGetValue("publishdate", out object publishDateObj) && publishDateObj is Timestamp timestamp)
            {
                publishdatetime = timestamp.ToDateTime();
            }


            // add songs have a publishhdate between filterdatestart and filterdateend
            if (publishdatetime > filterdatestart && publishdatetime < filterdateend)
            //  if (false)
            {
                Song s = new Song
                {
                    Title = documentDictionary["title"].ToString(),
                    Artist = documentDictionary["artist"].ToString(),
                    Publishdate = documentDictionary["publishdate"].ToString(),
                    ImageLink = documentDictionary["imglink"].ToString(),
                    Artistlink = documentDictionary["artistlink"].ToString(),
                    Id = documentDictionary["id"].ToString(),
                    Tune = documentDictionary["tune"].ToString(),
                };

                Songs.Add(s);
                SongOfTheDay = s;


            }

        }


        // to be continued...  i want every track to be added to the playlist..
        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            // Populate winamp playlist
            var tracks = Songs.Select(song => new
            {
                metaData = new
                {
                    artist = song.Artist,
                    title = song.Title,
                    album = "Acid December " + year
                },
                url = $"https://storage.googleapis.com/acid-december2012/2023/{song.Tune}",
                duration = 5.322286 // Replace with actual duration if available
            });

            // randomize the order of the tracks, except for the first one
            var random = new Random();
            tracks = tracks.OrderBy(x => random.Next()).ToList();


            ViewData["InitialTracks"] = JsonConvert.SerializeObject(tracks);
        }
    }
}



