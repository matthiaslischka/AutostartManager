using System;
using System.IO;
using AutostartManagement;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutostartManagerTests
{
    [TestClass]
    public class UnitTest
    {
        private readonly string _commonStartupFolderString =
            Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup);

        private readonly string _currenUserStartupFolder =
            Environment.GetFolderPath(Environment.SpecialFolder.Startup);

        private string _appName;
        private FileInfo _commonStartupLnkFile;
        private FileInfo _currentUserStartupLnkFile;
        private string _executeablePath;
        private bool _forAllUsers;

        [TestInitialize]
        public void Setup()
        {
            _appName = "TestApp";
            _executeablePath = @"C:\Windows\System32\cmd.exe";
            _forAllUsers = false;
            _commonStartupLnkFile = new FileInfo(Path.Combine(_commonStartupFolderString, _appName + ".lnk"));
            _commonStartupLnkFile.Delete();
            _currentUserStartupLnkFile = new FileInfo(Path.Combine(_currenUserStartupFolder, _appName + ".lnk"));
            _currentUserStartupLnkFile.Delete();
        }

        [TestMethod]
        public void IsAutostartEnabled_NoAutostartEnabled_NotEnabled()
        {
            var autostartManager = new AutostartManager(_appName, _executeablePath, _forAllUsers);
            autostartManager.IsAutostartEnabled().Should().BeFalse();
        }

        [TestMethod]
        public void IsAutostartEnabled_AutostartEnabled_IsEnabled()
        {
            var autostartManager = new AutostartManager(_appName, _executeablePath, _forAllUsers);
            autostartManager.EnableAutostart();
            autostartManager.IsAutostartEnabled().Should().BeTrue();
        }

        [TestMethod]
        public void IsAutostartEnabled_AutostartEnabledAndDisabled_IsDisabled()
        {
            var autostartManager = new AutostartManager(_appName, _executeablePath, _forAllUsers);
            autostartManager.EnableAutostart();
            autostartManager.DisableAutostart();
            autostartManager.IsAutostartEnabled().Should().BeFalse();
        }

        [TestMethod]
        public void EnableAutostart_ForCurrentUser_LnkFileExistsInCurrentUserStartupFolder()
        {
            var autostartManager = new AutostartManager(_appName, _executeablePath, false);
            autostartManager.EnableAutostart();
            _currentUserStartupLnkFile.Exists.Should().BeTrue();
            _commonStartupLnkFile.Exists.Should().BeFalse();
        }

        [TestMethod]
        public void EnableAutostart_ForAllUser_LnkFileExistsInCommonStartupFolder()
        {
            var autostartManager = new AutostartManager(_appName, _executeablePath, true);
            autostartManager.EnableAutostart();
            _commonStartupLnkFile.Exists.Should().BeTrue();
            _currentUserStartupLnkFile.Exists.Should().BeFalse();
        }

        [TestMethod]
        public void IsAutostartEnabled_TurningOffAndOnAgainAndAgain_ShouldKeepUp()
        {
            var autostartManager = new AutostartManager(_appName, _executeablePath, false);
            autostartManager.IsAutostartEnabled().Should().BeFalse();
            autostartManager.EnableAutostart();
            autostartManager.IsAutostartEnabled().Should().BeTrue();
            autostartManager.DisableAutostart();
            autostartManager.IsAutostartEnabled().Should().BeFalse();
            autostartManager.EnableAutostart();
            autostartManager.IsAutostartEnabled().Should().BeTrue();
        }
    }
}