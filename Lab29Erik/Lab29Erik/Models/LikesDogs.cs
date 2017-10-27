using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lab29Erik.Models
{
    public class LikesDogs : AuthorizationHandler<MustLikeDogs>
    {
        
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MustLikeDogs requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                return Task.CompletedTask;

            }

            var likesDogs = Convert.ToString(context.User.FindFirst(c => c.Type == ClaimTypes.Email).Value);
            var admin = Convert.ToString(context.User.FindFirst(c => c.Type == ClaimTypes.Role).Value);

            if (likesDogs.Contains("y") || admin == "Administrator")
            {
                context.Succeed(requirement);
            }


            return Task.CompletedTask;
        }
    }
}
