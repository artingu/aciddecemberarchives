namespace aciddecemberarchives.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using AcidDec.Models;
using Google.Cloud.Storage.V1;
using System.Runtime.InteropServices;


// allows for the submission of new songs
public class SongDetailsModel
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Artist { get; set; } = string.Empty;
    [Required]
    public DateTime PublishDate { get; set; } = DateTime.UtcNow;

    public string ImageLink { get; set; } = string.Empty;

    public string? ArtistLink { get; set; } = string.Empty;

    [Required]
    public string Tune { get; set; } = string.Empty;

}

public class SubmitModel : PageModel
{
    private readonly FirestoreDb _db;
    private readonly ILogger<SubmitModel> _logger;

    private readonly TrackService _trackService;

    public List<Song> Songs => _trackService.Songs;
    public SubmitModel(ILogger<SubmitModel> logger, FirestoreDb db)
    {
        _logger = logger;
        _db = db;
        SongDetails = new SongDetailsModel();
        BucketObjects = new List<Google.Apis.Storage.v1.Data.Object>();
        _trackService = new TrackService(db);
    }

    // Bind the song details form
    [BindProperty(Name = "SongDetails")]
    public SongDetailsModel SongDetails { get; set; }

    // Property to hold the list of bucket objects
    public List<Google.Apis.Storage.v1.Data.Object> BucketObjects { get; set; }

    private async Task LoadBucketObjectsAsync()
    {
        // Initialize the StorageClient
        var storage = await StorageClient.CreateAsync();

        // Specify the bucket name and prefix
        string bucketName = "acid-december2012";
        string prefix = "2024/";

        // List the objects in the bucket
        var objects = storage.ListObjectsAsync(bucketName, prefix);

        // Initialize the list
        BucketObjects = new List<Google.Apis.Storage.v1.Data.Object>();

        await foreach (var storageObject in objects)
        {
            // Remove the prefix from the object name
            storageObject.Name = storageObject.Name.Replace(prefix, "");
            storageObject.MediaLink = storageObject.MediaLink.Replace(prefix, "");
            BucketObjects.Add(storageObject);

        }
    }
    public async Task OnGetAsync()
    {
        await _trackService.LoadTracksAsync();
        // sort the songs by id
        Songs.Sort((a, b) => a.Id.Value.CompareTo(b.Id.Value));

        await LoadBucketObjectsAsync();
    }

    // Handler for the song details
    public async Task<IActionResult> OnPostSaveSongAsync()
    {
        if (!TryValidateModel(SongDetails, nameof(SongDetails)))
        {
            // i want to return the page with the form filled out with the previous values



            return Page();
        }

        // Save song details to Firestore
        var newSong = new Dictionary<string, object>
        {
            {"title", SongDetails.Title},
            {"artist", SongDetails.Artist},
            {"publishdate", Timestamp.FromDateTime(SongDetails.PublishDate.ToUniversalTime())},
            {"imglink", SongDetails.ImageLink},
            {"artistlink", SongDetails.ArtistLink},
            {"tune", SongDetails.Tune},
            {"id", SongDetails.Id}
        };

        var songsRef = _db.Collection("aciddecember");
        await songsRef.AddAsync(newSong);
        // show success message
        ViewData["SaveSuccess"] = "Song details saved successfully!";

        // After processing, reload the bucket objects
        await LoadBucketObjectsAsync();
        return Page();
    }
}