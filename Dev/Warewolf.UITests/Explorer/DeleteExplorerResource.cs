﻿using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable InconsistentNaming

namespace Warewolf.UITests
{
    [CodedUITest]
    public class DeleteExplorerResource
    {
        const string flowSwitch = "DeleteExplorerResource_FileToDelete";
        const string flowSequence = "DeleteExplorerResource_FileToDelete2";

        [TestMethod]
        [TestCategory("Explorer")]
        public void Delete_ExplorerResource()
        {            
            UIMap.Filter_Explorer(flowSwitch);
            UIMap.Delete_FirstResource_FromContextMenu();
            UIMap.Click_MessageBox_Yes();
            UIMap.WaitForSpinner(UIMap.MainStudioWindow.DockManager.SplitPaneLeft.Explorer.Spinner);
            UIMap.Click_Explorer_Refresh_Button();
        }

        [TestMethod]
        [TestCategory("Explorer")]
        public void DeletedResourceIsRemovedFromResources()
        {
            var resourcesFolder = Environment.ExpandEnvironmentVariables("%programdata%") + @"\Warewolf\Resources";
            Assert.IsTrue(Directory.Exists(resourcesFolder));
            UIMap.Filter_Explorer(flowSequence);
            UIMap.Delete_FirstResource_FromContextMenu();
            UIMap.Click_MessageBox_Yes();
            UIMap.WaitForSpinner(UIMap.MainStudioWindow.DockManager.SplitPaneLeft.Explorer.Spinner);
            var allFiles = Directory.GetFiles(resourcesFolder, "*.xml", SearchOption.AllDirectories);
            var firstOrDefault = allFiles.FirstOrDefault(s => s.StartsWith(flowSequence));
            Assert.IsNull(firstOrDefault);
        }

        #region Additional test attributes

        [TestInitialize]
        public void MyTestInitialize()
        {
            UIMap.SetPlaybackSettings();
            UIMap.CloseHangingDialogs();
        }

        UIMap UIMap
        {
            get
            {
                if (_UIMap == null)
                {
                    _UIMap = new UIMap();
                }

                return _UIMap;
            }
        }

        private UIMap _UIMap;

        #endregion
    }
}