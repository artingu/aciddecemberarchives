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


public class RssModel : PageModel
{
    private readonly FirestoreDb _db;
    private readonly ILogger<RssModel> _logger;

    public List<Song>? Songs { get; set; }
    public List<string>? RssFeed { get; set; }

    public RssModel(ILogger<RssModel> logger, FirestoreDb db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task OnGetAsync()
    {
        CollectionReference acidDecemberRef = _db.Collection("aciddecember");
        QuerySnapshot snapshot = await acidDecemberRef.GetSnapshotAsync();

        /*      Populate the variable Songs with the data from Firestore. */
        Songs = new List<Song>();
        
        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            Dictionary<string, object> documentDictionary = document.ToDictionary();


            /* Check if Publishdate is in the future */
            if (DateTime.TryParse(documentDictionary["publishdate"]?.ToString()[10..], out DateTime publishDate) && publishDate > DateTime.Now)
            {
                continue;
            }
            else
            {
                Songs.Add(new Song
                {
                    Title = documentDictionary["title"].ToString(),
                    Artist = documentDictionary["artist"].ToString(),
                    Publishdate = documentDictionary["publishdate"].ToString(),
                    ImageLink = documentDictionary["imglink"].ToString(),
                    Artistlink = documentDictionary["artistlink"].ToString(),
                    Id = documentDictionary["id"].ToString()
                });
                
            }
            
        }
    }
}

