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
        [TestMethod]
        [ExpectedException(typeof(ParameterParsingException))]
        public void WhenNoParametersProvidedExceptionIsThrown()
        {
            var configurationMock = new Mock<IProgramConfiguration>();
            var parser = new ArgumentParser(configurationMock.Object);
            parser.Parse(Enumerable.Empty<string>());
        }

        [TestMethod]
        public void WhenAllParametersAreProvidedExceptionIsNotThrown()
        {
            var configurationMock = new Mock<IProgramConfiguration>
            {
                DefaultValue = DefaultValue.Mock
            };
            var parser = new ArgumentParser(configurationMock.Object);
            parser.Parse(new[]
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
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ParameterParsingException))]
        public void WhenUniqueParameterIsProvidedTwiceExceptionIsThrown()
        {
            var configurationMock = new Mock<IProgramConfiguration>
            {
                DefaultValue = DefaultValue.Mock
            };
            var parser = new ArgumentParser(configurationMock.Object);
            parser.Parse(new[]
            {
                "--gn=GitHubUser",
                "--gn=GitHubSuperUser",
                "--gt=GitHubToken",
                "--jn=JiraUser",
                "--jp=JiraPassword",
                "--rn=Repo",
                "--bn=Branch",
                "--tn=Tag",
                "--tp=XYZ",
                "--tpn=Template"
            });
        }

        [TestMethod]
        public void WhenTaskPrefixIsProvidedTwiceBothAreAddedToTheCollection()
        {
            var configurationMock = new Mock<IProgramConfiguration>
            {
                DefaultValue = DefaultValue.Mock
            };
            configurationMock.SetupGet(configuration => configuration.NoteCollector.TaskPrefixes).Returns(new List<string>());
            var parser = new ArgumentParser(configurationMock.Object);
            parser.Parse(new[]
            {
                "--gn=GitHubUser",
                "--gt=GitHubToken",
                "--jn=JiraUser",
                "--jp=JiraPassword",
                "--rn=Repo",
                "--bn=Branch",
                "--tn=Tag",
                "--tp=XYZ",
                "--tp=ABC",
                "--tpn=Template"
            });

            Assert.IsTrue(configurationMock.Object.NoteCollector.TaskPrefixes.Contains("XYZ"));
            Assert.IsTrue(configurationMock.Object.NoteCollector.TaskPrefixes.Contains("ABC"));
        }

        [TestMethod]
        public void CommaSeparatedTaskPrefixesAreAddedToTheCollection()
        {
            var configurationMock = new Mock<IProgramConfiguration>
            {
                DefaultValue = DefaultValue.Mock
            };
            configurationMock.SetupGet(configuration => configuration.NoteCollector.TaskPrefixes).Returns(new List<string>());
            var parser = new ArgumentParser(configurationMock.Object);
            parser.Parse(new[]
            {
                "--gn=GitHubUser",
                "--gt=GitHubToken",
                "--jn=JiraUser",
                "--jp=JiraPassword",
                "--rn=Repo",
                "--bn=Branch",
                "--tn=Tag",
                "--tp=XYZ,ABC",
                "--tpn=Template"
            });

            Assert.IsTrue(configurationMock.Object.NoteCollector.TaskPrefixes.Contains("XYZ"));
            Assert.IsTrue(configurationMock.Object.NoteCollector.TaskPrefixes.Contains("ABC"));
        }
    }
}
