# AzureDemoApp

## Description

Demo project to save JSON files to Azure Blob Storage and use Event Grid to store data from JSON into Azure Cosmos DB.  
This project demonstrates how Azure Functions can work with Event Grid and HTTP triggers to create a basic scenario in an Azure environment.

---

## Features

- **HTTP-triggered Azure Function** to upload JSON files to Azure Blob Storage.
- **Event Grid-triggered Azure Function** to process blob creation events, download the uploaded JSON, and store its content and metadata in Azure Cosmos DB.
- **HTTP-triggered Azure Function** to retrieve all stored JSON documents from Cosmos DB.

---

## Azure Functions API Overview

### 1. SendJsonToBlobStorage

**Link** ([link](https://github.com/kmaraszkiewicz86/azuredemoapp/blob/main/src/Functions/AzureJsonDataFlowFunction/Functions/SendJsonDataToBlobStorage.cs))  
**Namespace:** `SendJsonToBlobStorageFunction`  
**Trigger:** HTTP POST  
**Description:**  
Receives a JSON payload via HTTP POST and saves it as a `.json` file in the `jsonfiles` blob container.

**Key Points:**
- Validates the request body.
- Deserializes the payload to a `DemoPayload` object.
- Uploads the JSON to Azure Blob Storage.
- Returns a confirmation message.

---

### 2. BlobEventGridToBlobAndCosmos

**Link** ([link](https://github.com/kmaraszkiewicz86/azuredemoapp/blob/main/src/Functions/AzureJsonDataFlowFunction/Functions/BlobEventGridToBlobAndCosmos.cs))  
**Namespace:** `BlobEventGridToBlobAndCosmosFunction`  
**Trigger:** Event Grid (BlobCreated event)  
**Description:**  
Triggered when a new blob is created in storage. Downloads the blob content and stores both the event metadata and the JSON content in the `JsonFiles` container of the `JsonDb` Cosmos DB database.

**Key Points:**
- Parses the Event Grid event to extract the blob URL.
- Downloads the blob content from Azure Blob Storage.
- Stores event details and blob content in Cosmos DB.

---

### 3. GetJsonFilesFromCosmos

**Link** ([link](https://github.com/kmaraszkiewicz86/azuredemoapp/blob/main/src/Functions/AzureJsonDataFlowFunction/Functions/GetJsonFilesFromCosmos.cs))  
**Namespace:** `GetJsonFilesFromCosmos`  
**Trigger:** HTTP GET  
**Description:**  
Provides an HTTP endpoint to retrieve all JSON documents stored in the `JsonFiles` Cosmos DB container.

**Key Points:**
- Queries all items from Cosmos DB.
- Returns the results as a JSON array.

---

## How It Works

1. **Upload JSON:**  
   Use the HTTP endpoint to upload a JSON file to Azure Blob Storage.

2. **Event Grid Notification:**  
   When a new blob is created, Event Grid triggers the processing function.

3. **Process and Store:**  
   The function downloads the blob, extracts the JSON, and saves it to Cosmos DB.

4. **Retrieve Data:**  
   Use the HTTP GET endpoint to fetch all stored JSON documents from Cosmos DB.

---

## Requirements

- .NET 8
- Azure Functions v4 (Isolated Worker)
- Azure Blob Storage
- Azure Cosmos DB
- Event Grid Subscription (for BlobCreated events)

---

## Project Structure

- `SendJsonToBlobStorage.cs` – HTTP-triggered function to upload JSON to Blob Storage.
- `SendJsonEventGridToBlobAndCosmos.cs` – Event Grid-triggered function to process new blobs and store data in Cosmos DB.
- `GetJsonFilesFromCosmos.cs` – HTTP-triggered function to retrieve JSON documents from Cosmos DB.
- `Program.cs` – Dependency injection and configuration.
- `DemoPayload.cs` – Model for the uploaded JSON payload.

---

## Usage

1. Deploy the Azure Functions to your Azure environment.
2. Configure Blob Storage, Cosmos DB, and Event Grid as required.
3. Use the HTTP POST endpoint to upload JSON files.
4. Use the HTTP GET endpoint to retrieve stored JSON data.

---

# Azure Functions Demo UI (Angular)

This project is an Angular-based user interface for interacting with Azure Functions. It demonstrates integration with a backend API, state management using NgRx, and modern Angular features such as standalone components and HTTP interceptors.

## Features

- Fetching and displaying JSON data from Azure Functions
- Sending data to the backend via HTTP POST requests
- State management with NgRx Store and Effects
- Automatic conversion of API response keys to camelCase using a global HTTP interceptor
- Modular and maintainable code structure
- Responsive UI with loading and error handling

## Technologies

- Angular
- NgRx (Store, Effects)
- RxJS
- Bootstrap (for UI styling)

## Getting Started

1. Install dependencies:
   ```
   npm install
   ```
2. Configure environment variables in [src/environments/environment.ts](https://github.com/kmaraszkiewicz86/azuredemoapp/blob/main/src/UI/azuredemoui/src/app/environments/environment.ts).
3. Run the development server:
   ```
   ng serve
   ```
4. Open [http://localhost:4200](http://localhost:4200) in your browser.

## Project Structure

- `src/app/features/send-json` ([link](https://github.com/kmaraszkiewicz86/azuredemoapp/tree/main/src/UI/azuredemoui/src/app/features/send-json)) – Components and services for sending and displaying JSON data
- `src/app/interceptors` ([link](https://github.com/kmaraszkiewicz86/azuredemoapp/tree/main/src/UI/azuredemoui/src/app/interceptors)) – HTTP interceptors (e.g., camelCase conversion)
- `src/app/store` ([link](https://github.com/kmaraszkiewicz86/azuredemoapp/tree/main/src/UI/azuredemoui/src/app/features/send-json/store)) – NgRx actions, reducers, and effects

## Notes

- The UI expects the backend to return data in PascalCase; all responses are automatically mapped to camelCase.
- Make sure to provide the correct Azure Function URL and token in the environment configuration.

---

---

## License

This project is for demonstration purposes.
