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

// ReSharper disable once CheckNamespace
namespace Dev2.Studio.Diagnostics
{
    public class DebugLineGroupRow
    {
        public DebugLineGroupRow()
        {
            LineItems = new List<IDebugLineItem>();
        }

        public List<IDebugLineItem> LineItems { get; private set; }
    }
}