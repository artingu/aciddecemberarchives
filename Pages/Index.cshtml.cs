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


public class IndexModel : PageModel
{
    private readonly FirestoreDb _db;
    private readonly ILogger<IndexModel> _logger;

    public List<Song>? Songs { get; set; }

    // Song of the day
    public Song? SongOfTheDay { get; set; }

    public IndexModel(ILogger<IndexModel> logger, FirestoreDb db)
    {
        _logger = logger;
        _db = db;
    }
    public string InitialTracksJson { get; set; }


    public async Task OnGetAsync()
    {
        CollectionReference acidDecemberRef = _db.Collection("aciddecember");
        // get all posts from the database id ascending
        Query q = acidDecemberRef.OrderBy("id");

        QuerySnapshot snapshot = await q.GetSnapshotAsync();
        /*      Populate the variable Songs with the data from Firestore. */
        Songs = new List<Song>();

        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            Dictionary<string, object> documentDictionary = document.ToDictionary();


            /* Check if Publishdate is 2024, if it is, skip the song. */
            if (DateTime.Parse(documentDictionary["publishdate"].ToString().Substring(10)) > DateTime.Parse("2024-01-01"))
            {
                /*  Songs.Add(new Song
                 {
                     Title = "?",
                     Artist = "?",
                     Publishdate = documentDictionary["publishdate"].ToString(),
                     ImageLink = "_b4418ac7-6827-4180-844a-e33c1e28308f.jpeg",
                     Artistlink = ""

                 }); */
            }
            else
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
            // Populate winamp playlist
            var tracks = Songs.Select(song => new
            {
                metaData = new
                {
                    artist = song.Artist,
                    title = song.Title,
                    album = "Acid December 2023"
                },
                url = $"https://storage.googleapis.com/acid-december2012/2023/{song.Tune}",
                duration = 5.322286 // Replace with actual duration if available
            });

            InitialTracksJson = JsonConvert.SerializeObject(tracks);


        }
    }
}

