## One Click Install/Un-install a Windows service on Windows OS?
![Dependencies](https://img.shields.io/badge/dependencies-up%20to%20date-brightgreen.svg)
![Contributions welcome](https://img.shields.io/badge/contributions-welcome-orange.svg)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://opensource.org/licenses/MIT)


                    Tested with .NET version: v4.0.30319 @ Dev code :SPSU
                  

## + Install a .NET service

   1) Create a Directory in c drive C:\CustomService
   2) Build the .NET Project and Copy content of service at       CustomReminderService\bin\Debug\
   to the folder above - double check you should have file CutomWindowService.exe 
   3) copy both bat file from [Your directory location ]\CustomServiceInstaller into same directory where
   service exe reside.
   4) Open command prompt as **administrator** and change directory to C:\CustomService\Installer.bat. Enter the following credentials when prompted: 

                - username  - domain\username
                - password  - password

5) see the log message on console.


## + Uninstall a .NET Service

Open command prompt as **administrator** and run the uninstall bat file by changing directory to C:\CustomService\Uninstaller.bat

+ see the log msg for sucess