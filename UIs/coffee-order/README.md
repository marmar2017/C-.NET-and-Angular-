# CoffeeOrder

This project was generated using [Angular CLI](https://github.com/angular/angular-cli) version 19.2.10.

## Development server

To start a local development server, run:

```bash
ng serve
```

Once the server is running, open your browser and navigate to `http://localhost:4200/`. The application will automatically reload whenever you modify any of the source files.

## Code scaffolding

Angular CLI includes powerful code scaffolding tools. To generate a new component, run:

```bash
ng generate component component-name
```

For a complete list of available schematics (such as `components`, `directives`, or `pipes`), run:

```bash
ng generate --help
```

## Building

To build the project run:

```bash
ng build
```

This will compile your project and store the build artifacts in the `dist/` directory. By default, the production build optimizes your application for performance and speed.

## Running unit tests

To execute unit tests with the [Karma](https://karma-runner.github.io) test runner, use the following command:

```bash
ng test
```

## Running end-to-end tests

For end-to-end (e2e) testing, run:

```bash
ng e2e
```

Angular CLI does not come with an end-to-end testing framework by default. You can choose one that suits your needs.

## Additional Resources

For more information on using the Angular CLI, including detailed command references, visit the [Angular CLI Overview and Command Reference](https://angular.dev/tools/cli) page.


### How this app created 
**Coffee Order Processing System - Project Summary**

---

###  Overview



The system is designed to reflect best practices in asynchronous programming, background processing, API design, and frontend integration.

---

### Backend (.NET 8 API)

#### Features:

* `POST /orders`: Submits a coffee order with a JSON payload
* `GET /orders/{id}`: Fetches the status of an individual order
* `DELETE /orders`: (dev-only) Removes all orders for testing

#### Technologies Used:

* **Entity Framework Core** with **SQLite**
* Background worker using `IHostedService`
* `OrderStatus` enum for tracking state: `Pending`, `InProgress`, `Ready`, `Failed`
* Retry logic with max 3 attempts and error logging
* `Program.cs` configures DbContext, Swagger, and the hosted service
* API responses are documented using `[ProducesResponseType]` and optional XML comments for Swagger

#### Background Worker:

* Runs every 5 seconds to pick up `Pending` orders
* Simulates coffee preparation with `Task.Delay`
* Randomly fails orders to test retry and failure logic
* Updates status to `Ready` or `Failed` accordingly

---

### Frontend (Angular + TailwindCSS)

#### Features:

* Single `order-form` component using **standalone architecture**
* Users can:

  * Select coffee size, milk type, sugar level, extras
  * Submit the order
  * See real-time updates on order status via polling
* **Toasts** provide live feedback:

  * ☕ Order submitted
  * ⏳ Pending
  * ⚖️ In Progress
  * ✅ Ready
  * ❌ Failed
* On completion, the UI refreshes for the next order

#### Technologies Used:

* Angular 17+ with `provideHttpClient()` in `main.ts`
* TailwindCSS with DaisyUI for fast and clean form design
* Toast notifications using simple conditional blocks
* HTTP polling every 3 seconds to track backend order status

---

### 📝 Notes & Highlights

* The project demonstrates **asynchronous patterns**, **multithreading**, **logging**, and **database interaction** in .NET
* Frontend showcases **reactive feedback**, **form validation**, and **modern Angular setup**
* Backend is ready for extension with order history, filtering, or authentication
* The code adheres to the **SOLID principles**, and separation of concerns is maintained between layers

