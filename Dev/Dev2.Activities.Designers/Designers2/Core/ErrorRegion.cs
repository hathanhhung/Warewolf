﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using Dev2.Common.Interfaces.ToolBase;

namespace Dev2.Activities.Designers2.Core
{
    public class ErrorRegion : IToolRegion
    {
        public ErrorRegion()
        {
            ToolRegionName = "ErrorRegion";
            IsEnabled = true;
            Dependants = new List<IToolRegion>();
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Implementation of IToolRegion

        public string ToolRegionName { get; set; }
        public bool IsEnabled { get; set; }

        public IList<IToolRegion> Dependants { get; set; }

        public IToolRegion CloneRegion() => new ErrorRegion();

        public void RestoreRegion(IToolRegion toRestore) => throw new NotImplementedException();

        public EventHandler<List<string>> ErrorsHandler
        {
            get;
            set;
        }

        public IList<string> Errors => new List<string>();

        #endregion
    }
}