//------------------------------------------------------------------------------
// <copyright file="TemplatingEngineTests.cs" company="Brice Lambson">
//     Copyright (c) 2013 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Helpers
{
    using System.Collections.Generic;
    using Xunit;

    public class TemplatingEngineTests
    {
        [Fact]
        public void ReplaceTokens_replaces_tokens()
        {
            var template = "$Token2$ $Token1$";
            var tokens = new Dictionary<string, string>
                {
                    { "Token1", "Value1" },
                    { "Token2", "Value2" }
                };

            var result = TemplatingEngine.ReplaceTokens(template, tokens);

            Assert.Equal("Value2 Value1", result);
        }

        [Fact]
        public void ReplaceTokens_ignores_extra_tokens()
        {
            var template = "$Token1$";
            var tokens = new Dictionary<string, string>
                {
                    { "Token1", "Value1" },
                    { "Token2", "Value2" }
                };

            var result = TemplatingEngine.ReplaceTokens(template, tokens);

            Assert.Equal("Value1", result);
        }

        [Fact]
        public void ReplaceTokens_handles_missing_tokens()
        {
            var template = "$Token2$";
            var tokens = new Dictionary<string, string>
                {
                    { "Token1", "Value1" }
                };

            var result = TemplatingEngine.ReplaceTokens(template, tokens);

            Assert.Equal("", result);
        }
    }
}
