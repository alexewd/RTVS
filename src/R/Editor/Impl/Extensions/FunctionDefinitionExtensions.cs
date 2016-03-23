﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using Microsoft.R.Core.AST;
using Microsoft.R.Core.AST.Arguments;
using Microsoft.R.Core.AST.Expressions.Definitions;
using Microsoft.R.Core.AST.Functions.Definitions;
using Microsoft.R.Core.AST.Statements.Definitions;
using Microsoft.R.Core.AST.Variables;
using Microsoft.R.Support.Help.Definitions;
using Microsoft.R.Support.Help.Functions;
using Microsoft.VisualStudio.Text;

namespace Microsoft.R.Editor {
    public static class FunctionDefinitionExtensions {
        public static IFunctionInfo MakeFunctionInfo(this IFunctionDefinition fd, string functionName) {
            if (fd != null) {
                var fi = new FunctionInfo(functionName);
                fi.Signatures = new ISignatureInfo[] { fd.MakeSignature(functionName) };
                return fi;
            }
            return null;
        }

        /// <summary>
        /// Constructs function signature based on function name and 
        /// the function definitions found in the AST.
        /// </summary>
        public static ISignatureInfo MakeSignature(this IFunctionDefinition fd, string functionName) {
            var si = new SignatureInfo(functionName);
            si.Arguments = new List<IArgumentInfo>();
            foreach (var arg in fd.Arguments) {
                var na = arg as NamedArgument;
                if (na != null) {
                    si.Arguments.Add(new ArgumentInfo(na.Name));
                } else {
                    var ea = arg as ExpressionArgument;
                    if (ea != null && ea.Children.Count > 0) {
                        var exp = ea.Children[0] as IExpression;
                        if (exp != null && exp.Children.Count > 0) {
                            var v = exp.Children[0] as Variable;
                            if (v != null) {
                                si.Arguments.Add(new ArgumentInfo(v.Name));
                            }
                        }
                    }
                }
            }
            return si;
        }

        public static IFunctionDefinition FindFunctionDefinition(ITextBuffer textBuffer, AstRoot ast, int position, out Variable v) {
            v = null;
            // First determine if position is right before the function declaration
            var snapshot = textBuffer.CurrentSnapshot;
            ITextSnapshotLine currentLine = textBuffer.CurrentSnapshot.GetLineFromPosition(position);
            var line = FindFirstNonEmptyLine(snapshot, currentLine.LineNumber + 1);
            if (line != null) {
                var exp = ast.GetNodeOfTypeFromPosition<IExpressionStatement>(line.Start + line.Length / 2);
                if (exp != null) {
                    return exp.GetVariableOrFunctionDefinition(out v);
                }
            }
            return null;
        }

        private static ITextSnapshotLine FindFirstNonEmptyLine(ITextSnapshot snapshot, int lineNumber) {
            for (int i = lineNumber; i < snapshot.LineCount; i++) {
                var line = snapshot.GetLineFromLineNumber(i);
                if (!string.IsNullOrWhiteSpace(line.GetText())) {
                    return line;
                }
            }
            return null;
        }
    }
}
