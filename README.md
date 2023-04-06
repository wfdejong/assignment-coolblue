# assignment-coolblue

## Task 1 [bugfix]
- First I created a test to verify the bug.
- The code was that adds the 500 in case it is a laptop (or smartphone) was inside the part that was only hit if the sales value was more than 500 so I moved this part out and now this check is sales value independent.

## Task 2 [refactoring]
I chose for a domaindriven approach. Using DTOs for communication to external APIs and Request and Response classes for the endpoints. I introduced dependency injection so a seperation of concerns is easier to maintain.

### \controllers
    all endpoints
### \domain
    classes with domain logic. All business logic should be here
### \Dto
    data classes used for communication with external APIS. 
### \Infrastructure
    classes that provide communication to APIs using Dto classes.
### \Requests
    classes that are used for incoming data at endpoints
### \Responses
    classes returned and serialized by endpoinds
