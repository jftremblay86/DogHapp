using DogFetchApp;
using DogFetchApp.Models;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DogFetchApp.Helper
{
    public class DogApiProcessor
    {

        public static async Task<ObservableCollection<string>> LoadBreedList()
        {
            string url = $"https://dog.ceo/api/breeds/list/all";

            using HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                ObservableCollection<string> Breeds = new ObservableCollection<string>();

                BreedModel result = await response.Content.ReadAsAsync<BreedModel>();
                foreach (string b in result.Breed.Keys)
                {
                    Breeds.Add(b);
                    foreach (string sb in result.Breed[b])
                    {
                        Breeds.Add(b + "/" + sb);

                    }
                }
                return Breeds;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }

        }

        public static async Task<DogModel> GetImageUrl(string SelectedBreed)
        {

            string url = $"https://dog.ceo/api/breed/{SelectedBreed}/images/random";

            using HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {

                DogModel result = await response.Content.ReadAsAsync<DogModel>();
                
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
         
        }

        
    }
}
