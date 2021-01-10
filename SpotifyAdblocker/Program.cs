using System;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace SpotifyAdblocker
{
    class Program
    {
        static string currentPath = AppDomain.CurrentDomain.BaseDirectory;
        static string folderPath = $"{currentPath}data";
        static void Main(string[] args)
        {
            try
            {
                string zipPath = $"{currentPath}\\File.zip";

                if (File.Exists(zipPath)) File.Delete(zipPath);
                if (Directory.Exists(folderPath)) Directory.Delete(folderPath, true);

                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFile(new Uri("https://github.com/mrpond/BlockTheSpot/releases/latest/download/chrome_elf.zip"), zipPath);
                    ZipFile.ExtractToDirectory(zipPath, folderPath);
                    File.Delete(zipPath);
                    GetSpotifyDirectory();
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
            try
            {
                Console.Write("Please write your spotify directory and be sure the spotify not running: ");
                string spotifyPath = Console.ReadLine();
                if (spotifyPath == "" || spotifyPath == string.Empty || spotifyPath == null || spotifyPath.Contains(" ")) GetSpotifyDirectory();
                else if (!Directory.Exists(spotifyPath)) GetSpotifyDirectory();
                else if (!File.Exists($"{spotifyPath}\\Spotify.exe")) GetSpotifyDirectory();
                else
                {
                    string chromeDllPath = $"{spotifyPath}\\chrome_elf.dll";
                    string configIniPath = $"{spotifyPath}\\config.ini";

                    if (File.Exists(chromeDllPath)) File.Delete(chromeDllPath);
                    if (File.Exists(configIniPath)) File.Delete(configIniPath);

                    File.Move($"{folderPath}\\chrome_elf.dll", $"{spotifyPath}\\chrome_elf.dll");
                    File.Move($"{folderPath}\\config.ini", $"{spotifyPath}\\config.ini");
                    Directory.Delete(folderPath, true);
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
    }
}
