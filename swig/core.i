%rename(TokenValidity) token_validity;
%rename(AckResponse) ack_response;
%rename(queueTTL) queue_ttl;
%rename(Options, match="class") options;
%rename(QueryOptions) query_options;
%rename(JsonObject) json_object;
%rename(JsonResult) json_result;
%rename(LoginResult) login_result;
%rename(BoolResult) bool_result;
%rename(Statistics) statistics;
%rename(AllStatisticsResult) all_statistics_result;
%rename(StatisticsResult) statistics_result;
%rename(CollectionsList) collection_entry;
%rename(CollectionsListResult) collection_entry_result;
%rename(StringArrayResult) string_array_result;
%rename(KuzzleResponse) kuzzle_response;
%rename(KuzzleRequest) kuzzle_request;
%rename(ShardsResult) shards_result;
%rename(DateResult) date_result;
%rename(UserData) user_data;
%rename(RoomOptions) room_options;
%rename(SearchFilters) search_filters;
%rename(NotificationResult) notification_result;
%rename(NotificationContent) notification_content;
%rename(SubscribeToSelf) subscribe_to_self;
%rename(Mapping, match="class") mapping;

%rename(_auth, match="class") auth;
%rename(_kuzzle, match="class") kuzzle;
%rename(_realtime, match="class") realtime;
%rename(_collection, match="class") collection;
%rename(_document, match="class") document;
%rename(_server, match="class") server;

%ignore *::error;
%ignore *::status;
%ignore *::stack;

%feature("director") NotificationListener;
%feature("director") EventListener;
%feature("director") SubscribeListener;

%{
#include "websocket.cpp"
#include "search_result.cpp"
#include "user.cpp"
#include "user_right.cpp"
#include "kuzzle.cpp"

#include "collection.cpp"
#include "auth.cpp"
#include "index.cpp"
#include "server.cpp"
#include "document.cpp"
#include "default_constructors.cpp"
#include "realtime.cpp"

#define SWIG_FILE_WITH_INIT
%}

%include "stl.i"
%include "kcore.i"

%extend options {
    options() {
        options *o = kuzzle_new_options();
        return o;
    }

    ~options() {
        free($self);
    }
}

%extend kuzzleio::kuzzle_response {
    ~kuzzle_response() {
        kuzzle_free_kuzzle_response($self);
    }
}


%include "websocket.cpp"
%include "kuzzle.cpp"
%include "search_result.cpp"
%include "default_constructors.cpp"
%include "user.cpp"
%include "user_right.cpp"

%include "collection.cpp"
%include "document.cpp"
%include "realtime.cpp"
%include "auth.cpp"
%include "index.cpp"
%include "server.cpp"
