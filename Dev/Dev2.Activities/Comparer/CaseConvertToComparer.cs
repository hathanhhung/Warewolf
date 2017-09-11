﻿using System.Collections.Generic;
using Dev2.Common.Interfaces.Core.Convertors.Case;

namespace Dev2.Comparer
{
    public class CaseConvertToComparer:IEqualityComparer<ICaseConvertTO>
    {
        public bool Equals(ICaseConvertTO x, ICaseConvertTO y)
        {
            if (x == null && y == null) return true;
            return x != null && y != null && x.Equals(y);
        }

        public int GetHashCode(ICaseConvertTO obj)
        {
            return obj.GetHashCode();
        }
    }
}