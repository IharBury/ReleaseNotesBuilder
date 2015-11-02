using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReleaseNotesBuilder.Arguments;

namespace ReleaseNotesBuilder.Tests
{
    [TestClass]
    public class ArgumentParserTests
    {
        private static readonly string[] normalArguments =
        {
            "--gn=GitHubUser",
            "--gt=GitHubToken",
            "--jn=JiraUser",
            "--jp=JiraPassword",
            "--rn=Repo",
            "--bn=Branch",
            "--tn=Tag",
            "--tp=XYZ",
            "--tpn=Template"
        };

        private Mock<IProgramConfigurer> configurationMock;
        private ArgumentParser parser;

        [TestInitialize]
        public void TestInitialize()
        {
            configurationMock = new Mock<IProgramConfigurer>
            {
                DefaultValue = DefaultValue.Mock
            };
            configurationMock
                .SetupGet(configuration => configuration.TaskReferenceExtractor.TaskPrefixes)
                .Returns(new List<string>());
            parser = new ArgumentParser(configurationMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentParsingException))]
        public void WhenNoParametersProvidedExceptionIsThrown()
        {
            parser.Parse(Enumerable.Empty<string>());
        }

        [TestMethod]
        public void WhenAllParametersAreProvidedExceptionIsNotThrown()
        {
            parser.Parse(normalArguments);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentParsingException))]
        public void WhenUniqueParameterIsProvidedTwiceExceptionIsThrown()
        {
            parser.Parse(normalArguments.Concat(new[]
            {
                "--gn=GitHubSuperUser"
            }));
        }

        [TestMethod]
        public void WhenTaskPrefixIsProvidedTwiceBothAreAddedToTheCollection()
        {
            parser.Parse(normalArguments.Concat(new[]
            {
                "--tp=ABC"
            }));

            Assert.IsTrue(configurationMock.Object.TaskReferenceExtractor.TaskPrefixes.Contains("XYZ"));
            Assert.IsTrue(configurationMock.Object.TaskReferenceExtractor.TaskPrefixes.Contains("ABC"));
        }

        [TestMethod]
        public void CommaSeparatedTaskPrefixesAreAddedToTheCollection()
        {
            parser.Parse(normalArguments
                .Except(new[]
                {
                    "--tp=XYZ"    
                })
                .Concat(new[]
                {
                    "--tp=XYZ,ABC"
                }));

            Assert.IsTrue(configurationMock.Object.TaskReferenceExtractor.TaskPrefixes.Contains("XYZ"));
            Assert.IsTrue(configurationMock.Object.TaskReferenceExtractor.TaskPrefixes.Contains("ABC"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentParsingException))]
        public void WhenUnknownParametersProvidedExceptionIsThrown()
        {
            parser.Parse(normalArguments.Concat(new[]
            {
                "extra"
            }));
        }

        [TestMethod]
        [ExpectedException(typeof(HelpRequestedException))]
        public void HelpIsSupported()
        {
            parser.Parse(new[] { "--help" });
        }
    }
}
