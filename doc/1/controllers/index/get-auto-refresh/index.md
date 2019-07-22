---
code: true
type: page
title: getAutoRefresh
description: Returns the status of autorefresh flag
---

# GetAutoRefresh

The getAutoRefresh action returns the current autorefresh status for the index.

Each index has an autorefresh flag.  
When set to true, each write request trigger a [refresh](https://www.elastic.co/guide/en/elasticsearch/reference/current/docs-refresh.html) action on Elasticsearch.  
Without a refresh after a write request, the documents may not be immediately visible in search.

:::info
A refresh operation comes with some performance costs.  
While forcing the autoRefresh can be convenient on a development or test environment,  
we recommend that you avoid using it in production or at least carefully monitor its implications before using it.
:::

## Signature

```cs
Task<bool> GetAutoRefreshAsync(string index);
```

## Arguments

| Arguments | Type                       | Description       | Required |
| --------- | -------------------------- | ----------------- | -------- |
| `index`   | string                     | Index name        | yes      |

## Return

Returns a `bool` that indicate the status of the **autoRefresh** flag.

## Usage

<<< ./snippets/getAutoRefresh.cs