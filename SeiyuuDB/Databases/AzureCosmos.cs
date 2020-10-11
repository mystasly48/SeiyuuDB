using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using SeiyuuDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SeiyuuDB.Databases {
  public class AzureCosmos : ISeiyuuDB {
    private CosmosClient _client;
    private Database _database;

    private readonly string _endpointUri;
    private readonly string _primaryKey;
    private readonly string _databaseId = "SeiyuuDB";

    private readonly string _actorContainerId = "Actor";
    private readonly string _animeContainerId = "Anime";
    private readonly string _animeFilmographyContainerId = "AnimeFilmography";
    private readonly string _companyContainerId = "Company";
    private readonly string _externalLinkContainerId = "ExternalLink";
    private readonly string _noteContainerId = "Note";
    private readonly string _radioContainerId = "Radio";
    private readonly string _radioFilmographyContainerId = "RadioFilmography";
    private readonly string _containerKey = "/id";

    private Container _actorContainer;
    private Container _animeContainer;
    private Container _animeFilmographyContainer;
    private Container _companyContainer;
    private Container _externalLinkContainer;
    private Container _noteContainer;
    private Container _radioContainer;
    private Container _radioFilmographyContainer;
    
    public AzureCosmos(string endpoint_uri, string primary_key) {
      _endpointUri = endpoint_uri;
      _primaryKey = primary_key;

      _client = new CosmosClient(_endpointUri, _primaryKey);
    }

    public async Task CreateDatabaseAsync() {
      _database = await _client.CreateDatabaseIfNotExistsAsync(_databaseId);
      Console.WriteLine("Created database: {0}", _database.Id);
    }

    public async Task CreateContainersAsync() {
      _actorContainer = await _database.CreateContainerIfNotExistsAsync(_actorContainerId, _containerKey);
      _animeContainer = await _database.CreateContainerIfNotExistsAsync(_animeContainerId, _containerKey);
      _animeFilmographyContainer = await _database.CreateContainerIfNotExistsAsync(_animeFilmographyContainerId, _containerKey);
      _companyContainer = await _database.CreateContainerIfNotExistsAsync(_companyContainerId, _containerKey);
      _externalLinkContainer = await _database.CreateContainerIfNotExistsAsync(_externalLinkContainerId, _containerKey);
      _noteContainer = await _database.CreateContainerIfNotExistsAsync(_noteContainerId, _containerKey);
      _radioContainer = await _database.CreateContainerIfNotExistsAsync(_radioContainerId, _containerKey);
      _radioFilmographyContainer = await _database.CreateContainerIfNotExistsAsync(_radioFilmographyContainerId, _containerKey);
    }

    private string SerializeEntity<T>(T entity) {
      return JsonConvert.SerializeObject(entity);
    }

    private Container GetContainer<T>() where T : class, ISeiyuuEntity<T> {
      if (typeof(T) == typeof(Actor)) {
        return _actorContainer;
      } else if (typeof(T) == typeof(Anime)) {
        return _animeContainer;
      } else if (typeof(T) == typeof(AnimeFilmography)) {
        return _animeFilmographyContainer;
      } else if (typeof(T) == typeof(Company)) {
        return _companyContainer;
      } else if (typeof(T) == typeof(ExternalLink)) {
        return _externalLinkContainer;
      } else if (typeof(T) == typeof(Note)) {
        return _noteContainer;
      } else if (typeof(T) == typeof(Radio)) {
        return _radioContainer;
      } else if (typeof(T) == typeof(RadioFilmography)) {
        return _radioFilmographyContainer;
      }
      return null;
    }

    public async Task unti() {
      var sql = 
        @"SELECT * 
          FROM Actor a JOIN c IN a WHERE c.id = '1'";
      var query = new QueryDefinition(sql);
      var resultIterator = _noteContainer.GetItemQueryIterator<Note>(query);
      
      if (resultIterator.HasMoreResults) {
        var items = await resultIterator.ReadNextAsync();
        Console.WriteLine("RUs: {0}", items.RequestCharge);
        foreach (var item in items) {
          Console.WriteLine(item);
        }
      }
      await Task.CompletedTask;
    }

    public async Task<int> InsertAsync<T>(T entity) where T : class, ISeiyuuEntity<T> {
      // check if the entity is already exists
      //ItemResponse<Actor> response = await _actorContainer.ReadItemAsync<Actor>(SerializeEntity(entity), new PartitionKey(entity.Id));
      try {
        var response = await GetContainer<T>().CreateItemAsync(entity, new PartitionKey(entity.Id.ToString()));
        Console.WriteLine("Created with Id: {0}, consumed RUs: {1}", response.Resource.Id, response.RequestCharge);
        return response.Resource.Id;
      } catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.Conflict) {
        Console.WriteLine("The entity is already exists.");
      }
      return -1;
    }

    public async Task<T> GetEntityAsync<T>(int id) where T : class, ISeiyuuEntity<T> {
      var sql = $"SELECT * FROM c WHERE c.id = '{id}'";
      var query = new QueryDefinition(sql);
      var resultIterator = GetContainer<T>().GetItemQueryIterator<T>(query);

      if (resultIterator.HasMoreResults) {
        var items = await resultIterator.ReadNextAsync();
        Console.WriteLine("RUs: {0}", items.RequestCharge);
        return items.FirstOrDefault();
      }
      return null;
    }

    public async Task<List<T>> GetTableListAsync<T>() where T : class, ISeiyuuEntity<T> {
      var sql = $"SELECT * FROM c";
      var query = new QueryDefinition(sql);
      var resultIterator = GetContainer<T>().GetItemQueryIterator<T>(query);
      
      var result = new List<T>();
      while (resultIterator.HasMoreResults) {
        var items = await resultIterator.ReadNextAsync();
        Console.WriteLine("RUs: {0}", items.RequestCharge);
        foreach (var item in items) {
          result.Add(item);
        }
      }
      return result;
    }

    public T GetEntity<T>(int id) where T : class, ISeiyuuEntity<T> => throw new NotImplementedException();
    public T[] GetTableArray<T>() where T : class, ISeiyuuEntity<T> => throw new NotImplementedException();
    public bool IsExists<T>(T entity) where T : class, ISeiyuuEntity<T> => throw new NotImplementedException();
    public int Insert<T>(T entity) where T : class, ISeiyuuEntity<T> => throw new NotImplementedException();
    public int Update<T>(T entity) where T : class, ISeiyuuEntity<T> => throw new NotImplementedException();
    public int Delete<T>(T entity) where T : class, ISeiyuuEntity<T> => throw new NotImplementedException();
    public string SavePictureToBlob(string url) => throw new NotImplementedException();
  }
}
