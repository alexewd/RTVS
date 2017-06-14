﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.R.Core.AST;

namespace Microsoft.Languages.Editor.Completions {
    public interface IRIntellisenseContext: IIntellisenseContext {
        AstRoot AstRoot { get; }
        bool AutoShownCompletion { get; }
        bool InternalFunctions { get; set; }
     }
}