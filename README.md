# AutostartManager

Simple library to register applications for autostart. Adds a shortcut to the application in the users startup folder.
```C#
var registerShortcutForAllUser = false;
var autostartManager = new AutostartManager(Application.ProductName, Application.ExecutablePath, registerShortcutForAllUser);

autostartManager.IsAutostartEnabled();
autostartManager.EnableAutostart();
autostartManager.DisableAutostart();
```

https://www.nuget.org/packages/AutostartManager