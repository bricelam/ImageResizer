//------------------------------------------------------------------------------
// <copyright file="TemplatingEngine.cs" company="Brice Lambson">
//     Copyright (c) 2013 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Helpers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text.RegularExpressions;

    internal static class TemplatingEngine
    {
        public static string ReplaceTokens(string template, IDictionary<string, string> tokens)
        {
            Debug.Assert(template != null);
            Debug.Assert(tokens != null);

            return Regex.Replace(
                template,
                @"\$(?<tokenName>\w+)\$",
                match =>
                {
                    var tokenName = match.Groups["tokenName"].Value;
                    string value;

                    return tokens.TryGetValue(tokenName, out value)
                        ? value
                        : null;
                });
        }
    }
}
