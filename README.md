# Intent
The project purpose is to define a set of patterns to allow to mock call to external services in order to be able to :
- launch end to end test in a mastered environment
- mock a service when it is broken and could take time to be up again (in all environment but production)
- mock a service on all environment but production (for call time or money reason)

# Strategies
To resume, we have :
- Method mock
- Object mock
- Context which can be used to :
  * filter on specific context (sessionId or anything else) to know when apply the strategy
  * containing the object to return

The idea is to behave like NSubstitute but with a database between the client (which define the mock) and the service (which apply the defined strategy through a proxy)

You should look at unit tests to understand all available strategies

# Todo list
- Extract the context check in proxy
- Give example of Factory/Builder/... to build object before send it
- Give an example with parameter base strategy
- Give an example with multiple methods for the main classe
- Use of method identifier. It could be cool to find a way to do it like NSubstitute with something like myService.Get(Arg.Any()).Return()
- Regarding Should_not_mock_When_context_is_not_the_same test, we get the latest object where we define property IsUsed. It is kick solution but we should find something else
- Mock only on error
- Example with async
- Use a real database
- Create a view to allow mock on demand
- Give an example with front application to show interaction