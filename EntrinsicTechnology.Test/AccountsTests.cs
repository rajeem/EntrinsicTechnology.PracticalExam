using EntrinsicTechnology.Data.Domains;
using EntrinsicTechnology.Data.DTOs;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EntrinsicTechnology.Test
{
    public class AccountsTests
    {
        private string host = "http://localhost:47937";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void BalanceTest()
        {
            User user = CreateUser();

            Assert.IsTrue(user != null);

            Account acct = CreateAccount(user.Id);

            Assert.IsTrue(acct != null);

            var depositDto = new TransactionDto
            {
                AccountId = acct.Id,
                Amount = 1000
            };

            string json = JsonConvert.SerializeObject(depositDto);
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Username", user.UserName);
            HttpResponseMessage response = client.PostAsync(host + "/api/account/deposit", data).Result;

            var withdrawDto = new TransactionDto
            {
                AccountId = acct.Id,
                Amount = 900
            };

            json = JsonConvert.SerializeObject(withdrawDto);
            data = new StringContent(json, Encoding.UTF8, "application/json");

            response = client.PostAsync(host + "/api/account/withdraw", data).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);

            response = client.GetAsync(host + "/api/account").Result;

            IEnumerable<Account> accounts = JsonConvert.DeserializeObject<IEnumerable<Account>>(response.Content.ReadAsStringAsync().Result);

            Assert.IsTrue(accounts?.Count() > 0);
            Assert.IsTrue(accounts.Any(x => x.Id == acct.Id));
            Assert.IsTrue(accounts.Single(x => x.Id == acct.Id).Balance == 100);
        }

        private User CreateUser()
        {
            var user = new User
            {
                FirstName = "John",
                LastName = "Doe",
                CreatedDate = DateTime.Now,
                UserName = "u" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 9)
            };

            string json = JsonConvert.SerializeObject(user);
            StringContent data = new(json, Encoding.UTF8, "application/json");

            HttpClient client = new();
            client.DefaultRequestHeaders.Add("Username", "Admin");
            HttpResponseMessage response = client.PostAsync(host + "/api/user", data).Result;
            
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return null;
            }
        }

        private Account CreateAccount(int userId)
        {
            var acct = new Account
            {
                Name = "Savings Account " + Guid.NewGuid().ToString(),
                CreatedDate = DateTime.Now,
                UserId = userId
            };

            string json = JsonConvert.SerializeObject(acct);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Username", "Admin");
            HttpResponseMessage response = client.PostAsync(host + "/api/account", data).Result;

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<Account>(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return null;
            }
        }
    }
}