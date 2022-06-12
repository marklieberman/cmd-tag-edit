// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder();
builder.AddCommandLine(args);

var config = builder.Build();

var input = config["input"];
if (String.IsNullOrEmpty(input))
{
    Console.WriteLine("--input is required");
    return;
}

if (!File.Exists(input))
{
    Console.WriteLine($"Cannot find the file: {input}");
    return;
}

var model = TagLib.File.Create(input);
var modified = false;

// Artist
var artist = config["artist"];
if (!String.IsNullOrEmpty(artist))
{
    model.Tag.Performers = artist.Split("/");
    modified |= true;
}

// Album Artist
var albumArtist = config["albumartist"];
if (!String.IsNullOrEmpty(albumArtist))
{
    model.Tag.AlbumArtists = albumArtist.Split("/");
    modified |= true;
}

// Album
var album = config["album"];
if (!String.IsNullOrEmpty(album))
{
    model.Tag.Album = album;
    modified |= true;
}

// Title
var title = config["title"];
if (!String.IsNullOrEmpty(title))
{
    model.Tag.Title = title;
    modified |= true;
}

// Track
var rawTrack = config["track"];
if (!String.IsNullOrEmpty(rawTrack))
{
    var parts = rawTrack.Split("/");
    if (parts.Length >= 1)
    {
        if (uint.TryParse(parts[0], out uint track))
        {
            model.Tag.Track = track;
            modified |= true;
        }
        else
        {
            Console.WriteLine($"Track '{parts[0]}' is not valid");
        }
    }
    if (parts.Length == 2)
    {
        if (uint.TryParse(parts[1], out uint count))
        {
            model.Tag.TrackCount = count;
            modified |= true;
        }
        else
        {
            Console.WriteLine($"Track count '{parts[1]}' is not valid");
        }
    }
}

// Year
var rawYear = config["year"];
if (!String.IsNullOrEmpty(rawYear) )
{
    if (uint.TryParse(rawYear, out uint year))
    {
        model.Tag.Year = year;
        modified |= true;
    }
    else
    {
        Console.WriteLine($"Year '{rawYear}' is not valid");
    }
}

// Save the tag.
if (modified)
{
    Console.WriteLine("Writing tag.");
    model.Save();
}
else 
{
    Console.WriteLine("No changes");
}