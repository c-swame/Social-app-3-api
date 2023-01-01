using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Social_app_3_api.Model.User;
using System.Security.Claims;

namespace Social_app_3_api.Test
{
    public class PostControllerTest
    {
        [Fact]
        public async void TestPostRepositoryPostNewPost()
        {
            Post newPostData = new()
            {
                Content = "post1 content",
                UserId = 1
            };

            Post expectedResponse = new()
            {
                PostId = 1,
                Content = "post1 content",
                UserId = 1
            };

            var mockRepo1 = new Mock<IPostRepository>();
            mockRepo1.Setup(x => x.PostNewPost(It.IsAny<Post>())).Returns(Task.FromResult<Post?>(expectedResponse));

            var mockRepo2 = new Mock<IUserRepository>();
            mockRepo2.Setup(x => x.GetUser("email1@email.com")).ReturnsAsync(new User { Email = "email1@email.com", Id = 1 });

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, "email1@email.com"),
                new Claim("Id", "1"),
                new Claim(ClaimTypes.Name, "user1"),
                new Claim(ClaimTypes.Role, "Regular")
            }, "mock"));

            var postsController = new PostsController(mockRepo1.Object, mockRepo2.Object);

            postsController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = userClaims }
            };

            var response = (await postsController.AddPost(post)) as OkObjectResult;

            response.Should().BeOfType(typeof(OkObjectResult));
            response?.Value.Should().Be(expectedResponse);
        }
    }
}