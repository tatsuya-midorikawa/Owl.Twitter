namespace Owl.Twitter

open System

module SearchTweets = 
  [<Literal>]
  let private ep_recent = "https://api.twitter.com/2/tweets/search/recent"
  [<Literal>]
  let private ep_all = "https://api.twitter.com/2/tweets/search/all"
  
  type Expansions = 
    { value: string }
    static member attachments'poll'ids = { value = "attachments.poll_ids" }
    static member attachments'media'keys = { value = "attachments.media_keys" }
    static member author'id = { value = "author_id" }
    static member entities'mentions'username = { value = "entities.mentions.username" }
    static member geo'place'id = { value = "geo.place_id" }
    static member in'reply'to'user'id = { value = "in_reply_to_user_id" }
    static member referenced'tweets'id = { value = "referenced_tweets.id" }
    static member referenced'tweets'id'author'id = { value = "referenced_tweets.id.author_id" }

  type MediaFields =
    { value: string }
    static member duration'ms = { value = "duration_ms" }
    static member height = { value = "height" }
    static member media'key = { value = "media_key" }
    static member preview'image'url = { value = "preview_image_url" }
    static member type' = { value = "type" }
    static member url = { value = "url" }
    static member width = { value = "width" }
    static member public'metrics = { value = "public_metrics" }
    static member non'public'metrics = { value = "non_public_metrics" }
    static member organic'metrics = { value = "organic_metrics" }
    static member promoted'metrics = { value = "promoted_metrics" }
    static member alt'text = { value = "alt_text" }

  type PlaceFields = 
    { value: string }
    static member contained'within = { value = "contained_within" }
    static member country = { value = "country" }
    static member country'code = { value = "country_code" }
    static member full'name = { value = "full_name" }
    static member geo = { value = "geo" }
    static member id = { value = "id" }
    static member name = { value = "name" }
    static member place'type = { value = "place_type" }
  
  type PollFields =
    { value: string }
    static member duration'minutes = { value = "duration_minutes" }
    static member end'datetime = { value = "end_datetime" }
    static member id = { value = "id" }
    static member options = { value = "options" }
    static member voting'status = { value = "voting_status" }

  type TweetFields =
    { value: string }
    static member attachments = { value = "attachments" }
    static member author_id = { value = "tweet_author_id" }
    static member context_annotations = { value = "context_annotations" }
    static member conversation_id = { value = "conversation_id" }
    static member created_at = { value = "created_at" }
    static member entities = { value = "entities" }
    static member geo = { value = "geo" }
    static member id = { value = "id" }
    static member in_reply_to_user_id = { value = "in_reply_to_user_id" }
    static member lang = { value = "lang" }
    static member non_public_metrics = { value = "non_public_metrics" }
    static member public_metrics = { value = "public_metrics" }
    static member organic_metrics = { value = "organic_metrics" }
    static member promoted_metrics = { value = "promoted_metrics" }
    static member possibly_sensitive = { value = "possibly_sensitive" }
    static member referenced_tweets = { value = "referenced_tweets" }
    static member reply_settings = { value = "reply_settings" }
    static member source = { value = "source" }
    static member text = { value = "text" }
    static member withheld = { value = "withheld" }
