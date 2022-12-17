using CmdTagEdit;
using System.CommandLine;
using System.Drawing;
using System.Drawing.Imaging;

// See https://aka.ms/new-console-template for more information

var fileOption = new Option<string>(
        name: "--file",
        description: "File to tag."
);
fileOption.IsRequired = true;
var artistOption = new Option<string>(
        name: "--artist"
);
var albumArtistOption = new Option<string>(
        name: "--album-artist"
);
var albumOption = new Option<string>(
        name: "--album"
);
var titleOption = new Option<string>(
        name: "--title"
);
var trackOption = new Option<string>(
        name: "--track"
);
var yearOption = new Option<int?>(
        name: "--year"
);
var genreOption = new Option<string>(
        name: "--genre"
);
var coverUrlOption = new Option<string>(
        name: "--cover-url"
);

var rootCommand = new Command("tag", "Write tags in a file.")
{
    fileOption,
    artistOption,
    albumArtistOption,
    albumOption,
    titleOption,
    trackOption,
    genreOption,
    yearOption,    
    coverUrlOption
};

rootCommand.SetHandler(async (context) =>
{
    await tag(
        context.ParseResult.GetValueForOption(fileOption),
        context.ParseResult.GetValueForOption(artistOption),
        context.ParseResult.GetValueForOption(albumArtistOption),
        context.ParseResult.GetValueForOption(albumOption),
        context.ParseResult.GetValueForOption(titleOption),
        context.ParseResult.GetValueForOption(trackOption),
        context.ParseResult.GetValueForOption(genreOption),
        context.ParseResult.GetValueForOption(yearOption),
        context.ParseResult.GetValueForOption(coverUrlOption)
    );
});

return await rootCommand.InvokeAsync(args);

async Task tag (string? file, string? artist, string? albumArtist, string? album, string? title, string? track, string? genre, int? year, string? coverUrl)
{
    if (!File.Exists(file))
    {
        Console.WriteLine($"Cannot find the file: ${file}");
        return;
    }

    var model = new ATL.Track(file);
    bool modified = false;

    if (!String.IsNullOrEmpty(artist))
    {
        model.Artist = artist;
        modified |= true;
    }

    if (!String.IsNullOrEmpty(albumArtist))
    {
        model.AlbumArtist = albumArtist;
        modified |= true;
    }

    if (!String.IsNullOrEmpty(album))
    {
        model.Album = album;
        modified |= true;
    }

    if (!String.IsNullOrEmpty(title))
    {
        model.Title = title;
        modified |= true;
    }

    if (!String.IsNullOrEmpty(track))
    {
        var parts = track.Split("/");
        if (parts.Length >= 1)
        {
            if (int.TryParse(parts[0], out int trackNumber))
            {
                model.TrackNumber = trackNumber;
                modified |= true;
            }
            else
            {
                Console.WriteLine($"Track '{parts[0]}' is not valid");
            }
        }
        if (parts.Length == 2)
        {
            if (int.TryParse(parts[1], out int trackTotal))
            {
                model.TrackTotal = trackTotal;
                modified |= true;
            }
            else
            {
                Console.WriteLine($"Track count '{parts[1]}' is not valid");
            }
        }
    }

    if (!String.IsNullOrEmpty(genre))
    {
        model.Genre = genre;
        modified |= true;
    }

    if (year != null)
    {
        model.Year = year;
        modified |= true;
    }

    if (!String.IsNullOrEmpty(coverUrl))
    {
        // Remove any existing artwork.
        var coverIndex = 0;
        while (coverIndex > -1)
        {
            coverIndex = model.EmbeddedPictures.FindIndex(0, p => p.PicType == ATL.PictureInfo.PIC_TYPE.Front);
            if (coverIndex > -1)
            {
                model.EmbeddedPictures.RemoveAt(coverIndex);
                modified |= true;
            }
        }

        // Embed the new artwork.        
        try
        {
            // TODO Need to convert WebP to JPG.
            Console.Write("Embedding artwork: Front...");
            var data = await new HttpClient().GetByteArrayAsync(coverUrl);
            ATL.PictureInfo coverArt = ATL.PictureInfo.fromBinaryData(data, ATL.PictureInfo.PIC_TYPE.Front);
            model.EmbeddedPictures.Add(coverArt);            
            Console.WriteLine("Done");

            modified |= true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to embed artwork: ${ex.Message} ${ex.StackTrace}");
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
}

