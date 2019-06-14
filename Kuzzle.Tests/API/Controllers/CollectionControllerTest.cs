﻿using System;
using System.Collections.Generic;
using KuzzleSdk.API.Controllers;
using KuzzleSdk.API.DataObjects;
using KuzzleSdk.API.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Kuzzle.Tests.API.Controllers {
  public class CollectionControllerTest {
    private readonly CollectionController _collectionController;
    private readonly KuzzleApiMock _api;

    public CollectionControllerTest() {
      _api = new KuzzleApiMock();
      _collectionController = new CollectionController(_api.MockedObject);
    }

    [Theory]
    [
      MemberData(nameof(MockGenerators.GenerateMappings),
      MemberType = typeof(MockGenerators))
    ]
    public async void CreateAsyncTest(JObject mappings) {
      _api.SetResult(@"{result: {acknowledge: true}}");
      await _collectionController.CreateAsync("foo", "bar", mappings);

      _api.Verify(new JObject {
        { "controller", "collection" },
        { "action", "create" },
        { "index", "foo" },
        { "collection", "bar" },
        { "body", mappings}
      });
    }

    [Fact]
    public async void DeleteSpecificationsAsyncTest() {
      _api.SetResult(@"{result: {acknowledge: true}}");
      await _collectionController.DeleteSpecificationsAsync("foo", "bar");

      _api.Verify(new JObject {
        { "controller", "collection" },
        { "action", "deleteSpecifications" },
        { "index", "foo" },
        { "collection", "bar" }
      });
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async void ExistsAsyncTest(bool result) {
      _api.SetResult($"{{result: {result.ToString().ToLower()}}}");

      Assert.Equal(
        result,
        await _collectionController.ExistsAsync("foo", "bar"));

      _api.Verify(new JObject {
        { "controller", "collection" },
        { "action", "exists" },
        { "index", "foo" },
        { "collection", "bar" }
      });
    }

    [Fact]
    public async void GetMappingAsyncTest() {
      var expected = JObject.Parse(@"
      {
        foo: {
          mappings: {
            bar: {
              some: 'mappings'
            }
          }
        }
      }");

      _api.SetResult(new JObject { { "result", expected } });

      JObject mappings =
        await _collectionController.GetMappingAsync("foo", "bar");

      _api.Verify(new JObject {
        { "controller", "collection" },
        { "action", "getMapping" },
        { "index", "foo" },
        { "collection", "bar" }
      });

      Assert.Equal(
        expected["foo"]["mappings"]["bar"],
        mappings,
        new JTokenEqualityComparer());
    }

    [Fact]
    public async void GetSpecificationsAsyncTest() {
      var expected = JObject.Parse(@"
      {
        collection: 'bar',
        index: 'foo',
        validation: {
          fields: { fields: 'data' },
          strict: true
        }
      }");

      _api.SetResult(new JObject { { "result", expected } });

      JObject mappings =
        await _collectionController.GetSpecificationsAsync("foo", "bar");

      _api.Verify(new JObject {
        { "controller", "collection" },
        { "action", "getSpecifications" },
        { "index", "foo" },
        { "collection", "bar" }
      });

      Assert.Equal(
        expected["validation"],
        mappings,
        new JTokenEqualityComparer());
    }

    [Theory]
    [
      MemberData(nameof(MockGenerators.GenerateListOptions),
      MemberType = typeof(MockGenerators))
    ]
    public async void ListAsyncTest(ListOptions opts) {
      _api.SetResult(@"{result: {foo: 123}}");

      JObject result = await _collectionController.ListAsync("foo", opts);

      var expected = new JObject {
        { "controller", "collection" },
        { "action", "list" },
        { "index", "foo" }
      };

      if (opts != null) {
        expected.Merge(JObject.FromObject(opts));
      }

      _api.Verify(expected);

      Assert.Equal(
       new JObject { { "foo", 123 } },
       result,
       new JTokenEqualityComparer());
    }

    [Theory]
    [
      MemberData(nameof(MockGenerators.GenerateSearchOptions),
      MemberType = typeof(MockGenerators))
    ]
    public async void SearchSpecificationsAsyncTest(SearchOptions opts) {
      _api.SetResult(@"{
        result: {
          hits: ['foo', 'bar', 'baz'],
          total: 42,
          scrollId: 'foobar',
          aggregations: { agg: 'agg' }
        }
      }");

      var filters = new JObject { { "foo", "bar" } };

      SearchResults result =
        await _collectionController.SearchSpecificationsAsync(filters, opts);

      var expected = new JObject {
        { "controller", "collection" },
        { "action", "searchSpecifications" },
        { "body", filters }
      };

      if (opts != null) {
        expected.Merge(JObject.FromObject(opts));
      }

      _api.Verify(expected);

      Assert.Equal(
       JArray.Parse("['foo', 'bar', 'baz']"),
       result.Hits,
       new JTokenEqualityComparer());

      Assert.Equal(42, result.Total);
      Assert.Equal(3, result.Fetched);
      Assert.Equal("foobar", result.ScrollId);

      Assert.Equal(
        new JObject { { "agg", "agg" } },
        result.Aggregations,
        new JTokenEqualityComparer());
    }

    [Fact]
    public async void TruncateAsyncTest() {
      _api.SetResult(@"{result: {acknowledge: true}}");
      await _collectionController.TruncateAsync("foo", "bar");

      _api.Verify(new JObject {
        { "controller", "collection" },
        { "action", "truncate" },
        { "index", "foo" },
        { "collection", "bar" }
      });
    }

    [Theory]
    [
      MemberData(nameof(MockGenerators.GenerateMappings),
      MemberType = typeof(MockGenerators))
    ]
    public async void UpdateMappingAsyncTest(JObject mappings) {
      _api.SetResult(@"{result: {acknowledge: true}}");
      await _collectionController.UpdateMappingAsync("foo", "bar", mappings);

      _api.Verify(new JObject {
        { "controller", "collection" },
        { "action", "updateMapping" },
        { "index", "foo" },
        { "collection", "bar" },
        { "body", mappings }
      });
    }

    [Fact]
    public async void UpdateSpecificationsAsyncTest() {
      var expected = JObject.Parse(@"
      {
        foo: {
          bar: {
            fields: { fields: 'data' },
            strict: true
          }
        }
      }");

      var payload = new JObject { { "foo", "bar" } };

      _api.SetResult(new JObject { { "result", expected } });

      JObject mappings = await _collectionController.UpdateSpecificationsAsync(
        "foo", "bar", payload);

      _api.Verify(new JObject {
        { "controller", "collection" },
        { "action", "updateSpecifications" },
        { "index", "foo" },
        { "collection", "bar" },
        { "body", payload }
      });

      Assert.Equal(
        expected["foo"]["bar"],
        mappings,
        new JTokenEqualityComparer());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async void ValidateSpecificationsAsyncTest(bool response) {
      _api.SetResult(JObject.Parse(
        $"{{ result: {{ valid: {response.ToString().ToLower()} }} }}"));

      var payload = new JObject { { "foo", "bar" } };

      bool result =
        await _collectionController.ValidateSpecificationsAsync(payload);

      _api.Verify(new JObject {
        { "controller", "collection" },
        { "action", "validateSpecifications" },
        { "body", payload }
      });

      Assert.Equal(response, result);
    }
  }
}
