# ![cf](http://i.imgur.com/7v5ASc8.png) Lab 29
## Identity Day 4 Custom Policies


### Directions
1. Building off of what you've made from lab 28:
  1.  Update your login a page in your application where you can be associated with special "claims" (at least 2).
    1. Add those claims to an identity
    1. Add the identity to the principle
    1. Add that Identity to the User (Demo code was updated to reflect this)
    1. Using HTTPContext, sign the user in by checking if the username/password are correct, and tie the ClaimsPrinciples to the authentication. 
    **NOTE** - Bug from the lecture demo code was found (and corrected in the demo code posted). Instead of doing a CheckSignInAsync(), stick with the PasswordSignInAsync(). The Demo code in the Account Controller -> Login action reflects this change.
 
1. Create 2 new custom policies add them to the Authorization in your Startup for your applicaiton. 
  1. 1 policy should have both the AuthorizationHandler and IAuthorizationRequirement in the smae file
  1. 1 policy should be 'reusable' : both the Authorization Handler and IAuthorizationRequirement should be in separate file.
    1. Dont forget the dependency Injection in the startup class

1. Require the new policies to be required at least one action each. 

### ReadMe
- Your readme should include the following information:
	- How long did it take you to complete this assignment?
	- What did you struggle with? Why? How did you solve?
	- What did you learn during this assignment?
    - What resources did you utilize for this assignment?
    

### To Submit this Assignment
- fork this repository
- write all of your code in a branch named `lab-#`; + `<your name>` **e.g.** `lab18-amanda`
- push to your repository
- submit a pull request to this repository
- submit a link to your PR in canvas


### Rubric
- 2pts: Site contians all of the required pages/requirements specified above.
- 4pts: 2 custom policies created. The above specifications of the policies are met
- 2pts: Login behavior updated to match the description above
- 2pts: Readme included with answers to questions

