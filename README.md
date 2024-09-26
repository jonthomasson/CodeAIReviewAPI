# Code Review API

## Overview
The Code Review API is a .NET Core web service that integrates with Firebase for authentication, and Azure's OpenAI SDK for providing AI-generated code review suggestions. This API is designed to securely process code changes in pull requests, analyze them, and return suggestions for improvement.

## Features
- **Firebase Authentication**: Uses Firebase to authenticate users, ensuring only authorized users can access the API.
- **AI Code Review**: Sends the file changes to Azure's ChatGPT SDK to analyze code modifications and return suggestions.

## Prerequisites
Before running this API, ensure the following are set up:
- [.NET 8+](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Firebase Project](https://firebase.google.com/) (for user authentication)
- [Azure Subscription with ChatGPT SDK access](https://azure.microsoft.com/en-us/services/cognitive-services/openai/)

## Installation

1. Set up your environment variables in `appsettings.json`:
   
    ```json
    {
      "FirebaseProjectName": "your-firebase-project-id",
      "AzureAI": {
            "Endpoint": "your-azure-ai-endpoint",
            "Key": "your-azure-ai-key",
      }
    }
    ```

2. Run the API:

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
