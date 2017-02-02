/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2017 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System.Collections.Generic;

namespace Dev2.Common.Interfaces.Diagnostics.Debug
{
    public interface IFilePath
    {
        string Title { get; set; }
    }

    public interface IDirectoryPath
    {
        string RealPath { get; set; }
        string PathToSerialize { get; set; }

        void SetRealPath();

        void SetSerializePath();
    }

    public interface IDebugProvider
    {
        IEnumerable<IDebugState> GetDebugStates(string serverWebUri, IDirectoryPath directory, IFilePath path);
    }
}