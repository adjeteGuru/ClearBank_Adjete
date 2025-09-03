# repos# ClearBank Developer Test

## Overview
This project is a payment processing system designed to handle various payment schemes. It includes services for making payments, validating accounts, and processing transactions.

## Features
- Supports multiple payment schemes.
- Validates account details before processing payments.
- Processes transactions securely.

## What I did to meet the Acceptance Criteria
The code has been refactored and structured in various folders with the following principles in mind:
- **SOLID Principles**: The code adheres to the SOLID principles of object-oriented design, ensuring that it is modular and maintainable.
- **Testability**: The design allows for easy unit testing of components, promoting the use of mocks and dependency injection.
- **Readability**: The code is structured and commented for clarity, making it easier for developers to understand and work with.

- ## What I did not do and why
- **UpdateAccount**: I did not implement the `UpdateAccount` method in the `AccountDataStore` class because the initial implementation focused on the core functionality of processing payments and validating accounts. The `UpdateAccount` method, while essential for updating account details post-transaction, was deprioritized to maintain a concise codebase. Future iterations will address this functionality to enhance the system's capabilities.
- **Error Handling**: I did not implement extensive error handling to keep the code concise and improve the user experience. Although it is not mentioned in the Acceptance criteria, I would like to handle various exceptions and edge cases more robustly.
- **Logging**: I did not include logging mechanisms to track the flow of operations and errors. This could be beneficial for debugging and monitoring the system.

## Validating Payment Schemes
`paymentValidator` checks whether the payment can be processed based on the current state of the account and the specifics of the payment request.
 It is crucial for ensuring that only valid payments are processed. If the payment is not valid, the subsequent processing steps (like updating account balances or executing the transaction) will not occur.

## Payment Processor
`transactionProcessor` is responsible for executing the payment transaction. It handles the actual transfer of funds and updates the account balances accordingly. This component is essential for completing the payment process once the payment has been validated.

## Mechanism used to fulfill the requirements
The project employs a combination of design patterns and principles to meet the specified requirements:
- **Dependency Injection**: This pattern is used to inject dependencies into classes, making them more testable and flexible.
- **Strategy Pattern**: Different payment schemes are implemented as separate strategies, allowing for easy addition of new schemes without modifying existing code.
- **Single Responsibility Principle**: Each class has a single responsibility, making the code easier to maintain and understand.
- **Open/Closed Principle**: The system is open for extension but closed for modification, allowing new payment schemes to be added without altering existing code.
- **Liskov Substitution Principle**: Subtypes can be substituted for their base types without affecting the correctness of the program, ensuring that derived classes extend the base class behavior.
- **Interface Segregation Principle**: Clients are not forced to depend on interfaces they do not use, promoting a more modular design.
- **Dependency Inversion Principle**: High-level modules do not depend on low-level modules; both depend on abstractions, enhancing the flexibility and scalability of the system.

## Testing
- **Unit Testing**: The code is structured to facilitate unit testing, with clear separation of concerns and the use of mocks for dependencies.
The project includes a comprehensive suite of unit tests to ensure the correctness of each component. Tests cover various scenarios, including valid and invalid payment requests, different account states, and edge cases.
Mocking is used to isolate components during testing, allowing for focused and reliable tests.

## Contributor
Adjete Adjevi
