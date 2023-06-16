using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ReduxSimple;
using static ReduxSimple.Effects;
using Rownd.Core;
using Rownd.Models.Domain;

namespace Rownd.Models.Repos
{
    public class StateRepo
    {
        public ReduxStore<GlobalState> Store
        {
            get;
            private set;
        }

        public static StateRepo Get()
        {
            return Shared.ServiceProvider.GetService<StateRepo>();
        }

        IDisposable statePersistenceListener;

        public StateRepo()
        {

        }

        public void Setup()
        {
            LoadState();
            var appConfigRepo = AppConfigRepo.Get();
            var task = new Task(async () => { await appConfigRepo.LoadAppConfigAsync(); });
            task.Start();
        }

        private void LoadState()
        {
            try
            {
                String existingStateJsonStr = Shared.App.Properties["rownd_state"] as string;
                GlobalState existingState = JsonConvert.DeserializeObject<GlobalState>(existingStateJsonStr);
                InitializeStore(existingState);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                InitializeStore();
                return;
            }
        }

        private void SaveState(GlobalState state)
        {
            var stateJson = JsonConvert.SerializeObject(state);
            Console.WriteLine($"Saving serialized state to storage: {stateJson}");
            Shared.App.Properties["rownd_state"] = stateJson;

        }

        private void InitializeStore(GlobalState existingState = null)
        {
            if (Store != null)
            {
                return;
            }

            Store = new ReduxStore<GlobalState>(StateReducers.CreateReducers(), existingState);

            statePersistenceListener = Store.Select()
                .Subscribe(state =>
                {
                    // Listening to the full state (when any property changes)
                    Console.WriteLine($"Updating state: {state}");
                    SaveState(state);
                });
        }
    }
}

