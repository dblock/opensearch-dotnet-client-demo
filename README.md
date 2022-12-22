# OpenSearch DotNet (.NET) Client Demo

Makes requests to Amazon OpenSearch using the [OpenSearch DotNet Client](https://github.com/opensearch-project/opensearch-net).

### Install Prerequisites

#### DotNet Framework

Install [DotNet](https://learn.microsoft.com/en-us/dotnet/core/install). YMMV.

```
$ dotnet --version
7.0.100
```

## Running

Create an OpenSearch domain in (AWS) which support IAM based AuthN/AuthZ and run the demo.

```
export AWS_ACCESS_KEY_ID=
export AWS_SECRET_ACCESS_KEY=
export AWS_SESSION_TOKEN=

export OPENSEARCH_ENDPOINT=https://....us-west-2.es.amazonaws.com
export OPENSEARCH_REGION=us-west-2

dotnet run
```

The [code](Program.cs) will connect to OpenSearch, display its version, create an index, add a document, search for it, output the search result, then cleanup.

```
opensearch: 2.3.0

{
  "Explanation": null,
  "Fields": null,
  "Highlight": {},
  "Id": "1",
  "Index": "sample-index",
  "InnerHits": {},
  "MatchedQueries": [],
  "Nested": null,
  "PrimaryTerm": null,
  "Routing": null,
  "Score": 0.13353139,
  "SequenceNumber": null,
  "Sorts": [],
  "Source": {
    "Id": 1,
    "FirstName": "Bruce"
  },
  "Type": null,
  "Version": 0
}
```

## License 

This project is licensed under the [Apache v2.0 License](LICENSE.txt).

## Copyright

Copyright OpenSearch Contributors. See [NOTICE](NOTICE.txt) for details.
