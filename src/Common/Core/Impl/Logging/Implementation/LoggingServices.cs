﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel.Composition;
using System.Threading;
using Microsoft.Common.Core.Shell;

namespace Microsoft.Common.Core.Logging.Implementation {
    [Export(typeof(ILoggingServices))]
    internal sealed class LoggingServices : ILoggingServices {
        private static Logger _instance;
        private readonly IApplicationConstants _appConstants;

        [ImportingConstructor]
        public LoggingServices(ILoggingPermissions permissions, IApplicationConstants appConstants) {
            Permissions = permissions;
            _appConstants = appConstants;
        }

        public ILoggingPermissions Permissions { get; }

        public IActionLog GetOrCreateLog(string appName) {
            if (_instance == null) {
                var instance = new Logger(_appConstants.ApplicationName, Permissions, writer: null);
                Interlocked.Exchange(ref _instance, instance);
            }
            return _instance;
        }

        public void Dispose() {
            _instance?.Dispose();
        }
    }
}
