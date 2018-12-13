using System;
using System.Collections.Generic;
using System.Linq;
using DeveloperTest.Config;

namespace DeveloperTest.Data
{
    public class DataStoreFactory : IDataStoreFactory
    {
        private readonly IConfigProvider _configProvider;
        private readonly IEnumerable<IDataStore> _dataStores;
        private const string DataStoreType = "DataStoreType";

        public DataStoreFactory(IConfigProvider configProvider, IEnumerable<IDataStore> dataStores)
        {
            _configProvider = configProvider;
            _dataStores = dataStores;
        }

        public IDataStore Get()
        {
            var dataStore = this._configProvider.GetAppSettings(DataStoreType);

            Guard.AgainstNullOrEmpty(dataStore, "Datastore config not found.");

            IDataStore store = this._dataStores.FirstOrDefault(d => d.Type.Equals(dataStore,StringComparison.InvariantCultureIgnoreCase));

            Guard.AgainstNull(store, $"DataStore {dataStore} is not registered.");

            return store;
            
        }
    }
}
