﻿using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;
using KuzzleSdk.Protocol;
using KuzzleSdk.API;
using KuzzleSdk.API.Controllers;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using KuzzleSdk.API.Offline;
using KuzzleSdk.EventHandler;

[assembly: InternalsVisibleTo("Kuzzle.Tests")]

// Allow using Moq on internal objects/interfaces
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace KuzzleSdk {
  /// <summary>
  /// Kuzzle API interface.
  /// </summary>
  public interface IKuzzleApi {
    /// <summary>
    /// Gets or sets the authentication token.
    /// </summary>
    /// <value>The authentication token.</value>
    string AuthenticationToken { get; set; }

    /// <summary>
    /// Event handler
    /// </summary>
    AbstractKuzzleEventHandler EventHandler { get; }

    /// <summary>
    /// Gets the instance identifier.
    /// </summary>
    /// <value>The instance identifier.</value>
    string InstanceId { get; }

    /// <summary>
    /// Gets the SDK name.
    /// </summary>
    /// <value>The SDK name identifier.</value>
    string SdkName { get; }

    /// <summary>
    /// Gets the network protocol.
    /// </summary>
    /// <value>The network protocol.</value>
    AbstractProtocol NetworkProtocol { get; }

    /// <summary>
    /// Sends a query to Kuzzle's API
    /// </summary>
    /// <returns>The query response.</returns>
    /// <param name="query">Kuzzle API query</param>
    ConfiguredTaskAwaitable<Response> QueryAsync(JObject query);
  }

  internal interface IKuzzle {
    string AuthenticationToken { get; set; }

    IAuthController GetAuth();
    IRealtimeController GetRealtime();
    AbstractKuzzleEventHandler GetEventHandler();

    TaskCompletionSource<Response> GetRequestById(string requestId);
    }

  /// <summary>
  /// Main entry point for this SDK.
  /// </summary>
  public sealed class Kuzzle : IKuzzleApi, IKuzzle {
    private AbstractProtocol networkProtocol;

    private SemaphoreSlim requestsSemaphore = new SemaphoreSlim(1, 1);

    internal readonly Dictionary<string, TaskCompletionSource<Response>>
        requests = new Dictionary<string, TaskCompletionSource<Response>>();

    // General informations

    /// <summary>
    /// This SDK Version
    /// </summary>
    public readonly string Version;

    /// <summary>
    /// Instance unique identifier.
    /// </summary>
    public string InstanceId { get; }

    /// <summary>
    /// SDK name.
    /// </summary>
    public string SdkName { get; }

    /// <summary>
    /// Exposes actions from the "auth" Kuzzle API controller
    /// </summary>
    public AuthController Auth { get; private set; }

    /// <summary>
    /// Exposes actions from the "collection" Kuzzle API controller
    /// </summary>
    public CollectionController Collection { get; private set; }

    /// <summary>
    /// Exposes actions from the "document" Kuzzle API controller
    /// </summary>
    public DocumentController Document { get; private set; }

    /// <summary>
    /// Exposes actions from the "index" Kuzzle API controller
    /// </summary>
    public IndexController Index { get; private set; }

    /// <summary>
    /// Exposes actions from the "realtime" Kuzzle API controller
    /// </summary>
    public RealtimeController Realtime { get; private set; }

    /// <summary>
    /// Exposes actions from the "server" Kuzzle API controller
    /// </summary>
    public ServerController Server { get; private set; }

    /// <summary>
    /// Exposes actions from the "bulk" Kuzzle API controller
    /// </summary>
    public BulkController Bulk { get; private set; }

    /// <summary>
    /// Exposes actions from the "admin" Kuzzle API controller
    /// </summary>
    public AdminController Admin { get; private set; }

    /// <summary>
    /// Exposes the event handler
    /// </summary>
    public AbstractKuzzleEventHandler EventHandler { get; private set; }

    /// <summary>
    /// Exposes actions from the OfflineManager
    /// </summary>
    public OfflineManager Offline { get; private set; }

    /// <summary>
    /// The maximum amount of elements that the queue can contains.
    /// If set to -1, the size is unlimited.
    /// </summary>
    public int MaxQueueSize {
      get { return Offline.MaxQueueSize; }
      set { Offline.MaxQueueSize = value; }
    }

    /// <summary>
    /// The minimum duration of a Token after refresh.
    /// If set to -1 the SDK does not refresh the token automaticaly.
    /// </summary>
    public int RefreshedTokenDuration {
      get { return Offline.RefreshedTokenDuration; }
      set { Offline.RefreshedTokenDuration = value; }
    }

    /// <summary>
    /// The minimum duration of a Token before being automaticaly refreshed.
    /// If set to -1 the SDK does not refresh the token automaticaly.
    /// </summary>
    public int MinTokenDuration {
      get { return Offline.MinTokenDuration; }
      set { Offline.MinTokenDuration = value; }
    }

    /// <summary>
    /// The maximum delay between two requests to be replayed
    /// </summary>
    public int MaxRequestDelay {
      get { return Offline.MaxRequestDelay; }
      set { Offline.MaxRequestDelay = value; }
    }

    /// <summary>
    /// Custom filter function: if it returns "false", the query is discarded
    /// instead of being queued.
    /// </summary>
    public Func<JObject, bool> QueueFilter {
      get { return Offline.QueueFilter; }
      set { Offline.QueueFilter = value; }
    }

    /// <summary>
    /// Queue requests when network is down,
    /// and automatically replay them when the SDK successfully reconnects.
    /// </summary>
    public bool AutoRecover {
      get { return Offline.AutoRecover; }
      set { Offline.AutoRecover = value; }
    }

    /// <summary>
    /// Authentication token
    /// </summary>
    public string AuthenticationToken { get; set; }

    /// <summary>
    /// Network Protocol
    /// </summary>
    public AbstractProtocol NetworkProtocol {
      get {
        return networkProtocol;
      }
      set {
        if (networkProtocol != null) {
          networkProtocol.ResponseEvent -= ResponsesListener;
          NetworkProtocol.StateChanged -= StateChangeListener;
        }

        AuthenticationToken = null;
        networkProtocol = value;
      }
    }

    /// <summary>
    /// Handles the ResponseEvent event from the network protocol
    /// </summary>
    /// <param name="sender">Network Protocol instance</param>
    /// <param name="payload">raw API Response</param>
    internal void ResponsesListener(object sender, string payload) {
      Response response = Response.FromString(payload);

      if (!requests.ContainsKey(response.Room)) {
        EventHandler.DispatchUnhandledResponse(response);
        return;
      }

      TaskCompletionSource<Response> task = requests[response.RequestId];

      if (response.Error != null) {
        if (response.Error.Message == "Token expired") {
          EventHandler.DispatchTokenExpired();
        }

        task.SetException(new Exceptions.ApiErrorException(response));
      }
      else {
        task.SetResult(response);
      }

      requestsSemaphore.Wait();
      try {
        requests.Remove(response.RequestId);
      }
      finally {
        requestsSemaphore.Release();
      }

      Offline?.QueryReplayer?.Remove(
        (obj) => obj["requestId"].ToString() == response.RequestId);
    }

    internal void StateChangeListener(object sender, ProtocolState state) {
      // If not connected anymore: close pending tasks and clean up the requests
      // buffer.
      // If reconnecting, only requests submitted AFTER the disconnection event
      // can be queued: we have no information about requests submitted before
      // that event. For all we know, Kuzzle could have received & processed
      // those requests, but couldn't forward the response to us
      if (state == ProtocolState.Closed || state == ProtocolState.Reconnecting) {
        requestsSemaphore.Wait();
        try {
          foreach (var task in requests.Values) {
            task.SetException(new Exceptions.ConnectionLostException());
          }

          requests.Clear();
        }
        finally {
          requestsSemaphore.Release();
        }
      }
    }


    /// <summary>
    /// Initialize a new instance of the <see cref="T:Kuzzle.Kuzzle"/> class.
    /// </summary>
    public Kuzzle(
      AbstractProtocol networkProtocol,
      int refreshedTokenDuration = 3600000,
      int minTokenDuration = 3600000,
      int maxQueueSize = -1,
      int maxRequestDelay = 1000,
      bool autoRecover = false,
      Func<JObject, bool> queueFilter = null
    ) {
      NetworkProtocol = networkProtocol;
      NetworkProtocol.ResponseEvent += ResponsesListener;
      NetworkProtocol.StateChanged += StateChangeListener;

      EventHandler = new KuzzleEventHandler(this);

      // Initializes the controllers
      Auth = new AuthController(this);
      Collection = new CollectionController(this);
      Document = new DocumentController(this);
      Index = new IndexController(this);
      Realtime = new RealtimeController(this);
      Server = new ServerController(this);
      Bulk = new BulkController(this);
      Admin = new AdminController(this);

      Offline = new OfflineManager(networkProtocol, this) {
        RefreshedTokenDuration = refreshedTokenDuration,
        MinTokenDuration = minTokenDuration,
        MaxQueueSize = maxQueueSize,
        MaxRequestDelay = maxRequestDelay,
        QueueFilter = queueFilter,
        AutoRecover = autoRecover
      };

      // Initializes instance unique properties
      Version = typeof(Kuzzle)
        .GetTypeInfo()
        .Assembly
        .GetName()
        .Version
        .ToString();

      InstanceId = Guid.NewGuid().ToString();

      SdkName = $"csharp@{Version}";
    }

    /// <summary>
    /// Releases unmanaged resources and performs other cleanup operations
    /// before the <see cref="T:KuzzleSdk.Kuzzle"/>
    /// is reclaimed by garbage collection.
    /// </summary>
    ~Kuzzle() {
      NetworkProtocol.ResponseEvent -= ResponsesListener;
      NetworkProtocol.StateChanged -= StateChangeListener;
    }

    /// <summary>
    /// Establish a network connection
    /// </summary>
    public async Task ConnectAsync(CancellationToken cancellationToken) {
      await NetworkProtocol.ConnectAsync(cancellationToken);
    }

    /// <summary>
    /// Disconnect this instance.
    /// </summary>
    public void Disconnect() {
      NetworkProtocol.Disconnect();
    }

    TaskCompletionSource<Response> IKuzzle.GetRequestById(string requestId) {
      return requests[requestId];
    }

    /// <summary>
    /// Sends an API request to Kuzzle and returns the corresponding API
    /// response.
    /// </summary>
    /// <returns>API response</returns>
    /// <param name="query">Kuzzle API query</param>
    public ConfiguredTaskAwaitable<Response> QueryAsync(JObject query) {
      if (query == null) {
        throw new Exceptions.InternalException("You must provide a query", 400);
      }

      if (NetworkProtocol.State == ProtocolState.Closed) {
        throw new Exceptions.NotConnectedException();
      }

      if (query["waitForRefresh"] != null) {
        if (query["waitForRefresh"].ToObject<bool>()) {
          query.Add("refresh", "wait_for");
        }
        query.Remove("waitForRefresh");
      }

      if (AuthenticationToken != null) {
        query["jwt"] = AuthenticationToken;
      }

      string requestId = Guid.NewGuid().ToString();
      query["requestId"] = requestId;

      if (query["volatile"] == null) {
        query["volatile"] = new JObject();
      } else if (!(query["volatile"] is JObject)) {
        throw new Exceptions.InternalException("Volatile data must be a JObject", 400);
      }

      query["volatile"]["sdkName"] = SdkName;
      query["volatile"]["sdkInstanceId"] = InstanceId;

      requestsSemaphore.Wait();
      requests[requestId] = new TaskCompletionSource<Response>(
        TaskCreationOptions.RunContinuationsAsynchronously);
      requestsSemaphore.Release();

      if (NetworkProtocol.State == ProtocolState.Open) {
        NetworkProtocol.Send(query);
      }
      else if (NetworkProtocol.State == ProtocolState.Reconnecting) {
        Offline.QueryReplayer.Enqueue(query);
      }

      return requests[requestId].Task.ConfigureAwait(false);
    }

    IAuthController IKuzzle.GetAuth() {
      return Auth;
    }

    IRealtimeController IKuzzle.GetRealtime() {
      return Realtime;
    }

    AbstractKuzzleEventHandler IKuzzle.GetEventHandler() {
      return EventHandler;
    }
  }
}
