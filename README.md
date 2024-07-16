# Unity Firebase Integration

This guide explains how to integrate Firebase Realtime Database functionality into a Unity WebGL game. It includes instructions for setting up necessary files and configuring your project to use Firebase for managing leaderboard data.

## File Structure

```
FirebaseScoreboard/
├── Scripts/
│   ├── FirebaseManager.cs
│   ├── UIButtonHandler.cs
│   └── JsonHelper.cs
├── WebGLTemplates/
│   └── CustomTemplate/
│       └── index.html
├── public/
│   └── firebaseConfig.json
└── Plugins/
    └── firebase.jslib
```

## File Purpose and Setup

### `FirebaseManager.cs`

**Purpose**: Manages interactions between Unity and Firebase, including initializing Firebase, saving scores, and loading single or multiple leaderboard entries.

**Setup**:

1. Place `FirebaseManager.cs` in the `Scripts` folder of your Unity project.
2. Ensure it is attached to a GameObject in your scene.

### `UIButtonHandler.cs`

**Purpose**: Handles user interactions with UI buttons to trigger Firebase operations, such as saving scores and loading leaderboard entries. Updates UI elements with the retrieved data.

**Setup**:

1. Place `UIButtonHandler.cs` in the `Scripts` folder of your Unity project.
2. Ensure it is attached to a GameObject in your scene.
3. Assign the relevant buttons and TextMeshPro elements in the Unity Inspector.

### `JsonHelper.cs`

**Purpose**: Provides methods for converting arrays to and from JSON strings using Unity's JsonUtility, which is necessary for serializing and deserializing arrays.

**Setup**:

1. Place `JsonHelper.cs` in the `Scripts` folder of your Unity project.

### `firebase.jslib`

**Purpose**: Extends Unity WebGL functionality by integrating Firebase Realtime Database operations. This JavaScript library facilitates adding, updating, and fetching leaderboard entries.

**Setup**:

1. Place `firebase.jslib` in the `Plugins` folder of your Unity project.

### `firebaseConfig.json`

**Purpose**: Stores Firebase configuration details securely, which are loaded at runtime to initialize Firebase.

**Setup**:

1. Create a `public` folder in the root of your Unity project.
2. Place `firebaseConfig.json` in the `public` folder.
3. Fill in your Firebase configuration details in the JSON file.

Example `firebaseConfig.json`:

```json
{
  "apiKey": "YOUR_API_KEY",
  "authDomain": "YOUR_AUTH_DOMAIN",
  "databaseURL": "YOUR_DATABASE_URL",
  "projectId": "YOUR_PROJECT_ID",
  "storageBucket": "YOUR_STORAGE_BUCKET",
  "messagingSenderId": "YOUR_MESSAGING_SENDER_ID",
  "appId": "YOUR_APP_ID"
}
```

### `index.html`

**Purpose**: Sets up the environment to run a Unity WebGL build with integrated Firebase functionality. Loads Firebase configuration from `firebaseConfig.json` and initializes Firebase.

**Setup**:

1. Create a `WebGLTemplates` folder in the root of your Unity project.
2. Inside `WebGLTemplates`, create a `CustomTemplate` folder.
3. Place `index.html` inside the `CustomTemplate` folder.

Ensure you use Firebase SDK version 8.10.0 for compatibility. The paths are important and should reflect the structure where the build folder is inside a `public` folder, and the output files from the build process are uncompressed.

## Integration Steps

1. **Create Folder Structure**:

   - Create the following folders in your Unity project:
     - `Scripts`
     - `WebGLTemplates/CustomTemplate`
     - `public`
     - `Plugins`

2. **Add Scripts**:

   - Place `FirebaseManager.cs`, `UIButtonHandler.cs`, and `JsonHelper.cs` in the `Scripts` folder.
   - Place `firebase.jslib` in the `Plugins` folder.

3. **Add Configuration**:

   - Create `firebaseConfig.json` in the `public` folder and fill in your Firebase configuration details.

4. **Set Up WebGL Template**:

   - Place `index.html` in the `WebGLTemplates/CustomTemplate` folder.

5. **Configure Unity**:

   - Attach `FirebaseManager` and `UIButtonHandler` scripts to appropriate GameObjects in your scene.
   - Assign buttons and TextMeshPro elements to the `UIButtonHandler` script in the Unity Inspector.

6. **Build Settings**:

   - Open `File > Build Settings` in Unity.
   - Select `WebGL` as the platform.
   - In the `Player Settings`, set the `WebGL Template` to `CustomTemplate`.

7. **Build and Run**:
   - Build and run your Unity project. Ensure the `Build` folder is inside a `public` folder if you follow the provided paths in `index.html`.

By following this guide, you will integrate Firebase functionality into your Unity WebGL game, allowing you to save and load leaderboard data securely and efficiently.
