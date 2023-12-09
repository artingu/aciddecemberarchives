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


            /* Check if Publishdate is in the future */
            if (DateTime.Parse(documentDictionary["publishdate"].ToString().Substring(10)) > DateTime.Now)
            {
                Songs.Add(new Song
                {
                    Title = "?",
                    Artist = "?",
                    Publishdate = documentDictionary["publishdate"].ToString(),
                    ImageLink = "_b4418ac7-6827-4180-844a-e33c1e28308f.jpeg",
                    Artistlink = ""

                });
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
                    Id = documentDictionary["id"].ToString()
                };
               
                Songs.Add(s);
                SongOfTheDay = s;
            }
   // song of the day
     
    
        }
    }
}

