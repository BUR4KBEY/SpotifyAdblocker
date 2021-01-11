using System;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace SpotifyAdblocker
{
    class Program
    {
        static string currentPath = AppDomain.CurrentDomain.BaseDirectory;
        static string appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        static string zipName = "SpotifyAdblocker.zip";
        static void Main(string[] args)
        => GetSpotifyDirectory();

        static void Next(string spotifyDirectory)
        {
            try
            {
                Console.Clear();
                Console.WriteLine($"Spotify Directory: {spotifyDirectory}");
                Console.WriteLine("Checking files...");

                string dllPath = $"{spotifyDirectory}\\chrome_elf.dll";
                string configPath = $"{spotifyDirectory}\\config.ini";

                if (File.Exists(dllPath)) File.Delete(dllPath);
                if (File.Exists(configPath)) File.Delete(configPath);

                Console.WriteLine("Download starting...");

                using (WebClient wc = new WebClient())
                {
                    string zipPath = $"{spotifyDirectory}\\{zipName}";
                    wc.DownloadFile(new Uri("https://github.com/mrpond/BlockTheSpot/releases/latest/download/chrome_elf.zip"), zipPath);
                    Console.WriteLine("Extracting...");
                    ZipFile.ExtractToDirectory(zipPath, spotifyDirectory);
                    File.Delete(zipPath);
                    Console.WriteLine("Compleated!");
                    Console.WriteLine("Successful! You can close this window and run the spotify. Enjoy :)");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
        }

        static void GetSpotifyDirectory()
        {
            if (File.Exists($"{appdataPath}\\Spotify\\Spotify.exe")) Next($"{appdataPath}\\Spotify");
            else
            {
                Console.Write("Please write your spotify directory and be sure the spotify not running: ");
                string spotifyPath = Console.ReadLine();
                if (spotifyPath == "" || spotifyPath == string.Empty || spotifyPath == null || spotifyPath.Contains(" ")) GetSpotifyDirectory();
                else if (!Directory.Exists(spotifyPath)) GetSpotifyDirectory();
                else if (!File.Exists($"{spotifyPath}\\Spotify.exe")) GetSpotifyDirectory();
                else Next(spotifyPath);
            }
        }
    }
}
