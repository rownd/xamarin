using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ReduxSimple;
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
				String existingStateJsonStr = Shared.app.Properties["rownd_state"] as string;
			} catch (Exception ex)
			{
				Console.WriteLine(ex);
                Store = new ReduxStore<GlobalState>(StateReducers.CreateReducers());
                return;
            }
        }
    }
}

