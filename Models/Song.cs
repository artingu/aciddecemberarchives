using Google.Cloud.Firestore;
namespace AcidDec.Models
{
    [FirestoreData]
    public class Song
    {
        [FirestoreProperty("title")]
        public string? Title { get; set; }
        [FirestoreProperty("artist")]
        public string? Artist { get; set; }
        [FirestoreProperty("publishdate")]
         public Google.Cloud.Firestore.Timestamp? Publishdate { get; set; } 
        [FirestoreProperty("imglink")]
        public string? ImageLink { get; set; }
        [FirestoreProperty("artistlink")]
        public string? Artistlink { get; set; }
        [FirestoreProperty("tune")]
        public string? Tune { get; set; }
        [FirestoreProperty("id")]
        public int? Id { get; set; }
    }
}
