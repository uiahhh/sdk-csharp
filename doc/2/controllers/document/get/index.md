---
code: true
type: page
title: GetAsync
description: Gets a document from kuzzle.
---

# GetAsync

Gets a document.

## Arguments

```csharp
public async Task<JObject> GetAsync(
  string index, 
  string collection, 
  string id);

```

<br/>

| Argument     | Type                                 | Description     |
| ------------ | ------------------------------------ | --------------- |
| `index`      | <pre>string</pre>        | Index name      |
| `collection` | <pre>string</pre>        | Collection name |
| `id`         | <pre>string</pre>        | Document ID     |

## Return

A JObject representing the document content.

| Property | Type              | Description      |
| -------- | ----------------- | ---------------- |
| `_id` | <pre>string</pre> | Document ID |
| `_source` | <pre>JObject</pre> | Document content |

## Exceptions

Throws a `KuzzleException` if there is an error. See how to [handle errors](/sdk/csharp/2/essentials/error-handling).

## Usage

<<< ./snippets/get.cs
