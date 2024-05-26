using Microsoft.Azure.Cosmos;
using VPop.Data;

namespace VPop.Server.Services.Database
{
    /// <summary>
    /// Accesses the Cosmos DB database
    /// </summary>
    public class CharacterDbService(
        Microsoft.Azure.Cosmos.CosmosClient dbClient,
        string databaseName,
        string containerName)
    {
        private readonly Container _container = dbClient.GetContainer(databaseName, containerName);

        /// <summary>
        /// Adds a character to the database
        /// </summary>
        public async Task AddCharacterAsync(Character character)
        {
            if (string.IsNullOrEmpty(character.Id))
            {
                character.Id = Guid.NewGuid().ToString();
            }
            await _container.CreateItemAsync(character);
        }

        /// <summary>
        /// Deletes a character from the database
        /// </summary>
        public async Task DeleteCharacterAsync(string id)
        {
            await _container.DeleteItemAsync<Character>(id, new PartitionKey(id));
        }

        /// <summary>
        /// Gets character information from the database
        /// </summary>
        public async Task<Character> GetCharacterAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<Character>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a list of characters from the database
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Character>> GetCharactersAsync(string queryString)
        {
            var query = _container.GetItemQueryIterator<Character>(new QueryDefinition(queryString));
            var results = new List<Character>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }
    }
}
