/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2018 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.IO;
using Dev2.Common;
using Dev2.Common.Interfaces.Diagnostics.Debug;
using Dev2.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dev2.Tests.Diagnostics
{
    
    [TestClass]
    public class DebugItemTests
    {
        const string LongText = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua."
            + "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. "
            + "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. "
            + "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

        #region Constructor

        [TestMethod]
        public void Constructor_With_Array_Expected_InitializesWithArray()
        {
            var result = new DebugItemResult { GroupName = "Hello", Value = "world" };
            var item = new DebugItem();
            item.Add(result);
            Assert.AreEqual(1, item.FetchResultsList().Count);
            Assert.AreSame(result, item.FetchResultsList()[0]);
        }

        #endregion

        #region Contains

        [TestMethod]

        public void Contains_With_NullFilter_Expected_ReturnsInstance()
        {
            var item = new DebugItem();
            item.Add(new DebugItemResult { GroupName = "Hello", Value = "world" });
            var result = item.Contains(null);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Contains_With_EmptyFilter_Expected_ReturnsInstance()
        {
            var item = new DebugItem();
            item.Add(new DebugItemResult { GroupName = "Hello", Value = "world" });
            var result = item.Contains(string.Empty);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Contains_With_ValidFilter_Expected_ReturnsInstance()
        {
            var item = new DebugItem();
            item.Add(new DebugItemResult { GroupName = "Hello", Value = "world" });
            var result = item.Contains("world");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Contains_With_InvalidFilter_Expected_ReturnsInstance()
        {
            var item = new DebugItem();
            item.Add(new DebugItemResult { GroupName = "Hello", Value = "world" });
            var result = item.Contains("the");
            Assert.IsFalse(result);
        }


        [TestMethod]
        public void Contains_With_Filter_Expected_IsCaseInsensitive()
        {
            var item = new DebugItem();
            item.Add(new DebugItemResult { GroupName = "Hello", Value = "world" });
            var result = item.Contains("hel");
            Assert.IsTrue(result);
        }

        #endregion

        #region TryCache

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]

        public void TryCache_With_NullParameters_Expected_ThrowsArgumentNullException()

        {
            var debugState = new DebugItem();
            debugState.TryCache(null);
        }

        [TestMethod]

        public void TryCache_With_ValueGreaterThanMaxCharDispatchCount_Expected_TruncatesValueToActCharDispatchCount()

        {
            var item = CreateDebugItemWithLongValue();

            var debugState = new DebugItem();
            debugState.TryCache(item);

            Assert.AreEqual(DebugItem.ActCharDispatchCount, item.Value.Length);
        }

        [TestMethod]

        public void TryCache_With_ValueGreaterThanMaxCharDispatchCount_Expected_InvokesSaveFileWithFullContent()

        {
            var item = CreateDebugItemWithLongValue();

            var expectedContents = item.Value;

            var debugItem = new DebugItemMock();
            debugItem.TryCache(item);

            Assert.AreEqual(1, debugItem.SaveFileHitCount);
            Assert.AreEqual(expectedContents, debugItem.SaveFileContents);
        }

        [TestMethod]

        public void TryCache_With_ValueEqualToMaxCharDispatchCount_Expected_DoesNotTruncateValueToActCharDispatchCount()

        {
            var item = CreateDebugItemWithLongValue();
            item.Value = item.Value.Substring(0, DebugItem.MaxCharDispatchCount);

            var debugItem = new DebugItemMock();
            debugItem.TryCache(item);

            Assert.AreEqual(DebugItem.MaxCharDispatchCount, item.Value.Length);
        }

        [TestMethod]

        public void TryCache_With_ValueEqualToMaxCharDispatchCount_Expected_DoesNotInvokeSaveFile()

        {
            var item = CreateDebugItemWithLongValue();
            item.Value = item.Value.Substring(0, DebugItem.MaxCharDispatchCount);

            var debugState = new DebugItemMock();
            debugState.TryCache(item);

            Assert.AreEqual(0, debugState.SaveFileHitCount);
        }

        [TestMethod]

        public void TryCache_With_ValueLessThanMaxCharDispatchCount_Expected_DoesNotTruncateValueToActCharDispatchCount()

        {
            const int ExpectedLength = 100;
            var item = CreateDebugItemWithLongValue();
            item.Value = item.Value.Substring(0, ExpectedLength);

            var debugState = new DebugItemMock();
            debugState.TryCache(item);

            Assert.AreEqual(ExpectedLength, item.Value.Length);
        }

        [TestMethod]

        public void TryCache_With_ValueLessThanMaxCharDispatchCount_Expected_DoesNotInvokeSaveFile()

        {
            const int ExpectedLength = 100;
            var item = CreateDebugItemWithLongValue();
            item.Value = item.Value.Substring(0, ExpectedLength);

            var debugState = new DebugItemMock();
            debugState.TryCache(item);

            Assert.AreEqual(0, debugState.SaveFileHitCount);
        }
        #endregion

        #region SaveFile

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]

        public void SaveFile_With_NullParameters_Expected_ThrowsArgumentNullException()

        {
            var debugState = new DebugItem();

            debugState.SaveFile(null, null);
        }

        [TestMethod]

        public void SaveFile_With_Contents_Expected_SavesFileToDisk()

        {
            var debugState = new DebugItem();

            debugState.ClearFile("TestFile.txt");
            EnvironmentVariables.WebServerUri = "http://localhost:3142";
            var uri = debugState.SaveFile(LongText, "TestFile.txt");
            var path = new Uri(uri).OriginalString.Replace("?DebugItemFilePath=", "").Replace(EnvironmentVariables.WebServerUri + "/Services/FetchDebugItemFileService", "");
            var exists = File.Exists(path);
            Assert.IsTrue(exists);

            var contents = File.ReadAllText(path);
            Assert.AreEqual(LongText, contents);
        }

        [TestMethod]
        [Owner("Massimo Guerrera")]
        [TestCategory("DebugIem_SaveFile")]

        public void DebugIem_SaveFile_WithContentsNewLineChars_ExpectedSavesFileToDiskWithCorrectChars()

        {
            var debugState = new DebugItem();

            debugState.ClearFile("TestFile.txt");
            EnvironmentVariables.WebServerUri = "http://localhost:3142";
            const string expeced = "\r\nThis is\r\n the text\\n that we are writing";
            const string textToWrite = "\nThis is\r\n the text\\n that we are writing";

            var uri = debugState.SaveFile(textToWrite, "TestFile.txt");
            var path = new Uri(uri).OriginalString.Replace("?DebugItemFilePath=", "").Replace(EnvironmentVariables.WebServerUri + "/Services/FetchDebugItemFileService", "");
            var exists = File.Exists(path);
            Assert.IsTrue(exists);

            var contents = File.ReadAllText(path);
            Assert.AreEqual(expeced, contents);
        }

        #endregion

        #region CreateDebugItemWithLongValue

        static DebugItemResult CreateDebugItemWithLongValue()
        {
            return new DebugItemResult { Type = DebugItemResultType.Value, Value = LongText };
        }

        #endregion

    }
}
