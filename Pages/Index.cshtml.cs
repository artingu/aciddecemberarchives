namespace aciddecemberarchives.Pages;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AcidDec.Models;
using Google.Type;
using System.Text.RegularExpressions;

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
        Year = string.Empty;
        InitialTracksJson = string.Empty;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _db = db ?? throw new ArgumentNullException(nameof(db));
        /*  Year = "2024"; */
    }

    public string InitialTracksJson { get; set; }
    public string Year { get; set; }
    // Get the year from the URL
    
    /// <summary>
    /// Determines the "Acid December" year for a given date.
    /// An Acid December year starts in November. For example, January 2025 belongs to the 2024 season.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns>The corresponding Acid December year.</returns>
    private int GetAcidDecemberYear(System.DateTime date)
    {
        return date.Month < 11 ? date.Year - 1 : date.Year;
    }


    public async Task OnGetAsync(int? urlyear)
    {
        System.DateTime filterdatestart;
        System.DateTime filterdateend;
        System.DateTime? publishdatetime = null;

        IsRootPage = !urlyear.HasValue;
        // If no year is specified in the URL, we want to show the current "Acid December" season.
        // For example, in January 2025, the current season is still 2024.
        var yearToQuery = urlyear ?? GetAcidDecemberYear(System.DateTime.UtcNow);

        Year = yearToQuery.ToString();

        if (IsRootPage) // For the "current" year, which is the root page
        {
            // The season starts in November.
            filterdatestart = new System.DateTime(yearToQuery, 11, 1);
            // And it can go into the next year.
            filterdateend = filterdatestart.AddMonths(3);
        }
        else
        {
            // every other year
            // a range of dates from the 30th of November to the 30th of January the following year
            filterdatestart = new System.DateTime(yearToQuery, 11, 1);
            filterdateend = new System.DateTime(yearToQuery + 1, 2, 1);
        }

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
            if (publishdatetime.HasValue && publishdatetime.Value >= filterdatestart && publishdatetime.Value < filterdateend)
            {

                Song s = new Song
                {
                    Title = documentDictionary["title"].ToString(),
                    Artist = documentDictionary["artist"].ToString(),
                    Publishdate = documentDictionary["publishdate"] as Timestamp?,
                    ImageLink = documentDictionary.ContainsKey("imglink") && documentDictionary["imglink"] != null ? documentDictionary["imglink"].ToString() : "Designer.png",
                    Id = documentDictionary["id"] as long?,
                    Tune = documentDictionary["tune"].ToString(),
                };

                Songs.Add(s);
                if (IsRootPage)
                {
                    SongOfTheDay = s;
                }

            }

        }

        if (Songs.Count == 0)
        {
            // no songs found
            // redirect to the 404 page
            Response.Redirect("/404");
            return;
        }

        // Populate winamp playlist
        var tracks = Songs.Select(song => new
        {
            metaData = new
            {
                artist = song.Artist,
                title = song.Title,
                album = "Acid December " + Year
            },
            url = $"https://storage.googleapis.com/acid-december2012/{Year}/{song.Tune}",
            duration = 5.322286 // This is a placeholder, it gets overwritten by the player.
        });

        // For past years, randomize the playlist. For the current year, show newest first.
        if (!IsRootPage)
        {
            var random = new Random();
            tracks = tracks.OrderBy(x => random.Next());
        }

        ViewData["InitialTracks"] = JsonConvert.SerializeObject(tracks.Reverse());
        ViewData["Year"] = Year;
    }
}
