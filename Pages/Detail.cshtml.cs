/* Show a single song from the database. */

using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Google.Cloud.Firestore;

namespace aciddecemberarchives.Pages;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;
using AcidDec.Models;
using Google.Type;
using Newtonsoft.Json;

public class DetailModel : PageModel
{
    private readonly FirestoreDb _db;
    private readonly ILogger<DetailModel> _logger;
    public Song? Song { get; set; }
    public string Year { get; set; }
    public DetailModel(ILogger<DetailModel> logger, FirestoreDb db)
    {
        _logger = logger;
        _db = db;
    }
    public async Task OnGetAsync(string urlyear, int id)
    {
        Year = urlyear;

        CollectionReference acidDecemberRef = _db.Collection("aciddecember");
        // get the id from the url
        if (Request.Query["id"].ToString() == null)
        {
            Song = new Song { };

        }
        else

        {

            int songid = id;
            // get the firestore document with the id
            // create a variable from system.datetime with the current time


            // rewrite the query so it only gets the song with the id that is november of urlyear, to january of urlyear + 1
          
            System.DateTime novemberyear = System.DateTime.SpecifyKind(new(int.Parse(urlyear), 11, 1), DateTimeKind.Utc);
            System.DateTime januaryyear = System.DateTime.SpecifyKind(new(int.Parse(urlyear) + 1, 1, 1), DateTimeKind.Utc);
            Query query = acidDecemberRef.WhereEqualTo("id", songid).WhereGreaterThanOrEqualTo("publishdate", novemberyear).WhereLessThanOrEqualTo("publishdate", januaryyear);
            QuerySnapshot querysnapshot = await query.GetSnapshotAsync();
            if (querysnapshot.Count == 0)
            {
                Song = new Song { };
            }
            else
            {
                DocumentSnapshot snapshot = querysnapshot.Documents[0];
                // get the data from the document
                // convert from google datetime to system datetime

                Song = new Song
                {
                    Title = snapshot.GetValue<string>("title") ?? string.Empty,
                    Artist = snapshot.GetValue<string>("artist") ?? string.Empty,
                    ImageLink = snapshot.GetValue<string>("imglink") ?? "_57b26574-11c6-4b6a-84d7-de0789a3c33a.jpeg",
                    Artistlink = snapshot.GetValue<string>("artistlink") ?? string.Empty,
                    Tune = snapshot.GetValue<string>("tune") ?? string.Empty,
                };

                var tracks = new List<Song> { Song }.Select(song => new
                {
                    metaData = new
                    {
                        artist = song.Artist,
                        title = song.Title,
                        album = "Acid December"
                    },
                    url = $"https://storage.googleapis.com/acid-december2012/{Year}/{song.Tune}",
                    duration = 5.322286 // This means nothing, it gets overwritten by the actual duration
                });

                ViewData["InitialTracks"] = JsonConvert.SerializeObject(tracks);

            }


        }

    }

}
