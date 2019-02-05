using System.Collections.Generic;
using System.Threading.Tasks;

namespace Samples.AspNetCoreMvc.ClientInjectedToStartup.PetStore
{
    /// <summary>This partial class allows to wrap and extend the generated client logic if you need that.</summary>
    public partial class Client : IPetStoreClient
    {
        // Note: lets add interface methods we want to expose in our appliction via dependency 

        public async Task<int> GetSoldPetsCountAsync()
        {
            ICollection<Pet> soldPets = await this.FindPetsByStatusAsync(new[] { Anonymous.Sold });
            return soldPets.Count;
        }
    }

    /// <summary>Interface defines what we want to expose in our appliction via dependency injection.</summary>
    public interface IPetStoreClient
    {
        /// <summary>Returns number of all sold pets in the store.</summary>
        Task<int> GetSoldPetsCountAsync();

        // NOTE: you can also define here methods from generated Client partial class to expose them from interface.
    }
}
