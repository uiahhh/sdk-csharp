---
code: true
type: page
title: GetAllStatsAsync
description: Gets all stored internal statistics snapshots.
---

# GetAllStatsAsync

Gets all stored internal statistics snapshots.
By default, snapshots are made every 10 seconds and they are stored for 1 hour.

These statistics include:

- the number of connected users per protocol (not available for all protocols)
- the number of ongoing requests
- the number of completed requests since the last frame
- the number of failed requests since the last frame

## Arguments

```csharp
async Task<JObject> GetAllStatsAsync()
```

## Return

Returns all the stored statistics as a `JObject`.

## Usage

<<< ./snippets/getAllStats.cs
