using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Downloader;
using Infrastructure;
using Player;
using Playlist;
using UserManager;
using VKAPI;

namespace VVKtest1
{
    class Program
    {
        static void Main(string[] args)
        {
            Player.Player Player1 = new Player.Player();
            Song some = new Song(1);
            some.Downloaded = true;
            some.DownloadedUri = new Uri("C:/test.mp3");
            Player1.SetSource(some);
            Player1.Play();
            Console.Write("go");
            Console.ReadKey();
        }
    }
}
