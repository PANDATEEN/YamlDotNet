﻿//  This file is part of YamlDotNet - A .NET library for YAML.
//  Copyright (c) Antoine Aubry and contributors

//  Permission is hereby granted, free of charge, to any person obtaining a copy of
//  this software and associated documentation files (the "Software"), to deal in
//  the Software without restriction, including without limitation the rights to
//  use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
//  of the Software, and to permit persons to whom the Software is furnished to do
//  so, subject to the following conditions:

//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.

//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.

using System;
using System.Collections.Generic;
using YamlDotNet.Core;

namespace YamlDotNet.Representation.Schemas
{
    /// <summary>
    /// Defines .NET-specific tags for commonly used types.
    /// </summary>
    public static class DotNetSchema
    {
        public static readonly ContextFreeSchema Instance = new ContextFreeSchema(CreateMatchers());

        private static IEnumerable<NodeMatcher> CreateMatchers()
        {
            yield return NodeMatcher
                .ForScalars(
                    NodeMapper.CreateScalarMapper(
                        YamlTagRepository.Binary,
                        s => Convert.FromBase64String(s.Expect<Scalar>().Value),
                        val => Convert.ToBase64String((byte[])val!)
                    )
                )
                .MatchPattern(@"^[ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789\+/\r\n\s]*(=|==)?$")
                .MatchEmptyTags()
                .MatchTypes(typeof(byte[]))
                .Create();

            yield return NodeMatcher
                .ForScalars(
                    NodeMapper.CreateScalarMapper(
                        YamlTagRepository.Binary,
                        s => TimestampParser.Parse(s.Value),
                        val => ((DateTimeOffset)val!).ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFFK")
                    )
                )
                .MatchPattern(TimestampParser.TimestampPattern)
                .MatchEmptyTags()
                .MatchTypes(typeof(DateTimeOffset))
                .Create();
        }
    }
}