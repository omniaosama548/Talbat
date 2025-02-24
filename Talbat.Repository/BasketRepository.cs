﻿using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talbat.Core.Entites;
using Talbat.Core.Repositories;

namespace Talbat.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;

        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string BasketId)
        {
            return await _database.KeyDeleteAsync(BasketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
        {
            var Basket=await _database.StringGetAsync(BasketId);
            return Basket.IsNull?null:JsonSerializer.Deserialize<CustomerBasket>(Basket);   
            //Deserialize =>from json to obj
            //Serialize =>from obj to json
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket Basket)
        {
            var JsonBasket=JsonSerializer.Serialize(Basket);
            var CreatedOrUpdatedBasket=await _database.StringSetAsync(Basket.Id, JsonBasket,TimeSpan.FromDays(1));
            if(!CreatedOrUpdatedBasket) return null;
            return await GetBasketAsync(Basket.Id);
        }
    }
}
