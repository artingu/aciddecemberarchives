/* Get tunes from Firestore and display them in a table. */

/* The datamodel of the object song is 
    * Title: the title of the song
    * Artist: the artist of the song
    * Publishdate: the date the song was published
    * ImageLink: the link to the image of the song
    * Artistlink: the link to the artist of the song


 */

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


public class IndexModel : PageModel
{
    private readonly FirestoreDb _db;
    private readonly ILogger<IndexModel> _logger;

    public List<Song>? Songs { get; set; }

    public IndexModel(ILogger<IndexModel> logger, FirestoreDb db)
    {
        _logger = logger;
        _db = db;
    }

    public class Song
    {
        public string? Title { get; set; }
        public string? Artist { get; set; }
        public string? Publishdate { get; set; }
        public string? ImageLink { get; set; }
        public string? Artistlink { get; set; }
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
            if (DateTime.Parse(documentDictionary["publishdate"].ToString().Substring(10)) > DateTime.Now)
            {
                Songs.Add(new Song
                {
                    Title = "?",
                    Artist = "?",
                    Publishdate = documentDictionary["publishdate"].ToString(),
                    ImageLink ="_b4418ac7-6827-4180-844a-e33c1e28308f.jpeg",
                    Artistlink = "?"

                });
            }
            else
            {
                Songs.Add(new Song
                {
                    Title = documentDictionary["title"].ToString(),
                    Artist = documentDictionary["artist"].ToString(),
                    Publishdate = documentDictionary["publishdate"].ToString(),
                    ImageLink = documentDictionary["imglink"].ToString(),
                    Artistlink = documentDictionary["artistlink"].ToString()
                });
            }
            
        }
    }
}

