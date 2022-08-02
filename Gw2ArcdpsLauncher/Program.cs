using System;

namespace Gw2ArcdpsLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Select which version of Arcdps:");
            Console.WriteLine("[1] DX9");
            Console.WriteLine("[2] DX11");
            string arcDpsVersion = Console.ReadLine();

            string arcDpsFileName = "";
            string arcDpsInstallDir = "";
            switch (arcDpsVersion)
            {
                case "1":
                    arcDpsFileName = "d3d9.dll";
                    arcDpsInstallDir = @"\bin64\d3d9.dll";
                    break;
                case "2":
                    arcDpsFileName = "d3d11.dll";
                    arcDpsInstallDir = @"\d3d11.dll";
                    break;
                default:
                    Console.WriteLine("Must select a valid option.");
                    return;
            }

                    Console.Write("Detecting Downloads directory...");
            string arcdpsSource = "";

            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                string potentialDownloadDir = d.Name + @"Users\" + Environment.UserName + @"\Downloads\";
                string potentialDownloadDir2 = d.Name + @"Downloads\";
                if (Directory.Exists(potentialDownloadDir))
                {
                    arcdpsSource = potentialDownloadDir + arcDpsFileName;
                }
                else if (Directory.Exists(potentialDownloadDir2))
                {
                    arcdpsSource = potentialDownloadDir2 + arcDpsFileName;
                }
            }

            if (arcdpsSource == "")
            {
                Console.WriteLine("Failed!\n");
                Console.WriteLine("Could not locate Downloads directory");
                Console.WriteLine("    You may have an unusual Downloads directory, currently only Downloads in DRIVE:\\User\\USERNAME\\Downloads or DRIVE:\\Downloads are supported.");
                Console.WriteLine("    Please either open an issue on Github with details of your installation or use a normal Downloads setup.\n");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
                return;
            }
            else
            {
                Console.WriteLine("Done!");
                Console.WriteLine($"Downloads directory detected at {arcdpsSource}!\n");
            }

            // Detect Gw2 install
            Console.Write("Detecting GW2 install directory...");
            string gw2Directory = "";

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
            File.Copy(arcdpsSource, $"{gw2Directory}{arcDpsInstallDir}", true);
            Console.WriteLine("Done!");

            // Check if GW2 Launcher exists, if so update Arcdps
            string gw2LauncherDirectory = gw2Directory + @"\Gw2Launcher";
            if (Directory.Exists(gw2LauncherDirectory))
            {
                Console.WriteLine("\nGw2Launcher detected... Copying Arcdps");
                foreach (string folderName in Directory.GetDirectories(gw2LauncherDirectory))
                {
                    string fullPath = $"{folderName}{arcDpsInstallDir}";
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