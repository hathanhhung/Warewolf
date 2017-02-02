/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2017 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using Dev2.Threading;

namespace Dev2.Activities.Designers2.SqlServerDatabase
{
    public partial class SqlServerDatabaseDesigner
    {
        public SqlServerDatabaseDesigner()
        {
            InitializeComponent();
        }

        protected override SqlServerDatabaseDesignerViewModel CreateViewModel()
        {
            return new SqlServerDatabaseDesignerViewModel(ModelItem,new AsyncWorker());
        }

    }
}
    