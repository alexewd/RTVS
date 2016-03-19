﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Diagnostics;
using Microsoft.Languages.Core.Text;
using Microsoft.Languages.Core.Tokens;
using Microsoft.R.Core.AST.DataTypes;
using Microsoft.R.Core.AST.Definitions;
using Microsoft.R.Core.AST.Operators.Definitions;
using Microsoft.R.Core.Tokens;

namespace Microsoft.R.Core.AST.Operators {
    [DebuggerDisplay("[{OperatorType} [{Start}...{End})]")]
    public abstract class Operator : RValueNode<RObject>, IOperator {
        #region IOperator
        public IRValueNode LeftOperand { get; set; }

        public virtual OperatorType OperatorType { get; protected set; }

        public IRValueNode RightOperand { get; set; }

        public int Precedence => OperatorPrecedence.GetPrecedence(OperatorType);

        public bool IsUnary { get; protected set; }

        public virtual Associativity Associativity { get; internal set; } = Associativity.Left;
        #endregion

        /// <summary>
        /// Given token stream and operator type determines if operator is unary
        /// </summary>
        /// <param name="tokens">Token stream</param>
        /// <param name="textProvider">Text provider</param>
        /// <param name="operatorType">Operator type</param>
        public static bool IsUnaryOperator(TokenStream<RToken> tokens, ITextProvider textProvider, OperatorType operatorType) {
            if (operatorType != OperatorType.Subtract && operatorType != OperatorType.Add) {
                return false;
            }

            // If operator is preceded by an operator, it is then unary
            // Look back two tokens since operator parsing already consumed its token.
            if (tokens.LookAhead(-2).TokenType == RTokenType.Operator) {
                return true;
            }

            return tokens.IsLineBreakAfter(textProvider, tokens.Position - 1);
        }
    }
}
