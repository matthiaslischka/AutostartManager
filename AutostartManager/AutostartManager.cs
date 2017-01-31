using System;
using System.Diagnostics;
using System.IO;
using IWshRuntimeLibrary;

namespace AutostartManagement
{
    public class AutostartManager
    {
        private readonly string _executeablePath;
        private readonly FileInfo _lnkFileInfo;

        public AutostartManager(string executeablePath)
            : this(Path.GetFileNameWithoutExtension(executeablePath), executeablePath, false)
        {
        }

        public AutostartManager(string executeablePath, bool forAllUsers)
            : this(Path.GetFileNameWithoutExtension(executeablePath), executeablePath, forAllUsers)
        {
        }

        public AutostartManager(string appName, string executeablePath, bool forAllUsers)
        {
            _executeablePath = executeablePath;
            var startupFolderPath = forAllUsers
                ? Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup)
                : Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            _lnkFileInfo = new FileInfo(Path.Combine(startupFolderPath, appName + ".lnk"));
        }

        public void EnableAutostart()
        {
            var shell = new WshShell();
            var wshShortcut = (IWshShortcut) shell.CreateShortcut(_lnkFileInfo.FullName);
            wshShortcut.TargetPath = _executeablePath;
            ErrorIfException(() => wshShortcut.Save());
        }

        public void DisableAutostart()
        {
            ErrorIfException(() => _lnkFileInfo.Delete());
        }

        public bool IsAutostartEnabled()
        {
            _lnkFileInfo.Refresh();
            return _lnkFileInfo.Exists;
        }

        private void ErrorIfException(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
            }
        }
    }
}