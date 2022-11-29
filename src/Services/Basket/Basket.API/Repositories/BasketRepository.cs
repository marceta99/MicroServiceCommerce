using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        //in startup class we defined redis as distributed cache, and we installed package for disitrbuted 
        //cache in redis, and that is from where this instance of redis cache comes from  
        public BasketRepository(IDistributedCache redisCache)
        {
            this._redisCache = redisCache;
        }

        public async Task DeleteBasket(string userName)
        {
            //remove basket with that username as a key
            await _redisCache.RemoveAsync(userName); 
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);

            if (string.IsNullOrEmpty(basket))
            {
                return null; 
            }
            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        //update basket will do insert basket and also update basket because we insert new basket for same
        //key which is username, so when we update basket we will also insert new basket for that username key
        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            //here we are writing userName key, and then give the value which is basket 
            await _redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));

            return await GetBasket(basket.UserName); 
        }

    }
}
