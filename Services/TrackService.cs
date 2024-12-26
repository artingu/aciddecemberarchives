using Microsoft.Extensions.Logging;
using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel.DataAnnotations;
using AcidDec.Models;

public class TrackService
{
    private readonly FirestoreDb _db;
    public List<Song> Songs { get; private set; } = new();

    public TrackService(FirestoreDb db)
    {
        _db = db;
    }

    public async Task LoadTracksAsync()
    {
        var tracksRef = _db.Collection("aciddecember");
        var snapshot = await tracksRef.GetSnapshotAsync();
        Songs = snapshot.Documents.Select(doc => doc.ConvertTo<Song>()).ToList();
    }
}