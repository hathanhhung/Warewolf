﻿using System;
using System.Activities.Presentation.Model;
using System.Windows;

namespace Dev2.Studio.Interfaces
{
    public interface IConflictNode
    {
        ModelItem CurrentActivity { get; set; }
        ModelItem CurrentFlowStep { get; set; }
        bool HasConflict { get; set; }
        Point NodeLocation { get; set; }
    }
}