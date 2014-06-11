using Tweetinvi;
using Tweetinvi.Core.Enum;

namespace Testinvi.IntegrationTests
{
    public class TweetLists
    {
        public void TweetList_Lifecycle()
        {
            var loggedUser = User.GetLoggedUser();
            var newList = TweetList.CreateList("myTemporaryList", PrivacyMode.Private, "tmp");
            var userLists = TweetList.GetUserLists(loggedUser, true);
            var newListVerify = TweetList.GetExistingList(newList);
            var updateParameter = TweetList.GenerateUpdateParameters();
            updateParameter.Name = "piloupe";
            newListVerify.Update(updateParameter);
            newListVerify.Destroy();
        }
    }
}