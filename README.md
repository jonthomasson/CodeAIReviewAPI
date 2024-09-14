# Code Review API

## Overview
The Code Review API is a .NET Core web service that integrates with Firebase for authentication, GitHub for pull request (PR) information, and Azure's ChatGPT SDK for providing AI-generated code review suggestions. This API is designed to securely process code changes in pull requests, analyze them, and return suggestions for improvement.

## Features
- **Firebase Authentication**: Uses Firebase to authenticate users, ensuring only authorized users can access the API.
- **GitHub Integration**: Retrieves pull request file changes from GitHub repositories using the GitHub API.
- **AI Code Review**: Sends the file changes to Azure's ChatGPT SDK to analyze code modifications and return suggestions.

## Prerequisites
Before running this API, ensure the following are set up:
- [.NET 6+](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Firebase Project](https://firebase.google.com/) (for user authentication)
- [GitHub Account & OAuth App](https://docs.github.com/en/developers/apps/building-oauth-apps/creating-an-oauth-app)
- [Azure Subscription with ChatGPT SDK access](https://azure.microsoft.com/en-us/services/cognitive-services/openai/)

## Installation

1. Clone the repository:

    ```bash
    git clone https://github.com/yourusername/code-review-api.git
    cd code-review-api
    ```

2. Install required dependencies:

    ```bash
    dotnet restore
    ```

3. Set up your environment variables in `appsettings.json`:
   
    ```json
    {
      "FirebaseProjectName": "your-firebase-project-id",
      "AzureChatGPTEndpoint": "https://your-chatgpt-endpoint/",
      "AzureChatGPTApiKey": "your-azure-api-key"
    }
    ```

4. Run the API:

    ```bash
    dotnet run
    ```

## Endpoints

### `POST /api/review`
Analyze file changes in a pull request and provide AI-based suggestions.

- **URL**: `/api/review`
- **Method**: `POST`
- **Headers**:
  - `Authorization: Bearer {firebaseToken}` – A Firebase user token is required for authentication.
  
- **Request Body**:
  - `fileChanges`: An array of objects containing `fileName` and `patch` fields.
  
  Example:
  ```json
  {
    "fileChanges": [
      {
        "fileName": "Services/Service1.cs",
        "patch": "@@ -1,12 +1,7 @@\n- using System;\n+ namespace Services"
      }
    ]
  }
