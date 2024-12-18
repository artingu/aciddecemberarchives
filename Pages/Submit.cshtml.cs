using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Google.Cloud.Firestore;
using AcidDec.Models;
using Google.Cloud.Storage.V1;


namespace aciddecemberarchives.Pages;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel.DataAnnotations;

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

    public string ArtistLink { get; set; } = string.Empty;
    [Required]
    public string Tune { get; set; } = string.Empty;
}

/* public class FileUploadModel
{
    [Required]
    public IFormFile File { get; set; } = null!;
} */

public class SubmitModel : PageModel
{
    private readonly FirestoreDb _db;
    private readonly ILogger<SubmitModel> _logger;

    public SubmitModel(ILogger<SubmitModel> logger, FirestoreDb db)
    {
        _logger = logger;
        _db = db;
        SongDetails = new SongDetailsModel();
        /*    FileUpload = new FileUploadModel(); */
        BucketObjects = new List<Google.Apis.Storage.v1.Data.Object>();
    }

    // Bind the song details form
    [BindProperty(Name = "SongDetails")]
    public SongDetailsModel SongDetails { get; set; }

    // Bind the file upload form
    /*   [BindProperty(Name = "FileUpload")]
      public FileUploadModel FileUpload { get; set; }
   */
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
            BucketObjects.Add(storageObject);
        }
    }
    public async Task OnGetAsync()
    {


        await LoadBucketObjectsAsync();
    }

    // Handler for the song details form
    public async Task<IActionResult> OnPostSaveSongAsync()
    {
        if (!TryValidateModel(SongDetails, nameof(SongDetails)))
        {
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

    // Handler for the file upload form
    /*   public async Task<IActionResult> OnPostUploadFileAsync()
      {
          if (!TryValidateModel(FileUpload, nameof(FileUpload)))
          {
              return Page();
          }

          // Upload the file to Google Cloud Storage
          string bucketName = "your-bucket-name";
          string objectName = $"uploads/{Guid.NewGuid()}_{FileUpload.File.FileName}";

          using (var stream = FileUpload.File.OpenReadStream())
          {
              var storage = await StorageClient.CreateAsync();
              await storage.UploadObjectAsync(bucketName, objectName, null, stream);
          }
          // After processing, reload the bucket objects
          await LoadBucketObjectsAsync();
          // Store the uploaded file info in TempData
          TempData["UploadedFileName"] = objectName;

          ViewData["UploadSuccess"] = "File uploaded successfully!";
          return Page();
      } */

}