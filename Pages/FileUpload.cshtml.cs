namespace aciddecemberarchives.Pages;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Google.Cloud.Firestore;
using AcidDec.Models;
using Google.Cloud.Storage.V1;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class FileUploadModel : PageModel
{
    [BindProperty]
    [Required]
    public new IFormFile File { get; set; } = null!;
    private readonly FirestoreDb? _db;
    private readonly ILogger<FileUploadModel> _logger;




    public FileUploadModel(ILogger<FileUploadModel> logger)
    {
        _logger = logger;
        BucketObjects = new List<Google.Apis.Storage.v1.Data.Object>();
        // FileUpload = new FileUploadModel(); // Removed incorrect instantiation
    }


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

    // Handler for the file upload form
    public async Task<IActionResult> OnPostUploadFileAsync()
    {
        if (!TryValidateModel(this, nameof(File)))
        {
            return Page();
            //show error message


        }

        // Upload the file to Google Cloud Storage
        string bucketName = "acid-december2012";

        string objectName = $"2024/{File.FileName}";

        using (var stream = File.OpenReadStream())
        {
            var storage = await StorageClient.CreateAsync();
            await storage.UploadObjectAsync(bucketName, objectName, null, stream);
        }
        // After processing, reload the bucket objects
        await LoadBucketObjectsAsync();
        // Store the uploaded file info in TempData
       /*  TempData["UploadedFileName"] = objectName;
 */
        ViewData["UploadSuccess"] = "File uploaded successfully!";
        return Page();
    }

}