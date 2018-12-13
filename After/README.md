## Legacy Code -- Refactored

### High Level Design:

- Original ```PaymentService``` class primarily uses a "switch" statement to determine what logic to apply for each Payment type, for example for     FasterPayements Balance should be less then the amount requested while for Chaps account status should be live. 

    In order to refactor that firstly we can divide the responsibility of each payment type into it's own class which can encapsulate the logic for a particular payment type, that way we can get a S of SOLID into our design. Hence we have  ``` BacsPayment, ChapsPayment, FasterPayments ``` (all implementing ```IPaymentType``` interface) class each only doing the logic for that type of payment.

    However, we still have the switch statement in ```PaymentService``` and if we want to add say a payment type of Crypto we would need to create ```CryptoPayment``` class and still modify the ```PaymentService``` class to add additional switch. This totally goes against the "open/closed" principle. So to avoid the "modification" of ```PaymentService```, we will introduce Strategy Pattern which can determine at run-time what PaymentType to use, and to add a new payment type it's a matter of impmenting ```IPayementType``` interface and letting our DI Container know about avilaiity of a new PaymentType.

- DataStore classes can be easily refactored using same priciple as above and introducing interfaces for them and making them easily mockable
- Configuration was read directly via ```ConfigurationManager.AppSettings[...]``` which will not be very Unit Test Friendly!, but it can be     refactored by introducing a ```IConigProvider``` and mocking out ```ConfigProvider``` in the test

### Testing
- Typically you would unit test each individual classes and it's public interface, so for example we would have ```BacsPaymentTests``` ```ChapsPaymentTests``` etc, which is the approach taken in the refactor here, however we can also do an alternate approach of testing a higher level component like ```PaymentService```. See ```PaymentServiceTests_AltApproach``` in the Tests project.a