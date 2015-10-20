# Release Notes Builder

## The ReleaseNotesBuilder performs three steps to collect information and create Release Notes:
* It collects all the commits from a Github branch, starting from a specified tag, filtering the commit messages on Task Prefix (e.g. SHD).
* It will then create a collection with all the Task Identifiers (e.g. SHD-100) and query Jira api for Stories with the Task Identifiers. The data that is collected
contains the summary for the Jira task and a special field specifically for Release Notes, filled by for instance the Product Owner
* Finally, the collected task release notes are parsed into a template and outputted to screen or filesystem

## Prerequisites for running the ReleaseNotesBuilder:
* Github credentials / oauth token
* Repository name
* Branch name
* Tag name to start grabbing commits from
* Commit Task Prefix which matches Jira Task Prefix
* Jira credentials

## TODO:
* Test coverage for existing functionality
* Add release version name at the top of the release notes
* Create webbased ui to be able to generate release notes for any customer / version
(create customer dropdown / branch dropdown / tag dropdown -> generate release notes)