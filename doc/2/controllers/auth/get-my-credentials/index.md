---
code: true
type: page
title: GetMyCredentialsAsync
description: Returns the current user's credential information for the specified strategy.
---

# GetMyCredentialsAsync

Returns the current user's credential information for the specified strategy. The data returned will depend on the specified strategy. The result can be an empty string.

## Arguments

```csharp
public async Task<JObject> GetMyCredentialsAsync(string strategy);
```

| Argument   | Type              | Description     |
|------------|-------------------|-----------------|
| `strategy` | <pre>string</pre> | Strategy to use |

## Return

Returns a JObject representing the credentials for the provided authentication strategy.

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle error](/sdk/csharp/2/essentials/error-handling).

## Usage

<<< ./snippets/get-my-credentials.cs
