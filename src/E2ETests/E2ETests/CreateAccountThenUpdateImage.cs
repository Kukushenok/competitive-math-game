using FluentAssertions;
using CompetitiveBackend.BackendUsage.Objects;
using Allure.Xunit.Attributes.Steps;
using IntegrationalTests;
using System.Net.Http.Json;
using RepositoriesRealisation.Models;
using Bogus;
using Microsoft.EntityFrameworkCore;
using CompetitiveBackend.Core.Objects;
using Allure.Net.Commons;
namespace E2ETests
{
    public class FuncTestStructure
    {
        public bool IsPositive { get; init; }
        public string ResultStorage { get; init; }
        public string TestStorage { get; init; }
        public FuncTestStructure(string testStorage, bool isPositive, string ResultStorage)
        {
            TestStorage = testStorage;
            IsPositive = isPositive;
            this.ResultStorage = ResultStorage;
        }
        public LoadData Data => new LoadData(TestStorage);
        public async Task SaveTest(LargeData data)
        {
            await File.WriteAllBytesAsync(ResultStorage, data.Data);
        }
    }
    public class LoadData: IAsyncDisposable
    {
        private Stream s;
        public MultipartFormDataContent FormContent { get; set; }
        public LoadData(string name)
        {
            s = File.OpenRead(name);
            FormContent = new MultipartFormDataContent
            {
                { new StreamContent(s), "file", Path.GetFileNameWithoutExtension(name)}
            };
        }

        public async ValueTask DisposeAsync()
        {
            FormContent.Dispose();
            await ((IAsyncDisposable)s).DisposeAsync();
        }
    }
    //public class TempData : IDisposable
    //{
    //    public string name {{ get;} }
    //    public TempData(string name)
    //    {
    //        this.name = name;
    //    }

    //    public Task Save()
    //    {

    //    }

    //    public void Dispose()
    //    {
    //        File.Delete(name); 
    //    }
    //}

    public class CreateAccountThenUpdateImageE2ETest : IntegrationalTest
    {
        public CreateAccountThenUpdateImageE2ETest(IntegrationalFixture f) : base(f)
        {

        }

        [Theory]
        [ClassData(typeof(ImageTestingDataManager))]
        public async Task CompetitionFetchOne(FuncTestStructure std)
        {
            
            var acc = await CreateAccount();
            if(std.IsPositive)
            {
                await DoSuccessful(std, acc.Token);
            } 
            else
            {
                await DoFailure(std, acc.Token);
            }
            await RemoveAccount(acc);
        }
        [AllureStep("Create account")]
        private async Task<AuthSuccessResultDTO> CreateAccount()
        {
            
            var x = await Client.PostAsJsonAsync("/api/v1/auth/register", new AccountCreationDTO(
                Faker.Internet.UserName(),
                "MySuperLongPassword",
                "mrcoolmoder@gmail.com"
            ));
            var result = await x.FromJSONAsync<AuthSuccessResultDTO>();
            //Client.DefaultRequestHeaders.Add("Bearer", result.Token);
            return result;
        }

        [AllureStep("Cleanup")]
        private async Task RemoveAccount(AuthSuccessResultDTO dto)
        {
            int idx = dto.AccountID;
            var accs = await Context.AccountsReadOnly.Where(x => x.Id == idx).ToListAsync();
            Context.AccountsReadOnly.RemoveRange(accs);
            await Context.SaveChangesAsync();
        }

        [AllureStep("Do successful")]
        private async Task DoSuccessful(FuncTestStructure std, string token)
        {
            await using var dpd = std.Data;
            dpd.FormContent.Headers.Add("Bearer", token);
            var res1 = await Client.PutAsync($"/api/v1/players/me/image", dpd.FormContent);
            res1.IsSuccessStatusCode.Should().BeTrue();

            Client.DefaultRequestHeaders.Add("Bearer", token);
            var res2 = await Client.GetAsync($"/api/v1/players/me/image");
            Client.DefaultRequestHeaders.Remove("Bearer");
            res2.IsSuccessStatusCode.Should().BeTrue();
            var result = await res2.Content.ReadAsByteArrayAsync();
            AllureApi.AddAttachment("Processed image", "image/jpeg", result, ".jpg");
        }
        [AllureStep("Do failure")]
        private async Task DoFailure(FuncTestStructure std, string token)
        {
            await using var dpd = std.Data;
            dpd.FormContent.Headers.Add("Bearer", token);
            var res1 = await Client.PutAsync($"/api/v1/players/me/image", dpd.FormContent);
            res1.IsSuccessStatusCode.Should().BeFalse();
        }
    }
}
