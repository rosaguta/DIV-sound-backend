namespace Logic.Helper;

using System;
using TagLib;

public class MetadataHandler
{
    public static void HandleMetadata(string filePath)
    {
        try
        {
            File file = File.Create(filePath);
            
            // Check the file format
            if (file.Name.EndsWith(".mp3"))
            {
                // Handle metadata for mp3 files
                var tag = (TagLib.Id3v2.Tag)file.GetTag(TagTypes.Id3v2);
                if (tag != null)
                {
                    string title = tag.Title;
                    string artist = tag.FirstPerformer;
                    string album = tag.Album;
                    
                    // Do something with the metadata (e.g., save to a database)
                    Console.WriteLine($"MP3 Metadata: Title - {title}, Artist - {artist}, Album - {album}");
                }
            }
            else if (file.Name.EndsWith(".wav"))
            {
                // Handle metadata for wav files (TagLib# doesn't fully support wav files)
                // You might want to use a different library for detailed WAV metadata extraction.
                // For basic metadata like duration, you can use TagLib#
                var duration = file.Properties.Duration;
                
                // Do something with the metadata (e.g., save to a database)
                Console.WriteLine($"WAV Duration: {duration}");
            }
            else
            {
                Console.WriteLine("Unsupported file format.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
