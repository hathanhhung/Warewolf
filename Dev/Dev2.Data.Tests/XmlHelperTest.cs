/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2018 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using Dev2.Data.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Dev2.Data.Tests
{
    /// <summary>
    /// Summary description for XmlHelperTest
    /// </summary>
    [TestClass]
    public class XmlHelperTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }
       
        [TestMethod]
        [Owner("Nkosinathi Sangweni")]
        public void IsXml_GivenValidXml_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------

            const string XmlFragment = @"<InnerError>Index #0
Message: Login failed for user 'testuser2'.
LineNumber: 65536
Source: .Net SqlClient Data Provider
Procedure: 
</InnerError><InnerError>ExecuteReader requires an open and available Connection. The connection's current state is closed.
   at System.Data.SqlClient.SqlCommand.ValidateCommand(String method, Boolean async)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior)
   at Dev2.Services.Sql.SqlServer.ExecuteReader[T](SqlCommand command, CommandBehavior commandBehavior, Func`2 handler) in c:\Development\Dev\Dev2.Services.Sql\SqlServer.cs:line 121
   at Dev2.Services.Sql.SqlServer.FetchDataTable(SqlParameter[] parameters) in c:\Development\Dev\Dev2.Services.Sql\SqlServer.cs:line 61
   at Dev2.Services.Execution.DatabaseServiceExecution.SqlExecution(ErrorResultTO errors, Object& executeService) in c:\Development\Dev\Dev2.Services.Execution\DatabaseServiceExecution.cs:line 118</InnerError>";
            var isXml = XmlHelper.IsXml(XmlFragment, out bool isFragment, out bool isHtml);
            //---------------Test Result -----------------------
            Assert.IsTrue(isXml);
        }

        [TestMethod]
        [Owner("Nkosinathi Sangweni")]
        public void IsXml_GivenValidIvalidXml_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------

            const string XmlFragment = @"HHHHHHH";
            var isXml = XmlHelper.IsXml(XmlFragment, out bool isFragment, out bool isHtml);
            //---------------Test Result -----------------------
            Assert.IsFalse(isXml);
        }

        [TestMethod]
        [Owner("Nkosinathi Sangweni")]
        public void ToCleanXml_GivenDirtXmlWithToStripTags_ShouldReuturnCleanXml()
        {
            //---------------Set up test pack-------------------
            var xml = @"<XmlData>Hello world<XmlData>";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var cleanXml = xml.ToCleanXml();
            //---------------Test Result -----------------------
            Assert.AreEqual("Hello world", cleanXml);
        }

        [TestMethod]
        [Owner("Nkosinathi Sangweni")]
        public void ToCleanXml_GivenDirtXmlWithnaughtyTags_ShouldReuturnNoData()
        {
            //---------------Set up test pack-------------------
            var xml = @"<WebXMLConfiguration>Hello world</WebXMLConfiguration>";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var cleanXml = xml.ToCleanXml();
            //---------------Test Result -----------------------
            Assert.AreEqual("", cleanXml);
        }

        [TestMethod]
        [Owner("Nkosinathi Sangweni")]
        public void ToCleanXml_GivenDirtXmlWithnaughtyTagsAndValid_ShouldReuturnCleanXml()
        {
            //---------------Set up test pack-------------------
            var xml = @"<Person><WebXMLConfiguration>Hello world</WebXMLConfiguration></Person>";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var cleanXml = xml.ToCleanXml();
            //---------------Test Result -----------------------
            Assert.AreEqual("<Person></Person>", cleanXml);
        }

    }
}
