using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lab29Erik.Models
{
    public class Is21 : AuthorizationHandler<MinimumAgeRequierment>
    {
        private const int minAge = 21;
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequierment requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            {
                return Task.CompletedTask;

            }

            var birthday = Convert.ToDateTime(context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth).Value);

            int age = DateTime.Compare(DateTime.Now, birthday);

            if (age >= minAge)
            {
                context.Succeed(requirement);
            }


            return Task.CompletedTask;
        }
    }
}
