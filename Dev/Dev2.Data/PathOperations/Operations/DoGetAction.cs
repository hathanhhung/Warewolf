﻿using Dev2.Common.Interfaces.Wrappers;
using Dev2.Data.Interfaces;
using System;
using System.IO;
using Warewolf.Resource.Errors;
using Dev2.Common.Wrappers;
using Dev2.PathOperations;
using System.Security.Principal;
using Dev2.Common;

namespace Dev2.Data.PathOperations.Operations
{
    public class DoGetAction : PerformStreamIOOperation
    {
        protected readonly IDev2LogonProvider _logOnProvider;
        protected readonly IActivityIOPath _path;
        protected readonly IFile _fileWrapper;
        protected readonly SafeTokenHandle _safeToken;

        public DoGetAction(IActivityIOPath path)
        {
            _logOnProvider = new LogonProvider();
            _fileWrapper = new FileWrapper();
            _safeToken = RequiresAuth(path, _logOnProvider);
            _path = path;
        }
        public override Stream GetOperation()
        {
            if (_safeToken != null)
            {
                return GetOperationWithAuth();
            }
            if (_fileWrapper.Exists(_path.Path))
            {
                return new MemoryStream(_fileWrapper.ReadAllBytes(_path.Path));
            }
            throw new Exception(string.Format(ErrorResource.FileNotFound, _path.Path));
        }

        public override Stream GetOperationWithAuth()
        {
            WindowsImpersonationContext impersonatedUser = null;
            try
            {
                using (_safeToken)
                {
                    var newID = new WindowsIdentity(_safeToken.DangerousGetHandle());
                    using (impersonatedUser = newID.Impersonate())
                    {
                        return new MemoryStream(_fileWrapper.ReadAllBytes(_path.Path));
                    }
                }
            }
            catch (Exception exception)
            {
                Dev2Logger.Error(exception.Message, GlobalConstants.WarewolfError);
                throw new Exception(exception.Message, exception);
            }
            finally
            {
                impersonatedUser?.Undo();
            }
        }
    }
}