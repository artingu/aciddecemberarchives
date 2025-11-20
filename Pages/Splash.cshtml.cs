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
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;

public class SplashModel : PageModel
{
    private readonly FirestoreDb _db;
    private readonly ILogger<SplashModel> _logger;

    public List<Song>? Songs { get; set; }
    public List<Song>? Artists { get; set; }

    public SplashModel(ILogger<SplashModel> logger, FirestoreDb db)
    {
        _logger = logger;
        _db = db;
    }

    public HtmlString xmlitem = new("<item>");
    public HtmlString xmlitemend = new("</item>");
    public async Task OnGetAsync()
    {
        CollectionReference acidDecemberRef = _db.Collection("aciddecember");
        // get all posts from the database id ascending
        Query q = acidDecemberRef.OrderBy("id");

        QuerySnapshot snapshot = await q.GetSnapshotAsync();



        /*Get every artist */
        Songs = new List<Song>();
        Artists = new List<Song>();

        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            Dictionary<string, object> documentDictionary = document.ToDictionary();
            var publishTimestamp = documentDictionary["publishdate"] as Google.Cloud.Firestore.Timestamp?;
            var year = publishTimestamp?.ToDateTime().Year.ToString() ?? "unknown";
            if (publishTimestamp?.ToDateTime().Month < 11)
            {
                year = (publishTimestamp?.ToDateTime().Year - 1).ToString();
                publishTimestamp = Google.Cloud.Firestore.Timestamp.FromDateTime(new DateTime((int)(publishTimestamp?.ToDateTime().Year - 1), 11, publishTimestamp?.ToDateTime().Day ?? 1, publishTimestamp?.ToDateTime().Hour ?? 0, publishTimestamp?.ToDateTime().Minute ?? 0, publishTimestamp?.ToDateTime().Second ?? 0, DateTimeKind.Utc));
            }
            Songs.Add(new Song
            {
                Id = Convert.ToInt32(documentDictionary["id"]),
                Title = documentDictionary["title"]?.ToString(),
                Artist = documentDictionary["artist"]?.ToString(),
                Artistlink = documentDictionary["artistlink"]?.ToString(),
                Publishdate = publishTimestamp,
                Tune = $"https://storage.googleapis.com/acid-december2012/{year}/{documentDictionary["tune"]?.ToString()}"
            });
        }
        // remove duplicates from Songs based on Artist and sort alphabetically
        Artists = [.. Songs.GroupBy(s => s.Artist).Select(g => g.First()).OrderBy(a => a.Artist)];
    }
}
