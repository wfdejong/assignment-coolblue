# assignment-coolblue

## Task 1 [bugfix]
- First I created a test to verify the bug.
- The code was that adds the 500 in case it is a laptop (or smartphone) was inside the part that was only hit if the sales value was more than 500 so I moved this part out and now this check is sales value independent.

## Task 2 [refactoring]
I chose for a domaindriven approach. Using DTOs for communication to external systems and Request and Response classes for the endpoints. I introduced dependency injection so a seperation of concerns is easier to maintain.

A common way for a round trip on an endpoint would be as follows:
1. in the controller class a endpoint is defined
2. the endpoint method receives data as an object defined in \Requests
3. then the repository layer is used with primitive types (like id, or name) as parameters
4. the repository returns a dto class and is checked if the data is succesfully received
5. the dto is used to create a domain class where validation and business rules are applied.
6. the domain class is mapped to a response class and returned


### \Controllers
    all endpoints
### \Domain
    classes with domain logic. All business logic should be here
### \Dto
    data classes used for communication with external systems. In case of an REST API the wrapper class ApiDto can be used and should contain a specific Dto class. ApiDto contains meta information of the call like the response code and if it was a successful call to the API. 
### \Infrastructure
    classes that provide communication to external systems like APIs or for maintaining a persistence layer. 
### \Requests
    classes that are used for incoming data at endpoints
### \Responses
    classes returned and serialized by endpoinds

## Task 3 [Feature 1]
A new endpoint is created where a list of products can be posted. Every item in this list is processed and added to a CartInsurance domain object which takes care of calculating the total insurance value of the list of products. This domain object is mapped to a CartInsuranceResponse object and returned.

## Task 4 [Feature 2]
The domain class created in Task 3 is ideal to check if the list of products contains a product of type "Digital cameras" since the scope of this class is the scope of the cart. So in the AddProductInsurance method an extra check is done to add the 500 if the product is a digital camera, but only time.

## Task 5 [Feature 3]
For this feature the following assumptions are made:
-  If producttypename is already in the surcharge list, it will be overwritten with the new value
- If producttype.canBeInsured is false, surcharge is added anyway.
    - Reason: it makes sense to overwrite this value this way. Otherwise a change needs to the products api to achieve a surcharge on products that have this flag set to false.
- No check is made if the product type actually exists.
    - Reason: There is no method to get a product type by name, if a check is needed I would also change the product type api. 
- Surcharges can be save in mememory and will dissapear after app restart.
    - Reason: verified by interviewers.

Surcharches are saved in a dictionary in the SurchargeRepository that is injected as a singleton. This way the items in the dictionary are saves as long as application keeps running. A restart would mean a delete of the saved surcharges. If a product insurance is calculated the surcharge looked up in the list and send to the calculation. If the product type does not exists in the surcharge list the value defaults to 0.

## Testing
I kept the initial testing method intact and added more tests to it. Although I am not enterily sure it is the best way of integration testing it does the job. I added multiple tests before refactoring to ensure the workings of the busines logic.

I introduced unit tests to test the domain logic, but after refoctoring. I added those to test the calculations and only after that I refactored the calculation part that I moved to the constructor of the ProductInsurance class.

I added swagger for testing and documentation.

## Remarks
### Task 2
The BaseApi contains a method to get a product and a method to get a product type. This is not strictly following the seperation of concerns principle, but since the class is not too large and understandable I kept it this way. If another method would be introduced here it should be refactored using the Liskov principle, and probably using generics.
### Task 3
Hitting an external system in a loop is not a good idea, but the products api does not have an enpoint for posting a list of ids to get the info of multiple products or product types. Adding endpoints to the products api that provide this could give an increase in the performance of the insurance api.

### Task 5
Check if a produt exists could be done by getting all the products and see if it is in the list, but a better approach would be to add a new endpoint to the products api that returns a producttype by producttype name. Then if a product type doesn't exists a 404 code could be returned.
