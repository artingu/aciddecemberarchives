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



        /*      Get every artist */
        Songs = new List<Song>();

        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            Dictionary<string, object> documentDictionary = document.ToDictionary();
            Songs.Add(new Song
            {
                Id = Convert.ToInt32(documentDictionary["id"]),
                Title = documentDictionary["title"]?.ToString(),
                Artist = documentDictionary["artist"]?.ToString(),

            });
        }
        // remove duplicates from Songs based on Artist
        Songs = [.. Songs.GroupBy(s => s.Artist).Select(g => g.First())];
    }
}
