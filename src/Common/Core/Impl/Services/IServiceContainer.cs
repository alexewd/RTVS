﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Common.Core.Services {
    public interface IServiceContainer {
        /// <summary>
        /// Provides access to global application services
        /// </summary>
        T GetService<T>(Type type = null) where T : class;

        /// <summary>
        /// Enumerates all available services
        /// </summary>
        IEnumerable<Type> Services { get; }

        /// <summary>
        /// Fire when service is added
        /// </summary>
        event EventHandler<ServiceContainerEventArgs> ServiceAdded;

        /// <summary>
        /// Fires when service is removed
        /// </summary>
        event EventHandler<ServiceContainerEventArgs> ServiceRemoved;
    }
}
