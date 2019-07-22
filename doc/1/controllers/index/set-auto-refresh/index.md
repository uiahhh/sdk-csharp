---
code: true
type: page
title: setAutoRefresh
description: Set the autorefresh flag
---

# setAutoRefresh(index, autorefresh, [options])

The setAutoRefresh action allows to set the autorefresh flag for the index.

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
Task SetAutoRefreshAsync(string index , boolean autoRefresh);
```

## Arguments

| Arguments     | Type                       | Description       | Required |
| ------------- | -------------------------- | ----------------- | -------- |
| `index`       | string                     | Index name        | yes      |
| `autoRefresh` | bool                       | autoRefresh flag  | yes      |

## Usage

<<< ./snippets/setAutoRefresh.cs