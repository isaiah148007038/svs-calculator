SVS Web App - Version 2

What is new in v2:
- Alliance login page
- Separate admin login page
- Calculator saves player entries
- Optional screenshot upload and notes
- Compare page for saved players
- Admin dashboard with delete controls
- Data stored in Data/players.json
- Uploaded screenshots stored in wwwroot/uploads

Default codes:
- Alliance code: BRG225
- Admin code: BRG225-ADMIN

How to run locally:
1. Install .NET 8 SDK
2. Open a terminal in this folder
3. Run: dotnet run
4. Open the localhost URL shown in the terminal

Or open SvsWebApp.csproj in Visual Studio and press F5.

Important:
- Screenshot uploads are saved, but this version does NOT auto-read stats from images yet.
- Change the alliance and admin codes before public hosting.
- For production, add a database and real user accounts later.
