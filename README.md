# Blog Engine

This little application uses .Net Core 6 to implement Web Api to provide the basic functionality of a blog engime the have 3 roles: Writer, Editor and Public. Based on the user role the following actiosn are implemented:

The API should allow the following operations for the specified roles:

- Retrieve a list of all published posts (all roles)
- Add comments to a published post (all roles)
- Get own posts, create and edit posts (Writer)
  - Writers should be able to create new posts and retrieve the posts they have written.
  - Writers should be able to edit existing posts that haven't been published or submitted.
- Submit posts (Writer)
  - When a Writer submits a post, the post should move to a “pending approval” status where it’s locked and cannot be updated.
- Get, Approve or Reject pending posts (Editor)
  - Editors should be able to query for “pending” posts and approve or reject their publishing. Once an Editor approves a post, it will be published and visible to all roles. If the post is rejected, it will be unlocked and editable again by Writers.
  - Editor should be able to include a comment when rejecting a post, and this comment should be visible to the Writer only.

The UI interface are build in .Net Core 6 also as Web Application project using Razor Pages in C# and consuming the endpoints in the API.

## Installation

In order to run the application from the code is needed to have the .Net Core 6 SDK and Sql Server from version 2014 or more. The database can be recreated from the included Sql script or executing the internal database migrations that exists in the project. The database needs to be named **BlogEngine** and the database credentials are located in the **appsettings.json** file of the **WebApi** project.

With the current configuration the Web Api and Web App projects can be executed with the .Net Core 6 SDK commands, **dotnet run** from every folder. 

## Application Users

The application users are the following list and the password for every account are **String123456!**:

- admin@blog.net 
- writer@blog.net
- editor@blog.net
- public@blog.net
