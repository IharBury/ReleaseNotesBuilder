using System.Collections.Generic;
using System.Linq;

using Moq;

using NUnit.Framework;

using ReleaseNotesBuilder.Builders.Notes;
using ReleaseNotesBuilder.Providers.GitHub;
using ReleaseNotesBuilder.Providers.GitHub.Client;
using ReleaseNotesBuilder.Providers.GitHub.Models;

namespace ReleaseNotesBuilder.Tests
{
    public class GitHubDataProviderTests
    {
        [Test]
        public void FindCommitCallsGitHubRestClientWithCorrectLink()
        {
            var mock = new Mock<IGitHubRestClient>(MockBehavior.Strict);
            mock.Setup(x => x.OwnerName).Returns("Owner");

            var tagLinkForSHA = string.Format("repos/{0}/{1}/git/refs/tags/{2}", "Owner", "Repo", "release-0.1");

            mock.Setup(x => x.GetResponse<TagDataModel>(tagLinkForSHA)).Returns(new LinkedResponsePayload<TagDataModel>()
            {
                Data = new TagDataModel() { SHA = "release-0.1" }
            }).Verifiable();

            var tagLinkForData = string.Format("repos/{0}/{1}/git/tags/{2}", "Owner", "Repo", "release-0.1"); ;

            mock.Setup(x => x.GetResponse<TagDataModel>(tagLinkForData)).Returns(new LinkedResponsePayload<TagDataModel>()
            {
                Data = new TagDataModel() { SHA = "Some sha" }
            }).Verifiable();
            
            var commitsLink = string.Format("repos/{0}/{1}/commits?sha={2}&page={3}&per_page=100", "Owner", "Repo", "Branch", "1");
            
            mock.Setup(x => x.GetResponse<List<CommitDataModel>>(commitsLink)).Returns(new LinkedResponsePayload<List<CommitDataModel>>()
            {
                Data = Enumerable.Repeat(new CommitDataModel
                {
                    Message = "1",
                    SHA = "1"
                }, 100).ToList(),
                NextPageAvailable = false
            }).Verifiable();


            var provider = new GitHubDataProvider(mock.Object);
            provider.FindCommits("Repo", "Branch", "release-0.1");

            mock.VerifyAll();
        }
    }
}
