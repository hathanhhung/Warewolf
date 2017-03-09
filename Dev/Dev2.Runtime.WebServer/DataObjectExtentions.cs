﻿using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using Dev2.Common;
using Dev2.Common.Interfaces.Data;
using Dev2.Interfaces;
using Dev2.Runtime.Interfaces;
using Dev2.Runtime.WebServer.TransferObjects;
using Dev2.Services.Security;
using Dev2.Web;

namespace Dev2.Runtime.WebServer
{
    internal static class DataObjectExtentions
    {
        public static string SetEmitionType(this IDSFDataObject dataObject, string serviceName, NameValueCollection headers)
        {
            int loc;
            if (!string.IsNullOrEmpty(serviceName) && (loc = serviceName.LastIndexOf(".", StringComparison.Ordinal)) > 0)
            {
                // default it to xml
                dataObject.ReturnType = EmitionTypes.XML;

                if (loc > 0)
                {
                    var typeOf = serviceName.Substring(loc + 1).ToUpper();
                    EmitionTypes myType;
                    if (Enum.TryParse(typeOf, out myType))
                    {
                        dataObject.ReturnType = myType;
                    }

                    if (typeOf.StartsWith("tests", StringComparison.InvariantCultureIgnoreCase))
                    {
                        dataObject.IsServiceTestExecution = true;
                        var idx = serviceName.LastIndexOf("/", StringComparison.InvariantCultureIgnoreCase);
                        if (idx > loc)
                        {
                            var testName = serviceName.Substring(idx + 1).ToUpper();
                            dataObject.TestName = string.IsNullOrEmpty(testName) ? "*" : testName;
                        }
                        else
                        {
                            dataObject.TestName = "*";
                        }
                        dataObject.ReturnType = EmitionTypes.TEST;
                    }

                    if (typeOf.Equals("api", StringComparison.OrdinalIgnoreCase))
                    {
                        dataObject.ReturnType = EmitionTypes.SWAGGER;
                    }
                    serviceName = serviceName.Substring(0, loc);
                    dataObject.ServiceName = serviceName;
                }
            }
            else
            {
                dataObject.SetContentType(headers);
            }
            return serviceName;
        }

        public static void SetContentType(this IDSFDataObject dataObject, NameValueCollection headers)
        {
            if (headers != null)
            {
                var contentType = headers.Get("Content-Type");
                if (string.IsNullOrEmpty(contentType))
                {
                    contentType = headers.Get("ContentType");
                }
                if (!string.IsNullOrEmpty(contentType) && !dataObject.IsServiceTestExecution)
                {
                    if (contentType.ToLowerInvariant().Contains("json"))
                    {
                        dataObject.ReturnType = EmitionTypes.JSON;
                    }
                    if (contentType.ToLowerInvariant().Contains("xml"))
                    {
                        dataObject.ReturnType = EmitionTypes.XML;
                    }
                }
            }
            else
            {
                dataObject.ReturnType = EmitionTypes.XML;
            }
        }

        public static void SetupForWebDebug(this IDSFDataObject dataObject, WebRequestTO webRequest)
        {
            var contains = webRequest?.Variables?.AllKeys.Contains("IsDebug");
            if (contains != null && contains.Value)
            {
                dataObject.IsDebug = true;
                dataObject.IsDebugFromWeb = true;
                dataObject.ClientID = Guid.NewGuid();
                dataObject.DebugSessionID = Guid.NewGuid();
            }
        }

        public static void SetupForRemoteInvoke(this IDSFDataObject dataObject, NameValueCollection headers)
        {
            Dev2Logger.Debug("Remote Invoke");
            if (headers != null)
            {
                var isRemote = headers.Get(HttpRequestHeader.Cookie.ToString());
                var remoteId = headers.Get(HttpRequestHeader.From.ToString());

                if (isRemote != null && remoteId != null)
                {
                    if (isRemote.Equals(GlobalConstants.RemoteServerInvoke))
                    {
                        // we have a remote invoke ;)
                        dataObject.RemoteInvoke = true;
                    }
                    if (isRemote.Equals(GlobalConstants.RemoteDebugServerInvoke))
                    {
                        // we have a remote invoke ;)
                        dataObject.RemoteNonDebugInvoke = true;
                    }

                    dataObject.RemoteInvokerID = remoteId;
                }
            }
        }

        public static void SetTestResourceIds(this IDSFDataObject dataObject, IResourceCatalog catalog, WebRequestTO webRequest, string serviceName)
        {
            if (webRequest.IsRunAllTestsRequest(serviceName))
            {
                var pathOfAllResources = webRequest.GetPathForAllResources();
                dataObject.ResourceID = Guid.Empty;
                if (string.IsNullOrEmpty(pathOfAllResources))
                {
                    var resources = catalog.GetResources(GlobalConstants.ServerWorkspaceID);
                    dataObject.TestsResourceIds = resources.Select(p => p.ResourceID).ToList();
                }
                else
                {
                    var resources = catalog.GetResources(GlobalConstants.ServerWorkspaceID);
                    var resourcesToRunTestsFor = resources?.Where(a => a.GetResourcePath(GlobalConstants.ServerWorkspaceID)
                                                   .StartsWith(pathOfAllResources, StringComparison.InvariantCultureIgnoreCase));
                    dataObject.TestsResourceIds = resourcesToRunTestsFor?.Select(p => p.ResourceID).ToList();
                }
            }
        }

        public static void SetupForTestExecution(this IDSFDataObject dataObject, WebRequestTO requestTO, string serviceName, NameValueCollection headers)
        {
            if (requestTO.IsRunAllTestsRequest(serviceName))
            {
                dataObject.ReturnType = EmitionTypes.TEST;
                dataObject.IsServiceTestExecution = true;
                dataObject.TestName = "*";
            }
            else
            {
                dataObject.SetContentType(headers);
            }
        }

        public static void SetResourceNameAndId(this IDSFDataObject dataObject, IResourceCatalog catalog, string serviceName, out IResource resource)
        {
            Guid resourceID;
            if (Guid.TryParse(serviceName, out resourceID))
            {
                resource = catalog.GetResource(dataObject.WorkspaceID, resourceID);
                if (resource != null)
                {
                    dataObject.ServiceName = resource.ResourceName;
                    dataObject.ResourceID = resource.ResourceID;
                    dataObject.SourceResourceID = resource.ResourceID;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(dataObject.ServiceName))
                {
                    resource = catalog.GetResource(dataObject.WorkspaceID, dataObject.ServiceName);
                    if (resource != null)
                    {
                        dataObject.ResourceID = resource.ResourceID;
                        dataObject.SourceResourceID = resource.ResourceID;
                    }
                }
            }
            resource = null;
        }

        public static bool CanExecuteCurrentResource(this IDSFDataObject dataObject, IResource resource, IAuthorizationService service)
        {
            if (service != null && dataObject.ReturnType != EmitionTypes.TEST)
            {
                var hasView = service.IsAuthorized(AuthorizationContext.View, dataObject.ResourceID.ToString());
                var hasExecute = service.IsAuthorized(AuthorizationContext.Execute, dataObject.ResourceID.ToString());
                return (hasExecute && hasView) || ((dataObject.RemoteInvoke || dataObject.RemoteNonDebugInvoke) && hasExecute) || (resource != null && resource.ResourceType == "ReservedService");
            }
            return false;
        }

    }
}
