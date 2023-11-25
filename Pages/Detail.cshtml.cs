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


public class DetailModel : PageModel
{
    private readonly FirestoreDb _db;
    private readonly ILogger<DetailModel> _logger;
    public Song? Song { get; set; }

    public DetailModel(ILogger<DetailModel> logger, FirestoreDb db)
    {
        _logger = logger;
        _db = db;
    }
    public async Task OnGetAsync(int id)
    {
        CollectionReference acidDecemberRef = _db.Collection("aciddecember");
        // get the id from the url
        if (Request.Query["id"].ToString() == null)
        {
                  Song = new Song {};
              
        }
        else

        {

            int songid = id;
            // get the firestore document with the id
            // create a variable from system.datetime with the current time


            Query query = acidDecemberRef.WhereEqualTo("id", songid).WhereLessThanOrEqualTo("publishdate", Timestamp.GetCurrentTimestamp());
            QuerySnapshot querysnapshot = await query.GetSnapshotAsync();
            if (querysnapshot.Count == 0)
            {
                  Song = new Song {};
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

            }


        }

    }

}





