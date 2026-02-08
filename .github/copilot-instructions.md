# Copilot Instructions for Acid December Archives

## Project Overview
A .NET 8 ASP.NET Core Razor Pages web application that archives and catalogs the Acid December electronic music festival. Features include:
- Firestore-backed song database with metadata (artist, title, publish date, image links, audio files)
- User authentication with BCrypt password hashing for content submission
- Rate limiting on submissions (40 requests per 12 seconds)
- Session-based authentication for `/FileUpload` and `/Submit` pages only
- Google Cloud Storage integration for audio files
- Webamp playlist streaming for playback

## Architecture

### Core Data Flow
1. **Models** (`Models/Song.cs`): Firestore data model with `[FirestoreProperty]` attributes mapping to database fields
2. **Services** (`Services/TrackService.cs`): Singleton service managing song collection with `LoadTracksAsync()` 
3. **Pages** (`Pages/`): Razor Page handlers using dependency injection for FirestoreDb and ILogger
4. **wwwroot**: Static content organized by year (2011-2022), songs reference year-specific paths

### Key Integration Points
- **Google Cloud Firestore**: Collection `"aciddecember"` stores Song documents; collection `"acidlogins"` stores user credentials
- **Google Cloud Storage**: Bucket `"acid-december2012"` holds audio files referenced by `Song.Tune` field
- **Authentication**: Cookie-based using `CookieAuthenticationDefaults` with 1440-minute session expiry and sliding expiration

## Common Patterns

### Razor Page Handlers
- Constructor injection of `FirestoreDb`, `ILogger<T>`, and services
- `OnGetAsync()` for page loads; `OnPostXxxAsync()` for form submissions (e.g., `OnPostSaveSongAsync()`)
- Use `[BindProperty]` attribute to map form inputs to model properties
- Use `ViewData` dictionary for passing data to Razor views
- `[Authorize]` attribute gates authenticated pages (e.g., Submit.cshtml.cs)

**Example**: See `Pages/Index.cshtml.cs` for date filtering logic across November 30 - January 30 seasonal ranges.

### Firestore Operations
- Explicit `Timestamp.FromDateTime()` conversion for DateTime fields
- `Document.ToDictionary()` pattern for type-safe field extraction
- Use document Firestore IDs (not numeric `id` field) for updates: `_db.Collection("aciddecember").Document(docId).UpdateAsync()`

**Example**: Submit.cshtml.cs demonstrates both `AddAsync()` for new songs and `UpdateAsync()` for modifications.

### Authentication Flow
- Login validates against `"acidlogins"` collection using BCrypt.Verify()
- Session middleware applied conditionally via `UseWhen()` in Program.cs—only for Submit/FileUpload paths
- Failed logins return generic "Invalid credentials" message (no user enumeration)

## Configuration & Deployment

### Build & Run
- **Framework**: .NET 8.0 with nullable reference types enabled
- **Dependencies**: Google Cloud Firestore, Storage, BCrypt.Net, Newtonsoft.Json, QRCoder, Otp.Net
- **Tasks**: Use `dotnet build`, `dotnet publish`, or `dotnet watch run` (configured in VS Code)
- **Docker**: Multi-stage build; runs on port 8080 with aspnet:8.0 runtime

### Environment
- **Firestore Project ID**: `"seismic-ground-286510"` (hardcoded in Program.cs)
- **Google Cloud credentials**: Expected via standard Application Default Credentials flow (requires `GOOGLE_APPLICATION_CREDENTIALS` or workload identity)
- **Development**: appsettings.Development.json enables DetailedErrors

## Important Constraints & Gotchas

1. **Year Routing**: Index.cshtml.cs uses URL parameter `urlyear` to filter songs between Nov 30 and Jan 30. Future years require hardcoding date ranges.
2. **Rate Limiter**: 40 permits per 12 seconds applies globally (set in Program.cs), affects FileUpload/Submit endpoints.
3. **Session Only on Protected Paths**: Authentication/session middleware bypasses most pages via `UseWhen()` condition—don't assume User.Identity is populated everywhere.
4. **Document ID vs ID Field**: Songs have both a numeric `id` field (for sorting) and a Firestore document ID; updates require the document ID string.
5. **Timezone**: DateTime conversions use UTC; `DateTime.Now` comparisons may drift in production.
6. **Redundant TrackService Initialization**: Submit.cshtml.cs creates a new TrackService instance instead of injecting singleton—consider refactoring.

## Recommended Workflows

- **Adding a new song field**: Add to `Song.cs` with `[FirestoreProperty]` attribute, update form in Submit.cshtml, and Firestore write logic
- **Modifying authentication**: Update `Program.cs` session/cookie options and Login.cshtml.cs validation logic
- **Scaling storage**: Replace hardcoded bucket name `"acid-december2012"` with configuration
- **Testing**: No unit tests present; consider adding for Firestore mocking and date range logic
