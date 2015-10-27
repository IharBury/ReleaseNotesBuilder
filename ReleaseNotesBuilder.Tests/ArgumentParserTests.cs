﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReleaseNotesBuilder.Arguments;

namespace ReleaseNotesBuilder.Tests
{
    [TestClass]
    public class ArgumentParserTests
    {
        [TestMethod]
        [ExpectedException(typeof(ParameterParsingException))]
        public void WhenNoParametersProvidedExceptionIsThrown()
        {
            var configurationMock = new Mock<IProgramConfiguration>
            {
                DefaultValue = DefaultValue.Mock
            };
            var parser = new ArgumentParser(configurationMock.Object);
            parser.Parse(Enumerable.Empty<string>());
        }
    }
}