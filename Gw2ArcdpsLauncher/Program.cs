using System;

namespace Gw2ArcdpsLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            string arcdpsSource = $@"C:\Users\{Environment.UserName}\Downloads\d3d9.dll";

            // Detect Gw2 install
            Console.Write("Detecting GW2 install directory...");
            string gw2Directory = "";

            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                string potentialGw2InstallDirectory = d.Name + @"\Program Files\Guild Wars 2\";
                if (Directory.Exists(potentialGw2InstallDirectory))
                {
                    gw2Directory = potentialGw2InstallDirectory;
                }
            }

            if(gw2Directory == "")
            {
                Console.WriteLine("Failed!\n");
                Console.WriteLine("Could not locate Gw2 install");
                Console.WriteLine("    You may have an unusual Gw2 installation, currently only installs in Program Files directories are supported.");
                Console.WriteLine("    Please either open an issue on Github with details of your installation or install Gw2 in Program Files.\n");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
                return;
            }
            else
            {
                Console.WriteLine("Done!");
                Console.WriteLine($"GW2 install directory detected at {gw2Directory}!\n");
            }

            // Download the latest Arcdps file
            Console.Write("Downloading Latest Arcdps...");
            using (var client = new System.Net.WebClient())
            {
                client.DownloadFile("https://www.deltaconnected.com/arcdps/x64/d3d9.dll", arcdpsSource);
            }
            Console.WriteLine("Done!");

            // Copy to main Gw2 directory
            Console.Write($"Copying to GW2 install at {gw2Directory}...");
            File.Copy(arcdpsSource, gw2Directory + @"\bin64\d3d9.dll", true);
            Console.WriteLine("Done!");

            // Check if GW2 Launcher exists, if so update Arcdps
            string gw2LauncherDirectory = gw2Directory + @"\Gw2Launcher";
            if (Directory.Exists(gw2LauncherDirectory))
            {
                Console.WriteLine("\nGw2Launcher detected... Copying Arcdps");
                foreach (string folderName in Directory.GetDirectories(gw2LauncherDirectory))
                {
                    string fullPath = folderName + @"\bin64\d3d9.dll";
                    Console.Write($"Copying to {fullPath}...");
                    File.Copy(arcdpsSource, fullPath, true);
                    Console.WriteLine("Done!");
                }
            }

            // Clean up downloaded file
            Console.Write("\nCleaning up Arcdps download...");
            File.Delete(arcdpsSource);
            Console.WriteLine("Done!\n");

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}