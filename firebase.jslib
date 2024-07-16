/**
 * Firebase Integration for Unity WebGL
 *
 * This script extends Unity WebGL functionality by integrating Firebase Realtime Database operations.
 * It enables Unity to interact with Firebase for adding, updating, and fetching leaderboard entries.
 *
 * Functionality provided by this script includes:
 * - Checking if Firebase is initialised and ready to use.
 * - Adding new entries to the Firebase database.
 * - Fetching the highest single entry (based on score) from the database.
 * - Fetching the top N entries from the database, ordered by score.
 *
 * This script uses the 'mergeInto' function provided by the Unity WebGL framework to merge the
 * defined JavaScript functions into the Unity runtime, allowing them to be called from C# scripts
 * in Unity.
 *
 * Note:
 * - Ensure that Firebase is correctly initialised in the hosting HTML page
 *           This will be the WebGLTemplates\CustomTemplate\index.html file
 *           And Ultimately the index.html file for each build.
 * - Ensure that the Unity GameObject and method names match the ones used in this script.
 */

mergeInto(LibraryManager.library, {
  // Checks if Firebase is initialised and ready
  CheckFirebaseInitialisation: function () {
    if (
      window.database &&
      window.firebaseReady &&
      typeof window.database.ref === "function"
    ) {
      return 1; // Firebase is ready
    }
    return 0; // Firebase not ready
  },

  // Adds an entry to the Firebase database
  AddEntryToDatabase: function (jsonEntryPtr) {
    var jsonEntry = UTF8ToString(jsonEntryPtr); // Convert pointer to string
    var entryObj = JSON.parse(jsonEntry); // Parse JSON string to object
    var newScoreKey = window.database.ref("leaderboardEntries").push().key; // Generate new key
    var updates = {};
    updates["/leaderboardEntries/" + newScoreKey] = entryObj;

    // Update database with new entry
    window.database
      .ref()
      .update(updates)
      .then(function () {
        console.log("Entry added or updated successfully.");
      })
      .catch(function (error) {
        console.error("Error adding or updating entry:", error);
      });
  },

  // Fetches the highest score entry from the Firebase database
  FetchSingleEntry: function () {
    window.database
      .ref("leaderboardEntries")
      .orderByChild("score") // Order entries by the 'score' field
      .limitToLast(1) // Get the entry with the highest score
      .once("value")
      .then(function (snapshot) {
        var entry = snapshot.val();

        // Extract the first entry's values
        var simplifiedEntry = Object.values(entry)[0];

        var entryJson = JSON.stringify(simplifiedEntry);

        try {
          if (window.unityInstance) {
            window.unityInstance.SendMessage(
              "FirebaseManager", // Unity GameObject name
              "OnSingleEntryLoaded", // Unity method name
              entryJson
            );
            console.log("Sent entry JSON to Unity");
          } else {
            console.error("Unity instance is not defined");
          }
        } catch (err) {
          console.error("Error sending message to Unity:", err);
        }
      })
      .catch(function (error) {
        console.error("Error fetching entry:", error);
      });
  },

  // Fetches top entries from the Firebase database
  FetchTopEntries: function (count) {
    window.database
      .ref("leaderboardEntries")
      .orderByChild("score")
      .limitToLast(count)
      .once("value")
      .then(function (snapshot) {
        var entries = [];
        snapshot.forEach(function (childSnapshot) {
          var entry = childSnapshot.val();
          entries.push(entry);
        });

        // Reverse the array to have the highest scores first
        entries.reverse();

        var entriesJson = JSON.stringify(entries);

        try {
          if (window.unityInstance) {
            window.unityInstance.SendMessage(
              "FirebaseManager", // Unity GameObject name - must be exact!
              "OnTopEntriesLoaded", // Unity method name - must be exact!
              entriesJson
            );
            console.log("Sent entries JSON to Unity");
          } else {
            console.error("Unity instance is not defined");
          }
        } catch (err) {
          console.error("Error sending message to Unity:", err);
        }
      })
      .catch(function (error) {
        console.error("Error fetching entries:", error);
      });
  },
});
