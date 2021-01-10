using HtmlAgilityPack;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Xml.XPath;

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

                var url = "https://github.com/mrpond/BlockTheSpot/releases/latest";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream());
                string sourceCode = sr.ReadToEnd();
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(sourceCode);
                XPathNavigator navigator = (HtmlNodeNavigator)document.CreateNavigator();
                string link = "https://github.com" + navigator.SelectSingleNode("/html/body/div[4]/div/main/div[2]/div/div[2]/div/div[2]/details/div/div/div[1]/a").GetAttribute("href", string.Empty);

                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFile(new Uri(link), zipPath);
                    ZipFile.ExtractToDirectory(zipPath, folderPath);
                    File.Delete(zipPath);
                    GetSpotifyDirectory();
                }
            } catch(Exception ex)
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
            } catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
        }
    }
}
